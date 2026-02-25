using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalStudents = await _context.dbsStudent.CountAsync();
            var totalTeachers = await _context.dbsStaff
                .Where(s => s.Department != null && s.Department.DepartmentName == "Teacher")
                .CountAsync();
            var totalClasses = await _context.dbsStandard.CountAsync();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

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

        [HttpGet("chart-data")]
        public async Task<IActionResult> GetChartData()
        {
            var last12Months = Enumerable.Range(0, 12)
                .Select(i => DateTime.Now.AddMonths(-i))
                .OrderBy(d => d)
                .ToList();

            var incomeData = new List<decimal>();
            var expenseData = new List<decimal>();
            var labels = new List<string>();

            foreach (var month in last12Months)
            {
                var income = await _context.GeneralIncomes
                    .Where(i => i.Date.Month == month.Month && i.Date.Year == month.Year)
                    .SumAsync(i => i.Amount);

                var expense = await _context.GeneralExpenses
                    .Where(e => e.Date.Month == month.Month && e.Date.Year == month.Year)
                    .SumAsync(e => e.Amount);

                incomeData.Add(income);
                expenseData.Add(expense);
                labels.Add(month.ToString("MMM"));
            }

            return Ok(new
            {
                Labels = labels,
                Income = incomeData,
                Expense = expenseData
            });
        }

        [HttpGet("student-distribution")]
        public async Task<IActionResult> GetStudentDistribution()
        {
            var distribution = await _context.dbsStudent
                .Include(s => s.Standard)
                .GroupBy(s => s.Standard.StandardName)
                .Select(g => new
                {
                    ClassName = g.Key ?? "Unassigned",
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(distribution);
        }
    }
}
