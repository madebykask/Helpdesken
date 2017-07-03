-- update DB from 5.3.32 to 5.3.33 version

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

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblCaseSolutionConditionProperties' AND xtype='U')
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
			'dbo.tblStateSecondary',
			'Id',
			'StateSecondary',
			'StateSecondaryGUID',
			0)


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
			'dbo.tblWorkingGroup',
			'Id',
			'WorkingGroup',
			'WorkingGroupGUID',
			1)

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
			'dbo.tblPriority',
			'Id',
			'PriorityName',
			'PriorityGUID',
			2)

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
			'dbo.tblStatus',
			'Id',
			'StatusName',
			'StatusGUID'
			,3)

	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('case_ProductArea.ProductAreaGUID',
			'Ärende - Produktområde',
			'dbo.tblProductArea',
			'Id',
			'ProductArea',
			'ProductAreaGUID'
			,4)


	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('user_workinggroup',
			'Användare - Driftgrupp',
			'dbo.tblWorkingGroup',
			'Id',
			'WorkingGroup',
			'WorkingGroupGUID'
			,5
			)


	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('casesolution_ProductArea.ProductAreaGUID',
			'Ärendemall - Produktområde',
			'dbo.tblProductArea',
			'Id',
			'ProductArea',
			'ProductAreaGUID',
			6)

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

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.33'