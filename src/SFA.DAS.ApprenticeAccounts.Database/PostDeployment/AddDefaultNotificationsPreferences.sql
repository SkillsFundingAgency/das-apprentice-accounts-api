CREATE TABLE #TempPreference
(
	[PreferenceId] INT ,
    [PreferenceMeaning] NVARCHAR(50)
)

INSERT INTO #TempPreference VALUES 
(1,'Feedback on your training provider')

SET IDENTITY_INSERT [dbo].[Preference] ON;

MERGE [Preference] TARGET
USING #TempPreference SOURCE
ON TARGET.PreferenceId=SOURCE.PreferenceId
WHEN MATCHED THEN
UPDATE SET TARGET.PreferenceMeaning = SOURCE.PreferenceMeaning
WHEN NOT MATCHED BY TARGET THEN 
INSERT (PreferenceId,PreferenceMeaning)
VALUES (SOURCE.PreferenceId,SOURCE.PreferenceMeaning);

SET IDENTITY_INSERT [dbo].[Preference] OFF;
DROP TABLE #TempPreference