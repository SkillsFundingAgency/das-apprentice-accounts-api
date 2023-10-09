CREATE TABLE [dbo].[MyApprenticeship](
	[Id] [uniqueidentifier]  NOT NULL DEFAULT newid(),
	[ApprenticeId] [uniqueidentifier] NOT NULL,
	[Uln] [bigint] NULL,
	[ApprenticeshipId] [bigint] NULL,
	[EmployerName] [nvarchar](200) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[TrainingProviderId] [bigint] NULL,
	[TrainingProviderName] [nvarchar](200) NULL,
	[TrainingCode] [nvarchar](15) NULL,
	[StandardUId] [nvarchar](20) NULL,
	[CreatedOn] [datetime2](7) NOT NULL DEFAULT (getutcdate()),
 	CONSTRAINT [PK_MyApprenticeship_Id] PRIMARY KEY (Id),
 	CONSTRAINT FK_MyApprenticeship_Apprentice FOREIGN KEY (ApprenticeId) REFERENCES [Apprentice] (Id),
	INDEX IXU_MyApprenticeship_ApprenticeId UNIQUE NONCLUSTERED (ApprenticeId)
	)