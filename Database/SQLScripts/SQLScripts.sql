-- update DB from 5.3.28 to 5.3.29 version

if not exists(select * from sysobjects WHERE Name = N'tblCaseFollowUps')
begin
	CREATE TABLE [dbo].[tblCaseFollowUps](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[CaseId] [int] NOT NULL,
		[FollowUpDate] [datetime] NOT NULL,
		[IsActive] [bit] NOT NULL,
		CONSTRAINT [PK_tblCaseFollowUps] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblCaseFollowUps_tblUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUsers] ([Id]),
		CONSTRAINT [FK_tblCaseFollowUps_tblCases] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[tblCase] ([Id])
	);
end
Go

if not exists(select * from sysobjects WHERE Name = N'tblCaseExtraFollowers')
begin
	CREATE TABLE [dbo].[tblCaseExtraFollowers](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Follower] nvarchar(MAX) NOT NULL,
		[CaseId] [int] NOT NULL,
		[CreatedByUser_Id] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		CONSTRAINT [PK_tblCaseExtraFollowers] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [FK_tblCaseExtraFollowers_tblCases] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[tblCase] ([Id]),
		CONSTRAINT [FK_tblCaseExtraFollowers_tblUsers] FOREIGN KEY ([CreatedByUser_Id]) REFERENCES [dbo].[tblUsers] ([Id])
	);
end
Go

UPDATE [dbo].[tblCaseSettings]
  SET [tblCaseName] = '_temporary_LeadTime'
  WHERE [tblCaseName] = '_temporary_.LeadTime'
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.29'

