using Microsoft.AspNetCore.Mvc;
using SchoolApiService.Services;
using SchoolApp.Models.Email;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamplesEmailUsageController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ExamplesEmailUsageController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Example 1: Send Exam Schedule Published Notification
        /// </summary>
        [HttpPost("send-exam-schedule")]
        public async Task<IActionResult> SendExamScheduleNotification()
        {
            try
            {
                var emailData = new Dictionary<string, string>
                {
                    { "EventType", "ExamSchedulePublished" },
                    { "StudentName", "Ahmed Khan" },
                    { "ExamName", "Mid-Term Examination 2025" },
                    { "ClassName", "Class 10-A" },
                    { "SubjectName", "Mathematics" },
                    { "ExamDate", "15th February 2025" },
                    { "ExamTime", "09:00 AM - 12:00 PM" },
                    { "Duration", "180" },
                    { "TotalMarks", "100" },
                    { "CalculatorAllowed", "Yes" }
                };

                var result = await _emailService.SendNotificationEmailAsync(
                    NotificationEvent.ExamSchedulePublished,
                    "student@example.com",
                    "Ahmed Khan",
                    emailData
                );

                return Ok(new { success = result.IsSuccess, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Example 2: Send Result Announcement
        /// </summary>
        [HttpPost("send-result-announcement")]
        public async Task<IActionResult> SendResultAnnouncement()
        {
            try
            {
                var emailData = new Dictionary<string, string>
                {
                    { "EventType", "ResultAnnounced" },
                    { "StudentName", "Fatima Ali" },
                    { "ExamName", "Final Term Examination 2024" },
                    { "ClassName", "Class 9-B" },
                    { "RollNumber", "2024-FB-0456" },
                    { "ResultPublishDate", "January 25, 2025" },
                    { "TotalSubjects", "8" },
                    { "ObtainedMarks", "720" },
                    { "TotalMarks", "800" },
                    { "Percentage", "90" },
                    { "Grade", "A+" },
                    { "ResultStatus", "PASSED" },
                    { "ResultColor", "#10b981" }, // Green for pass
                    { "VerificationDeadline", "February 5, 2025" }
                };

                var result = await _emailService.SendNotificationEmailAsync(
                    NotificationEvent.ResultAnnounced,
                    "student@example.com",
                    "Fatima Ali",
                    emailData
                );

                return Ok(new { success = result.IsSuccess, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Example 3: Send Login Alert
        /// </summary>
        [HttpPost("send-login-alert")]
        public async Task<IActionResult> SendLoginAlert()
        {
            try
            {
                var emailData = new Dictionary<string, string>
                {
                    { "EventType", "LoginAlert" },
                    { "UserName", "Dr. Muhammad Bilal" },
                    { "LoginTime", DateTime.Now.ToString("MMMM dd, yyyy at hh:mm tt") },
                    { "IpAddress", "192.168.1.105" },
                    { "Device", "Windows Desktop" },
                    { "Browser", "Google Chrome 120.0" },
                    { "Location", "Lahore, Punjab, Pakistan" }
                };

                var result = await _emailService.SendNotificationEmailAsync(
                    NotificationEvent.LoginAlert,
                    "teacher@example.com",
                    "Dr. Muhammad Bilal",
                    emailData
                );

                return Ok(new { success = result.IsSuccess, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Example 4: Send Fee Voucher Generated Notification
        /// </summary>
        [HttpPost("send-fee-voucher")]
        public async Task<IActionResult> SendFeeVoucherNotification()
        {
            try
            {
                var emailData = new Dictionary<string, string>
                {
                    { "EventType", "FeeVoucherGenerated" },
                    { "StudentName", "Ayesha Malik" },
                    { "Month", "February 2025" },
                    { "VoucherNumber", "FV-2025-02-1234" },
                    { "TotalAmount", "PKR 15,000" },
                    { "DueDate", "February 10, 2025" },
                    { "LateFee", "PKR 500" },
                    { "ClassName", "Class 8-C" },
                    { "RollNumber", "2024-AC-0789" }
                };

                var result = await _emailService.SendNotificationEmailAsync(
                    NotificationEvent.FeeVoucherGenerated,
                    "parent@example.com",
                    "Mr. & Mrs. Malik",
                    emailData
                );

                return Ok(new { success = result.IsSuccess, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Example 5: Send Leave Approval Notification
        /// </summary>
        [HttpPost("send-leave-approval")]
        public async Task<IActionResult> SendLeaveApproval()
        {
            try
            {
                var emailData = new Dictionary<string, string>
                {
                    { "EventType", "LeaveApproved" },
                    { "TeacherName", "Ms. Sara Ahmed" },
                    { "LeaveType", "Medical Leave" },
                    { "FromDate", "February 15, 2025" },
                    { "ToDate", "February 18, 2025" },
                    { "TotalDays", "4" },
                    { "ApprovedBy", "Principal Dr. Hassan" },
                    { "ApprovalDate", "February 5, 2025" },
                    { "Reason", "Medical treatment as per doctor's recommendation" },
                    { "Comments", "Please ensure all pending work is completed before leave" }
                };

                var result = await _emailService.SendNotificationEmailAsync(
                    NotificationEvent.LeaveApproved,
                    "teacher@example.com",
                    "Ms. Sara Ahmed",
                    emailData
                );

                return Ok(new { success = result.IsSuccess, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Example 6: Send Bulk Notifications (to all students in a class)
        /// </summary>
        [HttpPost("send-bulk-exam-notifications")]
        public async Task<IActionResult> SendBulkExamNotifications()
        {
            try
            {
                // Simulating student list from database
                var students = new List<(string Email, string Name, string RollNo)>
                {
                    ("student1@example.com", "Ali Hassan", "2024-001"),
                    ("student2@example.com", "Zainab Khan", "2024-002"),
                    ("student3@example.com", "Usman Ahmad", "2024-003")
                };

                var emailRequests = new List<EmailRequest>();

                foreach (var student in students)
                {
                    var emailData = new Dictionary<string, string>
                    {
                        { "StudentName", student.Name },
                        { "ExamName", "Monthly Test - March 2025" },
                        { "ClassName", "Class 10-A" },
                        { "SubjectName", "Physics" },
                        { "ExamDate", "March 5, 2025" },
                        { "ExamTime", "10:00 AM - 12:00 PM" },
                        { "Duration", "120" },
                        { "TotalMarks", "75" },
                        { "CalculatorAllowed", "Yes" }
                    };

                    var request = new EmailRequest
                    {
                        To = student.Email,
                        ToName = student.Name,
                        Subject = "Exam Schedule Published - Monthly Test March 2025",
                        TemplateData = emailData
                    };

                    emailRequests.Add(request);
                }

                var success = await _emailService.SendBulkEmailsAsync(emailRequests);

                return Ok(new { 
                    success, 
                    message = $"Sent {emailRequests.Count} notifications",
                    totalRecipients = students.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Example 7: Send Custom Email with Attachment
        /// </summary>
        [HttpPost("send-salary-slip")]
        public async Task<IActionResult> SendSalarySlipWithAttachment()
        {
            try
            {
                // Simulating PDF generation (in real scenario, generate actual PDF)
                byte[] pdfContent = System.Text.Encoding.UTF8.GetBytes("Sample PDF Content");

                var emailData = new Dictionary<string, string>
                {
                    { "TeacherName", "Prof. Ahmed Raza" },
                    { "Month", "January 2025" },
                    { "BasicSalary", "PKR 80,000" },
                    { "Allowances", "PKR 20,000" },
                    { "Deductions", "PKR 5,000" },
                    { "NetSalary", "PKR 95,000" }
                };

                var request = new EmailRequest
                {
                    To = "teacher@example.com",
                    ToName = "Prof. Ahmed Raza",
                    Subject = "Salary Slip - January 2025",
                    TemplateData = emailData,
                    Attachments = new List<EmailAttachment>
                    {
                        new EmailAttachment
                        {
                            FileName = "SalarySlip_Jan2025.pdf",
                            Content = pdfContent,
                            ContentType = "application/pdf"
                        }
                    }
                };

                var result = await _emailService.SendEmailWithTemplateAsync(request, "SalarySlipGenerated");

                return Ok(new { success = result.IsSuccess, message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
