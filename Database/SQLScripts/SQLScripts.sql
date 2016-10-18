-- update DB from 5.3.27 to 5.3.28 version



if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SetCurrentUserAsPerformer' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD SetCurrentUserAsPerformer int NULL
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.28'

