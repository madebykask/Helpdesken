DECLARE @CaseDocumentParagraphGUID UNIQUEIDENTIFIER,
	@CaseDocumentParagraphName NVARCHAR(MAX),
	@ParagraphTypeFooter int = 5,
	@CaseDocumentTextGUID UNIQUEIDENTIFIER, 
	@CaseDocumentParagraph_Id INT,
	@CaseDocumentTextName NVARCHAR(MAX),
	@TextSortOrder INT,
	@Text NVARCHAR(MAX),
	@SortOrder INT


BEGIN TRAN


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
VALUES(15, 'Change Terms & Conditions', '<Position Title (Local Job Name) of Reports To Line Manager>', 'tabs.OrganisationalAssignment.sections.STD_S_OrganisationHiring.instances[0].controls.ReportsToLineManagerPositionTitle', NULL)
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


-- ########################################## Paragraph - FOOTER with Initials
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





ROLLBACK