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
using SchoolApiService.Services;

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
        private readonly ImageUploadService _imageService;

        public class PromotionRequest
        {
            public List<int> StudentIds { get; set; } = [];
            public int NextClassId { get; set; }
            public int NextSectionId { get; set; }
            public int NextAcademicYearId { get; set; }
        }

        public class StudentDto
        {
            public int StudentId { get; set; }
            public int? AdmissionNo { get; set; }
            public int? EnrollmentNo { get; set; }
            public int UniqueStudentAttendanceNumber { get; set; }
            public string? StudentName { get; set; }
            public string? ImagePath { get; set; }
            public DateTime StudentDOB { get; set; }
            public string? StudentGender { get; set; }
            public string? StudentReligion { get; set; }
            public string? StudentBloodGroup { get; set; }
            public string? StudentNationality { get; set; }
            public string? StudentNIDNumber { get; set; }
            public string? StudentContactNumber1 { get; set; }
            public string? StudentContactNumber2 { get; set; }
            public string? StudentEmail { get; set; }
            public string? ParentEmail { get; set; }
            public string? PermanentAddress { get; set; }
            public string? TemporaryAddress { get; set; }
            public string? FatherName { get; set; }
            public string? FatherNID { get; set; }
            public string? FatherContactNumber { get; set; }
            public string? MotherName { get; set; }
            public string? MotherNID { get; set; }
            public string? MotherContactNumber { get; set; }
            public string? LocalGuardianName { get; set; }
            public string? LocalGuardianContactNumber { get; set; }
            public int? StandardId { get; set; }
            public object? Standard { get; set; }
            public string? Section { get; set; }
            public string? GuardianPhone { get; set; }
            public string? PreviousSchool { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? AdmissionDate { get; set; }
            public string? Status { get; set; }
            public int? SectionId { get; set; }
            public int? AcademicYearId { get; set; }
            public int? CampusId { get; set; }
            public int? ParentId { get; set; }
            public string? UserId { get; set; }
            public decimal? DefaultDiscount { get; set; }
            public List<StudentFeeDto>? StudentFees { get; set; }
        }

        public class StudentFeeDto
        {
            public int StudentFeeId { get; set; }
            public int StudentId { get; set; }
            public int FeeId { get; set; }
            public decimal AssignedAmount { get; set; }
            public string? FeeName { get; set; }
            public string? PaymentFrequency { get; set; }
        }

        public class StudentStatsDto
        {
            public int TotalStudents { get; set; }
            public int ActiveStudents { get; set; }
            public int InactiveStudents { get; set; }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<StudentStatsDto>> GetStudentStats([FromQuery] int? academicYearId = null)
        {
            try
            {
                var query = _context.dbsStudent.AsQueryable();
                if (academicYearId.HasValue)
                {
                    query = query.Where(s => s.AcademicYearId == academicYearId.Value);
                }

                var total = await query.CountAsync();
                var active = await query.CountAsync(s => s.Status != null && s.Status.ToLower() == "active");
                var inactive = await query.CountAsync(s => s.Status != null && s.Status.ToLower() == "inactive");

                return Ok(new StudentStatsDto
                {
                    TotalStudents = total,
                    ActiveStudents = active,
                    InactiveStudents = inactive
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public StudentsController(SchoolDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ImageUploadService imageService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _imageService = imageService;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromQuery] int? academicYearId = null, [FromQuery] int skip = 0, [FromQuery] int take = 200)
        {
            var query = _context.dbsStudent.AsQueryable();

            if (academicYearId.HasValue)
            {
                query = query.Where(s => s.AcademicYearId == academicYearId.Value);
            }

            return await query
                .AsNoTracking()
                .OrderByDescending(s => s.StudentId)
                .Skip(skip)
                .Take(take)
                .Select(s => new StudentDto
                {
                    StudentId = s.StudentId,
                    AdmissionNo = s.AdmissionNo,
                    EnrollmentNo = s.EnrollmentNo,
                    UniqueStudentAttendanceNumber = s.UniqueStudentAttendanceNumber,
                    StudentName = s.StudentName,
                    ImagePath = (s.ImagePath != null && s.ImagePath.Length < 2000) ? s.ImagePath : null, // Prevent hang from large legacy Base64 blobs
                    StudentDOB = s.StudentDOB,
                    StudentGender = s.StudentGender.HasValue ? s.StudentGender.Value.ToString() : null,
                    StudentReligion = s.StudentReligion,
                    StudentBloodGroup = s.StudentBloodGroup,
                    StudentNationality = s.StudentNationality,
                    StudentNIDNumber = s.StudentNIDNumber,
                    StudentContactNumber1 = s.StudentContactNumber1,
                    StudentContactNumber2 = s.StudentContactNumber2,
                    StudentEmail = s.StudentEmail,
                    ParentEmail = s.ParentEmail,
                    PermanentAddress = s.PermanentAddress,
                    TemporaryAddress = s.TemporaryAddress,
                    FatherName = s.FatherName,
                    FatherNID = s.FatherNID,
                    FatherContactNumber = s.FatherContactNumber,
                    MotherName = s.MotherName,
                    MotherNID = s.MotherNID,
                    MotherContactNumber = s.MotherContactNumber,
                    LocalGuardianName = s.LocalGuardianName,
                    LocalGuardianContactNumber = s.LocalGuardianContactNumber,
                    StandardId = s.StandardId,
                    Standard = s.Standard != null ? new { s.Standard.StandardName } : null,
                    Section = s.Section,
                    GuardianPhone = s.GuardianPhone,
                    PreviousSchool = s.PreviousSchool,
                    CreatedAt = s.CreatedAt,
                    AdmissionDate = s.AdmissionDate,
                    Status = s.Status,
                    SectionId = s.SectionId,
                    AcademicYearId = s.AcademicYearId,
                    CampusId = s.CampusId,
                    ParentId = s.ParentId,
                    UserId = s.UserId,
                    DefaultDiscount = s.DefaultDiscount,
                    StudentFees = s.StudentFees.Select(f => new StudentFeeDto
                    {
                        StudentFeeId = f.StudentFeeId,
                        StudentId = f.StudentId,
                        FeeId = f.FeeId,
                        AssignedAmount = f.AssignedAmount,
                        FeeName = (f.Fee != null && f.Fee.feeType != null) ? f.Fee.feeType.TypeName : (f.Fee != null ? "General Fee" : "Assigned Fee"),
                        PaymentFrequency = f.Fee != null ? f.Fee.PaymentFrequency.ToString() : "Monthly"
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.dbsStudent
                .Where(s => s.StudentId == id)
                .Select(s => new StudentDto
                {
                    StudentId = s.StudentId,
                    AdmissionNo = s.AdmissionNo,
                    EnrollmentNo = s.EnrollmentNo,
                    UniqueStudentAttendanceNumber = s.UniqueStudentAttendanceNumber,
                    StudentName = s.StudentName,
                    ImagePath = s.ImagePath,
                    StudentDOB = s.StudentDOB,
                    StudentGender = s.StudentGender.HasValue ? s.StudentGender.Value.ToString() : null,
                    StudentReligion = s.StudentReligion,
                    StudentBloodGroup = s.StudentBloodGroup,
                    StudentNationality = s.StudentNationality,
                    StudentNIDNumber = s.StudentNIDNumber,
                    StudentContactNumber1 = s.StudentContactNumber1,
                    StudentContactNumber2 = s.StudentContactNumber2,
                    StudentEmail = s.StudentEmail,
                    ParentEmail = s.ParentEmail,
                    PermanentAddress = s.PermanentAddress,
                    TemporaryAddress = s.TemporaryAddress,
                    FatherName = s.FatherName,
                    FatherNID = s.FatherNID,
                    FatherContactNumber = s.FatherContactNumber,
                    MotherName = s.MotherName,
                    MotherNID = s.MotherNID,
                    MotherContactNumber = s.MotherContactNumber,
                    LocalGuardianName = s.LocalGuardianName,
                    LocalGuardianContactNumber = s.LocalGuardianContactNumber,
                    StandardId = s.StandardId,
                    Standard = s.Standard != null ? new { s.Standard.StandardName } : null,
                    Section = s.Section,
                    GuardianPhone = s.GuardianPhone,
                    PreviousSchool = s.PreviousSchool,
                    CreatedAt = s.CreatedAt,
                    AdmissionDate = s.AdmissionDate,
                    Status = s.Status,
                    SectionId = s.SectionId,
                    AcademicYearId = s.AcademicYearId,
                    CampusId = s.CampusId,
                    ParentId = s.ParentId,
                    UserId = s.UserId,
                    DefaultDiscount = s.DefaultDiscount,
                    StudentFees = s.StudentFees.Select(f => new StudentFeeDto
                    {
                        StudentFeeId = f.StudentFeeId,
                        StudentId = f.StudentId,
                        FeeId = f.FeeId,
                        AssignedAmount = f.AssignedAmount,
                        FeeName = (f.Fee != null && f.Fee.feeType != null) ? f.Fee.feeType.TypeName : (f.Fee != null ? "General Fee" : "Assigned Fee"),
                        PaymentFrequency = f.Fee != null ? f.Fee.PaymentFrequency.ToString() : "Monthly"
                    }).ToList()
                })
                .FirstOrDefaultAsync();

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

            var existingStudent = await _context.dbsStudent
                .Include(s => s.Standard)
                .Include(s => s.SectionObject)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (existingStudent == null)
            {
                return NotFound("Student not found.");
            }

            // 1. Basic Fields
            existingStudent.AdmissionNo = student.AdmissionNo;
            existingStudent.EnrollmentNo = student.EnrollmentNo;
            existingStudent.UniqueStudentAttendanceNumber = student.UniqueStudentAttendanceNumber;
            existingStudent.StudentName = student.StudentName;
            existingStudent.StudentDOB = student.StudentDOB;
            existingStudent.StudentGender = student.StudentGender;
            existingStudent.StudentReligion = student.StudentReligion;
            existingStudent.StudentBloodGroup = student.StudentBloodGroup;
            existingStudent.StudentNationality = student.StudentNationality;
            existingStudent.StudentNIDNumber = student.StudentNIDNumber;
            existingStudent.StudentContactNumber1 = student.StudentContactNumber1;
            existingStudent.StudentContactNumber2 = student.StudentContactNumber2;
            existingStudent.StudentEmail = student.StudentEmail;
            existingStudent.ParentEmail = student.ParentEmail;
            existingStudent.PermanentAddress = student.PermanentAddress;
            existingStudent.TemporaryAddress = student.TemporaryAddress;
            existingStudent.FatherName = student.FatherName;
            existingStudent.FatherNID = student.FatherNID;
            existingStudent.FatherContactNumber = student.FatherContactNumber;
            existingStudent.MotherName = student.MotherName;
            existingStudent.MotherNID = student.MotherNID;
            existingStudent.MotherContactNumber = student.MotherContactNumber;
            existingStudent.LocalGuardianName = student.LocalGuardianName;
            existingStudent.LocalGuardianContactNumber = student.LocalGuardianContactNumber;
            existingStudent.GuardianPhone = student.GuardianPhone;
            existingStudent.PreviousSchool = student.PreviousSchool;
            existingStudent.DefaultDiscount = student.DefaultDiscount;
            existingStudent.AdmissionDate = student.AdmissionDate;
            existingStudent.Status = student.Status;
            existingStudent.AcademicYearId = student.AcademicYearId;
            existingStudent.CampusId = student.CampusId;

            // 2. Relational Fields (Class & Section)
            if (student.StandardId != existingStudent.StandardId)
            {
                existingStudent.StandardId = student.StandardId;
                if (student.StandardId != null)
                {
                    existingStudent.Standard = await _context.dbsStandard.FindAsync(student.StandardId);
                }
                else
                {
                    existingStudent.Standard = null;
                }
            }

            if (student.SectionId != existingStudent.SectionId)
            {
                existingStudent.SectionId = student.SectionId;
                if (student.SectionId != null)
                {
                    existingStudent.SectionObject = await _context.Sections.FindAsync(student.SectionId);
                    existingStudent.Section = existingStudent.SectionObject?.SectionName;
                }
                else
                {
                    existingStudent.SectionObject = null;
                    existingStudent.Section = student.Section; // fallback to string if provided
                }
            }
            else
            {
                // Ensure text representation is in sync
                existingStudent.Section = student.Section;
            }

            // 3. Image Upload
            if (student.ImageUpload?.ImageData != null)
            {
                // Delete old file if it exists
                if (!string.IsNullOrEmpty(existingStudent.ImagePath))
                {
                    _imageService.DeleteOldImage(existingStudent.ImagePath);
                }

                var path = await _imageService.Upload(student.ImageUpload);
                if (path != null)
                {
                    existingStudent.ImagePath = path;
                }
            }

            // 4. Sync Student Fees
            if (student.StudentFees != null)
            {
                // Remove fees that are no longer sent
                var incomingFeeIds = student.StudentFees.Select(f => f.FeeId).ToList();
                var feesToRemove = _context.StudentFees
                    .Where(sf => sf.StudentId == existingStudent.StudentId && !incomingFeeIds.Contains(sf.FeeId))
                    .ToList();
                
                if (feesToRemove.Any())
                {
                    _context.StudentFees.RemoveRange(feesToRemove);
                }

                // Add or update fees
                foreach (var incomingFee in student.StudentFees)
                {
                    var existingFee = _context.StudentFees
                        .FirstOrDefault(sf => sf.StudentId == existingStudent.StudentId && sf.FeeId == incomingFee.FeeId);
                    
                    if (existingFee != null)
                    {
                        existingFee.AssignedAmount = incomingFee.AssignedAmount;
                        _context.Entry(existingFee).State = EntityState.Modified;
                    }
                    else
                    {
                        incomingFee.StudentId = existingStudent.StudentId;
                        _context.StudentFees.Add(incomingFee);
                    }
                }
            }

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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.InnerException?.Message ?? ex.Message}");
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

            // Check if binary image data is provided for upload
            if (student.ImageUpload?.ImageData != null)
            {
                var path = await _imageService.Upload(student.ImageUpload);
                if (path != null)
                {
                    student.ImagePath = path;
                }
            }

            // Automatically generate UniqueStudentAttendanceNumber
            int nextAttendanceNumber = 1000;
            int nextAdmissionNo = 1000;
            int nextEnrollmentNo = 2000;

            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync<ActionResult<Student>>(async () =>
            {
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

                // Add the student to the context (temporarily clear StudentFees to avoid FK issue)
                var incomingStudentFees = student.StudentFees?.ToList() ?? new List<StudentFee>();
                student.StudentFees = null; // Detach to prevent EF from trying to insert with StudentId=0
                _context.dbsStudent.Add(student);
                await _context.SaveChangesAsync(); // Student now has a real StudentId

                // Now save the StudentFees with the correct StudentId
                if (incomingStudentFees.Any())
                {
                    foreach (var fee in incomingStudentFees)
                    {
                        fee.StudentId = student.StudentId;
                        fee.StudentFeeId = 0; // Ensure it's treated as a new insert
                        _context.StudentFees.Add(fee);
                    }
                    await _context.SaveChangesAsync();
                }

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
                return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Unable to save changes: {ex.InnerException?.Message ?? ex.Message}");
            }
            });
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
