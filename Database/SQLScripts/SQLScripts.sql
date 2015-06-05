-- update DB from 5.3.8.XX to 5.3.9.xx version

ALTER TABLE tblSettings ALTER COLUMN LDAPFilter nvarchar(100) NOT NULL

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.9'
