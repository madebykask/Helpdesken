--update DB from 5.3.44 to 5.3.45 version

RAISERROR ('Add SendMethod to tblMailTemplate table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'SendMethod' and sysobjects.name = N'tblMailTemplate')
BEGIN
    ALTER TABLE tblMailTemplate
    ADD SendMethod int not null default 0 
END

RAISERROR ('Add RemoveFileViewLogs to tblGDPRDataPrivacyFavorite table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'RemoveFileViewLogs' and sysobjects.name = N'tblGDPRDataPrivacyFavorite')
BEGIN
    ALTER TABLE tblGDPRDataPrivacyFavorite
    ADD RemoveFileViewLogs bit not null default 1 
END

RAISERROR ('Increasing length of tblCaseHistory.ClosingReason', 10, 1) WITH NOWAIT
ALTER TABLE tblCaseHistory
ALTER COLUMN ClosingReason NVARCHAR(300) NULL

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.45'
GO

