
CREATE TABLE [dbo].[MyApprenticeship](
	[Id] [uniqueidentifier] NOT NULL,
	[ApprenticeId] [uniqueidentifier] NOT NULL,
	[Uln] [bigint] NULL,
	[ApprenticeshipId] [bigint] NULL,
	[EmployerName] [nvarchar](200) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[TrainingProviderId] [bigint] NULL,
	[TrainingProviderName] [nvarchar](200) NULL,
	[TrainingCode] [int] NULL,
	[TrainingCourseOption] [nvarchar](200) NULL,
	[StandardUid] [nvarchar](20) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MyApprenticeship_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MyApprenticeship] ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[MyApprenticeship] ADD  CONSTRAINT [DF_MyApprenticeship_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO


CREATE NONCLUSTERED INDEX [IDX_MyApprenticeship_ApprenticeId] ON [dbo].[MyApprenticeship]
(
	[ApprenticeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IDX_MyApprenticeship_CreatedOn] ON [dbo].[MyApprenticeship]
(
	[CreatedOn] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO