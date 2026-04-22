using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;


namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CommonController(SchoolDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Common/Frequency
        [HttpGet("Frequency")]
        public ActionResult<string[]> GetFrequency()
        {
            // Retrieve the enum values
            string[] frequencies = Enum.GetNames(typeof(Frequency));
            return Ok(frequencies);
        }

        [HttpGet("GetAllPaymentByStudentId/{studentId}")]
        public async Task<ActionResult<IEnumerable<MonthlyPayment>>> GetAllPaymentByStudentId(int studentId)
        {
            var payments = await _context.monthlyPayments
                .Include(p => p.PaymentDetails) // Include PaymentDetails
        .Include(p => p.paymentMonths) // Include paymentMonths
        .Where(p => p.StudentId == studentId)
                .ToListAsync();

            if (payments == null || payments.Count == 0)
            {
                return Ok(new List<MonthlyPayment>());
            }

            return Ok(payments);
        }

        [HttpGet("GetAllOtherPaymentByStudentId/{studentId}")]
        public async Task<ActionResult<IEnumerable<OthersPayment>>> GetAllOtherPaymentByStudentId(int studentId)
        {
            var otherPayments = await _context.othersPayments
                .Include(p => p.otherPaymentDetails)
                .Where(p => p.StudentId == studentId)
                .ToListAsync();

            if (otherPayments == null || otherPayments.Count == 0)
            {
                return Ok(new List<OthersPayment>());
            }

            return Ok(otherPayments);
        }




        // GET: api/Common/DueBalances
        [HttpGet("DueBalances")]
        public async Task<ActionResult<IEnumerable<DueBalance>>> GetDueBalances()
        {
            return await _context.dbsDueBalance.ToListAsync();
        }

        // GET: api/Common/DueBalances/5
        [HttpGet("DueBalances/{studentId}")]
        public async Task<ActionResult<DueBalance>> GetDueBalance(int studentId)
        {
            var dueBalance = await _context.dbsDueBalance.FirstOrDefaultAsync(b => b.StudentId == studentId);

            if (dueBalance == null)
            {
                return Ok(new DueBalance { StudentId = studentId, DueBalanceAmount = 0, LastUpdate = DateTime.Now });
            }

            return Ok(dueBalance);
        }


        //[HttpGet("GetPaymentDetailsByStudentId/{studentId}")]
        //public async Task<ActionResult<IEnumerable<object>>> GetPaymentDetailsByStudentId(int studentId)
        //{
        //    var result = await _context.PaymentDetails
        //        .Join(_context.monthlyPayments, pd => pd.MonthlyPaymentId, mp => mp.MonthlyPaymentId, (pd, mp) => new { pd, mp })
        //        .Join(_context.paymentMonths, pdmp => pdmp.mp.MonthlyPaymentId, pm => pm.MonthlyPaymentId, (pdmp, pm) => new { pdmp, pm })
        //        .Where(x => x.pdmp.mp.StudentId == studentId)
        //        .GroupBy(x => new { x.pdmp.pd.FeeName, x.pm.MonthName })
        //        .Select(group => new
        //        {
        //            FeeName = group.Key.FeeName,
        //            MonthName = group.Key.MonthName,
        //            NumberOfMonths = group.Count()
        //        })
        //        .OrderBy(entry => entry.FeeName)

        //        .ToListAsync();

        //    if (result == null || result.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}







        [HttpGet("GetPaymentDetailsByStudentId/{studentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPaymentDetailsByStudentId(int studentId)
        {
            var result = await _context.PaymentDetails
                .Join(_context.monthlyPayments, pd => pd.MonthlyPaymentId, mp => mp.MonthlyPaymentId, (pd, mp) => new { pd, mp })
                .Join(_context.paymentMonths, pdmp => pdmp.mp.MonthlyPaymentId, pm => pm.MonthlyPaymentId, (pdmp, pm) => new { pdmp, pm })
                .Where(x => x.pdmp.mp.StudentId == studentId)
                .GroupBy(x => new { x.pdmp.pd.FeeName })
                .Select(group => new
                {
                    FeeName = group.Key.FeeName,
                    Months = group.Select(g => g.pm.MonthName).Distinct().ToList(),
                    NumberOfMonths = group.Count()
                })
                .OrderBy(entry => entry.FeeName)
                .ToListAsync();

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }












        [HttpGet("debug-images")]
        public IActionResult DebugImages()
        {
            try
            {
                var contentRoot = _environment.ContentRootPath;
                var webRoot = _environment.WebRootPath;
                var imagesDir = Path.Combine(webRoot ?? Path.Combine(contentRoot, "wwwroot"), "images");

                if (!Directory.Exists(imagesDir))
                {
                    return Ok(new
                    {
                        message = "Images directory not found",
                        searchedPath = imagesDir,
                        contentRoot = contentRoot,
                        webRoot = webRoot
                    });
                }

                var files = Directory.GetFiles(imagesDir)
                    .Select(Path.GetFileName)
                    .ToList();

                return Ok(new
                {
                    count = files.Count,
                    path = imagesDir,
                    files = files,
                    contentRoot = contentRoot,
                    webRoot = webRoot
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
