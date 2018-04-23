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

---------------------------------------------------------------------------------

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.37'
--ROLLBACK --TMP


