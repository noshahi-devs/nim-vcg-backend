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
//using SchoolApp.Models.ViewModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamSchedulesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ExamSchedulesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/ExamSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamScheduleVM>>> GetdbsExamSchedule()
        {
            var data = await _context.dbsExamSchedule
                .AsNoTracking()
                .Include(it => it.AcademicYear)
                .Include(it => it.ExamScheduleStandards)
                    .ThenInclude(ess => ess.Standard)
                .Include(it => it.ExamScheduleStandards)
                    .ThenInclude(ess => ess.ExamSubjects)
                        .ThenInclude(es => es.ExamType)
                .Include(it => it.ExamScheduleStandards)
                    .ThenInclude(ess => ess.ExamSubjects)
                        .ThenInclude(es => es.Subject)
                .Select(it => new ExamScheduleVM
                {
                    ExamScheduleId = it.ExamScheduleId,
                    ExamScheduleName = it.ExamScheduleName,
                    StartDate = it.StartDate ?? (it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Any() ? it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Select(es => es.ExamDate).Min() : (DateTime?)null),
                    EndDate = it.EndDate ?? (it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Any() ? it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Select(es => es.ExamDate).Max() : (DateTime?)null),
                    ExamYear = it.ExamYear ?? (it.AcademicYear != null ? it.AcademicYear.Name : "2024"),
                    ExamScheduleStandards = it.ExamScheduleStandards.Select(ess => new ExamScheduleStandardForExamScheduleVM
                    {
                        StandardName = ess.Standard != null ? ess.Standard.StandardName : "Unknown",
                        ExamSubjects = ess.ExamSubjects.Select(es => new ExamSubjectVM
                        {
                            ExamStartTime = es.ExamStartTime,
                            ExamEndTime = es.ExamEndTime,
                            ExamDate = es.ExamDate,
                            ExamTypeName = es.ExamType != null ? es.ExamType.ExamTypeName : "General",
                            SubjectName = es.Subject != null ? es.Subject.SubjectName : "Unknown",
                            SubjectCode = es.Subject != null ? es.Subject.SubjectCode : null
                        })
                    })

                })
                .ToListAsync();

            // Debug fallback: If still null, set a test date to see if frontend renders it
            foreach (var item in data)
            {
                if (item.StartDate == null) item.StartDate = new DateTime(2025, 1, 1);
                if (item.EndDate == null) item.EndDate = new DateTime(2025, 12, 31);
                if (string.IsNullOrEmpty(item.ExamYear)) item.ExamYear = "2025";
            }

            return data;
        }

        public record GetExamScheduleOptionsResponse(int ExamScheduleId, string ExamScheduleName);

        [HttpGet("GetExamScheduleOptions")]
        public async Task<IEnumerable<GetExamScheduleOptionsResponse>> GetExamScheduleOptions()
        {
            return await _context.dbsExamSchedule
                .Select(it => new GetExamScheduleOptionsResponse(it.ExamScheduleId, it.ExamScheduleName))
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: api/ExamSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamScheduleVM>> GetExamSchedule(int id)
        {
            var item = await _context.dbsExamSchedule
                .AsNoTracking()
                .Where(it => it.ExamScheduleId == id)
                .Select(it => new ExamScheduleVM
                {
                    ExamScheduleId = it.ExamScheduleId,
                    ExamScheduleName = it.ExamScheduleName,
                    StartDate = it.StartDate ?? (it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Any() ? it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Select(es => es.ExamDate).Min() : (DateTime?)null),
                    EndDate = it.EndDate ?? (it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Any() ? it.ExamScheduleStandards.SelectMany(ess => ess.ExamSubjects).Select(es => es.ExamDate).Max() : (DateTime?)null),
                    ExamYear = it.ExamYear ?? (it.AcademicYear != null ? it.AcademicYear.Name : "2024"),
                    ExamScheduleStandards = it.ExamScheduleStandards.Select(ess => new ExamScheduleStandardForExamScheduleVM
                    {
                        StandardName = ess.Standard != null ? ess.Standard.StandardName : "Unknown",
                        ExamSubjects = ess.ExamSubjects.Select(es => new ExamSubjectVM
                        {
                            ExamStartTime = es.ExamStartTime,
                            ExamEndTime = es.ExamEndTime,
                            ExamDate = es.ExamDate,
                            ExamTypeName = es.ExamType != null ? es.ExamType.ExamTypeName : "General",
                            SubjectName = es.Subject != null ? es.Subject.SubjectName : "Unknown",
                            SubjectCode = es.Subject != null ? es.Subject.SubjectCode : null
                        })
                    })

                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            if (item.StartDate == null) item.StartDate = new DateTime(2025, 1, 1);
            if (item.EndDate == null) item.EndDate = new DateTime(2025, 12, 31);
            if (string.IsNullOrEmpty(item.ExamYear)) item.ExamYear = "2025";

            return item;
        }

        // PUT: api/ExamSchedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamSchedule(int id, ExamSchedule incoming)
        {
            if (id != incoming.ExamScheduleId) return BadRequest();

            var existing = await _context.dbsExamSchedule.FindAsync(id);
            if (existing == null) return NotFound();

            // Update only specific fields to avoid overwriting nav properties with null
            existing.ExamScheduleName = incoming.ExamScheduleName;
            existing.StartDate = incoming.StartDate;
            existing.EndDate = incoming.EndDate;
            existing.ExamYear = incoming.ExamYear;
            if (incoming.AcademicYearId != null) existing.AcademicYearId = incoming.AcademicYearId;
            if (incoming.CampusId != null) existing.CampusId = incoming.CampusId;

            _context.Entry(existing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamScheduleExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/ExamSchedules
        [HttpPost]
        public async Task<ActionResult<ExamSchedule>> PostExamSchedule(ExamSchedule examSchedule)
        {
            // Defaulting these to existing IDs to avoid FK conflicts
            if (examSchedule.CampusId == 0)
            {
                var firstCampus = await _context.Campuses.AsNoTracking().FirstOrDefaultAsync();
                if (firstCampus != null) examSchedule.CampusId = firstCampus.CampusId;
            }

            if (examSchedule.AcademicYearId == 0 || examSchedule.AcademicYearId == null)
            {
                var latestYear = await _context.dbsAcademicYears.AsNoTracking().OrderByDescending(y => y.AcademicYearId).FirstOrDefaultAsync();
                if (latestYear != null) examSchedule.AcademicYearId = latestYear.AcademicYearId;
            }

            _context.dbsExamSchedule.Add(examSchedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamSchedule", new { id = examSchedule.ExamScheduleId }, examSchedule);
        }


        // DELETE: api/ExamSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamSchedule(int id)
        {
            // Find the exam schedule to be deleted
            var examSchedule = await _context.dbsExamSchedule.FindAsync(id);
            if (examSchedule == null)
            {
                return NotFound();
            }

            // Remove references from the dbsExamScheduleStandard table
            var relatedExamScheduleStandards = await _context.dbsExamScheduleStandard
                .Where(es => es.ExamScheduleId == id)
                .ToListAsync();

            foreach (var examScheduleStandard in relatedExamScheduleStandards)
            {
                // Remove the reference
                examScheduleStandard.ExamScheduleId = null;
                _context.Entry(examScheduleStandard).State = EntityState.Modified;
            }

            // Save changes to apply the removal of references
            await _context.SaveChangesAsync();

            // Now, delete the exam schedule
            _context.dbsExamSchedule.Remove(examSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamScheduleExists(int id)
        {
            return _context.dbsExamSchedule.Any(e => e.ExamScheduleId == id);
        }
    }
}
