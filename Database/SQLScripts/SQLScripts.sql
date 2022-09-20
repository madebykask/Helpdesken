--update DB from 5.3.56 to 5.3.57 version

Go
  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.57'
GO