CREATE TABLE [dbo].[Item] (
    [Id]                  BIGINT          NOT NULL,
    [Name]                NVARCHAR (100)  NULL,
    [UnitPrice]           DECIMAL (19, 4) NULL,
    [CreatedDateTime]     DATETIME        NULL,
    [LastUpdatedDateTime] DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);







