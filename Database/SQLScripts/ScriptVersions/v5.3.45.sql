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

RAISERROR ('Setting length of tblCaseHistory.ClosingReason to NVARCHAR(300)', 10, 1) WITH NOWAIT
ALTER TABLE tblCaseHistory
ALTER COLUMN ClosingReason NVARCHAR(300) NULL


RAISERROR ('Setting length of tblComputerType.ComputerType to NVARCHAR(60)', 10, 1) WITH NOWAIT
ALTER TABLE tblComputerType
ALTER COLUMN ComputerType NVARCHAR(60) NULL

RAISERROR ('Changing ReportGetHistoricalData stored procedure', 10, 1) WITH NOWAIT
if exists(select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where sysobjects.name = N'ReportGetHistoricalData')
BEGIN
	EXECUTE sp_executesql N'ALTER PROCEDURE [dbo].[ReportGetHistoricalData] 
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
		@administrators AS dbo.IDList READONLY, 
		@departments AS dbo.IDList READONLY, 
		@caseTypes AS dbo.IDList READONLY, 
		@productAreas AS dbo.IDList READONLY,
		@workingGroups AS dbo.IDList READONLY
	AS
	BEGIN
		DECLARE @checkChangeWorkingGroups INT = 0,
			@checkAdministrators INT = 0,
			@checkDepartments INT = 0,
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
		SELECT TOP 1 @checkDepartments = 1 FROM @departments
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
			ROW_NUMBER() OVER (PARTITION BY C.Id, WG.ID ORDER BY CH.CreatedDate) R, 
			ROW_NUMBER() OVER (PARTITION BY C.Id ORDER BY CH.CreatedDate) R_CASE FROM tblCase C 
		JOIN tblCaseHistory CH ON CH.Case_Id = C.Id
		JOIN tblCaseType CT ON CH.CaseType_Id = CT.ID
		LEFT JOIN tblWorkingGroup WG ON CH.WorkingGroup_Id = WG.Id
		WHERE 1=1
		AND CH.Customer_Id = @customerID
		AND CT.Customer_Id = @customerID
		AND ((@checkCurrentCustomerOnly = 1 AND C.Customer_Id = @customerID) OR @checkCurrentCustomerOnly = 0)
		AND (@checkAdministrators = 0 OR EXISTS(SELECT ID FROM @administrators A WHERE C.CaseResponsibleUser_Id = A.ID))
		AND (@checkDepartments = 0 OR EXISTS(SELECT ID FROM @departments D WHERE C.Department_Id = D.ID))
		AND (@checkCaseTypes = 0 OR EXISTS(SELECT ID FROM @caseTypes CT WHERE C.CaseType_Id = CT.ID))
		AND (@checkProductAreas = 0 OR EXISTS(SELECT ID FROM @productAreas PA WHERE C.ProductArea_Id = PA.ID))
		AND (@checkWorkingGroups = 0 OR EXISTS(SELECT ID FROM @workingGroups WG WHERE C.WorkingGroup_Id = WG.ID) 
			OR (@includeCasesWithNoWorkingGroup = 1 AND C.WorkingGroup_Id IS NULL))
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
			AND (R2.WorkingGroupID = R.WorkingGroupID
				OR (R2.WorkingGroupID IS NULL AND R.WorkingGroupID IS NULL))
			AND R2.R_ROW = R.R_ROW - 1
			AND R2.R_CASE = R.R_CASE - 1
		WHERE R2.CaseID IS NULL
	  			AND (@checkChangeWorkingGroups = 0 OR 
				EXISTS(SELECT ID FROM @changeWorkingGroups CWG WHERE R.WorkingGroupID = CWG.ID) OR 
				(@includeCasesWithHistoricalNoWorkingGroup = 1 AND R.WorkingGroupID IS NULL))
		ORDER BY CaseID

		DROP TABLE #rows
	END'
END


RAISERROR ('Renaming NEW_ADVANCED_CASE_SEARCH to USE_DEPRICATED_ADVANCED_CASE_SEARCH in tblFeatureToggle', 10, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM tblFeatureToggle FT WHERE FT.StrongName = 'NEW_ADVANCED_CASE_SEARCH')
BEGIN
	UPDATE tblFeatureToggle
	SET
	 StrongName = 'USE_DEPRICATED_ADVANCED_CASE_SEARCH',
	 Active = 0,
	 Description = 'Use old advanced search feature'
	 WHERE StrongName = 'NEW_ADVANCED_CASE_SEARCH'
END

RAISERROR ('Renaming NEW_REPORTED_TIME_REPORT to USE_DEPRICATED_REPORTED_TIME_REPORT in tblFeatureToggle', 10, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM tblFeatureToggle FT WHERE FT.StrongName = 'NEW_REPORTED_TIME_REPORT')
BEGIN
	UPDATE tblFeatureToggle
	SET
	 StrongName = 'USE_DEPRICATED_REPORTED_TIME_REPORT',
	 Active = 0,
	 Description = 'Use old Reported Time report implementation'
	 WHERE StrongName = 'NEW_REPORTED_TIME_REPORT'
END

RAISERROR ('Renaming NEW_NUMBER_OF_CASES_REPORT to USE_DEPRICATED_NUMBER_OF_CASES_REPORT in tblFeatureToggle', 10, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM tblFeatureToggle FT WHERE FT.StrongName = 'NEW_NUMBER_OF_CASES_REPORT')
BEGIN
	UPDATE tblFeatureToggle
	SET
	 StrongName = 'USE_DEPRICATED_NUMBER_OF_CASES_REPORT',
	 Active = 0,
	 Description = 'Use old Number of Cases report implementation'
	 WHERE StrongName = 'NEW_NUMBER_OF_CASES_REPORT'
END

RAISERROR ('Changing module name in tblModule', 10, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM tblModule WHERE [Name] = 'Ärendeöversikt')
BEGIN
	UPDATE tblModule
	SET
	 [Name] = 'Sammanfattning - Ärende'
	 WHERE [Name] = 'Ärendeöversikt'
END
RAISERROR ('Add column UseMobileRouting to tblSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UseMobileRouting' and sysobjects.name = N'tblSettings')
BEGIN
	ALTER TABLE tblSettings
	ADD UseMobileRouting BIT NOT NULL DEFAULT(0)
END

RAISERROR ('Add column MobileSiteUrl to tblGlobalSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'MobileSiteUrl' and sysobjects.name = N'tblGlobalSettings')
BEGIN
	ALTER TABLE tblGlobalSettings
	ADD MobileSiteUrl NVARCHAR(MAX) NOT NULL DEFAULT('')
END

RAISERROR ('Add UseInitiatorAutocomplete to tblCustomer table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UseInitiatorAutocomplete' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD UseInitiatorAutocomplete bit not null default 1 
END



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.45'
GO

