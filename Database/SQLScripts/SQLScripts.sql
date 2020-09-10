--update DB from 5.3.47 to 5.3.48 version

RAISERROR('Increase NovelClient column lenght in tblComputer', 10, 1) WITH NOWAIT
ALTER TABLE dbo.tblComputer
  ALTER COLUMN NovellClient NVARCHAR(100) NOT NULL;

RAISERROR ('Add ServerFileName to tblServer', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ServerFileName' and sysobjects.name = N'tblServer')
BEGIN
    ALTER TABLE tblServer
    ADD ServerFileName nvarchar(100) 
END

RAISERROR ('Add ServerDocument to tblServer', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ServerDocument' and sysobjects.name = N'tblServer')
BEGIN
    ALTER TABLE tblServer
    ADD ServerDocument image   
END

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.48'
GO

