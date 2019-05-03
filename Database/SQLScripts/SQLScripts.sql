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

RAISERROR('Adding new NewAdvancedSearch column in tblGlobalSettings table ', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'NewAdvancedSearch' and Object_ID = Object_ID(N'dbo.tblGlobalSettings'))
BEGIN
    ALTER TABLE tblGlobalSettings
    ADD [NewAdvancedSearch] int null
        
    DECLARE @SQLQuery AS NVARCHAR(500)
    SET @SQLQuery = N'UPDATE tblGlobalSettings SET NewAdvancedSearch = 1'
    EXECUTE sp_executesql @SQLQuery

    ALTER TABLE tblGlobalSettings 
    ALTER COLUMN NewAdvancedSearch int not null    
END
GO

RAISERROR('Changing tblOperationLog text columns to new size', 10, 1) WITH NOWAIT
GO

--creating temp procedure to remove default constraints if any 
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'tempRemoveDefaultConstaint')
  DROP PROCEDURE tempRemoveDefaultConstaint
GO
CREATE PROCEDURE tempRemoveDefaultConstaint(
    @tableName as nvarchar(128), 
    @columnName as nvarchar(128)) as
BEGIN        
        
    DECLARE @name as nvarchar(256)
    
    --get column default constraint 
    select @name = dc.name
    from sys.default_constraints dc
    join sys.objects o
	   on o.object_id = dc.parent_object_id
    join sys.columns c
	   on o.object_id = c.object_id
	   and c.column_id = dc.parent_column_id
    where o.name = @tableName
    and c.name = @columnName

    PRINT 'Found:' + ISNULL(@name, 'unknown')

    IF (LEN(@name) > 0) 
    BEGIN
	   DECLARE @cmd nvarchar(1024) 
	   SET @cmd = N'ALTER TABLE dbo.' + @tableName + ' DROP CONSTRAINT ' + @name
	   PRINT @cmd
	   EXEC (@cmd)
    END    
END
GO
    --remove default constraints
    exec tempRemoveDefaultConstaint @tableName = 'tblOperationLog', @columnName = 'LogText'
    GO    
    exec tempRemoveDefaultConstaint @tableName = 'tblOperationLog', @columnName = 'LogAction'
    GO
    exec tempRemoveDefaultConstaint @tableName = 'tblOperationLog', @columnName = 'LogTextExternal'
    GO
       
    ALTER TABLE tblOperationLog ALTER COLUMN LogAction NVARCHAR(MAX) NOT NULL    
    GO
    
    ALTER TABLE tblOperationLog ALTER COLUMN LogAction NVARCHAR(MAX) NOT NULL    
    GO 

    ALTER TABLE tblOperationLog  ALTER COLUMN LogText NVARCHAR(MAX) NOT NULL
    GO

    ALTER TABLE tblOperationLog
    ALTER COLUMN LogTextExternal NVARCHAR(MAX) NOT NULL   
    GO

    --restore default values constraints
    ALTER TABLE tblOperationLog ADD CONSTRAINT DF_tblOperationLog_LogText
    DEFAULT('') FOR [LogText]
    GO

    ALTER TABLE tblOperationLog ADD CONSTRAINT DF_tblOperationLog_LogAction
    DEFAULT('') FOR [LogAction]
    GO

    ALTER TABLE tblOperationLog ADD CONSTRAINT DF_tblOperationLog_LogTextExternal
    DEFAULT('') FOR [LogTextExternal]
    GO

-- drop temp proc
DROP PROCEDURE tempRemoveDefaultConstaint
GO

-- Create feature toogle
RAISERROR('Adding Feature toggle table tblFeatureToggle', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblFeatureToggle' AND type='U')
BEGIN
	CREATE TABLE [dbo].[tblFeatureToggle](
		[StrongName] [nvarchar](100) NOT NULL,
		[Active] [bit] NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[ChangeDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_tblFeatureToggle] PRIMARY KEY CLUSTERED 
	(
		[StrongName] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[tblFeatureToggle] ADD  CONSTRAINT [DF_tblFeatureToggle_Active]  DEFAULT ((0)) FOR [Active]
	ALTER TABLE [dbo].[tblFeatureToggle] ADD  CONSTRAINT [DF_tblFeatureToggle_Description]  DEFAULT ('') FOR [Description]
	ALTER TABLE [dbo].[tblFeatureToggle] ADD  CONSTRAINT [DF_tblFeatureToggle_ChangeDate]  DEFAULT (getutcdate()) FOR [ChangeDate]
END
GO

RAISERROR('Adding Feature toggle trigger trFeatureToggleChange', 10, 1) WITH NOWAIT
IF EXISTS (SELECT * FROM sysobjects WHERE name='trFeatureToggleChange' AND type='TR')
BEGIN	
	DROP TRIGGER [dbo].[trFeatureToggleChange] 
END
GO

CREATE TRIGGER [dbo].[trFeatureToggleChange] 
	ON  [dbo].[tblFeatureToggle] 
	AFTER UPDATE
AS 
BEGIN
	DECLARE @strongName NVARCHAR(MAX)
	UPDATE FT SET ChangeDate = GETUTCDATE() FROM inserted U
	JOIN tblFeatureToggle FT ON U.StrongName = FT.StrongName
END
GO

RAISERROR('Adding feature toggle REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT * FROM tblFeatureToggle WHERE StrongName = 'REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH')
BEGIN
	INSERT INTO [tblFeatureToggle](StrongName, Active, [Description]) 
	VALUES ('REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH',	0, 'Should the report generator use the previous implementation of the search method')
END
GO

RAISERROR('Adding feature toggle NEW_ADVANCED_CASE_SEARCH', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT * FROM tblFeatureToggle WHERE StrongName = 'NEW_ADVANCED_CASE_SEARCH')
BEGIN
	INSERT INTO [tblFeatureToggle](StrongName, Active, [Description]) 
	VALUES ('NEW_ADVANCED_CASE_SEARCH', 1, 'Use new advanced search feature')
END
GO

RAISERROR('Dropping NewAdvancedSearch column in NewAdvancedSearch', 10, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'NewAdvancedSearch' and Object_ID = Object_ID(N'dbo.tblGlobalSettings'))
   ALTER TABLE tblGlobalSettings DROP COLUMN NewAdvancedSearch
GO


RAISERROR('Adding new EMailSubject column to tblMail2Ticket', 10, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'EMailSubject' and Object_ID = Object_ID(N'dbo.tblMail2Ticket'))
   ALTER TABLE tblMail2Ticket
   ADD EMailSubject nvarchar(512) NULL
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.41'
GO

--ROLLBACK --TMP
