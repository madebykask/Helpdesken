--update DB from 5.3.43 to 5.3.44 version

RAISERROR ('Add column Operation to tblFileViewLog table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Operation' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ADD Operation int null    
END
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.44'
GO



