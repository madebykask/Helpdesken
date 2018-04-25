--update DB from 5.3.36 to 5.3.37 version

-- New field in tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IntegrationType' and sysobjects.name = N'tblSettings')
   ALTER TABLE tblSettings ADD IntegrationType int NOT NULL Default(1)
GO

-- Add tblRegion to FTS catalog
IF EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[tblRegion]'))
	DROP FULLTEXT INDEX ON [dbo].[tblRegion]
GO

CREATE FULLTEXT INDEX ON [dbo].[tblRegion] (Region)  
    KEY INDEX [PK_tblRegion]
ON SearchCasesFTS  
WITH STOPLIST = SYSTEM
GO

-- IX_tblCase_Customer_Id: recreate to add new columns to index - Region_Id, User_Id
if exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id')
	DROP INDEX [IX_tblCase_Customer_Id] ON [dbo].[tblCase]
GO
if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id')
	CREATE NONCLUSTERED INDEX [IX_tblCase_Customer_Id] 
	   ON [dbo].[tblCase] (
		   [Customer_Id] ASC,		  
		   [Region_Id] ASC,
		   [Department_Id] ASC,
		   [User_Id] ASC,
		   [WorkingGroup_Id] ASC,
		   [FinishingDate] ASC,		   
		   [Deleted] ASC,
		   [RegTime] ASC)
	   INCLUDE (
		  [Id],
		  [Casenumber],
		  [Place],
		  [UserCode]) 
GO


RAISERROR ('Adding column AddFollowersBtn on table tblCaseSolution', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AddFollowersBtn' and sysobjects.name = N'tblCaseSolution')
   ALTER TABLE tblCaseSolution ADD AddFollowersBtn bit NULL

   INSERT INTO tblCaseSolutionFieldSettings (CaseSolution_Id, FieldName_Id, Mode, CreatedDate, ChangedDate)
		SELECT cs2.CaseSolution_Id, 67, cs2.Mode, GETDATE(), GETDATE()		
		FROM tblCaseSolutionFieldSettings as cs2
		LEFT JOIN tblCaseSolutionFieldSettings as cs1 on cs2.CaseSolution_Id = cs1.CaseSolution_Id AND cs1.FieldName_Id = 67
		WHERE cs2.FieldName_Id = 17 AND cs1.FieldName_Id is NULL
GO

RAISERROR ('Adding column SyncChangedDate on table tblPrinter', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SyncChangedDate' and sysobjects.name = N'tblPrinter')
   ALTER TABLE dbo.tblPrinter ADD SyncChangedDate datetime NULL
GO

---------------------------------------------------------------
-- Adding new SyncChangedDate field settings in InventoryTypes 
---------------------------------------------------------------
RAISERROR ('Adding SyncChangedDate field settings for InventoryTypes', 10, 1) WITH NOWAIT
DECLARE @CustomInventoryTypes TABLE (Id int) 
DECLARE @inventoryTypeId int = 0,
	   @itemsCount int = 0

-- fill temp table with inventory types that don't have SyncDate field settings
INSERT INTO @CustomInventoryTypes(Id)
select inv.Id
from tblInventoryType inv 
  LEFT JOIN tblInventoryTypeProperty inv_p on inv_p.InventoryType_Id = inv.Id AND PropertyType = -15
WHERE inv_p.Id IS NULL

SET @itemsCount = ISNULL((SELECT Count(*) from @CustomInventoryTypes),0)

-- add new SyncDate field settings for each inventory type
while (@itemsCount > 0)
BEGIN
    SELECT TOP 1 @inventoryTypeId = Id FROM @CustomInventoryTypes

    IF (NOT EXISTS(SELECT * FROM tblInventoryTypeProperty WHERE InventoryType_Id = @inventoryTypeId AND PropertyType = -15))
    BEGIN	   
	   --select @inventoryTypeId, NULL, 'Synkroniserad datum', 0, '', -15, 1000, 0,  0, 0, NULL, 0, GETDATE(), GETDATE()
	   INSERT INTO dbo.tblInventoryTypeProperty (InventoryType_Id, InventoryTypeGroup_Id, PropertyValue, PropertyPos, PropertyDefault, PropertyType, PropertySize, ShowInList,  Show, [ReadOnly], XMLTag, [Unique], ChangedDate, CreatedDate)
	   VALUES (@inventoryTypeId, NULL, 'Synkroniserad datum', 0, '', -15, 1000, 0,  0, 0, NULL, 0, GETDATE(), GETDATE())
    END

    DELETE FROM @CustomInventoryTypes WHERE Id = @inventoryTypeId
    SET @itemsCount = ISNULL((SELECT Count(*) from @CustomInventoryTypes),0)
END
------------------------------------------------------------


---------------------------------------------------------------------------------

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'PerformanceLogActive' and sysobjects.name = N'tblGlobalSettings')
BEGIN
		ALTER TABLE [dbo].[tblGlobalSettings]
		ADD [PerformanceLogActive] BIT NOT NULL DEFAULT(0),
        [PerformanceLogFrequency] INT NOT NULL DEFAULT(0),
		[PerformanceLogSettingsCache] INT NOT NULL DEFAULT(0);
END

--tblCustomerUser
ALTER TABLE tblCustomerUser
    ALTER COLUMN CaseRegionFilter NVARCHAR(100)


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.37'
--ROLLBACK --TMP


