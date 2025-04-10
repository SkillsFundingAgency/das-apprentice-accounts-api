IF NOT EXISTS (SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE upper(TABLE_NAME) = 'ApprenticeArticle'
        AND upper(COLUMN_NAME) = 'EntryTitle')
BEGIN
    ALTER TABLE ApprenticeArticle ADD [EntryTitle] [nvarchar](1000) NULL;
END
GO