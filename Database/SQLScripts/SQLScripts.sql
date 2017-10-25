
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
		ALTER TABLE [tblUsers] ADD [CaseUnlockPermission] bit NOT NULL DEFAULT(0)
		EXEC('UPDATE [tblUsers] SET [CaseUnlockPermission] = 1 WHERE [UserGroup_Id] > 1')
	END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CustomerInExtendedSearch' and sysobjects.name = N'tblSettings')
   ALTER TABLE [tblSettings] ADD [CustomerInExtendedSearch] int NOT NULL DEFAULT(0)
GO	

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.34'