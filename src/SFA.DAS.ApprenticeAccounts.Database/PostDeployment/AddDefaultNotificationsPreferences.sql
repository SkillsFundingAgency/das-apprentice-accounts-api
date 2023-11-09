CREATE TABLE #TempPreference
(
    [PreferenceId] INT,
    [PreferenceMeaning] NVARCHAR(200),
    [PreferenceHint] NVARCHAR(2000)
)

INSERT INTO #TempPreference VALUES 
(1, 'Giving feedback on your training provider', 'Emails will be sent every 3 months.'),
(2, 'Getting help and advice about your apprenticeship', 'You''ll receive regular emails during your apprenticeship')

MERGE [Preference] AS TARGET
USING #TempPreference AS SOURCE
ON TARGET.PreferenceId = SOURCE.PreferenceId
WHEN MATCHED THEN
    UPDATE SET TARGET.PreferenceMeaning = SOURCE.PreferenceMeaning, TARGET.PreferenceHint = SOURCE.PreferenceHint
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (PreferenceId, PreferenceMeaning, PreferenceHint)
    VALUES (SOURCE.PreferenceId, SOURCE.PreferenceMeaning, SOURCE.PreferenceHint);

DROP TABLE #TempPreference
