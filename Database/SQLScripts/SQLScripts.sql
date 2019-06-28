--update DB from 5.3.42 to 5.3.43 version

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'EMailAnswerSeparator' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    ALTER COLUMN EMailAnswerSeparator nvarchar(512) not null    
END
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.43'
GO

--ROLLBACK --TMP
