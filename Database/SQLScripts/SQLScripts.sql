-- update DB from 5.3.4.XX to 5.3.5.xx version

-- Add new values to tblTextType - Nina
If not exists (select * from tblTextType where Id = 0)
	insert into tblTextType (Id, TextType, Status) VALUES (0, 'Dh Helpdesk', 1)
GO

If not exists (select * from tblTextType where Id = 1)
	insert into tblTextType (Id, TextType, Status) VALUES (1, 'Grunddata', 1)
GO

IF COL_LENGTH('dbo.tblGlobalSettings','HelpdeskDBVersion') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblGlobalSettings]
	ADD [HelpdeskDBVersion] nvarchar(20) NULL 
END
GO 

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.5'
