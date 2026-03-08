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
using Microsoft.AspNetCore.Identity;
using SchoolApp.Models.DataModels.SecurityModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StudentsController(SchoolDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

                // Create User Account if Email is provided
                if (!string.IsNullOrEmpty(student.StudentEmail))
                {
                    var studentUserResult = await CreateUserIfNotExist(student.StudentEmail, student.StudentName, "Student", student.StudentPassword);
                    if (!studentUserResult.Succeeded)
                    {
                        return BadRequest($"Failed to create student user account: {studentUserResult.ErrorMessage}");
                    }
                    student.UserId = studentUserResult.User?.Id;
                }

                // Create Parent record and User Account if ParentEmail is provided
                /*
                if (!string.IsNullOrEmpty(student.ParentEmail))
                {
                    var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Email == student.ParentEmail);
                    if (parent == null)
                    {
                        parent = new Parent
                        {
                            ParentName = student.FatherName ?? student.MotherName ?? "Parent of " + student.StudentName,
                            Email = student.ParentEmail,
                            Phone = student.FatherContactNumber ?? student.MotherContactNumber,
                            CampusId = student.CampusId
                        };
                        _context.Parents.Add(parent);
                        await _context.SaveChangesAsync();
                    }

                    student.ParentId = parent.ParentId;

                    // Create User Account for Parent
                    var parentUserResult = await CreateUserIfNotExist(student.ParentEmail, parent.ParentName, "Parent", student.ParentPassword);
                    if (parentUserResult.Succeeded && string.IsNullOrEmpty(parent.UserId))
                    {
                        parent.UserId = parentUserResult.User?.Id;
                        _context.Entry(parent).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else if (!parentUserResult.Succeeded)
                    {
                         // We might want to warn or fail here. For now, let's at least log it.
                         Console.WriteLine($"Warning: Failed to create parent user account: {parentUserResult.ErrorMessage}");
                    }
                }
                */

                _context.Entry(student).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Unable to save changes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        private async Task<(ApplicationUser? User, bool Succeeded, string? ErrorMessage)> CreateUserIfNotExist(string email, string name, string role, string? providedPassword = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email, Name = name };
                string password = string.IsNullOrWhiteSpace(providedPassword) 
                    ? (role == "Parent" ? "Parent@123" : "Student@123") 
                    : providedPassword;
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);
                    return (user, true, null);
                }
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                return (null, false, error);
            }
            return (user, true, null);
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
