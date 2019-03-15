--update DB from 5.3.40 to 5.3.41 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowOnMobile' and sysobjects.name = N'tblCaseSolution')
BEGIN
    ALTER TABLE tblCaseSolution
    ADD ShowOnMobile int NOT NULL DEFAULT(0)        
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryViewPermission' and sysobjects.name = N'tblUsers')
BEGIN
    ALTER TABLE tblUsers
    ADD InventoryViewPermission int NOT NULL DEFAULT(0)        
END
GO

--Update Users with InventoryViewPermission = 1 where InventoryPermission = 1
Update tblUsers Set InventoryViewPermission = 1 
Where InventoryPermission = 1

RAISERROR('Creating index IX_tblCaseHistory_Case_Id', 10, 1) WITH NOWAIT
if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCaseHistory_Case_Id')	
    CREATE NONCLUSTERED INDEX [IX_tblCaseHistory_Case_Id] ON [dbo].[tblCaseHistory]
    (
	   [Case_Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.41'
--ROLLBACK --TMP






