-- ================================================
-- NIM Email Notification System - Database Setup
-- ================================================
-- Run this script to create all necessary tables
-- Execute in SQL Server Management Studio (SSMS)
-- ================================================

USE SchoolSystemDb;
GO

-- ================================================
-- 1. Create NotificationLogs Table
-- ================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationLogs')
BEGIN
    CREATE TABLE [dbo].[NotificationLogs] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [RecipientEmail] NVARCHAR(100) NOT NULL,
        [RecipientName] NVARCHAR(200) NULL,
        [Subject] NVARCHAR(500) NOT NULL,
        [HtmlBody] NVARCHAR(MAX) NOT NULL,
        [NotificationType] NVARCHAR(50) NOT NULL,
        [Status] NVARCHAR(50) NOT NULL,
        [ErrorMessage] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [SentAt] DATETIME2 NULL,
        [RetryCount] INT NOT NULL DEFAULT 0,
        [MessageId] NVARCHAR(200) NULL,
        [UserId] INT NULL,
        [ExamId] INT NULL,
        [StudentId] INT NULL,
        [TeacherId] INT NULL,
        [FeeId] INT NULL,
        [Metadata] NVARCHAR(MAX) NULL,
        CONSTRAINT [CK_NotificationLogs_Status] CHECK ([Status] IN ('Sent', 'Failed', 'Pending'))
    );

    PRINT 'Table [NotificationLogs] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [NotificationLogs] already exists.';
END
GO

-- ================================================
-- 2. Create NotificationSettings Table
-- ================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationSettings')
BEGIN
    CREATE TABLE [dbo].[NotificationSettings] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [SettingKey] NVARCHAR(100) NOT NULL UNIQUE,
        [IsEnabled] BIT NOT NULL DEFAULT 1,
        [Description] NVARCHAR(500) NULL,
        [Category] NVARCHAR(50) NOT NULL,
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] INT NULL,
        CONSTRAINT [FK_NotificationSettings_Users] FOREIGN KEY ([UpdatedBy]) 
            REFERENCES [dbo].[Users]([Id])
    );

    PRINT 'Table [NotificationSettings] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [NotificationSettings] already exists.';
END
GO

-- ================================================
-- 3. Create Indexes for Performance
-- ================================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotificationLogs_RecipientEmail')
BEGIN
    CREATE INDEX [IX_NotificationLogs_RecipientEmail] ON [dbo].[NotificationLogs]([RecipientEmail]);
    PRINT 'Index [IX_NotificationLogs_RecipientEmail] created.';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotificationLogs_Status')
BEGIN
    CREATE INDEX [IX_NotificationLogs_Status] ON [dbo].[NotificationLogs]([Status]);
    PRINT 'Index [IX_NotificationLogs_Status] created.';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotificationLogs_CreatedAt')
BEGIN
    CREATE INDEX [IX_NotificationLogs_CreatedAt] ON [dbo].[NotificationLogs]([CreatedAt] DESC);
    PRINT 'Index [IX_NotificationLogs_CreatedAt] created.';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotificationLogs_NotificationType')
BEGIN
    CREATE INDEX [IX_NotificationLogs_NotificationType] ON [dbo].[NotificationLogs]([NotificationType]);
    PRINT 'Index [IX_NotificationLogs_NotificationType] created.';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotificationSettings_SettingKey')
BEGIN
    CREATE INDEX [IX_NotificationSettings_SettingKey] ON [dbo].[NotificationSettings]([SettingKey]);
    PRINT 'Index [IX_NotificationSettings_SettingKey] created.';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotificationSettings_Category')
BEGIN
    CREATE INDEX [IX_NotificationSettings_Category] ON [dbo].[NotificationSettings]([Category]);
    PRINT 'Index [IX_NotificationSettings_Category] created.';
END
GO

-- ================================================
-- 4. Insert Default Notification Settings
-- ================================================
PRINT 'Inserting default notification settings...';

MERGE INTO [dbo].[NotificationSettings] AS Target
USING (VALUES
    -- Authentication Notifications
    ('SendLoginAlert', 1, 'Send email alerts when users log in', 'Authentication'),
    ('SendNewAccountCreation', 1, 'Send welcome email with credentials for new accounts', 'Authentication'),
    ('SendPasswordResetRequest', 1, 'Send password reset link emails', 'Authentication'),
    
    -- Academic Notifications
    ('SendExamSchedulePublished', 1, 'Notify students when exam schedule is published', 'Academic'),
    ('SendExamDateUpdated', 1, 'Notify students when exam dates are changed', 'Academic'),
    ('SendResultAnnounced', 1, 'Notify students when results are published', 'Academic'),
    ('SendResultUpdated', 1, 'Notify students when results are updated/rechecked', 'Academic'),
    
    -- Leave Notifications
    ('SendLeaveApproved', 1, 'Notify staff when leave is approved', 'Leave'),
    ('SendLeaveRejected', 1, 'Notify staff when leave is rejected', 'Leave'),
    
    -- Assignment Notifications
    ('SendTeacherAssigned', 1, 'Notify teachers when assigned to class/subject', 'Assignment'),
    ('SendClassOrSectionChange', 1, 'Notify students about class/section changes', 'Assignment'),
    
    -- Finance Notifications
    ('SendFeeVoucherGenerated', 1, 'Send fee vouchers to students/parents', 'Finance'),
    ('SendFeePaymentReceived', 1, 'Send payment confirmation receipts', 'Finance'),
    ('SendSalarySlipGenerated', 1, 'Send salary slips to staff', 'Finance'),
    
    -- System Notifications
    ('SendAnnouncements', 1, 'Send important announcements', 'System'),
    ('SendMaintenanceAlerts', 1, 'Send system maintenance notifications', 'System')
) AS Source ([SettingKey], [IsEnabled], [Description], [Category])
ON Target.[SettingKey] = Source.[SettingKey]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([SettingKey], [IsEnabled], [Description], [Category], [UpdatedAt])
    VALUES (Source.[SettingKey], Source.[IsEnabled], Source.[Description], Source.[Category], GETUTCDATE());

PRINT CAST(@@ROWCOUNT AS VARCHAR(10)) + ' notification settings inserted/updated.';
GO

-- ================================================
-- 5. Create Stored Procedure for Email Log Cleanup
-- ================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_CleanupOldEmailLogs')
BEGIN
    DROP PROCEDURE [dbo].[sp_CleanupOldEmailLogs];
END
GO

CREATE PROCEDURE [dbo].[sp_CleanupOldEmailLogs]
    @DaysToKeep INT = 90
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CutoffDate DATETIME2 = DATEADD(DAY, -@DaysToKeep, GETUTCDATE());
    DECLARE @DeletedCount INT;
    
    DELETE FROM [dbo].[NotificationLogs]
    WHERE [CreatedAt] < @CutoffDate AND [Status] = 'Sent';
    
    SET @DeletedCount = @@ROWCOUNT;
    
    PRINT 'Cleanup completed. Deleted ' + CAST(@DeletedCount AS VARCHAR(10)) + ' old email logs.';
    
    RETURN @DeletedCount;
END
GO

PRINT 'Stored procedure [sp_CleanupOldEmailLogs] created successfully.';
GO

-- ================================================
-- 6. Create View for Email Statistics
-- ================================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_EmailStatistics')
BEGIN
    DROP VIEW [dbo].[vw_EmailStatistics];
END
GO

CREATE VIEW [dbo].[vw_EmailStatistics]
AS
SELECT 
    [NotificationType],
    [Status],
    COUNT(*) AS [TotalCount],
    COUNT(CASE WHEN [CreatedAt] >= DATEADD(DAY, -7, GETUTCDATE()) THEN 1 END) AS [Last7Days],
    COUNT(CASE WHEN [CreatedAt] >= DATEADD(DAY, -30, GETUTCDATE()) THEN 1 END) AS [Last30Days],
    AVG(CASE WHEN [Status] = 'Sent' THEN [RetryCount] END) AS [AvgRetries],
    MAX([CreatedAt]) AS [LastSent]
FROM [dbo].[NotificationLogs]
GROUP BY [NotificationType], [Status];
GO

PRINT 'View [vw_EmailStatistics] created successfully.';
GO

-- ================================================
-- 7. Grant Permissions (Optional - Adjust as needed)
-- ================================================
-- If using a specific database user, grant permissions:
-- GRANT SELECT, INSERT, UPDATE ON [dbo].[NotificationLogs] TO [YourDatabaseUser];
-- GRANT SELECT, INSERT, UPDATE ON [dbo].[NotificationSettings] TO [YourDatabaseUser];
-- GRANT EXECUTE ON [dbo].[sp_CleanupOldEmailLogs] TO [YourDatabaseUser];
-- GRANT SELECT ON [dbo].[vw_EmailStatistics] TO [YourDatabaseUser];

-- ================================================
-- 8. Verification Queries
-- ================================================
PRINT '';
PRINT '========================================';
PRINT 'Database setup completed successfully!';
PRINT '========================================';
PRINT '';

-- Check tables
SELECT 
    'NotificationLogs' AS [TableName],
    COUNT(*) AS [RecordCount]
FROM [dbo].[NotificationLogs]
UNION ALL
SELECT 
    'NotificationSettings',
    COUNT(*)
FROM [dbo].[NotificationSettings];

-- Display settings
PRINT 'Current Notification Settings:';
SELECT 
    [Category],
    [SettingKey],
    CASE WHEN [IsEnabled] = 1 THEN 'Enabled' ELSE 'Disabled' END AS [Status],
    [Description]
FROM [dbo].[NotificationSettings]
ORDER BY [Category], [SettingKey];

PRINT '';
PRINT '✅ All tables, indexes, and stored procedures created successfully!';
PRINT '📧 Email notification system is ready to use.';
PRINT '';
PRINT 'Next steps:';
PRINT '1. Update appsettings.json with your Gmail App Password';
PRINT '2. Configure institute information in appsettings.json';
PRINT '3. Run the application and test email sending';
PRINT '';
GO
