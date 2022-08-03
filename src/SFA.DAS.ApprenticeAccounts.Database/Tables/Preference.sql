CREATE TABLE [dbo].[Preference]
(
	[PreferenceId] INT NOT NULL
					IDENTITY(1, 1)
					PRIMARY KEY, 
	[PreferenceMeaning] NVARCHAR(50) NOT NULL
)
