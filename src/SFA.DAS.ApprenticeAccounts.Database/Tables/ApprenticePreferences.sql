CREATE TABLE [dbo].[ApprenticePreferences]
(
	[ApprenticeId] UNIQUEIDENTIFIER NOT NULL , 
    [PreferenceId] INT NOT NULL, 
    [Status] BIT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedOn] DATETIME NULL, 
    CONSTRAINT [FK_ApprenticePreferences_Apprentice_ApprenticeId] FOREIGN KEY ([ApprenticeId]) REFERENCES [dbo].[Apprentice] ([Id]),
    CONSTRAINT [FK_ApprenticePreferences_Preference_PreferenceId] FOREIGN KEY ([PreferenceId]) REFERENCES [dbo].[Preference] ([PreferenceId])
)
    GO

    CREATE UNIQUE CLUSTERED INDEX IXU_ApprenticeIdPreferenceId
                                ON [dbo].[ApprenticePreferences](ApprenticeId, PreferenceId)
    GO
