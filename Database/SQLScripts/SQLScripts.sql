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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
		where syscolumns.name = N'ShowOnExtPageCases' and sysobjects.name = N'tblProductArea')
	begin
		ALTER TABLE [dbo].[tblProductArea] ADD [ShowOnExtPageCases] [int] NOT NULL DEFAULT ((0))
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


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.32'
