--update DB from 5.3.35 to 5.3.36 version



 
-- DROP Foreign key FK_tblCaseFile_tblUser

IF NOT EXISTS (
    SELECT *
    FROM sys.indexes AS si
    JOIN sys.objects AS so on si.object_id=so.object_id
    JOIN sys.schemas AS sc on so.schema_id=sc.schema_id
    WHERE 
        sc.name='dbo' /* Schema */
        AND so.name ='tblEMailLog' /* Table */
        AND si.name='FK_tblCaseHistory' /* Index */)
BEGIN
	RAISERROR('Create index tblEMailLog.CaseHistory_Id', 10, 1) WITH NOWAIT
	CREATE NONCLUSTERED INDEX [FK_tblCaseHistory] ON [dbo].[tblEMailLog]
	(
		[CaseHistory_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

RAISERROR('Foreign key tblProject_tblUsers_ProjectManager', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblProject_tblUsers') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblProject] WITH NOCHECK 
	ADD CONSTRAINT [FK_tblProject_tblUsers] FOREIGN KEY ([ProjectManager]) REFERENCES [dbo].[tblUsers] ([Id]);
END
GO

RAISERROR ('Update column InventoryNumber on table tblCase', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblCase')
BEGIN
    ALTER TABLE [dbo].[tblCase]
	ALTER COLUMN [InventoryNumber] nvarchar(60)
END

-- set NOCHECK constraint for Foreign Key FK_tblQuestionnaireCircularParticipant_tblCase
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblQuestionnaireCircularParticipant_tblCase') AND type = 'F')
BEGIN    
    ALTER TABLE [dbo].[tblQuestionnaireCircularPart] NOCHECK CONSTRAINT [FK_tblQuestionnaireCircularParticipant_tblCase]
END
GO
 
-- DROP Foreign key FK_tblCaseFile_tblUser
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCaseFile_tblUser') AND type = 'F')
BEGIN    
    ALTER TABLE [dbo].[tblCaseFile] DROP CONSTRAINT [FK_tblCaseFile_tblUser]
END
GO  



RAISERROR('Foreign key tblProject_tblUsers_ProjectManager', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblProject_tblUsers') AND type = 'F')
BEGIN
	ALTER TABLE [dbo].[tblProject] WITH NOCHECK 
	ADD CONSTRAINT [FK_tblProject_tblUsers] FOREIGN KEY ([ProjectManager]) REFERENCES [dbo].[tblUsers] ([Id]);
END
GO

RAISERROR ('Update column InventoryNumber on table tblCase', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblCase')
BEGIN
    ALTER TABLE [dbo].[tblCase]
	ALTER COLUMN [InventoryNumber] nvarchar(60)
END
GO

RAISERROR ('Update column InventoryNumber on table tblCaseHistory', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE [dbo].[tblCaseHistory]
	ALTER COLUMN [InventoryNumber] nvarchar(60)
END
GO

RAISERROR ('Update column ComputerName on table tblComputer', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ComputerName' and sysobjects.name = N'tblComputer')
BEGIN
    ALTER TABLE [dbo].[tblComputer]
	ALTER COLUMN [ComputerName] nvarchar(60) NOT NULL
END
GO

RAISERROR ('Update column PrinterName on table tblPrinter', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'PrinterName' and sysobjects.name = N'tblPrinter')
BEGIN
    ALTER TABLE [dbo].[tblPrinter]
	ALTER COLUMN [PrinterName] nvarchar(60) NOT NULL
END
GO

RAISERROR ('Update column ServerName on table tblServer', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ServerName' and sysobjects.name = N'tblServer')
BEGIN
    ALTER TABLE [dbo].[tblServer]
	ALTER COLUMN [ServerName] nvarchar(60) NOT NULL
END
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.36'

--ROLLBACK --TMP

