using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SectionsController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetSections()
        {
            var sections = await _context.Sections
                .Select(s => new
                {
                    s.SectionId,
                    s.SectionName,
                    s.ClassName,
                    s.SectionCode,
                    s.StaffId,
                    ClassTeacherName = s.ClassTeacher != null ? s.ClassTeacher.StaffName : "No Teacher Assigned",
                    s.RoomNo,
                    s.Capacity
                })
                .ToListAsync();

            return Ok(sections);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Section>> GetSection(int id)
        {
            var section = await _context.Sections.Include(s => s.ClassTeacher).FirstOrDefaultAsync(s => s.SectionId == id);

            if (section == null)
            {
                return NotFound();
            }

            return section;
        }

        [HttpPost]
        public async Task<ActionResult<Section>> PostSection(Section section)
        {
            // Prevent EF from trying to create/update the related Staff entity
            section.ClassTeacher = null;

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSection", new { id = section.SectionId }, section);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSection(int id, Section section)
        {
            if (id != section.SectionId)
            {
                return BadRequest();
            }

            _context.Entry(section).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            // Check for related Students (using the DbSet name found in SchoolDbContext)
            var hasStudents = await _context.dbsStudent.AnyAsync(s => s.SectionId == id);
            if (hasStudents)
            {
                return BadRequest(new { message = "Cannot delete section because it has students enrolled. Please reassign or remove the students first." });
            }

            // Check for related SubjectAssignments
            var hasAssignments = await _context.SubjectAssignments.AnyAsync(sa => sa.SectionId == id);
            if (hasAssignments)
            {
                return BadRequest(new { message = "Cannot delete section because it has subject assignments. Please remove the assignments first." });
            }

            try
            {
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the section. It may have dependent records.", details = ex.Message });
            }

            return NoContent();
        }

        private bool SectionExists(int id)
        {
            return _context.Sections.Any(e => e.SectionId == id);
        }
    }
}
