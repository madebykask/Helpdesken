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

RAISERROR('Creating column Type in table tblCaseSettings', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Type' and sysobjects.name = N'tblCaseSettings')
BEGIN
	RAISERROR('SET NOEXEC ON - No script execution untill SET NOEXEC OFF', 10, 1) WITH NOWAIT
	SET NOEXEC ON	
END

ALTER TABLE tblCaseSettings
ADD [Type] int NOT NULL DEFAULT(0) -- Case Overview type 

GO

DECLARE @UserGroupId int
DECLARE @Type int
DECLARE @CurrentDate datetime
SET @UserGroupId = 0
SET @CurrentDate = GETDATE();
SET @Type = 1
RAISERROR('Creating default values for AdvancedSearch in tblCaseSettings', 10, 1) WITH NOWAIT
INSERT INTO tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, 
	MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime, ChangeTime, ColStyle, CaseSettingsGUID, [Type]) 
VALUES
	(NULL, NULL, 'CaseNumber', 1, 40, @UserGroupId, 1, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'Persons_Name', 1, 100, @UserGroupId, 2, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'Caption', 1, 200, @UserGroupId, 3, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'Description', 1, 200, @UserGroupId, 4, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'Performer_User_Id', 1, 100, @UserGroupId, 5, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'WorkingGroup_Id', 1, 100, @UserGroupId, 6, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'Department_Id', 1, 100, @UserGroupId, 7, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type),
	(NULL, NULL, 'RegTime', 1, 50, @UserGroupId, 8, 0, @CurrentDate, @CurrentDate, NULL, NEWID(), @Type)

DECLARE @CustomerId int

DECLARE MY_CURSOR CURSOR 
	LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT DISTINCT CustomerId 
FROM tblCaseSettings
WHERE [Type] = 0 AND CustomerId IS NOT NULL

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @CustomerId
WHILE @@FETCH_STATUS = 0
BEGIN 	
	IF not exists (SELECT * FROM tblCaseSettings WHERE CustomerId = @CustomerId AND [Type] = 1) 
	BEGIN
		INSERT INTO tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, 
		MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime, ChangeTime, ColStyle, CaseSettingsGUID, [Type]) 
		SELECT @CustomerId, [User_Id], tblCaseName, Line, 
		MinWidth, 0, ColOrder, ShowInMobileList, RegTime, ChangeTime, ColStyle, CaseSettingsGUID,
		1 -- Advanced Search Type
		FROM tblCaseSettings
		WHERE UserGroup = 0 AND CustomerId IS NULL AND [Type] = 1

		PRINT @CustomerId
	END
	FETCH NEXT FROM MY_CURSOR INTO @CustomerId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

SET NOEXEC OFF
RAISERROR('SET NOEXEC OFF', 10, 1) WITH NOWAIT

GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.41'
--ROLLBACK --TMP






