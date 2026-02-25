using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        [HttpGet("profit-loss")]
        public async Task<IActionResult> GetProfitLoss([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string? campus = null)
        {
            var generalIncome = await _context.GeneralIncomes
                .Where(i => i.Date >= startDate && i.Date <= endDate && (string.IsNullOrEmpty(campus) || i.Campus == campus))
                .SumAsync(i => i.Amount);

            var feeIncome = await _context.monthlyPayments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .SumAsync(p => p.AmountPaid);

            var otherFeeIncome = await _context.othersPayments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .SumAsync(p => p.AmountPaid);

            var totalExpense = await _context.GeneralExpenses
                .Where(e => e.Date >= startDate && e.Date <= endDate && (string.IsNullOrEmpty(campus) || e.Campus == campus))
                .SumAsync(e => e.Amount);

            var totalIncome = generalIncome + feeIncome + otherFeeIncome;

            var incomeByCategory = new List<object>
            {
                new { Category = "General Income", Amount = generalIncome },
                new { Category = "Student Fees", Amount = feeIncome },
                new { Category = "Other Fees", Amount = otherFeeIncome }
            };

            var expenseByCategory = await _context.GeneralExpenses
                .Where(e => e.Date >= startDate && e.Date <= endDate && (string.IsNullOrEmpty(campus) || e.Campus == campus))
                .GroupBy(e => e.ExpenseType)
                .Select(g => new { Category = g.Key, Amount = g.Sum(e => e.Amount) })
                .ToListAsync();

            return Ok(new
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpense,
                NetProfit = totalIncome - totalExpense,
                IncomeByCategory = incomeByCategory,
                ExpenseByCategory = expenseByCategory
            });
        }

        [HttpGet("ledger")]
        public async Task<IActionResult> GetLedger([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var incomes = await _context.GeneralIncomes
                .Select(i => new { i.Date, Description = i.Description, Amount = i.Amount, Type = "Income", Category = "General" })
                .ToListAsync();

            var expenses = await _context.GeneralExpenses
                .Select(e => new { e.Date, Description = e.Description, Amount = e.Amount, Type = "Expense", Category = e.ExpenseType })
                .ToListAsync();

            var fees = await _context.monthlyPayments
                .Select(p => new { Date = p.PaymentDate, Description = "Student Fee Payment", Amount = p.AmountPaid, Type = "Income", Category = "Fee" })
                .ToListAsync();

            var all = incomes.Concat(expenses).Concat(fees)
                .OrderBy(x => x.Date)
                .ToList();

            decimal balance = 0;
            var ledger = all.Select((x, index) => {
                decimal debit = x.Type == "Expense" ? x.Amount : 0;
                decimal credit = x.Type == "Income" ? x.Amount : 0;
                balance += credit - debit;
                return new
                {
                    Id = index + 1,
                    Date = x.Date,
                    TransactionId = $"TXN-{x.Date.Year}-{index + 1:D4}",
                    Type = x.Type,
                    Category = x.Category,
                    Description = x.Description,
                    Debit = debit,
                    Credit = credit,
                    Balance = balance
                };
            }).OrderByDescending(x => x.Date).ToList();

            return Ok(ledger);
        }
        [HttpGet("dashboard-data")]
        public async Task<IActionResult> GetDashboardData()
        {
            var incomes = await _context.GeneralIncomes.ToListAsync();
            var expenses = await _context.GeneralExpenses.ToListAsync();
            var feePayments = await _context.monthlyPayments.ToListAsync();

            var monthMap = new Dictionary<string, (decimal Income, decimal Expenses)>();

            string GetMonthKey(DateTime date) => $"{date.Year}-{date.Month:D2}";

            void ProcessItem(DateTime date, decimal amount, bool isIncome)
            {
                var key = GetMonthKey(date);
                if (!monthMap.ContainsKey(key))
                {
                    monthMap[key] = (0, 0);
                }
                var current = monthMap[key];
                if (isIncome) current.Income += amount;
                else current.Expenses += amount;
                monthMap[key] = current;
            }

            foreach (var i in incomes) ProcessItem(i.Date, i.Amount, true);
            foreach (var f in feePayments) ProcessItem(f.PaymentDate, f.AmountPaid, true);
            foreach (var e in expenses) ProcessItem(e.Date, e.Amount, false);

            var sortedKeys = monthMap.Keys.OrderBy(k => k).ToList();
            var monthsLabels = sortedKeys.Select(k => {
                var parts = k.Split('-');
                var date = new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), 1);
                return date.ToString("MMM yyyy");
            }).ToList();

            var incomeSeries = sortedKeys.Select(k => monthMap[k].Income).ToList();
            var expenseSeries = sortedKeys.Select(k => monthMap[k].Expenses).ToList();

            var totalIncome = incomes.Sum(i => i.Amount) + feePayments.Sum(f => f.AmountPaid);
            var totalExpenses = expenses.Sum(e => e.Amount);

            var recentTransactions = incomes.Select(i => new { i.Date, Type = "Income", Category = i.Source, Description = i.Description, Amount = i.Amount })
                .Concat(expenses.Select(e => new { e.Date, Type = "Expense", Category = e.ExpenseType, Description = e.Description, Amount = e.Amount }))
                .OrderByDescending(t => t.Date)
                .Take(5)
                .Select((t, idx) => new
                {
                    Id = idx + 1,
                    t.Date,
                    TransactionId = $"TXN-{t.Date.Year}-{idx + 1:D4}",
                    t.Type,
                    t.Category,
                    t.Description,
                    Debit = t.Type == "Expense" ? t.Amount : 0,
                    Credit = t.Type == "Income" ? t.Amount : 0,
                    Balance = 0 // Balance simplified for dashboard
                }).ToList();

            return Ok(new
            {
                ChartData = new
                {
                    Months = monthsLabels,
                    Income = incomeSeries,
                    Expenses = expenseSeries
                },
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                ProfitLoss = totalIncome - totalExpenses,
                CashBankBalance = totalIncome - totalExpenses,
                RecentTransactions = recentTransactions
            });
        }
    }
}
