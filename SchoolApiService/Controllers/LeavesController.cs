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
    public class LeavesController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaves()
        {
            return await _context.Leaves
                .Include(l => l.Staff)
                .Include(l => l.ApprovedBy)
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            var leave = await _context.Leaves
                .Include(l => l.Staff)
                .Include(l => l.ApprovedBy)
                .FirstOrDefaultAsync(l => l.LeaveId == id);

            if (leave == null)
            {
                return NotFound();
            }

            return leave;
        }

        [HttpGet("staff/{staffId}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetStaffLeaves(int staffId)
        {
            return await _context.Leaves
                .Include(l => l.ApprovedBy)
                .Where(l => l.StaffId == staffId)
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Leave>> PostLeave(Leave leave)
        {
            leave.AppliedDate = DateTime.Now;
            leave.Status = LeaveStatus.Pending;
            
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeave", new { id = leave.LeaveId }, leave);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeave(int id, Leave leave)
        {
            if (id != leave.LeaveId)
            {
                return BadRequest();
            }

            _context.Entry(leave).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(id))
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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] LeaveStatusUpdateDto update)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }

            leave.Status = update.Status;
            leave.ApprovedByStaffId = update.AdminId;
            leave.AdminRemarks = update.Remarks;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.LeaveId == id);
        }
    }

    public class LeaveStatusUpdateDto
    {
        public LeaveStatus Status { get; set; }
        public int AdminId { get; set; }
        public string? Remarks { get; set; }
    }
}
