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

        public class PromotionRequest
        {
            public List<int> StudentIds { get; set; } = [];
            public int NextClassId { get; set; }
            public int NextSectionId { get; set; }
            public int NextAcademicYearId { get; set; }
        }

        public StudentsController(SchoolDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents([FromQuery] int? academicYearId = null)
        {
            var query = _context.dbsStudent.AsQueryable();

            if (academicYearId.HasValue)
            {
                query = query.Where(s => s.AcademicYearId == academicYearId.Value);
            }

            return await query
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

            // Check if the ImageUpload is provided
            if (student.ImageUpload?.ImageData != null)
            {
                student.ImagePath = student.ImageUpload?.ImageData;
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

            // Fallback for AcademicYearId
            if (student.AcademicYearId == null)
            {
                student.AcademicYearId = await GetActiveAcademicYearId();
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

            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
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
                await _context.SaveChangesAsync();

                // Create User Account if Email is provided
                if (!string.IsNullOrEmpty(student.StudentEmail))
                {
                    var studentUserResult = await CreateUserIfNotExist(student.StudentEmail, student.StudentName, "Student", student.StudentPassword);
                    if (!studentUserResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Failed to create student user account: {studentUserResult.ErrorMessage}");
                    }
                    student.UserId = studentUserResult.User?.Id;
                }
                // Create Parent record and User Account if ParentEmail is provided
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

                _context.Entry(student).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
                    ? (role == "Parent" ? $"Parent@{Guid.NewGuid().ToString().Substring(0, 6)}!" : $"Student@{Guid.NewGuid().ToString().Substring(0, 6)}!") 
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


        [HttpPost("bulk-promote")]
        public async Task<IActionResult> BulkPromote([FromBody] PromotionRequest request)
        {
            if (request == null || request.StudentIds.Count == 0)
            {
                return BadRequest("Invalid promotion request.");
            }

            var nextClass = await _context.dbsStandard.FindAsync(request.NextClassId);
            var nextSection = await _context.Sections.FindAsync(request.NextSectionId);
            var nextYear = await _context.dbsAcademicYears.FindAsync(request.NextAcademicYearId);

            if (nextClass == null || nextSection == null || nextYear == null)
            {
                return BadRequest("Invalid destination class, section, or academic year.");
            }

            var students = await _context.dbsStudent
                .Where(s => request.StudentIds.Contains(s.StudentId))
                .ToListAsync();

            foreach (var student in students)
            {
                student.StandardId = request.NextClassId;
                student.SectionId = request.NextSectionId;
                student.Section = nextSection.SectionName;
                student.AcademicYearId = request.NextAcademicYearId;
                _context.Entry(student).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = $"{students.Count} students promoted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.dbsStudent.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // 1. Check for dependencies to provide better error messages
            var dependencies = new List<string>();

            if (await _context.dbsMark.AnyAsync(m => m.StudentId == id) || 
                await _context.dbsStudentMarksDetails.AnyAsync(md => md.StudentId == id))
                dependencies.Add("Academic Marks/Details");

            if (await _context.dbsAttendance.AnyAsync(a => a.AttendanceIdentificationNumber == student.UniqueStudentAttendanceNumber))
                dependencies.Add("Attendance Records");

            if (await _context.monthlyPayments.AnyAsync(p => p.StudentId == id) ||
                await _context.othersPayments.AnyAsync(p => p.StudentId == id))
                dependencies.Add("Payment History");

            if (await _context.dbsDueBalance.AnyAsync(d => d.StudentId == id))
                dependencies.Add("Outstanding Dues");

            if (await _context.NotificationLogs.AnyAsync(n => n.StudentId == id))
                dependencies.Add("Communication Logs");

            if (dependencies.Any())
            {
                var depList = string.Join(", ", dependencies);
                return BadRequest($"Unable to delete student <strong>{student.StudentName}</strong> because active records exist for: <strong>{depList}</strong>. Please remove these records first to maintain data integrity.");
            }

            // 2. Wrap deletion in a try-catch to handle any slipped-through FK constraints
            try
            {
                _context.dbsStudent.Remove(student);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Unable to delete student <strong>{student.StudentName}</strong>. This record is linked to other system data that must be removed first.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred while deleting the student: {ex.Message}");
            }
        }

        private bool StudentExists(int id)
        {
            return _context.dbsStudent.Any(e => e.StudentId == id);
        }

        private async Task<int?> GetActiveAcademicYearId()
        {
            var activeYearName = await _context.SystemSettings
                .Where(s => s.SettingKey == "academicYear" && s.Category == "General")
                .Select(s => s.SettingValue)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(activeYearName)) return null;

            return await _context.dbsAcademicYears
                .Where(y => y.Name == activeYearName)
                .Select(y => y.AcademicYearId)
                .FirstOrDefaultAsync();
        }
    }
}
