-- update DB from 5.3.32 to 5.3.33 version



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.33'

--UPDATE tblCaseSolutionCondition field length
ALTER TABLE tblCaseSolutionCondition
ALTER COLUMN [Values] nvarchar(4000)