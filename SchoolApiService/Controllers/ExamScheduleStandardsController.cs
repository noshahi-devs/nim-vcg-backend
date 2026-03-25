using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApiService.ViewModels;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleStandardsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ExamScheduleStandardsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/ExamScheduleStandards
        [HttpGet]
        public async Task<IEnumerable<ExamScheduleStandardVM>> GetdbsExamScheduleStandard()
        {
            return await _context.dbsExamScheduleStandard
                .Select(it => new ExamScheduleStandardVM
                {
                    ExamScheduleName = it.ExamSchedule.ExamScheduleName,
                    ExamScheduleStandardId = it.ExamScheduleStandardId,
                    ExamScheduleId = it.ExamScheduleId,
                    StandardId = it.StandardId,
                    StandardName = it.Standard.StandardName,
                    ExamSubjects = it.ExamSubjects.Select(es => new ExamSubjectVM
                    {
                        ExamTypeId = es.ExamTypeId,
                        SubjectId = es.SubjectId,
                        ExamDate = es.ExamDate,
                        ExamEndTime = es.ExamEndTime,
                        ExamStartTime = es.ExamStartTime,
                        ExamTypeName = es.ExamType.ExamTypeName,
                        SubjectCode = es.Subject.SubjectCode,
                        SubjectName = es.Subject.SubjectName,
                    })
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: api/ExamScheduleStandards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamScheduleStandardVM>> GetExamScheduleStandard(int id)
        {
            var examScheduleStandard = await _context.dbsExamScheduleStandard
                .Select(it => new ExamScheduleStandardVM
                {
                    ExamScheduleName = it.ExamSchedule.ExamScheduleName,
                    ExamScheduleStandardId = it.ExamScheduleStandardId,
                    ExamScheduleId = it.ExamScheduleId,
                    StandardId = it.StandardId,
                    StandardName = it.Standard.StandardName,
                    ExamSubjects = it.ExamSubjects.Select(es => new ExamSubjectVM
                    {
                        ExamTypeId = es.ExamTypeId,
                        SubjectId = es.SubjectId,
                        ExamDate = es.ExamDate,
                        ExamEndTime = es.ExamEndTime,
                        ExamStartTime = es.ExamStartTime,
                        ExamTypeName = es.ExamType.ExamTypeName,
                        SubjectCode = es.Subject.SubjectCode,
                        SubjectName = es.Subject.SubjectName,
                    })
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(es => es.ExamScheduleStandardId == id);

            if (examScheduleStandard == null)
            {
                return NotFound();
            }

            return examScheduleStandard;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamScheduleStandard(int id, UpdateExamScheduleStandardVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingExamScheduleStandard = await _context.dbsExamScheduleStandard
                            .Include(es => es.ExamSubjects)
                            .FirstOrDefaultAsync(es => es.ExamScheduleStandardId == id);

                        if (existingExamScheduleStandard == null)
                        {
                            return NotFound("Exam schedule standard Id not found.") as IActionResult;
                        }

                        // Existence Checks for updated values
                        var scheduleExists = await _context.dbsExamSchedule.AnyAsync(s => s.ExamScheduleId == request.ExamScheduleId);
                        if (!scheduleExists) return BadRequest($"Exam Schedule with ID {request.ExamScheduleId} does not exist.") as IActionResult;

                        var standardExists = await _context.dbsStandard.AnyAsync(s => s.StandardId == request.StandardId);
                        if (!standardExists) return BadRequest($"Standard with ID {request.StandardId} does not exist.") as IActionResult;

                        // Update ExamScheduleId if needed
                        if (existingExamScheduleStandard.ExamScheduleId != request.ExamScheduleId)
                        {
                            existingExamScheduleStandard.ExamScheduleId = request.ExamScheduleId;
                        }

                        // Update StandardId if needed
                        if (existingExamScheduleStandard.StandardId != request.StandardId)
                        {
                            existingExamScheduleStandard.StandardId = request.StandardId;
                            _context.dbsExamSubject.RemoveRange(existingExamScheduleStandard.ExamSubjects);
                        }

                        // Update or Add ExamSubjects
                        if (request.ExamSubjects != null)
                        {
                            foreach (var examSubject in request.ExamSubjects)
                            {
                                if (existingExamScheduleStandard.StandardId == await _context.dbsSubject
                                    .Where(s => s.SubjectId == examSubject.SubjectId)
                                    .Select(s => s.StandardId)
                                    .SingleOrDefaultAsync())
                                {
                                    DateTime? startTime = null;
                                    if (!string.IsNullOrEmpty(examSubject.ExamStartTime) && DateTime.TryParse(examSubject.ExamStartTime, out DateTime st))
                                        startTime = st;

                                    DateTime? endTime = null;
                                    if (!string.IsNullOrEmpty(examSubject.ExamEndTime) && DateTime.TryParse(examSubject.ExamEndTime, out DateTime et))
                                        endTime = et;

                                    var existingExamSubject = existingExamScheduleStandard.ExamSubjects.FirstOrDefault(es => es.SubjectId == examSubject.SubjectId);

                                    if (existingExamSubject != null)
                                    {
                                        existingExamSubject.ExamDate = examSubject.ExamDate;
                                        existingExamSubject.ExamStartTime = startTime;
                                        existingExamSubject.ExamEndTime = endTime;
                                        existingExamSubject.ExamTypeId = examSubject.ExamTypeId;
                                    }
                                    else
                                    {
                                        existingExamScheduleStandard.ExamSubjects.Add(new ExamSubject
                                        {
                                            ExamDate = examSubject.ExamDate,
                                            ExamStartTime = startTime,
                                            ExamEndTime = endTime,
                                            SubjectId = examSubject.SubjectId,
                                            ExamScheduleStandardId = existingExamScheduleStandard.ExamScheduleStandardId,
                                            ExamTypeId = examSubject.ExamTypeId,
                                        });
                                    }
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return NoContent() as IActionResult;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        var errorMsg = ex.Message;
                        if (ex.InnerException != null) errorMsg += " | Inner: " + ex.InnerException.Message;
                        return StatusCode(500, "Error updating: " + errorMsg) as IActionResult;
                    }
                }
            });
        }


        [HttpPost]
        public async Task<IActionResult> PostExamScheduleStandard(CreateExamScheduleStandardVM request)
        {
            if (request.ExamScheduleId <= 0 || request.StandardId <= 0)
            {
                return BadRequest("Exam Schedule and Standard must be selected.");
            }

            if (request.ExamSubjects == null || !request.ExamSubjects.Any())
            {
                return BadRequest("At least one exam subject is required.");
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Existence Checks
                        var scheduleExists = await _context.dbsExamSchedule.AnyAsync(s => s.ExamScheduleId == request.ExamScheduleId);
                        if (!scheduleExists) return BadRequest($"Exam Schedule with ID {request.ExamScheduleId} does not exist.") as IActionResult;

                        var standardExists = await _context.dbsStandard.AnyAsync(s => s.StandardId == request.StandardId);
                        if (!standardExists) return BadRequest($"Standard with ID {request.StandardId} does not exist.") as IActionResult;

                        if (
                                await _context.dbsExamScheduleStandard.AnyAsync(it =>
                                it.ExamScheduleId == request.ExamScheduleId &&
                                it.StandardId == request.StandardId
                                )
                            )
                        {
                            return BadRequest("Exam schedule standard already exists for this selection.") as IActionResult;
                        }

                        var examScheduleStandard = new ExamScheduleStandard
                        {
                            ExamScheduleId = request.ExamScheduleId,
                            StandardId = request.StandardId
                        };

                        await _context.dbsExamScheduleStandard.AddAsync(examScheduleStandard);
                        await _context.SaveChangesAsync();

                        List<ExamSubject> examSubjects = new List<ExamSubject>();
                        foreach (var examSubject in request.ExamSubjects)
                        {
                            if (examSubject.SubjectId > 0 && examSubject.ExamTypeId > 0)
                            {
                                var subjectStandardId = await _context.dbsSubject.Where(it => it.SubjectId == examSubject.SubjectId).Select(it => it.StandardId).SingleOrDefaultAsync();

                                if (request.StandardId == subjectStandardId)
                                {
                                    DateTime? startTime = null;
                                    if (!string.IsNullOrEmpty(examSubject.ExamStartTime) && DateTime.TryParse(examSubject.ExamStartTime, out DateTime st))
                                        startTime = st;

                                    DateTime? endTime = null;
                                    if (!string.IsNullOrEmpty(examSubject.ExamEndTime) && DateTime.TryParse(examSubject.ExamEndTime, out DateTime et))
                                        endTime = et;

                                    examSubjects.Add(new ExamSubject
                                    {
                                        ExamDate = examSubject.ExamDate,
                                        ExamStartTime = startTime,
                                        ExamEndTime = endTime,
                                        SubjectId = examSubject.SubjectId,
                                        ExamScheduleStandardId = examScheduleStandard.ExamScheduleStandardId,
                                        ExamTypeId = examSubject.ExamTypeId,
                                    });
                                }
                            }
                        }

                        if (examSubjects.Count > 0)
                        {
                            await _context.dbsExamSubject.AddRangeAsync(examSubjects);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            return BadRequest("No valid subjects were provided for the selected standard.") as IActionResult;
                        }

                        await transaction.CommitAsync();
                        return Ok() as IActionResult;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        var errorMsg = ex.Message;
                        if (ex.InnerException != null) errorMsg += " | Inner: " + ex.InnerException.Message;
                        return StatusCode(500, "Error: " + errorMsg) as IActionResult;
                    }
                }
            });
        }

        // DELETE: api/ExamScheduleStandards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamScheduleStandard(int id)
        {
            var examScheduleStandard = await _context.dbsExamScheduleStandard.Include(e => e.ExamSubjects).FirstOrDefaultAsync(e => e.ExamScheduleStandardId == id);

            if (examScheduleStandard == null)
            {
                return NotFound();
            }

            _context.dbsExamScheduleStandard.Remove(examScheduleStandard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamScheduleStandardExists(int id)
        {
            return _context.dbsExamScheduleStandard.Any(e => e.ExamScheduleStandardId == id);
        }
    }
}
