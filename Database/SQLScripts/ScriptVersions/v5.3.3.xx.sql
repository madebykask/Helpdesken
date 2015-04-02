--USE [dhHelpdeskNG_Test_IMS]
--GO
--USE [CHS_NG]
--GO

-- Ändra storlek på fält tblLink
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'URLAddress' and sysobjects.name = N'tblLink')
	begin
		ALTER TABLE tblLink ALTER COLUMN URLAddress nvarchar(300) not null
	end
GO

--invoice tabeller
if not exists(select * from sysobjects WHERE Name = N'tblInvoiceArticleUnit')
	begin
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

		ALTER TABLE [dbo].[tblInvoiceArticleUnit] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticleUnit_tblCustomer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[tblCustomer] ([Id])
	end
GO


if not exists(select * from sysobjects WHERE Name = N'tblInvoiceArticle')
	begin
		CREATE TABLE [dbo].[tblInvoiceArticle]
		(
			[Id] INT IDENTITY(1,1) NOT NULL,
			[ParentId] INT NULL,
			[Number] INT NOT NULL,
			[Name] NVARCHAR(100) NOT NULL,
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

		ALTER TABLE [dbo].[tblInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticle] FOREIGN KEY([ParentId]) REFERENCES [dbo].[tblInvoiceArticle] ([Id])
		 
		ALTER TABLE [dbo].[tblInvoiceArticle] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticle]

		ALTER TABLE [dbo].[tblInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticleUnit] FOREIGN KEY([UnitId]) REFERENCES [dbo].[tblInvoiceArticleUnit] ([Id])

		ALTER TABLE [dbo].[tblInvoiceArticle] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblInvoiceArticleUnit]

		ALTER TABLE [dbo].[tblInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblInvoiceArticle_tblCustomer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[tblCustomer] ([Id])

		ALTER TABLE [dbo].[tblInvoiceArticle] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblCustomer]
	end
GO


if not exists(select * from sysobjects WHERE Name = N'tblCaseInvoice')
	begin
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

		ALTER TABLE [dbo].[tblCaseInvoice] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoice_tblCase] FOREIGN KEY([CaseId]) REFERENCES [dbo].[tblCase] ([Id])
	end
go


if not exists(select * from sysobjects WHERE Name = N'tblCaseInvoiceOrder')
	begin
		CREATE TABLE [dbo].[tblCaseInvoiceOrder]
		(
			[Id] INT IDENTITY(1,1) NOT NULL,
			[InvoiceId] INT NOT NULL,
			[Number] SMALLINT NOT NULL DEFAULT(0),
			[DeliveryPeriod] DATETIME NULL
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

		ALTER TABLE [dbo].[tblCaseInvoiceOrder] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoiceOrder_tblCaseInvoice] FOREIGN KEY([InvoiceId]) REFERENCES [dbo].[tblCaseInvoice] ([Id])
	end
GO


if not exists(select * from sysobjects WHERE Name = N'tblCaseInvoiceArticle')
	begin
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

		ALTER TABLE [dbo].[tblCaseInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoiceArticle_tblCaseInvoiceOrder] FOREIGN KEY([OrderId]) REFERENCES [dbo].[tblCaseInvoiceOrder] ([Id])

		ALTER TABLE [dbo].[tblCaseInvoiceArticle] WITH CHECK ADD CONSTRAINT [FK_tblCaseInvoiceArticle_tblInvoiceArticle] FOREIGN KEY([ArticleId]) REFERENCES [dbo].[tblInvoiceArticle] ([Id])

		ALTER TABLE [dbo].[tblCaseInvoiceArticle] ADD  CONSTRAINT [DF_tblCaseInvoiceArticle_IsInvoiced] DEFAULT (0) FOR [IsInvoiced]

		ALTER TABLE [dbo].[tblCaseInvoiceArticle] ADD  CONSTRAINT [DF_tblCaseInvoiceArticle_Position]  DEFAULT (0) FOR [Position]
	end
go

--nytt fält tblSettings
IF COL_LENGTH('dbo.tblSettings','ModuleCaseInvoice') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblSettings] ADD [ModuleCaseInvoice] INT NOT NULL DEFAULT(0)
END
GO


--diverse nya fält
IF COL_LENGTH('dbo.tblCustomerUser','CaseRegistrationDateFilterShow') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseRegistrationDateFilterShow] BIT NOT NULL DEFAULT(0)
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseWatchDateFilterShow') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseWatchDateFilterShow] BIT NOT NULL DEFAULT(0)
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingDateFilterShow') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseClosingDateFilterShow] BIT NOT NULL DEFAULT(0)
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseRegistrationDateStartFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseRegistrationDateStartFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseRegistrationDateEndFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseRegistrationDateEndFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseWatchDateStartFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseWatchDateStartFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseWatchDateEndFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseWatchDateEndFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingDateStartFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseClosingDateStartFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingDateEndFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser] ADD [CaseClosingDateEndFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingReasonFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]	ADD [CaseClosingReasonFilter] NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','OrderNum') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCaseSolution]	ADD [OrderNum] INT NULL
END
GO


IF COL_LENGTH('dbo.tblCaseFile','UserId') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCaseFile]	ADD [UserId] INT NULL
	ALTER TABLE [dbo].[tblCaseFile] WITH CHECK ADD CONSTRAINT [FK_tblCaseFile_tblUser] FOREIGN KEY([UserId]) REFERENCES [dbo].[tblUsers] ([Id])
END
GO

--data till tblModule?
--SPRÅKHANTERING????? 
/*
if not exists (select [name] from tblModule where [name] like 'Customer')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Customer', 'Customer')
end
go
if not exists (select [name] from tblModule where [name] like 'Problem')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Problem', 'Problem')
end
go
if not exists (select [name] from tblModule where [name] like 'Statistics')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Statistics', 'Statistics')
end
go
if not exists (select [name] from tblModule where [name] like 'Bulletin Board')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Bulletin Board', 'Bulletin Board')
end
go
if not exists (select [name] from tblModule where [name] like 'Calendar')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Calendar', 'Calendar')
end
go
if not exists (select [name] from tblModule where [name] like 'FAQ')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('FAQ', 'FAQ')
end
go
if not exists (select [name] from tblModule where [name] like 'Operational Log')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Operational Log', 'Operational Log')
end
go
if not exists (select [name] from tblModule where [name] like 'Daily Report')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Daily Report', 'Daily Report')
end
go
if not exists (select [name] from tblModule where [name] like 'Calendar')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Calendar', 'Calendar')
end
go
if not exists (select [name] from tblModule where [name] like 'FAQ')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('FAQ', 'FAQ')
end
go
if not exists (select [name] from tblModule where [name] like 'Daily Report')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Daily Report', 'Daily Report')
end
go
if not exists (select [name] from tblModule where [name] like 'Quick Link')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Quick Link', 'Quick Link')
end
go
if not exists (select [name] from tblModule where [name] like 'Document')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Document', 'Document')
end
go
if not exists (select [name] from tblModule where [name] like 'Change Management')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Change Management', 'Change Management')
end
go
if not exists (select [name] from tblModule where [name] like 'My Cases')
begin
	INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('My Cases', 'My Cases')
end
go
*/

if not exists(select * from sysobjects WHERE Name = N'tblCaseSolutionFieldSettings')
	begin
		CREATE TABLE [dbo].[tblCaseSolutionFieldSettings](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[CaseSolution_Id] [int] NOT NULL,
			[FieldName_Id] [int] NOT NULL,
			[Mode] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
			[ChangedDate] [datetime] NOT NULL,
		 CONSTRAINT [PK_tblCaseSolutionFieldSettings] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_Mode]  DEFAULT ((0)) FOR [Mode]

		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolutionFieldSettings_tblCaseSolution] FOREIGN KEY([CaseSolution_Id]) REFERENCES [dbo].[tblCaseSolution] ([Id])

		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] CHECK CONSTRAINT [FK_tblCaseSolutionFieldSettings_tblCaseSolution]

		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD CONSTRAINT [DF_tblCaseSolutionFieldSettings_UNIQUE] UNIQUE (CaseSolution_Id, FieldName_Id)

		ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD CONSTRAINT [DF_tblCaseSolutionFieldSettings_Mode_Check] CHECK (Mode >= 1 AND Mode <= 3)
	end
go


-- Majid 2014-11-25 

UPDATE tblModule Set Name = 'Ärendeöversikt' WHERE Id=1;
GO

UPDATE tblModule Set Name = 'Problem' WHERE Id=2;
GO

UPDATE tblModule Set Name = 'Statistik' WHERE Id=3;
GO

UPDATE tblModule Set Name = 'Anslagstavla' WHERE Id=4;
GO

UPDATE tblModule Set Name = 'Aktuellt' WHERE Id=5;
GO

UPDATE tblModule Set Name = 'Driftlogg' WHERE Id=7;
GO

UPDATE tblModule Set Name = 'Dagrapport' WHERE Id=8;
GO

UPDATE tblModule Set Name = 'Snabblänk' WHERE Id=9;
GO

UPDATE tblModule Set Name = 'Dokument' WHERE Id=10;
GO

UPDATE tblModule Set Name = 'Ändringshantering' WHERE Id=11;
GO

UPDATE tblModule Set Name = 'Mina tilldelade ärenden' WHERE Id=12;
GO


-- Majid 2014-12-03 
Update tblSettings set CaseWorkingGroupSource = 1
Go

IF COL_LENGTH('dbo.tblGlobalSettings','HelpdeskDBVersion') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblGlobalSettings]
	ADD [HelpdeskDBVersion] nvarchar(20) NULL 
END
GO 

UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.0.4'
GO
-- 2015-01-14 Released Version 5.0.4.1  ------------------------------------------------------------------------------------

-- Nina 2015-01-10 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseSolution_Id' and sysobjects.name = N'tblLink')
begin
	ALTER TABLE tblLInk ADD CaseSolution_Id int NULL 

	ALTER TABLE [dbo].[tblLink] ADD 
		CONSTRAINT [FK_tblLink_tblCaseSolution] FOREIGN KEY 
		(
			[CaseSolution_Id]
		) REFERENCES [dbo].[tblCaseSolution] (
			[Id]
		)	
end
GO 

-- Alexander 2015-01-16
if not exists(select * from sysobjects WHERE Name = N'tblSurvey')
begin
	CREATE TABLE [dbo].[tblSurvey](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[caseId] [int] NOT NULL,
		[VoteResult] [int] NOT NULL,
	 CONSTRAINT [PK_tblSurvey_1] PRIMARY KEY CLUSTERED 
	(
		[caseId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]	
end
Go

-- Alexander 2015-01-16
if Not exists (select * from tblreport where id = 22) 
begin
  insert into [dbo].[tblReport] values(22);
end
GO

-- Danial 2015-01-16
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Modal' and sysobjects.name = N'tblForm')
begin		
  Alter Table tblForm Add Modal bit default 0		
end
GO

update tblForm set Modal = 0


-- Alexi 2015-01-15 
if not exists(select * from sysobjects WHERE Name = N'tblCaseInvoiceOrderFile')
begin
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
end

-- Majid 2015_01_23
IF COL_LENGTH('dbo.tbltext','ChangedByUser_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].[tbltext] ADD [ChangedByUser_Id] int NULL 
END
GO

IF COL_LENGTH('dbo.tbltextTranslation','ChangedByUser_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblTextTranslation] ADD [ChangedByUser_Id] int NULL 
END
GO

IF COL_LENGTH('dbo.tbltextTranslation','ChangedDate') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblTextTranslation] ADD [ChangedDate] Datetime NULL  default((getdate()))
END
GO

IF COL_LENGTH('dbo.tbltextTranslation','CreatedDate') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblTextTranslation] ADD [CreatedDate] Datetime NULL default((getdate()))
END
GO

IF COL_LENGTH('dbo.tblPriority','OrderNum') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblPriority] ADD [OrderNum] int NULL 
END
GO

-- Majid 215-01-29
ALTER TABLE tblCustomer 
 ALTER COLUMN CommunicateWithNotifier int NOT NULL 
Go

-- Majid 215-01-29
DECLARE @Command nvarchar(max), @ConstaintName nvarchar(max), @TableName nvarchar(max),@ColumnName nvarchar(max)
SET @TableName = 'tblCustomer'
SET @ColumnName ='CommunicateWithNotifier'
SELECT @ConstaintName = name
    FROM sys.default_constraints
    WHERE parent_object_id = object_id(@TableName)
        AND parent_column_id = columnproperty(object_id(@TableName), @ColumnName, 'ColumnId')

SELECT @Command = 'ALTER TABLE '+@TableName+' drop constraint '+ @ConstaintName

IF @Command IS NOT NULL
BEGIN
    EXECUTE sp_executeSQL @Command
    SELECT @Command = 'ALTER TABLE '+@TableName+' ADD CONSTRAINT '+@ConstaintName+' DEFAULT ((1)) FOR ' + @ColumnName
    EXECUTE sp_executeSQL @Command
END
Go

UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.0'


-- Released Version 5.3.0  (2015-01-22) **************************************************************************************************************** 

-- Nina 2015-01-27
IF COL_LENGTH('dbo.tblCaseSolution','PersonsName') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD PersonsName NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','PersonsPhone') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD PersonsPhone NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','PersonsCellPhone') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD PersonsCellPhone NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Region_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Region_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblRegion] FOREIGN KEY 
			(
				[Region_Id]
			) REFERENCES [dbo].[tblRegion] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','OU_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD OU_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblOU] FOREIGN KEY 
			(
				[OU_Id]
			) REFERENCES [dbo].[tblOU] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Place') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Place NVARCHAR(100) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','UserCode') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD UserCode NVARCHAR(20) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','System_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD System_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblSystem] FOREIGN KEY 
			(
				[System_Id]
			) REFERENCES [dbo].[tblSystem] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Urgency_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Urgency_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblUrgency] FOREIGN KEY 
			(
				Urgency_Id
			) REFERENCES [dbo].[tblUrgency] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Impact_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Impact_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblImpact] FOREIGN KEY 
			(
				Impact_Id
			) REFERENCES [dbo].[tblImpact] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InvoiceNumber') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InvoiceNumber NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','ReferenceNumber') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD ReferenceNumber NVARCHAR(200) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Status_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Status_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblStatus] FOREIGN KEY 
			(
				Status_Id
			) REFERENCES [dbo].[tblStatus] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','StateSecondary_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD StateSecondary_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblStateSecondary] FOREIGN KEY 
			(
				StateSecondary_Id
			) REFERENCES [dbo].tblStateSecondary (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Verified') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD Verified int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','VerifiedDescription') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD VerifiedDescription NVARCHAR(200) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','SolutionRate') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD SolutionRate NVARCHAR(10) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InventoryNumber') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InventoryNumber NVARCHAR(20) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InventoryType') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InventoryType NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InventoryLocation') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InventoryLocation NVARCHAR(100) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Supplier_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Supplier_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblSupplier] FOREIGN KEY 
			(
				Supplier_Id
			) REFERENCES [dbo].tblSupplier (
				[Id]
			)		
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','SMS') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD SMS int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Available') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Available NVARCHAR(100) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Cost') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD Cost int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','OtherCost') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD OtherCost int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Currency') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Currency NVARCHAR(10) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','ContactBeforeAction') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD ContactBeforeAction int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Problem_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Problem_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblProblem] FOREIGN KEY 
			(
				Problem_Id
			) REFERENCES [dbo].tblProblem (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Change_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Change_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblChange] FOREIGN KEY 
			(
				Change_Id
			) REFERENCES [dbo].tblChange (
				[Id]
			)	
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WatchDate' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD WatchDate datetime NULL			
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDate' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD FinishingDate datetime NULL			
	end
GO

IF COL_LENGTH('dbo.tblCaseSolution','FinishingDescription') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD FinishingDescription NVARCHAR(200) NULL 
END
GO

-- Majid 2015-02-10
--IF COL_LENGTH('dbo.tblCustomer','SendMailToNotifierByChangeSubstate') IS NULL
--BEGIN
--	ALTER TABLE [dbo].tblCustomer ADD SendMailToNotifierByChangeSubstate bit Not NULL default ((1))
--END
--GO

-- Danial 2015-02-10
IF COL_LENGTH('dbo.tblCaseSolution','FormGUID') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD FormGUID uniqueidentifier NULL 
END
GO

-- Nina 2015-02-11
IF COL_LENGTH('dbo.tblCaseSolution','UpdateNotifierInformation') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD UpdateNotifierInformation int null 
END
GO

UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.1'

-- Released Version 5.3.1.7  (2015-01-22) **************************************************************************************************************** 

-- Nina 2015-02-18
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PlanDate' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD PlanDate datetime NULL			
	end
GO

IF COL_LENGTH('dbo.tblCaseSolution','CausingPartId') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD CausingPartId int null
END
GO

-- Majid 2015-02-18
IF COL_LENGTH('dbo.tblCustomer','ShowDocumentsOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowDocumentsOnExternalPage int not null default (0)
END
GO

-- Majid 2015-02-19
IF COL_LENGTH('dbo.tblDocumentCategory','ShowOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblDocumentCategory ADD ShowOnExternalPage bit not null default (0)
END
GO

-- Majid 2015-02-20
if not exists(select * from sysobjects WHERE Name = N'tblADFSSetting')
begin
	CREATE TABLE [dbo].[tblADFSSetting](
	[ApplicationId] [nvarchar](50) NULL,
	[AttrDomain] [nvarchar](50) NULL,
	[AttrUserId] [nvarchar](50) NULL,
	[AttrEmployeeNumber] [nvarchar](50) NULL,
	[AttrFirstName] [nvarchar](50) NULL,
	[AttrSurName] [nvarchar](50) NULL,
	[AttrEmail] [nvarchar](50) NULL,
	[SaveSSOLog] [bit] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblADFSSetting] ADD  CONSTRAINT [DF_tblADFSSetting_SaveSSOLog]  DEFAULT ((1)) FOR [SaveSSOLog]

end

UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.2'


-- Released Version 5.3.2.4  (2015-02-27) **************************************************************************************************************** 

-- Majid 2015-02-27
IF COL_LENGTH('dbo.tblCustomer','ShowFAQOnExternalStartPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowFAQOnExternalStartPage int null default (0)
END
GO

-- Majid 2015-03-03
IF COL_LENGTH('dbo.tblCustomer','ShowCoWorkersOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowCoWorkersOnExternalPage int not null default (0)
END
GO


-- Majid 2015-03-03
IF COL_LENGTH('dbo.tblCustomer','ShowHelpOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowHelpOnExternalPage int not null default (1)
END
GO

-- Alexi 2015-03-05
IF COL_LENGTH('dbo.tblComputerUsers','UserCode') IS NOT NULL
BEGIN
	ALTER TABLE [dbo].[tblComputerUsers] 
	ALTER COLUMN [UserCode] NVARCHAR(50) NOT NULL
END
GO 


-- Majid 2015-03-06
IF COL_LENGTH('dbo.tblCaseInvoiceOrder','Reference') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseInvoiceOrder ADD Reference nvarchar(100) null 
END
GO 

-- Majid 2015-03-06
IF COL_LENGTH('dbo.tblCaseInvoiceOrder','Date') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseInvoiceOrder ADD [Date] datetime not null default((getdate()))
END
GO 

-- Alexi 2015-03-11
IF COL_LENGTH('dbo.tblCustomerUser','ShowOnStartPage') IS NOT NULL
BEGIN	
	DECLARE @ConstraintName VARCHAR(256)
	SET @ConstraintName = (
		 SELECT [obj].[name]
		 FROM [sys].[columns] col 
			LEFT OUTER JOIN [sys].[objects] obj ON obj.[object_id] = col.[default_object_id] 
			AND obj.[type] = 'D' 
		 WHERE col.[object_id] = OBJECT_ID('tblCustomerUser') 
			AND obj.[name] IS NOT NULL
			AND col.[name] = 'ShowOnStartPage'
	) 

	IF(@ConstraintName IS NOT NULL)
	BEGIN
		EXEC ('ALTER TABLE [dbo].[tblCustomerUser] DROP CONSTRAINT [' + @ConstraintName + ']')
	END
		
	ALTER TABLE [dbo].[tblCustomerUser] 
	ADD CONSTRAINT [DF_tblCustomerUser_ShowOnStartPage] DEFAULT ((1)) FOR [ShowOnStartPage]
END
GO 

-- Majid 2015-03-12
update tblcustomeruser set showonstartpage = 1 where user_id in 
(select tblusers.id from tblusers inner join tblCustomerUser on tblusers.id = tblcustomeruser.User_Id
 where tblusers.id not in 
    (select user_id from tblcustomeruser where showonstartpage = 1))

UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.3'


-- Released Version 5.3.3.13  (2015-03-17) **************************************************************************************************************** 
