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

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.17'


