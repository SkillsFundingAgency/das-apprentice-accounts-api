CREATE TABLE [dbo].[Apprenticeship]
(
	[Id] BIGINT NOT NULL IDENTITY,
	[ApprenticeId] UNIQUEIDENTIFIER NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT current_timestamp, 
    [LastViewed] DATETIME2 NULL, 
    CONSTRAINT PK_Apprenticeship_Id PRIMARY KEY ([Id]), 
	CONSTRAINT FK_Apprenticeship_Apprentice_ApprenticeId FOREIGN KEY ([ApprenticeId]) REFERENCES [Apprentice] ([Id])
)

GO

CREATE INDEX [IX_Apprenticeship_ApprenticeId] ON [Apprenticeship] ([ApprenticeId]);
GO