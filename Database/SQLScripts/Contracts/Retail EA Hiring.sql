--########################################
--########## RET EA HIR ##################
--########################################

BEGIN TRAN


-- #################################### General paragraphs & values ####################################

-- Get Logo info
DECLARE @logoGuid UNIQUEIDENTIFIER = 'EB0434AA-0BBF-4CA8-AB0A-BF853129FB9D'
DECLARE @logoID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @logoGuid)

-- Get footer info
DECLARE @footerGuid UNIQUEIDENTIFIER = 'd43619b6-be1c-4def-af32-460cf8d38f63'
DECLARE @footerID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @footerGuid)

-- Get address and company info
DECLARE @addressInfoGuid UNIQUEIDENTIFIER = '3E55AA5C-B241-4C01-9DB3-837B07118BF7'
DECLARE @addressInfoID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @addressInfoGuid)

-- Draft ID
DECLARE @draftGuid UNIQUEIDENTIFIER = '51220147-E756-492E-88A1-C1671BDE6AA5'
DECLARE @draftID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @draftGuid)

-- Paragraph types
DECLARE @ParagraphTypeText INT = 1
DECLARE @ParagraphTypeTableNumeric INT = 2
DECLARE @ParagraphTypeLogo INT = 3
DECLARE @ParagraphTypeTableTwoColumns INT = 4
DECLARE @ParagraphTypeFooter INT = 5
DECLARE @ParagraphTypeDraft INT = 6
DECLARE @ParagraphTypeHeader INT = 7

DECLARE @userID INT = 2
DECLARE @now DATETIME = GETDATE()

DECLARE @prefix NVARCHAR(MAX) = 'RET EA HIR'

-- #################################### Contract Clusters – Retail EA (Hiring) ####################################

-- Get the form
DECLARE @retHiringGuid UNIQUEIDENTIFIER = 'E4BF7D03-3360-4758-B3B3-58EEAAFBF701'
DECLARE @retHiringID INT, @counter INT = 0
SELECT @retHiringID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retHiringGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @retHiringGuid

-- #################################### Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @footerID, @counter
SET @counter = @counter + 1

-- #################################### Address and company info
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @addressInfoID, @counter
SET @counter = @counter + 1


-- #################################### Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @retHiringEmployGreetingGuid UNIQUEIDENTIFIER = 'AC6891B8-18CF-45F3-A4C0-CC27294C6DF2',
	@retHiringEmployGreetingName NVARCHAR(MAX) = @prefix + ' Greeting',
	@retHiringEmployGreetingParagraphType INT = @ParagraphTypeText,
	@retHiringEmployGreetingDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retHiringEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retHiringEmployGreetingName, @retHiringEmployGreetingDescription, @retHiringEmployGreetingParagraphType, @retHiringEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retHiringEmployGreetingName, [Description] = @retHiringEmployGreetingDescription, ParagraphType = @retHiringEmployGreetingParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retHiringEmployGreetingGuid
END
DECLARE @retHiringEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retHiringEmployGreetingGuid)

---- Create or update text A, No change valid to end date
DECLARE @retHiringEmployGreetingTextAGuid UNIQUEIDENTIFIER = '8E52C1BE-9CB5-43DF-87BF-E4EA8B9A91E2',
	@retHiringEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Greeting, no contract end date',
	@retHiringEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@retHiringEmployGreetingTextAText NVARCHAR(MAX) = 'IKEA Pty Ltd (IKEA) is pleased to present you with a contract of employment under the terms and conditions of the IKEA Enterprise Agreement 2017.',
	@retHiringEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@retHiringEmployGreetingTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringEmployGreetingID, 
		@retHiringEmployGreetingTextAName, 
		@retHiringEmployGreetingTextADescription,
		@retHiringEmployGreetingTextAText, 
		@retHiringEmployGreetingTextAHeadline,
		@retHiringEmployGreetingTextASortOrder,
		@retHiringEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringEmployGreetingID,
		[Name] = @retHiringEmployGreetingTextAName, 
		[Description] = @retHiringEmployGreetingTextADescription, 
		[Text] = @retHiringEmployGreetingTextAText,
		[Headline] = @retHiringEmployGreetingTextAHeadline,
		SortOrder = @retHiringEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringEmployGreetingTextAGuid
END
DECLARE @retHiringEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringEmployGreetingTextAGuid)

-- Create condition for Text A,  Has change valid to end date
DECLARE @retHiringEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = 'C547488F-E403-471F-A1F7-A36948018CB9',
	@retHiringEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retHiringEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retHiringEmployGreetingTextACondAValues NVARCHAR(MAX) = '',
	@retHiringEmployGreetingTextACondADescription NVARCHAR(MAX) = 'Has no contract end date',
	@retHiringEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringEmployGreetingTextACondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringEmployGreetingTextACondAGuid,
		@retHiringEmployGreetingTextAID,
		@retHiringEmployGreetingTextACondAPropertyName,
		@retHiringEmployGreetingTextACondAOperator,
		@retHiringEmployGreetingTextACondAValues,
		@retHiringEmployGreetingTextACondADescription,
		@retHiringEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringEmployGreetingTextAID,
		Property_Name = @retHiringEmployGreetingTextACondAPropertyName,
		Operator = @retHiringEmployGreetingTextACondAOperator,
		[Values] = @retHiringEmployGreetingTextACondAValues,
		[Description] = @retHiringEmployGreetingTextACondADescription,
		[Status] = @retHiringEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @retHiringEmployGreetingTextBGuid UNIQUEIDENTIFIER = 'E8B2F985-D317-4E06-9934-7593AC6C1A1E',
	@retHiringEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Greeting, has contract end date',
	@retHiringEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@retHiringEmployGreetingTextBText NVARCHAR(MAX) = 'IKEA Pty Ltd (IKEA) is pleased to present you with a temporary contract of employment under the terms and conditions of the IKEA Enterprise Agreement 2017.',
	@retHiringEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@retHiringEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringEmployGreetingID, 
		@retHiringEmployGreetingTextBName, 
		@retHiringEmployGreetingTextBDescription,
		@retHiringEmployGreetingTextBText, 
		@retHiringEmployGreetingTextBHeadline,
		@retHiringEmployGreetingTextBSortOrder,
		@retHiringEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringEmployGreetingID,
		[Name] = @retHiringEmployGreetingTextBName, 
		[Description] = @retHiringEmployGreetingTextBDescription, 
		[Text] = @retHiringEmployGreetingTextBText,
		[Headline] = @retHiringEmployGreetingTextBHeadline,
		SortOrder = @retHiringEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringEmployGreetingTextBGuid
END
DECLARE @retHiringEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @retHiringEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = '6B682E31-49E6-4A85-9251-69F279ECBFB1',
	@retHiringEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retHiringEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retHiringEmployGreetingTextBCondAValues NVARCHAR(MAX) = '',
	@retHiringEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Has contract end date',
	@retHiringEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringEmployGreetingTextBCondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringEmployGreetingTextBCondAGuid,
		@retHiringEmployGreetingTextBID,
		@retHiringEmployGreetingTextBCondAPropertyName,
		@retHiringEmployGreetingTextBCondAOperator,
		@retHiringEmployGreetingTextBCondAValues,
		@retHiringEmployGreetingTextBCondADescription,
		@retHiringEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringEmployGreetingTextBID,
		Property_Name = @retHiringEmployGreetingTextBCondAPropertyName,
		Operator = @retHiringEmployGreetingTextBCondAOperator,
		[Values] = @retHiringEmployGreetingTextBCondAValues,
		[Description] = @retHiringEmployGreetingTextBCondADescription,
		[Status] = @retHiringEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringEmployGreetingTextBCondAGuid
END

-- Add greeting paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @retHiringEmployGreetingID, @counter
SET @counter = @counter + 1


-- ################################# Begin two column table 


---- Create or update paragraph
-- Paragraph guid
DECLARE @retHiringTableGuid UNIQUEIDENTIFIER = '9138949C-F253-4803-9E0E-4643811D5DEA',
	@retHiringTableName NVARCHAR(MAX) = @prefix + ' Table',
	@retHiringTableParagraphType INT = @ParagraphTypeTableTwoColumns,
	@retHiringTableDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retHiringTableGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retHiringTableName, @retHiringTableDescription, @retHiringTableParagraphType, @retHiringTableGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retHiringTableName, [Description] = @retHiringTableDescription, ParagraphType = @retHiringTableParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retHiringTableGuid
END
DECLARE @retHiringTableID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retHiringTableGuid)

DECLARE @tableCounter INT = 0

-- #################################### Classification
DECLARE @retHiringTableClassGuid UNIQUEIDENTIFIER = '350AE198-CABA-47C9-BA6D-10441E0DD3D7',
	@retHiringTableClassName NVARCHAR(MAX) = @prefix + ' Classification',
	@retHiringTableClassDescription NVARCHAR(MAX) = '',
	@retHiringTableClassText NVARCHAR(MAX) = '<Position Title (Local Job Name)>',
	@retHiringTableClassHeadline NVARCHAR(MAX) = 'Classification:',
	@retHiringTableClassSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableClassGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableClassName, 
		@retHiringTableClassDescription,
		@retHiringTableClassText, 
		@retHiringTableClassHeadline,
		@retHiringTableClassSortOrder,
		@retHiringTableClassGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableClassName, 
		[Description] = @retHiringTableClassDescription, 
		[Text] = @retHiringTableClassText,
		[Headline] = @retHiringTableClassHeadline,
		SortOrder = @retHiringTableClassSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableClassGuid
END


-- #################################### Location
DECLARE @retHiringTableLocGuid UNIQUEIDENTIFIER = '53CF156C-7029-4D46-8C4B-0BF44A4FDE82',
	@retHiringTableLocName NVARCHAR(MAX) = @prefix + ' Location',
	@retHiringTableLocDescription NVARCHAR(MAX) = '',
	@retHiringTableLocText NVARCHAR(MAX) = '<Business Unit>',
	@retHiringTableLocHeadline NVARCHAR(MAX) = 'Location:',
	@retHiringTableLocSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableLocGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableLocName, 
		@retHiringTableLocDescription,
		@retHiringTableLocText, 
		@retHiringTableLocHeadline,
		@retHiringTableLocSortOrder,
		@retHiringTableLocGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableLocName, 
		[Description] = @retHiringTableLocDescription, 
		[Text] = @retHiringTableLocText,
		[Headline] = @retHiringTableLocHeadline,
		SortOrder = @retHiringTableLocSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableLocGuid
END

-- #################################### Dates

-- A. Has no contract end date (Effective date)
DECLARE @retHiringTableDateAGuid UNIQUEIDENTIFIER = 'C1122EE9-4EA2-4365-B091-F2AC6C8915ED',
	@retHiringTableDateAName NVARCHAR(MAX) = @prefix + ' Date, no valid to',
	@retHiringTableDateADescription NVARCHAR(MAX) = '',
	@retHiringTableDateAText NVARCHAR(MAX) = '<Contract Start Date>',
	@retHiringTableDateAHeadline NVARCHAR(MAX) = 'Effective date:',
	@retHiringTableDateASortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableDateAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableDateAName, 
		@retHiringTableDateADescription,
		@retHiringTableDateAText, 
		@retHiringTableDateAHeadline,
		@retHiringTableDateASortOrder,
		@retHiringTableDateAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableDateAName, 
		[Description] = @retHiringTableDateADescription, 
		[Text] = @retHiringTableDateAText,
		[Headline] = @retHiringTableDateAHeadline,
		SortOrder = @retHiringTableDateASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableDateAGuid
END
DECLARE @retHiringTableDateAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableDateAGuid)

-- A. Condition (has no contract end date)
DECLARE @retHiringTableDateACondAGuid UNIQUEIDENTIFIER = 'CF362805-DA68-4548-A5F5-8A9E47577436',
	@retHiringTableDateACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retHiringTableDateACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retHiringTableDateACondAValues NVARCHAR(MAX) = '',
	@retHiringTableDateACondADescription NVARCHAR(MAX) = 'Has no contract end date',
	@retHiringTableDateACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableDateACondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableDateACondAGuid,
		@retHiringTableDateAID,
		@retHiringTableDateACondAPropertyName,
		@retHiringTableDateACondAOperator,
		@retHiringTableDateACondAValues,
		@retHiringTableDateACondADescription,
		@retHiringTableDateACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableDateAID,
		Property_Name = @retHiringTableDateACondAPropertyName,
		Operator = @retHiringTableDateACondAOperator,
		[Values] = @retHiringTableDateACondAValues,
		[Description] = @retHiringTableDateACondADescription,
		[Status] = @retHiringTableDateACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableDateACondAGuid
END

-- B. Has contract end date (Temporary Contract Period)
DECLARE @retHiringTableDateBGuid UNIQUEIDENTIFIER = '445B4588-CD93-4F51-BC70-C2B2FD1B3810',
	@retHiringTableDateBName NVARCHAR(MAX) = @prefix + ' Date, has contract end date',
	@retHiringTableDateBDescription NVARCHAR(MAX) = '',
	@retHiringTableDateBText NVARCHAR(MAX) = 'from <Contract Start Date> to <Contract End Date>',
	@retHiringTableDateBHeadline NVARCHAR(MAX) = 'Temporary Contract Period:',
	@retHiringTableDateBSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableDateBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableDateBName, 
		@retHiringTableDateBDescription,
		@retHiringTableDateBText, 
		@retHiringTableDateBHeadline,
		@retHiringTableDateBSortOrder,
		@retHiringTableDateBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableDateBName, 
		[Description] = @retHiringTableDateBDescription, 
		[Text] = @retHiringTableDateBText,
		[Headline] = @retHiringTableDateBHeadline,
		SortOrder = @retHiringTableDateBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableDateBGuid
END
DECLARE @retHiringTableDateBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableDateBGuid)

-- B. Condition (has contract end date)
DECLARE @retHiringTableDateBCondAGuid UNIQUEIDENTIFIER = 'DADC6E18-B2F2-4450-A61E-E2E29210CE32',
	@retHiringTableDateBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retHiringTableDateBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retHiringTableDateBCondAValues NVARCHAR(MAX) = '',
	@retHiringTableDateBCondADescription NVARCHAR(MAX) = 'Has contract end date',
	@retHiringTableDateBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableDateBCondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableDateBCondAGuid,
		@retHiringTableDateBID,
		@retHiringTableDateBCondAPropertyName,
		@retHiringTableDateBCondAOperator,
		@retHiringTableDateBCondAValues,
		@retHiringTableDateBCondADescription,
		@retHiringTableDateBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableDateBID,
		Property_Name = @retHiringTableDateBCondAPropertyName,
		Operator = @retHiringTableDateBCondAOperator,
		[Values] = @retHiringTableDateBCondAValues,
		[Description] = @retHiringTableDateBCondADescription,
		[Status] = @retHiringTableDateBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableDateBCondAGuid
END


-- #################################### Employment type

-- A. Employment, full time
DECLARE @retHiringTableEmployAGuid UNIQUEIDENTIFIER = '488A0CCB-F937-428D-9271-0F9947E34A10',
	@retHiringTableEmployAName NVARCHAR(MAX) = @prefix + ' Employment, full time',
	@retHiringTableEmployADescription NVARCHAR(MAX) = '',
	@retHiringTableEmployAText NVARCHAR(MAX) = 'Full Time',
	@retHiringTableEmployAHeadline NVARCHAR(MAX) = 'Employment type:',
	@retHiringTableEmployASortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableEmployAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableEmployAName, 
		@retHiringTableEmployADescription,
		@retHiringTableEmployAText, 
		@retHiringTableEmployAHeadline,
		@retHiringTableEmployASortOrder,
		@retHiringTableEmployAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableEmployAName, 
		[Description] = @retHiringTableEmployADescription, 
		[Text] = @retHiringTableEmployAText,
		[Headline] = @retHiringTableEmployAHeadline,
		SortOrder = @retHiringTableEmployASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableEmployAGuid
END
DECLARE @retHiringTableEmployAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableEmployAGuid)

-- A. Employment, full time condition
DECLARE @retHiringTableEmployACondAGuid UNIQUEIDENTIFIER = 'EC6ED1A1-0DB5-40EE-A71D-B72CE9E9405E',
	@retHiringTableEmployACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retHiringTableEmployACondAOperator NVARCHAR(MAX) = 'Equal',
	@retHiringTableEmployACondAValues NVARCHAR(MAX) = '76',
	@retHiringTableEmployACondADescription NVARCHAR(MAX) = 'Has full time',
	@retHiringTableEmployACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableEmployACondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableEmployACondAGuid,
		@retHiringTableEmployAID,
		@retHiringTableEmployACondAPropertyName,
		@retHiringTableEmployACondAOperator,
		@retHiringTableEmployACondAValues,
		@retHiringTableEmployACondADescription,
		@retHiringTableEmployACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableEmployAID,
		Property_Name = @retHiringTableEmployACondAPropertyName,
		Operator = @retHiringTableEmployACondAOperator,
		[Values] = @retHiringTableEmployACondAValues,
		[Description] = @retHiringTableEmployACondADescription,
		[Status] = @retHiringTableEmployACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableEmployACondAGuid
END

-- B. Employment, part time
DECLARE @retHiringTableEmployBGuid UNIQUEIDENTIFIER = 'D4FB935F-8AD2-4F73-BD10-A616DAD2B46F',
	@retHiringTableEmployBName NVARCHAR(MAX) = @prefix + ' Employment, part time',
	@retHiringTableEmployBDescription NVARCHAR(MAX) = '',
	@retHiringTableEmployBText NVARCHAR(MAX) = 'Part Time',
	@retHiringTableEmployBHeadline NVARCHAR(MAX) = 'Employment type:',
	@retHiringTableEmployBSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableEmployBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableEmployBName, 
		@retHiringTableEmployBDescription,
		@retHiringTableEmployBText, 
		@retHiringTableEmployBHeadline,
		@retHiringTableEmployBSortOrder,
		@retHiringTableEmployBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableEmployBName, 
		[Description] = @retHiringTableEmployBDescription, 
		[Text] = @retHiringTableEmployBText,
		[Headline] = @retHiringTableEmployBHeadline,
		SortOrder = @retHiringTableEmployBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableEmployBGuid
END
DECLARE @retHiringTableEmployBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableEmployBGuid)

-- B. Employment, part time, condition A, less than 76
DECLARE @retHiringTableEmployBCondAGuid UNIQUEIDENTIFIER = 'A9786F2D-441E-4EB9-909C-00C619838ECC',
	@retHiringTableEmployBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retHiringTableEmployBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@retHiringTableEmployBCondAValues NVARCHAR(MAX) = '76',
	@retHiringTableEmployBCondADescription NVARCHAR(MAX) = 'Has less than 76 hours',
	@retHiringTableEmployBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableEmployBCondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableEmployBCondAGuid,
		@retHiringTableEmployBID,
		@retHiringTableEmployBCondAPropertyName,
		@retHiringTableEmployBCondAOperator,
		@retHiringTableEmployBCondAValues,
		@retHiringTableEmployBCondADescription,
		@retHiringTableEmployBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableEmployBID,
		Property_Name = @retHiringTableEmployBCondAPropertyName,
		Operator = @retHiringTableEmployBCondAOperator,
		[Values] = @retHiringTableEmployBCondAValues,
		[Description] = @retHiringTableEmployBCondADescription,
		[Status] = @retHiringTableEmployBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableEmployBCondAGuid
END

-- B. Employment, part time, condition B, more than 0
DECLARE @retHiringTableEmployBCondBGuid UNIQUEIDENTIFIER = '08E59549-8574-4DDD-AE75-724486A49CE3',
	@retHiringTableEmployBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retHiringTableEmployBCondBOperator NVARCHAR(MAX) = 'LargerThan',
	@retHiringTableEmployBCondBValues NVARCHAR(MAX) = '0',
	@retHiringTableEmployBCondBDescription NVARCHAR(MAX) = 'Has more than 0 hours',
	@retHiringTableEmployBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableEmployBCondBGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableEmployBCondBGuid,
		@retHiringTableEmployBID,
		@retHiringTableEmployBCondBPropertyName,
		@retHiringTableEmployBCondBOperator,
		@retHiringTableEmployBCondBValues,
		@retHiringTableEmployBCondBDescription,
		@retHiringTableEmployBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableEmployBID,
		Property_Name = @retHiringTableEmployBCondBPropertyName,
		Operator = @retHiringTableEmployBCondBOperator,
		[Values] = @retHiringTableEmployBCondBValues,
		[Description] = @retHiringTableEmployBCondBDescription,
		[Status] = @retHiringTableEmployBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableEmployBCondBGuid
END


-- C. Employment, casual
DECLARE @retHiringTableEmployCGuid UNIQUEIDENTIFIER = '00B85E9F-FAC3-4FF6-AD75-01326E096231',
	@retHiringTableEmployCName NVARCHAR(MAX) = @prefix + ' Employment, casual',
	@retHiringTableEmployCDescription NVARCHAR(MAX) = '',
	@retHiringTableEmployCText NVARCHAR(MAX) = 'Casual',
	@retHiringTableEmployCHeadline NVARCHAR(MAX) = 'Employment type:',
	@retHiringTableEmployCSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableEmployCGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableEmployCName, 
		@retHiringTableEmployCDescription,
		@retHiringTableEmployCText, 
		@retHiringTableEmployCHeadline,
		@retHiringTableEmployCSortOrder,
		@retHiringTableEmployCGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableEmployCName, 
		[Description] = @retHiringTableEmployCDescription, 
		[Text] = @retHiringTableEmployCText,
		[Headline] = @retHiringTableEmployCHeadline,
		SortOrder = @retHiringTableEmployCSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableEmployCGuid
END
DECLARE @retHiringTableEmployCID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableEmployCGuid)

-- C. Employment, casual condition
DECLARE @retHiringTableEmployCCondAGuid UNIQUEIDENTIFIER = 'FDADC78A-E395-442B-9BAD-355AE56AEBC3',
	@retHiringTableEmployCCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retHiringTableEmployCCondAOperator NVARCHAR(MAX) = 'Equal',
	@retHiringTableEmployCCondAValues NVARCHAR(MAX) = '0',
	@retHiringTableEmployCCondADescription NVARCHAR(MAX) = 'Has no contracted hours',
	@retHiringTableEmployCCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableEmployCCondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableEmployCCondAGuid,
		@retHiringTableEmployCID,
		@retHiringTableEmployCCondAPropertyName,
		@retHiringTableEmployCCondAOperator,
		@retHiringTableEmployCCondAValues,
		@retHiringTableEmployCCondADescription,
		@retHiringTableEmployCCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableEmployCID,
		Property_Name = @retHiringTableEmployCCondAPropertyName,
		Operator = @retHiringTableEmployCCondAOperator,
		[Values] = @retHiringTableEmployCCondAValues,
		[Description] = @retHiringTableEmployCCondADescription,
		[Status] = @retHiringTableEmployCCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableEmployCCondAGuid
END

-- #################################### Contracted hours per fortnight

-- Has contracted hours
DECLARE @retHiringTableHoursAGuid UNIQUEIDENTIFIER = '8FF33EAD-BEF3-469E-ABC1-37B5A5B9D681',
	@retHiringTableHoursAName NVARCHAR(MAX) = @prefix + ' Contracted hours, has',
	@retHiringTableHoursADescription NVARCHAR(MAX) = '',
	@retHiringTableHoursAText NVARCHAR(MAX) = '<Contracted Hours>',
	@retHiringTableHoursAHeadline NVARCHAR(MAX) = 'Contracted Hours per Fortnight:',
	@retHiringTableHoursASortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableHoursAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableHoursAName, 
		@retHiringTableHoursADescription,
		@retHiringTableHoursAText, 
		@retHiringTableHoursAHeadline,
		@retHiringTableHoursASortOrder,
		@retHiringTableHoursAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableHoursAName, 
		[Description] = @retHiringTableHoursADescription, 
		[Text] = @retHiringTableHoursAText,
		[Headline] = @retHiringTableHoursAHeadline,
		SortOrder = @retHiringTableHoursASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableHoursAGuid
END
DECLARE @retHiringTableHoursAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableHoursAGuid)

-- A. Has contracted hours
DECLARE @retHiringTableHoursACondAGuid UNIQUEIDENTIFIER = 'C7E9D22F-A65B-4E8C-9A70-0FB0E2F22F58',
	@retHiringTableHoursACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retHiringTableHoursACondAOperator NVARCHAR(MAX) = 'LargerThan',
	@retHiringTableHoursACondAValues NVARCHAR(MAX) = '0',
	@retHiringTableHoursACondADescription NVARCHAR(MAX) = 'Has contracted hours',
	@retHiringTableHoursACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableHoursACondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableHoursACondAGuid,
		@retHiringTableHoursAID,
		@retHiringTableHoursACondAPropertyName,
		@retHiringTableHoursACondAOperator,
		@retHiringTableHoursACondAValues,
		@retHiringTableHoursACondADescription,
		@retHiringTableHoursACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableHoursAID,
		Property_Name = @retHiringTableHoursACondAPropertyName,
		Operator = @retHiringTableHoursACondAOperator,
		[Values] = @retHiringTableHoursACondAValues,
		[Description] = @retHiringTableHoursACondADescription,
		[Status] = @retHiringTableHoursACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableHoursACondAGuid
END


-- B. Has no contracted hours
DECLARE @retHiringTableHoursBGuid UNIQUEIDENTIFIER = 'B64FE969-F1DB-46E1-A893-0CDA60212619',
	@retHiringTableHoursBName NVARCHAR(MAX) = @prefix + ' Contracted hours, has no',
	@retHiringTableHoursBDescription NVARCHAR(MAX) = '',
	@retHiringTableHoursBText NVARCHAR(MAX) = '',
	@retHiringTableHoursBHeadline NVARCHAR(MAX) = '',
	@retHiringTableHoursBSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringTableHoursBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringTableID, 
		@retHiringTableHoursBName, 
		@retHiringTableHoursBDescription,
		@retHiringTableHoursBText, 
		@retHiringTableHoursBHeadline,
		@retHiringTableHoursBSortOrder,
		@retHiringTableHoursBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringTableID,
		[Name] = @retHiringTableHoursBName, 
		[Description] = @retHiringTableHoursBDescription, 
		[Text] = @retHiringTableHoursBText,
		[Headline] = @retHiringTableHoursBHeadline,
		SortOrder = @retHiringTableHoursBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringTableHoursBGuid
END
DECLARE @retHiringTableHoursBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retHiringTableHoursBGuid)

-- Has contracted hours
DECLARE @retHiringTableHoursBCondAGuid UNIQUEIDENTIFIER = 'A8F95C94-3900-4B92-B846-506B5E105DFE',
	@retHiringTableHoursBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retHiringTableHoursBCondAOperator NVARCHAR(MAX) = 'Equal',
	@retHiringTableHoursBCondAValues NVARCHAR(MAX) = '0',
	@retHiringTableHoursBCondADescription NVARCHAR(MAX) = 'Has no contracted hours',
	@retHiringTableHoursBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retHiringTableHoursBCondAGuid)
BEGIN
	INSERT INTO tblCaseDocumentTextCondition(
		CaseDocumentTextConditionGUID, 
		CaseDocumentText_Id, 
		Property_Name,
		Operator,
		[Values],
		[Description],
		[Status],
		CreatedDate,
		CreatedByUser_Id,
		ChangedDate,
		ChangedByUser_Id)
	VALUES(
		@retHiringTableHoursBCondAGuid,
		@retHiringTableHoursBID,
		@retHiringTableHoursBCondAPropertyName,
		@retHiringTableHoursBCondAOperator,
		@retHiringTableHoursBCondAValues,
		@retHiringTableHoursBCondADescription,
		@retHiringTableHoursBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retHiringTableHoursBID,
		Property_Name = @retHiringTableHoursBCondAPropertyName,
		Operator = @retHiringTableHoursBCondAOperator,
		[Values] = @retHiringTableHoursBCondAValues,
		[Description] = @retHiringTableHoursBCondADescription,
		[Status] = @retHiringTableHoursBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retHiringTableHoursBCondAGuid
END




-- Add table paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @retHiringTableID, @counter
SET @counter = @counter + 1

-- #################################### Signature

---- Create or update paragraph
-- Paragraph guid
DECLARE @retHiringSignatureGuid UNIQUEIDENTIFIER = 'A933C122-8E38-43CD-B857-1249C9AD463C',
	@retHiringSignatureName NVARCHAR(MAX) = @prefix + ' Signature',
	@retHiringSignatureParagraphType INT = @ParagraphTypeText,
	@retHiringSignatureDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retHiringSignatureGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retHiringSignatureName, @retHiringSignatureDescription, @retHiringSignatureParagraphType, @retHiringSignatureGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retHiringSignatureName, [Description] = @retHiringSignatureDescription, ParagraphType = @retHiringSignatureParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retHiringSignatureGuid
END
DECLARE @retHiringSignatureID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retHiringSignatureGuid)

---- Create or update text A 
DECLARE @retHiringSignatureTextAGuid UNIQUEIDENTIFIER = '696409BA-C2D0-4BA2-809D-9ED7E9D0141C',
	@retHiringSignatureTextAName NVARCHAR(MAX) = @prefix + ' Signature A',
	@retHiringSignatureTextADescription NVARCHAR(MAX) = '',
	@retHiringSignatureTextAText NVARCHAR(MAX) = 'Your terms and conditions of employment will be as per the IKEA Enterprise Agreement 2017, the IKEA Group Code of Conduct and IKEA policies and procedures, as amended from time to time.  You can access this information via ‘ico-worker.com’ <i>(the IKEA co-worker website)</i> or ‘IKEA Inside’ <i>(the IKEA intranet)</i>.',
	@retHiringSignatureTextAHeadline NVARCHAR(MAX) = '',
	@retHiringSignatureTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringSignatureTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringSignatureID, 
		@retHiringSignatureTextAName, 
		@retHiringSignatureTextADescription,
		@retHiringSignatureTextAText, 
		@retHiringSignatureTextAHeadline,
		@retHiringSignatureTextASortOrder,
		@retHiringSignatureTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringSignatureID,
		[Name] = @retHiringSignatureTextAName, 
		[Description] = @retHiringSignatureTextADescription, 
		[Text] = @retHiringSignatureTextAText,
		[Headline] = @retHiringSignatureTextAHeadline,
		SortOrder = @retHiringSignatureTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringSignatureTextAGuid
END

---- Create or update text B
DECLARE @retHiringSignatureTextBGuid UNIQUEIDENTIFIER = '2E936F7F-62C2-4A60-BDDB-66852F6FD273',
	@retHiringSignatureTextBName NVARCHAR(MAX) = @prefix + ' Signature B',
	@retHiringSignatureTextBDescription NVARCHAR(MAX) = '',
	@retHiringSignatureTextBText NVARCHAR(MAX) = '<p><Reports To Line Manager><br />		<Position Title (Local Job Name) of Reports To Line Manager><br />		IKEA <Business Unit></p>		<hr style="height:2px;border:none;color:#000;background-color:#000;" />		',
	@retHiringSignatureTextBHeadline NVARCHAR(MAX) = '',
	@retHiringSignatureTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringSignatureTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringSignatureID, 
		@retHiringSignatureTextBName, 
		@retHiringSignatureTextBDescription,
		@retHiringSignatureTextBText, 
		@retHiringSignatureTextBHeadline,
		@retHiringSignatureTextBSortOrder,
		@retHiringSignatureTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringSignatureID,
		[Name] = @retHiringSignatureTextBName, 
		[Description] = @retHiringSignatureTextBDescription, 
		[Text] = @retHiringSignatureTextBText,
		[Headline] = @retHiringSignatureTextBHeadline,
		SortOrder = @retHiringSignatureTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringSignatureTextBGuid
END


---- Create or update text C
DECLARE @retHiringSignatureTextCGuid UNIQUEIDENTIFIER = '6D8BA196-5157-4765-8933-05F23FA6775E',
	@retHiringSignatureTextCName NVARCHAR(MAX) = @prefix + ' Signature C',
	@retHiringSignatureTextCDescription NVARCHAR(MAX) = '',
	@retHiringSignatureTextCText NVARCHAR(MAX) = '<p style="text-align:center;"><strong>Acknowledgement</strong></p>		<p>I accept the terms and conditions of employment as detailed above.</p>		<p>Signed:  _____________________________        Date:  _______________</p>		<p>Co-worker Name: <Co-worker First Name> <Co-worker Last Name></p>		',
	@retHiringSignatureTextCHeadline NVARCHAR(MAX) = '',
	@retHiringSignatureTextCSortOrder INT = 2

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retHiringSignatureTextCGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retHiringSignatureID, 
		@retHiringSignatureTextCName, 
		@retHiringSignatureTextCDescription,
		@retHiringSignatureTextCText, 
		@retHiringSignatureTextCHeadline,
		@retHiringSignatureTextCSortOrder,
		@retHiringSignatureTextCGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retHiringSignatureID,
		[Name] = @retHiringSignatureTextCName, 
		[Description] = @retHiringSignatureTextCDescription, 
		[Text] = @retHiringSignatureTextCText,
		[Headline] = @retHiringSignatureTextCHeadline,
		SortOrder = @retHiringSignatureTextCSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retHiringSignatureTextCGuid
END


-- Add signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retHiringID, @retHiringSignatureID, @counter
SET @counter = @counter + 1



-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @retHiringGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder

ROLLBACK


