if not exists(select * from sysobjects WHERE Name = N'tblEntityInfo')
begin
	CREATE TABLE [dbo].[tblEntityInfo](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[EntityGuid] [uniqueidentifier] NOT NULL,
		[EntityName] [nvarchar](50) NOT NULL,
		[EntityType] [nvarchar](50) NOT NULL,
		[EntityDescription] [nvarchar](3000) NULL,
	 CONSTRAINT [PK_tblEntityInfo] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
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
GO



/* WORKFLOW */

/* Modify CaseSolution */

--if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsWorkflowStep' and sysobjects.name = N'tblCaseSolution')
--	ALTER TABLE [dbo].[tblCaseSolution] ADD [IsWorkflowStep] [bit] NOT NULL DEFAULT ((0))
--GO


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
		[CaseField_Name] [nvarchar](100) NULL,
		[Values] [nvarchar](100) NULL,
		[Sequence] [int] NULL,
		[Status] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[ChangedDate] [datetime] NULL,
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
GO






