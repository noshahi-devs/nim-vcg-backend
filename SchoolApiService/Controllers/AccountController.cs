using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApiService.Services;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;
using SchoolApp.Models.DataModels.SecurityModels;
using SchoolApp.Models.Email;
using System.ComponentModel.DataAnnotations;

namespace SchoolApiService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SchoolDbContext context,
            ITokenService tokenService,
            IEmailService emailService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SchoolDbContext _context = context;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IEmailService _emailService = emailService;


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            //student register
            //if (!request.Role.Any(r => r == "Student"))

            //	request.Role.Add("Student");


            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine($"[AccountController] Registration model invalid for {request.Email}: {errors}");
                return BadRequest(ModelState);
            }

            foreach (var role in request.Role)
            {

                if (await this._roleManager.RoleExistsAsync(role))
                {

                }
                else
                {
                    await this._roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            var user = new ApplicationUser { UserName = request.Username, Email = request.Email, Role = request.Role, PhoneNumber = request.PhoneNumber };

            var result = await _userManager.CreateAsync(
                 user,
                request.Password!
            );

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, request.Role);

                // Send Welcome Email
                try
                {
                    var emailData = new Dictionary<string, string>
                    {
                        { "UserName", user.UserName ?? "User" },
                        { "Email", user.Email ?? "" },
                        { "Password", request.Password },
                        { "Role", string.Join(", ", request.Role) },
                        { "CreatedAt", DateTime.Now.ToString("f") }
                    };

                    await _emailService.SendNotificationEmailAsync(
                        NotificationEvent.NewAccountCreation,
                        user.Email!,
                        user.UserName ?? "User",
                        emailData
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send welcome email: {ex.Message}");
                }

                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = request.Role }, request);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            // Log errors for debugging
            var errorMessages = string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            Console.WriteLine($"Registration failed for {request.Email}: {errorMessages}");

            return BadRequest(ModelState);
        }






        [HttpPost()]//https://domain.com/api/users/login
        [Route("login")]//https://domain.com/login
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailInput = request.Email?.Trim();
            var managedUser = await _userManager.FindByEmailAsync(emailInput!);

            if (managedUser == null)
            {
                // Fallback to checking by username in case the user typed their username instead of email
                managedUser = await _userManager.FindByNameAsync(emailInput!);
            }

            if (managedUser == null)
            {
                return BadRequest(new { message = "User account not found. Please ensure you have registered correctly." });
            }

            if (await _userManager.IsLockedOutAsync(managedUser) || managedUser.Status == "Inactive")
            {
                return BadRequest(new { message = "Your account has been deactivated. Please contact the administrator." });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

            if (!isPasswordValid)
            {
                return BadRequest(new { message = "Invalid password. Please try again." });
            }

            var userInDb = await _context.Users.FindAsync(managedUser.Id);

            if (userInDb is null)
            {
                return Unauthorized(request);
            }

            var roles = await _userManager.GetRolesAsync(userInDb);
            userInDb.Role = roles;

            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            // Login Alert Email - temporarily disabled to avoid SMTP timeout
            // try
            // {
            //     var userAgent = Request.Headers["User-Agent"].ToString();
            //     var browser = "Unknown Browser";
            //     if (userAgent.Contains("Chrome")) browser = "Google Chrome";
            //     else if (userAgent.Contains("Firefox")) browser = "Mozilla Firefox";
            //     else if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome")) browser = "Apple Safari";
            //     else if (userAgent.Contains("Edge")) browser = "Microsoft Edge";
            //
            //     var emailData = new Dictionary<string, string>
            //     {
            //         { "UserName", userInDb.UserName ?? "User" },
            //         { "LoginTime", DateTime.Now.ToString("f") },
            //         { "IpAddress", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown" },
            //         { "Device", userAgent.Contains("Windows") ? "Windows PC" : userAgent.Contains("Android") ? "Android Mobile" : userAgent.Contains("iPhone") ? "iPhone" : "Mobile/PC" },
            //         { "Browser", browser },
            //         { "Location", "Not specified (Security Policy)" }
            //     };
            //
            //     await _emailService.SendNotificationEmailAsync(
            //         NotificationEvent.LoginAlert,
            //         userInDb.Email!,
            //         userInDb.UserName ?? "User",
            //         emailData
            //     );
            // }
            // catch (Exception ex)
            // {
            //     // Log error but don't block login
            //      Console.WriteLine($"Failed to send login email: {ex.Message}");
            // }


            var staff = _context.Set<Staff>().FirstOrDefault(s => s.Email == request.Email);
            var student = _context.dbsStudent.FirstOrDefault(s => s.StudentEmail == request.Email);
            
            string fullName = staff?.StaffName ?? student?.StudentName ?? userInDb.UserName;
            int? studentId = student?.StudentId;

            return Ok(new AuthResponse
            {
                Id = userInDb.Id,
                Username = userInDb.UserName,
                Email = userInDb.Email,
                FullName = fullName,
                Token = accessToken,
                Roles = userInDb.Role.ToArray(),
                StudentId = studentId
            });
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> UserIndex()
        {
            var users = await _userManager.Users.Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role,
                Status = u.Status,
                CreatedOn = u.CreatedOn,
                PhoneNumber = u.PhoneNumber
            }).ToListAsync();
            
            return Ok(users);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> UserIndex(string id)
        {
            return Ok(await _userManager.FindByIdAsync(id));
        }

        [HttpGet]
        [Route("GetRoles")]
        [Authorize()]
        public async Task<IActionResult> RoleIndex()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleDtos = new List<UserRoleDto>();

            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var permissions = claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();
                
                roleDtos.Add(new UserRoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = permissions
                });
            }

            return Ok(roleDtos);
        }

        [HttpPost("create-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateRole([FromBody] UserRoleDto request)
        {
            IdentityRole role = new IdentityRole()
            {
                Name = request.Name,
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                if (request.Permissions != null && request.Permissions.Any())
                {
                    foreach (var perm in request.Permissions)
                    {
                        await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", perm));
                    }
                }
                return Ok(request);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
        [HttpPut("edit-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditRole([FromBody] UserRoleDto request)//1234 Admin
        {


            var role = await _roleManager.FindByIdAsync(request.Id);//1234 ADMINS

            if (role == null) return BadRequest();

            role.Name = request.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                // Update claims if permissions are provided
                if (request.Permissions != null)
                {
                    var existingClaims = await _roleManager.GetClaimsAsync(role);
                    var permissionClaims = existingClaims.Where(c => c.Type == "Permission").ToList();

                    // Remove current permission claims
                    foreach (var claim in permissionClaims)
                    {
                        await _roleManager.RemoveClaimAsync(role, claim);
                    }

                    // Add new permissions
                    foreach (var perm in request.Permissions)
                    {
                        await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", perm));
                    }
                }

                return Ok(request);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
        [HttpDelete("delete-role/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteRole(string id)//1234 Admin
        {


            IdentityRole? role = await _roleManager.FindByIdAsync(id);//1234 ADMIN

            if (role == null) return BadRequest();


            var users = await _userManager.GetUsersInRoleAsync(role.Name);


            if (users.Count > 0)
            {
                return BadRequest("User exists in this role");
            }


            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("AssignRole")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null || string.IsNullOrEmpty(model.Id))
            {
                return BadRequest("Invalid user data provided.");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user is null) return BadRequest($"User not found by id: {model.Id}");

                var userRoles = await _userManager.GetRolesAsync(user);
                List<string?> dbRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

                // Ensure model.Role is not null
                var requestedRoles = model.Role ?? new List<string>();

                foreach (var role in dbRoles)
                {
                    if (requestedRoles.Contains(role))
                    {
                        if (!userRoles.Contains(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }
                    else
                    {
                        if (userRoles.Contains(role))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role);
                        }
                    }
                }

                user.Role = requestedRoles;
                await _userManager.UpdateAsync(user);

                return Ok(new { message = "Role assigned successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid) 
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"[ChangePassword] Model invalid: {errors}");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) 
            {
                Console.WriteLine($"[ChangePassword] User not found: {model.Email}");
                return NotFound(new { message = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            
            if (result.Succeeded)
            {
                return Ok(new { message = "Password updated successfully" });
            }

            var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
            Console.WriteLine($"[ChangePassword] Identity error for {model.Email}: {identityErrors}");
            return BadRequest(new { message = identityErrors });
        }

        [HttpPost("force-reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ForceResetPassword([FromBody] ForceResetPasswordDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound(new { message = "User account not found." });

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded && removeResult.Errors.All(e => e.Code != "UserHasNoPassword"))
            {
                return BadRequest(new { message = string.Join(", ", removeResult.Errors.Select(e => e.Description)) });
            }

            var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (addResult.Succeeded)
            {
                return Ok(new { message = "Password has been force-reset successfully." });
            }

            return BadRequest(new { message = string.Join(", ", addResult.Errors.Select(e => e.Description)) });
        }


        [HttpPost]
        [Route("logout")]
        public Task<ActionResult> Logout()
        {
            return Task.FromResult<ActionResult>(Ok());
        }

        [HttpDelete("delete-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");
            
            // Check for associated staff record
            var staff = await _context.Set<Staff>().Include(s => s.StaffExperiences).FirstOrDefaultAsync(s => s.Email == user.Email);
            if (staff != null)
            {
                // Enforce safety constraints before deleting staff
                var isSectionTeacher = await _context.Sections.AnyAsync(s => s.StaffId == staff.StaffId);
                var hasSubjectAssignments = await _context.SubjectAssignments.AnyAsync(sa => sa.StaffId == staff.StaffId);
                var hasLeaves = await _context.Leaves.AnyAsync(l => l.StaffId == staff.StaffId);
                var hasMarks = await _context.dbsMark.AnyAsync(m => m.StaffId == staff.StaffId);

                if (isSectionTeacher || hasSubjectAssignments || hasLeaves || hasMarks)
                {
                    return BadRequest(new { message = "Cannot delete this login because the associated staff member has linked school records (Classes, Subjects, etc.). Please reassign their duties first." });
                }

                _context.Set<StaffExperience>().RemoveRange(staff.StaffExperiences ?? new List<StaffExperience>());
                _context.Set<Staff>().Remove(staff);
                await _context.SaveChangesAsync();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return Ok();
            
            return BadRequest(result.Errors);
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email is required");
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new { exists = user != null });
        }

        [HttpPut("toggle-status/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleUserStatus(string id, [FromBody] ToggleStatusDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");

            var isActive = dto.Status?.ToLower() == "active";

            if (isActive)
            {
                // Activate: remove lockout
                await _userManager.SetLockoutEnabledAsync(user, false);
                await _userManager.SetLockoutEndDateAsync(user, null);
                user.Status = "Active";
            }
            else
            {
                // Deactivate: lock out permanently
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                user.Status = "Inactive";
            }

            await _userManager.UpdateAsync(user);
            return Ok(new { message = $"User status updated to {dto.Status}" });
        }
    }

    public class ToggleStatusDto
    {
        public string Status { get; set; } = "Active";
    }

    public class ChangePasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ForceResetPasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}