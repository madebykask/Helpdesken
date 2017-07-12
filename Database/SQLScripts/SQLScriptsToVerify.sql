
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





if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentTemplate')
begin


	CREATE TABLE [dbo].[tblCaseDocumentTemplate](
		[Id] [int] NULL,
		[Name] [nvarchar](50) NULL,
		[Margins] [nvarchar](50) NULL,
		[PageNumbersUse] [bit] NULL,
		[DraftUse] [bit] NULL,
		[BarCodeUse] [bit] NULL,
		[Footer] [nvarchar](max) NULL,
		[Header] [nvarchar](max) NULL,
		[LanguageId] [int] NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_PageNumbersUse]  DEFAULT ((0)) FOR [PageNumbersUse]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_DraftUse]  DEFAULT ((0)) FOR [DraftUse]
	
	ALTER TABLE [dbo].[tblCaseDocumentTemplate] ADD  CONSTRAINT [DF_tblCaseDocumentTemplate_BarCodeUse]  DEFAULT ((0)) FOR [BarCodeUse]
	
end




if not exists(select * from sysobjects WHERE Name = N'tblCaseDocument')
begin

	CREATE TABLE [dbo].[tblCaseDocument](
		[Id] [int] NOT NULL,
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
	
end


-- New column in tblCaseDocument
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseDocumentTemplate_Id' and sysobjects.name = N'tblCaseDocument')
begin
   ALTER TABLE tblCaseDocument ADD CaseDocumentTemplate_Id int NOT NULL
end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocumentCondition')
begin

	CREATE TABLE [dbo].[tblCaseDocumentCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocumentConditionGUID] [uniqueidentifier] NULL,
		[CaseDocument_Id] [int] NULL,
		[Property_Name] [nvarchar](600) NOT NULL,
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
	
	ALTER TABLE [dbo].[tblCaseDocumentCondition]  WITH NOCHECK ADD  CONSTRAINT [FK_ExtendedCaseDocumentsCondition_ExtendedCaseDocuments] FOREIGN KEY([CaseDocument_Id])
	REFERENCES [dbo].[tblCaseDocument] ([Id])
	
	ALTER TABLE [dbo].[tblCaseDocumentCondition] NOCHECK CONSTRAINT [FK_ExtendedCaseDocumentsCondition_ExtendedCaseDocuments]
	
	ALTER TABLE [dbo].[tblCaseDocumentCondition]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseDocumentCondition_tblCaseDocument] FOREIGN KEY([CaseDocument_Id])
	REFERENCES [dbo].[tblCaseDocument] ([Id])
	
	ALTER TABLE [dbo].[tblCaseDocumentCondition] CHECK CONSTRAINT [FK_tblCaseDocumentCondition_tblCaseDocument]
	
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
	 CONSTRAINT [PK_tblCaseDocumentText] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	
	ALTER TABLE [dbo].[tblCaseDocumentText] ADD  CONSTRAINT [DF_tblCaseDocumentText_SortOrder]  DEFAULT ((0)) FOR [SortOrder]
	

end

if not exists(select * from sysobjects WHERE Name = N'tblCaseDocument_CaseDocumentParagraph')
begin
	CREATE TABLE [dbo].[tblCaseDocument_CaseDocumentParagraph](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CaseDocument_Id] [int] NULL,
		[CaseDocumentParagraph_Id] [int] NULL,
		[SortOrder] [int] NULL
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