


BEGIN TRAN

-- ########################################## Basic properties of script

DECLARE @userID INT = 1,
	@now DATETIME = GETDATE()

-- ########################################## General variables (used for paragraph creation)

DECLARE @CaseDocumentParagraphGUID UNIQUEIDENTIFIER,
	@CaseDocumentParagraphName NVARCHAR(MAX),
	@CaseDocumentTextGUID UNIQUEIDENTIFIER, 
	@CaseDocumentParagraph_Id INT,
	@CaseDocumentTextName NVARCHAR(MAX),
	@TextSortOrder INT,
	@Text NVARCHAR(MAX),
	@ParagraphTypeFooter INT = 5


-- ########################################## Form GUIDS & IDs

DECLARE @dcHiringGuid UNIQUEIDENTIFIER = '33216A30-3CE1-40CE-8C15-40D2F364B547', -- DC EA (Hiring)
	@dcTcGuid UNIQUEIDENTIFIER = 'AB07E815-2578-444E-8C58-BD28FE0CD502', -- DC EA (T&C)
	@dcSalHiringGuid UNIQUEIDENTIFIER = 'D91F9293-C9C0-4F0F-88CA-AE30F04451CF', -- DC Salaried (Hiring)
	@dcSalTcGuid UNIQUEIDENTIFIER = 'ED5D9C91-69A7-4564-8A4B-FB141CA4C007', -- DC Salaried (T&C)
	@retHiringGuid UNIQUEIDENTIFIER = 'E4BF7D03-3360-4758-B3B3-58EEAAFBF701', -- Retail EA (Hiring)
	@retTcGuid UNIQUEIDENTIFIER = '0A574914-AA5C-4883-9AFA-A2B9C9115A10', -- Retail EA (T&C)
	@retSalHiringGuid UNIQUEIDENTIFIER = 'E11C26B9-C538-4BBB-AB9E-DB2D49D49CD4', -- Retail Salaried (Hiring)
	@retSalTcGuid UNIQUEIDENTIFIER = 'EA1D92DF-AF56-4D3E-BA70-C45E4C3C30DA' -- Retail Salaried (T&C)

DECLARE @dcHiringID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcHiringGuid), -- DC EA (Hiring)
	@dcTcID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcTcGuid), -- DC EA (T&C)
	@dcSalHiringID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcSalHiringGuid), -- DC Salaried (Hiring)
	@dcSalTcID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcSalTcGuid), -- DC Salaried (T&C)
	@retHiringID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retHiringGuid), -- Retail EA (Hiring)
	@retTcID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retTcGuid), -- Retail EA (T&C)
	@retSalHiringID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retSalHiringGuid), -- Retail Salaried (Hiring)
	@retSalTcID INT = (SELECT ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retSalTcGuid) -- Retail Salaried (T&C)


-- ########################################## tblCaseDocumentTextIdentifier - Populate

DECLARE @master_tblCaseDocumentTextIdentifier TABLE
(
	ExtendedCaseFormId INT,
	Process NVARCHAR(MAX),
	Identifier NVARCHAR(MAX),
	PropertyName NVARCHAR(MAX),
	DisplayName NVARCHAR(MAX)
)

SET NOCOUNT ON 

-- TODO bind using GUID for form specific identifiers (extended case forms does currently not have a value in their GUID column) JWE 20170818
-- tblCaseDocumentTextIdentifier
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, '', '<Business Unit>', 'Case.Department.DepartmentName', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, '', '<Co-worker First Name>', 'Case.PersonsName', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, '', '<Co-worker GlobalView ID>', 'Case.ReportedBy', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, '', '<Co-worker Last Name>', 'Case.PersonsPhone', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, '', '<Todays Date - Long>', 'Date.NowLong', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, '', '<Todays Date - Short>', 'Date.NowShort', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Address Line 1>', 'tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.AddressLine1', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Address Line 2>', 'tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.AddressLine2', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Address Line 3>', 'tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.AddressLine3', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Basic Pay Amount>', 'tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.BasicPayAmount', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Contract End Date>', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditionsHiring.instances[0].controls.ContractEndDate', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Contract Start Date>', 'tabs.ServiceRequestDetails.sections.AU_S_ProcessingDetailsHiring.instances[0].controls.ContractStartDate', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Contracted Hours>', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditionsHiring.instances[0].controls.ContractedHours', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Next Salary Review Year>', 'tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.NextSalaryReviewYear', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Position Title (Local Job Name) of Reports To Line Manager>', 'tabs.OrganisationalAssignment.sections.STD_S_OrganisationHiring.instances[0].controls.ReportsToLineManagerPositionTitle', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Position Title (Local Job Name)>', 'tabs.OrganisationalAssignment.sections.STD_S_JobHiring.instances[0].controls.PositionTitleLocalJobName', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Postal Code>', 'tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.PostalCode', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Reports To Line Manager>', 'tabs.OrganisationalAssignment.sections.STD_S_OrganisationHiring.instances[0].controls.ReportsToLineManager', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<Shift Type>', 'tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.ShiftType', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', '<State>', 'tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.State', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Address Line 1>', 'tabs.ServiceRequestDetails.sections.Address.instances[0].controls.AddressLine1', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Address Line 2>', 'tabs.ServiceRequestDetails.sections.Address.instances[0].controls.AddressLine2', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Address Line 3>', 'tabs.ServiceRequestDetails.sections.Address.instances[0].controls.AddressLine3', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Basic Pay Amount>', 'tabs.PaymentInformation.sections.STD_S_BasicPayChange.instances[0].controls.BasicPayAmount', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Change Valid From>', 'tabs.ServiceRequestDetails.sections.STD_S_DetailsCTC.instances[0].controls.ChangeValidFrom', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Change Valid To>', 'tabs.ServiceRequestDetails.sections.STD_S_DetailsCTC.instances[0].controls.ChangeValidTo', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Contracted Hours>', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditions.instances[0].controls.ContractedHours', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Position Title (Local Job Name) of Reports To Line Manager>', 'tabs.OrganisationalAssignment.sections.STD_S_OrganisationChange.instances[0].controls.ReportsToLineManagerPositionTitle', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Position Title (Local Job Name)>', 'tabs.OrganisationalAssignment.sections.STD_S_JobChange.instances[0].controls.PositionTitleLocalJobName', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Postal Code>', 'tabs.ServiceRequestDetails.sections.Address.instances[0].controls.PostalCode', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Reports To Line Manager>', 'tabs.OrganisationalAssignment.sections.STD_S_OrganisationChange.instances[0].controls.ReportsToLineManager', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<Shift Type>', 'tabs.PaymentInformation.sections.STD_S_BasicPayChange.instances[0].controls.ShiftType', NULL)
INSERT INTO @master_tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & Conditions', '<State>', 'tabs.ServiceRequestDetails.sections.Address.instances[0].controls.State', NULL)

SET NOCOUNT OFF

-- UPDATE Existing value
PRINT 'UPDATE changed values in tblCaseDocumentTextIdentifier'
UPDATE CDTI SET Process = M.Process,
	CDTI.DisplayName = M.DisplayName,
	CDTI.PropertyName = M.PropertyName
FROM tblCaseDocumentTextIdentifier CDTI
JOIN @master_tblCaseDocumentTextIdentifier M ON CDTI.ExtendedCaseFormId = M.ExtendedCaseFormId 
	AND CDTI.Identifier = M.Identifier
WHERE CDTI.Process <> M.Process OR 
	CDTI.DisplayName <> M.Process OR
	CDTI.PropertyName <> M.PropertyName


-- UPDATE Existing value
PRINT 'INSERT new values in tblCaseDocumentTextIdentifier'
INSERT INTO tblCaseDocumentTextIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
SELECT M.ExtendedCaseFormId, M.Process, M.Identifier, M.PropertyName, M.DisplayName 
FROM @master_tblCaseDocumentTextIdentifier M
LEFT JOIN tblCaseDocumentTextIdentifier CDTI ON CDTI.ExtendedCaseFormId = M.ExtendedCaseFormId 
	AND CDTI.Identifier = M.Identifier
WHERE CDTI.Id IS NULL



-- ########################################## tblCaseDocumentTextIdentifier - Populate

DECLARE @master_tblCaseDocumentTextConditionIdentifier TABLE
(
	ExtendedCaseFormId INT,
	Process NVARCHAR(MAX),
	Identifier NVARCHAR(MAX),
	PropertyName NVARCHAR(MAX),
	DisplayName NVARCHAR(MAX)
)

SET NOCOUNT ON 

-- tblCaseDocumentTextConditionIdentifier

INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, 'Hiring', 'case_BusinessUnit', 'Department.DepartmentName', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(0, 'Hiring', 'case_StateSecondaryID', 'StateSecondary.StateSecondaryId', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', 'extendedcase_ContractedHours', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditionsHiring.instances[0].controls.ContractedHours', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(13, 'Hiring', 'extendedcase_ContractEndDate', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditionsHiring.instances[0].controls.ContractEndDate', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & C', 'extendedcase_ChangeValidFrom', 'tabs.ServiceRequestDetails.sections.STD_S_Details.instances[0].controls.ChangeValidFrom', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & C', 'extendedcase_ChangeValidTo', 'tabs.ServiceRequestDetails.sections.STD_S_DetailsCTC.instances[0].controls.ChangeValidTo', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & C', 'extendedcase_ContractedHours', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditions.instances[0].controls.ContractedHours', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & C', 'extendedcase_ProbationPeriod', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditions.instances[0].controls.ProbationPeriod', NULL)
INSERT INTO @master_tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
VALUES(15, 'Change Terms & C', 'extendedcase_ExtensionFixedTerm', 'tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditions.instances[0].controls.ExtensionFixedPosition', NULL)

SET NOCOUNT OFF

-- UPDATE Existing value
PRINT 'UPDATE changed values in tblCaseDocumentTextConditionIdentifier'
UPDATE CDTCI SET Process = M.Process,
	CDTCI.DisplayName = M.DisplayName,
	CDTCI.PropertyName = M.PropertyName
FROM tblCaseDocumentTextConditionIdentifier CDTCI
JOIN @master_tblCaseDocumentTextConditionIdentifier M ON CDTCI.ExtendedCaseFormId = M.ExtendedCaseFormId 
	AND CDTCI.Identifier = M.Identifier
WHERE CDTCI.Process <> M.Process OR 
	CDTCI.DisplayName <> M.Process OR
	CDTCI.PropertyName <> M.PropertyName


-- UPDATE Existing value
PRINT 'INSERT new values in tblCaseDocumentTextConditionIdentifier'
INSERT INTO tblCaseDocumentTextConditionIdentifier(ExtendedCaseFormId, Process, Identifier, PropertyName, DisplayName)
SELECT M.ExtendedCaseFormId, M.Process, M.Identifier, M.PropertyName, M.DisplayName 
FROM @master_tblCaseDocumentTextConditionIdentifier M
LEFT JOIN tblCaseDocumentTextConditionIdentifier CDTI ON CDTI.ExtendedCaseFormId = M.ExtendedCaseFormId 
	AND CDTI.Identifier = M.Identifier
WHERE CDTI.Id IS NULL

-- ########################################## tblCaseDocumentCondition - Populate
SET NOCOUNT ON

DECLARE @master_tblCaseDocumentCondition TABLE
(
	[CaseDocumentConditionGUID] [uniqueidentifier] NULL,
	[CaseDocument_Id] [int] NULL,
	[Property_Name] [nvarchar](500) NULL,
	[Values] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Status] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedByUser_Id] [int] NULL,
	[ChangedDate] [datetime] NULL,
	[ChangedByUser_Id] [int] NULL
)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('92D4FE4F-A1E5-4922-A744-2BE1569D1056', @retHiringID, 'case_ProductArea.ProductAreaGUID', '10ED7779-E56E-4C75-B02C-4486D8029DCE', 'ProductArea = Hiring ', 1, '2017-07-18 21:14:38.753', 2, '2017-07-19 16:45:43.007', 2)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('D1A696B0-E14A-4270-91CB-7B9A56927E02', @retHiringID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.PayrollCategory', 'QW', 'PayRollCategory = Waged', 1, '2017-07-18 21:14:38.757', 2, '2017-07-19 16:45:43.010', 2)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('4EE7578E-876A-421E-B19C-00713449AB90', @retHiringID, 'case_Region.Code', '1300', 'Company Code = 1300', 1, '2017-07-18 21:14:38.760', 2, '2017-07-19 16:45:43.010', 2)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('3EA2EE2E-2941-4538-A043-F74B57BCF17B', @retSalHiringID, 'case_ProductArea.ProductAreaGUID', '10ED7779-E56E-4C75-B02C-4486D8029DCE', 'ProductArea = Hiring', 1, '2017-07-19 13:56:44.590', 2, '2017-07-19 13:56:44.590', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('F983798D-346C-45CD-B1DA-CABF5D07E8ED', @retSalHiringID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.PayrollCategory', 'QS', 'PayRollCategory = QS', 1, '2017-07-19 13:56:44.593', 2, '2017-07-19 13:56:44.593', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('A93301E6-717D-41C0-BCE2-1B8189EEF01C', @retSalHiringID, 'case_Region.Code', '1300', 'Company Code = 1300', 1, '2017-07-19 13:56:44.593', 2, '2017-07-19 13:56:44.593', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('1030B5FD-2632-4A54-A28D-70E90CCBE014', @dcHiringID, 'case_ProductArea.ProductAreaGUID', '10ED7779-E56E-4C75-B02C-4486D8029DCE', 'ProductArea = Hiring', 1, '2017-07-19 13:56:50.503', 2, '2017-07-19 13:56:50.503', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('6DE62B78-EC81-48FB-96D9-A2600A38B6ED', @dcHiringID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.PayrollCategory', 'QW', 'PayRollCategory = Waged', 1, '2017-07-19 13:56:50.503', 2, '2017-07-19 13:56:50.503', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('3DBB1385-0585-4D8F-A18D-71581C3EAE8C', @dcHiringID, 'case_Region.Code', '1301', 'Company Code = 1301', 1, '2017-07-19 13:56:50.503', 2, '2017-07-19 13:56:50.503', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('D38632D7-FACE-4720-B093-3670D32E805D', @dcSalHiringID, 'case_ProductArea.ProductAreaGUID', '10ED7779-E56E-4C75-B02C-4486D8029DCE', 'ProductArea = Hiring', 1, '2017-07-19 13:56:53.943', 2, '2017-07-19 13:56:53.943', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('9F3768D5-C2A2-4F84-BCBB-9C8FE28E3CBE', @dcSalHiringID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.PayrollCategory', 'QS', 'PayRollCategory = QS', 1, '2017-07-19 13:56:53.943', 2, '2017-07-19 13:56:53.943', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('B7568E08-B7A2-423C-9ABF-47A1E0366E6A', @dcSalHiringID, 'case_Region.Code', '1301', 'Company Code = 1301', 1, '2017-07-19 13:56:53.947', 2, '2017-07-19 13:56:53.947', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('DA3C54C7-FE34-474F-97A3-B83FB171B8B6', @retTcID, 'case_ProductArea.ProductAreaGUID', '1754BBBC-B572-459B-B7CF-CB024CDED347', 'ProductArea = Change Terms & Conditions', 1, '2017-07-19 13:56:58.417', 2, '2017-07-19 13:56:58.417', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('10619567-B44B-4DF3-B2F8-D2583959C92A', @retTcID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayChange.instances[0].controls.PayrollCategory', 'QW', 'PayRollCategory = Waged', 1, '2017-07-19 13:56:58.417', 2, '2017-07-19 13:56:58.417', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('1FF1FF67-C555-4A5B-B297-88796B2AA769', @retTcID, 'case_Region.Code', '1300', 'Company Code = 1300', 1, '2017-07-19 13:56:58.417', 2, '2017-07-19 13:56:58.417', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('A36B6131-3578-47CA-946A-7ADB53CDAFDF', @retSalTcID, 'case_ProductArea.ProductAreaGUID', '1754BBBC-B572-459B-B7CF-CB024CDED347', 'ProductArea = Change Terms & Conditions', 1, '2017-07-19 13:57:02.193', 2, '2017-07-19 13:57:02.193', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('85D091B7-9AC8-4ED2-AB6C-388B0D78619C', @retSalTcID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayChange.instances[0].controls.PayrollCategory', 'QS', 'PayRollCategory = Salaried', 1, '2017-07-19 13:57:02.193', 2, '2017-07-19 13:57:02.193', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('8127B044-380F-433F-BCAC-B99727487AEF', @retSalTcID, 'case_Region.Code', '1300', 'Company Code = 1300', 1, '2017-07-19 13:57:02.193', 2, '2017-07-19 13:57:02.193', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('58174BC3-882B-4175-B9A1-BA9B8B0571EB', @dcTcID, 'case_ProductArea.ProductAreaGUID', '1754BBBC-B572-459B-B7CF-CB024CDED347', 'ProductArea = Change Terms & Conditions', 1, '2017-07-19 13:57:06.010', 2, '2017-07-19 13:57:06.010', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('47629EF6-B561-40DB-92DE-01CD71376432', @dcTcID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayChange.instances[0].controls.PayrollCategory', 'QW', 'PayRollCategory = Waged', 1, '2017-07-19 13:57:06.010', 2, '2017-07-19 13:57:06.010', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('59F20986-8FA9-4520-B609-A0AD0EFE7122', @dcTcID, 'case_Region.Code', '1301', 'Company Code = 1301', 1, '2017-07-19 13:57:06.010', 2, '2017-07-19 13:57:06.010', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('10C156FC-67A2-4A74-B7DE-0430C66897DC', @dcSalTcID, 'case_ProductArea.ProductAreaGUID', '1754BBBC-B572-459B-B7CF-CB024CDED347', 'ProductArea = Change Terms & Conditions', 1, '2017-07-19 13:57:09.960', 2, '2017-07-19 13:57:09.960', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('5462E9F6-7993-47CA-84B6-D91F0FB392BA', @dcSalTcID, 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayChange.instances[0].controls.PayrollCategory', 'QS', 'PayRollCategory = Salaried', 1, '2017-07-19 13:57:09.960', 2, '2017-07-19 13:57:09.960', NULL)
INSERT INTO @master_tblCaseDocumentCondition(CaseDocumentConditionGuid, CaseDocument_Id, Property_Name, [Values], [Description],  [Status], CreatedDate, CreatedByUser_Id, ChangedDate, ChangedByUser_Id)
VALUES('4C791C34-CFC4-4380-BB6D-84783F774C0E', @dcSalTcID, 'case_Region.Code', '1301', 'Company Code = 1301', 1, '2017-07-19 13:57:09.960', 2, '2017-07-19 13:57:09.960', NULL)

SET NOCOUNT OFF
-- UPDATE Existing value
PRINT 'UPDATE changed values in tblCaseDocumentCondition'
UPDATE CDC SET 
	CaseDocument_Id = M.CaseDocument_Id,
	Property_Name = M.Property_Name,
	[Values] = M.[Values],
	[Description] = M.[Description],
	[Status] = M.[Status],
	ChangedDate = @now,
	ChangedByUser_Id = @userID
FROM tblCaseDocumentCondition CDC
JOIN @master_tblCaseDocumentCondition M ON CDC.CaseDocumentConditionGuid = M.CaseDocumentConditionGuid 
	AND CDC.CaseDocumentConditionGUID = M.CaseDocumentConditionGUID
WHERE CDC.CaseDocument_Id <> M.CaseDocument_Id OR 
	CDC.Property_Name <> M.Property_Name OR
	CDC.[Values] <> M.[Values] OR
	CDC.[Description] <> M.[Description] OR
	CDC.[Status] <> M.[Status]


-- INSERT new value
PRINT 'INSERT new values in tblCaseDocumentCondition'
INSERT INTO tblCaseDocumentCondition([CaseDocumentConditionGUID], 
	[CaseDocument_Id],
	[Property_Name],
	[Values],
	[Description],
	[Status],
	[CreatedDate],
	[CreatedByUser_Id],
	[ChangedDate],
	[ChangedByUser_Id])
SELECT M.[CaseDocumentConditionGUID], 
	M.[CaseDocument_Id],
	M.[Property_Name],
	M.[Values],
	M.[Description],
	M.[Status],
	@now,
	@userID,
	@now,
	@userID
FROM @master_tblCaseDocumentCondition M
LEFT JOIN tblCaseDocumentCondition CDC ON CDC.CaseDocumentConditionGUID = M.CaseDocumentConditionGUID 
WHERE CDC.Id IS NULL


-- ########################################## Paragraph - FOOTER with Initials
SET NOCOUNT ON
begin 
	set @CaseDocumentParagraphGUID = 'A7626F89-C428-475C-8E10-160CCE0F2B5D'
	set @CaseDocumentParagraphName = 'AU - FOOTER Initial'

	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Footer section with initials'
				   ,@ParagraphTypeFooter
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeFooter

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

		begin /* Text - Footer with intial */

		set @CaseDocumentTextGUID = '8D7DFEFF-F5DB-4A57-985B-AE22FDB85C76'
		set @CaseDocumentTextName  = 'Text - Footer initial'
		set @TextSortOrder = 1
	

		set @Text = 
		'<!DOCTYPE html>
<html>
	<head></head>
	<body style="text-align:center !important; width:100%;">
		<p style="color:#ccc; font-size:16px; font-family:Verdana !important;">
			<strong>Confidential</strong>
			<br />
			Contract with <Co-worker First Name> <Co-worker Last Name> dated <Todays Date - Short>
			<br />
			Initials: _____
		</p>
	</body>
</html>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* Text - CompanyText */

end /* Paragraph - FOOTER with Initials*/





COMMIT