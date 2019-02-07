--update DB from 5.3.40 to 5.3.41 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowOnMobile' and sysobjects.name = N'tblCaseSolution')
BEGIN
    ALTER TABLE tblCaseSolution
    ADD ShowOnMobile int NOT NULL DEFAULT(0)        
END
GO



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.41'
--ROLLBACK --TMP






