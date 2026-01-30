using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralExpensesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public GeneralExpensesController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneralExpense>>> GetGeneralExpenses()
        {
            return await _context.GeneralExpenses.OrderByDescending(e => e.Date).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralExpense>> GetGeneralExpense(int id)
        {
            var expense = await _context.GeneralExpenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpPost]
        public async Task<ActionResult<GeneralExpense>> PostGeneralExpense(GeneralExpense expense)
        {
            _context.GeneralExpenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneralExpense", new { id = expense.Id }, expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneralExpense(int id, GeneralExpense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneralExpenseExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneralExpense(int id)
        {
            var expense = await _context.GeneralExpenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.GeneralExpenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneralExpenseExists(int id)
        {
            return _context.GeneralExpenses.Any(e => e.Id == id);
        }
    }
}
