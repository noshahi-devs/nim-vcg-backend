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
                .Where(s => (s.Department != null && s.Department.DepartmentName == "Teacher") || s.Designation == SchoolApp.Models.DataModels.Designation.Teacher)
                .CountAsync();
            var totalClasses = await _context.dbsStandard.CountAsync();
            var totalStaff = await _context.dbsStaff.CountAsync();
            var totalSections = await _context.Sections.CountAsync();
            var totalSubjects = await _context.dbsSubject.CountAsync();

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
                TotalStaff = totalStaff,
                TotalSections = totalSections,
                TotalSubjects = totalSubjects,
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

        [HttpGet("weekly-admissions")]
        public async Task<IActionResult> GetWeeklyAdmissions()
        {
            var sevenDaysAgo = DateTime.Now.Date.AddDays(-6);
            var admissions = await _context.dbsStudent
                .Where(s => s.CreatedAt >= sevenDaysAgo)
                .GroupBy(s => s.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var labels = new List<string>();
            var data = new List<int>();

            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Now.Date.AddDays(-i);
                labels.Add(date.ToString("ddd"));
                data.Add(admissions.FirstOrDefault(a => a.Date == date)?.Count ?? 0);
            }

            return Ok(new { Labels = labels, Data = data });
        }
    }
}
