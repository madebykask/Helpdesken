--BEGIN TRAN -- TMP


--update DB from 5.3.33 to 5.3.34 version

--UPDATE tblCustomerUser field length
ALTER TABLE tblCustomerUser
ALTER COLUMN CaseDepartmentFilter nvarchar(100)

-- New field in tblCaseSolutionConditionProperties
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'TableFieldStatus' and sysobjects.name = N'tblCaseSolutionConditionProperties')
   ALTER TABLE tblCaseSolutionConditionProperties ADD TableFieldStatus nvarchar(100) NULL
GO	

-- CREATE NONClustered Index for tblEntityRelationship.ParentItem_Guid
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblEntityRelationship_ParentItemGuid')
	DROP INDEX [IX_tblEntityRelationship_ParentItemGuid] ON [dbo].[tblEntityRelationship]
GO
CREATE NONCLUSTERED INDEX [IX_tblEntityRelationship_ParentItemGuid] ON [dbo].[tblEntityRelationship]
(
	[ParentItem_Guid] ASC
) ON [PRIMARY]
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'FetchDataFromApiOnExternalPage' and sysobjects.name = N'tblCustomer')
   ALTER TABLE tblCustomer ADD FetchDataFromApiOnExternalPage  bit Not null default (0)
GO	

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'RestrictUserToGroupOnExternalPage' and sysobjects.name = N'tblCustomer')
   ALTER TABLE tblCustomer ADD RestrictUserToGroupOnExternalPage  bit Not null default (0)
GO	

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'MyCasesUserGroup' and sysobjects.name = N'tblCustomer')
   ALTER TABLE tblCustomer ADD MyCasesUserGroup  bit Not null default (0)
GO	

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CaseUnlockUGPermissions' and sysobjects.name = N'tblUsers')
	BEGIN
		ALTER TABLE [tblUsers] DROP CONSTRAINT [DF_tblUsers_CaseUnlockUGPermissions]
		ALTER TABLE [tblUsers] DROP COLUMN [CaseUnlockUGPermissions]
	END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CaseUnlockPermission' and sysobjects.name = N'tblUsers')
	BEGIN
		ALTER TABLE [tblUsers] ADD [CaseUnlockPermission] int NOT NULL DEFAULT(0)
		EXEC('UPDATE [tblUsers] SET [CaseUnlockPermission] = 1 WHERE [UserGroup_Id] > 1')
	END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CustomerInExtendedSearch' and sysobjects.name = N'tblSettings')
   ALTER TABLE [tblSettings] ADD [CustomerInExtendedSearch] int NOT NULL DEFAULT(0)
GO	

-- New field in tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'M2TNewCaseMailTo' and sysobjects.name = N'tblSettings')
   ALTER TABLE tblSettings ADD M2TNewCaseMailTo int NOT NULL default (0)
-- #Make margin float instead of int
IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MarginTop' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginTop]
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ALTER COLUMN [MarginTop] float NOT NULL;
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginTop]  DEFAULT ((0)) FOR [MarginTop]
end

IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MarginBottom' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginBottom] 
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ALTER COLUMN [MarginBottom] float NOT NULL;
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginBottom]  DEFAULT ((0)) FOR [MarginBottom]
end

IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MarginLeft' and sysobjects.name = N'tblCaseDocumentTemplate')
begin

	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginLeft] 
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ALTER COLUMN [MarginLeft] float NOT NULL;
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginLeft]  DEFAULT ((0)) FOR [MarginLeft]
	
end

IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MarginRight' and sysobjects.name = N'tblCaseDocumentTemplate')
begin

	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginRight] 
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ALTER COLUMN [MarginRight] float NOT NULL;
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginRight]  DEFAULT ((0)) FOR [MarginRight]
end

-- Logotype on firstpage in header, footer
IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowAlternativeHeaderOnFirstPage' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [ShowAlternativeHeaderOnFirstPage] bit NOT NULL Default(0)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowAlternativeFooterOnFirstPage' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [ShowAlternativeFooterOnFirstPage] bit NOT NULL Default(0)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DraftHeight' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [DraftHeight] float NOT NULL Default(0)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DraftYLocation' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [DraftYLocation] float NOT NULL Default(0)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DraftRotateAngle' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [DraftRotateAngle] float NOT NULL Default(0)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HtmlViewerWidth' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [HtmlViewerWidth] int NOT NULL Default(0)
end


IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FooterHeight' and sysobjects.name = N'tblCaseDocumentTemplate')
begin

	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_FooterHeight] 
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ALTER COLUMN [FooterHeight] float NOT NULL;
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_FooterHeight]  DEFAULT ((0)) FOR [FooterHeight]
	
end

IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HeaderHeight' and sysobjects.name = N'tblCaseDocumentTemplate')
begin

	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_HeaderHeight] 
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ALTER COLUMN [HeaderHeight] float NOT NULL;
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_HeaderHeight]  DEFAULT ((0)) FOR [HeaderHeight]
	
end


IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PageNumbersUse' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP  CONSTRAINT [DF_tblCaseDocumentTemplate_PageNumbersUse] 
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] DROP COLUMN [PageNumbersUse] 
end

-- set MetaDataText to max size (AM issue)
ALTER TABLE [dbo].[tblMetaData]
ALTER COLUMN MetaDataText nvarchar(MAX) NOT NULL

GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblQuestionnaireCircularExtraEmails' AND xtype='U')
BEGIN
	CREATE TABLE [dbo].[tblQuestionnaireCircularExtraEmails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireCircular_Id] [int] NOT NULL,
	[Email] [nvarchar](100) NOT NULL
 CONSTRAINT [PK_tblQuestionnaireExtraEmail] PRIMARY KEY CLUSTERED (	[Id] ASC)
 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]
 
ALTER TABLE [dbo].[tblQuestionnaireCircularExtraEmails]  WITH NOCHECK ADD  CONSTRAINT [FK_tblQuestionnaireCircularExtraEmails_tblQuestionnaireCircular] FOREIGN KEY([QuestionnaireCircular_Id])
REFERENCES [dbo].[tblQuestionnaireCircular] ([Id])

END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'MailTemplate_Id' and sysobjects.name = N'tblQuestionnaireCircular')
   ALTER TABLE [tblQuestionnaireCircular] ADD [MailTemplate_Id] int NULL
GO	

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CaseInternalLogPermission' and sysobjects.name = N'tblUsers')
	BEGIN
		ALTER TABLE [tblUsers] ADD [CaseInternalLogPermission] int NOT NULL DEFAULT(0)
		EXEC('UPDATE [tblUsers] SET [CaseInternalLogPermission] = 1')
	END
GO

if not exists(select * from sysobjects WHERE Name = N'FK_tblCategory_tblCategory')
	ALTER TABLE [dbo].[tblCategory] ADD 
		CONSTRAINT [FK_tblCategory_tblCategory] FOREIGN KEY 
			(
				[Parent_Category_Id]
			) REFERENCES [dbo].[tblCategory] (
				[Id]
			)
GO 

/************************************************************************** CBD **************************************************************************/

RAISERROR ('Add column ComputerUserCategoryID on table tblComputerUsers', 10, 1) WITH NOWAIT
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ComputerUsersCategoryID' and sysobjects.name = N'tblComputerUsers')
BEGIN
    ALTER TABLE [dbo].[tblComputerUsers]
    ADD [ComputerUsersCategoryID] INT NULL;
END
GO	

RAISERROR('Create table tblComputerUsersCategory', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblComputerUsersCategory' AND type='U')
BEGIN
	CREATE TABLE [dbo].[tblComputerUsersCategory] (
		[ID]                        INT              IDENTITY (1, 1) NOT NULL,
		[Name]                      NVARCHAR (MAX)   NOT NULL,
		[CaseSolutionID]            INT              NULL,
		[ComputerUsersCategoryGuid] UNIQUEIDENTIFIER NOT NULL,
		[IsReadOnly]                BIT              NOT NULL,
		[CustomerID]                INT              NOT NULL,
		[ExtendedCaseFormID]        INT              NULL,
		CONSTRAINT [PK_tblComputerUsersCategory] PRIMARY KEY CLUSTERED ([ID] ASC)
	);
END
GO

RAISERROR('Create index for tblComputerUsers on Customer_Id and ComputerUsersCategoryID', 10, 1) WITH NOWAIT
IF EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_tblComputerUsers_ComputerUSersCategory')
	DROP INDEX IX_tblComputerUsers_ComputerUSersCategory ON [dbo].[tblComputerUsers]
GO
CREATE NONCLUSTERED INDEX [IX_tblComputerUsers_ComputerUSersCategory]
    ON [dbo].[tblComputerUsers]([Customer_Id] ASC, [ComputerUsersCategoryID] ASC, [Status] ASC)
    INCLUDE([Id], [UserId], [FirstName], [SurName], [Location], [Phone], [Cellphone], [Email], [UserCode], [Department_Id], [OU_Id], [CostCentre]);
GO


RAISERROR('Create table tblCase_tblCaseSection_ExtendedCaseData for connecting case section with extended data', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblCase_tblCaseSection_ExtendedCaseData' AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] (
		[Case_Id]             INT NOT NULL,
		[CaseSection_Id]      INT NOT NULL,
		[ExtendedCaseData_Id] INT NOT NULL,
		CONSTRAINT [PK_tblCase_tblCaseSection_ExtendedCaseData] PRIMARY KEY CLUSTERED ([Case_Id] ASC, [CaseSection_Id] ASC, [ExtendedCaseData_Id] ASC)
	);

END
GO

RAISERROR('Create table tblCaseSolution_tblCaseSection_ExtendedCaseForm to connect section, solution and extended case form', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblCaseSolution_tblCaseSection_ExtendedCaseForm' AND xtype='U')
BEGIN
	CREATE TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] (
		[ID]                 INT IDENTITY (1, 1) NOT NULL,
		[tblCaseSolutionID]  INT NOT NULL,
		[tblCaseSectionID]   INT NOT NULL,
		[ExtendedCaseFormID] INT NOT NULL,
		CONSTRAINT [PK_tblCaseSolution_tblCaseSection_ExtendedCaseForm] PRIMARY KEY CLUSTERED ([ID] ASC)
	);
END
GO

RAISERROR('Create table tblComputerUserCategory_ExtendedCaseForm to connect computerusercategory with extended case form. TODO: Evaluate usage, already exist in tblComputerUsersCategory', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblComputerUserCategory_ExtendedCaseForm' AND xtype='U')
BEGIN
	CREATE TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] (
		[ComputerUserCategoryID] INT NOT NULL,
		[ExtendedCaseFormID]     INT NOT NULL,
		CONSTRAINT [PK_tblComputerUserCategory_ExtendedCaseForm_1] PRIMARY KEY CLUSTERED ([ComputerUserCategoryID] ASC)
	);
END
GO


RAISERROR('Default constraint on tblComputerUsersCategory.IsReadOnly, value 0', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'DF_tblComputerUsersCategory_ReadOnly') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[tblComputerUsersCategory] 
	ADD CONSTRAINT [DF_tblComputerUsersCategory_ReadOnly] DEFAULT ((0)) FOR [IsReadOnly];
END
GO

RAISERROR('Foreign key tblCase_tblCaseSection_ExtendedCaseData.Case_Id', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCase_tblCaseSection_ExtendedCaseData_tblCase') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH NOCHECK 
	ADD CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_tblCase] FOREIGN KEY ([Case_Id]) REFERENCES [dbo].[tblCase] ([Id]);
END
GO

RAISERROR('Foreign key tblCase_tblCaseSection_ExtendedCaseData.CaseSectionID ', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCase_tblCaseSection_ExtendedCaseData_tblCaseSections') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_tblCaseSections] FOREIGN KEY ([CaseSection_Id]) REFERENCES [dbo].[tblCaseSections] ([Id]);
END
GO

RAISERROR('Foreign key tblCase_tblCaseSection_ExtendedCaseData.ExtendedCaseData_Id', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCase_tblCaseSection_ExtendedCaseData_ExtendedCaseData') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_ExtendedCaseData] FOREIGN KEY ([ExtendedCaseData_Id]) REFERENCES [dbo].[ExtendedCaseData] ([Id]);
END

RAISERROR('Foreign key tblCaseSolution_tblCaseSection_ExtendedCaseForm.tblCaseSolutionID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSolution') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSolution] FOREIGN KEY ([tblCaseSolutionID]) REFERENCES [dbo].[tblCaseSolution] ([Id]);
END

RAISERROR('Foreign key tblCaseSolution_tblCaseSection_ExtendedCaseForm.tblCaseSectionID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSections') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSections] FOREIGN KEY ([tblCaseSectionID]) REFERENCES [dbo].[tblCaseSections] ([Id]);
END

RAISERROR('Foreign key tblCaseSolution_tblCaseSection_ExtendedCaseForm.ExtendedCaseFormID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_ExtendedCaseForms') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_ExtendedCaseForms] FOREIGN KEY ([ExtendedCaseFormID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);
END
GO

RAISERROR('Foreign key tblComputerUserCategory_ExtendedCaseForm.ComputerUserCategoryID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUserCategory_ExtendedCaseForm_tblComputerUsersCategory') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_tblComputerUsersCategory] FOREIGN KEY ([ComputerUserCategoryID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);
END
GO

RAISERROR('Foreign key tblComputerUserCategory_ExtendedCaseForm.ComputerUserCategoryID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUserCategory_ExtendedCaseForm_tblComputerUsersCategory') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_tblComputerUsersCategory] FOREIGN KEY ([ComputerUserCategoryID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);
END
GO

RAISERROR('Foreign key tblComputerUserCategory_ExtendedCaseForm.ExtendedCaseFormID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUserCategory_ExtendedCaseForm_ExtendedCaseForms') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_ExtendedCaseForms] FOREIGN KEY ([ExtendedCaseFormID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);
END
GO

/* Foreign key tblComputerUsersCategory.ID TODO: Remove this one*/
/*IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUsersCategory_tblComputerUsersCategory') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_tblComputerUsersCategory] FOREIGN KEY ([ID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);
END
GO*/

RAISERROR('Foreign key tblComputerUsersCategory.ExtendedCaseFormID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUsersCategory_ExtendedCaseForms1') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_ExtendedCaseForms1] FOREIGN KEY ([ExtendedCaseFormID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);
END
GO

RAISERROR('Foreign key tblComputerUsersCategory.CustomerID ', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUsersCategory_tblCustomer') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_tblCustomer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tblCustomer] ([Id]);
END
GO

/* Foreign key tblComputerUsersCategory TODO: Remove this one */
/*IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUsersCategory_ExtendedCaseForms') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_ExtendedCaseForms] FOREIGN KEY ([ID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);
END
GO*/


RAISERROR('Foreign key tblComputerUsers.ComputerUsersCategoryID', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblComputerUsers_tblComputerUsersCategory') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblComputerUsers] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsers_tblComputerUsersCategory] FOREIGN KEY ([ComputerUsersCategoryID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);
END
GO






-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.34'

--ROLLBACK --TMP