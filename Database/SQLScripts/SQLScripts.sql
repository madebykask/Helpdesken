-- update DB from 5.3.16 to 5.3.17 version

IF COL_LENGTH('dbo.tblWatchDateCalendarValue','ValidUntilDate') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].tblWatchDateCalendarValue ADD [ValidUntilDate] DateTime	null
END
GO

IF COL_LENGTH('dbo.tblSettings','FileIndexingServerName') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblSettings] ADD [FileIndexingServerName] nvarchar(50) null 
END
Go

IF COL_LENGTH('dbo.tblSettings','FileIndexingCatalogName') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblSettings] ADD [FileIndexingCatalogName] nvarchar(50) null 
END
GO

IF COL_LENGTH('tblRegion','Code') IS NULL
BEGIN
       ALTER TABLE tblRegion ADD Code NVARCHAR(20) NULL
END
GO

IF COL_LENGTH('tblDepartment','Code') IS NULL
BEGIN
      ALTER TABLE tblDepartment ADD Code NVARCHAR(20) NULL
END
GO

IF COL_LENGTH('tblOU','Code') IS NULL
BEGIN
      ALTER TABLE tblOU ADD Code NVARCHAR(20) NULL
END
GO

IF COL_LENGTH('tblCustomerUser','CaseRemainingTimeFilter') IS NULL
BEGIN
       ALTER TABLE tblCustomerUser ADD CaseRemainingTimeFilter NVARCHAR(50) NULL
END
GO

IF COL_LENGTH('tblGlobalSettings','CaseLockExtendTime') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].tblGlobalSettings ADD [CaseLockExtendTime] int not null default(60)
END
GO

if not exists(select * from sysobjects WHERE Name = N'tblCaseFilterFavorite')
BEGIN
	CREATE TABLE [dbo].[tblCaseFilterFavorite](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Customer_Id] [int] NOT NULL,
		[User_Id] [int] NOT NULL,
		[Name] [nvarchar](80) NOT NULL,
		[RegisteredByFilter] [nvarchar](80) NULL,
		[RegionFilter] [nvarchar](80) NULL,
		[DepartmentFilter] [nvarchar](80) NULL,		
		[CaseTypeFilter] [nvarchar](80) NULL,
		[ProductAreaFilter] [nvarchar](80) NULL,
		[WorkingGroupFilter] [nvarchar](80) NULL,
		[ResponsibleFilter] [nvarchar](80) NULL,
		[AdministratorFilter] [nvarchar](80) NULL,
		[PriorityFilter] [nvarchar](80) NULL,
		[StatusFilter] [nvarchar](80) NULL,
		[SubStatusFilter] [nvarchar](80) NULL,
		[RemainingTimeFilter] [nvarchar](80) NULL,
		[ClosingReasonFilter] [nvarchar](80) NULL,
		[RegistrationDateStartFilter] [datetime] NULL,
		[RegistrationDateEndFilter] [datetime] NULL,
		[WatchDateStartFilter] [datetime] NULL,
		[WatchDateEndFilter] [datetime] NULL,
		[ClosingDateStartFilter] [datetime] NULL,
		[ClosingDateEndFilter] [datetime] NULL,
		[CreatedDate] [datetime] Not NULL,
	 CONSTRAINT [PK_tblCaseFilterFavorite] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

ALTER TABLE tblUsers ALTER COLUMN UserId nvarchar(40) not null
GO


IF COL_LENGTH('dbo.tblUsers','ShowCaseStatistics') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblUsers] ADD [ShowCaseStatistics] int not null default(1)
END
Go
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.17'

