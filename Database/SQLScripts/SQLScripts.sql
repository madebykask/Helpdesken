--update DB from 5.3.35 to 5.3.36 version



 
-- DROP Foreign key FK_tblCaseFile_tblUser

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes AS si
    JOIN sys.objects AS so on si.object_id=so.object_id
    JOIN sys.schemas AS sc on so.schema_id=sc.schema_id
    WHERE 
        sc.name='dbo' /* Schema */
        AND so.name ='tblEMailLog' /* Table */
        AND si.name='FK_tblCaseHistory' /* Index */)
BEGIN
	RAISERROR('Create index tblEMailLog.CaseHistory_Id', 10, 1) WITH NOWAIT
	CREATE NONCLUSTERED INDEX [FK_tblCaseHistory] ON [dbo].[tblEMailLog]
	(
		[CaseHistory_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

RAISERROR('Foreign key tblProject_tblUsers_ProjectManager', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblProject_tblUsers') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblProject] WITH NOCHECK 
	ADD CONSTRAINT [FK_tblProject_tblUsers] FOREIGN KEY ([ProjectManager]) REFERENCES [dbo].[tblUsers] ([Id]);
END
GO

RAISERROR ('Update column InventoryNumber on table tblCase', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblCase')
BEGIN
	ALTER FULLTEXT INDEX ON [dbo].[tblCase] DROP ([InventoryNumber])
    ALTER TABLE [dbo].[tblCase]
	ALTER COLUMN [InventoryNumber] nvarchar(60)
	ALTER FULLTEXT INDEX ON [dbo].[tblCase] ADD ([InventoryNumber])
END

-- set NOCHECK constraint for Foreign Key FK_tblQuestionnaireCircularParticipant_tblCase
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblQuestionnaireCircularParticipant_tblCase') AND type = 'F')
BEGIN    
    ALTER TABLE [dbo].[tblQuestionnaireCircularPart] NOCHECK CONSTRAINT [FK_tblQuestionnaireCircularParticipant_tblCase]
END
GO
 
-- DROP Foreign key FK_tblCaseFile_tblUser
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCaseFile_tblUser') AND type = 'F')
BEGIN    
    ALTER TABLE [dbo].[tblCaseFile] DROP CONSTRAINT [FK_tblCaseFile_tblUser]
END
GO  



RAISERROR('Foreign key tblProject_tblUsers_ProjectManager', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblProject_tblUsers') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblProject] WITH NOCHECK 
	ADD CONSTRAINT [FK_tblProject_tblUsers] FOREIGN KEY ([ProjectManager]) REFERENCES [dbo].[tblUsers] ([Id]);
END
GO

RAISERROR ('Update column InventoryNumber on table tblCaseHistory', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE [dbo].[tblCaseHistory]
	ALTER COLUMN [InventoryNumber] nvarchar(60)
END
GO

RAISERROR ('Update column ComputerName on table tblComputer', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ComputerName' and sysobjects.name = N'tblComputer')
BEGIN
    ALTER TABLE [dbo].[tblComputer]
	ALTER COLUMN [ComputerName] nvarchar(60) NOT NULL
END
GO

RAISERROR ('Update column PrinterName on table tblPrinter', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'PrinterName' and sysobjects.name = N'tblPrinter')
BEGIN
    ALTER TABLE [dbo].[tblPrinter]
	ALTER COLUMN [PrinterName] nvarchar(60) NOT NULL
END
GO

RAISERROR ('Update column ServerName on table tblServer', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ServerName' and sysobjects.name = N'tblServer')
BEGIN
    ALTER TABLE [dbo].[tblServer]
	ALTER COLUMN [ServerName] nvarchar(60) NOT NULL
END
GO

-- AllowMoveCaseToAnyCustomer
RAISERROR ('Add column AllowMoveCaseToAnyCustomer to tblSettings', 10, 1) WITH NOWAIT
IF not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'AllowMoveCaseToAnyCustomer' and	 sysobjects.name = N'tblSettings')
BEGIN
    -- add column
    ALTER TABLE [dbo].[tblSettings] ADD AllowMoveCaseToAnyCustomer bit NOT NULL DEFAULT(0)
END

-- GDPR Privacy Access table
RAISERROR('Create table tblGDPRDataPrivacyAccess', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblGDPRDataPrivacyAccess' AND type='U')
BEGIN

    CREATE TABLE dbo.tblGDPRDataPrivacyAccess(
	    [Id] int IDENTITY(1,1) NOT NULL,
	    [User_Id] int NULL,
	    [CreatedDate] datetime NOT NULL,
	CONSTRAINT PK_tblGDPRDataPrivacyAccess PRIMARY KEY CLUSTERED (Id ASC) ON [PRIMARY]
    ) ON [PRIMARY]

    -- CreatedDate default value
    ALTER TABLE dbo.tblGDPRDataPrivacyAccess 
    ADD CONSTRAINT DF_tblGDPRDataPrivacyAccess_CreatedDate DEFAULT (getdate()) FOR CreatedDate

    -- FK: Users
    ALTER TABLE dbo.tblGDPRDataPrivacyAccess WITH NOCHECK 
    ADD CONSTRAINT FK_tblGDPRDataPrivacyAccess_tblUsers FOREIGN KEY(User_Id) REFERENCES dbo.tblUsers (Id)

END
GO

RAISERROR('Create table tblGDPROperationsAudit', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblGDPROperationsAudit' AND type='U')
BEGIN

    CREATE TABLE [dbo].[tblGDPROperationsAudit](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [User_Id] [int] NOT NULL,
	    [Operation] [nvarchar](50) NOT NULL,
	    [Parameters] [nvarchar](max) NULL,
	    [Result] [nvarchar](max) NULL,
	    [Url] [nvarchar](256) NOT NULL,
	    [Application] [nvarchar](50) NOT NULL,
	    [Success] [bit] NOT NULL,
	    [Error] [nvarchar](max) NULL,
	    [CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_tblGDPROperationsAudit_CreatedDate]  DEFAULT (getdate()),
	CONSTRAINT [PK_tblGDPROperationsAudit] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

    ALTER TABLE [dbo].[tblGDPROperationsAudit]  WITH CHECK 
    ADD CONSTRAINT [FK_tblGDPROperationsAudit_tblUsers] FOREIGN KEY([User_Id]) REFERENCES [dbo].[tblUsers] ([Id])	 

    ALTER TABLE [dbo].[tblGDPROperationsAudit] CHECK CONSTRAINT [FK_tblGDPROperationsAudit_tblUsers]

END
GO

RAISERROR('Create table tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblGDPRDataPrivacyFavorite' AND type='U')
BEGIN
   CREATE TABLE [dbo].[tblGDPRDataPrivacyFavorite](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [Name] [nvarchar](256) NOT NULL,
	    [CustomerId] [int] NOT NULL,
	    [RetentionPeriod] [int] NOT NULL,
	    [CalculateRegistrationDate] [bit] NOT NULL,
	    [RegisterDateFrom] [datetime] NOT NULL,
	    [RegisterDateTo] [datetime] NOT NULL,
	    [ClosedOnly] [bit] NOT NULL,
	    [FieldsNames] [nvarchar](1024) NOT NULL,
	    [ReplaceDataWith] [nvarchar](256) NOT NULL,
	    [ReplaceDatesWith] [datetime] NULL,
	    [RemoveCaseAttachments] [bit] NOT NULL,
	    [RemoveLogAttachments] [bit] NOT NULL,
	    CONSTRAINT [PK_tblGDPRDataPrivacyFavorite] PRIMARY KEY CLUSTERED 
	   (
		   [Id] ASC
	   ) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

--SPINT 11: 
RAISERROR('Add Customer_Id column to tblGDPROperationsAudit', 10, 1) WITH NOWAIT
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Customer_Id' and sysobjects.name = N'tblGDPROperationsAudit')
BEGIN
    ALTER TABLE tblGDPROperationsAudit
    ADD Customer_Id int NULL 
END
GO

RAISERROR('Add FK_tblGDPROperationsAudit_tblCustomer FK to tblGDPROperationsAudit', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblGDPROperationsAudit_tblCustomer') AND type = 'F')
BEGIN
    ALTER TABLE [dbo].[tblGDPROperationsAudit]  WITH CHECK 
    ADD CONSTRAINT [FK_tblGDPROperationsAudit_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END

RAISERROR('Add ReplaceEmails column to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ReplaceEmails' and sysobjects.name = N'tblGDPRDataPrivacyFavorite')
BEGIN
    ALTER TABLE tblGDPRDataPrivacyFavorite
    ADD ReplaceEmails bit NOT NULL DEFAULT(1)
END
GO

RAISERROR('Add InvoiceChargeType column to tblDepartment', 10, 1) WITH NOWAIT
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InvoiceChargeType' and sysobjects.name = N'tblDepartment')
BEGIN
    ALTER TABLE [dbo].[tblDepartment]
    ADD InvoiceChargeType INT NOT NULL DEFAULT(0)
END
GO

RAISERROR('Add Changed/Created columns to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedByUser_Id' and sysobjects.name = N'tblGDPRDataPrivacyFavorite')
BEGIN
    
    ALTER TABLE tblGDPRDataPrivacyFavorite
    ADD [CreatedDate] [datetime] NULL

    ALTER TABLE tblGDPRDataPrivacyFavorite
    ADD [CreatedByUser_Id] [int] NULL
          
    ALTER TABLE tblGDPRDataPrivacyFavorite
    ADD [ChangedDate] [datetime] NULL	
          
    ALTER TABLE tblGDPRDataPrivacyFavorite
    ADD [ChangedByUser_Id] [int] NULL    

    ALTER TABLE [dbo].[tblGDPRDataPrivacyFavorite] ADD  CONSTRAINT [DF_tblGDPRDataPrivacyFavorite_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
    ALTER TABLE [dbo].[tblGDPRDataPrivacyFavorite] ADD  CONSTRAINT [DF_tblGDPRDataPrivacyFavorite_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
END
GO


-- Search fix
-- .176
RAISERROR('Creating index IX_tblCase_Customer', 10, 1) WITH NOWAIT
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer')
	DROP INDEX [IX_tblCase_Customer] ON [dbo].[tblCase]
GO
CREATE NONCLUSTERED INDEX [IX_tblCase_Customer] ON [dbo].[tblCase]
(
	[Customer_Id] ASC
)
INCLUDE ( 	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

RAISERROR('Creating index IX_tblCase_Customer_Department', 10, 1) WITH NOWAIT
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Department')
	DROP INDEX [IX_tblCase_Customer_Department] ON [dbo].[tblCase]
GO
CREATE NONCLUSTERED INDEX [IX_tblCase_Customer_Department] ON [dbo].[tblCase]
(
	[Customer_Id] ASC
)
INCLUDE ( 	[Id],
	[Department_Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


RAISERROR('Creating index IX_tblCase_Customer_Id', 10, 1) WITH NOWAIT
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id')
	DROP INDEX [IX_tblCase_Customer_Id] ON [dbo].[tblCase]
GO
CREATE NONCLUSTERED INDEX [IX_tblCase_Customer_Id] ON [dbo].[tblCase]
(
	[Customer_Id] ASC,
	[FinishingDate] ASC,
	[WorkingGroup_Id] ASC,
	[Deleted] ASC,
	[Department_Id] ASC,
	[Id] ASC,
	[RegTime] ASC
)
INCLUDE ( 	[Casenumber],
	[Place],
	[UserCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	
RAISERROR('Creating index IX_tblCase_Id_DepartmentId', 10, 1) WITH NOWAIT
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Id_DepartmentId')
	DROP INDEX [IX_tblCase_Id_DepartmentId] ON [dbo].[tblCase]
GO	
/****** Object:  Index [IX_tblCase_Id_DepartmentId]    Script Date: 2018-03-20 12:46:29 ******/
CREATE NONCLUSTERED INDEX [IX_tblCase_Id_DepartmentId] ON [dbo].[tblCase]
(
	[Id] ASC,
	[Department_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

RAISERROR('Creating index IX_tblCustomer_new', 10, 1) WITH NOWAIT
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCustomer_new')
	DROP INDEX [IX_tblCustomer_new] ON [dbo].[tblCustomer]
GO	
/****** Object:  Index [IX_tblCustomer_new]    Script Date: 2018-03-20 12:42:18 ******/
CREATE NONCLUSTERED INDEX [IX_tblCustomer_new] ON [dbo].[tblCustomer]
(
	[Id] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
GO

RAISERROR('Creating index IX_tblDepartment_new', 10, 1) WITH NOWAIT
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblDepartment_new')
	DROP INDEX [IX_tblDepartment_new] ON [dbo].[tblDepartment]
GO	
/****** Object:  Index [IX_tblDepartment_new]    Script Date: 2018-03-20 13:06:57 ******/
CREATE NONCLUSTERED INDEX [IX_tblDepartment_new] ON [dbo].[tblDepartment]
(
	[Id] ASC,
	[DepartmentId] ASC,
	[Department] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.36'
--ROLLBACK --TMP
