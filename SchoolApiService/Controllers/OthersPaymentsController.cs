using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

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
            public decimal? TotalFeeAmount { get; set; }
            public decimal? PreviousDue { get; set; }
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

        public class FeeTypeSummaryDto { public string? TypeName { get; set; } }
        public class MonthSummaryDto { public int MonthId { get; set; } public string? MonthName { get; set; } }

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
                        TotalFeeAmount = fp.TotalFeeAmount,
                        PreviousDue = fp.PreviousDue,
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
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOthersPaymentById(int id)
        {
            try
            {
                var payment = await _context.othersPayments
                    .Include(fp => fp.fees).ThenInclude(f => f.feeType)
                    .Include(fp => fp.academicMonths)
                    .Include(fp => fp.otherPaymentDetails)
                    .Include(fp => fp.Student).ThenInclude(s => s.Standard)
                    .FirstOrDefaultAsync(fp => fp.OthersPaymentId == id);

                if (payment == null) return NotFound($"Payment ID {id} not found");
                return Ok(payment);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal Server Error: {ex.Message}"); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOthersPayment(int id, [FromBody] OthersPayment updatedPayment)
        {
            if (id != updatedPayment.OthersPaymentId) return BadRequest("Invalid ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var existingPayment = await _context.othersPayments
                    .Include(p => p.fees)
                    .Include(p => p.academicMonths)
                    .Include(p => p.otherPaymentDetails)
                    .FirstOrDefaultAsync(p => p.OthersPaymentId == id);

                if (existingPayment == null) return NotFound($"Payment ID {id} not found.");

                existingPayment.StudentId = updatedPayment.StudentId;
                existingPayment.PaymentDate = updatedPayment.PaymentDate;
                existingPayment.Waver = updatedPayment.Waver;
                existingPayment.AmountPaid = updatedPayment.AmountPaid;

                await SyncAcademicMonthsAsync(existingPayment, updatedPayment.academicMonths);
                await SyncFeesAsync(existingPayment, updatedPayment.fees);
                await CalculatePaymentFieldsAsync(existingPayment);

                // Build child records
                existingPayment.otherPaymentDetails?.Clear();
                if (existingPayment.fees != null)
                {
                    foreach (var fee in existingPayment.fees)
                    {
                        var feeType = await _context.dbsFeeType.FindAsync(fee.FeeTypeId);
                        existingPayment.otherPaymentDetails ??= new List<OtherPaymentDetail>();
                        existingPayment.otherPaymentDetails.Add(new OtherPaymentDetail { FeeAmount = fee.Amount, FeeName = feeType?.TypeName ?? "Fee" });
                    }
                }

                await PrepareDueBalanceUpdateAsync(existingPayment);
                await _context.SaveChangesAsync();
                return Ok(existingPayment);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal Server Error: {ex.Message}"); }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOthersPayment([FromBody] OthersPayment othersPayment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await SyncAcademicMonthsAsync(othersPayment, othersPayment.academicMonths);
                await SyncFeesAsync(othersPayment, othersPayment.fees);

                // Duplicate check
                if (othersPayment.academicMonths != null && othersPayment.academicMonths.Any())
                {
                    var incomingMonthIds = othersPayment.academicMonths.Select(m => m.MonthId).ToList();
                    var existing = await _context.othersPayments
                        .Include(p => p.academicMonths)
                        .Where(p => p.StudentId == othersPayment.StudentId)
                        .Where(p => p.academicMonths.Any(am => incomingMonthIds.Contains(am.MonthId)))
                        .ToListAsync();

                    if (existing.Any())
                    {
                        var months = string.Join(", ", existing.SelectMany(p => p.academicMonths).Where(m => incomingMonthIds.Contains(m.MonthId)).Select(m => m.MonthName).Distinct());
                        return BadRequest($"A payment for {months} has already been recorded for this student.");
                    }
                }

                await CalculatePaymentFieldsAsync(othersPayment);

                othersPayment.otherPaymentDetails = new List<OtherPaymentDetail>();
                if (othersPayment.fees != null)
                {
                    foreach (var fee in othersPayment.fees)
                    {
                        var feeType = await _context.dbsFeeType.FindAsync(fee.FeeTypeId);
                        othersPayment.otherPaymentDetails.Add(new OtherPaymentDetail { FeeAmount = fee.Amount, FeeName = feeType?.TypeName ?? "Fee" });
                    }
                }

                await PrepareDueBalanceUpdateAsync(othersPayment);
                _context.othersPayments.Add(othersPayment);
                await _context.SaveChangesAsync();
                return Ok(othersPayment);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal Server Error: {ex.Message}"); }
        }

        private async Task PrepareDueBalanceUpdateAsync(OthersPayment othersPayment)
        {
            if (othersPayment.StudentId == null) return;
            var dueBalance = await _context.dbsDueBalance.FirstOrDefaultAsync(db => db.StudentId == othersPayment.StudentId);
            if (dueBalance != null)
            {
                dueBalance.DueBalanceAmount = othersPayment.AmountRemaining;
                dueBalance.LastUpdate = DateTime.Now;
            }
            else
            {
                _context.dbsDueBalance.Add(new DueBalance { StudentId = othersPayment.StudentId.Value, DueBalanceAmount = othersPayment.AmountRemaining ?? 0, LastUpdate = DateTime.Now });
            }
        }

        private async Task CalculatePaymentFieldsAsync(OthersPayment othersPayment)
        {
            if (othersPayment.StudentId == null) return;

            var sumFees = othersPayment.fees?.Sum(fs => fs.Amount) ?? 0;
            var monthCount = othersPayment.academicMonths?.Count ?? 0;
            othersPayment.TotalFeeAmount = sumFees * (monthCount > 0 ? monthCount : 1);

            var dueBalance = await _context.dbsDueBalance.AsNoTracking().FirstOrDefaultAsync(db => db.StudentId == othersPayment.StudentId);
            othersPayment.PreviousDue = dueBalance?.DueBalanceAmount ?? 0;

            decimal gross = othersPayment.TotalFeeAmount ?? 0;
            decimal waver = othersPayment.Waver ?? 0;
            decimal prev = othersPayment.PreviousDue ?? 0;
            decimal paid = othersPayment.AmountPaid ?? 0;

            decimal netCurrent = gross - (gross * (waver / 100));
            othersPayment.TotalAmount = netCurrent + prev;
            othersPayment.AmountRemaining = othersPayment.TotalAmount - paid;
        }

        private async Task SyncFeesAsync(OthersPayment othersPayment, IEnumerable<Fee>? fees)
        {
            if (fees != null && fees.Any())
            {
                var feeIds = fees.Select(f => f.FeeId).ToList();
                othersPayment.fees = await _context.fees.Where(f => feeIds.Contains(f.FeeId)).ToListAsync();
            }
        }

        private async Task SyncAcademicMonthsAsync(OthersPayment othersPayment, IEnumerable<AcademicMonth>? months)
        {
            if (months != null && months.Any())
            {
                var monthIds = months.Select(m => m.MonthId).ToList();
                othersPayment.academicMonths = await _context.dbsAcademicMonths.Where(m => monthIds.Contains(m.MonthId)).ToListAsync();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOthersPayment(int id)
        {
            var payment = await _context.othersPayments.Include(p => p.academicMonths).FirstOrDefaultAsync(p => p.OthersPaymentId == id);
            if (payment == null) return NotFound();

            // Smart rollback logic
            var dueBalance = await _context.dbsDueBalance.FirstOrDefaultAsync(db => db.StudentId == payment.StudentId);
            if (dueBalance != null)
            {
                dueBalance.DueBalanceAmount = payment.PreviousDue;
                dueBalance.LastUpdate = DateTime.Now;
            }

            _context.othersPayments.Remove(payment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
