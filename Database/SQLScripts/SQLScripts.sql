-- update DB from 5.3.13 to 5.3.14 version

if (SELECT COLUMNPROPERTY(OBJECT_ID('tblCaseSolution', 'U'), 'RegistrationSource', 'AllowsNull')) = 0
BEGIN
	ALTER TABLE tblCaseSolution ALTER COLUMN RegistrationSource int NULL
END

-- http://helpdesk5.dhsolutions.se/Cases/Edit/53386
if COL_LENGTH('tblDepartment','Department') != 400
BEGIN	
	alter table tblDepartment alter column Department nvarchar(200) not null
END
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.14'


