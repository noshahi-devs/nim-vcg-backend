using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OthersPaymentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public OthersPaymentsController(SchoolDbContext context)
        {
            _context = context;
        }


        public class OthersPaymentDto
        {
            public int OthersPaymentId { get; set; }
            public int? StudentId { get; set; }
            public StudentSummaryDto? Student { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? AmountPaid { get; set; }
            public decimal? AmountRemaining { get; set; }
            public decimal? Waver { get; set; }
            public DateTime PaymentDate { get; set; }
            public List<FeeDto>? Fees { get; set; }
            public List<MonthSummaryDto>? AcademicMonths { get; set; }
        }

        public class StudentSummaryDto
        {
            public string? StudentName { get; set; }
            public int? EnrollmentNo { get; set; }
            public int? StandardId { get; set; }
            public string? StandardName { get; set; }
        }

        public class FeeDto
        {
            public int FeeId { get; set; }
            public decimal Amount { get; set; }
            public FeeTypeSummaryDto? FeeType { get; set; }
        }

        public class FeeTypeSummaryDto
        {
            public string? TypeName { get; set; }
        }

        public class MonthSummaryDto
        {
            public int MonthId { get; set; }
            public string? MonthName { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OthersPaymentDto>>> GetothersPayments()
        {
            try
            {
                var othersPayments = await _context.othersPayments
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderByDescending(fp => fp.OthersPaymentId)
                    .Select(fp => new OthersPaymentDto
                    {
                        OthersPaymentId = fp.OthersPaymentId,
                        StudentId = fp.StudentId,
                        Student = fp.Student != null ? new StudentSummaryDto
                        {
                            StudentName = fp.Student.StudentName,
                            EnrollmentNo = fp.Student.EnrollmentNo,
                            StandardId = fp.Student.StandardId,
                            StandardName = fp.Student.Standard != null ? fp.Student.Standard.StandardName : null
                        } : null,
                        TotalAmount = fp.TotalAmount,
                        AmountPaid = fp.AmountPaid,
                        AmountRemaining = fp.AmountRemaining,
                        Waver = fp.Waver,
                        PaymentDate = fp.PaymentDate,
                        Fees = fp.fees != null ? fp.fees.Select(f => new FeeDto
                        {
                            FeeId = f.FeeId,
                            Amount = f.Amount,
                            FeeType = f.feeType != null ? new FeeTypeSummaryDto { TypeName = f.feeType.TypeName } : null
                        }).ToList() : null,
                        AcademicMonths = fp.academicMonths != null ? fp.academicMonths.Select(m => new MonthSummaryDto
                        {
                            MonthId = m.MonthId,
                            MonthName = m.MonthName
                        }).ToList() : null
                    })
                    .ToListAsync();

                return Ok(othersPayments);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOthersPaymentById(int id)
        {
            try
            {
                var monthlyPayment = await _context.othersPayments
                    .Include(fp => fp.fees).ThenInclude(f => f.feeType)
                    .Include(fp => fp.academicMonths)
                    .Include(fp => fp.otherPaymentDetails)
                     .Include(fp => fp.Student).ThenInclude(s => s.Standard)



                    .FirstOrDefaultAsync(fp => fp.OthersPaymentId == id);

                if (monthlyPayment == null)
                {
                    return NotFound($"monthlyPayment with ID {id} not found");
                }

                return Ok(monthlyPayment);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOthersPayment(int id, [FromBody] OthersPayment updatedPayment)
        {
            if (id != updatedPayment.OthersPaymentId)
            {
                return BadRequest("Invalid ID");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingPayment = await _context.othersPayments
                    .Include(p => p.fees)
                    .Include(p => p.academicMonths)
                    .Include(p => p.otherPaymentDetails)
                    .FirstOrDefaultAsync(p => p.OthersPaymentId == id);

                if (existingPayment == null)
                {
                    return NotFound($"Payment with ID {id} not found.");
                }

                // Update basic fields
                existingPayment.StudentId = updatedPayment.StudentId;
                existingPayment.PaymentDate = updatedPayment.PaymentDate;
                existingPayment.Waver = updatedPayment.Waver;
                existingPayment.AmountPaid = updatedPayment.AmountPaid;

                // Sync relationships
                await SyncAcademicMonthsAsync(existingPayment, updatedPayment.academicMonths);
                await SyncFeesAsync(existingPayment, updatedPayment.fees);

                // Recalculate fields
                await CalculatePaymentFieldsAsync(existingPayment);

                // Rebuild payment details (child records)
                existingPayment.otherPaymentDetails?.Clear();
                if (existingPayment.fees != null)
                {
                    foreach (var fee in existingPayment.fees)
                    {
                        var feeType = await _context.dbsFeeType.FindAsync(fee.FeeTypeId);
                        existingPayment.otherPaymentDetails ??= new List<OtherPaymentDetail>();
                        existingPayment.otherPaymentDetails.Add(new OtherPaymentDetail
                        {
                            FeeAmount = fee.Amount,
                            FeeName = feeType?.TypeName
                        });
                    }
                }

                // Update due balance
                UpdateDueBalance(existingPayment);

                await _context.SaveChangesAsync();

                return Ok(existingPayment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Error: {ex}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateOthersPayment([FromBody] OthersPayment othersPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Attach related entities
                await SyncAcademicMonthsAsync(othersPayment, othersPayment.academicMonths);
                await SyncFeesAsync(othersPayment, othersPayment.fees);

                // Calculate amounts
                await CalculatePaymentFieldsAsync(othersPayment);

                // Build child records
                othersPayment.otherPaymentDetails = new List<OtherPaymentDetail>();
                if (othersPayment.fees != null)
                {
                    foreach (var fee in othersPayment.fees)
                    {
                        var feeType = await _context.dbsFeeType.FindAsync(fee.FeeTypeId);
                        othersPayment.otherPaymentDetails.Add(new OtherPaymentDetail
                        {
                            FeeAmount = fee.Amount,
                            FeeName = feeType?.TypeName
                        });
                    }
                }

                // Update student balance
                UpdateDueBalance(othersPayment);

                _context.othersPayments.Add(othersPayment);
                await _context.SaveChangesAsync();

                return Ok(othersPayment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Error: {ex}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }




        private void UpdateDueBalance(OthersPayment othersPayment)
        {
            if (othersPayment.StudentId == null) return;

            var dueBalance = _context.dbsDueBalance
                .Where(db => db.StudentId == othersPayment.StudentId)
                .FirstOrDefault();

            if (dueBalance != null)
            {
                dueBalance.DueBalanceAmount = (dueBalance.DueBalanceAmount ?? 0) + (othersPayment.AmountRemaining ?? 0);
                dueBalance.LastUpdate = DateTime.Now;
            }
            else
            {
                _context.dbsDueBalance.Add(new DueBalance
                {
                    StudentId = othersPayment.StudentId,
                    DueBalanceAmount = othersPayment.AmountRemaining ?? 0,
                    LastUpdate = DateTime.Now
                });
            }
        }

        private async Task CalculatePaymentFieldsAsync(OthersPayment othersPayment)
        {
            if (othersPayment.StudentId == null) return;

            othersPayment.TotalAmount = othersPayment.fees?.Sum(fs => fs.Amount) ?? 0;

            decimal totalAmount = othersPayment.TotalAmount ?? 0;
            decimal waver = othersPayment.Waver ?? 0;
            decimal amountPaid = othersPayment.AmountPaid ?? 0;

            decimal totalAfterDiscount = totalAmount - (totalAmount * (waver / 100));

            othersPayment.AmountRemaining = totalAfterDiscount - amountPaid;
        }

        private async Task SyncFeesAsync(OthersPayment othersPayment, IEnumerable<Fee>? fees)
        {
            othersPayment.fees?.Clear();
            if (fees != null && fees.Any())
            {
                var feeIds = fees.Select(f => f.FeeId).ToList();
                othersPayment.fees = await _context.fees
                    .Where(f => feeIds.Contains(f.FeeId))
                    .ToListAsync();
            }
        }

        private async Task SyncAcademicMonthsAsync(OthersPayment othersPayment, IEnumerable<AcademicMonth>? months)
        {
            othersPayment.academicMonths?.Clear();
            if (months != null && months.Any())
            {
                var monthIds = months.Select(m => m.MonthId).ToList();
                othersPayment.academicMonths = await _context.dbsAcademicMonths
                    .Where(m => monthIds.Contains(m.MonthId))
                    .ToListAsync();
            }
        }





        // DELETE: api/OthersPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOthersPayment(int id)
        {
            var othersPayment = await _context.othersPayments
                .Include(fp => fp.fees)
                .FirstOrDefaultAsync(fp => fp.OthersPaymentId == id);

            if (othersPayment == null)
            {
                return NotFound();
            }

            _context.othersPayments.Remove(othersPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OthersPaymentExists(int id)
        {
            return _context.othersPayments.Any(e => e.OthersPaymentId == id);
        }
    }
}
