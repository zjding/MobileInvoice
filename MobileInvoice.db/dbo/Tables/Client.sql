CREATE TABLE [dbo].[Client] (
    [Id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]   BIGINT         NULL,
    [Name]     NVARCHAR (100) NULL,
    [Phone]    NVARCHAR (50)  NULL,
    [Email]    NVARCHAR (50)  NULL,
    [Street1]  NVARCHAR (100) NULL,
    [Street2]  NVARCHAR (100) NULL,
    [City]     NVARCHAR (50)  NULL,
    [State]    NVARCHAR (50)  NULL,
    [Country]  NVARCHAR (50)  NULL,
    [PostCode] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([Id] ASC)
);



