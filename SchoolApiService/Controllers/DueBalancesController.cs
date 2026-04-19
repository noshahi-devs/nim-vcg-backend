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
    public class DueBalancesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public DueBalancesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/DueBalances?skip=0&take=2000
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetdbsDueBalance([FromQuery] int skip = 0, [FromQuery] int take = 2000)
        {
            var result = await _context.dbsDueBalance
                .AsNoTracking()
                .Where(d => d.DueBalanceAmount > 0)
                .OrderByDescending(d => d.LastUpdate)
                .Skip(skip)
                .Take(take)
                .Select(d => new
                {
                    dueBalanceId = d.DueBalanceId,
                    studentId = d.StudentId,
                    dueBalanceAmount = d.DueBalanceAmount,
                    lastUpdate = d.LastUpdate,
                    student = d.Student == null ? null : new
                    {
                        studentId = d.Student.StudentId,
                        studentName = d.Student.StudentName,
                        enrollmentNo = d.Student.EnrollmentNo,
                        standardId = d.Student.StandardId,
                        standard = d.Student.Standard == null ? null : new
                        {
                            standardId = d.Student.Standard.StandardId,
                            standardName = d.Student.Standard.StandardName
                        }
                    }
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/DueBalances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DueBalance>> GetDueBalance(int id)
        {
            var dueBalance = await _context.dbsDueBalance.FindAsync(id);

            if (dueBalance == null)
            {
                return NotFound();
            }

            return dueBalance;
        }

        // PUT: api/DueBalances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDueBalance(int id, DueBalance dueBalance)
        {
            if (id != dueBalance.DueBalanceId)
            {
                return BadRequest();
            }

            _context.Entry(dueBalance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DueBalanceExists(id))
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

        // POST: api/DueBalances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DueBalance>> PostDueBalance(DueBalance dueBalance)
        {
            _context.dbsDueBalance.Add(dueBalance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDueBalance", new { id = dueBalance.DueBalanceId }, dueBalance);
        }

        // DELETE: api/DueBalances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDueBalance(int id)
        {
            var dueBalance = await _context.dbsDueBalance.FindAsync(id);
            if (dueBalance == null)
            {
                return NotFound();
            }

            _context.dbsDueBalance.Remove(dueBalance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/DueBalances/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncBalances()
        {
            try
            {
                var students = await _context.dbsStudent
                    .Where(s => s.Status == null || s.Status.ToLower() == "active")
                    .ToListAsync();
                
                int updatedCount = 0;

                foreach (var student in students)
                {
                    // 1. Get latest MonthlyPayment for this student
                    var latestMonthly = await _context.monthlyPayments
                        .Where(p => p.StudentId == student.StudentId)
                        .OrderByDescending(p => p.PaymentDate)
                        .FirstOrDefaultAsync();

                    decimal currentDue = 0;

                    if (latestMonthly != null)
                    {
                        currentDue = latestMonthly.AmountRemaining ?? 0;
                    }
                    else
                    {
                        // 2. If no monthly payment record, check standard fees
                        if (student.StandardId.HasValue)
                        {
                            currentDue = await _context.fees
                                .Where(f => f.StandardId == student.StandardId.Value)
                                .SumAsync(f => f.Amount);
                        }
                    }

                    // 3. Update or Add to DueBalance table
                    if (currentDue > 0)
                    {
                        var dueBalance = await _context.dbsDueBalance
                            .FirstOrDefaultAsync(db => db.StudentId == student.StudentId);

                        if (dueBalance != null)
                        {
                            dueBalance.DueBalanceAmount = currentDue;
                            dueBalance.LastUpdate = DateTime.Now;
                        }
                        else
                        {
                            _context.dbsDueBalance.Add(new DueBalance
                            {
                                StudentId = student.StudentId,
                                DueBalanceAmount = currentDue,
                                LastUpdate = DateTime.Now
                            });
                        }
                        updatedCount++;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = $"Financial records synchronized successfully. {updatedCount} students with dues identified.", count = updatedCount });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sync Error: {ex.Message}");
                return StatusCode(500, $"Sync Failed: {ex.Message}");
            }
        }

        private bool DueBalanceExists(int id)
        {
            return _context.dbsDueBalance.Any(e => e.DueBalanceId == id);
        }

    }
}
