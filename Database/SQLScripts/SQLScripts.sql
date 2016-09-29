-- update DB from 5.3.26 to 5.3.27 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ActionLeadTime' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD ActionLeadTime Int not NULL Default(0)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ActionExternalTime' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD ActionExternalTime Int not NULL Default(0)
Go

-- tblSettings 
ALTER TABLE tblSettings ALTER COLUMN LDAPFilter nvarchar(150)
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.27'

