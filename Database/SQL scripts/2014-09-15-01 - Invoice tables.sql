IF OBJECT_ID (N'tblCaseInvoiceArticle', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblCaseInvoiceArticle]
GO

IF OBJECT_ID (N'tblCaseInvoiceOrder', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblCaseInvoiceOrder]
GO

IF OBJECT_ID (N'tblCaseInvoice', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblCaseInvoice]
GO

IF OBJECT_ID (N'tblInvoiceArticle', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblInvoiceArticle]
GO

IF OBJECT_ID (N'tblInvoiceArticleUnit', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblInvoiceArticleUnit]
GO

CREATE TABLE [dbo].[tblInvoiceArticleUnit]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(20) NOT NULL,
	[CustomerId] INT NOT NULL
	CONSTRAINT [PK_tblInvoiceArticleUnit] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) 
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON, 
		FILLFACTOR = 90) ON [PRIMARY]
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblInvoiceArticleUnit] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticleUnit_tblCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tblCustomer] ([Id])
GO

CREATE TABLE [dbo].[tblInvoiceArticle]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[ParentId] INT NULL,
	[Number] NVARCHAR(15) NOT NULL,
	[Name] NVARCHAR(100) NOT NULL,
	[NameEng] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(200) NULL,
	[UnitId] INT NULL,
	[Ppu] DECIMAL NULL,
	[ProductAreaId] INT NOT NULL,
	[CustomerId] INT NOT NULL
	CONSTRAINT [PK_tblInvoiceArticle] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) 
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON, 
		FILLFACTOR = 90) ON [PRIMARY]
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticle] FOREIGN KEY([ParentId])
REFERENCES [dbo].[tblInvoiceArticle] ([Id])
GO

ALTER TABLE [dbo].[tblInvoiceArticle] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticle]
GO

ALTER TABLE [dbo].[tblInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticleUnit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[tblInvoiceArticleUnit] ([Id])
GO

ALTER TABLE [dbo].[tblInvoiceArticle] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticleUnit]
GO

ALTER TABLE [dbo].[tblInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticle_tblCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tblCustomer] ([Id])
GO

ALTER TABLE [dbo].[tblInvoiceArticle] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblCustomer]
GO

CREATE TABLE [dbo].[tblCaseInvoice]
(
	[Id] INT IDENTITY(1,1) NOT NULL,	
	[CaseId] INT NOT NULL
	CONSTRAINT [PK_tblCaseInvoice] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) 
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON, 
		FILLFACTOR = 90) ON [PRIMARY]		
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblCaseInvoice] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoice_tblCase] FOREIGN KEY([CaseId])
REFERENCES [dbo].[tblCase] ([Id])
GO

CREATE TABLE [dbo].[tblCaseInvoiceOrder]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[InvoiceId] INT NOT NULL,
	[Number] SMALLINT NOT NULL DEFAULT(0),
	[DeliveryPeriod] NVARCHAR(200) NULL,
	[Reference] NVARCHAR(100) NULL,
	[Date] DATETIME NOT NULL
	CONSTRAINT [PK_tblCaseInvoiceOrder] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) 
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON, 
		FILLFACTOR = 90) ON [PRIMARY]	
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblCaseInvoiceOrder] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoiceOrder_tblCaseInvoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[tblCaseInvoice] ([Id])
GO

ALTER TABLE [dbo].[tblCaseInvoiceOrder] ADD CONSTRAINT [DF_tblCaseInvoiceOrder_Date] DEFAULT (GETDATE()) FOR [Date]
GO

CREATE TABLE [dbo].[tblCaseInvoiceArticle]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[OrderId] INT NOT NULL,
	[ArticleId] INT NULL,
	[Name] NVARCHAR(100) NOT NULL,
	[Amount] INT NULL,
	[Ppu] DECIMAL NULL,
	[Position] SMALLINT NOT NULL,
	[IsInvoiced] BIT NOT NULL
	CONSTRAINT [PK_tblCaseInvoiceArticle] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) 
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON, 
		FILLFACTOR = 90) ON [PRIMARY]
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblCaseInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoiceArticle_tblCaseInvoiceOrder] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tblCaseInvoiceOrder] ([Id])
GO

ALTER TABLE [dbo].[tblCaseInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoiceArticle_tblInvoiceArticle] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[tblInvoiceArticle] ([Id])
GO

ALTER TABLE [dbo].[tblCaseInvoiceArticle] ADD  CONSTRAINT [DF_tblCaseInvoiceArticle_IsInvoiced] DEFAULT (0) FOR [IsInvoiced]
GO

ALTER TABLE [dbo].[tblCaseInvoiceArticle] ADD  CONSTRAINT [DF_tblCaseInvoiceArticle_Position]  DEFAULT (0) FOR [Position]
GO

IF COL_LENGTH('dbo.tblSettings','ModuleCaseInvoice') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblSettings]
	ADD [ModuleCaseInvoice] INT NOT NULL DEFAULT(0)
END
