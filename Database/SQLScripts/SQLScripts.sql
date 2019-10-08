--update DB from 5.3.43 to 5.3.44 version

RAISERROR ('Add column Operation to tblFileViewLog table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Operation' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ADD Operation int null    
END
GO

RAISERROR ('Adding toggle for usage of file access logging. DISABLE_LOG_VIEW_CASE_FILE', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM tblFeatureToggle FT WHERE FT.StrongName = 'DISABLE_LOG_VIEW_CASE_FILE')
BEGIN
	INSERT INTO tblFeatureToggle(Active, ChangeDate, [Description], StrongName)
	SELECT 1, GETDATE(), 'Toogle for activating case file access logging', 'DISABLE_LOG_VIEW_CASE_FILE'
END
GO

RAISERROR ('Add column UserName to tblFileViewLog table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UserName' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ADD UserName nvarchar(200) null    
END
GO


RAISERROR ('Change column User_Id to tblFileViewLog table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'User_Id' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ALTER COLUMN [User_Id] int null    
END
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.44'
GO



