--update DB from 5.3.39 to 5.3.40 version


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.40'
--ROLLBACK --TMP


