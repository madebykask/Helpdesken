-- update DB from 5.3.11 to 5.3.12 version

	-- Nytt fält i tblCaseSolution
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RegistrationSource' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD RegistrationSource int Default(0) NOT NULL
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.12'
