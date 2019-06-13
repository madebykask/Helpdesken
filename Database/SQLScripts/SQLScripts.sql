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
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'EMailSubject' and Object_ID = Object_ID(N'dbo.tblMail2Ticket'))
   ALTER TABLE tblMail2Ticket
   ADD EMailSubject nvarchar(512) NULL
GO

RAISERROR('Creating index on tblCaseHistory', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('dbo.tblCaseHistory') AND name='idx_createddate_case_casetype_workinggroup')
BEGIN 
	CREATE NONCLUSTERED INDEX [idx_createddate_case_casetype_workinggroup] ON [dbo].[tblCaseHistory]
	(
		[CreatedDate] ASC
	)
	INCLUDE ( 	[Case_Id],
		[CaseType_Id],
		[WorkingGroup_Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO

RAISERROR('Creating type for ID list', 10, 1) WITH NOWAIT
IF type_id('dbo.IDList') IS NULL
BEGIN
   CREATE TYPE dbo.IDList AS TABLE
	(
		ID INT
	)
END
GO

RAISERROR('Adding stored procedure for historical case report', 10, 1) WITH NOWAIT
IF OBJECT_ID('ReportGetHistoricalData', 'P') IS NOT NULL
BEGIN
	DROP PROCEDURE ReportGetHistoricalData
END
GO

CREATE PROCEDURE [dbo].[ReportGetHistoricalData] 
	-- Add the parameters for the stored procedure here
	@caseStatus INT,
	@changeFrom DATETIME, 
	@changeTo DATETIME,
	@customerID INT,
	@changeWorkingGroups AS dbo.IDList READONLY,
	@registerFrom DATETIME,
	@registerTo DATETIME, 
	@closeFrom DATETIME, 
	@closeTo DATETIME, 
	@includeCasesWithHistoricalNoWorkingGroup BIT,
	@includeCasesWithNoWorkingGroup BIT,
	@includeCasesWithNoDepartment BIT,
	@administrators AS dbo.IDList READONLY, 
	@departments AS dbo.IDList READONLY, 
	@caseTypes AS dbo.IDList READONLY, 
	@productAreas AS dbo.IDList READONLY,
	@workingGroups AS dbo.IDList READONLY
AS
BEGIN
	DECLARE @checkChangeWorkingGroups INT = 0,
		@checkAdministrators INT = 0,
		@checkDepartments INT = 1,
		@checkCaseTypes INT = 0,
		@checkProductAreas INT = 0,
		@checkWorkingGroups INT = 1,
		@checkCaseStatus INT = CASE WHEN @caseStatus IS NULL THEN 0 ELSE 1 END,
		@checkChangeFrom INT = CASE WHEN @changeFrom IS NULL THEN 0 ELSE 1 END,
		@checkChangeTo INT = CASE WHEN @changeTo IS NULL THEN 0 ELSE 1 END,
		@checkRegisterFrom INT = CASE WHEN @registerFrom IS NULL THEN 0 ELSE 1 END,
		@checkRegisterTo INT = CASE WHEN @registerTo IS NULL THEN 0 ELSE 1 END,
		@checkCloseFrom INT = CASE WHEN @closeFrom IS NULL THEN 0 ELSE 1 END,
		@checkCloseTo INT = CASE WHEN @closeTo IS NULL THEN 0 ELSE 1 END,
		@checkCurrentCustomerOnly BIT = 0

	SELECT TOP 1 @checkChangeWorkingGroups = 1 FROM @changeWorkingGroups
	SELECT TOP 1 @checkAdministrators = 1 FROM @administrators
	--SELECT TOP 1 @checkDepartments = 1 FROM @departments
	SELECT TOP 1 @checkCaseTypes = 1 FROM @caseTypes
	SELECT TOP 1 @checkProductAreas = 1 FROM @productAreas
	--SELECT TOP 1 @checkWorkingGroups = 1 FROM @workingGroups

	SELECT @checkCurrentCustomerOnly = CASE WHEN (@checkAdministrators + @checkDepartments + @checkCaseTypes + @checkProductAreas + 
		@checkWorkingGroups + @checkCaseStatus + @checkRegisterFrom + @checkRegisterTo + @checkCloseFrom + 
		@checkCloseTo) > 0 THEN 1 ELSE 0 END	
	
	CREATE TABLE #rows 
	(
		CaseID INT,
		CaseTypeID INT,
		WorkingGroupID INT,
		Created DATETIME,
		R_ROW INT, 
		R_CASE INT,
		INDEX ixCase NONCLUSTERED(CaseID, CaseTypeID, WorkingGroupID, R_ROW, R_CASE)
	)

	INSERT INTO #rows(CaseId, CaseTypeID, Created, WorkingGroupID, R_ROW, R_CASE) 
	SELECT C.Id, CT.ID, CH.CreatedDate, WG.ID WorkginGroupId, 
		ROW_NUMBER() OVER (PARTITION BY C.Id, CT.ID, WG.ID ORDER BY CH.CreatedDate) R, 
		ROW_NUMBER() OVER (PARTITION BY C.Id ORDER BY CH.CreatedDate) R_CASE FROM tblCase C 
	JOIN tblCaseHistory CH ON CH.Case_Id = C.Id
	JOIN tblCaseType CT ON CH.CaseType_Id = CT.ID
	LEFT JOIN tblWorkingGroup WG ON CH.WorkingGroup_Id = WG.Id
	WHERE 1=1
	AND CH.Customer_Id = @customerID
	AND CT.Customer_Id = @customerID
	AND ((@checkCurrentCustomerOnly = 1 AND C.Customer_Id = @customerID) OR @checkCurrentCustomerOnly = 0)
	AND (@checkAdministrators = 0 OR EXISTS(SELECT ID FROM @administrators A WHERE C.CaseResponsibleUser_Id = A.ID))
	AND (@checkDepartments = 0 OR EXISTS(SELECT ID FROM @departments D WHERE C.Department_Id = D.ID) OR (@includeCasesWithNoDepartment = 1 AND C.Department_Id IS NULL))
	AND (@checkCaseTypes = 0 OR EXISTS(SELECT ID FROM @caseTypes CT WHERE C.CaseType_Id = CT.ID))
	AND (@checkProductAreas = 0 OR EXISTS(SELECT ID FROM @productAreas PA WHERE C.ProductArea_Id = PA.ID))
	AND (@checkWorkingGroups = 0 OR EXISTS(SELECT ID FROM @workingGroups WG WHERE C.WorkingGroup_Id = WG.ID) OR (@includeCasesWithNoWorkingGroup = 1 AND C.WorkingGroup_Id IS NULL))
	AND (@checkCaseStatus = 0 OR (@caseStatus = 1 AND C.FinishingDate IS NULL) OR (@caseStatus = 0 AND C.FinishingDate IS NOT NULL))
	AND (@checkChangeFrom = 0 OR CH.CreatedDate >= @changeFrom)
	AND (@checkChangeTo = 0 OR CH.CreatedDate <= @changeTo)
	AND (@checkRegisterFrom = 0 OR C.RegTime >= @registerFrom)
	AND (@checkRegisterTo = 0 OR C.RegTime <= @registerTo)
	AND (@checkCloseFrom = 0 OR C.FinishingDate >= @closeFrom)
	AND (@checkCloseTo = 0 OR C.FinishingDate <= @closeTo)
	GROUP BY CT.Id, CT.CaseType, WG.ID, C.ID, CH.CreatedDate
	ORDER BY C.Id

	SELECT R.*, CT.CaseType, WG.WorkingGroup FROM #rows R
	JOIN tblCaseType CT ON R.CaseTypeID = CT.ID
	LEFT JOIN tblWorkingGroup WG ON R.WorkingGroupID = WG.Id
	LEFT JOIN #rows R2 ON R.CaseID = R2.CaseID
		AND (R.WorkingGroupID = R2.WorkingGroupID
			OR (R.WorkingGroupID IS NULL AND R2.WorkingGroupID IS NULL))
		AND R2.CaseTypeID = R.CaseTypeID
		AND R2.R_ROW = R.R_ROW - 1
		AND R2.R_CASE = R.R_CASE - 1
	WHERE R2.CaseID IS NULL
	  		AND (@checkChangeWorkingGroups = 0 OR 
			EXISTS(SELECT ID FROM @changeWorkingGroups CWG WHERE R.WorkingGroupID = CWG.ID) OR 
			(@includeCasesWithHistoricalNoWorkingGroup = 1 AND R.WorkingGroupID IS NULL))
	ORDER BY CaseID

	DROP TABLE #rows
END

GO

RAISERROR('Adding feature toggle NEW_REPORTED_TIME_REPORT', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT * FROM tblFeatureToggle WHERE StrongName = 'NEW_REPORTED_TIME_REPORT')
BEGIN
	INSERT INTO [tblFeatureToggle](StrongName, Active, [Description]) 
	VALUES ('NEW_REPORTED_TIME_REPORT', 1, 'Use new Reported Time report implementation')
END
GO


RAISERROR('Adding feature toggle NEW_NUMBER_OF_CASES_REPORT', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT * FROM tblFeatureToggle WHERE StrongName = 'NEW_NUMBER_OF_CASES_REPORT')
BEGIN
	INSERT INTO [tblFeatureToggle](StrongName, Active, [Description]) 
	VALUES ('NEW_NUMBER_OF_CASES_REPORT', 1, 'Use new Number of Cases report implementation')
END


RAISERROR('Adding new TimeZoneId to tblCustomer', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TimeZoneId' and Object_ID = Object_ID(N'dbo.tblCustomer'))
   ALTER TABLE tblCustomer
   ADD TimeZoneId nvarchar(64) NOT NULL DEFAULT('W. Europe Standard Time')
GO


RAISERROR('Creating index idx_casehistory_casetype', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'idx_casehistory_casetype')	
BEGIN
	CREATE NONCLUSTERED INDEX [idx_casehistory_casetype]
		ON [dbo].[tblCaseHistory]([CaseType_Id] ASC, [CreatedDate] ASC)
		INCLUDE([Case_Id], [WorkingGroup_Id]);
END
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.41'
GO

--ROLLBACK --TMP
