# NIM Email Notification System - Complete Implementation Guide

## 📧 Overview
This document provides comprehensive guidance for implementing and using the Email Notification System in Noshahi Institute Manager (NIM).

## 🚀 Installation & Setup

### Step 1: Configure Gmail App Password

1. Log in to your Gmail account
2. Go to: https://myaccount.google.com/security
3. Enable 2-Step Verification
4. Go to "App Passwords" section
5. Select "Mail" and "Windows Computer" (or your device)
6. Generate password and copy it
7. Paste in `appsettings.json` > `EmailSettings` > `SenderPassword`

### Step 2: Update appsettings.json

```json
{
  "EmailSettings": {
    "SenderEmail": "your-gmail@gmail.com",
    "SenderPassword": "your-16-char-app-password",
    "SenderName": "Noshahi Institute Manager"
  },
  "InstituteInfo": {
    "Name": "Your Institute Name",
    "Address": "Your Complete Address",
    "Phone": "+92-xxx-xxxxxxx",
    "Email": "info@yourinstitute.com",
    "Website": "https://yourinstitute.com",
    "Logo": "https://yourinstitute.com/logo.png"
  }
}
```

### Step 3: Register Services in Program.cs

Add the following to your `Program.cs`:

```csharp
using SchoolApiService.Services;
using SchoolApp.Models.Email;

// Configure Email Settings from appsettings.json
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("NotificationSettings"));
builder.Services.Configure<InstituteInfo>(
    builder.Configuration.GetSection("InstituteInfo"));

// Register Email Service
builder.Services.AddScoped<IEmailService, EmailService>();
```

### Step 4: Create Database Tables

Run the following SQL script or Entity Framework migration:

```sql
-- Notification Logs Table
CREATE TABLE NotificationLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RecipientEmail NVARCHAR(100) NOT NULL,
    RecipientName NVARCHAR(200),
    Subject NVARCHAR(500) NOT NULL,
    HtmlBody NVARCHAR(MAX) NOT NULL,
    NotificationType NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    ErrorMessage NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    SentAt DATETIME2,
    RetryCount INT DEFAULT 0,
    MessageId NVARCHAR(200),
    UserId INT,
    ExamId INT,
    StudentId INT,
    TeacherId INT,
    FeeId INT,
    Metadata NVARCHAR(MAX)
);

-- Notification Settings Table
CREATE TABLE NotificationSettings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SettingKey NVARCHAR(100) NOT NULL,
    IsEnabled BIT NOT NULL DEFAULT 1,
    Description NVARCHAR(500),
    Category NVARCHAR(50) NOT NULL,
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedBy INT,
    FOREIGN KEY (UpdatedBy) REFERENCES Users(Id)
);

-- Create Indexes for Performance
CREATE INDEX IX_NotificationLogs_RecipientEmail ON NotificationLogs(RecipientEmail);
CREATE INDEX IX_NotificationLogs_Status ON NotificationLogs(Status);
CREATE INDEX IX_NotificationLogs_CreatedAt ON NotificationLogs(CreatedAt);
CREATE INDEX IX_NotificationSettings_SettingKey ON NotificationSettings(SettingKey);
```

### Step 5: Add DbContext Configuration

Update your `SchoolDbContext.cs`:

```csharp
public DbSet<NotificationLog> NotificationLogs { get; set; }
public DbSet<NotificationSettingsDb> NotificationSettings { get; set; }
```

## 📝 Usage Examples

### Example 1: Exam Schedule Published

```csharp
public class ExamsController : ControllerBase
{
    private readonly IEmailService _emailService;
    
    [HttpPost("publish-schedule")]
    public async Task<IActionResult> PublishExamSchedule(int examId)
    {
        // Your business logic to publish exam schedule
        var exam = await _context.Exams.FindAsync(examId);
        var students = await _context.Students
            .Where(s => s.ClassId == exam.ClassId)
            .ToListAsync();
        
        // Send email to each student
        foreach (var student in students)
        {
            var emailData = new Dictionary<string, string>
            {
                { "StudentName", student.Name },
                { "ExamName", exam.Name },
                { "ClassName", exam.Class.Name },
                { "SubjectName", exam.Subject.Name },
                { "ExamDate", exam.ExamDate.ToString("MMMM dd, yyyy") },
                { "ExamTime", exam.StartTime + " - " + exam.EndTime },
                { "Duration", exam.DurationMinutes.ToString() },
                { "TotalMarks", exam.TotalMarks.ToString() },
                { "CalculatorAllowed", exam.AllowCalculator ? "Yes" : "No" }
            };
            
            await _emailService.SendNotificationEmailAsync(
                NotificationEvent.ExamSchedulePublished,
                student.Email,
                student.Name,
                emailData
            );
        }
        
        return Ok(new { message = "Exam schedule published and notifications sent" });
    }
}
```

### Example 2: Result Announcement

```csharp
[HttpPost("announce-results")]
public async Task<IActionResult> AnnounceResults(int examId)
{
    var results = await _context.ExamResults
        .Include(r => r.Student)
        .Include(r => r.Exam)
        .Where(r => r.ExamId == examId)
        .ToListAsync();
    
    foreach (var result in results)
    {
        var emailData = new Dictionary<string, string>
        {
            { "StudentName", result.Student.Name },
            { "ExamName", result.Exam.Name },
            { "ClassName", result.Student.Class.Name },
            { "RollNumber", result.Student.RollNumber },
            { "ResultPublishDate", DateTime.Now.ToString("MMMM dd, yyyy") },
            { "TotalSubjects", result.SubjectResults.Count.ToString() },
            { "ObtainedMarks", result.TotalObtainedMarks.ToString() },
            { "TotalMarks", result.TotalMarks.ToString() },
            { "Percentage", result.Percentage.ToString("F2") },
            { "Grade", result.Grade },
            { "ResultStatus", result.IsPassed ? "PASSED" : "FAILED" },
            { "ResultColor", result.IsPassed ? "#10b981" : "#ef4444" },
            { "VerificationDeadline", DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy") }
        };
        
        await _emailService.SendNotificationEmailAsync(
            NotificationEvent.ResultAnnounced,
            result.Student.Email,
            result.Student.Name,
            emailData
        );
    }
    
    return Ok(new { message = "Results announced and notifications sent" });
}
```

### Example 3: Login Alert (in Authentication Controller)

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    // Your authentication logic
    var user = await _authService.AuthenticateAsync(request.Email, request.Password);
    
    if (user != null)
    {
        // Send login alert email
        var emailData = new Dictionary<string, string>
        {
            { "UserName", user.Name },
            { "LoginTime", DateTime.Now.ToString("MMMM dd, yyyy at hh:mm tt") },
            { "IpAddress", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown" },
            { "Device", Request.Headers["User-Agent"].ToString().Contains("Windows") ? "Windows" : "Other" },
            { "Browser", GetBrowserName(Request.Headers["User-Agent"].ToString()) },
            { "Location", await GetLocationFromIP(HttpContext.Connection.RemoteIpAddress) }
        };
        
        // Don't await - send async in background
        _ = _emailService.SendNotificationEmailAsync(
            NotificationEvent.LoginAlert,
            user.Email,
            user.Name,
            emailData
        );
        
        return Ok(new { token = GenerateToken(user) });
    }
    
    return Unauthorized();
}
```

### Example 4: Fee Voucher Generation

```csharp
[HttpPost("generate-fee-voucher")]
public async Task<IActionResult> GenerateFeeVoucher(int studentId, string month)
{
    var student = await _context.Students.FindAsync(studentId);
    var voucher = await GenerateVoucherAsync(student, month);
    
    var emailData = new Dictionary<string, string>
    {
        { "StudentName", student.Name },
        { "Month", month },
        { "VoucherNumber", voucher.VoucherNumber },
        { "ClassName", student.Class.Name },
        { "RollNumber", student.RollNumber },
        { "TuitionFee", voucher.TuitionFee.ToString("C") },
        { "TransportFee", voucher.TransportFee.ToString("C") },
        { "OtherCharges", voucher.OtherCharges.ToString("C") },
        { "TotalAmount", voucher.TotalAmount.ToString("C") },
        { "DueDate", voucher.DueDate.ToString("MMMM dd, yyyy") },
        { "LateFee", "PKR 500" }
    };
    
    await _emailService.SendNotificationEmailAsync(
        NotificationEvent.FeeVoucherGenerated,
        student.ParentEmail ?? student.Email,
        student.ParentName ?? student.Name,
        emailData
    );
    
    return Ok(voucher);
}
```

## 🎨 Creating Custom Email Templates

### Template Location
Place templates in: `SchoolApiService/EmailTemplates/`

###Template Structure
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        /* Your custom styles */
    </style>
</head>
<body>
    <div class="container">
        <!-- Use placeholders with double curly braces -->
        <p>Dear {{StudentName}},</p>
        <p>Your {{ExamName}} details...</p>
        
        <!-- Institute branding (automatically replaced) -->
        <img src="{{InstituteLogo}}" alt="{{InstituteName}}">
        <p>{{InstituteAddress}}</p>
        <p>{{InstitutePhone}} | {{InstituteEmail}}</p>
    </div>
</body>
</html>
```

## ⚙️ Admin Controls - Enable/Disable Notifications

### Via API

```csharp
[HttpPost("toggle-notification")]
public async Task<IActionResult> ToggleNotification(string settingKey, bool isEnabled)
{
    var setting = await _context.NotificationSettings
        .FirstOrDefaultAsync(s => s.SettingKey == settingKey);
    
    if (setting == null)
    {
        setting = new NotificationSettingsDb
        {
            SettingKey = settingKey,
            IsEnabled = isEnabled,
            Category = GetCategoryFromKey(settingKey),
            UpdatedBy = GetCurrentUserId()
        };
        _context.NotificationSettings.Add(setting);
    }
    else
    {
        setting.IsEnabled = isEnabled;
        setting.UpdatedAt = DateTime.UtcNow;
        setting.UpdatedBy = GetCurrentUserId();
    }
    
    await _context.SaveChangesAsync();
    return Ok(new { message = "Notification setting updated" });
}
```

## 📊 Monitoring & Logs

### View Email Logs

```csharp
[HttpGet("email-logs")]
public async Task<IActionResult> GetEmailLogs(
    int page = 1, 
    int pageSize = 50,
    string? status = null)
{
    var query = _context.NotificationLogs.AsQueryable();
    
    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(l => l.Status == status);
    }
    
    var logs = await query
        .OrderByDescending(l => l.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(l => new
        {
            l.Id,
            l.RecipientEmail,
            l.Subject,
            l.NotificationType,
            l.Status,
            l.CreatedAt,
            l.SentAt,
            l.ErrorMessage
        })
        .ToListAsync();
    
    return Ok(logs);
}
```

## 🔧 Troubleshooting

### Email Not Sending

1. **Check SMTP Settings**: Ensure Gmail App Password is correct
2. **Check Firewall**: Port 587 must be open
3. **Check Logs**: Review `NotificationLogs` table for errors
4. **Test Connection**: Use telnet to test SMTP connectivity

### Template Not Found

1. Ensure template file exists in `EmailTemplates/` folder
2. Check file name matches exactly (case-sensitive)
3. Verify file is included in project (Build Action: Content, Copy to Output: Copy if newer)

### Notification Not Triggering

1. Check `NotificationSettings` table - ensure setting is enabled
2. Check `appsettings.json` - ensure `EnableNotifications` is true
3. Verify event type matches enum value

## 📈 Performance Optimization

### Async Background Processing

For bulk emails, use background tasks:

```csharp
// Install: Microsoft.Extensions.Hosting
public class EmailBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Process email queue
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}

// Register in Program.cs
builder.Services.AddHostedService<EmailBackgroundService>();
```

## 🔒 Security Best Practices

1. **Never commit** `appsettings.json` with actual passwords
2. Use **Environment Variables** for production:
   ```csharp
   builder.Configuration.GetValue<string>("EmailSettings:SenderPassword")
   ```
3. Implement **rate limiting** to prevent email spam
4. Validate email addresses before sending
5. Use **SSL/TLS** for SMTP connections (already configured)

## 📦 Additional Email Templates Needed

You should create templates for:
- `NewAccountCreation.html`
- `PasswordReset.html`
- `ExamDateUpdated.html`
- `ResultUpdated.html`
- `LeaveApproved.html`
- `LeaveRejected.html`
- `TeacherAssigned.html`
- `ClassSectionChange.html`
- `FeePaymentReceived.html`
- `SalarySlipGenerated.html`
- `AnnouncementPublished.html`
- `MaintenanceAlert.html`

## 📞 Support

For issues or questions, contact:
- Email: support@noshahiinstitute.com
- Documentation: https://docs.noshahiinstitute.com
