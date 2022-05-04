CREATE TABLE [dbo].[ApprenticePreferences]
(
	[ApprenticeId] UNIQUEIDENTIFIER NOT NULL , 
    [PreferenceId] INT NOT NULL, 
    [Status] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedOn] DATETIME NULL, 
    CONSTRAINT [FK_ApprenticePreferences_Apprentice_ApprenticeId] FOREIGN KEY ([ApprenticeId]) REFERENCES [dbo].[Apprentice] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ApprenticePreferences_Preference_PreferenceId] FOREIGN KEY ([PreferenceId]) REFERENCES [dbo].[Preference] ([PreferenceId]) ON DELETE CASCADE

)
