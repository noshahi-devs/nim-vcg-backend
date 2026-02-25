using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ResultsController(SchoolDbContext context, Services.INotificationService notificationService) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;
        private readonly Services.INotificationService _notificationService = notificationService;

        [HttpPost("generate/{examId}")]
        public async Task<IActionResult> GenerateResults(int examId)
        {
            try
            {
                // 1. Fetch all Grade Scales
                var gradeScales = await _context.GradeScales.ToListAsync();
                if (!gradeScales.Any())
                {
                    // Auto-seed default Grade Scales if none exist
                    gradeScales = new List<GradeScale>
                    {
                        new GradeScale { Grade = "A+", MinPercentage = 95, MaxPercentage = 100, GradePoint = 4.0m, Remarks = "Outstanding" },
                        new GradeScale { Grade = "A", MinPercentage = 90, MaxPercentage = 94.99m, GradePoint = 3.7m, Remarks = "Excellent" },
                        new GradeScale { Grade = "B", MinPercentage = 80, MaxPercentage = 89.99m, GradePoint = 3.0m, Remarks = "Very Good" },
                        new GradeScale { Grade = "C", MinPercentage = 70, MaxPercentage = 79.99m, GradePoint = 2.0m, Remarks = "Good" },
                        new GradeScale { Grade = "D", MinPercentage = 60, MaxPercentage = 69.99m, GradePoint = 1.0m, Remarks = "Fair" },
                        new GradeScale { Grade = "F", MinPercentage = 0, MaxPercentage = 59.99m, GradePoint = 0.0m, Remarks = "Fail" }
                    };
                    _context.GradeScales.AddRange(gradeScales);
                    await _context.SaveChangesAsync();
                }

                // 2. Fetch all MarkEntries for this Exam
                var markEntries = await _context.dbsMarkEntry
                    .Where(me => me.ExamScheduleId == examId)
                    .Include(me => me.StudentMarksDetails)
                    .ToListAsync();

                if (!markEntries.Any())
                {
                    return NotFound($"No marks found for Exam ID {examId}. Please ensure marks are entered before calculating grades.");
                }

                int updatedCount = 0;

                // 3. Process each MarkEntry and its Student Details
                foreach (var entry in markEntries)
                {
                    if (entry.TotalMarks == null || entry.TotalMarks == 0) continue;

                    foreach (var studentDetail in entry.StudentMarksDetails)
                    {
                        if (studentDetail.ObtainedScore == null) continue;

                        decimal percentage = (decimal)studentDetail.ObtainedScore / (decimal)entry.TotalMarks * 100;

                        // Find matching Grade Scale
                        var matchedScale = gradeScales
                            .FirstOrDefault(s => percentage >= s.MinPercentage && percentage <= s.MaxPercentage);

                        if (matchedScale != null)
                        {
                            studentDetail.Grade = matchedScale.Grade;

                            // Update Pass/Fail status
                            // If Grade is 'F', it's a fail
                            if (matchedScale.Grade.Equals("F", StringComparison.OrdinalIgnoreCase))
                            {
                                studentDetail.PassStatus = PassFailStatus.Failed;
                            }
                            else
                            {
                                studentDetail.PassStatus = PassFailStatus.Passed;
                            }

                            updatedCount++;
                        }
                    }
                }

                if (updatedCount > 0)
                {
                    await _context.SaveChangesAsync();

                    // Log Notification Event
                    await _notificationService.LogEventAsync(
                        "System Admin",
                        "N/A",
                        "Result Generation",
                        "Completed",
                        $"Successfully calculated grades for {updatedCount} student records in Exam ID {examId}."
                    );

                    return Ok(new { message = $"Successfully calculated grades for {updatedCount} student records.", updatedCount });
                }

                return Ok(new { message = "No records were updated. No matching grade scales found or no scored records present.", updatedCount = 0 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during result generation: {ex.Message}");
            }
        }

        [HttpGet("exam/{examId}")]
        public async Task<IActionResult> GetResultsByExam(int examId)
        {
            var results = await _context.dbsStudentMarksDetails
                .Where(sd => sd.MarkEntry.ExamScheduleId == examId)
                .Include(sd => sd.Student)
                .Include(sd => sd.MarkEntry)
                .ThenInclude(me => me.Subject)
                .Select(sd => new
                {
                    sd.StudentId,
                    sd.StudentName,
                    Subject = sd.MarkEntry.Subject.SubjectName,
                    sd.ObtainedScore,
                    TotalMarks = sd.MarkEntry.TotalMarks,
                    Grade = sd.Grade.ToString(),
                    Status = sd.PassStatus.ToString()
                })
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("student/{studentId}/exam/{examId}")]
        public async Task<IActionResult> GetResultByStudent(int studentId, int examId)
        {
            var studentDetails = await _context.dbsStudentMarksDetails
                .Where(sd => sd.StudentId == studentId && sd.MarkEntry.ExamScheduleId == examId)
                .Include(sd => sd.Student)
                .Include(sd => sd.MarkEntry)
                .ThenInclude(me => me.Subject)
                .ToListAsync();

            if (!studentDetails.Any())
            {
                // Return Ok with empty subjects instead of NotFound to avoid console errors for valid queries
                return Ok(new { StudentId = studentId, ExamId = examId, Message = "No results found.", Subjects = new List<object>() });
            }

            var firstRecord = studentDetails.First();
            var student = firstRecord.Student;

            var result = new
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                RollNo = student.AdmissionNo?.ToString(),
                ExamId = examId,
                Subjects = studentDetails.Select(sd => new
                {
                    SubjectName = sd.MarkEntry?.Subject?.SubjectName ?? "N/A",
                    TotalMarks = sd.MarkEntry?.TotalMarks ?? 0,
                    ObtainedMarks = sd.ObtainedScore ?? 0,
                    Grade = sd.Grade.ToString(),
                    Status = sd.PassStatus.ToString()
                }).ToList()
            };

            return Ok(result);
        }
    }
}
