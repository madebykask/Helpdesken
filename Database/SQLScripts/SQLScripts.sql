-- update DB from 5.3.16 to 5.3.17 version

IF COL_LENGTH('dbo.tblWatchDateCalendarValue','ValidUntilDate') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].tblWatchDateCalendarValue ADD [ValidUntilDate] DateTime	null
END
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.17'


