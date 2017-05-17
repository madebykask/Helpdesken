-- update DB from 5.3.31 to 5.3.32 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CaseExtraFollowers' and sysobjects.name = N'tblCaseHistory')
	begin
		ALTER TABLE [dbo].[tblCaseHistory] ADD [CaseExtraFollowers] nvarchar(max) NOT NULL default ''
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'StandardTextName' and sysobjects.name = N'tblStandardText')
	begin
		ALTER TABLE [dbo].[tblStandardText] ADD [StandardTextName] nvarchar(50) NOT NULL default ''
	end
GO

--ADD GUID
--tblCaseSettings
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseSettingsGUID' and sysobjects.name = N'tblCaseSettings')
begin
		EXECUTE  sp_executesql  "update tblCaseSettings set CaseSettingsGUID = newid() where CaseSettingsGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblCaseSettings'
					  and c.name = 'CaseSettingsGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_CaseSettingsGUID')
		begin
			Alter table tblCaseSettings
			Add constraint DF_CaseSettingsGUID default (newid()) For CaseSettingsGUID		
		end		
end
else
begin
	Alter table tblCaseSettings
	Add CaseSettingsGUID uniqueIdentifier NOT NULL CONSTRAINT DF_CaseSettingsGUID default (newid())
end
GO

--tblStandardText
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StandardTextGUID' and sysobjects.name = N'tblStandardText')
begin
		EXECUTE  sp_executesql  "update tblStandardText set StandardTextGUID = newid() where StandardTextGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblStandardText'
					  and c.name = 'StandardTextGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_StandardTextGUID')
		begin
			Alter table tblStandardText
			Add constraint DF_StandardTextGUID default (newid()) For StandardTextGUID		
		end		
end
else
begin
	Alter table tblStandardText
	Add StandardTextGUID uniqueIdentifier NOT NULL CONSTRAINT DF_StandardTextGUID default (newid())
end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'AgreedDate' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE [dbo].[tblCaseSolution] ADD [AgreedDate] DateTime NULL
	end
GO

-- New field in tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EMailFolderArchive' and sysobjects.name = N'tblSettings')
            begin
                         ALTER TABLE tblSettings ADD EMailFolderArchive nvarchar(50) NULL
            end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Filter' and sysobjects.name = N'tblCaseInvoiceSettings')
	begin
		ALTER TABLE [dbo].[tblCaseInvoiceSettings] ADD [Filter] nvarchar(50) NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
		where syscolumns.name = N'ShowOnExtPageCases' and sysobjects.name = N'tblCaseType')
	begin
		ALTER TABLE [dbo].[tblCaseType] ADD [ShowOnExtPageCases] [int] NOT NULL DEFAULT ((0))
	end
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.32'
