CREATE TRIGGER [dbo].[trg_Apprentice_LogAppLastLoggedIn]
ON [dbo].[Apprentice]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(AppLastLoggedIn)
    BEGIN
        INSERT INTO [dbo].[ApprenticeAppLogins] (ApprenticeId, LoginDateTime)
        SELECT 
            i.[Id], 
            i.[AppLastLoggedIn]
        FROM inserted i
        INNER JOIN deleted d ON i.[Id] = d.[Id]
        WHERE 
            (i.[AppLastLoggedIn] <> d.[AppLastLoggedIn] 
             OR (i.[AppLastLoggedIn] IS NULL AND d.[AppLastLoggedIn] IS NOT NULL)
             OR (i.[AppLastLoggedIn] IS NOT NULL AND d.[AppLastLoggedIn] IS NULL));
    END
END;