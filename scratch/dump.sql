CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedOn] nvarchar(max) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Attendance] (
    [AttendanceId] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [Type] int NOT NULL,
    [AttendanceIdentificationNumber] int NOT NULL,
    [Description] nvarchar(max) NULL,
    [IsPresent] bit NOT NULL,
    [CheckOutTime] datetime2 NULL,
    CONSTRAINT [PK_Attendance] PRIMARY KEY ([AttendanceId])
);
GO


CREATE TABLE [Campus] (
    [CampusId] int NOT NULL IDENTITY,
    [CampusName] nvarchar(100) NOT NULL,
    [CampusCode] nvarchar(20) NOT NULL,
    [Address] nvarchar(max) NULL,
    [ContactNumber] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Campus] PRIMARY KEY ([CampusId])
);
GO


CREATE TABLE [ExamType] (
    [ExamTypeId] int NOT NULL IDENTITY,
    [ExamTypeName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ExamType] PRIMARY KEY ([ExamTypeId])
);
GO


CREATE TABLE [FeeType] (
    [FeeTypeId] int NOT NULL IDENTITY,
    [TypeName] nvarchar(max) NULL,
    CONSTRAINT [PK_FeeType] PRIMARY KEY ([FeeTypeId])
);
GO


CREATE TABLE [GradeScales] (
    [GradeId] int NOT NULL IDENTITY,
    [Grade] nvarchar(10) NOT NULL,
    [MinPercentage] decimal(18,2) NOT NULL,
    [MaxPercentage] decimal(18,2) NOT NULL,
    [GradePoint] decimal(18,2) NULL,
    [Remarks] nvarchar(max) NULL,
    [CreatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_GradeScales] PRIMARY KEY ([GradeId])
);
GO


CREATE TABLE [LeaveTypeMasters] (
    [LeaveTypeMasterId] int NOT NULL IDENTITY,
    [LeaveTypeName] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [MaxDaysAllowed] int NOT NULL,
    [IsPaid] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_LeaveTypeMasters] PRIMARY KEY ([LeaveTypeMasterId])
);
GO


CREATE TABLE [NotificationLogs] (
    [Id] int NOT NULL IDENTITY,
    [RecipientEmail] nvarchar(100) NOT NULL,
    [RecipientName] nvarchar(200) NOT NULL,
    [Subject] nvarchar(500) NOT NULL,
    [HtmlBody] nvarchar(max) NOT NULL,
    [NotificationType] nvarchar(50) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [SentAt] datetime2 NULL,
    [RetryCount] int NOT NULL,
    [MessageId] nvarchar(200) NULL,
    [UserId] int NULL,
    [ExamId] int NULL,
    [StudentId] int NULL,
    [TeacherId] int NULL,
    [FeeId] int NULL,
    [Metadata] nvarchar(max) NULL,
    CONSTRAINT [PK_NotificationLogs] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Notifications] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Link] nvarchar(500) NULL,
    [IsRead] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [NotificationType] nvarchar(50) NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [NotificationSettings] (
    [Id] int NOT NULL IDENTITY,
    [SettingKey] nvarchar(100) NOT NULL,
    [IsEnabled] bit NOT NULL,
    [Description] nvarchar(500) NULL,
    [Category] nvarchar(50) NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [UpdatedBy] int NULL,
    CONSTRAINT [PK_NotificationSettings] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [PaymentGatewaySettings] (
    [Id] int NOT NULL IDENTITY,
    [GatewayName] nvarchar(50) NOT NULL,
    [ApiKey] nvarchar(200) NOT NULL,
    [SecretKey] nvarchar(200) NOT NULL,
    [IsActive] bit NOT NULL,
    [IsTestMode] bit NOT NULL,
    [TransactionFee] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_PaymentGatewaySettings] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [StaffSalary] (
    [StaffSalaryId] int NOT NULL IDENTITY,
    [StaffName] nvarchar(max) NULL,
    [StaffId] int NULL,
    [PaymentDate] datetime2 NULL,
    [PaymentMonth] nvarchar(max) NULL,
    [BasicSalary] decimal(18,2) NULL,
    [FestivalBonus] decimal(18,2) NULL,
    [Allowance] decimal(18,2) NULL,
    [MedicalAllowance] decimal(18,2) NULL,
    [HousingAllowance] decimal(18,2) NULL,
    [TransportationAllowance] decimal(18,2) NULL,
    [SavingFund] decimal(18,2) NULL,
    [Taxes] decimal(18,2) NULL,
    [NetSalary] AS ([BasicSalary] + [FestivalBonus] + [Allowance] + [MedicalAllowance] + [HousingAllowance] + [TransportationAllowance] - [SavingFund] - [Taxes]),
    CONSTRAINT [PK_StaffSalary] PRIMARY KEY ([StaffSalaryId])
);
GO


CREATE TABLE [SystemSettings] (
    [Id] int NOT NULL IDENTITY,
    [SettingKey] nvarchar(100) NOT NULL,
    [SettingValue] nvarchar(max) NULL,
    [Category] nvarchar(200) NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_SystemSettings] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [UserMessages] (
    [Id] int NOT NULL IDENTITY,
    [SenderId] nvarchar(max) NOT NULL,
    [ReceiverId] nvarchar(max) NOT NULL,
    [Subject] nvarchar(500) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [IsRead] bit NOT NULL,
    [IsStarred] bit NOT NULL,
    [IsDeletedIn] bit NOT NULL,
    [IsDeletedOut] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_UserMessages] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([UserId], [LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AcademicYear] (
    [AcademicYearId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [CampusId] int NULL,
    CONSTRAINT [PK_AcademicYear] PRIMARY KEY ([AcademicYearId]),
    CONSTRAINT [FK_AcademicYear_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [BankAccounts] (
    [BankAccountId] int NOT NULL IDENTITY,
    [CampusId] int NULL,
    [AccountName] nvarchar(100) NOT NULL,
    [AccountNumber] nvarchar(50) NOT NULL,
    [BankName] nvarchar(100) NOT NULL,
    [AccountType] nvarchar(50) NOT NULL,
    [Balance] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_BankAccounts] PRIMARY KEY ([BankAccountId]),
    CONSTRAINT [FK_BankAccounts_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [Department] (
    [DepartmentId] int NOT NULL IDENTITY,
    [CampusId] int NULL,
    [DepartmentName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY ([DepartmentId]),
    CONSTRAINT [FK_Department_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [GeneralExpense] (
    [Id] int NOT NULL IDENTITY,
    [CampusId] int NULL,
    [Date] datetime2 NOT NULL,
    [ExpenseType] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentMethod] nvarchar(max) NOT NULL,
    [PaidTo] nvarchar(max) NOT NULL,
    [ApprovedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_GeneralExpense] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GeneralExpense_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [GeneralIncome] (
    [Id] int NOT NULL IDENTITY,
    [CampusId] int NULL,
    [Date] datetime2 NOT NULL,
    [Source] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentMethod] nvarchar(max) NOT NULL,
    [ReceivedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_GeneralIncome] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GeneralIncome_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [Parents] (
    [ParentId] int NOT NULL IDENTITY,
    [ParentName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [UserId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CampusId] int NULL,
    CONSTRAINT [PK_Parents] PRIMARY KEY ([ParentId]),
    CONSTRAINT [FK_Parents_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Parents_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [Standard] (
    [StandardId] int NOT NULL IDENTITY,
    [StandardName] nvarchar(max) NOT NULL,
    [CampusId] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Standard] PRIMARY KEY ([StandardId]),
    CONSTRAINT [FK_Standard_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [ExamSchedule] (
    [ExamScheduleId] int NOT NULL IDENTITY,
    [ExamScheduleName] nvarchar(max) NOT NULL,
    [StartDate] datetime2 NULL,
    [EndDate] datetime2 NULL,
    [ExamYear] nvarchar(max) NULL,
    [AcademicYearId] int NULL,
    [CampusId] int NULL,
    CONSTRAINT [PK_ExamSchedule] PRIMARY KEY ([ExamScheduleId]),
    CONSTRAINT [FK_ExamSchedule_AcademicYear_AcademicYearId] FOREIGN KEY ([AcademicYearId]) REFERENCES [AcademicYear] ([AcademicYearId]),
    CONSTRAINT [FK_ExamSchedule_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId])
);
GO


CREATE TABLE [Staff] (
    [StaffId] int NOT NULL IDENTITY,
    [StaffName] nvarchar(max) NOT NULL,
    [ImagePath] nvarchar(max) NULL,
    [UniqueStaffAttendanceNumber] int NOT NULL,
    [Gender] int NULL,
    [DOB] datetime2 NULL,
    [FatherName] nvarchar(max) NULL,
    [MotherName] nvarchar(max) NULL,
    [CNIC] nvarchar(max) NULL,
    [Experience] nvarchar(max) NULL,
    [TemporaryAddress] nvarchar(max) NULL,
    [PermanentAddress] nvarchar(max) NULL,
    [ContactNumber1] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Qualifications] nvarchar(max) NULL,
    [JoiningDate] datetime2 NULL,
    [Designation] int NULL,
    [BankAccountName] nvarchar(max) NULL,
    [BankAccountNumber] int NULL,
    [BankName] nvarchar(max) NULL,
    [BankBranch] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [DepartmentId] int NULL,
    [StaffSalaryId] int NULL,
    [CampusId] int NULL,
    CONSTRAINT [PK_Staff] PRIMARY KEY ([StaffId]),
    CONSTRAINT [FK_Staff_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId]),
    CONSTRAINT [FK_Staff_Department_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Department] ([DepartmentId]),
    CONSTRAINT [FK_Staff_StaffSalary_StaffSalaryId] FOREIGN KEY ([StaffSalaryId]) REFERENCES [StaffSalary] ([StaffSalaryId])
);
GO


CREATE TABLE [Subject] (
    [SubjectId] int NOT NULL IDENTITY,
    [SubjectName] nvarchar(max) NULL,
    [SubjectCode] int NULL,
    [StandardId] int NULL,
    CONSTRAINT [PK_Subject] PRIMARY KEY ([SubjectId]),
    CONSTRAINT [FK_Subject_Standard_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Standard] ([StandardId])
);
GO


CREATE TABLE [ExamScheduleStandard] (
    [ExamScheduleStandardId] int NOT NULL IDENTITY,
    [ExamScheduleId] int NULL,
    [StandardId] int NOT NULL,
    CONSTRAINT [PK_ExamScheduleStandard] PRIMARY KEY ([ExamScheduleStandardId]),
    CONSTRAINT [FK_ExamScheduleStandard_ExamSchedule_ExamScheduleId] FOREIGN KEY ([ExamScheduleId]) REFERENCES [ExamSchedule] ([ExamScheduleId]),
    CONSTRAINT [FK_ExamScheduleStandard_Standard_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Standard] ([StandardId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Leaves] (
    [LeaveId] int NOT NULL IDENTITY,
    [StaffId] int NOT NULL,
    [LeaveType] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Reason] nvarchar(500) NOT NULL,
    [Status] int NOT NULL,
    [AppliedDate] datetime2 NOT NULL,
    [ApprovedByStaffId] int NULL,
    [AdminRemarks] nvarchar(max) NULL,
    CONSTRAINT [PK_Leaves] PRIMARY KEY ([LeaveId]),
    CONSTRAINT [FK_Leaves_Staff_ApprovedByStaffId] FOREIGN KEY ([ApprovedByStaffId]) REFERENCES [Staff] ([StaffId]),
    CONSTRAINT [FK_Leaves_Staff_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staff] ([StaffId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Section] (
    [SectionId] int NOT NULL IDENTITY,
    [SectionName] nvarchar(max) NOT NULL,
    [ClassName] nvarchar(max) NOT NULL,
    [SectionCode] nvarchar(max) NOT NULL,
    [StaffId] int NULL,
    [CampusId] int NULL,
    [RoomNo] nvarchar(max) NOT NULL,
    [Capacity] int NOT NULL,
    [StandardId] int NULL,
    CONSTRAINT [PK_Section] PRIMARY KEY ([SectionId]),
    CONSTRAINT [FK_Section_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId]),
    CONSTRAINT [FK_Section_Staff_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staff] ([StaffId]),
    CONSTRAINT [FK_Section_Standard_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Standard] ([StandardId])
);
GO


CREATE TABLE [StaffExperience] (
    [StaffExperienceId] int NOT NULL IDENTITY,
    [CompanyName] nvarchar(max) NULL,
    [Designation] nvarchar(max) NULL,
    [JoiningDate] datetime2 NOT NULL,
    [LeavingDate] datetime2 NULL,
    [Responsibilities] nvarchar(max) NULL,
    [Achievements] nvarchar(max) NULL,
    [StaffId] int NULL,
    CONSTRAINT [PK_StaffExperience] PRIMARY KEY ([StaffExperienceId]),
    CONSTRAINT [FK_StaffExperience_Staff_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staff] ([StaffId])
);
GO


CREATE TABLE [MarkEntry] (
    [MarkEntryId] int NOT NULL IDENTITY,
    [MarkEntryDate] datetime2 NULL,
    [StaffId] int NOT NULL,
    [ExamScheduleId] int NOT NULL,
    [ExamTypeId] int NOT NULL,
    [SubjectId] int NOT NULL,
    [StandardId] int NOT NULL,
    [TotalMarks] int NULL,
    [PassMarks] int NULL,
    CONSTRAINT [PK_MarkEntry] PRIMARY KEY ([MarkEntryId]),
    CONSTRAINT [FK_MarkEntry_ExamSchedule_ExamScheduleId] FOREIGN KEY ([ExamScheduleId]) REFERENCES [ExamSchedule] ([ExamScheduleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MarkEntry_ExamType_ExamTypeId] FOREIGN KEY ([ExamTypeId]) REFERENCES [ExamType] ([ExamTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MarkEntry_Staff_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staff] ([StaffId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MarkEntry_Standard_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Standard] ([StandardId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MarkEntry_Subject_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subject] ([SubjectId]) ON DELETE CASCADE
);
GO


CREATE TABLE [ExamSubject] (
    [ExamSubjectId] int NOT NULL IDENTITY,
    [ExamScheduleStandardId] int NULL,
    [SubjectId] int NOT NULL,
    [ExamTypeId] int NOT NULL,
    [ExamDate] datetime2 NULL,
    [ExamStartTime] datetime2 NULL,
    [ExamEndTime] datetime2 NULL,
    CONSTRAINT [PK_ExamSubject] PRIMARY KEY ([ExamSubjectId]),
    CONSTRAINT [FK_ExamSubject_ExamScheduleStandard_ExamScheduleStandardId] FOREIGN KEY ([ExamScheduleStandardId]) REFERENCES [ExamScheduleStandard] ([ExamScheduleStandardId]),
    CONSTRAINT [FK_ExamSubject_ExamType_ExamTypeId] FOREIGN KEY ([ExamTypeId]) REFERENCES [ExamType] ([ExamTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExamSubject_Subject_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subject] ([SubjectId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Student] (
    [StudentId] int NOT NULL IDENTITY,
    [AdmissionNo] int NULL,
    [EnrollmentNo] int NULL,
    [UniqueStudentAttendanceNumber] int NOT NULL,
    [StudentName] nvarchar(max) NULL,
    [ImagePath] nvarchar(max) NULL,
    [StudentDOB] datetime2 NOT NULL,
    [StudentGender] int NULL,
    [StudentReligion] nvarchar(max) NULL,
    [StudentBloodGroup] nvarchar(max) NULL,
    [StudentNationality] nvarchar(max) NULL,
    [StudentNIDNumber] nvarchar(max) NULL,
    [StudentContactNumber1] nvarchar(max) NULL,
    [StudentContactNumber2] nvarchar(max) NULL,
    [StudentEmail] nvarchar(max) NULL,
    [ParentEmail] nvarchar(max) NULL,
    [PermanentAddress] nvarchar(max) NULL,
    [TemporaryAddress] nvarchar(max) NULL,
    [FatherName] nvarchar(max) NULL,
    [FatherNID] nvarchar(max) NULL,
    [FatherContactNumber] nvarchar(max) NULL,
    [MotherName] nvarchar(max) NULL,
    [MotherNID] nvarchar(max) NULL,
    [MotherContactNumber] nvarchar(max) NULL,
    [LocalGuardianName] nvarchar(max) NULL,
    [LocalGuardianContactNumber] nvarchar(max) NULL,
    [StandardId] int NULL,
    [Section] nvarchar(max) NULL,
    [GuardianPhone] nvarchar(max) NULL,
    [PreviousSchool] nvarchar(max) NULL,
    [DefaultDiscount] decimal(18,2) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [AdmissionDate] datetime2 NULL,
    [Status] nvarchar(max) NULL,
    [SectionId] int NULL,
    [AcademicYearId] int NULL,
    [CampusId] int NULL,
    [ParentId] int NULL,
    [UserId] nvarchar(450) NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY ([StudentId]),
    CONSTRAINT [FK_Student_AcademicYear_AcademicYearId] FOREIGN KEY ([AcademicYearId]) REFERENCES [AcademicYear] ([AcademicYearId]),
    CONSTRAINT [FK_Student_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Student_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId]),
    CONSTRAINT [FK_Student_Parents_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Parents] ([ParentId]),
    CONSTRAINT [FK_Student_Section_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Section] ([SectionId]),
    CONSTRAINT [FK_Student_Standard_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Standard] ([StandardId])
);
GO


CREATE TABLE [SubjectAssignment] (
    [SubjectAssignmentId] int NOT NULL IDENTITY,
    [StaffId] int NOT NULL,
    [SubjectId] int NOT NULL,
    [SectionId] int NOT NULL,
    CONSTRAINT [PK_SubjectAssignment] PRIMARY KEY ([SubjectAssignmentId]),
    CONSTRAINT [FK_SubjectAssignment_Section_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Section] ([SectionId]),
    CONSTRAINT [FK_SubjectAssignment_Staff_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staff] ([StaffId]),
    CONSTRAINT [FK_SubjectAssignment_Subject_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subject] ([SubjectId])
);
GO


CREATE TABLE [Mark] (
    [MarkId] int NOT NULL IDENTITY,
    [TotalMarks] int NOT NULL,
    [PassMarks] int NOT NULL,
    [ObtainedScore] int NOT NULL,
    [Grade] int NOT NULL,
    [PassStatus] int NOT NULL,
    [MarkEntryDate] datetime2 NULL,
    [Feedback] nvarchar(max) NULL,
    [StaffId] int NOT NULL,
    [StudentId] int NOT NULL,
    [SubjectId] int NOT NULL,
    CONSTRAINT [PK_Mark] PRIMARY KEY ([MarkId]),
    CONSTRAINT [FK_Mark_Staff_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staff] ([StaffId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mark_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Mark_Subject_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subject] ([SubjectId])
);
GO


CREATE TABLE [MonthlyPayment] (
    [MonthlyPaymentId] int NOT NULL IDENTITY,
    [CampusId] int NULL,
    [StudentId] int NULL,
    [TotalFeeAmount] decimal(18,2) NULL,
    [Waver] decimal(18,2) NULL,
    [PreviousDue] decimal(18,2) NULL,
    [TotalAmount] decimal(18,2) NULL,
    [AmountPaid] decimal(18,2) NULL,
    [AmountRemaining] decimal(18,2) NULL,
    [PaymentDate] datetime2 NOT NULL,
    CONSTRAINT [PK_MonthlyPayment] PRIMARY KEY ([MonthlyPaymentId]),
    CONSTRAINT [FK_MonthlyPayment_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId]),
    CONSTRAINT [FK_MonthlyPayment_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId])
);
GO


CREATE TABLE [OthersPayment] (
    [OthersPaymentId] int NOT NULL IDENTITY,
    [CampusId] int NULL,
    [StudentId] int NULL,
    [TotalAmount] decimal(18,2) NULL,
    [AmountPaid] decimal(18,2) NULL,
    [AmountRemaining] decimal(18,2) NULL,
    [Waver] decimal(18,2) NULL,
    [PaymentDate] datetime2 NOT NULL,
    CONSTRAINT [PK_OthersPayment] PRIMARY KEY ([OthersPaymentId]),
    CONSTRAINT [FK_OthersPayment_Campus_CampusId] FOREIGN KEY ([CampusId]) REFERENCES [Campus] ([CampusId]),
    CONSTRAINT [FK_OthersPayment_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId])
);
GO


CREATE TABLE [StudentMarksDetails] (
    [MarkEntryId] int NOT NULL,
    [StudentId] int NOT NULL,
    [StudentName] nvarchar(max) NULL,
    [ObtainedScore] int NULL,
    [Grade] nvarchar(max) NULL,
    [PassStatus] int NULL,
    [Feedback] nvarchar(max) NULL,
    CONSTRAINT [PK_StudentMarksDetails] PRIMARY KEY ([StudentId], [MarkEntryId]),
    CONSTRAINT [FK_StudentMarksDetails_MarkEntry_MarkEntryId] FOREIGN KEY ([MarkEntryId]) REFERENCES [MarkEntry] ([MarkEntryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_StudentMarksDetails_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId]) ON DELETE CASCADE
);
GO


CREATE TABLE [DueBalance] (
    [DueBalanceId] int NOT NULL IDENTITY,
    [StudentId] int NULL,
    [DueBalanceAmount] decimal(18,2) NULL,
    [LastUpdate] datetime2 NULL,
    [MonthlyPaymentId] int NULL,
    CONSTRAINT [PK_DueBalance] PRIMARY KEY ([DueBalanceId]),
    CONSTRAINT [FK_DueBalance_MonthlyPayment_MonthlyPaymentId] FOREIGN KEY ([MonthlyPaymentId]) REFERENCES [MonthlyPayment] ([MonthlyPaymentId]),
    CONSTRAINT [FK_DueBalance_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([StudentId])
);
GO


CREATE TABLE [PaymentDetail] (
    [PaymentDetailId] int NOT NULL IDENTITY,
    [MonthlyPaymentId] int NOT NULL,
    [FeeName] nvarchar(max) NOT NULL,
    [FeeAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_PaymentDetail] PRIMARY KEY ([PaymentDetailId]),
    CONSTRAINT [FK_PaymentDetail_MonthlyPayment_MonthlyPaymentId] FOREIGN KEY ([MonthlyPaymentId]) REFERENCES [MonthlyPayment] ([MonthlyPaymentId]) ON DELETE CASCADE
);
GO


CREATE TABLE [PaymentMonth] (
    [PaymentMonthId] int NOT NULL IDENTITY,
    [MonthlyPaymentId] int NOT NULL,
    [MonthName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PaymentMonth] PRIMARY KEY ([PaymentMonthId]),
    CONSTRAINT [FK_PaymentMonth_MonthlyPayment_MonthlyPaymentId] FOREIGN KEY ([MonthlyPaymentId]) REFERENCES [MonthlyPayment] ([MonthlyPaymentId]) ON DELETE CASCADE
);
GO


CREATE TABLE [AcademicMonth] (
    [MonthId] int NOT NULL IDENTITY,
    [MonthName] nvarchar(max) NULL,
    [MonthlyPaymentId] int NULL,
    [OthersPaymentId] int NULL,
    CONSTRAINT [PK_AcademicMonth] PRIMARY KEY ([MonthId]),
    CONSTRAINT [FK_AcademicMonth_MonthlyPayment_MonthlyPaymentId] FOREIGN KEY ([MonthlyPaymentId]) REFERENCES [MonthlyPayment] ([MonthlyPaymentId]),
    CONSTRAINT [FK_AcademicMonth_OthersPayment_OthersPaymentId] FOREIGN KEY ([OthersPaymentId]) REFERENCES [OthersPayment] ([OthersPaymentId])
);
GO


CREATE TABLE [Fee] (
    [FeeId] int NOT NULL IDENTITY,
    [FeeTypeId] int NOT NULL,
    [StandardId] int NOT NULL,
    [PaymentFrequency] int NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [MonthlyPaymentId] int NULL,
    [OthersPaymentId] int NULL,
    CONSTRAINT [PK_Fee] PRIMARY KEY ([FeeId]),
    CONSTRAINT [FK_Fee_FeeType_FeeTypeId] FOREIGN KEY ([FeeTypeId]) REFERENCES [FeeType] ([FeeTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Fee_MonthlyPayment_MonthlyPaymentId] FOREIGN KEY ([MonthlyPaymentId]) REFERENCES [MonthlyPayment] ([MonthlyPaymentId]),
    CONSTRAINT [FK_Fee_OthersPayment_OthersPaymentId] FOREIGN KEY ([OthersPaymentId]) REFERENCES [OthersPayment] ([OthersPaymentId]),
    CONSTRAINT [FK_Fee_Standard_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Standard] ([StandardId]) ON DELETE CASCADE
);
GO


CREATE TABLE [OtherPaymentDetail] (
    [PaymentDetailId] int NOT NULL IDENTITY,
    [OthersPaymentId] int NOT NULL,
    [FeeName] nvarchar(max) NOT NULL,
    [FeeAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OtherPaymentDetail] PRIMARY KEY ([PaymentDetailId]),
    CONSTRAINT [FK_OtherPaymentDetail_OthersPayment_OthersPaymentId] FOREIGN KEY ([OthersPaymentId]) REFERENCES [OthersPayment] ([OthersPaymentId]) ON DELETE CASCADE
);
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MonthId', N'MonthName', N'MonthlyPaymentId', N'OthersPaymentId') AND [object_id] = OBJECT_ID(N'[AcademicMonth]'))
    SET IDENTITY_INSERT [AcademicMonth] ON;
INSERT INTO [AcademicMonth] ([MonthId], [MonthName], [MonthlyPaymentId], [OthersPaymentId])
VALUES (1, N'January', NULL, NULL),
(2, N'February', NULL, NULL),
(3, N'March', NULL, NULL),
(4, N'April', NULL, NULL),
(5, N'May', NULL, NULL),
(6, N'June', NULL, NULL),
(7, N'July', NULL, NULL),
(8, N'August', NULL, NULL),
(9, N'September', NULL, NULL),
(10, N'October', NULL, NULL),
(11, N'November', NULL, NULL),
(12, N'December', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MonthId', N'MonthName', N'MonthlyPaymentId', N'OthersPaymentId') AND [object_id] = OBJECT_ID(N'[AcademicMonth]'))
    SET IDENTITY_INSERT [AcademicMonth] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CampusId', N'Address', N'CampusCode', N'CampusName', N'ContactNumber', N'CreatedAt', N'Email', N'IsActive') AND [object_id] = OBJECT_ID(N'[Campus]'))
    SET IDENTITY_INSERT [Campus] ON;
INSERT INTO [Campus] ([CampusId], [Address], [CampusCode], [CampusName], [ContactNumber], [CreatedAt], [Email], [IsActive])
VALUES (1, NULL, N'MAIN', N'Main Campus', NULL, '2026-04-22T15:32:05.4094757+05:00', NULL, CAST(1 AS bit));
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CampusId', N'Address', N'CampusCode', N'CampusName', N'ContactNumber', N'CreatedAt', N'Email', N'IsActive') AND [object_id] = OBJECT_ID(N'[Campus]'))
    SET IDENTITY_INSERT [Campus] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ExamScheduleId', N'AcademicYearId', N'CampusId', N'EndDate', N'ExamScheduleName', N'ExamYear', N'StartDate') AND [object_id] = OBJECT_ID(N'[ExamSchedule]'))
    SET IDENTITY_INSERT [ExamSchedule] ON;
INSERT INTO [ExamSchedule] ([ExamScheduleId], [AcademicYearId], [CampusId], [EndDate], [ExamScheduleName], [ExamYear], [StartDate])
VALUES (1, NULL, NULL, NULL, N'First Semester', NULL, NULL),
(2, NULL, NULL, NULL, N'Second Semester', NULL, NULL),
(3, NULL, NULL, NULL, N'Third Semester', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ExamScheduleId', N'AcademicYearId', N'CampusId', N'EndDate', N'ExamScheduleName', N'ExamYear', N'StartDate') AND [object_id] = OBJECT_ID(N'[ExamSchedule]'))
    SET IDENTITY_INSERT [ExamSchedule] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ExamTypeId', N'ExamTypeName') AND [object_id] = OBJECT_ID(N'[ExamType]'))
    SET IDENTITY_INSERT [ExamType] ON;
INSERT INTO [ExamType] ([ExamTypeId], [ExamTypeName])
VALUES (1, N'Midterm'),
(2, N'Final'),
(3, N'Practical'),
(4, N'Monthly Exam'),
(5, N'Lab Exam');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ExamTypeId', N'ExamTypeName') AND [object_id] = OBJECT_ID(N'[ExamType]'))
    SET IDENTITY_INSERT [ExamType] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'FeeTypeId', N'TypeName') AND [object_id] = OBJECT_ID(N'[FeeType]'))
    SET IDENTITY_INSERT [FeeType] ON;
INSERT INTO [FeeType] ([FeeTypeId], [TypeName])
VALUES (1, N'Registration Fee'),
(2, N'Tuition Fee'),
(3, N'Library Fee'),
(4, N'Examination Fee'),
(5, N'Sports Fee'),
(6, N'Transportation Fee');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'FeeTypeId', N'TypeName') AND [object_id] = OBJECT_ID(N'[FeeType]'))
    SET IDENTITY_INSERT [FeeType] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StaffExperienceId', N'Achievements', N'CompanyName', N'Designation', N'JoiningDate', N'LeavingDate', N'Responsibilities', N'StaffId') AND [object_id] = OBJECT_ID(N'[StaffExperience]'))
    SET IDENTITY_INSERT [StaffExperience] ON;
INSERT INTO [StaffExperience] ([StaffExperienceId], [Achievements], [CompanyName], [Designation], [JoiningDate], [LeavingDate], [Responsibilities], [StaffId])
VALUES (1, N'Received Employee of the Month award.', N'ABC Company', N'Software Engineer', '2020-05-10T00:00:00.0000000', '2022-08-15T00:00:00.0000000', N'Developed web applications.', NULL),
(2, N'Implemented a new data visualization system.', N'XYZ Corporation', N'Data Analyst', '2018-09-20T00:00:00.0000000', '2020-04-30T00:00:00.0000000', N'Analyzed data to provide insights.', NULL),
(3, N'Successfully delivered multiple projects on time.', N'EFG Ltd.', N'Project Manager', '2016-03-05T00:00:00.0000000', '2018-07-25T00:00:00.0000000', N'Led a team of developers.', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StaffExperienceId', N'Achievements', N'CompanyName', N'Designation', N'JoiningDate', N'LeavingDate', N'Responsibilities', N'StaffId') AND [object_id] = OBJECT_ID(N'[StaffExperience]'))
    SET IDENTITY_INSERT [StaffExperience] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StaffSalaryId', N'Allowance', N'BasicSalary', N'FestivalBonus', N'HousingAllowance', N'MedicalAllowance', N'PaymentDate', N'PaymentMonth', N'SavingFund', N'StaffId', N'StaffName', N'Taxes', N'TransportationAllowance') AND [object_id] = OBJECT_ID(N'[StaffSalary]'))
    SET IDENTITY_INSERT [StaffSalary] ON;
INSERT INTO [StaffSalary] ([StaffSalaryId], [Allowance], [BasicSalary], [FestivalBonus], [HousingAllowance], [MedicalAllowance], [PaymentDate], [PaymentMonth], [SavingFund], [StaffId], [StaffName], [Taxes], [TransportationAllowance])
VALUES (1, 500.0, 5000.0, 1000.0, 800.0, 300.0, '2026-04-22T15:32:05.4097237+05:00', NULL, 200.0, NULL, N'Jamir King', 500.0, 200.0),
(2, 500.0, 5000.0, 1000.0, 800.0, 300.0, '2026-04-22T15:32:05.4097255+05:00', NULL, 200.0, NULL, N'Jamir Jamidar', 500.0, 200.0),
(3, 500.0, 5000.0, 1000.0, 800.0, 300.0, '2026-04-22T15:32:05.4097265+05:00', NULL, 200.0, NULL, N'Jamir Amir', 500.0, 200.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StaffSalaryId', N'Allowance', N'BasicSalary', N'FestivalBonus', N'HousingAllowance', N'MedicalAllowance', N'PaymentDate', N'PaymentMonth', N'SavingFund', N'StaffId', N'StaffName', N'Taxes', N'TransportationAllowance') AND [object_id] = OBJECT_ID(N'[StaffSalary]'))
    SET IDENTITY_INSERT [StaffSalary] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StandardId', N'CampusId', N'CreatedAt', N'StandardName') AND [object_id] = OBJECT_ID(N'[Standard]'))
    SET IDENTITY_INSERT [Standard] ON;
INSERT INTO [Standard] ([StandardId], [CampusId], [CreatedAt], [StandardName])
VALUES (1, NULL, '2026-04-22T15:32:05.4097369+05:00', N'Class One'),
(2, NULL, '2026-04-22T15:32:05.4097374+05:00', N'Class Two'),
(3, NULL, '2026-04-22T15:32:05.4097378+05:00', N'Class Three'),
(4, NULL, '2026-04-22T15:32:05.4097382+05:00', N'Class Four'),
(5, NULL, '2026-04-22T15:32:05.4097385+05:00', N'Class Five'),
(6, NULL, '2026-04-22T15:32:05.4097389+05:00', N'Class Six'),
(7, NULL, '2026-04-22T15:32:05.4097395+05:00', N'Class Seven'),
(8, NULL, '2026-04-22T15:32:05.4097399+05:00', N'Class Eight'),
(9, NULL, '2026-04-22T15:32:05.4097425+05:00', N'Class Nine'),
(10, NULL, '2026-04-22T15:32:05.4097429+05:00', N'Class Ten');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StandardId', N'CampusId', N'CreatedAt', N'StandardName') AND [object_id] = OBJECT_ID(N'[Standard]'))
    SET IDENTITY_INSERT [Standard] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AcademicYearId', N'CampusId', N'Name') AND [object_id] = OBJECT_ID(N'[AcademicYear]'))
    SET IDENTITY_INSERT [AcademicYear] ON;
INSERT INTO [AcademicYear] ([AcademicYearId], [CampusId], [Name])
VALUES (1, 1, N'2000'),
(2, 1, N'2001'),
(3, 1, N'2002'),
(4, 1, N'2003'),
(5, 1, N'2004'),
(6, 1, N'2005'),
(7, 1, N'2006'),
(8, 1, N'2007'),
(9, 1, N'2008'),
(10, 1, N'2009'),
(11, 1, N'2010'),
(12, 1, N'2011'),
(13, 1, N'2012'),
(14, 1, N'2013'),
(15, 1, N'2014'),
(16, 1, N'2015'),
(17, 1, N'2016'),
(18, 1, N'2017'),
(19, 1, N'2018'),
(20, 1, N'2019'),
(21, 1, N'2020'),
(22, 1, N'2021'),
(23, 1, N'2022'),
(24, 1, N'2023'),
(25, 1, N'2024'),
(26, 1, N'2025'),
(27, 1, N'2026'),
(28, 1, N'2027'),
(29, 1, N'2028'),
(30, 1, N'2029'),
(31, 1, N'2030'),
(32, 1, N'2031'),
(33, 1, N'2032'),
(34, 1, N'2033'),
(35, 1, N'2034'),
(36, 1, N'2035'),
(37, 1, N'2036'),
(38, 1, N'2037'),
(39, 1, N'2038'),
(40, 1, N'2039'),
(41, 1, N'2040'),
(42, 1, N'2041');
INSERT INTO [AcademicYear] ([AcademicYearId], [CampusId], [Name])
VALUES (43, 1, N'2042'),
(44, 1, N'2043'),
(45, 1, N'2044'),
(46, 1, N'2045'),
(47, 1, N'2046'),
(48, 1, N'2047'),
(49, 1, N'2048'),
(50, 1, N'2049'),
(51, 1, N'2050');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AcademicYearId', N'CampusId', N'Name') AND [object_id] = OBJECT_ID(N'[AcademicYear]'))
    SET IDENTITY_INSERT [AcademicYear] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DepartmentId', N'CampusId', N'DepartmentName') AND [object_id] = OBJECT_ID(N'[Department]'))
    SET IDENTITY_INSERT [Department] ON;
INSERT INTO [Department] ([DepartmentId], [CampusId], [DepartmentName])
VALUES (1, 1, N'Teacher'),
(2, 1, N'Account'),
(3, 1, N'Administration'),
(4, 1, N'Student Affairs'),
(5, 1, N'Counseling'),
(6, 1, N'Sports'),
(7, 1, N'Library'),
(8, 1, N'Maintenance');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DepartmentId', N'CampusId', N'DepartmentName') AND [object_id] = OBJECT_ID(N'[Department]'))
    SET IDENTITY_INSERT [Department] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'FeeId', N'Amount', N'DueDate', N'FeeTypeId', N'MonthlyPaymentId', N'OthersPaymentId', N'PaymentFrequency', N'StandardId') AND [object_id] = OBJECT_ID(N'[Fee]'))
    SET IDENTITY_INSERT [Fee] ON;
INSERT INTO [Fee] ([FeeId], [Amount], [DueDate], [FeeTypeId], [MonthlyPaymentId], [OthersPaymentId], [PaymentFrequency], [StandardId])
VALUES (1, 1500.0, '2024-05-01T00:00:00.0000000', 1, NULL, NULL, 1, 1),
(2, 500.0, '2024-05-05T00:00:00.0000000', 2, NULL, NULL, 0, 1),
(3, 200.0, '2024-05-10T00:00:00.0000000', 3, NULL, NULL, 0, 1),
(4, 100.0, '2024-05-15T00:00:00.0000000', 6, NULL, NULL, 0, 1),
(5, 250.0, '2024-05-20T00:00:00.0000000', 5, NULL, NULL, 5, 1),
(6, 300.0, '2024-05-25T00:00:00.0000000', 4, NULL, NULL, 5, 1),
(7, 1500.0, '2024-06-01T00:00:00.0000000', 1, NULL, NULL, 1, 2),
(8, 500.0, '2024-06-05T00:00:00.0000000', 2, NULL, NULL, 0, 2),
(9, 200.0, '2024-06-10T00:00:00.0000000', 3, NULL, NULL, 0, 2),
(10, 100.0, '2024-06-15T00:00:00.0000000', 6, NULL, NULL, 0, 2),
(11, 250.0, '2024-06-20T00:00:00.0000000', 5, NULL, NULL, 5, 2),
(12, 300.0, '2024-06-25T00:00:00.0000000', 4, NULL, NULL, 5, 2),
(13, 1500.0, '2024-07-01T00:00:00.0000000', 1, NULL, NULL, 1, 3),
(14, 500.0, '2024-07-05T00:00:00.0000000', 2, NULL, NULL, 0, 3),
(15, 200.0, '2024-07-10T00:00:00.0000000', 3, NULL, NULL, 0, 3),
(16, 100.0, '2024-07-15T00:00:00.0000000', 6, NULL, NULL, 0, 3),
(17, 250.0, '2024-07-20T00:00:00.0000000', 5, NULL, NULL, 5, 3),
(18, 300.0, '2024-07-25T00:00:00.0000000', 4, NULL, NULL, 5, 3),
(19, 1500.0, '2024-08-01T00:00:00.0000000', 1, NULL, NULL, 1, 4),
(20, 500.0, '2024-08-05T00:00:00.0000000', 2, NULL, NULL, 0, 4);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'FeeId', N'Amount', N'DueDate', N'FeeTypeId', N'MonthlyPaymentId', N'OthersPaymentId', N'PaymentFrequency', N'StandardId') AND [object_id] = OBJECT_ID(N'[Fee]'))
    SET IDENTITY_INSERT [Fee] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StudentId', N'AcademicYearId', N'AdmissionDate', N'AdmissionNo', N'CampusId', N'CreatedAt', N'DefaultDiscount', N'EnrollmentNo', N'FatherContactNumber', N'FatherNID', N'FatherName', N'GuardianPhone', N'ImagePath', N'LocalGuardianContactNumber', N'LocalGuardianName', N'MotherContactNumber', N'MotherNID', N'MotherName', N'ParentEmail', N'ParentId', N'PermanentAddress', N'PreviousSchool', N'Section', N'SectionId', N'StandardId', N'Status', N'StudentBloodGroup', N'StudentContactNumber1', N'StudentContactNumber2', N'StudentDOB', N'StudentEmail', N'StudentGender', N'StudentNIDNumber', N'StudentName', N'StudentNationality', N'StudentReligion', N'TemporaryAddress', N'UniqueStudentAttendanceNumber', N'UserId') AND [object_id] = OBJECT_ID(N'[Student]'))
    SET IDENTITY_INSERT [Student] ON;
INSERT INTO [Student] ([StudentId], [AcademicYearId], [AdmissionDate], [AdmissionNo], [CampusId], [CreatedAt], [DefaultDiscount], [EnrollmentNo], [FatherContactNumber], [FatherNID], [FatherName], [GuardianPhone], [ImagePath], [LocalGuardianContactNumber], [LocalGuardianName], [MotherContactNumber], [MotherNID], [MotherName], [ParentEmail], [ParentId], [PermanentAddress], [PreviousSchool], [Section], [SectionId], [StandardId], [Status], [StudentBloodGroup], [StudentContactNumber1], [StudentContactNumber2], [StudentDOB], [StudentEmail], [StudentGender], [StudentNIDNumber], [StudentName], [StudentNationality], [StudentReligion], [TemporaryAddress], [UniqueStudentAttendanceNumber], [UserId])
VALUES (1, NULL, NULL, 1000, NULL, '2026-04-22T15:32:05.4097528+05:00', 0.0, 2000, N'9876543210', N'17948678987624322', N'Michael Doe', NULL, NULL, N'9876543230', N'Jane Smith', N'9876543220', N'17948678987754322', N'Alice Doe', NULL, NULL, N'123 Main Street, City, Country', NULL, NULL, NULL, 1, NULL, N'A+', N'1234567890', NULL, '0001-01-01T00:00:00.0000000', N'john.doe@example.com', 0, N'12345678901234567', N'John Doe', N'Bangladeshi', NULL, N'456 Elm Street, City, Country', 1000, NULL),
(2, NULL, NULL, 1001, NULL, '2026-04-22T15:32:05.4097544+05:00', 0.0, 2001, N'9876543220', N'12345678901234567', N'Abdul Rahman', NULL, NULL, N'9876543240', N'Kamal Ahmed', N'9876543230', N'12345678901234568', N'Ayesha Rahman', NULL, NULL, N'Dhaka, Bangladesh', NULL, NULL, NULL, 1, NULL, N'B+', N'9876543210', NULL, '0001-01-01T00:00:00.0000000', N'fatima.rahman@example.com', 1, N'12345678901234567', N'Fatima Rahman', N'Bangladeshi', NULL, N'Dhaka, Bangladesh', 1001, NULL),
(3, NULL, NULL, 1002, NULL, '2026-04-22T15:32:05.4097584+05:00', 0.0, 2002, N'9876543221', N'98765432109876544', N'Rahim Khan', NULL, NULL, N'9876543241', N'Kamal Ahmed', N'9876543231', N'98765432109876545', N'Fatima Khan', NULL, NULL, N'Chittagong, Bangladesh', NULL, NULL, NULL, 1, NULL, N'O+', N'9876543211', NULL, '0001-01-01T00:00:00.0000000', N'aryan.khan@example.com', 0, N'98765432109876543', N'Aryan Khan', N'Bangladeshi', NULL, N'Chittagong, Bangladesh', 1002, NULL),
(4, NULL, NULL, 1003, NULL, '2026-04-22T15:32:05.4097597+05:00', 0.0, 2003, N'9876543222', N'76543210987654322', N'Mahmud Ahmed', NULL, NULL, N'9876543242', N'Nadia Rahman', N'9876543232', N'76543210987654323', N'Farida Ahmed', NULL, NULL, N'Sylhet, Bangladesh', NULL, NULL, NULL, 2, NULL, N'AB+', N'9876543212', NULL, '0001-01-01T00:00:00.0000000', N'tasnim.ahmed@example.com', 1, N'76543210987654321', N'Tasnim Ahmed', N'Bangladeshi', NULL, N'Sylhet, Bangladesh', 1003, NULL),
(5, NULL, NULL, 1004, NULL, '2026-04-22T15:32:05.4097606+05:00', 0.0, 2004, N'9876543223', N'87654321098765433', N'Nasir Khan', NULL, NULL, N'9876543243', N'Abdul Ali', N'9876543233', N'87654321098765434', N'Sadia Khan', NULL, NULL, N'Rajshahi, Bangladesh', NULL, NULL, NULL, 2, NULL, N'A-', N'9876543213', NULL, '0001-01-01T00:00:00.0000000', N'imran.khan@example.com', 0, N'87654321098765432', N'Imran Khan', N'Bangladeshi', NULL, N'Rajshahi, Bangladesh', 1004, NULL),
(6, NULL, NULL, 1005, NULL, '2026-04-22T15:32:05.4097614+05:00', 0.0, 2005, N'9876543224', N'65432109876543211', N'Hasan Rahman', NULL, NULL, N'9876543244', N'Khaled Islam', N'9876543234', N'65432109876543212', N'Sabina Rahman', NULL, NULL, N'Dhaka, Bangladesh', NULL, NULL, NULL, 2, NULL, N'B-', N'9876543214', NULL, '0001-01-01T00:00:00.0000000', N'anika.rahman@example.com', 1, N'65432109876543210', N'Anika Rahman', N'Bangladeshi', NULL, N'Dhaka, Bangladesh', 1005, NULL),
(7, NULL, NULL, 1006, NULL, '2026-04-22T15:32:05.4097622+05:00', 0.0, 2006, N'9876543225', N'54321098765432110', N'Rahman Islam', NULL, NULL, N'9876543245', N'Farid Ahmed', N'9876543235', N'54321098765432111', N'Amina Islam', NULL, NULL, N'Chittagong, Bangladesh', NULL, NULL, NULL, 3, NULL, N'O-', N'9876543215', NULL, '0001-01-01T00:00:00.0000000', N'rafiul.islam@example.com', 0, N'54321098765432109', N'Rafiul Islam', N'Bangladeshi', NULL, N'Chittagong, Bangladesh', 1006, NULL),
(8, NULL, NULL, 1007, NULL, '2026-04-22T15:32:05.4097630+05:00', 0.0, 2007, N'9876543226', N'43210987654321099', N'Akram Khan', NULL, NULL, N'9876543246', N'Ayesha Begum', N'9876543236', N'43210987654321100', N'Taslima Khan', NULL, NULL, N'Rajshahi, Bangladesh', NULL, NULL, NULL, 3, NULL, N'AB-', N'9876543216', NULL, '0001-01-01T00:00:00.0000000', N'zara.khan@example.com', 1, N'43210987654321098', N'Zara Khan', N'Bangladeshi', NULL, N'Rajshahi, Bangladesh', 1007, NULL),
(9, NULL, NULL, 1008, NULL, '2026-04-22T15:32:05.4097666+05:00', 0.0, 2008, N'9876543227', N'32109876543210988', N'Kamal Hossain', NULL, NULL, N'9876543247', N'Salam Ahmed', N'9876543237', N'32109876543210989', N'Nazma Hossain', NULL, NULL, N'Sylhet, Bangladesh', NULL, NULL, NULL, 3, NULL, N'A+', N'9876543217', NULL, '0001-01-01T00:00:00.0000000', N'arif.hossain@example.com', 0, N'32109876543210987', N'Arif Hossain', N'Bangladeshi', NULL, N'Sylhet, Bangladesh', 1008, NULL),
(10, NULL, NULL, 1009, NULL, '2026-04-22T15:32:05.4097693+05:00', 0.0, 2009, N'9876543228', N'21098765432109877', N'Jamil Akter', NULL, NULL, N'9876543248', N'Khaled Rahman', N'9876543238', N'21098765432109878', N'Rina Akter', NULL, NULL, N'Dhaka, Bangladesh', NULL, NULL, NULL, 4, NULL, N'A-', N'9876543218', NULL, '0001-01-01T00:00:00.0000000', N'sabrina.akter@example.com', 1, N'21098765432109876', N'Sabrina Akter', N'Bangladeshi', NULL, N'Dhaka, Bangladesh', 1009, NULL),
(11, NULL, NULL, 1010, NULL, '2026-04-22T15:32:05.4097735+05:00', 0.0, 2010, N'9876543229', N'10987654321098766', N'Hasan Mahmud', NULL, NULL, N'9876543249', N'Farhana Akter', N'9876543239', N'10987654321098767', N'Nazma Hasan', NULL, NULL, N'Chittagong, Bangladesh', NULL, NULL, NULL, 4, NULL, N'O-', N'9876543219', NULL, '0001-01-01T00:00:00.0000000', N'rahat.hasan@example.com', 0, N'10987654321098765', N'Rahat Hasan', N'Bangladeshi', NULL, N'Chittagong, Bangladesh', 1010, NULL),
(12, NULL, NULL, 1011, NULL, '2026-04-22T15:32:05.4097744+05:00', 0.0, 2011, N'9876543230', N'09876543210987655', N'Rahim Rahman', NULL, NULL, N'9876543250', N'Kamal Hossain', N'9876543240', N'09876543210987656', N'Sara Rahman', NULL, NULL, N'Rajshahi, Bangladesh', NULL, NULL, NULL, 4, NULL, N'AB-', N'9876543220', NULL, '0001-01-01T00:00:00.0000000', N'asif.rahman@example.com', 0, N'09876543210987654', N'Asif Rahman', N'Bangladeshi', NULL, N'Rajshahi, Bangladesh', 1011, NULL),
(13, NULL, NULL, 1012, NULL, '2026-04-22T15:32:05.4097752+05:00', 0.0, 2012, N'9876543231', N'98765432109876544', N'Akram Khan', NULL, NULL, N'9876543251', N'Ayesha Begum', N'9876543241', N'98765432109876545', N'Taslima Khan', NULL, NULL, N'Sylhet, Bangladesh', NULL, NULL, NULL, 4, NULL, N'A+', N'9876543221', NULL, '0001-01-01T00:00:00.0000000', N'mehnaz.khan@example.com', 1, N'98765432109876543', N'Mehnaz Khan', N'Bangladeshi', NULL, N'Sylhet, Bangladesh', 1012, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StudentId', N'AcademicYearId', N'AdmissionDate', N'AdmissionNo', N'CampusId', N'CreatedAt', N'DefaultDiscount', N'EnrollmentNo', N'FatherContactNumber', N'FatherNID', N'FatherName', N'GuardianPhone', N'ImagePath', N'LocalGuardianContactNumber', N'LocalGuardianName', N'MotherContactNumber', N'MotherNID', N'MotherName', N'ParentEmail', N'ParentId', N'PermanentAddress', N'PreviousSchool', N'Section', N'SectionId', N'StandardId', N'Status', N'StudentBloodGroup', N'StudentContactNumber1', N'StudentContactNumber2', N'StudentDOB', N'StudentEmail', N'StudentGender', N'StudentNIDNumber', N'StudentName', N'StudentNationality', N'StudentReligion', N'TemporaryAddress', N'UniqueStudentAttendanceNumber', N'UserId') AND [object_id] = OBJECT_ID(N'[Student]'))
    SET IDENTITY_INSERT [Student] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SubjectId', N'StandardId', N'SubjectCode', N'SubjectName') AND [object_id] = OBJECT_ID(N'[Subject]'))
    SET IDENTITY_INSERT [Subject] ON;
INSERT INTO [Subject] ([SubjectId], [StandardId], [SubjectCode], [SubjectName])
VALUES (1, 1, 101, N'Mathematics'),
(2, 1, 102, N'Bengali'),
(3, 1, 103, N'Physics'),
(4, 2, 201, N'Mathematics'),
(5, 2, 202, N'Bengali'),
(6, 2, 203, N'Physics'),
(7, 3, 301, N'Mathematics'),
(8, 3, 302, N'Bengali'),
(9, 3, 303, N'Physics'),
(10, 4, 401, N'Mathematics'),
(11, 4, 402, N'Bengali'),
(12, 4, 403, N'Physics');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SubjectId', N'StandardId', N'SubjectCode', N'SubjectName') AND [object_id] = OBJECT_ID(N'[Subject]'))
    SET IDENTITY_INSERT [Subject] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StaffId', N'BankAccountName', N'BankAccountNumber', N'BankBranch', N'BankName', N'CNIC', N'CampusId', N'ContactNumber1', N'DOB', N'DepartmentId', N'Designation', N'Email', N'Experience', N'FatherName', N'Gender', N'ImagePath', N'JoiningDate', N'MotherName', N'PermanentAddress', N'Qualifications', N'StaffName', N'StaffSalaryId', N'Status', N'TemporaryAddress', N'UniqueStaffAttendanceNumber') AND [object_id] = OBJECT_ID(N'[Staff]'))
    SET IDENTITY_INSERT [Staff] ON;
INSERT INTO [Staff] ([StaffId], [BankAccountName], [BankAccountNumber], [BankBranch], [BankName], [CNIC], [CampusId], [ContactNumber1], [DOB], [DepartmentId], [Designation], [Email], [Experience], [FatherName], [Gender], [ImagePath], [JoiningDate], [MotherName], [PermanentAddress], [Qualifications], [StaffName], [StaffSalaryId], [Status], [TemporaryAddress], [UniqueStaffAttendanceNumber])
VALUES (1, N'John Doe', 1234567890, N'XYZ Branch', N'ABC Bank', NULL, NULL, N'1234567890', '1985-05-15T00:00:00.0000000', 1, 3, N'john.doe@example.com', NULL, N'Michael Doe', 0, NULL, '2010-07-01T00:00:00.0000000', N'Alice Doe', N'Permanent Address', N'Bachelor''s in Computer Science', N'Jamir King', 1, N'Active', N'Temporary Address', 201),
(2, N'Alice Smith', 9873210, N'UVW Branch', N'DEF Bank', NULL, NULL, N'9876543210', '1990-08-20T00:00:00.0000000', 2, 2, N'alice.smith@example.com', NULL, N'David Smith', 1, NULL, '2015-09-15T00:00:00.0000000', N'Emily Smith', N'Permanent Address', N'Master''s in Education', N'Jamir Jamidar', 2, N'Active', N'Temporary Address', 202),
(3, N'John Doe', 1234567890, N'Main Street', N'Anytown Bank', NULL, NULL, N'555-123-4567', '1980-01-01T00:00:00.0000000', 3, 0, N'john.doe@example.com', NULL, N'Richard Doe', 0, NULL, '2020-08-15T00:00:00.0000000', N'Jane Doe', N'456 Elm Street, Anytown', N'Bachelor of Science in Mathematics', N'Jamir Amir', 3, N'Active', N'123 Main Street, Anytown', 203);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'StaffId', N'BankAccountName', N'BankAccountNumber', N'BankBranch', N'BankName', N'CNIC', N'CampusId', N'ContactNumber1', N'DOB', N'DepartmentId', N'Designation', N'Email', N'Experience', N'FatherName', N'Gender', N'ImagePath', N'JoiningDate', N'MotherName', N'PermanentAddress', N'Qualifications', N'StaffName', N'StaffSalaryId', N'Status', N'TemporaryAddress', N'UniqueStaffAttendanceNumber') AND [object_id] = OBJECT_ID(N'[Staff]'))
    SET IDENTITY_INSERT [Staff] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MarkId', N'Feedback', N'Grade', N'MarkEntryDate', N'ObtainedScore', N'PassMarks', N'PassStatus', N'StaffId', N'StudentId', N'SubjectId', N'TotalMarks') AND [object_id] = OBJECT_ID(N'[Mark]'))
    SET IDENTITY_INSERT [Mark] ON;
INSERT INTO [Mark] ([MarkId], [Feedback], [Grade], [MarkEntryDate], [ObtainedScore], [PassMarks], [PassStatus], [StaffId], [StudentId], [SubjectId], [TotalMarks])
VALUES (1, N'Good job!', 1, '2026-04-22T15:32:05.4097110+05:00', 65, 40, 0, 1, 1, 1, 80),
(2, N'Excellent work!', 0, '2026-04-22T15:32:05.4097121+05:00', 75, 40, 0, 2, 2, 2, 90),
(3, N'Excellent work!', 0, '2026-04-22T15:32:05.4097128+05:00', 75, 40, 0, 3, 3, 3, 90);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'MarkId', N'Feedback', N'Grade', N'MarkEntryDate', N'ObtainedScore', N'PassMarks', N'PassStatus', N'StaffId', N'StudentId', N'SubjectId', N'TotalMarks') AND [object_id] = OBJECT_ID(N'[Mark]'))
    SET IDENTITY_INSERT [Mark] OFF;
GO


CREATE INDEX [IX_AcademicMonth_MonthlyPaymentId] ON [AcademicMonth] ([MonthlyPaymentId]);
GO


CREATE INDEX [IX_AcademicMonth_OthersPaymentId] ON [AcademicMonth] ([OthersPaymentId]);
GO


CREATE INDEX [IX_AcademicYear_CampusId] ON [AcademicYear] ([CampusId]);
GO


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO


CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO


CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO


CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO


CREATE INDEX [IX_BankAccounts_CampusId] ON [BankAccounts] ([CampusId]);
GO


CREATE INDEX [IX_Department_CampusId] ON [Department] ([CampusId]);
GO


CREATE INDEX [IX_DueBalance_MonthlyPaymentId] ON [DueBalance] ([MonthlyPaymentId]);
GO


CREATE INDEX [IX_DueBalance_StudentId] ON [DueBalance] ([StudentId]);
GO


CREATE INDEX [IX_ExamSchedule_AcademicYearId] ON [ExamSchedule] ([AcademicYearId]);
GO


CREATE INDEX [IX_ExamSchedule_CampusId] ON [ExamSchedule] ([CampusId]);
GO


CREATE INDEX [IX_ExamScheduleStandard_ExamScheduleId] ON [ExamScheduleStandard] ([ExamScheduleId]);
GO


CREATE INDEX [IX_ExamScheduleStandard_StandardId] ON [ExamScheduleStandard] ([StandardId]);
GO


CREATE INDEX [IX_ExamSubject_ExamScheduleStandardId] ON [ExamSubject] ([ExamScheduleStandardId]);
GO


CREATE INDEX [IX_ExamSubject_ExamTypeId] ON [ExamSubject] ([ExamTypeId]);
GO


CREATE INDEX [IX_ExamSubject_SubjectId] ON [ExamSubject] ([SubjectId]);
GO


CREATE INDEX [IX_Fee_FeeTypeId] ON [Fee] ([FeeTypeId]);
GO


CREATE INDEX [IX_Fee_MonthlyPaymentId] ON [Fee] ([MonthlyPaymentId]);
GO


CREATE INDEX [IX_Fee_OthersPaymentId] ON [Fee] ([OthersPaymentId]);
GO


CREATE INDEX [IX_Fee_StandardId] ON [Fee] ([StandardId]);
GO


CREATE INDEX [IX_GeneralExpense_CampusId] ON [GeneralExpense] ([CampusId]);
GO


CREATE INDEX [IX_GeneralIncome_CampusId] ON [GeneralIncome] ([CampusId]);
GO


CREATE INDEX [IX_Leaves_ApprovedByStaffId] ON [Leaves] ([ApprovedByStaffId]);
GO


CREATE INDEX [IX_Leaves_StaffId] ON [Leaves] ([StaffId]);
GO


CREATE INDEX [IX_Mark_StaffId] ON [Mark] ([StaffId]);
GO


CREATE INDEX [IX_Mark_StudentId] ON [Mark] ([StudentId]);
GO


CREATE INDEX [IX_Mark_SubjectId] ON [Mark] ([SubjectId]);
GO


CREATE INDEX [IX_MarkEntry_ExamScheduleId] ON [MarkEntry] ([ExamScheduleId]);
GO


CREATE INDEX [IX_MarkEntry_ExamTypeId] ON [MarkEntry] ([ExamTypeId]);
GO


CREATE INDEX [IX_MarkEntry_StaffId] ON [MarkEntry] ([StaffId]);
GO


CREATE INDEX [IX_MarkEntry_StandardId] ON [MarkEntry] ([StandardId]);
GO


CREATE INDEX [IX_MarkEntry_SubjectId] ON [MarkEntry] ([SubjectId]);
GO


CREATE INDEX [IX_MonthlyPayment_CampusId] ON [MonthlyPayment] ([CampusId]);
GO


CREATE INDEX [IX_MonthlyPayment_StudentId] ON [MonthlyPayment] ([StudentId]);
GO


CREATE INDEX [IX_OtherPaymentDetail_OthersPaymentId] ON [OtherPaymentDetail] ([OthersPaymentId]);
GO


CREATE INDEX [IX_OthersPayment_CampusId] ON [OthersPayment] ([CampusId]);
GO


CREATE INDEX [IX_OthersPayment_StudentId] ON [OthersPayment] ([StudentId]);
GO


CREATE INDEX [IX_Parents_CampusId] ON [Parents] ([CampusId]);
GO


CREATE INDEX [IX_Parents_UserId] ON [Parents] ([UserId]);
GO


CREATE INDEX [IX_PaymentDetail_MonthlyPaymentId] ON [PaymentDetail] ([MonthlyPaymentId]);
GO


CREATE INDEX [IX_PaymentMonth_MonthlyPaymentId] ON [PaymentMonth] ([MonthlyPaymentId]);
GO


CREATE INDEX [IX_Section_CampusId] ON [Section] ([CampusId]);
GO


CREATE INDEX [IX_Section_StaffId] ON [Section] ([StaffId]);
GO


CREATE INDEX [IX_Section_StandardId] ON [Section] ([StandardId]);
GO


CREATE INDEX [IX_Staff_CampusId] ON [Staff] ([CampusId]);
GO


CREATE INDEX [IX_Staff_DepartmentId] ON [Staff] ([DepartmentId]);
GO


CREATE INDEX [IX_Staff_StaffSalaryId] ON [Staff] ([StaffSalaryId]);
GO


CREATE UNIQUE INDEX [IX_Staff_UniqueStaffAttendanceNumber] ON [Staff] ([UniqueStaffAttendanceNumber]);
GO


CREATE INDEX [IX_StaffExperience_StaffId] ON [StaffExperience] ([StaffId]);
GO


CREATE INDEX [IX_Standard_CampusId] ON [Standard] ([CampusId]);
GO


CREATE INDEX [IX_Student_AcademicYearId] ON [Student] ([AcademicYearId]);
GO


CREATE INDEX [IX_Student_CampusId] ON [Student] ([CampusId]);
GO


CREATE INDEX [IX_Student_ParentId] ON [Student] ([ParentId]);
GO


CREATE INDEX [IX_Student_SectionId] ON [Student] ([SectionId]);
GO


CREATE INDEX [IX_Student_StandardId] ON [Student] ([StandardId]);
GO


CREATE UNIQUE INDEX [IX_Student_UniqueStudentAttendanceNumber] ON [Student] ([UniqueStudentAttendanceNumber]);
GO


CREATE INDEX [IX_Student_UserId] ON [Student] ([UserId]);
GO


CREATE INDEX [IX_StudentMarksDetails_MarkEntryId] ON [StudentMarksDetails] ([MarkEntryId]);
GO


CREATE INDEX [IX_Subject_StandardId] ON [Subject] ([StandardId]);
GO


CREATE UNIQUE INDEX [IX_Subject_SubjectCode] ON [Subject] ([SubjectCode]) WHERE [SubjectCode] IS NOT NULL;
GO


CREATE INDEX [IX_SubjectAssignment_SectionId] ON [SubjectAssignment] ([SectionId]);
GO


CREATE INDEX [IX_SubjectAssignment_StaffId] ON [SubjectAssignment] ([StaffId]);
GO


CREATE INDEX [IX_SubjectAssignment_SubjectId] ON [SubjectAssignment] ([SubjectId]);
GO


