CREATE TABLE [dbo].[Invoice_Attachment_Rel] (
    [Id]           BIGINT IDENTITY (1, 1) NOT NULL,
    [InvoiceId]    BIGINT NULL,
    [AttachmentId] BIGINT NULL,
    CONSTRAINT [PK_Invoice_Attachment_Rel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

