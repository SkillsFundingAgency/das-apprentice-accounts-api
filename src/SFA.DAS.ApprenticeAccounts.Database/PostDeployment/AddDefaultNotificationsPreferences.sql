CREATE TABLE #TempPreference
(
	[PreferenceId] INT ,
    [PreferenceMeaning] NVARCHAR(200),
	[PreferenceHint] NVARCHAR(2000)
)

INSERT INTO #TempPreference VALUES 
(1,'Giving feedback on your training provider','Emails will be sent every 3 months.')

SET IDENTITY_INSERT [dbo].[Preference] ON;

MERGE [Preference] TARGET
USING #TempPreference SOURCE
ON TARGET.PreferenceId=SOURCE.PreferenceId
WHEN MATCHED THEN
UPDATE SET TARGET.PreferenceMeaning = SOURCE.PreferenceMeaning, TARGET.PreferenceHint = SOURCE.PreferenceHint
WHEN NOT MATCHED BY TARGET THEN 
INSERT (PreferenceId,PreferenceMeaning,PreferenceHint)
VALUES (SOURCE.PreferenceId,SOURCE.PreferenceMeaning, SOURCE.PreferenceHint);

SET IDENTITY_INSERT [dbo].[Preference] OFF;
DROP TABLE #TempPreference

-- Add Default value for new apprentice preferences if they don't already exist and set to true.
MERGE INTO ApprenticePreferences Target
  USING (SELECT a.Id AS ApprenticeId,p.PreferenceId, 1 AS Status, GetUtcDate() AS CreatedOn, GetUtcDate() AS UpdatedOn FROM Apprentice a CROSS JOIN Preference p) AS Source
  ON Target.ApprenticeId = source.ApprenticeId AND Target.PreferenceId = source.PreferenceId
  WHEN NOT MATCHED THEN
	INSERT (ApprenticeId,PreferenceId,Status,CreatedOn,UpdatedOn) VALUES(ApprenticeId,PreferenceId,Status,CreatedOn,UpdatedOn); 