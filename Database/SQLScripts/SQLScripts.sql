-- update DB from 5.3.14 to 5.3.15 version
IF COL_LENGTH('dbo.tblglobalsettings','VirtualFileFolder') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblglobalsettings] ADD [VirtualFileFolder] nvarchar(50) NULL;	
END
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.15'


