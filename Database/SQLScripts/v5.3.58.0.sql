--update DB from 5.3.57 to 5.3.58 version
UPDATE tblUsers
SET [Password] = 'ds1'
WHERE UserId = 'ds'
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.58'
GO