using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        [HttpGet("exam/{examId}")]
        public async Task<IActionResult> GetExamAnalytics(int examId, [FromQuery] int? classId = null, [FromQuery] int? sectionId = null)
        {
            try
            {
                var query = _context.dbsStudentMarksDetails
                    .Where(sd => sd.MarkEntry.ExamScheduleId == examId);

                if (classId.HasValue && classId > 0)
                {
                    query = query.Where(sd => sd.Student.StandardId == classId.Value);
                }

                if (sectionId.HasValue && sectionId > 0)
                {
                    var section = await _context.Sections.FindAsync(sectionId.Value);
                    if (section != null)
                    {
                        query = query.Where(sd => sd.Student.Section == section.SectionName);
                    }
                }

                var marksDetails = await query
                    .Include(sd => sd.Student)
                    .Include(sd => sd.MarkEntry)
                    .ThenInclude(me => me.Subject)
                    .ToListAsync();

                if (!marksDetails.Any())
                {
                    return Ok(new { Message = "No marks data found for analytics calculation. Please ensure results are generated first.", Success = false });
                }

                // Overall Stats
                var studentGroups = marksDetails.GroupBy(sd => sd.StudentId);
                int totalStudents = studentGroups.Count();
                
                // Assuming "Passed" means the student passed ALL subjects they took in this exam
                var studentPassStatus = studentGroups.Select(g => new {
                    StudentId = g.Key,
                    IsPassed = g.All(sd => sd.PassStatus == PassFailStatus.Passed)
                }).ToList();

                int passedStudents = studentPassStatus.Count(s => s.IsPassed);
                int failedStudents = totalStudents - passedStudents;
                double passPercentage = totalStudents > 0 ? (double)passedStudents / totalStudents * 100 : 0;

                // Average Percentage
                // We calculate the average of all individual student percentages
                var individualPercentages = studentGroups.Select(g => {
                    var total = g.Sum(sd => sd.MarkEntry?.TotalMarks ?? 0);
                    var obtained = g.Sum(sd => sd.ObtainedScore ?? 0);
                    return total > 0 ? (double)obtained / total * 100 : 0;
                }).ToList();
                double averagePercentage = individualPercentages.Any() ? individualPercentages.Average() : 0;

                var exam = await _context.dbsExamSchedule.FindAsync(examId);
                string examName = exam?.ExamScheduleName ?? "N/A";

                // High/Low Marks
                var totalObtainedByStudent = studentGroups.Select(g => g.Sum(sd => sd.ObtainedScore ?? 0)).ToList();
                decimal highestMarks = totalObtainedByStudent.Any() ? totalObtainedByStudent.Max() : 0;
                decimal lowestMarks = totalObtainedByStudent.Any() ? totalObtainedByStudent.Min() : 0;

                // Top Performer
                var topGroup = studentGroups
                    .OrderByDescending(g => g.Sum(sd => sd.ObtainedScore ?? 0))
                    .FirstOrDefault();
                string topPerformer = topGroup?.FirstOrDefault()?.StudentName ?? "N/A";

                // Grade Distribution
                var grades = await _context.GradeScales.ToListAsync();
                var gradeDistribution = grades.Select(gs => {
                    int count = studentGroups.Count(g => {
                        var total = g.Sum(sd => sd.MarkEntry?.TotalMarks ?? 0);
                        var obtained = g.Sum(sd => sd.ObtainedScore ?? 0);
                        var percentage = total > 0 ? (double)obtained / total * 100 : 0;
                        return (decimal)percentage >= gs.MinPercentage && (decimal)percentage <= gs.MaxPercentage;
                    });
                    return new {
                        Grade = gs.Grade,
                        Count = count,
                        Percentage = totalStudents > 0 ? Math.Round((double)count / totalStudents * 100, 2) : 0
                    };
                }).ToList();

                // Subject-wise Analytics
                var subjectGroups = marksDetails.GroupBy(sd => sd.MarkEntry.Subject.SubjectName);
                var subjectWiseAnalytics = subjectGroups.Select(g => new
                {
                    SubjectName = g.Key,
                    AverageMarks = g.Any() ? Math.Round(g.Average(sd => (double)(sd.ObtainedScore ?? 0)), 2) : 0,
                    HighestMarks = g.Any() ? g.Max(sd => sd.ObtainedScore ?? 0) : 0,
                    LowestMarks = g.Any() ? g.Min(sd => sd.ObtainedScore ?? 0) : 0,
                    PassPercentage = g.Any() ? Math.Round((double)g.Count(sd => sd.PassStatus == PassFailStatus.Passed) / g.Count() * 100, 2) : 0
                }).ToList();

                var analytics = new
                {
                    ExamId = examId,
                    ExamName = examName,
                    TotalStudents = totalStudents,
                    PassedStudents = passedStudents,
                    FailedStudents = failedStudents,
                    PassPercentage = Math.Round(passPercentage, 2),
                    AveragePercentage = Math.Round(averagePercentage, 2),
                    HighestMarks = highestMarks,
                    LowestMarks = lowestMarks,
                    TopPerformer = topPerformer,
                    GradeDistribution = gradeDistribution,
                    SubjectWiseAnalytics = subjectWiseAnalytics
                };

                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during analytics calculation: {ex.Message}");
            }
        }
    }
}
