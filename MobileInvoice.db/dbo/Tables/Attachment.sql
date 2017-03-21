CREATE TABLE [dbo].[Attachment] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ImageName]   VARCHAR (50)  NULL,
    [Description] VARCHAR (500) NULL,
    CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

