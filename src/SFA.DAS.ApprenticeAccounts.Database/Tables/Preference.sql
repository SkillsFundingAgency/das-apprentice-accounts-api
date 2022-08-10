CREATE TABLE [dbo].[Preference]
(
	[PreferenceId] INT NOT NULL
					IDENTITY(1, 1)
					PRIMARY KEY, 
	[PreferenceMeaning] NVARCHAR(200) NOT NULL,
	[PreferenceHint] NVARCHAR(2000) NULL
)
