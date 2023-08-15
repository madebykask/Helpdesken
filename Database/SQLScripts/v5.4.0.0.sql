--update DB from 5.3.58.2 to 5.4.0.0 version


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.4.0.0'
GO