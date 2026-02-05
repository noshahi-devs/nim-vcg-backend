using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public LeaveTypesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveTypeMaster>>> GetLeaveTypes()
        {
            return await _context.LeaveTypeMasters.ToListAsync();
        }

        // GET: api/LeaveTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeMaster>> GetLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypeMasters.FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return leaveType;
        }

        // PUT: api/LeaveTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(int id, LeaveTypeMaster leaveType)
        {
            if (id != leaveType.LeaveTypeMasterId)
            {
                return BadRequest();
            }

            _context.Entry(leaveType).State = EntityState.Modified;
            leaveType.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeExists(id))
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

        // POST: api/LeaveTypes
        [HttpPost]
        public async Task<ActionResult<LeaveTypeMaster>> PostLeaveType(LeaveTypeMaster leaveType)
        {
            _context.LeaveTypeMasters.Add(leaveType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeaveType", new { id = leaveType.LeaveTypeMasterId }, leaveType);
        }

        // DELETE: api/LeaveTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypeMasters.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            _context.LeaveTypeMasters.Remove(leaveType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypeMasters.Any(e => e.LeaveTypeMasterId == id);
        }
    }
}
