-- update DB from 5.3.6.XX to 5.3.7.xx version

-- fix for old field names, we have discovered this bug on production 
-- 2015-04-28
UPDATE tblCaseSettings SET tblCaseName = Replace(tblCaseName, 'tblUsers.FirstName', 'Performer_User_Id')


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.7'
