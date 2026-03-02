using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public CampusesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Campuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campus>>> GetCampuses()
        {
            return await _context.Campuses.ToListAsync();
        }

        // GET: api/Campuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Campus>> GetCampus(int id)
        {
            var campus = await _context.Campuses.FindAsync(id);

            if (campus == null)
            {
                return NotFound();
            }

            return campus;
        }

        // PUT: api/Campuses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampus(int id, Campus campus)
        {
            if (id != campus.CampusId)
            {
                return BadRequest();
            }

            _context.Entry(campus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampusExists(id))
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

        // POST: api/Campuses
        [HttpPost]
        public async Task<ActionResult<Campus>> PostCampus(Campus campus)
        {
            _context.Campuses.Add(campus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCampus", new { id = campus.CampusId }, campus);
        }

        // DELETE: api/Campuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampus(int id)
        {
            var campus = await _context.Campuses.FindAsync(id);
            if (campus == null)
            {
                return NotFound();
            }

            _context.Campuses.Remove(campus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampusExists(int id)
        {
            return _context.Campuses.Any(e => e.CampusId == id);
        }
    }
}
