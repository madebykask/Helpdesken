--update DB from 5.3.47 to 5.3.48 version


RAISERROR('Increase NovelClient column lenght in tblComputer', 10, 1) WITH NOWAIT
ALTER TABLE dbo.tblComputer
  ALTER COLUMN NovellClient NVARCHAR(100) NOT NULL;


  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.48'
GO
