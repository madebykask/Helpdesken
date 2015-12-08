-- update DB from 5.3.17 to 5.3.18 version

ALTER TABLE tblEMailLog ALTER COLUMN ResponseMessage nvarchar(1000)
GO

IF COL_LENGTH('dbo.tblstatesecondary','RecalculateWatchDate') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].tblstatesecondary ADD [RecalculateWatchDate] int not null 
END
Go


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.18'

