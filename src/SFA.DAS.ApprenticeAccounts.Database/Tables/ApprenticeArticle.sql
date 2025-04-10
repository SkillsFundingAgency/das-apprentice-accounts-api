﻿CREATE TABLE [dbo].[ApprenticeArticle](
    [Id] [uniqueidentifier] NOT NULL,
    [EntryId] [nvarchar](200) NOT NULL,
    [EntryTitle] [nvarchar](1000) NULL,
    [IsSaved] [bit] NULL,
    [LikeStatus] [bit] NULL,
    [SaveTime] DATETIME2 NOT NULL DEFAULT current_timestamp, 
    [LastSaveStatusTime] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApprenticeArticle] ADD  CONSTRAINT [PK_key] PRIMARY KEY CLUSTERED 
(
    [Id] ASC,
    [EntryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
