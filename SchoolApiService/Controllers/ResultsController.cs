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
        public async Task<IActionResult> GetResultsByExam(int examId)
        {
            try
            {
                var results = await _context.dbsStudentMarksDetails
                    .Where(sd => sd.MarkEntry.ExamScheduleId == examId)
                    .Select(sd => new
                    {
                        sd.StudentId,
                        StudentName = sd.Student != null ? sd.Student.StudentName : sd.StudentName,
                        Subject = sd.MarkEntry != null && sd.MarkEntry.Subject != null ? sd.MarkEntry.Subject.SubjectName : "N/A",
                        ObtainedScore = sd.ObtainedScore ?? 0,
                        TotalMarks = sd.MarkEntry != null ? (sd.MarkEntry.TotalMarks ?? 0) : 0,
                        Grade = sd.Grade ?? "—",
                        Status = sd.PassStatus.HasValue ? sd.PassStatus.ToString() : "Passed"
                    })
                    .OrderBy(r => r.StudentName)
                    .ToListAsync();

                return Ok(results);
            }
            catch (Exception ex)
            {
                // Return detailed error in response for easier debugging
                return StatusCode(500, new { error = "Error fetching exam results", details = ex.Message, stack = ex.StackTrace });
            }
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
                var subjects = new List<object>();
                foreach (var sd in filteredMarks)
                {
                    subjects.Add(new
                    {
                        SubjectName = "Score Record", // Simple string for now
                        TotalMarks = 100,             // Simple int
                        ObtainedMarks = sd.ObtainedScore ?? 0,
                        Grade = "N/A",                // Simple string
                        Status = "Passed"              // Simple string
                    });
                }

                var result = new
                {
                    StudentId = studentId,
                    StudentName = filteredMarks.First()?.Student?.StudentName ?? "Student " + studentId,
                    ExamId = examId,
                    Subjects = subjects
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
