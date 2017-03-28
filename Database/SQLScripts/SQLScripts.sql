-- update DB from 5.3.30 to 5.3.31 version

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblUsers_UserId_Status')
	CREATE INDEX IX_tblUsers_UserId_Status ON tblUsers (UserId, Status)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id_User_Id')
	CREATE INDEX IX_tblCase_Customer_Id_User_Id ON tblCase (Customer_Id, User_Id)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblUsers_UserId_Password_Status')
	CREATE INDEX IX_tblUsers_UserId_Password_Status ON tblUsers (UserId, Password, Status)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblProductArea_Customer_Id_Parent_ProductArea_Id')
	CREATE INDEX IX_tblProductArea_Customer_Id_Parent_ProductArea_Id ON tblProductArea (Customer_Id, Parent_ProductArea_Id)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCustomerUser_UserId')
	CREATE INDEX IX_tblCustomerUser_UserId ON tblCustomerUser (User_Id)
GO

UPDATE [dbo].[tblOrderType] SET [CaptionUserInfo] = N'Användare' WHERE [CaptionUserInfo] is NULL
UPDATE [dbo].[tblOrderType] SET [CaptionOrdererInfo] = N'Beställare' WHERE [CaptionOrdererInfo] is NULL
UPDATE [dbo].[tblOrderType] SET [CaptionReceiverInfo] = N'Kontakt' WHERE [CaptionReceiverInfo] is NULL
GO

--New fields in tblCaseFieldSettings
INSERT INTO tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'Project', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'Project')

INSERT INTO tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'Problem', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'Problem')

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrdererID' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ALTER COLUMN [OrdererID] [nvarchar](600) NULL
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrdererID' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ALTER COLUMN [OrdererID] [nvarchar](600) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MultiValue' and sysobjects.name = N'tblOrderFieldSettings')
	ALTER TABLE [dbo].[tblOrderFieldSettings] ADD [MultiValue] [bit] NOT NULL CONSTRAINT [DF_tblOrderFieldSettings_MultiValue]  DEFAULT ((0))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesInitiator' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesInitiator] [bit] NOT NULL DEFAULT ((0))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesRegistrator' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesRegistrator] [bit] NOT NULL DEFAULT ((1))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesFollower' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesFollower] [bit] NOT NULL DEFAULT ((0))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesRegarding' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesRegarding] [bit] NOT NULL DEFAULT ((0))
GO

--tblMail2Ticket
ALTER TABLE tblMail2Ticket ALTER COLUMN EMailAddress NVARCHAR(200) NOT NULL

--ADD GUID
--tblAccountActivity
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountActivityGUID' and sysobjects.name = N'tblAccountActivity')
begin
		EXECUTE  sp_executesql  "update tblAccountActivity set AccountActivityGUID = newid() where AccountActivityGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblAccountActivity'
					  and c.name = 'AccountActivityGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_AccountActivityGUID')
		begin
			Alter table tblAccountActivity
			Add constraint DF_AccountActivityGUID default (newid()) For AccountActivityGUID		
		end		
end
else
begin
	Alter table tblAccountActivity
	Add AccountActivityGUID uniqueIdentifier NOT NULL CONSTRAINT DF_AccountActivityGUID default (newid())
end
GO

--tblDocumentCategory
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DocumentCategoryGUID' and sysobjects.name = N'tblDocumentCategory')
begin
		EXECUTE  sp_executesql  "update tblDocumentCategory set DocumentCategoryGUID = newid() where DocumentCategoryGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblDocumentCategory'
					  and c.name = 'DocumentCategoryGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_DocumentCategoryGUID')
		begin
			Alter table tblDocumentCategory
			Add constraint DF_DocumentCategoryGUID default (newid()) For DocumentCategoryGUID		
		end		
end
else
begin
	Alter table tblDocumentCategory
	Add DocumentCategoryGUID uniqueIdentifier NOT NULL CONSTRAINT DF_DocumentCategoryGUID default (newid())
end
GO

--tblCaseSolution
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseSolutionGUID' and sysobjects.name = N'tblCaseSolution')
begin
		EXECUTE  sp_executesql  "update tblCaseSolution set CaseSolutionGUID = newid() where CaseSolutionGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblCaseSolution'
					  and c.name = 'CaseSolutionGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_CaseSolutionGUID')
		begin
			Alter table tblCaseSolution
			Add constraint DF_CaseSolutionGUID default (newid()) For CaseSolutionGUID		
		end		
end
else
begin
	Alter table tblCaseSolution
	Add CaseSolutionGUID uniqueIdentifier NOT NULL CONSTRAINT DF_CaseSolutionGUID default (newid())
end
GO

--tblCaseSolutionCategory
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseSolutionCategoryGUID' and sysobjects.name = N'tblCaseSolutionCategory')
begin
		EXECUTE  sp_executesql  "update tblCaseSolutionCategory set CaseSolutionCategoryGUID = newid() where CaseSolutionCategoryGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblCaseSolutionCategory'
					  and c.name = 'CaseSolutionCategoryGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_CaseSolutionCategoryGUID')
		begin
			Alter table tblCaseSolutionCategory
			Add constraint DF_CaseSolutionCategoryGUID default (newid()) For CaseSolutionCategoryGUID		
		end		
end
else
begin
	Alter table tblCaseSolutionCategory
	Add CaseSolutionCategoryGUID uniqueIdentifier NOT NULL CONSTRAINT DF_CaseSolutionCategoryGUID default (newid())
end
GO

--tblProblem
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProblemGUID' and sysobjects.name = N'tblProblem')
begin
		EXECUTE  sp_executesql  "update tblProblem set ProblemGUID = newid() where ProblemGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblProblem'
					  and c.name = 'ProblemGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_ProblemGUID')
		begin
			Alter table tblProblem
			Add constraint DF_ProblemGUID default (newid()) For ProblemGUID		
		end		
end
else
begin
	Alter table tblProblem
	Add ProblemGUID uniqueIdentifier NOT NULL CONSTRAINT DF_ProblemGUID default (newid())
end
GO

--tblProject
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProjectGUID' and sysobjects.name = N'tblProject')
begin
		EXECUTE  sp_executesql  "update tblProject set ProjectGUID = newid() where ProjectGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblProject'
					  and c.name = 'ProjectGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_ProjectGUID')
		begin
			Alter table tblProject
			Add constraint DF_ProjectGUID default (newid()) For ProjectGUID		
		end		
end
else
begin
	Alter table tblProject
	Add ProjectGUID uniqueIdentifier NOT NULL CONSTRAINT DF_ProjectGUID default (newid())
end
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderTypeDescription' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ALTER COLUMN [OrderTypeDescription] nvarchar(1500) NULL
GO

if exists(select * from sysobjects WHERE Name = N'tblOrderType')
	begin
	Alter table [tblOrderType] alter column [CaptionUserInfo] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionOrdererInfo] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionReceiverInfo] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionGeneral] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionOrder] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionOrderInfo] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionDeliveryInfo] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionProgram] [nvarchar](50)
	Alter table [tblOrderType] alter column [CaptionOther] [nvarchar](50)
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ServerNameIP' and sysobjects.name = N'tblLogProgram')
	begin
		ALTER TABLE tblLogProgram ADD ServerNameIP Nvarchar(100) NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'NumberOfUsers' and sysobjects.name = N'tblLogProgram')
	begin
		ALTER TABLE tblLogProgram ADD NumberOfUsers int NULL
	end
GO


--tblCase
ALTER TABLE tblCase ALTER COLUMN UserCode NVARCHAR(50)

--tblCaseIsAbout
ALTER TABLE tblCaseIsAbout ALTER COLUMN UserCode NVARCHAR(50)

--tblCaseHistory
ALTER TABLE tblCaseHistory ALTER COLUMN UserCode NVARCHAR(50)
ALTER TABLE tblCaseHistory ALTER COLUMN IsAbout_UserCode NVARCHAR(50)

--tblCaseSolution
ALTER TABLE tblCaseSolution ALTER COLUMN UserCode NVARCHAR(50)
ALTER TABLE tblCaseSolution ALTER COLUMN IsAbout_UserCode NVARCHAR(50)

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'SynchronizedDate' and sysobjects.name = N'tblRegion')
	begin
		ALTER TABLE tblRegion ADD SynchronizedDate DateTime NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'SynchronizedDate' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD SynchronizedDate DateTime NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'SynchronizedDate' and sysobjects.name = N'tblOU')
	begin
		ALTER TABLE tblOU ADD SynchronizedDate DateTime NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Export' and sysobjects.name = N'tblAccount')
	begin
		ALTER TABLE tblAccount ADD Export int NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'BulletinBoardWGRestriction' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD BulletinBoardWGRestriction int NOT NULL Default(0)
	end
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.31'
