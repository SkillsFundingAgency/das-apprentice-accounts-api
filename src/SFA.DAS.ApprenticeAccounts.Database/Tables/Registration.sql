CREATE TABLE [dbo].[Registration]
(
    [RegistrationId] UNIQUEIDENTIFIER NOT NULL,
    [CommitmentsApprenticeshipId] BIGINT NOT NULL, 
    [CommitmentsApprovedOn] DATETIME2 NOT NULL, 
    [FirstName] NVARCHAR(150) NOT NULL, 
    [LastName] NVARCHAR(150) NOT NULL,
    [DateOfBirth] DATETIME2 NOT NULL,
    [Email] NVARCHAR(150) NOT NULL, 
    [UserIdentityId] UNIQUEIDENTIFIER NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT current_timestamp,
    [EmployerAccountLegalEntityId] BIGINT NOT NULL, 
    [EmployerName] NVARCHAR(100) NOT NULL , 
    [TrainingProviderId] BIGINT NOT NULL, 
    [TrainingProviderName] NVARCHAR(100) NOT NULL, 
    [CourseName] NVARCHAR(MAX) NOT NULL, 
    [CourseLevel] int NOT NULL, 
    [CourseOption] NVARCHAR(MAX) NULL, 
    [PlannedStartDate] datetime2 NOT NULL,
    [PlannedEndDate] datetime2 NOT NULL,
    [FirstViewedOn] DATETIME2 NULL, 
    [SignUpReminderSentOn] DATETIME2 NULL, 
    [CourseDuration] INT NULL,
    [ApprenticeId] UNIQUEIDENTIFIER NULL , 
    [ApprenticeshipId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT PK_Registration_ApprenticeId PRIMARY KEY CLUSTERED ([RegistrationId]),
    CONSTRAINT [FK_Registration_Apprentice_ApprenticeId] FOREIGN KEY ([ApprenticeId]) REFERENCES [dbo].[Apprentice] ([Id]) ON DELETE CASCADE
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Registration_CommitmentsApprenticeshipId]
    ON [dbo].[Registration]([CommitmentsApprenticeshipId]);

GO