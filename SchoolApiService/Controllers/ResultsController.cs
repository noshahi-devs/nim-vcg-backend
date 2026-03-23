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

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { status = "alive", time = DateTime.Now });
        }

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
        public async Task<IActionResult> GetResultsByExam(int examId, int? classId = null, int? sectionId = null)
        {
            try
            {
                var query = _context.dbsStudentMarksDetails
                    .Where(sd => sd.MarkEntry.ExamScheduleId == examId);

                if (classId.HasValue && classId > 0)
                    query = query.Where(sd => sd.Student.StandardId == classId);

                if (sectionId.HasValue && sectionId > 0)
                    query = query.Where(sd => sd.Student.SectionId == sectionId);

                var rawData = await query
                    .Include(sd => sd.Student)
                        .ThenInclude(s => s.Standard)
                    .Include(sd => sd.Student)
                        .ThenInclude(s => s.SectionObject)
                    .Include(sd => sd.MarkEntry)
                        .ThenInclude(me => me.Subject)
                    .AsNoTracking()
                    .ToListAsync();

                var aggregated = rawData
                    .GroupBy(sd => sd.StudentId)
                    .Select(g => {
                        var first = g.First();
                        var totalMarks = g.Sum(x => x.MarkEntry?.TotalMarks ?? 0);
                        var obtainedMarks = g.Sum(x => x.ObtainedScore ?? 0);
                        var percentage = totalMarks > 0 ? Math.Round((decimal)obtainedMarks / totalMarks * 100, 2) : 0m;

                        return new
                        {
                            studentId = g.Key,
                            studentName = first.Student?.StudentName ?? first.StudentName,
                            rollNo = first.Student?.AdmissionNo,
                            className = first.Student?.Standard?.StandardName ?? "N/A",
                            sectionName = first.Student?.SectionObject?.SectionName ?? first.Student?.Section ?? "N/A",
                            totalMarks = totalMarks,
                            obtainedMarks = obtainedMarks,
                            percentage = percentage,
                            grade = GetOverallGrade(percentage),
                            subjects = g.Select(s => new {
                                subjectName = s.MarkEntry?.Subject?.SubjectName ?? "N/A",
                                totalMarks = s.MarkEntry?.TotalMarks ?? 0,
                                obtainedMarks = s.ObtainedScore ?? 0,
                                grade = s.Grade ?? "—",
                                status = s.PassStatus?.ToString() ?? "Passed"
                            }).ToList()
                        };
                    })
                    .OrderBy(r => r.studentName)
                    .ToList();

                return Ok(aggregated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error fetching aggregated results", details = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        private string GetOverallGrade(decimal percentage)
        {
            if (percentage >= 90) return "A+";
            if (percentage >= 80) return "A";
            if (percentage >= 70) return "B";
            if (percentage >= 60) return "C";
            if (percentage >= 50) return "D";
            return "F";
        }

        [HttpGet("student/{studentId}/exam/{examId}")]
        public async Task<IActionResult> GetResultByStudent(int studentId, int examId)
        {
            try
            {
                // STEP 1: LOAD RAW DATA WITHOUT PROJECTIONS OR SERIALIZATION RISKS
                var allStudentMarks = await _context.dbsStudentMarksDetails
                    .Where(sd => sd.StudentId == studentId)
                    .Include(sd => sd.Student)
                    .Include(sd => sd.MarkEntry)
                        .ThenInclude(me => me.Subject)
                    .AsNoTracking()
                    .ToListAsync();

                // STEP 2: FILTER IN MEMORY
                var filteredMarks = allStudentMarks
                    .Where(sd => sd.MarkEntry != null && sd.MarkEntry.ExamScheduleId == examId)
                    .ToList();

                if (!filteredMarks.Any())
                {
                    return Ok(new { StudentId = studentId, ExamId = examId, Message = "No results found for this exam.", Subjects = new List<object>() });
                }

                // STEP 3: MAP TO SAFE TYPES MANUALLY
                var result = new
                {
                    studentId = studentId,
                    studentName = filteredMarks.FirstOrDefault()?.Student?.StudentName ?? filteredMarks.FirstOrDefault()?.StudentName ?? "N/A",
                    rollNo = filteredMarks.FirstOrDefault()?.Student?.AdmissionNo?.ToString(),
                    examId = examId,
                    subjects = filteredMarks.Select(sd => new
                    {
                        subjectName = sd.MarkEntry?.Subject?.SubjectName ?? "N/A",
                        totalMarks = sd.MarkEntry?.TotalMarks ?? 0,
                        obtainedMarks = sd.ObtainedScore ?? 0,
                        grade = sd.Grade ?? "—",
                        status = sd.PassStatus?.ToString() ?? "Passed"
                    }).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    error = "CRITICAL SERVER ERROR", 
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}
