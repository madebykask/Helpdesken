-- update DB from 5.3.24 to 5.3.25 version
--New fields in tblFormField
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Label' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD Label nvarchar(200) NULL
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Show' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD Show int NOT NULL Default(0)
GO
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.25'
