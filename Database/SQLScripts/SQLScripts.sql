-- update DB from 5.3.9.XX to 5.3.10.xx version

IF COL_LENGTH('dbo.tblSettings','ComputerUserSearchRestriction') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblSettings]
	ADD ComputerUserSearchRestriction int default 0 not null
END
GO



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.10'
