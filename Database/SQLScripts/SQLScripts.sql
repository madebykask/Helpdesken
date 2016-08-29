-- update DB from 5.3.25 to 5.3.26 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LatestSLACountDate' and sysobjects.name = N'tblCase')
	ALTER TABLE tblCase ADD LatestSLACountDate DateTime NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LatestSLACountDate' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD LatestSLACountDate DateTime NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnCaseOverview' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD ShowOnCaseOverview int NOT NULL Default(1)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ConnectedButton' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD ConnectedButton int NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowInsideCase' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD ShowInsideCase int NOT NULL Default(1)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SetCurrentUserAsPerformer' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD SetCurrentUserAsPerformer int NULL
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PerformerSetUser_id' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD PerformerSetUser_id int NULL
Go


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OverWritePopUp' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD OverWritePopUp int NOT NULL Default(0)
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.26'

