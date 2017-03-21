CREATE TABLE [dbo].[Business] (
    [Id]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (200) NOT NULL,
    [Owner] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED ([Id] ASC)
);

