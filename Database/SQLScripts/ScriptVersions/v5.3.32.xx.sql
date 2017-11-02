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
		ALTER TABLE [dbo].[tblCaseType] ADD [ShowOnExtPageCases] [int] NOT NULL DEFAULT ((1))
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
		where syscolumns.name = N'ShowOnExtPageCases' and sysobjects.name = N'tblProductArea')
	begin
		ALTER TABLE [dbo].[tblProductArea] ADD [ShowOnExtPageCases] [int] NOT NULL DEFAULT ((1))
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SortOrder' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE [dbo].[tblCaseSolution] ADD [SortOrder] [int] NOT NULL DEFAULT ((0))
GO

/* Add table for CaseSolution Conditions */
if not exists(select * from sysobjects WHERE Name = N'tblCaseSolutionCondition')
begin
	
	CREATE TABLE [dbo].[tblCaseSolutionCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseSolutionConditionGUID] [uniqueidentifier] NULL,
		[CaseSolution_Id] [int] NULL,
		[Property_Name] [nvarchar](100) NULL,
		[Values] [nvarchar](max) NULL,
		[Description] [nvarchar](200) NULL,
		[Status] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_tblCaseSolutionCondition] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseSolutionCondition] ADD  CONSTRAINT [DF_tblCaseSolutionCondition_CaseSolutionConditionGUID]  DEFAULT (newid()) FOR [CaseSolutionConditionGUID]

	ALTER TABLE [dbo].[tblCaseSolutionCondition] ADD  CONSTRAINT [DF_tblCaseSolutionCondition_Status]  DEFAULT ((0)) FOR [Status]

	ALTER TABLE [dbo].[tblCaseSolutionCondition] ADD  CONSTRAINT [DF_tblCaseSolutionCondition_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

	ALTER TABLE [dbo].[tblCaseSolutionCondition] ADD  CONSTRAINT [DF_tblCaseSolutionCondition_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

	ALTER TABLE [dbo].[tblCaseSolutionCondition]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolutionCondition_tblCaseSolution] FOREIGN KEY([CaseSolution_Id])
	REFERENCES [dbo].[tblCaseSolution] ([Id])

	ALTER TABLE [dbo].[tblCaseSolutionCondition] CHECK CONSTRAINT [FK_tblCaseSolutionCondition_tblCaseSolution]

end

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'[Values]' and sysobjects.name = N'tblCaseSolutionCondition')
begin
	ALTER TABLE [dbo].[tblCaseSolutionCondition] ALTER COLUMN [Values] [nvarchar](max) NULL
end
GO

/* ADD LOG */

if not exists(select * from sysobjects WHERE Name = N'tblCaseSolutionConditionLog')
begin

	CREATE TABLE [dbo].[tblCaseSolutionConditionLog](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseSoluionCondition_Id] [int] NOT NULL,
		[CaseSolutionConditionGUID] [uniqueidentifier] NULL,
		[CaseSolution_Id] [int] NULL,
		[Property_Name] [nvarchar](100) NULL,
		[Values] [nvarchar](max) NULL,
		[Description] [nvarchar](200) NULL,
		[Status] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_tblCaseSolutionConditionLog] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	
	ALTER TABLE [dbo].[tblCaseSolutionConditionLog] ADD  CONSTRAINT [DF_tblCaseSolutionConditionLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
	
	ALTER TABLE [dbo].[tblCaseSolutionConditionLog] ADD  CONSTRAINT [DF_tblCaseSolutionConditionLog_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
	
	ALTER TABLE [dbo].[tblCaseSolutionConditionLog]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolutionConditionLog_tblCaseSolution] FOREIGN KEY([CaseSolution_Id])
	REFERENCES [dbo].[tblCaseSolution] ([Id])
	
	ALTER TABLE [dbo].[tblCaseSolutionConditionLog] CHECK CONSTRAINT [FK_tblCaseSolutionConditionLog_tblCaseSolution]
	
end


/* END TABLE CASE SOLUTION CONDITIONS */


/* ADD TABLES FOR META DATA */
if not exists(select * from sysobjects WHERE Name = N'tblEntityInfo')
begin
	CREATE TABLE [dbo].[tblEntityInfo](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[EntityGuid] [uniqueidentifier] NOT NULL,
		[EntityName] [nvarchar](50) NOT NULL,
		[EntityType] [nvarchar](50) NOT NULL,
		[EntityDescription] [nvarchar](3000) NULL,
		[Translate] [bit] NOT NULL,
	 CONSTRAINT [PK_tblEntityInfo] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblEntityInfo] ADD  CONSTRAINT [DF_tblEntityInfo_EntityGuid]  DEFAULT (newid()) FOR [EntityGuid]

	ALTER TABLE [dbo].[tblEntityInfo] ADD  DEFAULT ((0)) FOR [Translate]

end
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'IX_tblEntityInfo') 
	BEGIN

	/****** Object:  Index [IX_tblEntityInfo]    Script Date: 2017-05-23 14:27:54 ******/
	CREATE UNIQUE NONCLUSTERED INDEX [IX_tblEntityInfo] ON [dbo].[tblEntityInfo]
	(
		[EntityGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END
GO

IF COL_LENGTH('tblEntityInfo','Translate') IS NULL
BEGIN
	  ALTER TABLE [tblEntityInfo] ADD [Translate] bit NOT NULL default(0)
END
GO

if not exists(select * from sysobjects WHERE Name = N'tblMetaData')
begin
	CREATE TABLE [dbo].[tblMetaData](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Customer_Id] [int] NOT NULL,
		[MetaDataGuid] [uniqueidentifier] NOT NULL,
		[EntityInfo_Guid] [uniqueidentifier] NOT NULL,
		[MetaDataCode] [nvarchar](100) NOT NULL,
		[MetaDataText] [nvarchar](250) NOT NULL,		
		[ExternalId] [int] NULL,
		[Status] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ChangedDate] [datetime] NULL,
		[SynchronizedDate] [datetime] NULL,
	 CONSTRAINT [PK_tblMetaData] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	ALTER TABLE [dbo].[tblMetaData]  WITH CHECK ADD  CONSTRAINT [FK_tblMetaData_tblCustomer] FOREIGN KEY([Customer_Id])
	REFERENCES [dbo].[tblCustomer] ([Id])

	ALTER TABLE [dbo].[tblMetaData] CHECK CONSTRAINT [FK_tblMetaData_tblCustomer]

	ALTER TABLE [dbo].[tblMetaData]  WITH CHECK ADD  CONSTRAINT [FK_tblMetaData_tblEntityInfo] FOREIGN KEY([EntityInfo_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])
	ON UPDATE CASCADE

	ALTER TABLE [dbo].[tblMetaData] CHECK CONSTRAINT [FK_tblMetaData_tblEntityInfo]

End
Go


if not exists(select * from sysobjects WHERE Name = N'tblEntityRelationship')
begin
	CREATE TABLE [dbo].[tblEntityRelationship](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ParentEntity_Guid] [uniqueidentifier] NOT NULL,
		[ChildEntity_Guid] [uniqueidentifier] NOT NULL,
		[ParentItem_Guid] [uniqueidentifier] NOT NULL,
		[ChildItem_Guid] [uniqueidentifier] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ChangedDate] [datetime] NULL,
		[SynchronizedDate] [datetime] NULL,
	 CONSTRAINT [PK_tblEntityRelationship] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	
	ALTER TABLE [dbo].[tblEntityRelationship]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityRelationship_tblEntityInfo] FOREIGN KEY([ParentEntity_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])

	ALTER TABLE [dbo].[tblEntityRelationship] CHECK CONSTRAINT [FK_tblEntityRelationship_tblEntityInfo]

	ALTER TABLE [dbo].[tblEntityRelationship]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityRelationship_tblEntityInfo1] FOREIGN KEY([ChildEntity_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])

	ALTER TABLE [dbo].[tblEntityRelationship] CHECK CONSTRAINT [FK_tblEntityRelationship_tblEntityInfo1]
End
Go


if not exists(select * from sysobjects WHERE Name = N'tblEntityDefault')
begin
	CREATE TABLE [dbo].[tblEntityDefault](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ParentEntity_Guid] [uniqueidentifier] NOT NULL,
		[ParentItem_Guid] [uniqueidentifier] NOT NULL,
		[DefaultEntity_Guid] [uniqueidentifier] NOT NULL,
		[DefaultItem_Guid] [uniqueidentifier] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ChangedDate] [datetime] NULL,
		[SynchronizedDate] [datetime] NULL,
	 CONSTRAINT [PK_tblMetaDataDefaults] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblEntityDefault]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityDefault_tblEntityInfo] FOREIGN KEY([ParentEntity_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])

	ALTER TABLE [dbo].[tblEntityDefault] CHECK CONSTRAINT [FK_tblEntityDefault_tblEntityInfo]

	ALTER TABLE [dbo].[tblEntityDefault]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityDefault_tblEntityInfo1] FOREIGN KEY([DefaultEntity_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])

	ALTER TABLE [dbo].[tblEntityDefault] CHECK CONSTRAINT [FK_tblEntityDefault_tblEntityInfo1]
End
GO


if not exists(select * from sysobjects WHERE Name = N'tblEntityAttribute')
begin
	CREATE TABLE [dbo].[tblEntityAttribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentEntity_Guid] [uniqueidentifier] NOT NULL,
	[ParentItem_Guid] [uniqueidentifier] NOT NULL,
	[AttributeEntity_Guid] [uniqueidentifier] NOT NULL,
	[Attribute_Guid] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NULL,
	[SynchronizedDate] [datetime] NULL,
		CONSTRAINT [PK_tblEntityAttribute] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblEntityAttribute]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityAttribute_tblEntityInfo] FOREIGN KEY([ParentEntity_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])

	ALTER TABLE [dbo].[tblEntityAttribute] CHECK CONSTRAINT [FK_tblEntityAttribute_tblEntityInfo]

	ALTER TABLE [dbo].[tblEntityAttribute]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityAttribute_tblEntityInfo1] FOREIGN KEY([AttributeEntity_Guid])
	REFERENCES [dbo].[tblEntityInfo] ([EntityGuid])

	ALTER TABLE [dbo].[tblEntityAttribute] CHECK CONSTRAINT [FK_tblEntityAttribute_tblEntityInfo1]

end
GO

/* ADD TABLES FOR META DATA */


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Parent_Category_Id' and sysobjects.name = N'tblCategory')
	ALTER TABLE [dbo].[tblCategory] ADD [Parent_Category_Id] int null
GO


--tblPriority - make sure it does not allow null and add newid()
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PriorityGUID' and sysobjects.name = N'tblPriority')
begin
		EXECUTE  sp_executesql  "update tblPriority set PriorityGUID = newid() where PriorityGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblPriority'
					  and c.name = 'PriorityGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_PriorityGUID')
		begin
			Alter table tblPriority 
			Add constraint DF_PriorityGUID default (newid()) For PriorityGUID		
		end		

		Alter table tblPriority 
		ALTER COLUMN [PriorityGUID] uniqueIdentifier NOT NULL
end
else
begin
	Alter table tblPriority
	Add PriorityGUID uniqueIdentifier NOT NULL CONSTRAINT DF_PriorityGUID default (newid())
end
GO

--tblStatus - make sure it does not allow null and add newid()
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StatusGUID' and sysobjects.name = N'tblStatus')
begin
		EXECUTE  sp_executesql  "update tblStatus set StatusGUID = newid() where StatusGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblStatus'
					  and c.name = 'StatusGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_StatusGUID')
		begin
			Alter table tblStatus 
			Add constraint DF_StatusGUID default (newid()) For StatusGUID		
		end		

		Alter table tblStatus 
		ALTER COLUMN [StatusGUID] uniqueIdentifier NOT NULL

end
else
begin
	Alter table tblStatus
	Add StatusGUID uniqueIdentifier NOT NULL CONSTRAINT DF_StatusGUID default (newid())
end
GO
 

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CaseSolution_Id' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE [dbo].[tblCase] ADD [CaseSolution_Id] int NULL

		ALTER TABLE [dbo].[tblCase]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseSolution_tblCase] FOREIGN KEY([CaseSolution_Id])
		REFERENCES [dbo].[tblCaseSolution] ([Id])

	end
GO



	
	
--IX_tblLog_Case_Id:
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblLog_Case_Id')
	DROP INDEX [IX_tblLog_Case_Id] ON [dbo].[tblLog]
GO
CREATE NONCLUSTERED INDEX [IX_tblLog_Case_Id] ON [dbo].[tblLog]
(
	[Case_Id] ASC
)
GO

-- IX_tblFormFieldValue_Case_Id
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblFormFieldValue_Case_Id')
	DROP INDEX [IX_tblFormFieldValue_Case_Id] ON [dbo].[tblFormFieldValue]
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblFormFieldValue_Case_Id')
CREATE NONCLUSTERED INDEX [IX_tblFormFieldValue_Case_Id] ON [dbo].[tblFormFieldValue]
(
	[Case_Id] ASC
)
GO

-- IX_tblCase_Customer_Id
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id')
	DROP INDEX [IX_tblCase_Customer_Id] ON [dbo].[tblCase]
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id')
	CREATE NONCLUSTERED INDEX [IX_tblCase_Customer_Id] ON [dbo].[tblCase]
	(
		[Customer_Id] ASC,
		[Deleted] ASC,
		[FinishingDate] ASC
	)
	INCLUDE ([Casenumber]) 
GO




/* ADD Language Columns to Region, Department and ComputerUsers */
DECLARE @addlng bit=0
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageId' and sysobjects.name = N'tblRegion')
	begin
		SET @addlng=1
	end

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageId' and sysobjects.name = N'tblRegion')
BEGIN
                      ALTER TABLE tblRegion 
                      ADD LanguageId int NULL
                      DEFAULT 0
END                    
if (@addlng = 1)
BEGIN
	execute	('update tblRegion set languageid=0')
END
GO

DECLARE @addlng bit = 0
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageId' and sysobjects.name = N'tblDepartment')
	BEGIN
		SET @addlng=1
	END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageId' and sysobjects.name = N'tblDepartment')
BEGIN
                      ALTER TABLE tblDepartment
                      ADD LanguageId int NULL
                      DEFAULT 0
           
END

if (@addlng=1)
BEGIN
	execute	('update tblDepartment set languageid=0')
END
GO

DECLARE @addlng bit = 0
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageId' and sysobjects.name = N'tblComputerUsers')
	BEGIN
		SET @addlng=1
	END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageId' and sysobjects.name = N'tblComputerUsers')
BEGIN
                      ALTER TABLE tblComputerUsers
                      ADD LanguageId int NULL
                      DEFAULT 0
                      
END					
if @addlng=1
BEGIN
    execute	('update tblComputerUsers set languageid=0')	
END
GO

INSERT INTO [dbo].[tblComputerUserFieldSettings]
			   ([Customer_Id]
			   ,[ComputerUserField]
			   ,[Show]
			   ,[Required]
			   ,[MinLength]
			   ,[ShowInList]
			   ,[LDAPAttribute])
		SELECT  DISTINCT
				Customer_Id,
				'Language_Id',
				1,
				0,
				0,
				1,
				''
		FROM tblComputerUserFieldSettings	
		WHERE Customer_Id IS NOT NULL AND Customer_Id NOT IN (SELECT Customer_Id From tblComputerUserFieldSettings WHERE Customer_Id IS NOT NULL AND ComputerUserField='Language_Id')
			
		
	--Swedish
	insert into  tblComputerUserFS_tblLanguage 
	Select fs.Id, 1, 'Språk', '' from  tblComputerUserFieldSettings fs left join 
					tblComputerUserFS_tblLanguage fsl on (fs.Id = fsl.ComputerUserFieldSettings_Id and fsl.Language_Id = 1)
	where fsl.ComputerUserFieldSettings_Id is null and  fs.ComputerUserField = 'Language_Id' 


	--English
	insert into  tblComputerUserFS_tblLanguage 
	Select fs.Id, 2, 'Language', '' from  tblComputerUserFieldSettings fs left join 
					tblComputerUserFS_tblLanguage fsl on (fs.Id = fsl.ComputerUserFieldSettings_Id and fsl.Language_Id = 2)
	where fsl.ComputerUserFieldSettings_Id is null and  fs.ComputerUserField = 'Language_Id' 

				

if not exists(select * from sysobjects WHERE Name = N'tblLink_tblWorkingGroup')
begin

	CREATE TABLE [dbo].[tblLink_tblWorkingGroup](
		[Link_Id] [int] NOT NULL,
		[WorkingGroup_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblLink_tblWorkingGroup] PRIMARY KEY CLUSTERED 
	(
		[Link_Id] ASC,
		[WorkingGroup_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	ALTER TABLE [dbo].[tblLink_tblWorkingGroup]  WITH CHECK ADD  CONSTRAINT [FK_tblLink_tblWorkingGroup_tblLink] FOREIGN KEY([Link_Id])
	REFERENCES [dbo].[tblLink] ([Id])


	ALTER TABLE [dbo].[tblLink_tblWorkingGroup] CHECK CONSTRAINT [FK_tblLink_tblWorkingGroup_tblLink]


	ALTER TABLE [dbo].[tblLink_tblWorkingGroup]  WITH CHECK ADD  CONSTRAINT [FK_tblLink_tblWorkingGroup_tblWorkingGroup] FOREIGN KEY([WorkingGroup_Id])
	REFERENCES [dbo].[tblWorkingGroup] ([Id])
	

	ALTER TABLE [dbo].[tblLink_tblWorkingGroup] CHECK CONSTRAINT [FK_tblLink_tblWorkingGroup_tblWorkingGroup]


end

Update tblCaseType set ShowOnExtPageCases = 0 where ShowOnExternalPage = 0

Update tblProductArea set ShowOnExtPageCases = 0 where ShowOnExternalPage = 0

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShortDescription' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE [dbo].[tblCaseSolution] ADD [ShortDescription] nvarchar(100) null
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Information' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE [dbo].[tblCaseSolution] ADD [Information] nvarchar(max) null
GO

--Update old casesolutions with new field "AgreedDate"
insert into tblCaseSolutionFieldSettings(casesolution_id, fieldname_id, mode)
select id, 68, 1 from tblCaseSolution
where id not in (select casesolution_id from  tblCaseSolutionFieldSettings where fieldname_id = 68 )

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.32'


