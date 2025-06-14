
IF OBJECT_ID(N'FK_tblCase_tblCausingPart', 'F') IS NOT NULL 
    ALTER TABLE dbo.tblCase DROP CONSTRAINT FK_tblCase_tblCausingPart
    
GO

IF OBJECT_ID(N'FK_tblCaseHistory_tblCausingPart', 'F') IS NOT NULL 
    ALTER TABLE dbo.tblCaseHistory DROP CONSTRAINT FK_tblCaseHistory_tblCausingPart

GO

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

GO

IF OBJECT_ID(N'FK_tblCase_tblCausingPart', 'F') IS NULL 
BEGIN
    ALTER TABLE dbo.tblCase
	ADD CONSTRAINT FK_tblCase_tblCausingPart
	FOREIGN KEY (CausingPartId)
	REFERENCES dbo.tblCausingPart(Id)
END

GO

IF OBJECT_ID(N'FK_tblCaseHistory_tblCausingPart', 'F') IS NULL 
BEGIN
    ALTER TABLE dbo.tblCaseHistory ADD  CONSTRAINT FK_tblCaseHistory_tblCausingPart
    FOREIGN KEY([CausingPartId])
    REFERENCES [dbo].[tblCausingPart] ([Id])
END

GO

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




