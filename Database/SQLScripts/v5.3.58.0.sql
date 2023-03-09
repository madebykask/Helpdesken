--update DB from 5.3.57 to 5.3.58 version
UPDATE tblUsers
SET [Password] = 'kattapladaskis',
[PasswordChangedDate] = GetDate()
WHERE UserId = 'kattis'
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.58'
GO