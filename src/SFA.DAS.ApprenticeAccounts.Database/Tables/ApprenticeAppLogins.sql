CREATE TABLE [dbo].[ApprenticeAppLogins] (
    [Id] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_ApprenticeAppLogins_Id DEFAULT NEWID(),
    [ApprenticeId] UNIQUEIDENTIFIER NOT NULL,
    [LoginDateTime] DATETIME2 (7) NULL,
    CONSTRAINT PK_ApprenticeAppLogins PRIMARY KEY CLUSTERED ([Id] ASC)
);