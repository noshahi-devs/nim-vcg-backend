using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var monthlyInvoices = await _context.monthlyPayments
                .Include(m => m.Student)
                .Select(m => new InvoiceViewModel
                {
                    InvoiceId = m.MonthlyPaymentId,
                    InvoiceNumber = $"INV-M-{m.MonthlyPaymentId.ToString().PadLeft(6, '0')}",
                    StudentName = m.Student != null ? m.Student.StudentName : "N/A",
                    Date = DateTime.Now.ToString("dd MMM yyyy"), // Placeholder or use m.PaymentDate if exists
                    TotalAmount = (double)m.TotalAmount,
                    AmountPaid = (double)m.AmountPaid,
                    Status = m.AmountRemaining <= 0 ? "Paid" : "Pending",
                    Type = "Monthly"
                }).ToListAsync();

            var otherInvoices = await _context.othersPayments
                .Include(o => o.Student)
                .Select(o => new InvoiceViewModel
                {
                    InvoiceId = o.OthersPaymentId,
                    InvoiceNumber = $"INV-O-{o.OthersPaymentId.ToString().PadLeft(6, '0')}",
                    StudentName = o.Student != null ? o.Student.StudentName : "N/A",
                    Date = DateTime.Now.ToString("dd MMM yyyy"),
                    TotalAmount = (double)o.TotalAmount,
                    AmountPaid = (double)o.AmountPaid,
                    Status = o.AmountRemaining <= 0 ? "Paid" : "Pending",
                    Type = "Other"
                }).ToListAsync();

            var allInvoices = monthlyInvoices.Concat(otherInvoices).OrderByDescending(i => i.InvoiceId).ToList();
            return Ok(allInvoices);
        }

        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> GetInvoiceDetails(string type, int id)
        {
            if (type.ToLower() == "monthly")
            {
                var payment = await _context.monthlyPayments
                    .Include(m => m.Student)
                    .Include(m => m.PaymentDetails)
                    .FirstOrDefaultAsync(m => m.MonthlyPaymentId == id);
                return Ok(payment);
            }
            else
            {
                var payment = await _context.othersPayments
                    .Include(o => o.Student)
                    .Include(o => o.otherPaymentDetails)
                    .FirstOrDefaultAsync(o => o.OthersPaymentId == id);
                return Ok(payment);
            }
        }
    }

    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
        public double AmountPaid { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
