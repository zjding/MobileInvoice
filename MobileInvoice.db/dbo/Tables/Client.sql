CREATE TABLE [dbo].[Client] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]   BIGINT        NULL,
    [Name]     VARCHAR (100) NULL,
    [Phone]    VARCHAR (20)  NULL,
    [Email]    VARCHAR (50)  NULL,
    [Street1]  VARCHAR (100) NULL,
    [Street2]  VARCHAR (100) NULL,
    [City]     VARCHAR (50)  NULL,
    [State]    VARCHAR (50)  NULL,
    [Country]  VARCHAR (50)  NULL,
    [PostCode] VARCHAR (20)  NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([Id] ASC)
);

