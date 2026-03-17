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
    public class SubjectsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SubjectsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetdbsSubject()
        {
            return await _context.dbsSubject.Include(s => s.Standard).ToListAsync();
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            var subject = await _context.dbsSubject.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }

        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;
            
            // Check for duplicate SubjectCode if it's changing
            if (subject.SubjectCode.HasValue)
            {
                var existingSubjectWithCode = await _context.dbsSubject
                    .AnyAsync(s => s.SubjectCode == subject.SubjectCode && s.SubjectId != id);
                
                if (existingSubjectWithCode)
                {
                    return BadRequest(new { message = $"A subject with the code '{subject.SubjectCode}' already exists. Please use a unique code." });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("SubjectCode") == true || ex.InnerException?.Message.Contains("Unique") == true)
                {
                    return BadRequest(new { message = $"A subject with the code '{subject.SubjectCode}' already exists. Please use a unique code." });
                }
                return StatusCode(500, new { message = "An error occurred while updating the subject.", details = ex.Message });
            }

            return NoContent();
        }

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            // Check for duplicate SubjectCode
            if (subject.SubjectCode.HasValue)
            {
                var existingSubjectWithCode = await _context.dbsSubject.AnyAsync(s => s.SubjectCode == subject.SubjectCode);
                if (existingSubjectWithCode)
                {
                    return BadRequest(new { message = $"A subject with the code '{subject.SubjectCode}' already exists. Please use a unique code." });
                }
            }

            try
            {
                _context.dbsSubject.Add(subject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("SubjectCode") == true || ex.InnerException?.Message.Contains("Unique") == true)
                {
                    return BadRequest(new { message = $"A subject with the code '{subject.SubjectCode}' already exists. Please use a unique code." });
                }
                return StatusCode(500, new { message = "An error occurred while saving the subject.", details = ex.Message });
            }

            return CreatedAtAction("GetSubject", new { id = subject.SubjectId }, subject);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _context.dbsSubject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            // Check for related Marks
            var hasMarks = await _context.dbsMark.AnyAsync(m => m.SubjectId == id);
            if (hasMarks)
            {
                return BadRequest(new { message = "Cannot delete subject because it has associated student marks. Please remove or reassign the marks first." });
            }

            // Check for related SubjectAssignments
            var hasAssignments = await _context.SubjectAssignments.AnyAsync(sa => sa.SubjectId == id);
            if (hasAssignments)
            {
                return BadRequest(new { message = "Cannot delete subject because it is currently assigned to sections/teachers. Please remove the assignments first." });
            }

            try
            {
                _context.dbsSubject.Remove(subject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the subject. It may have dependent records.", details = ex.Message });
            }

            return NoContent();
        }

        private bool SubjectExists(int id)
        {
            return _context.dbsSubject.Any(e => e.SubjectId == id);
        }
    }
}
