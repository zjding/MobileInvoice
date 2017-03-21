CREATE TABLE [dbo].[Item] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100)   NULL,
    [Price]          MONEY           NULL,
    [Quantity]       INT             NULL,
    [DiscountType]   VARCHAR (1)     NULL,
    [DiscountAmount] DECIMAL (18, 2) NULL,
    [Taxable]        BIT             NULL,
    [Note]           VARCHAR (1000)  NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC)
);

