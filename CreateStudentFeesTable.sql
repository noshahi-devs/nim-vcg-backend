-- Run this in SQL Server Management Studio against your school database
-- This creates the StudentFees table that stores custom fee overrides per student

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudentFees')
BEGIN
    CREATE TABLE [dbo].[StudentFees] (
        [StudentFeeId]   INT             IDENTITY(1,1) NOT NULL,
        [StudentId]      INT             NOT NULL,
        [FeeId]          INT             NOT NULL,
        [AssignedAmount] DECIMAL(18, 2)  NOT NULL,
        [CreatedAt]      DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT [PK_StudentFees] PRIMARY KEY ([StudentFeeId]),

        CONSTRAINT [FK_StudentFees_Student] FOREIGN KEY ([StudentId])
            REFERENCES [dbo].[dbsStudent] ([StudentId])
            ON DELETE CASCADE,

        CONSTRAINT [FK_StudentFees_Fee] FOREIGN KEY ([FeeId])
            REFERENCES [dbo].[fees] ([FeeId])
            ON DELETE CASCADE
    );

    CREATE INDEX [IX_StudentFees_StudentId] ON [dbo].[StudentFees] ([StudentId]);
    CREATE INDEX [IX_StudentFees_FeeId]     ON [dbo].[StudentFees] ([FeeId]);

    -- Register migration so EF Core doesn't try to re-run it
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260422_AddStudentFeesTable', '8.0.4');

    PRINT 'StudentFees table created successfully.';
END
ELSE
BEGIN
    PRINT 'StudentFees table already exists. No changes made.';
END
