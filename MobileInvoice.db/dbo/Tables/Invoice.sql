CREATE TABLE [dbo].[Invoice] (
    [Id]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100)  NULL,
    [IssueDate] DATE            NULL,
    [DueTerm]   NVARCHAR (50)   NULL,
    [DueDate]   DATE            NULL,
    [ClientId]  BIGINT          NULL,
    [Note]      NVARCHAR (1000) NULL,
    [Signature] NVARCHAR (50)   NULL,
    [Total]     DECIMAL (19, 4) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



