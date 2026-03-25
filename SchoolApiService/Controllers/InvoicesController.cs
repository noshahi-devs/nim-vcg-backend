using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        // GET: api/Invoices — lightweight list for the invoice list page
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var monthlyInvoices = await _context.monthlyPayments
                .AsNoTracking()
                .Select(m => new InvoiceViewModel
                {
                    InvoiceId = m.MonthlyPaymentId,
                    InvoiceNumber = $"INV-M-{m.MonthlyPaymentId.ToString().PadLeft(6, '0')}",
                    StudentName = m.Student != null ? m.Student.StudentName : "N/A",
                    ImagePath = m.Student != null ? m.Student.ImagePath : "",
                    Date = m.PaymentDate.ToString("dd MMM yyyy"),
                    TotalAmount = (double)(m.TotalAmount ?? 0),
                    AmountPaid = (double)(m.AmountPaid ?? 0),
                    Status = (m.AmountRemaining ?? 0) <= 0 ? "Paid" : "Pending",
                    Type = "Monthly"
                }).ToListAsync();

            var otherInvoices = await _context.othersPayments
                .AsNoTracking()
                .Select(o => new InvoiceViewModel
                {
                    InvoiceId = o.OthersPaymentId,
                    InvoiceNumber = $"INV-O-{o.OthersPaymentId.ToString().PadLeft(6, '0')}",
                    StudentName = o.Student != null ? o.Student.StudentName : "N/A",
                    ImagePath = o.Student != null ? o.Student.ImagePath : "",
                    Date = o.PaymentDate.ToString("dd MMM yyyy"),
                    TotalAmount = (double)(o.TotalAmount ?? 0),
                    AmountPaid = (double)(o.AmountPaid ?? 0),
                    Status = (o.AmountRemaining ?? 0) <= 0 ? "Paid" : "Pending",
                    Type = "Other"
                }).ToListAsync();

            var allInvoices = monthlyInvoices.Concat(otherInvoices)
                .OrderByDescending(i => i.InvoiceId)
                .ToList();

            return Ok(allInvoices);
        }

        // GET: api/Invoices/{type}/{id} — optimized detail projection (no circular refs)
        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> GetInvoiceDetails(string type, int id)
        {
            if (type.ToLower() == "monthly")
            {
                var payment = await _context.monthlyPayments
                    .AsNoTracking()
                    .Where(m => m.MonthlyPaymentId == id)
                    .Select(m => new
                    {
                        m.MonthlyPaymentId,
                        m.StudentId,
                        m.TotalFeeAmount,
                        m.Waver,
                        m.PreviousDue,
                        m.TotalAmount,
                        m.AmountPaid,
                        m.AmountRemaining,
                        m.PaymentDate,
                        student = m.Student != null ? new
                        {
                            m.Student.StudentName,
                            m.Student.EnrollmentNo,
                            m.Student.ImagePath,
                            standard = m.Student.Standard != null
                                ? new { m.Student.Standard.StandardName }
                                : null
                        } : null,
                        paymentDetails = m.PaymentDetails.Select(pd => new
                        {
                            pd.FeeName,
                            pd.FeeAmount
                        }),
                        academicMonths = m.academicMonths.Select(am => new
                        {
                            am.MonthId,
                            am.MonthName
                        })
                    })
                    .FirstOrDefaultAsync();

                if (payment == null) return NotFound();
                return Ok(payment);
            }
            else
            {
                var payment = await _context.othersPayments
                    .AsNoTracking()
                    .Where(o => o.OthersPaymentId == id)
                    .Select(o => new
                    {
                        o.OthersPaymentId,
                        o.StudentId,
                        o.TotalAmount,
                        o.AmountPaid,
                        o.AmountRemaining,
                        o.Waver,
                        o.PaymentDate,
                        student = o.Student != null ? new
                        {
                            o.Student.StudentName,
                            o.Student.EnrollmentNo,
                            o.Student.ImagePath,
                            standard = o.Student.Standard != null
                                ? new { o.Student.Standard.StandardName }
                                : null
                        } : null,
                        otherPaymentDetails = o.otherPaymentDetails.Select(pd => new
                        {
                            pd.FeeName,
                            pd.FeeAmount
                        }),
                        academicMonths = o.academicMonths.Select(am => new
                        {
                            am.MonthId,
                            am.MonthName
                        })
                    })
                    .FirstOrDefaultAsync();

                if (payment == null) return NotFound();
                return Ok(payment);
            }
        }
    }

    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public string Date { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
        public double AmountPaid { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
