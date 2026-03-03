using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentDashboardController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StudentDashboardController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats/{studentId}")]
        public async Task<IActionResult> GetStudentStats(int studentId)
        {
            var student = await _context.dbsStudent
                .Include(s => s.Standard)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null) return NotFound("Student not found");

            // Attendance Stats
            var attendanceRecords = await _context.dbsAttendance
                .Where(a => a.Type == AttendanceType.Student && a.AttendanceIdentificationNumber == studentId)
                .ToListAsync();

            double attendancePercentage = 0;
            if (attendanceRecords.Any())
            {
                attendancePercentage = (double)attendanceRecords.Count(a => a.IsPresent) / attendanceRecords.Count * 100;
            }

            // Fee Stats
            var feePayments = await _context.monthlyPayments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();

            decimal totalPaid = feePayments.Sum(p => p.AmountPaid);
            decimal totalDue = feePayments.Sum(p => p.AmountRemaining);

            // Marks Stats
            var recentMarks = await _context.dbsMark
                .Where(m => m.StudentId == studentId)
                .OrderByDescending(m => m.MarkEntryDate)
                .Take(5)
                .Include(m => m.Subject)
                .ToListAsync();

            // Next Exam
            var nextExam = await _context.dbsExamSubject
                .Where(e => e.ExamDate >= DateTime.Now)
                .OrderBy(e => e.ExamDate)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                AttendancePercentage = Math.Round(attendancePercentage, 2),
                TotalPaid = totalPaid,
                TotalDue = totalDue,
                RecentMarks = recentMarks.Select(m => new {
                    Subject = m.Subject?.SubjectName,
                    Score = m.ObtainedScore,
                    Total = m.TotalMarks,
                    Grade = m.Grade.ToString(),
                    Status = m.PassStatus.ToString(),
                    Date = m.MarkEntryDate
                }),
                NextExam = nextExam != null ? new {
                    Subject = nextExam.Subject?.SubjectName,
                    Date = nextExam.ExamDate,
                    StartTime = nextExam.ExamStartTime,
                    EndTime = nextExam.ExamEndTime
                } : null,
                StudentInfo = new {
                    Name = student.StudentName,
                    ClassName = student.Standard?.StandardName,
                    AdmissionNo = student.AdmissionNo,
                    EnrollmentNo = student.EnrollmentNo
                }
            });
        }

        [HttpGet("attendance-history/{studentId}")]
        public async Task<IActionResult> GetAttendanceHistory(int studentId)
        {
            var history = await _context.dbsAttendance
                .Where(a => a.Type == AttendanceType.Student && a.AttendanceIdentificationNumber == studentId)
                .OrderByDescending(a => a.Date)
                .Take(30)
                .Select(a => new {
                    a.Date,
                    a.IsPresent,
                    a.Description
                })
                .ToListAsync();

            return Ok(history);
        }
    }
}
