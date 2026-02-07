using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolApiService.Controllers;
using SchoolApiService.Services;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels.SecurityModels;
using SchoolApp.Models.Email;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SchoolApi.Tests
{
    public class AccountControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private SchoolDbContext _context;
        private Mock<ITokenService> _mockTokenService;
        private Mock<IEmailService> _mockEmailService;
        private UsersController _controller;

        public AccountControllerTests()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Unique DB per test
                .Options;
            _context = new SchoolDbContext(options);

            _mockTokenService = new Mock<ITokenService>();
            _mockEmailService = new Mock<IEmailService>();

            _controller = new UsersController(
                _mockUserManager.Object,
                _mockRoleManager.Object,
                _context,
                _mockTokenService.Object,
                _mockEmailService.Object
            );

            // Mock HttpContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            _controller.HttpContext.Request.Headers["User-Agent"] = "TestAgent";
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_SendsLoginAlertEmail()
        {
            // Arrange
            var email = "test@example.com";
            var username = "testuser";
            var user = new ApplicationUser 
            { 
                UserName = username, 
                Email = email,
                Role = new List<string> { "User" }
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);
            
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);
                
            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<ApplicationUser>()))
                .Returns("fake-jwt-token");

            var request = new AuthRequest { Email = email, Password = "password" };

            // Act
            var result = await _controller.Authenticate(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            
            // Verify email service was called once with LoginAlert event
            _mockEmailService.Verify(x => x.SendNotificationEmailAsync(
                NotificationEvent.LoginAlert,
                email,
                username,
                It.IsAny<Dictionary<string, string>>()
            ), Times.Once);
        }
    }
}
