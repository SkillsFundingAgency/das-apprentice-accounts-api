CREATE TABLE [dbo].[Apprentice]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[FirstName] NVARCHAR(100) NULL,
	[LastName] NVARCHAR(100) NULL,
	[Email] NVARCHAR(200) NOT NULL, 
    [DateOfBirth] DATETIME2 NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT current_timestamp, 
    [TermsOfUseAcceptedOn] DATETIME2 NULL,
    [UpdatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
	[GovUkIdentifier] NVARCHAR(150) NULL,
    [AppLastLoggedIn] DATETIME2 NULL,	
    CONSTRAINT PK_Apprentice_Id PRIMARY KEY ([Id])
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Apprentice_Email]
    ON [dbo].[Apprentice]
	(
		[Email] ASC
	);

GO

CREATE NONCLUSTERED INDEX [IX_Apprentice_UpdatedOn] ON [Apprentice] ([UpdatedOn]) INCLUDE ([Id], [FirstName], [LastName], [Email], [DateOfBirth])
GO

CREATE NONCLUSTERED INDEX [IX_Apprentice_GovUkIdentifier] ON [Apprentice] ([GovUkIdentifier]) 
	INCLUDE ([Id], [FirstName], [LastName], [Email], [DateOfBirth],[CreatedOn], [TermsOfUseAcceptedOn], [UpdatedOn])
GO