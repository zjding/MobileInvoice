CREATE TABLE [dbo].[Attachment] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ImageName]   NVARCHAR (50)  NULL,
    [Description] NVARCHAR (500) NULL,
    CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED ([Id] ASC)
);



