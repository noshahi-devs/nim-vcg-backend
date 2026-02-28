using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApp.DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectAssignmentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SubjectAssignmentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/SubjectAssignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectAssignment>>> GetSubjectAssignments()
        {
            return await _context.SubjectAssignments
                .Include(sa => sa.Staff)
                .Include(sa => sa.Subject)
                .Include(sa => sa.Section)
                .ToListAsync();
        }

        // GET: api/SubjectAssignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectAssignment>> GetSubjectAssignment(int id)
        {
            var subjectAssignment = await _context.SubjectAssignments
                .Include(sa => sa.Staff)
                .Include(sa => sa.Subject)
                .Include(sa => sa.Section)
                .FirstOrDefaultAsync(sa => sa.SubjectAssignmentId == id);

            if (subjectAssignment == null)
            {
                return NotFound();
            }

            return subjectAssignment;
        }

        // GET: api/SubjectAssignments/BySection/5
        [HttpGet("BySection/{sectionId}")]
        public async Task<ActionResult<IEnumerable<SubjectAssignment>>> GetAssignmentsBySection(int sectionId)
        {
            return await _context.SubjectAssignments
                .Include(sa => sa.Staff)
                .Include(sa => sa.Subject)
                .Where(sa => sa.SectionId == sectionId)
                .ToListAsync();
        }

        // GET: api/SubjectAssignments/ByTeacher/5
        [HttpGet("ByTeacher/{staffId}")]
        public async Task<ActionResult<IEnumerable<SubjectAssignment>>> GetAssignmentsByTeacher(int staffId)
        {
            return await _context.SubjectAssignments
                .Include(sa => sa.Subject)
                .Include(sa => sa.Section)
                .Where(sa => sa.StaffId == staffId)
                .ToListAsync();
        }


        // PUT: api/SubjectAssignments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubjectAssignment(int id, SubjectAssignment subjectAssignment)
        {
            if (id != subjectAssignment.SubjectAssignmentId)
            {
                return BadRequest();
            }

            _context.Entry(subjectAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectAssignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SubjectAssignments
        [HttpPost]
        public async Task<ActionResult<SubjectAssignment>> PostSubjectAssignment(SubjectAssignment subjectAssignment)
        {
            _context.SubjectAssignments.Add(subjectAssignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubjectAssignment", new { id = subjectAssignment.SubjectAssignmentId }, subjectAssignment);
        }

        // DELETE: api/SubjectAssignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectAssignment(int id)
        {
            var subjectAssignment = await _context.SubjectAssignments.FindAsync(id);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            _context.SubjectAssignments.Remove(subjectAssignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectAssignmentExists(int id)
        {
            return _context.SubjectAssignments.Any(e => e.SubjectAssignmentId == id);
        }
    }
}
