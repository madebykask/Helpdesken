-- update DB from 5.3.13 to 5.3.14 version

if (SELECT COLUMNPROPERTY(OBJECT_ID('tblCaseSolution', 'U'), 'RegistrationSource', 'AllowsNull')) = 0
BEGIN
	ALTER TABLE tblCaseSolution ALTER COLUMN RegistrationSource int NULL
END


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.14'


