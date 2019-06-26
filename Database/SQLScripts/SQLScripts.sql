--update DB from 5.3.42 to 5.3.43 version



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.43'
GO

--ROLLBACK --TMP
