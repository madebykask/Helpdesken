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

if not exists(select * from sysobjects WHERE Name = N'sp_PopulateTblDate')
BEGIN
exec('CREATE PROCEDURE [dbo].[sp_PopulateTblDate]

	@StartDate  DATE = NULL,
	@EndDate  DATE = NULL

	AS

	BEGIN 

	--Setting language
	SET LANGUAGE English

	WHILE @StartDate <= @EndDate
		BEGIN
			INSERT INTO dbo.tblDate
			(  DateKey
			 ,FullDate
			 ,DayNumberOfWeek
			 ,DayNameOfWeek
			 ,DayShortNameOfWeek
			 ,DayNumberOfMonth
			 ,DayNumberOfYear
			 ,WeekNumberOfYear
			 ,CalendarMonthName
			 ,CalendarShortMonthName
			 ,CalendarMonthNumberOfYear
			 ,CalendarQuarter
			 ,CalendarYear
			 ,CalendarSemester
			 ,CalendarYearMonth
			)

			SELECT  
				CONVERT(INTEGER, CONVERT(CHAR(10),  @StartDate, 112)) AS DateKey
				, CAST(@StartDate AS DATE) AS FullDate
				, DATEPART(dw, @StartDate) AS DayNumberOfWeek
				, DATENAME(dw, @StartDate) AS DayNameOfWeek
				, LEFT(DATENAME(dw, @StartDate), 3) AS DayShortNameOfWeek
				, DAY (@StartDate) AS DayNumberOfMonth
				, DATENAME(dayofyear, @StartDate) AS DayNumberOfYear
				, DATENAME(week, @StartDate) AS WeekNumberOfYear
				, DATENAME(mm, @StartDate) AS CalendarMonthName
				, LEFT(DATENAME(mm, @StartDate),3) AS CalendarShortMonthName
				, MONTH(@StartDate) AS CalendarMonthNumberOfYear
				, DATENAME(quarter, @StartDate) AS CalendarQuarter
				, YEAR(@StartDate) AS CalendarYear
				, CASE
					 WHEN DATENAME(quarter, @StartDate) <= 2 THEN 1 
					 ELSE 2
				  END AS CalendarSemester
				, YEAR(@StartDate) * 100 + MONTH(@StartDate) AS CalendarYearMonth
			SET @StartDate = DATEADD(dd, 1, @StartDate)
		END
	END')
end
GO

if not exists(select * from sysobjects WHERE Name = N'tblDate')
begin
CREATE TABLE [dbo].[tblDate](
	[DateKey] [int] NOT NULL,
	[FullDate] [datetime] NOT NULL,
	[DayNumberOfWeek] [tinyint] NOT NULL,
	[DayNameOfWeek] [nvarchar](20) NOT NULL,
	[DayShortNameOfWeek] [nchar](3) NOT NULL,
	[DayNumberOfMonth] [tinyint] NOT NULL,
	[DayNumberOfYear] [smallint] NOT NULL,
	[WeekNumberOfYear] [tinyint] NOT NULL,
	[CalendarMonthName] [nvarchar](20) NOT NULL,
	[CalendarShortMonthName] [nchar](3) NULL,
	[CalendarMonthNumberOfYear] [smallint] NOT NULL,
	[CalendarQuarter] [tinyint] NOT NULL,
	[CalendarYear] [int] NOT NULL,
	[CalendarSemester] [tinyint] NOT NULL,
	[CalendarYearMonth] [int] NOT NULL,
 CONSTRAINT [PK_DimDate] PRIMARY KEY CLUSTERED 
(
	[DateKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
End
Go

if not exists(select * from tblDate where DateKey = '20140101')
begin
 exec [dbo].[sp_PopulateTblDate] '2014-01-01', '2014-12-31'
end
GO	

if not exists(select * from tblDate where DateKey = '20150101')
begin
 exec [dbo].[sp_PopulateTblDate] '2015-01-01', '2015-12-31'
end
GO	

if not exists(select * from tblDate where DateKey = '20160101')
begin
 exec [dbo].[sp_PopulateTblDate] '2016-01-01', '2016-12-31'
end
GO	
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.17'

