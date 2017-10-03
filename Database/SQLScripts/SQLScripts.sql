

--update DB from 5.3.32 to 5.3.33 version
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ExcludeAdministrators' and sysobjects.name = N'tblQuestionnaire')
	ALTER TABLE [dbo].[tblQuestionnaire] ADD [ExcludeAdministrators] bit not null DEFAULT(0)
GO

-- New field in tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LDAPCreateOrganization' and sysobjects.name = N'tblSettings')
   ALTER TABLE tblSettings ADD LDAPCreateOrganization int NOT NULL Default(0)
GO

--UPDATE tblCaseSolutionCondition field length
ALTER TABLE tblCaseSolutionCondition
ALTER COLUMN [Values] nvarchar(4000)

IF EXISTS (SELECT * FROM sysobjects WHERE name='tblCaseSolutionConditionProperties' AND xtype='U')
begin
	DROP TABLE tblCaseSolutionConditionProperties
end
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblCaseSolutionConditionProperties' AND xtype='U')
BEGIN
	
	CREATE TABLE [dbo].[tblCaseSolutionConditionProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaseSolutionConditionProperty] [nvarchar](100) NULL,
	[Text] [nvarchar](400) NULL,
	[Table] [nvarchar](100) NULL,
	[TableFieldId] [nvarchar](100) NULL,
	[TableFieldName] [nvarchar](100) NULL,
	[TableFieldGuid] [nvarchar](100) NULL,
	[TableParentId] [nvarchar](100) NULL,
	[SortOrder] [int] NULL,
	[Status] [int] NULL,
	 CONSTRAINT [PK_tblCaseSolutionConditionProperties] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblCaseSolutionConditionProperties] ADD  CONSTRAINT [DF_tblCaseSolutionConditionProperties_SortOrder]  DEFAULT ((0)) FOR [SortOrder]

	ALTER TABLE [dbo].[tblCaseSolutionConditionProperties] ADD  CONSTRAINT [DF_tblCaseSolutionConditionProperties_Status]  DEFAULT ((1)) FOR [Status]
END


	TRUNCATE TABLE [dbo].[tblCaseSolutionConditionProperties]

	declare @sortOrder int = 0

	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('case_StateSecondary.StateSecondaryGUID',
			'Ärende - Understatus',
			'tblStateSecondary',
			'Id',
			'StateSecondary',
			'StateSecondaryGUID',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('case_WorkingGroup.WorkingGroupGUID',
			'Ärende - Driftgrupp',
			'tblWorkingGroup',
			'Id',
			'WorkingGroup',
			'WorkingGroupGUID',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('case_Priority.PriorityGUID',
			'Ärende - Prioritet',
			'tblPriority',
			'Id',
			'PriorityName',
			'PriorityGUID',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
		   
     VALUES
           ('case_Status.StatusGUID',
			'Ärende - Status',
			'tblStatus',
			'Id',
			'StatusName',
			'StatusGUID'
			,@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder]
		   ,[TableParentId])
     VALUES
           ('case_ProductArea.ProductAreaGUID',
			'Ärende - Produktområde',
			'tblProductArea',
			'Id',
			'ProductArea',
			'ProductAreaGUID'
			,@sortOrder
			,'Parent_ProductArea_Id')

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('case_Relation',
			'Kopplat ärende',
			'tblYesNo',
			'Id',
			'Value',
			'Id',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('user_WorkingGroup.WorkingGroupGUID',
			'Användare - Driftgrupp',
			'tblWorkingGroup',
			'Id',
			'WorkingGroup',
			'WorkingGroupGUID'
			,@sortOrder
			)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder]
		   ,[TableParentId])
     VALUES
           ('casesolution_ProductArea.ProductAreaGUID',
			'Ärendemall - Produktområde',
			'tblProductArea',
			'Id',
			'ProductArea',
			'ProductAreaGUID',
			@sortOrder,
			'Parent_ProductArea_Id')

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('application_type',
			'Applikation',
			'tblApplicationType',
			'Id',
			'ApplicationType',
			'Id',
			@sortOrder)





update tblCaseSolutionCondition set property_name =  'user_WorkingGroup.WorkingGroupGUID' where property_name = 'user_WorkingGroup'
delete from tblCaseSolutionCondition where [Values] = 'null'

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IconSrc' and sysobjects.name = N'tblQuestionnaireQuestionOption')
	ALTER TABLE [dbo].[tblQuestionnaireQuestionOption] ADD [IconSrc] varbinary(2048) null
GO

-- New field in tblCustomer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowCasesOnExternalPage' and sysobjects.name = N'tblCustomer')
   ALTER TABLE tblCustomer ADD ShowCasesOnExternalPage int NOT NULL Default(1)
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'GroupCaseTemplates' and sysobjects.name = N'tblCustomer')
   ALTER TABLE tblCustomer ADD GroupCaseTemplates int NOT NULL Default(0)
GO


IF EXISTS (SELECT * FROM sysobjects WHERE name='tblApplicationType' AND xtype='U')
begin
	DROP TABLE tblApplicationType
end
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblApplicationType' AND xtype='U')
	BEGIN

		CREATE TABLE [dbo].[tblApplicationType](
			[Id] [int] NULL,
			[ApplicationType] [nvarchar](50) NULL,
			[ApplicationTypeGUID] [uniqueidentifier] NULL,
			[Customer_Id] [int] NULL
		) ON [PRIMARY]


		ALTER TABLE [dbo].[tblApplicationType] ADD  CONSTRAINT [DF_tblApplicationType_ApplicationTypeGUID]  DEFAULT (newid()) FOR [ApplicationTypeGUID]

	END


	TRUNCATE TABLE [dbo].[tblApplicationType]
	INSERT INTO [dbo].[tblApplicationType]
           ([Id]
           ,[ApplicationType]
		   )
     VALUES
           (1,
			'Helpdesk'
			)

	INSERT INTO [dbo].[tblApplicationType]
           ([Id]
           ,[ApplicationType]
		   )
     VALUES
           (2,
			'Selfservice'
			)


if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
           where syscolumns.name = N'MetaDataText' and sysobjects.name = N'tblMetaData')
	ALTER TABLE [dbo].[tblMetaData] ALTER COLUMN [MetaDataText] nvarchar(3500) not Null 
GO



if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseForms')
begin

	CREATE TABLE [dbo].[ExtendedCaseForms]
	(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
		[MetaData] NVARCHAR(MAX) NOT NULL, 
		[Description] NVARCHAR(500) NULL, 
		[CreatedOn] DATETIME NOT NULL, 
		[CreatedBy] NVARCHAR(50) NOT NULL, 
		[UpdatedOn] DATETIME NULL, 
		[UpdatedBy] NVARCHAR(50) NULL
	)

end

if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseAssignments')
begin
CREATE TABLE [dbo].[ExtendedCaseAssignments]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ExtendedCaseFormId] INT NOT NULL, 
    [UserRole] INT NULL, 
    [CaseStatus] INT NULL, 
    [CustomerId] INT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [UpdatedBy] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_EFormAssignments_ExtendedCaseForms] FOREIGN KEY ([ExtendedCaseFormId]) REFERENCES [ExtendedCaseForms]([Id])
)
end

if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseCustomDataSources')
begin

	CREATE TABLE [dbo].[ExtendedCaseCustomDataSources]
	(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
		[DataSourceId] NVARCHAR(100) NOT NULL, 
		[Description] NVARCHAR(500) NULL, 
		[MetaData] NVARCHAR(MAX) NOT NULL,
		[CreatedOn] DATETIME NOT NULL, 
		[CreatedBy] NVARCHAR(50) NOT NULL, 
		[UpdatedOn] DATETIME NULL, 
		[UpdatedBy] NVARCHAR(50) NULL
	)
end

if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseOptionDataSources')
begin
	CREATE TABLE [dbo].[ExtendedCaseOptionDataSources]
	(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
		[DataSourceId] NVARCHAR(100) NOT NULL, 
		[Description] NVARCHAR(500) NULL, 
		[MetaData] NVARCHAR(MAX) NOT NULL,
		[CreatedOn] DATETIME NOT NULL, 
		[CreatedBy] NVARCHAR(50) NOT NULL, 
		[UpdatedOn] DATETIME NULL, 
		[UpdatedBy] NVARCHAR(50) NULL
	)
end

if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseTranslations')
begin
	CREATE TABLE [dbo].[ExtendedCaseTranslations]
	(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
		[LanguageId] INT NULL, 
		[Property] NVARCHAR(200) NOT NULL, 
		[Text] NVARCHAR(MAX) NOT NULL
	)
end

if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseData')
begin
	CREATE TABLE [dbo].[ExtendedCaseData](
		[Id] [int] NOT NULL PRIMARY KEY IDENTITY, 
		[ExtendedCaseGuid] [uniqueidentifier]  NOT NULL DEFAULT NEWID(),
		[ExtendedCaseFormId] [int] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[UpdatedOn] [datetime] NULL,
		[UpdatedBy] [nvarchar](50) NULL,	
		CONSTRAINT [FK_ExtendedCaseData_ExtendedCaseForms] FOREIGN KEY([ExtendedCaseFormId]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]))
	
		
	end

if exists(select * from sysobjects WHERE Name = N'ExtendedCaseData')
begin

	if not exists(SELECT * FROM sys.objects WHERE type = 'D' AND name = 'DF_ExtendedCaseData_CreatedOn')
	begin
		ALTER TABLE [dbo].[ExtendedCaseData] ADD  CONSTRAINT [DF_ExtendedCaseData_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
	end
end


if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseValues')
begin

	CREATE TABLE [dbo].[ExtendedCaseValues](
		[Id] [int]  NOT NULL PRIMARY KEY IDENTITY,
		[ExtendedCaseDataId] [int] NOT NULL,
		[FieldId] [nvarchar](500) NOT NULL,
		[Value] [nvarchar](max) NULL,
		[SecondaryValue] [nvarchar](max) NULL,
		CONSTRAINT [FK_ExtendedCaseValues_ExtendedCaseData] FOREIGN KEY([ExtendedCaseDataId]) REFERENCES [dbo].[ExtendedCaseData] ([Id])
	)

end

if not exists(select * from sysobjects WHERE Name = N'tblCase_ExtendedCaseData')
begin
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCase_ExtendedCaseData](
		[Case_Id] [int] NOT NULL,
		[ExtendedCaseData_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblCase_ExtendedCaseData] PRIMARY KEY CLUSTERED 
	(
		[Case_Id] ASC,
		[ExtendedCaseData_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCase_ExtendedCaseData]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCase_ExtendedCaseData_ExtendedCaseData] FOREIGN KEY([ExtendedCaseData_Id])
	REFERENCES [dbo].[ExtendedCaseData] ([Id])

	ALTER TABLE [dbo].[tblCase_ExtendedCaseData] CHECK CONSTRAINT [FK_tblCase_ExtendedCaseData_ExtendedCaseData]

	ALTER TABLE [dbo].[tblCase_ExtendedCaseData]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCase_ExtendedCaseData_tblCase] FOREIGN KEY([Case_Id])
	REFERENCES [dbo].[tblCase] ([Id])

	ALTER TABLE [dbo].[tblCase_ExtendedCaseData] CHECK CONSTRAINT [FK_tblCase_ExtendedCaseData_tblCase]
end


if not exists(select * from sysobjects WHERE Name = N'tblCaseSolution_ExtendedCaseForms')
begin

	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCaseSolution_ExtendedCaseForms](
		[CaseSolution_Id] [int] NOT NULL,
		[ExtendedCaseForms_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblCaseSolution_ExtendedCaseForms] PRIMARY KEY CLUSTERED 
	(
		[CaseSolution_Id] ASC,
		[ExtendedCaseForms_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_ExtendedCaseForms] FOREIGN KEY([ExtendedCaseForms_Id])
	REFERENCES [dbo].[ExtendedCaseForms] ([Id])

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms] CHECK CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_ExtendedCaseForms]

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_tblCaseSolution] FOREIGN KEY([CaseSolution_Id])
	REFERENCES [dbo].[tblCaseSolution] ([Id])

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms] CHECK CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_tblCaseSolution]

end


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Name' and sysobjects.name = N'ExtendedCaseForms')
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Name] nvarchar(100) null
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Guid' and sysobjects.name = N'ExtendedCaseForms')
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Guid] [uniqueidentifier] NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'ExtendedCaseForms')
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Status] [int] NOT NULL Default(0)
GO


-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultTab' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE [dbo].tblCaseSolution ADD DefaultTab nvarchar(100) NOT NULL DEFAULT('case-tab')
	end
GO

-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ModuleExtendedCase' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE [dbo].[tblSettings] ADD [ModuleExtendedCase] int not null Default(0)
	end
GO

-- OK
/* StateSecondary columns, Make sure they exist in db, these are old ones.... */
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AlternativeStateSecondaryName' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE [dbo].[tblStateSecondary] ADD [AlternativeStateSecondaryName] [nvarchar](50) NULL
	end
GO

-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondaryId' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE [dbo].[tblStateSecondary] ADD [StateSecondaryId] int not null Default(0)
	end
GO

/* StateSecondary columns, Make sure they exist in db, these are old ones.... */


-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroupId' and sysobjects.name = N'tblWorkingGroup')
	begin
		ALTER TABLE [dbo].[tblWorkingGroup] ADD [WorkingGroupId] int not null Default(0)
	end
GO


--FOR CURRENT RECORD - TEST

-- EJ MED I TRANSFER SCRIPT = OK
----tblComputerUsers - make sure it does not allow null and add newid()
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ComputerUserGUID' and sysobjects.name = N'tblComputerUsers')
begin
		EXECUTE  sp_executesql  "update tblComputerUsers set ComputerUserGUID = newid() where ComputerUserGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblComputerUsers'
					  and c.name = 'ComputerUserGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_ComputerUserGUID')
		begin
			Alter table tblComputerUsers
			Add constraint DF_ComputerUserGUID default (newid()) For ComputerUserGUID		
		end		

		Alter table tblComputerUsers
		ALTER COLUMN [ComputerUserGUID] uniqueIdentifier NOT NULL
end
else
begin
	Alter table tblComputerUsers
	Add ComputerUserGUID uniqueIdentifier NOT NULL CONSTRAINT DF_ComputerUserGUID default (newid())
end
GO

-- EJ MED I TRANSFER SCRIPT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ExtendedCasePath' and sysobjects.name = N'tblGlobalSettings')
	begin
		ALTER TABLE [dbo].tblGlobalSettings ADD ExtendedCasePath nvarchar(500) NULL 
	end
GO

-- EJ MED I TRANSFER SCRIPT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ActiveTab' and sysobjects.name = N'tblCaseLock')
	begin
		ALTER TABLE [dbo].tblCaseLock ADD ActiveTab nvarchar(100) NULL 
	end
GO

-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ValidateOnChange' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE [dbo].tblCaseSolution ADD ValidateOnChange nvarchar(100) NULL 
	end
GO


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CacheData' and sysobjects.name = N'tblFormSettings')
	begin
		ALTER TABLE [dbo].tblFormSettings ADD CacheData bit
	end
GO

-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AttachmentPlacement' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE [dbo].[tblSettings] ADD [AttachmentPlacement] int not null Default(0)
	end
GO

-- OK - Not used in Transfer script
----tblUsers - make sure it does not allow null and add newid()
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserGUID' and sysobjects.name = N'tblUsers')
begin
		EXECUTE  sp_executesql  "update tblUsers set UserGUID = newid() where UserGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblUsers'
					  and c.name = 'UserGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_UserGUID')
		begin
			Alter table tblUsers
			Add constraint DF_UserGUID default (newid()) For UserGUID		
		end		

		Alter table tblUsers
		ALTER COLUMN [UserGUID] uniqueIdentifier NOT NULL
end
else
begin
	Alter table tblUsers
	Add UserGUID uniqueIdentifier NOT NULL CONSTRAINT DF_UserGUID default (newid())
end
GO

---- OK - not used in Transfer Script
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Customer_Id' and sysobjects.name = N'tblEntityDefault')
	begin
		ALTER TABLE [dbo].[tblEntityDefault] ADD [Customer_Id] int not null

		ALTER TABLE [dbo].[tblEntityDefault]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityDefault_tblCustomer] FOREIGN KEY([Customer_Id])
		REFERENCES [dbo].[tblCustomer] ([Id])

		ALTER TABLE [dbo].[tblEntityDefault] CHECK CONSTRAINT [FK_tblEntityDefault_tblCustomer]

	end
GO

---- OK - not used in Transfer Script
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Customer_Id' and sysobjects.name = N'tblEntityRelationship')
	begin
		ALTER TABLE [dbo].[tblEntityRelationship] ADD [Customer_Id] int not null 

		ALTER TABLE [dbo].[tblEntityRelationship]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityRelationship_tblCustomer] FOREIGN KEY([Customer_Id])
		REFERENCES [dbo].[tblCustomer] ([Id])

		ALTER TABLE [dbo].[tblEntityRelationship] CHECK CONSTRAINT [FK_tblEntityRelationship_tblCustomer]

	end
GO

---- OK - not used in Transfer Script
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Customer_Id' and sysobjects.name = N'tblEntityAttribute')
	begin
		ALTER TABLE [dbo].[tblEntityAttribute] ADD [Customer_Id] int not null 

		ALTER TABLE [dbo].[tblEntityAttribute]  WITH CHECK ADD  CONSTRAINT [FK_tblEntityAttribute_tblCustomer] FOREIGN KEY([Customer_Id])
		REFERENCES [dbo].[tblCustomer] ([Id])

		ALTER TABLE [dbo].[tblEntityAttribute] CHECK CONSTRAINT [FK_tblEntityAttribute_tblCustomer]

	end
GO

-- 20170628 Johan Weinitz, added support for independent child cases (parent case can be closed with open independent child cases)
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Independent' and sysobjects.name = N'tblParentChildCaseRelations')
	begin
		ALTER TABLE [dbo].tblParentChildCaseRelations ADD Independent BIT DEFAULT(0) NOT NULL
	end
GO
if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentTemplate')
begin

	CREATE TABLE [dbo].[tblCaseDocumentTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PageNumbersUse] [bit] NOT NULL,
	[CaseDocumentTemplateGUID] [uniqueidentifier] NULL,
	[MarginTop] [int] NOT NULL,
	[MarginBottom] [int] NOT NULL,
	[MarginLeft] [int] NOT NULL,
	[MarginRight] [int] NOT NULL,
	[FooterHeight] [int] NOT NULL,
	[HeaderHeight] [int] NOT NULL,
	 CONSTRAINT [PK_tblCaseDocumentTemplate] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_PageNumbersUse]  DEFAULT ((0)) FOR [PageNumbersUse]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_CaseDocumentTemplateGUID]  DEFAULT (newid()) FOR [CaseDocumentTemplateGUID]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginTop]  DEFAULT ((0)) FOR [MarginTop]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginBottom]  DEFAULT ((0)) FOR [MarginBottom]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginLeft]  DEFAULT ((0)) FOR [MarginLeft]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_MarginRight]  DEFAULT ((0)) FOR [MarginRight]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_FooterHeight]  DEFAULT ((0)) FOR [FooterHeight]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_HeaderHeight]  DEFAULT ((0)) FOR [HeaderHeight]
	
end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocument')
begin

	CREATE TABLE [dbo].[tblCaseDocument](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocumentGUID] [uniqueidentifier] NULL,
		[Name] [nvarchar](100) NULL,
		[Description] [nvarchar](200) NULL,
		[Customer_Id] [int] NULL,
		[FileType] [nvarchar](10) NOT NULL,
		[SortOrder] [int] NOT NULL,
		[Status] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NOT NULL,
		[ChangedByUser_Id] [int] NULL,
		[CaseDocumentTemplate_Id] [int] NOT NULL,
		[Version] [int] NOT NULL,
	 CONSTRAINT [PK_ExtendedCaseDocuments] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseDocument] ADD  CONSTRAINT [DF_tblCaseDocument_CaseDocumentGUID]  DEFAULT (newid()) FOR [CaseDocumentGUID]

	ALTER TABLE [dbo].[tblCaseDocument] ADD  CONSTRAINT [DF_tblCaseDocument_SortOrder]  DEFAULT ((0)) FOR [SortOrder]

	ALTER TABLE [dbo].[tblCaseDocument] ADD  CONSTRAINT [DF_tblCaseDocument_Status]  DEFAULT ((1)) FOR [Status]

	ALTER TABLE [dbo].[tblCaseDocument] ADD  CONSTRAINT [DF_tblCaseDocument_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

	ALTER TABLE [dbo].[tblCaseDocument] ADD  CONSTRAINT [DF_tblCaseDocument_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

	ALTER TABLE [dbo].[tblCaseDocument] ADD  DEFAULT ((0)) FOR [Version]

end



if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentCondition')
begin
	CREATE TABLE [dbo].[tblCaseDocumentCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocumentConditionGUID] [uniqueidentifier] NULL,
		[CaseDocument_Id] [int] NULL,
		[Property_Name] [nvarchar](500) NULL,
		[Values] [nvarchar](max) NOT NULL,
		[Description] [nvarchar](200) NULL,
		[Status] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_ExtendedCaseDocumentsCondition] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[tblCaseDocumentCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentsCondition_CaseSolutionConditionGUID]  DEFAULT (newid()) FOR [CaseDocumentConditionGUID]

	ALTER TABLE [dbo].[tblCaseDocumentCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentsCondition_Status]  DEFAULT ((0)) FOR [Status]

	ALTER TABLE [dbo].[tblCaseDocumentCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentsCondition_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

	ALTER TABLE [dbo].[tblCaseDocumentCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentsCondition_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

	ALTER TABLE [dbo].[tblCaseDocumentCondition]  WITH NOCHECK ADD  CONSTRAINT [FK_CaseDocumentsCondition_CaseDocument] FOREIGN KEY([CaseDocument_Id])
	REFERENCES [dbo].[tblCaseDocument] ([Id])

	ALTER TABLE [dbo].[tblCaseDocumentCondition] NOCHECK CONSTRAINT [FK_CaseDocumentsCondition_CaseDocument]

	ALTER TABLE [dbo].[tblCaseDocumentCondition]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseDocumentCondition_tblCaseDocument] FOREIGN KEY([CaseDocument_Id])
	REFERENCES [dbo].[tblCaseDocument] ([Id])

	ALTER TABLE [dbo].[tblCaseDocumentCondition] CHECK CONSTRAINT [FK_tblCaseDocumentCondition_tblCaseDocument]
	
end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentParagraph')
begin
	CREATE TABLE [dbo].[tblCaseDocumentParagraph](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](500) NULL,
		[Description] [nvarchar](50) NULL,
		[ParagraphType] [int] NULL,
		[CaseDocumentParagraphGUID] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_tblCaseDocumentParagraph] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseDocumentParagraph] ADD  CONSTRAINT [DF_tblCaseDocumentParagraph_ParagraphType]  DEFAULT ((1)) FOR [ParagraphType]
end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentParagraph')
begin

	CREATE TABLE [dbo].[tblCaseDocumentParagraph](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NULL,
		[Description] [nvarchar](50) NULL,
		[ParagraphType] [int] NULL,
	 CONSTRAINT [PK_tblCaseDocumentParagraph] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseDocumentParagraph] ADD  CONSTRAINT [DF_tblCaseDocumentParagraph_ParagraphType]  DEFAULT ((1)) FOR [ParagraphType]

end



if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentText')
begin

	CREATE TABLE [dbo].[tblCaseDocumentText](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocumentParagraph_Id] [int] NULL,
		[Name] [nvarchar](50) NULL,
		[Description] [nvarchar](50) NULL,
		[Text] [nvarchar](max) NOT NULL,
		[Headline] [nvarchar](200) NULL,
		[SortOrder] [int] NOT NULL,
		[CaseDocumentTextGUID] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_tblCaseDocumentText] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseDocumentText] ADD  CONSTRAINT [DF_tblCaseDocumentText_SortOrder]  DEFAULT ((0)) FOR [SortOrder]

	ALTER TABLE [dbo].[tblCaseDocumentText]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseDocumentText_tblCaseDocumentParagraph] FOREIGN KEY([CaseDocumentParagraph_Id])
	REFERENCES [dbo].[tblCaseDocumentParagraph] ([Id])

	ALTER TABLE [dbo].[tblCaseDocumentText] CHECK CONSTRAINT [FK_tblCaseDocumentText_tblCaseDocumentParagraph]

end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocument_CaseDocumentParagraph')
begin

	CREATE TABLE [dbo].[tblCaseDocument_CaseDocumentParagraph](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocument_Id] [int] NOT NULL,
		[CaseDocumentParagraph_Id] [int] NOT NULL,
		[SortOrder] [int] NOT NULL,
	 CONSTRAINT [PK_tblCaseDocument_CaseDocumentParagraph] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseDocument_CaseDocumentParagraph]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseDocument_tblCaseDocumentParagraph_tblCaseDocument] FOREIGN KEY([CaseDocument_Id])
	REFERENCES [dbo].[tblCaseDocument] ([Id])

	ALTER TABLE [dbo].[tblCaseDocument_CaseDocumentParagraph] CHECK CONSTRAINT [FK_tblCaseDocument_tblCaseDocumentParagraph_tblCaseDocument]

	ALTER TABLE [dbo].[tblCaseDocument_CaseDocumentParagraph]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseDocument_tblCaseDocumentParagraph_tblCaseDocumentParagraph] FOREIGN KEY([CaseDocumentParagraph_Id])
	REFERENCES [dbo].[tblCaseDocumentParagraph] ([Id])

	ALTER TABLE [dbo].[tblCaseDocument_CaseDocumentParagraph] CHECK CONSTRAINT [FK_tblCaseDocument_tblCaseDocumentParagraph_tblCaseDocumentParagraph]
end


-- New column in ExtendedCaseForms
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Version' and sysobjects.name = N'ExtendedCaseForms')
begin
   ALTER TABLE ExtendedCaseForms ADD [Version] int NOT NULL Default(0)
end


if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentParagraphCondition')
begin
	CREATE TABLE [dbo].[tblCaseDocumentParagraphCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocumentParagraphConditionGUID] [uniqueidentifier] NULL,
		[CaseDocumentParagraph_Id] [int] NULL,
		[Property_Name] [nvarchar](500) NULL,
		[Operator] [nvarchar](50) NULL,
		[Values] [nvarchar](max) NULL,
		[Description] [nvarchar](200) NULL,
		[Status] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_ExtendedCaseDocumentParagraphsCondition] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	ALTER TABLE [dbo].[tblCaseDocumentParagraphCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentParagraphsCondition_CaseSolutionConditionGUID]  DEFAULT (newid()) FOR [CaseDocumentParagraphConditionGUID]

	ALTER TABLE [dbo].[tblCaseDocumentParagraphCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentParagraphsCondition_Status]  DEFAULT ((0)) FOR [Status]

	ALTER TABLE [dbo].[tblCaseDocumentParagraphCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentParagraphsCondition_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

	ALTER TABLE [dbo].[tblCaseDocumentParagraphCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentParagraphsCondition_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentTextCondition')
begin

	CREATE TABLE [dbo].[tblCaseDocumentTextCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocumentTextConditionGUID] [uniqueidentifier] NULL,
		[CaseDocumentText_Id] [int] NULL,
		[Property_Name] [nvarchar](500) NULL,
		[Operator] [nvarchar](50) NULL,
		[Values] [nvarchar](max) NULL,
		[Description] [nvarchar](200) NULL,
		[Status] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_ExtendedCaseDocumentTextsCondition] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[tblCaseDocumentTextCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentTextsCondition_CaseSolutionConditionGUID]  DEFAULT (newid()) FOR [CaseDocumentTextConditionGUID]

	ALTER TABLE [dbo].[tblCaseDocumentTextCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentTextsCondition_Status]  DEFAULT ((0)) FOR [Status]

	ALTER TABLE [dbo].[tblCaseDocumentTextCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentTextsCondition_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

	ALTER TABLE [dbo].[tblCaseDocumentTextCondition] ADD  CONSTRAINT [DF_ExtendedCaseDocumentTextsCondition_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

	ALTER TABLE [dbo].[tblCaseDocumentTextCondition]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseDocumentTextCondition_tblCaseDocumentText] FOREIGN KEY([CaseDocumentText_Id])
	REFERENCES [dbo].[tblCaseDocumentText] ([Id])

	ALTER TABLE [dbo].[tblCaseDocumentTextCondition] CHECK CONSTRAINT [FK_tblCaseDocumentTextCondition_tblCaseDocumentText]
end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentTextIdentifier')
begin

	CREATE TABLE [dbo].[tblCaseDocumentTextIdentifier](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ExtendedCaseFormId] [int] NOT NULL,
		[Process] [nvarchar](50) NULL,
		[Identifier] [nvarchar](500) NOT NULL,
		[PropertyName] [nvarchar](500) NOT NULL,
		[DisplayName] [nvarchar](50) NULL,
	 CONSTRAINT [PK_tblCaseDocumentTextIdentifier] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

end
GO

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Properties' and sysobjects.name = N'ExtendedCaseValues')
begin
	ALTER TABLE [dbo].[ExtendedCaseValues] ADD [Properties] [nvarchar](Max) NULL 
end
GO


if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentTextConditionIdentifier')
begin

	CREATE TABLE [dbo].[tblCaseDocumentTextConditionIdentifier](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ExtendedCaseFormId] [int] NOT NULL,
		[Process] [nvarchar](50) NULL,
		[Identifier] [nvarchar](500) NOT NULL,
		[PropertyName] [nvarchar](500) NOT NULL,
		[DisplayName] [nvarchar](50) NULL,
	 CONSTRAINT [PK_tblCaseDocumentTextConditionIdentifier] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

end


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'CurrentCaseSolution_Id' and sysobjects.name = N'tblCase')
   ALTER TABLE tblCase ADD CurrentCaseSolution_Id int NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'SplitToCaseSolution_Id' and sysobjects.name = N'tblCaseSolution')
   ALTER TABLE tblCaseSolution ADD SplitToCaseSolution_Id int NULL
GO


if not exists(select * from sysobjects WHERE Name = N'tblCaseSections')
begin
CREATE TABLE [dbo].[tblCaseSections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Customer_Id] [int] NOT NULL,
	[SectionType] [int] NOT NULL,
	[IsNewCollapsed] [bit] NOT NULL DEFAULT (0),
	[IsEditCollapsed] [bit] NOT NULL DEFAULT (0),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_tblCaseSections_CreatedDate]  DEFAULT (getdate()),
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_tblCaseSections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tblCaseSections] ADD  CONSTRAINT [FK_tblCaseSections_tblCustomer] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[tblCustomer] ([Id])

END
GO

if not exists(select * from sysobjects WHERE Name = N'tblCaseSectionFields')
begin
CREATE TABLE [dbo].[tblCaseSectionFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaseSection_Id] [int] NOT NULL,
	[CaseFieldSetting_Id] [int] NOT NULL,
 CONSTRAINT [PK_tblCaseSectionFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tblCaseSectionFields] ADD  CONSTRAINT [FK_tblCaseSectionFields_tblCaseSections] FOREIGN KEY([CaseSection_Id])
REFERENCES [dbo].[tblCaseSections] ([Id])

ALTER TABLE [dbo].[tblCaseSectionFields] ADD  CONSTRAINT [FK_tblCaseSectionFields_tblCaseFieldSettings] FOREIGN KEY([CaseFieldSetting_Id])
REFERENCES [dbo].[tblCaseFieldSettings] ([Id])

END
GO



if not exists(select * from sysobjects WHERE Name = N'tblCaseSections_tblLang')
begin
CREATE TABLE [dbo].[tblCaseSections_tblLang](
	[CaseSection_Id] [int] NOT NULL,
	[Language_Id] [int] NOT NULL,
	[Label] [nvarchar](50) NULL
 CONSTRAINT [PK_tblCaseSections_tblLang] PRIMARY KEY CLUSTERED 
(
	[CaseSection_Id] ASC,
	[Language_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tblCaseSections_tblLang] ADD  CONSTRAINT [FK_tblCaseSections_tblLang_tblCaseSections] FOREIGN KEY([CaseSection_Id])
REFERENCES [dbo].[tblCaseSections] ([Id])

ALTER TABLE [dbo].[tblCaseSections_tblLang] ADD  CONSTRAINT [FK_tblCaseSections_tblLang_tblLanguage] FOREIGN KEY([Language_Id])
REFERENCES [dbo].[tblLanguage] ([Id])

END
GO



if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseSolutionDescription' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution
	ADD CaseSolutionDescription nvarchar(4000)
GO

if not exists(select * from sysobjects WHERE Name = N'tblLogFileExisting')
BEGIN
CREATE TABLE [dbo].[tblLogFileExisting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Log_Id] [int] NULL,
	[Case_Id] [int] NOT NULL,
	[FileName] [nvarchar](200) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_tblLogFileExisting_CreatedDate]  DEFAULT (getdate())
 CONSTRAINT [PK_tblLogFileExisting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tblLogFileExisting] ADD  CONSTRAINT [FK_tblLogFileExisting_tblLog] FOREIGN KEY([Log_Id])
REFERENCES [dbo].[tblLog] ([Id])

ALTER TABLE [dbo].[tblLogFileExisting] ADD  CONSTRAINT [FK_tblLogFileExisting_tblCase] FOREIGN KEY([Case_Id])
REFERENCES [dbo].[tblCase] ([Id])

END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsCaseFile' and sysobjects.name = N'tblLogFile')
	ALTER TABLE tblLogFile
	ADD [IsCaseFile] bit NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ParentLog_Id' and sysobjects.name = N'tblLogFile')
	ALTER TABLE tblLogFile
	ADD [ParentLog_Id] int NULL
GO

-- OK
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'NextStepState' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE [dbo].tblCaseSolution ADD NextStepState int NULL
	end
GO


IF EXISTS (SELECT * FROM sysobjects WHERE name='tblYesNo' AND xtype='U')
begin
	DROP TABLE tblYesNo
end
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblYesNo' AND xtype='U')
	BEGIN

		CREATE TABLE [dbo].[tblYesNo](
			[Id] [int] NULL,
			[Value] [nvarchar](50) NULL,
			[GUID] [uniqueidentifier] NULL,
			[Customer_Id] [int] NULL
		) ON [PRIMARY]


		ALTER TABLE [dbo].[tblYesNo] ADD  CONSTRAINT [DF_tblYesNo_GUID]  DEFAULT (newid()) FOR [GUID]

	END


	TRUNCATE TABLE [dbo].[tblYesNo]
	INSERT INTO [dbo].[tblYesNo]
           ([Id]
           ,[Value]
		   )
     VALUES
           (0,
			'Nej'
			)

	INSERT INTO [dbo].[tblYesNo]
           ([Id]
           ,[Value]
		   )
     VALUES
           (1,
			'Ja'
			)


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Source' and sysobjects.name = N'tblEntityInfo')
	begin
		ALTER TABLE [dbo].tblEntityInfo ADD [Source] int NULL
	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblCaseType_tblProductArea')
BEGIN

CREATE TABLE [dbo].[tblCaseType_tblProductArea](
	[CaseType_Id] [int] NOT NULL,
	[ProductArea_Id] [int] NOT NULL,
 CONSTRAINT [PK_tblCaseType_tblProductArea] PRIMARY KEY CLUSTERED 
(
	[CaseType_Id] ASC,
	[ProductArea_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tblCaseType_tblProductArea] ADD  CONSTRAINT [FK_tblCaseType_tblProductArea_tblCaseType] FOREIGN KEY([CaseType_Id])
REFERENCES [dbo].[tblCaseType] ([Id])

ALTER TABLE [dbo].[tblCaseType_tblProductArea] ADD  CONSTRAINT [FK_tblCaseType_tblProductArea_tblProductArea] FOREIGN KEY([ProductArea_Id])
REFERENCES [dbo].[tblProductArea] ([Id])

END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'RequiredIfReopened' and sysobjects.name = N'tblCaseFieldSettings')
   ALTER TABLE tblCaseFieldSettings ADD RequiredIfReopened int NOT NULL Default(0)
GO

-- REBUILD tblCase INDEXES to fix index fragmentation
ALTER INDEX ALL ON tblCase REBUILD
GO


if not exists(select * from sysobjects WHERE Name = N'ExtendedCaseFormState')
BEGIN

	CREATE TABLE [dbo].[ExtendedCaseFormState](
	[Id] [int]  NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ExtendedCaseDataId] [int] NOT NULL,
	[TabId] [nvarchar](50) NOT NULL,
	[SectionId] [nvarchar](50) NOT NULL,
	[SectionIndex] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
	CONSTRAINT [FK_ExtendedCaseFormState_ExtendedCaseData] FOREIGN KEY([ExtendedCaseDataId]) REFERENCES [dbo].[ExtendedCaseData] ([Id]))

END


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowQuickNewCaseLink' and sysobjects.name = N'tblSettings')
   ALTER TABLE tblSettings ADD ShowQuickNewCaseLink int NOT NULL Default(0)
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'QuickNewCaseLinkText' and sysobjects.name = N'tblSettings')
   ALTER TABLE tblSettings ADD QuickNewCaseLinkText nvarchar(50) NOT NULL Default(N'+')
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'QuickNewCaseLinkUrl' and sysobjects.name = N'tblSettings')
   ALTER TABLE tblSettings ADD QuickNewCaseLinkUrl nvarchar(255) NOT NULL Default(N'/cases/new')
GO

Update tblCaseType set ShowOnExtPageCases = 0 where ShowOnExternalPage = 0

Update tblProductArea set ShowOnExtPageCases = 0 where ShowOnExternalPage = 0

Update tblCaseType set ShowOnExtPageCases = 1 where ShowOnExternalPage = 1

Update tblProductArea set ShowOnExtPageCases = 1 where ShowOnExternalPage = 1


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

-- Add more columns to table 
-- #58972
IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DataType' and sysobjects.name = N'tblCaseDocumentTextIdentifier')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTextIdentifier] ADD [DataType] [nvarchar](50) NULL 
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DataFormat' and sysobjects.name = N'tblCaseDocumentTextIdentifier')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTextIdentifier] ADD [DataFormat] [nvarchar](50) NULL 
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DisplayFormat' and sysobjects.name = N'tblCaseDocumentTextIdentifier')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTextIdentifier] ADD [DisplayFormat] [nvarchar](50) NULL 
end

GO

--#59041
IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowHeaderFromPageNr' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [ShowHeaderFromPageNr] int NOT NULL Default(0)
end

GO

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowFooterFromPageNr' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [ShowFooterFromPageNr] int NOT NULL Default(0)
end

GO

--#59045, #59044, #5904, #59042
IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Style' and sysobjects.name = N'tblCaseDocumentTemplate')
begin
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD [Style] [nvarchar](max) NOT NULL Default('')
end

GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.33'
