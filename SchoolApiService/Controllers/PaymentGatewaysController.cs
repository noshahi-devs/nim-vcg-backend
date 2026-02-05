using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewaysController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public PaymentGatewaysController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentGateways
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentGatewaySetting>>> GetPaymentGateways()
        {
            return await _context.PaymentGatewaySettings.ToListAsync();
        }

        // GET: api/PaymentGateways/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentGatewaySetting>> GetPaymentGateway(int id)
        {
            var gateway = await _context.PaymentGatewaySettings.FindAsync(id);

            if (gateway == null)
            {
                return NotFound();
            }

            return gateway;
        }

        // PUT: api/PaymentGateways/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentGateway(int id, PaymentGatewaySetting gateway)
        {
            if (id != gateway.Id)
            {
                return BadRequest();
            }

            _context.Entry(gateway).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentGatewayExists(id))
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

        // POST: api/PaymentGateways
        [HttpPost]
        public async Task<ActionResult<PaymentGatewaySetting>> PostPaymentGateway(PaymentGatewaySetting gateway)
        {
            _context.PaymentGatewaySettings.Add(gateway);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentGateway", new { id = gateway.Id }, gateway);
        }

        // DELETE: api/PaymentGateways/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentGateway(int id)
        {
            var gateway = await _context.PaymentGatewaySettings.FindAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            _context.PaymentGatewaySettings.Remove(gateway);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentGatewayExists(int id)
        {
            return _context.PaymentGatewaySettings.Any(e => e.Id == id);
        }
    }
}
