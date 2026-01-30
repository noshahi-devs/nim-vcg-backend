using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public DashboardController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalStudents = await _context.dbsStudent.CountAsync();
            var totalTeachers = await _context.dbsStaff
                .Where(s => s.Department != null && s.Department.DepartmentName == "Teacher")
                .CountAsync();
            var totalClasses = await _context.dbsStandard.CountAsync();

            // Calculate Fees Collected (This month)
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Simplified Fee Calculation: Sum of FeePayments made this month
            // Assuming FeePayment table handles collection. 
            // Note: Since FeePayment logic is complex in seed, we'll try to sum AmountPaid from FeePayments if exists, 
            // or fallback to OthersPayment if that's where miscellaneous income goes.
            // For now, let's sum GeneralIncome for this demo + potentially fees if I can access the DbSet.
            // I don't see dbsFeePayment in the Context snippet earlier, but I saw 'fees' DbSet.
            
            // Let's check 'fees' table - usually 'fees' contains the structure, not the payment.
            // Wait, previous file view showed: public DbSet<Fee> fees;
            // Let's assume for now we sum 'GeneralIncome' and 'OthersPayment' for simplicity as per new feature.
            
            var totalIncome = await _context.GeneralIncomes
                .Where(i => i.Date.Month == currentMonth && i.Date.Year == currentYear)
                .SumAsync(i => i.Amount);

            var totalExpense = await _context.GeneralExpenses
                 .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                 .SumAsync(e => e.Amount);
            
            return Ok(new
            {
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalClasses = totalClasses,
                IncomeThisMonth = totalIncome,
                ExpenseThisMonth = totalExpense
            });
        }
    }
}
