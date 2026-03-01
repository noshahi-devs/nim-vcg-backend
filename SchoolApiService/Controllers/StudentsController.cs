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
    //[Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.dbsStudent
                .Include(s => s.Standard)
                .ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.dbsStudent
                .Include(s => s.Standard)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound("Sorry! No Student is found. Try next time. Good luck.");
            }

            return student;
        }


        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest("Invalid StudentId");
            }

            if (student.StandardId != null)
            {
                student.Standard = await _context.dbsStandard.FindAsync(student.StandardId);
                if (student.Standard == null)
                {
                    return BadRequest($"Invalid StandardId: {student.StandardId}");
                }
            }

            if (student.SectionId != null)
            {
                student.SectionObject = await _context.Sections.FindAsync(student.SectionId);
                if (student.SectionObject == null)
                {
                    return BadRequest($"Invalid SectionId: {student.SectionId}");
                }
                student.Section = student.SectionObject.SectionName; // keeping fallback copy
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Check if the StandardId is valid
            if (student.StandardId != null)
            {
                student.Standard = await _context.dbsStandard.FindAsync(student.StandardId);
                if (student.Standard == null)
                {
                    return BadRequest($"Invalid StandardId: {student.StandardId}");
                }
            }

            // Check if the SectionId is valid
            if (student.SectionId != null)
            {
                student.SectionObject = await _context.Sections.FindAsync(student.SectionId);
                if (student.SectionObject == null)
                {
                    return BadRequest($"Invalid SectionId: {student.SectionId}");
                }
                student.Section = student.SectionObject.SectionName; // keeping fallback copy
            }

            // Check if the ImageUpload is provided
            if (student.ImageUpload?.ImageData != null)
            {
                student.ImagePath = student.ImageUpload?.ImageData;
            }

            // Automatically generate UniqueStudentAttendanceNumber
            int nextAttendanceNumber = 1000;
            int nextAdmissionNo = 1000;
            int nextEnrollmentNo = 2000;

            if (await _context.dbsStudent.AnyAsync())
            {
                nextAttendanceNumber = await _context.dbsStudent.MaxAsync(s => s.UniqueStudentAttendanceNumber) + 1;
                
                var maxAdmission = await _context.dbsStudent.MaxAsync(s => s.AdmissionNo);
                nextAdmissionNo = (maxAdmission ?? 999) + 1;

                var maxEnrollment = await _context.dbsStudent.MaxAsync(s => s.EnrollmentNo);
                nextEnrollmentNo = (maxEnrollment ?? 1999) + 1;
            }
            
            student.UniqueStudentAttendanceNumber = nextAttendanceNumber;
            student.AdmissionNo = nextAdmissionNo;
            student.EnrollmentNo = nextEnrollmentNo;

            // Add the student to the context
            _context.dbsStudent.Add(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Unable to save changes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }


        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.dbsStudent.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.dbsStudent.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.dbsStudent.Any(e => e.StudentId == id);
        }
    }
}
