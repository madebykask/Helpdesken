
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
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.34'