-- update DB from 5.3.28 to 5.3.29 version

if not exists(select * from sysobjects WHERE Name = N'tblCaseFollowUps')
begin
	CREATE TABLE [dbo].[tblCaseFollowUps](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[User_Id] [int] NOT NULL,
		[Case_Id] [int] NOT NULL,
		[FollowUpDate] [datetime] NOT NULL,
		[IsActive] [bit] NOT NULL,
		CONSTRAINT [PK_tblCaseFollowUps] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblCaseFollowUps_tblUsers] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[tblUsers] ([Id]),
		CONSTRAINT [FK_tblCaseFollowUps_tblCases] FOREIGN KEY ([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
	);
end
Else 
begin
IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'CaseId'
      AND Object_ID = Object_ID(N'tblCaseFollowUps'))
    EXEC sp_rename N'tblCaseFollowUps.CaseId', N'Case_Id', N'COLUMN';

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'UserId'
      AND Object_ID = Object_ID(N'tblCaseFollowUps'))
	EXEC sp_rename N'tblCaseFollowUps.UserId', N'User_Id', N'COLUMN';
	
end
Go

if not exists(select * from sysobjects WHERE Name = N'tblCaseExtraFollowers')
begin
	CREATE TABLE [dbo].[tblCaseExtraFollowers](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Follower] nvarchar(MAX) NOT NULL,
		[Case_Id] [int] NOT NULL,
		[CreatedByUser_Id] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		CONSTRAINT [PK_tblCaseExtraFollowers] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblCaseExtraFollowers_tblCases] FOREIGN KEY ([Case_Id]) REFERENCES [dbo].[tblCase] ([Id]),
		CONSTRAINT [FK_tblCaseExtraFollowers_tblUsers] FOREIGN KEY ([CreatedByUser_Id]) REFERENCES [dbo].[tblUsers] ([Id])
	);
end
Else 
begin
IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'CaseId'
      AND Object_ID = Object_ID(N'tblCaseExtraFollowers'))
    EXEC sp_rename N'tblCaseExtraFollowers.CaseId', N'Case_Id', N'COLUMN';
end
Go

UPDATE [dbo].[tblCaseSettings]
  SET [tblCaseName] = '_temporary_LeadTime'
  WHERE [tblCaseName] = '_temporary_.LeadTime'
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Type' and sysobjects.name = N'tblQuestionnaire')
	ALTER TABLE [tblQuestionnaire] ADD [Type] int not NULL Default(0)
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Identifier' and sysobjects.name = N'tblQuestionnaire')
	ALTER TABLE [tblQuestionnaire] ADD [Identifier] nvarchar(100) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IconId' and sysobjects.name = N'tblQuestionnaireQuestionOption')
	ALTER TABLE [tblQuestionnaireQuestionOption] ADD [IconId] nvarchar(200) NULL
GO

ALTER TABLE [dbo].[tblQuestionnaire] ALTER COLUMN [QuestionnaireDescription] ntext NULL

ALTER TABLE [dbo].[tblQuestionnaire_tblLanguage] ALTER COLUMN [QuestionnaireDescription] ntext NULL

ALTER TABLE [dbo].[tblQuestionnaireQuestion] ALTER COLUMN [NoteText] nvarchar(1000) NULL

ALTER TABLE [dbo].[tblQuestionnaireQues_tblLang] ALTER COLUMN [NoteText] nvarchar(1000) NULL


if not exists(select * from tblDate where DateKey = '20170101')
begin
 exec [dbo].[sp_PopulateTblDate] '2017-01-01', '2017-12-31'
end
GO	


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.29'

