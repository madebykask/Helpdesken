-- update DB from 5.3.25 to 5.3.26 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LatestSLACountDate' and sysobjects.name = N'tblCase')
	ALTER TABLE tblCase ADD LatestSLACountDate DateTime NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LatestSLACountDate' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD LatestSLACountDate DateTime NULL 
Go


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.26'

