
IF COL_LENGTH('dbo.tblDocument','ShowOnStartPage') IS NULL
BEGIN
	ALTER TABLE dbo.tblDocument
	ADD [ShowOnStartPage] INT NULL

	ALTER TABLE [dbo].[tblDocument] 
	ADD CONSTRAINT [DF_tblDocument_ShowOnStartPage]  
	DEFAULT ((0)) FOR [ShowOnStartPage]
END

IF OBJECT_ID(N'FK_tblCase_tblCausingPart', 'F') IS NOT NULL 
    ALTER TABLE dbo.tblCase DROP CONSTRAINT FK_tblCase_tblCausingPart

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID (N'tblCausingPart', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblCausingPart]

CREATE TABLE [dbo].[tblCausingPart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Parent_CausingPart_Id] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[Status] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblCausingPart] ADD  CONSTRAINT [DF_tblCausingPart_Status]  DEFAULT ((1)) FOR [Status]
GO

ALTER TABLE [dbo].[tblCausingPart] ADD  CONSTRAINT [DF_tblCausingPart_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[tblCausingPart] ADD  CONSTRAINT [DF_tblCausingPart_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
GO

ALTER TABLE [dbo].[tblCausingPart]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCausingPart_tblCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tblCustomer] ([Id])
GO


IF COL_LENGTH('dbo.tblCase','CausingPartId') IS NULL
BEGIN
	ALTER TABLE dbo.tblCase
	ADD [CausingPartId] INT NULL

	ALTER TABLE dbo.tblCase
	ADD CONSTRAINT FK_tblCase_tblCausingPart
	FOREIGN KEY (CausingPartId)
	REFERENCES dbo.tblCausingPart(Id)
END

DELETE FROM dbo.tblCausingPart

DECLARE @customerId INT
DECLARE @customer_cursor CURSOR 
SET @customer_cursor = CURSOR FAST_FORWARD FOR SELECT Id FROM dbo.tblCustomer
OPEN @customer_cursor
FETCH NEXT FROM @customer_cursor INTO @customerId
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @id INT
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'Forwarder', 'Forwarder')
	SET @id = SCOPE_IDENTITY();
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Agility Road', 'Agility Road')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Alwex', 'Alwex')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Schenker', 'Schenker')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'DFDS', 'DFDS')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'DGF Air', 'DGF Air')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'DGF Ocean', 'DGF Ocean')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'DGF Road (including DHL Express)', 'DGF Road (including DHL Express)')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'DHL Paket/Swednet', 'DHL Paket/Swednet')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'DSV', 'DSV')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Gefco', 'Gefco')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Gondrand Stella', 'Gondrand Stella')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Gopet Trans', 'Gopet Trans')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Green Cargo', 'Green Cargo')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'KAJ', 'KAJ')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Karlsson', 'Karlsson')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Lehnkering', 'Lehnkering')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'LKW Walter', 'LKW Walter')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'OOCL Logistics', 'OOCL Logistics')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'PEKAES', 'PEKAES')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, @id, 'Itella', 'Itella')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'IMS Internal', 'IMS Internal')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'INTER IKEA Systems', 'INTER IKEA Systems')

	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'Supplier', 'Supplier')
	SET @id = SCOPE_IDENTITY();
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) 
	SELECT @customerId, @id, Supplier, Supplier 
	FROM dbo.tblSupplier

	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'User', 'User')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'WH400', 'WH400')
	INSERT INTO dbo.tblCausingPart ([CustomerId], [Parent_CausingPart_Id], [Name], [Description]) VALUES (@customerId, NULL, 'WH650', 'WH650')

	FETCH NEXT FROM @customer_cursor INTO @customerId
END

CLOSE @customer_cursor
DEALLOCATE @customer_cursor


IF OBJECT_ID (N'tblUsers_tblModule', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblUsers_tblModule]

IF OBJECT_ID (N'tblModule', N'U') IS NOT NULL 
	DROP TABLE [dbo].[tblModule]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DELETE FROM dbo.tblModule 
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Customer', 'Customer')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Problem', 'Problem')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Statistics', 'Statistics')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Bulletin Board', 'Bulletin Board')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Calendar', 'Calendar')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('FAQ', 'FAQ')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Operational Log', 'Operational Log')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Daily Report', 'Daily Report')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Quick Link', 'Quick Link')
INSERT INTO dbo.tblModule ([Name], [Description]) VALUES ('Document', 'Document')

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUsers_tblModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[Module_Id] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[isVisible] [bit] NOT NULL,
	[NumberOfRows] [int] NULL,
 CONSTRAINT [PK_tblUsers_tblModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblUsers_tblModule] ADD  CONSTRAINT [DF_tblUser_tblModule_Position]  DEFAULT ((0)) FOR [Position]
GO

ALTER TABLE [dbo].[tblUsers_tblModule] ADD  CONSTRAINT [DF_tblUser_tblModule_isVisible]  DEFAULT ((1)) FOR [isVisible]
GO

ALTER TABLE [dbo].[tblUsers_tblModule] ADD  CONSTRAINT [DF_tblUser_tblModule_NumberOfRows]  DEFAULT ((3)) FOR [NumberOfRows]
GO

ALTER TABLE [dbo].[tblUsers_tblModule]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_tblModule_tblModule] FOREIGN KEY([Module_Id])
REFERENCES [dbo].[tblModule] ([Id])
GO

ALTER TABLE [dbo].[tblUsers_tblModule] CHECK CONSTRAINT [FK_tblUser_tblModule_tblModule]
GO

ALTER TABLE [dbo].[tblUsers_tblModule]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_tblModule_tblUsers] FOREIGN KEY([User_Id])
REFERENCES [dbo].[tblUsers] ([Id])
GO

ALTER TABLE [dbo].[tblUsers_tblModule] CHECK CONSTRAINT [FK_tblUser_tblModule_tblUsers]
GO

DECLARE @fieldName NVARCHAR(20)
SET @fieldName = 'CausingPart'
DECLARE @fieldText NVARCHAR(100)
SET @fieldText = 'Causing Part'

DELETE FROM dbo.tblCaseFieldSettings_tblLang WHERE Label = @fieldText

DELETE FROM dbo.tblCaseFieldSettings WHERE CaseField = @fieldName

INSERT INTO dbo.tblCaseFieldSettings ([Customer_Id], [CaseField])
SELECT Id, @fieldName FROM dbo.tblCustomer

DECLARE @languageId INT
DECLARE @language_cursor CURSOR 
SET @language_cursor = CURSOR FAST_FORWARD FOR SELECT Id FROM dbo.tblLanguage
OPEN @language_cursor
FETCH NEXT FROM @language_cursor INTO @languageId
WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO dbo.tblCaseFieldSettings_tblLang ([CaseFieldSettings_Id], [Language_Id], [Label])
	SELECT Id, @languageId, @fieldText FROM dbo.tblCaseFieldSettings
	WHERE CaseField = @fieldName

	FETCH NEXT FROM @language_cursor INTO @languageId
END

CLOSE @language_cursor
DEALLOCATE @language_cursor




