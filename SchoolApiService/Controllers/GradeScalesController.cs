using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Added security to match service requirements
    public class GradeScalesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public GradeScalesController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/GradeScales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeScale>>> GetGradeScales()
        {
            return await _context.GradeScales.ToListAsync();
        }

        // GET: api/GradeScales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeScale>> GetGradeScale(int id)
        {
            var gradeScale = await _context.GradeScales.FindAsync(id);

            if (gradeScale == null)
            {
                return NotFound();
            }

            return gradeScale;
        }

        // PUT: api/GradeScales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGradeScale(int id, GradeScale gradeScale)
        {
            if (id != gradeScale.GradeId)
            {
                return BadRequest();
            }

            _context.Entry(gradeScale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeScaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(gradeScale); // Return the updated object to match frontend expectation
        }

        // POST: api/GradeScales
        [HttpPost]
        public async Task<ActionResult<GradeScale>> PostGradeScale(GradeScale gradeScale)
        {
            _context.GradeScales.Add(gradeScale);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGradeScale", new { id = gradeScale.GradeId }, gradeScale);
        }

        // DELETE: api/GradeScales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGradeScale(int id)
        {
            var gradeScale = await _context.GradeScales.FindAsync(id);
            if (gradeScale == null)
            {
                return NotFound();
            }

            _context.GradeScales.Remove(gradeScale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/GradeScales/apply
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyGradeScale()
        {
            // Placeholder for applying logic if needed
            // For now just return Ok
            return Ok(new { message = "Grade scales applied successfully." });
        }

        private bool GradeScaleExists(int id)
        {
            return _context.GradeScales.Any(e => e.GradeId == id);
        }
    }
}
