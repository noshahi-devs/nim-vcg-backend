using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicYearsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public AcademicYearsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/AcademicYears
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcademicYear>>> GetAcademicYears()
        {
            return await _context.dbsAcademicYears.ToListAsync();
        }

        // GET: api/AcademicYears/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AcademicYear>> GetAcademicYear(int id)
        {
            var academicYear = await _context.dbsAcademicYears.FindAsync(id);

            if (academicYear == null)
            {
                return NotFound();
            }

            return academicYear;
        }

        // PUT: api/AcademicYears/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAcademicYear(int id, AcademicYear academicYear)
        {
            if (id != academicYear.AcademicYearId)
            {
                return BadRequest();
            }

            _context.Entry(academicYear).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcademicYearExists(id))
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

        // POST: api/AcademicYears
        [HttpPost]
        public async Task<ActionResult<AcademicYear>> PostAcademicYear(AcademicYear academicYear)
        {
            _context.dbsAcademicYears.Add(academicYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAcademicYear", new { id = academicYear.AcademicYearId }, academicYear);
        }

        // DELETE: api/AcademicYears/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcademicYear(int id)
        {
            var academicYear = await _context.dbsAcademicYears.FindAsync(id);
            if (academicYear == null)
            {
                return NotFound();
            }

            _context.dbsAcademicYears.Remove(academicYear);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AcademicYearExists(int id)
        {
            return _context.dbsAcademicYears.Any(e => e.AcademicYearId == id);
        }
    }
}
