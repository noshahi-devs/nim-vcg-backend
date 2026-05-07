using Microsoft.AspNetCore.Mvc;
using SchoolApiService.Services;
using SchoolApp.Models.DataModels;
using SchoolApp.Models.Email;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public SupportController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SubmitFeature")]
        public async Task<IActionResult> SubmitFeatureRequest([FromBody] FeatureRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            {
                return BadRequest(new { message = "Invalid request data. Title and Description are required." });
            }

            try
            {
                // Construct the email content
                var adminEmail = "noshahidevelopersinc@gmail.com";
                var subject = $"[NIM Feature Request] {request.Title} ({request.Category})";
                
                var htmlBody = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; color: #333;'>
                        <h2 style='color: #800000;'>New Feature Request Received</h2>
                        <hr style='border: 1px solid #eee;' />
                        <p><strong>Title:</strong> {request.Title}</p>
                        <p><strong>Category:</strong> {request.Category}</p>
                        <p><strong>Submitted By:</strong> {request.UserName ?? "Anonymous"} ({request.UserEmail ?? "N/A"})</p>
                        <div style='background: #f9f9f9; padding: 15px; border-radius: 8px; margin-top: 15px;'>
                            <strong>Description:</strong><br/>
                            {request.Description.Replace("\n", "<br/>")}
                        </div>
                        <p style='font-size: 12px; color: #999; margin-top: 20px;'>
                            This message was sent automatically from the Noshahi Institute Manager Support Portal.
                        </p>
                    </div>";

                var emailRequest = new EmailRequest
                {
                    To = adminEmail,
                    ToName = "Noshahi Developers Support",
                    Subject = subject,
                    HtmlBody = htmlBody,
                    IsHighPriority = true
                };

                // Send email to admin
                var result = await _emailService.SendEmailAsync(emailRequest);

                if (result.IsSuccess)
                {
                    return Ok(new { success = true, message = "Feature request submitted successfully. Our engineering team has been notified." });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Failed to send notification email. " + result.ErrorDetails });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
    }
}
