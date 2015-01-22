IF OBJECT_ID (N'tblCaseInvoiceOrderFile', N'U') IS NULL  
BEGIN

	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCaseInvoiceOrderFile](
		[Id] INT IDENTITY(1,1) NOT NULL,
		[OrderId] INT NOT NULL,
		[FileName] NVARCHAR(200) NOT NULL,
		[CreatedDate] DATETIME NOT NULL,
	 CONSTRAINT [PK_tblCaseInvoiceOrderFile] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseInvoiceOrderFile]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseInvoiceOrderFile_tblCaseInvoiceOrder] FOREIGN KEY([OrderId])
	REFERENCES [dbo].[tblCaseInvoiceOrder] ([Id])

	ALTER TABLE [dbo].[tblCaseInvoiceOrderFile] CHECK CONSTRAINT [FK_tblCaseInvoiceOrderFile_tblCaseInvoiceOrder]

	ALTER TABLE [dbo].[tblCaseInvoiceOrderFile] ADD  CONSTRAINT [DF_tblCaseInvoiceOrderFile_FileName]  DEFAULT ('') FOR [FileName]

	ALTER TABLE [dbo].[tblCaseInvoiceOrderFile] ADD  CONSTRAINT [DF_tblCaseInvoiceOrderFile_CreatedDate]  DEFAULT (GETDATE()) FOR [CreatedDate]

END