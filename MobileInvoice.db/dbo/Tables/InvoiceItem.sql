CREATE TABLE [dbo].[InvoiceItem] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (100)  NULL,
    [UnitPrice]      DECIMAL (19, 4) NULL,
    [Quantity]       INT             NULL,
    [DiscountType]   NVARCHAR (1)    NULL,
    [DiscountAmount] DECIMAL (18, 2) NULL,
    [Taxable]        BIT             NULL,
    [Note]           NVARCHAR (1000) NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC)
);

