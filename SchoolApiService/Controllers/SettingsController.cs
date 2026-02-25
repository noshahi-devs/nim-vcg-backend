using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController(SchoolDbContext context) : ControllerBase
    {
        private readonly SchoolDbContext _context = context;

        // ================= GENERAL SETTINGS =================

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralSettings()
        {
            var settings = await _context.SystemSettings
                .Where(s => s.Category == "General")
                .ToListAsync();
            return Ok(settings);
        }

        [HttpPost("general")]
        public async Task<IActionResult> UpdateGeneralSettings(List<SystemSetting> settings)
        {
            foreach (var setting in settings)
            {
                var existing = await _context.SystemSettings
                    .FirstOrDefaultAsync(s => s.SettingKey == setting.SettingKey && s.Category == "General");

                if (existing != null)
                {
                    existing.SettingValue = setting.SettingValue;
                    existing.UpdatedAt = DateTime.UtcNow;
                    _context.Entry(existing).State = EntityState.Modified;
                }
                else
                {
                    setting.Category = "General";
                    setting.UpdatedAt = DateTime.UtcNow;
                    _context.SystemSettings.Add(setting);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "General settings updated successfully" });
        }

        // ================= NOTIFICATION SETTINGS =================

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotificationSettings()
        {
            var settings = await _context.NotificationSettings.ToListAsync();
            return Ok(settings);
        }

        [HttpPost("notifications")]
        public async Task<IActionResult> UpdateNotificationSettings(List<NotificationSettingsDb> settings)
        {
            foreach (var setting in settings)
            {
                var existing = await _context.NotificationSettings.FindAsync(setting.Id);
                if (existing != null)
                {
                    existing.IsEnabled = setting.IsEnabled;
                    existing.UpdatedAt = DateTime.UtcNow;
                    _context.Entry(existing).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Notification settings updated successfully" });
        }

        // ================= PAYMENT GATEWAY SETTINGS =================

        [HttpGet("payment-gateway")]
        public async Task<IActionResult> GetPaymentGatewaySettings()
        {
            var settings = await _context.PaymentGatewaySettings.ToListAsync();
            return Ok(settings);
        }

        [HttpPost("payment-gateway")]
        public async Task<IActionResult> UpdatePaymentGatewaySettings(PaymentGatewaySetting setting)
        {
            var existing = await _context.PaymentGatewaySettings.FindAsync(setting.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(setting);
                existing.IsActive = setting.IsActive; // Explicitly map if needed
                _context.Entry(existing).State = EntityState.Modified;
            }
            else
            {
                _context.PaymentGatewaySettings.Add(setting);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Payment gateway settings updated successfully" });
        }
    }
}
