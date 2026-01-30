using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralIncomesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public GeneralIncomesController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneralIncome>>> GetGeneralIncomes()
        {
            return await _context.GeneralIncomes.OrderByDescending(i => i.Date).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralIncome>> GetGeneralIncome(int id)
        {
            var income = await _context.GeneralIncomes.FindAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            return income;
        }

        [HttpPost]
        public async Task<ActionResult<GeneralIncome>> PostGeneralIncome(GeneralIncome income)
        {
            _context.GeneralIncomes.Add(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneralIncome", new { id = income.Id }, income);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneralIncome(int id, GeneralIncome income)
        {
            if (id != income.Id)
            {
                return BadRequest();
            }

            _context.Entry(income).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneralIncomeExists(id))
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
        public async Task<IActionResult> DeleteGeneralIncome(int id)
        {
            var income = await _context.GeneralIncomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            _context.GeneralIncomes.Remove(income);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneralIncomeExists(int id)
        {
            return _context.GeneralIncomes.Any(e => e.Id == id);
        }
    }
}
