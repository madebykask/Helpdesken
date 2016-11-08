-- update DB from 5.3.27 to 5.3.28 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SetCurrentUserAsPerformer' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD SetCurrentUserAsPerformer int NULL
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SetCurrentUsersWorkingGroup' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD SetCurrentUsersWorkingGroup int NULL
Go


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SaveAndClose' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD SaveAndClose int NULL
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsUniqueEmail' and sysobjects.name = N'tblQuestionnaireCircular')
	ALTER TABLE tblQuestionnaireCircular ADD IsUniqueEmail Bit not NULL Default(0)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDateFrom' and sysobjects.name = N'tblQuestionnaireCircular')
	ALTER TABLE tblQuestionnaireCircular ADD FinishingDateFrom Datetime NULL Default(0)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDateTo' and sysobjects.name = N'tblQuestionnaireCircular')
	ALTER TABLE tblQuestionnaireCircular ADD FinishingDateTo Datetime NULL Default(0)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SelectedProcent' and sysobjects.name = N'tblQuestionnaireCircular')
	ALTER TABLE tblQuestionnaireCircular ADD SelectedProcent Int not NULL Default(5)
Go

if not exists(select * from sysobjects WHERE Name = N'tblQuestionnaireCircularDepartments')
begin
	CREATE TABLE [dbo].[tblQuestionnaireCircularDepartments](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[QuestionnaireCircularId] [int] NOT NULL,
		[DepartmentId] [int] NOT NULL,
		CONSTRAINT [PK_tblQuestionnaireCircularDepartments] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblQuestionnaireCircularDepartments_tblQuestionnaireCircular] FOREIGN KEY ([QuestionnaireCircularId]) REFERENCES [dbo].[tblQuestionnaireCircular] ([Id]),
		CONSTRAINT [FK_tblQuestionnaireCircularDepartments_tblDepartment] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[tblDepartment] ([Id])
	);
end
Go

if not exists(select * from sysobjects WHERE Name = N'tblQuestionnaireCircularCaseTypes')
begin
	CREATE TABLE [dbo].[tblQuestionnaireCircularCaseTypes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[QuestionnaireCircularId] [int] NOT NULL,
		[CaseTypeId] [int] NOT NULL,
		CONSTRAINT [PK_tblQuestionnaireCircularCaseTypes] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblQuestionnaireCircularCaseTypes_tblQuestionnaireCircular] FOREIGN KEY ([QuestionnaireCircularId]) REFERENCES [dbo].[tblQuestionnaireCircular] ([Id]),
		CONSTRAINT [FK_tblQuestionnaireCircularCaseTypes_tblCaseType] FOREIGN KEY ([CaseTypeId]) REFERENCES [dbo].[tblCaseType] ([Id])
	);
end
Go

if not exists(select * from sysobjects WHERE Name = N'tblQuestionnaireCircularProductAreas')
begin
	CREATE TABLE [dbo].[tblQuestionnaireCircularProductAreas](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[QuestionnaireCircularId] [int] NOT NULL,
		[ProductAreaId] [int] NOT NULL,
		CONSTRAINT [PK_tblQuestionnaireCircularProductAreas] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblQuestionnaireCircularProductAreas_tblQuestionnaireCircular] FOREIGN KEY ([QuestionnaireCircularId]) REFERENCES [dbo].[tblQuestionnaireCircular] ([Id]),
		CONSTRAINT [FK_tblQuestionnaireCircularProductAreas_tblProductArea] FOREIGN KEY ([ProductAreaId]) REFERENCES [dbo].[tblProductArea] ([Id])
	);
end
Go

if not exists(select * from sysobjects WHERE Name = N'tblQuestionnaireCircularWorkingGroups')
begin
	CREATE TABLE [dbo].[tblQuestionnaireCircularWorkingGroups](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[QuestionnaireCircularId] [int] NOT NULL,
		[WorkingGroupId] [int] NOT NULL,
		CONSTRAINT [PK_tblQuestionnaireCircularWorkingGroups] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblQuestionnaireCircularWorkingGroups_tblQuestionnaireCircular] FOREIGN KEY ([QuestionnaireCircularId]) REFERENCES [dbo].[tblQuestionnaireCircular] ([Id]),
		CONSTRAINT [FK_tblQuestionnaireCircularWorkingGroups_tblWorkingGroup] FOREIGN KEY ([WorkingGroupId]) REFERENCES [dbo].[tblWorkingGroup] ([Id])
	);
end
Go

-- New fields tblSettings for SMTP settings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SMTPServer' and sysobjects.name = N'tblSettings')
	ALTER TABLE tblSettings ADD SMTPServer nvarchar(50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SMTPPort' and sysobjects.name = N'tblSettings')
	ALTER TABLE tblSettings ADD SMTPPort int not NULL Default(0)
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SMTPUserName' and sysobjects.name = N'tblSettings')
	ALTER TABLE tblSettings ADD SMTPUserName nvarchar(50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SMTPPassword' and sysobjects.name = N'tblSettings')
	ALTER TABLE tblSettings ADD SMTPPassword nvarchar(50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsSMTPSecured' and sysobjects.name = N'tblSettings')
	ALTER TABLE tblSettings ADD IsSMTPSecured Bit not NULL Default(0)
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.28'

