--update DB from 5.3.48 to 5.3.49 version

RAISERROR ('Add Status to tblSystem', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Status' and sysobjects.name = N'tblSystem')
BEGIN
    ALTER TABLE tblSystem
    ADD [Status] int not null default 1
END



  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.49'
GO

