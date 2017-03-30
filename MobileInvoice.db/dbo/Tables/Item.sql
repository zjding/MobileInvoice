CREATE TABLE [dbo].[Item] (
    [Id]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (100)  NULL,
    [UnitPrice]           DECIMAL (19, 4) NULL,
    [CreatedDateTime]     DATETIME        NULL,
    [LastUpdatedDateTime] DATETIME        NULL,
    CONSTRAINT [PK__Table__3214EC0747BFAB87] PRIMARY KEY CLUSTERED ([Id] ASC)
);









