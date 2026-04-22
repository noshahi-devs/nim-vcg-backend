using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyPaymentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public MonthlyPaymentsController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlyPayments([FromQuery] int skip = 0, [FromQuery] int take = 2000)
        {
            try
            {
                var monthlyPayments = await _context.monthlyPayments
                    .AsNoTracking()
                    .Include(p => p.Student)
                    .Include(p => p.fees)
                    .Include(p => p.academicMonths)
                    .OrderByDescending(p => p.PaymentDate)
                    .Skip(skip)
                    .Take(take)
                    .Select(p => new
                    {
                        p.MonthlyPaymentId,
                        p.StudentId,
                        p.TotalFeeAmount,
                        p.Waver,
                        p.PreviousDue,
                        p.TotalAmount,
                        p.AmountPaid,
                        p.AmountRemaining,
                        p.PaymentDate,
                        student = p.Student != null ? new 
                        { 
                            p.Student.StudentId,
                            p.Student.StudentName,
                            p.Student.EnrollmentNo,
                            standard = p.Student.Standard != null ? new { p.Student.Standard.StandardName } : null,
                            standardId = p.Student.StandardId
                        } : null,
                        fees = p.fees.Select(f => new 
                        {
                            f.FeeId,
                            f.Amount,
                            feeType = f.feeType != null ? new { f.feeType.TypeName } : null
                        }),
                        academicMonths = p.academicMonths.Select(m => new 
                        {
                            m.MonthId,
                            m.MonthName
                        })
                    })
                    .ToListAsync();

                return Ok(monthlyPayments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] GetMonthlyPayments: {ex}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonthlyPaymentById(int id)
        {
            try
            {
                var monthlyPayment = await _context.monthlyPayments
                    .Include(fp => fp.PaymentDetails)
                    .Include(fp => fp.paymentMonths)
                    .Include(fp => fp.Student).ThenInclude(s => s.Standard)
                    .Include(fp => fp.dueBalances)
                    .Include(fp => fp.fees).ThenInclude(f => f.feeType)
                    .Include(fp => fp.academicMonths)
                    .FirstOrDefaultAsync(fp => fp.MonthlyPaymentId == id);

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

        // PUT: api/MonthlyPayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonthlyPayment(int id, [FromBody] MonthlyPayment updatedmonthlyPayment)
        {
            if (id != updatedmonthlyPayment.MonthlyPaymentId)
            {
                return BadRequest("ID Mismatch");
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"Update Model Error: {errors}");
                return BadRequest(ModelState);
            }

            try
            {
                var existingMonthlyPayment = await _context.monthlyPayments
                    .Include(p => p.fees)
                    .Include(p => p.paymentMonths)
                    .Include(p => p.PaymentDetails)
                    .FirstOrDefaultAsync(p => p.MonthlyPaymentId == id);

                if (existingMonthlyPayment == null)
                {
                    return NotFound($"Payment with ID {id} not found.");
                }

                // 1. Update basic properties
                existingMonthlyPayment.StudentId = updatedmonthlyPayment.StudentId;
                existingMonthlyPayment.TotalFeeAmount = updatedmonthlyPayment.TotalFeeAmount;
                existingMonthlyPayment.Waver = updatedmonthlyPayment.Waver;
                existingMonthlyPayment.PreviousDue = updatedmonthlyPayment.PreviousDue;
                existingMonthlyPayment.AmountPaid = updatedmonthlyPayment.AmountPaid;
                existingMonthlyPayment.PaymentDate = updatedmonthlyPayment.PaymentDate;
                existingMonthlyPayment.PaymentMethod = updatedmonthlyPayment.PaymentMethod;
                existingMonthlyPayment.TransactionId = updatedmonthlyPayment.TransactionId;

                // 2. Refresh related collections
                existingMonthlyPayment.paymentMonths ??= new List<PaymentMonth>();
                existingMonthlyPayment.paymentMonths.Clear();

                existingMonthlyPayment.PaymentDetails ??= new List<PaymentDetail>();
                existingMonthlyPayment.PaymentDetails.Clear();

                await AttachFeeAsync(existingMonthlyPayment, updatedmonthlyPayment);
                await AttachAcademicMonthAsync(existingMonthlyPayment, updatedmonthlyPayment);

                await CalculatePaymentFieldsAsync(existingMonthlyPayment);

                // 3. Re-populate details
                if (existingMonthlyPayment.fees != null)
                {
                    var studentOverrides = await _context.StudentFees
                        .Where(sf => sf.StudentId == existingMonthlyPayment.StudentId)
                        .ToListAsync();

                    foreach (var fee in existingMonthlyPayment.fees)
                    {
                        var feeType = await _context.dbsFeeType.FindAsync(fee.FeeTypeId);
                        var overrideFee = studentOverrides.FirstOrDefault(o => o.FeeId == fee.FeeId);
                        
                        existingMonthlyPayment.PaymentDetails.Add(new PaymentDetail
                        {
                            FeeId = fee.FeeId,
                            FeeAmount = overrideFee?.AssignedAmount ?? fee.Amount,
                            FeeName = (feeType?.TypeName ?? "Fee") + (overrideFee != null ? " (Custom)" : "")
                        });
                    }
                }

                if (existingMonthlyPayment.academicMonths != null)
                {
                    foreach (var m in existingMonthlyPayment.academicMonths)
                    {
                        existingMonthlyPayment.paymentMonths.Add(new PaymentMonth
                        {
                            MonthName = m.MonthName ?? "Month"
                        });
                    }
                }

                // 4. Update Due Balance
                await PrepareDueBalanceUpdateAsync(existingMonthlyPayment);

                // 5. Finalize
                await _context.SaveChangesAsync();
                return Ok(existingMonthlyPayment);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "No Inner Exception";
                Console.WriteLine($"Update Error: {ex.Message} {inner}");
                return StatusCode(500, $"Update Failed: {ex.Message}. {inner}");
            }
        }

        private async Task AttachAcademicMonthAsync(MonthlyPayment existingMonthlyPayment, MonthlyPayment updatedMonthlyPayment)
        {
            if (updatedMonthlyPayment.academicMonths != null && updatedMonthlyPayment.academicMonths.Any())
            {
                var monthIds = updatedMonthlyPayment.academicMonths.Select(m => m.MonthId).ToList();
                existingMonthlyPayment.academicMonths = await _context.dbsAcademicMonths
                    .Where(am => monthIds.Contains(am.MonthId))
                    .ToListAsync();
            }
        }

        private async Task AttachFeeAsync(MonthlyPayment existingMonthlyPayment, MonthlyPayment updatedMonthlyPayment)
        {
            if (updatedMonthlyPayment.fees != null && updatedMonthlyPayment.fees.Any())
            {
                var feeIds = updatedMonthlyPayment.fees.Select(m => m.FeeId).ToList();
                existingMonthlyPayment.fees = await _context.fees
                    .Where(am => feeIds.Contains(am.FeeId))
                    .ToListAsync();
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateMonthlyPayment([FromBody] MonthlyPayment monthlyPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 1. Attach related lookup data (populate fees/months based on IDs)
                await AttachFeeAsync(monthlyPayment);
                await AttachAcademicMonthAsync(monthlyPayment);
                
                // 2. Perform calculations
                await CalculatePaymentFieldsAsync(monthlyPayment);

                // 3. Prepare child records (PaymentDetails and MonthDetails) 
                monthlyPayment.PaymentDetails = new List<PaymentDetail>();
                if (monthlyPayment.fees != null)
                {
                    var studentOverrides = await _context.StudentFees
                        .Where(sf => sf.StudentId == monthlyPayment.StudentId)
                        .ToListAsync();

                    foreach (var fee in monthlyPayment.fees)
                    {
                        var feeType = await _context.dbsFeeType.FindAsync(fee.FeeTypeId);
                        var overrideFee = studentOverrides.FirstOrDefault(o => o.FeeId == fee.FeeId);

                        monthlyPayment.PaymentDetails.Add(new PaymentDetail
                        {
                            FeeId = fee.FeeId,
                            FeeAmount = overrideFee?.AssignedAmount ?? fee.Amount,
                            FeeName = (feeType?.TypeName ?? "Fee") + (overrideFee != null ? " (Custom)" : "")
                        });
                    }
                }

                monthlyPayment.paymentMonths = new List<PaymentMonth>();
                if (monthlyPayment.academicMonths != null)
                {
                    foreach (var m in monthlyPayment.academicMonths)
                    {
                        monthlyPayment.paymentMonths.Add(new PaymentMonth
                        {
                            MonthName = m.MonthName ?? "Month"
                        });
                    }
                }

                // 4. Update DueBalance summary
                await PrepareDueBalanceUpdateAsync(monthlyPayment);

                // 5. Finalize
                _context.monthlyPayments.Add(monthlyPayment);
                await _context.SaveChangesAsync();

                return Ok(monthlyPayment);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "No Inner Exception";
                Console.WriteLine($"Create Error: {ex.Message}. Inner: {inner}");
                return StatusCode(500, $"Save Failed: {ex.Message}. {inner}");
            }
        }

        private async Task CalculatePaymentFieldsAsync(MonthlyPayment monthlyPayment)
        {
            if (monthlyPayment.StudentId == 0) return;

            var academicMonthsCount = monthlyPayment.academicMonths?.Count ?? 0;
            
            // Fetch student overrides to calculate corect total
            var studentOverrides = await _context.StudentFees
                .Where(sf => sf.StudentId == monthlyPayment.StudentId)
                .ToListAsync();

            decimal sumFees = 0;
            if (monthlyPayment.fees != null)
            {
                foreach (var fee in monthlyPayment.fees)
                {
                    var overrideFee = studentOverrides.FirstOrDefault(o => o.FeeId == fee.FeeId);
                    sumFees += overrideFee?.AssignedAmount ?? fee.Amount;
                }
            }

            monthlyPayment.TotalFeeAmount = sumFees * (academicMonthsCount > 0 ? academicMonthsCount : 1);

            // Fetch current arrear if not provided or to ensure accuracy
            var dueBalance = await _context.dbsDueBalance
                .AsNoTracking()
                .Where(b => b.StudentId == monthlyPayment.StudentId)
                .FirstOrDefaultAsync();

            monthlyPayment.PreviousDue = dueBalance?.DueBalanceAmount ?? 0;
            
            decimal totalFee = monthlyPayment.TotalFeeAmount ?? 0;
            decimal waverPercent = monthlyPayment.Waver ?? 0;
            decimal prevDue = monthlyPayment.PreviousDue ?? 0;
            decimal paid = monthlyPayment.AmountPaid ?? 0;

            // Total Amount = (Gross - Discount) + Arrears
            monthlyPayment.TotalAmount = (totalFee - (totalFee * (waverPercent / 100))) + prevDue;
            
            // Remaining = Total Amount - Current Paid
            monthlyPayment.AmountRemaining = monthlyPayment.TotalAmount - paid;
        }

        private async Task PrepareDueBalanceUpdateAsync(MonthlyPayment monthlyPayment)
        {
            var dueBalance = await _context.dbsDueBalance
                .Where(db => db.StudentId == monthlyPayment.StudentId)
                .FirstOrDefaultAsync();

            if (dueBalance != null)
            {
                dueBalance.DueBalanceAmount = monthlyPayment.AmountRemaining;
                dueBalance.LastUpdate = DateTime.Now;
            }
            else
            {
                _context.dbsDueBalance.Add(new DueBalance
                {
                    StudentId = monthlyPayment.StudentId,
                    DueBalanceAmount = monthlyPayment.AmountRemaining,
                    LastUpdate = DateTime.Now
                });
            }
        }


        //private void SaveMonthDetails(MonthlyPayment monthlyPayment)
        //{
        //    if (monthlyPayment.academicMonths != null && monthlyPayment.academicMonths.Any())
        //    {
        //        foreach (var academicMonth in monthlyPayment.academicMonths)
        //        {
        //            var paymentMonth = new PaymentMonth
        //            {
        //                PaymentId = monthlyPayment.MonthlyPaymentId,
        //                MonthName = academicMonth.MonthName
        //            };

        //            _context.paymentMonths.Add(paymentMonth);
        //        }

        //        _context.SaveChanges();
        //    }
        //}

        //private void SavePaymentDetail(MonthlyPayment monthlyPayment)
        //{
        //    if (monthlyPayment.fees != null && monthlyPayment.fees.Any())
        //    {
        //        foreach (var fees in monthlyPayment.fees)
        //        {

        //            var feesType = _context.fees
        //                .Where(ft => ft.FeeId == fees.FeeId)
        //                .FirstOrDefault();

        //            var paymentDetails = new PaymentDetail
        //            {

        //                MonthlyPaymentId=monthlyPayment.MonthlyPaymentId,
        //                FeeAmount=feesType.Amount,
        //                FeeName= feesType?.FeeName

        //            };

        //            _context.PaymentDetails.Add(paymentDetails);
        //        }

        //        _context.SaveChanges();
        //    }
        //}


        private async Task AttachFeeAsync(MonthlyPayment monthlyPayment)
        {
            if (monthlyPayment.fees != null && monthlyPayment.fees.Any())
            {
                var feeIds = monthlyPayment.fees.Select(f => f.FeeId).ToList();
                monthlyPayment.fees = await _context.fees
                    .Where(fs => feeIds.Contains(fs.FeeId))
                    .ToListAsync();
            }
        }

        private async Task AttachAcademicMonthAsync(MonthlyPayment monthlyPayment)
        {
            if (monthlyPayment.academicMonths != null && monthlyPayment.academicMonths.Any())
            {
                var monthIds = monthlyPayment.academicMonths.Select(m => m.MonthId).ToList();
                monthlyPayment.academicMonths = await _context.dbsAcademicMonths
                    .Where(am => monthIds.Contains(am.MonthId))
                    .ToListAsync();
            }
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonthlyPayment(int id)
        {
            var monthlyPayment = await _context.monthlyPayments
                .Include(fp => fp.fees)
                .Include(fp => fp.academicMonths)
                .FirstOrDefaultAsync(fp => fp.MonthlyPaymentId == id);

            if (monthlyPayment == null)
            {
                return NotFound();
            }

            foreach (var academicMonth in monthlyPayment.academicMonths)
            {
                academicMonth.monthlyPayment = null;
            }

            _context.monthlyPayments.Remove(monthlyPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool MonthlyPaymentExists(int id)
        {
            return _context.monthlyPayments.Any(e => e.MonthlyPaymentId == id);
        }
    }
}
