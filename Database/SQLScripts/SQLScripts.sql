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
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'User_Id' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ALTER COLUMN [User_Id] int null    
END
GO

RAISERROR ('Change size of tblEmailLog.MessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblEmailLog
ALTER COLUMN MessageId NVARCHAR(300) NULL



RAISERROR ('Change tblProblemLog.CreatedDate to default UTC date', 10, 1) WITH NOWAIT
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblProblemLog_CreatedDate]') AND type = 'D')
BEGIN 
	ALTER TABLE [dbo].[tblProblemLog] DROP  CONSTRAINT [DF_tblProblemLog_CreatedDate] 
END	
ALTER TABLE [dbo].[tblProblemLog] ADD  CONSTRAINT [DF_tblProblemLog_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]

GO

RAISERROR ('Change tblProblemLog.ChangeDate to default UTC date', 10, 1) WITH NOWAIT
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblProblemLog_ChangedDate]') AND type = 'D')
BEGIN 
	ALTER TABLE [dbo].[tblProblemLog] DROP  CONSTRAINT [DF_tblProblemLog_ChangedDate]  
END
ALTER TABLE [dbo].[tblProblemLog] ADD  CONSTRAINT [DF_tblProblemLog_ChangedDate]  DEFAULT (getutcdate()) FOR [ChangedDate]



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.44'
GO



