--########################################
--########## DC Retail Salary Hiring #####
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
DECLARE @draftID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @addressInfoGuid)

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

DECLARE @prefix NVARCHAR(MAX) = 'RET SAL HIR'

-- #################################### Contract Clusters – DC EA (Hiring) ####################################

-- Get the form
DECLARE @retSalHiringGuid UNIQUEIDENTIFIER = '8595E209-075C-4384-ADDB-8BF2E52A2CE5'
DECLARE @retSalHiringID INT, @counter INT = 0
SELECT @retSalHiringID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retSalHiringGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @retSalHiringGuid

-- #################################### 1. Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @footerID, @counter
SET @counter = @counter + 1



-- #################################### 2-8. Address and company info
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @addressInfoID, @counter
SET @counter = @counter + 1

-- #################################### 10a-b. Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalHiringEmployGreetingGuid UNIQUEIDENTIFIER = '79CA236D-154C-403A-A4D8-EB67DB0CCCFF',
	@cdpName NVARCHAR(MAX) = @prefix + ' Employment greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalHiringEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @retSalHiringEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalHiringEmployGreetingGuid
END
DECLARE @retSalHiringEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalHiringEmployGreetingGuid)

---- Create or update text A, Full Time
DECLARE @retSalHiringEmployGreetingTextAGuid UNIQUEIDENTIFIER = '2DD1625E-9B06-457C-8123-B27B825CB384',
	@retSalHiringEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Greeting, Full time',
	@retSalHiringEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@retSalHiringEmployGreetingTextAText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Full Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@retSalHiringEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@retSalHiringEmployGreetingTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringEmployGreetingID, 
		@retSalHiringEmployGreetingTextAName, 
		@retSalHiringEmployGreetingTextADescription,
		@retSalHiringEmployGreetingTextAText, 
		@retSalHiringEmployGreetingTextAHeadline,
		@retSalHiringEmployGreetingTextASortOrder,
		@retSalHiringEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringEmployGreetingID,
		[Name] = @retSalHiringEmployGreetingTextAName, 
		[Description] = @retSalHiringEmployGreetingTextADescription, 
		[Text] = @retSalHiringEmployGreetingTextAText,
		[Headline] = @retSalHiringEmployGreetingTextAHeadline,
		SortOrder = @retSalHiringEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringEmployGreetingTextAGuid
END
DECLARE @retSalHiringEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @retSalHiringEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = '2DD1625E-9B06-457C-8123-B27B825CB384',
	@retSalHiringEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'Equal',
	@retSalHiringEmployGreetingTextACondAValues NVARCHAR(MAX) = '76',
	@retSalHiringEmployGreetingTextACondADescription NVARCHAR(MAX) = 'Is full time',
	@retSalHiringEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringEmployGreetingTextACondAGuid)
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
		@retSalHiringEmployGreetingTextACondAGuid,
		@retSalHiringEmployGreetingTextAID,
		@retSalHiringEmployGreetingTextACondAPropertyName,
		@retSalHiringEmployGreetingTextACondAOperator,
		@retSalHiringEmployGreetingTextACondAValues,
		@retSalHiringEmployGreetingTextACondADescription,
		@retSalHiringEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringEmployGreetingTextAID,
		Property_Name = @retSalHiringEmployGreetingTextACondAPropertyName,
		Operator = @retSalHiringEmployGreetingTextACondAOperator,
		[Values] = @retSalHiringEmployGreetingTextACondAValues,
		[Description] = @retSalHiringEmployGreetingTextACondADescription,
		[Status] = @retSalHiringEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @retSalHiringEmployGreetingTextBGuid UNIQUEIDENTIFIER = '9572FFD6-DBBF-4913-AF5F-93D0B1F0F0FB',
	@retSalHiringEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Greeting, Part time',
	@retSalHiringEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@retSalHiringEmployGreetingTextBText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Part Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@retSalHiringEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@retSalHiringEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringEmployGreetingID, 
		@retSalHiringEmployGreetingTextBName, 
		@retSalHiringEmployGreetingTextBDescription,
		@retSalHiringEmployGreetingTextBText, 
		@retSalHiringEmployGreetingTextBHeadline,
		@retSalHiringEmployGreetingTextBSortOrder,
		@retSalHiringEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringEmployGreetingID,
		[Name] = @retSalHiringEmployGreetingTextBName, 
		[Description] = @retSalHiringEmployGreetingTextBDescription, 
		[Text] = @retSalHiringEmployGreetingTextBText,
		[Headline] = @retSalHiringEmployGreetingTextBHeadline,
		SortOrder = @retSalHiringEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringEmployGreetingTextBGuid
END
DECLARE @retSalHiringEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @retSalHiringEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = '0EF15E2F-7644-4480-9744-76C5B2D59030',
	@retSalHiringEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@retSalHiringEmployGreetingTextBCondAValues NVARCHAR(MAX) = '76',
	@retSalHiringEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Is part time',
	@retSalHiringEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringEmployGreetingTextBCondAGuid)
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
		@retSalHiringEmployGreetingTextBCondAGuid,
		@retSalHiringEmployGreetingTextBID,
		@retSalHiringEmployGreetingTextBCondAPropertyName,
		@retSalHiringEmployGreetingTextBCondAOperator,
		@retSalHiringEmployGreetingTextBCondAValues,
		@retSalHiringEmployGreetingTextBCondADescription,
		@retSalHiringEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringEmployGreetingTextBID,
		Property_Name = @retSalHiringEmployGreetingTextBCondAPropertyName,
		Operator = @retSalHiringEmployGreetingTextBCondAOperator,
		[Values] = @retSalHiringEmployGreetingTextBCondAValues,
		[Description] = @retSalHiringEmployGreetingTextBCondADescription,
		[Status] = @retSalHiringEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringEmployGreetingTextBCondAGuid
END


-- Add paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @retSalHiringEmployGreetingID, @counter
SET @counter = @counter + 1

-- #################################### 11a-33 Terms

DECLARE @termsCounter INT = 0

---- Create or update a terms paragraph
-- Paragraph guid
DECLARE @retSalHiringTermsGuid UNIQUEIDENTIFIER = 'C611D484-1149-4DAC-8F3F-0CDC51E8E509',
	@retSalHiringTermsName NVARCHAR(MAX) = @prefix + ' Terms',
	@retSalHiringTermsParagraphType INT = @ParagraphTypeTableNumeric,
	@termsDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalHiringTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalHiringTermsName, @termsDescription, @retSalHiringTermsParagraphType, @retSalHiringTermsGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalHiringTermsName, [Description] = @termsDescription, ParagraphType = @retSalHiringTermsParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalHiringTermsGuid
END
DECLARE @retSalHiringTermsID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalHiringTermsGuid)

-- #################################### 11a-b Position

---- Position A
DECLARE @retSalHiringTermsPositionAGuid UNIQUEIDENTIFIER = '864CCBC4-FF1F-4CF7-A6F8-097DDFE88ADF',
	@retSalHiringTermsPositionAName NVARCHAR(MAX) = @prefix + ' Employment - Position - Full time',
	@retSalHiringTermsPositionADescription NVARCHAR(MAX) = '',
	@retSalHiringTermsPositionAText NVARCHAR(MAX) = 'Your position is Full Time <Position Title (Local Job Name)> <Shift Type> Shift, reporting to  <Position Title (Local Job Name)> of <Reports To Line Manager>, which will be based at <Business Unit>. Your position (in terms of your duties & responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@retSalHiringTermsPositionAHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@retSalHiringTermsPositionASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsPositionAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsPositionAName, 
		@retSalHiringTermsPositionADescription,
		@retSalHiringTermsPositionAText, 
		@retSalHiringTermsPositionAHeadline,
		@retSalHiringTermsPositionASortOrder,
		@retSalHiringTermsPositionAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsPositionAName, 
		[Description] = @retSalHiringTermsPositionADescription, 
		[Text] = @retSalHiringTermsPositionAText,
		[Headline] = @retSalHiringTermsPositionAHeadline,
		SortOrder = @retSalHiringTermsPositionASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPositionAGuid
END
DECLARE @retSalHiringTermsPositionAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPositionAGuid)
SET @termsCounter = @termsCounter + 1

-- Create condition for position A
DECLARE @retSalHiringTermsPositionACondGuid UNIQUEIDENTIFIER = 'B9BED814-123E-4A0B-9D11-3D16FF1CAAD9',
	@retSalHiringTermsPositionACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringTermsPositionACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalHiringTermsPositionACondValues NVARCHAR(MAX) = '76',
	@retSalHiringTermsPositionACondDescription NVARCHAR(MAX) = 'Is full time',
	@retSalHiringTermsPositionACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsPositionACondGuid)
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
		@retSalHiringTermsPositionACondGuid,
		@retSalHiringTermsPositionAID,
		@retSalHiringTermsPositionACondPropertyName,
		@retSalHiringTermsPositionACondOperator,
		@retSalHiringTermsPositionACondValues,
		@retSalHiringTermsPositionACondDescription,
		@retSalHiringTermsPositionACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsPositionAID,
		Property_Name = @retSalHiringTermsPositionACondPropertyName,
		Operator = @retSalHiringTermsPositionACondOperator,
		[Values] = @retSalHiringTermsPositionACondValues,
		[Description] = @retSalHiringTermsPositionACondDescription,
		[Status] = @retSalHiringTermsPositionACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsPositionACondGuid
END

---- Position B
DECLARE @retSalHiringTermsPositionBGuid UNIQUEIDENTIFIER = '927711F2-75A7-4DE4-B46E-E88EEEC08354',
	@retSalHiringTermsPositionBName NVARCHAR(MAX) = @prefix + ' Employment - Position - Part time',
	@retSalHiringTermsPositionBDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsPositionBText NVARCHAR(MAX) = 'Your position is Part Time <Position Title (Local Job Name)> <Shift Type> Shift, reporting to  <Position Title (Local Job Name)> of <Reports To Line Manager>, which will be based at <Business Unit>. Your position (in terms of your duties & responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@retSalHiringTermsPositionBHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@retSalHiringTermsPositionBSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsPositionBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsPositionBName, 
		@retSalHiringTermsPositionBDescription,
		@retSalHiringTermsPositionBText, 
		@retSalHiringTermsPositionBHeadline,
		@retSalHiringTermsPositionBSortOrder,
		@retSalHiringTermsPositionBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsPositionBName, 
		[Description] = @retSalHiringTermsPositionBDescription, 
		[Text] = @retSalHiringTermsPositionBText,
		[Headline] = @retSalHiringTermsPositionBHeadline,
		SortOrder = @retSalHiringTermsPositionBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPositionBGuid
END
DECLARE @retSalHiringTermsPositionBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPositionBGuid)

-- Create condition for position A
DECLARE @retSalHiringTermsPositionBCondGuid UNIQUEIDENTIFIER = '94D7036D-0690-48CE-A353-4E0E6E8BD35E',
	@retSalHiringTermsPositionBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringTermsPositionBCondOperator NVARCHAR(MAX) = 'LessThan',
	@retSalHiringTermsPositionBCondValues NVARCHAR(MAX) = '76',
	@retSalHiringTermsPositionBCondDescription NVARCHAR(MAX) = 'Is part time',
	@retSalHiringTermsPositionBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsPositionBCondGuid)
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
		@retSalHiringTermsPositionBCondGuid,
		@retSalHiringTermsPositionBID,
		@retSalHiringTermsPositionBCondPropertyName,
		@retSalHiringTermsPositionBCondOperator,
		@retSalHiringTermsPositionBCondValues,
		@retSalHiringTermsPositionBCondDescription,
		@retSalHiringTermsPositionBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsPositionBID,
		Property_Name = @retSalHiringTermsPositionBCondPropertyName,
		Operator = @retSalHiringTermsPositionBCondOperator,
		[Values] = @retSalHiringTermsPositionBCondValues,
		[Description] = @retSalHiringTermsPositionBCondDescription,
		[Status] = @retSalHiringTermsPositionBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsPositionBCondGuid
END

-- #################################### 12a-b Commencement Date

---- Commencement A
DECLARE @retSalHiringTermsComAGuid UNIQUEIDENTIFIER = '5D926BEA-7199-4EB5-B5C1-10C8D1974D01',
	@retSalHiringTermsComAName NVARCHAR(MAX) = @prefix + ' Employment - Commencement - No date',
	@retSalHiringTermsComADescription NVARCHAR(MAX) = '',
	@retSalHiringTermsComAText NVARCHAR(MAX) = 'Your commencement date is <Contract Start Date>, unless otherwise terminated in accordance with this contract.',
	@retSalHiringTermsComAHeadline NVARCHAR(MAX) = 'Commencement Date',
	@retSalHiringTermsComASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsComAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsComAName, 
		@retSalHiringTermsComADescription,
		@retSalHiringTermsComAText, 
		@retSalHiringTermsComAHeadline,
		@retSalHiringTermsComASortOrder,
		@retSalHiringTermsComAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsComAName, 
		[Description] = @retSalHiringTermsComADescription, 
		[Text] = @retSalHiringTermsComAText,
		[Headline] = @retSalHiringTermsComAHeadline,
		SortOrder = @retSalHiringTermsComASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsComAGuid
END
DECLARE @retSalHiringTermsComAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsComAGuid)

-- Create condition for Commencement A
DECLARE @retSalHiringTermsComACondAGuid UNIQUEIDENTIFIER = '71E7994F-753B-46E7-B164-6A4EB1AAF6A9',
	@retSalHiringTermsComACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retSalHiringTermsComACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retSalHiringTermsComACondAValues NVARCHAR(MAX) = '',
	@retSalHiringTermsComACondADescription NVARCHAR(MAX) = 'Has no end date',
	@retSalHiringTermsComACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsComACondAGuid)
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
		@retSalHiringTermsComACondAGuid,
		@retSalHiringTermsComAID,
		@retSalHiringTermsComACondAPropertyName,
		@retSalHiringTermsComACondAOperator,
		@retSalHiringTermsComACondAValues,
		@retSalHiringTermsComACondADescription,
		@retSalHiringTermsComACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsComAID,
		Property_Name = @retSalHiringTermsComACondAPropertyName,
		Operator = @retSalHiringTermsComACondAOperator,
		[Values] = @retSalHiringTermsComACondAValues,
		[Description] = @retSalHiringTermsComACondADescription,
		[Status] = @retSalHiringTermsComACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsComACondAGuid
END

-- No support for 31.12.9999 yet
--DECLARE @retSalHiringTermsComACondBGuid UNIQUEIDENTIFIER = 'e68e3e7c-52b0-4018-964b-99a1d9d471b9',
--	@retSalHiringTermsComACondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
--	@retSalHiringTermsComACondBOperator NVARCHAR(MAX) = 'Equal',
--	@retSalHiringTermsComACondBValues NVARCHAR(MAX) = '31.12.9999',
--	@retSalHiringTermsComACondBDescription NVARCHAR(MAX) = 'Equal 31.12.9999',
--	@retSalHiringTermsComACondBStatus INT = 1

--IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsComACondBGuid)
--BEGIN
--	INSERT INTO tblCaseDocumentTextCondition(
--		CaseDocumentTextConditionGUID, 
--		CaseDocumentText_Id, 
--		Property_Name,
--		Operator,
--		[Values],
--		[Description],
--		[Status],
--		CreatedDate,
--		CreatedByUser_Id,
--		ChangedDate,
--		ChangedByUser_Id)
--	VALUES(
--		@retSalHiringTermsComACondBGuid,
--		@retSalHiringTermsComAID,
--		@retSalHiringTermsComACondBPropertyName,
--		@retSalHiringTermsComACondBOperator,
--		@retSalHiringTermsComACondBValues,
--		@retSalHiringTermsComACondBDescription,
--		@retSalHiringTermsComACondBStatus,
--		@now, 
--		@userID,
--		@now,
--		@userID
--	)
--END
--ELSE
--BEGIN
--	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsComAID,
--		Property_Name = @retSalHiringTermsComACondBPropertyName,
--		Operator = @retSalHiringTermsComACondBOperator,
--		[Values] = @retSalHiringTermsComACondBValues,
--		[Description] = @retSalHiringTermsComACondBDescription,
--		[Status] = @retSalHiringTermsComACondBStatus,
--		CreatedDate = @now,
--		CreatedByUser_Id = @userID,
--		ChangedDate = @now,
--		ChangedByUser_Id = @userID
--	FROM tblCaseDocumentTextCondition CDTC
--	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsComACondBGuid
--END

---- Position B
DECLARE @retSalHiringTermsComBGuid UNIQUEIDENTIFIER = 'A760E025-2266-426B-AB7F-0D13D8594D3B',
	@retSalHiringTermsComBName NVARCHAR(MAX) = @prefix + ' - Commencement - Has end date',
	@retSalHiringTermsComBDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsComBText NVARCHAR(MAX) = 'Your commencement date is <Contract Start Date> and will cease on <Contract End Date>, unless otherwise terminated in accordance with this contract.',
	@retSalHiringTermsComBHeadline NVARCHAR(MAX) = 'Commencement date',
	@retSalHiringTermsComBSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsComBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsComBName, 
		@retSalHiringTermsComBDescription,
		@retSalHiringTermsComBText, 
		@retSalHiringTermsComBHeadline,
		@retSalHiringTermsComBSortOrder,
		@retSalHiringTermsComBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsComBName, 
		[Description] = @retSalHiringTermsComBDescription, 
		[Text] = @retSalHiringTermsComBText,
		[Headline] = @retSalHiringTermsComBHeadline,
		SortOrder = @retSalHiringTermsComBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsComBGuid
END
DECLARE @retSalHiringTermsComBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsComBGuid)

-- Create condition for Commence B
DECLARE @retSalHiringTermsComBCondAGuid UNIQUEIDENTIFIER = '5BEDE30F-26A9-4C9A-845B-E823ABA24B0E',
	@retSalHiringTermsComBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retSalHiringTermsComBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalHiringTermsComBCondAValues NVARCHAR(MAX) = '',
	@retSalHiringTermsComBCondADescription NVARCHAR(MAX) = 'Has end date',
	@retSalHiringTermsComBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsComBCondAGuid)
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
		@retSalHiringTermsComBCondAGuid,
		@retSalHiringTermsComBID,
		@retSalHiringTermsComBCondAPropertyName,
		@retSalHiringTermsComBCondAOperator,
		@retSalHiringTermsComBCondAValues,
		@retSalHiringTermsComBCondADescription,
		@retSalHiringTermsComBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsComBID,
		Property_Name = @retSalHiringTermsComBCondAPropertyName,
		Operator = @retSalHiringTermsComBCondAOperator,
		[Values] = @retSalHiringTermsComBCondAValues,
		[Description] = @retSalHiringTermsComBCondADescription,
		[Status] = @retSalHiringTermsComBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsComBCondAGuid
END

-- No support for 31.12.9999 yet
/*DECLARE @retSalHiringTermsComBCondBGuid UNIQUEIDENTIFIER = '43c72a21-96c1-4d3c-a44a-5279593332c7',
	@retSalHiringTermsComBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@retSalHiringTermsComBCondBOperator NVARCHAR(MAX) = 'NotEqual',
	@retSalHiringTermsComBCondBValues NVARCHAR(MAX) = '31.12.9999',
	@retSalHiringTermsComBCondBDescription NVARCHAR(MAX) = 'Not equals 31.12.9999',
	@retSalHiringTermsComBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsComBCondBGuid)
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
		@retSalHiringTermsComBCondBGuid,
		@retSalHiringTermsComBID,
		@retSalHiringTermsComBCondBPropertyName,
		@retSalHiringTermsComBCondBOperator,
		@retSalHiringTermsComBCondBValues,
		@retSalHiringTermsComBCondBDescription,
		@retSalHiringTermsComBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsComBID,
		Property_Name = @retSalHiringTermsComBCondBPropertyName,
		Operator = @retSalHiringTermsComBCondBOperator,
		[Values] = @retSalHiringTermsComBCondBValues,
		[Description] = @retSalHiringTermsComBCondBDescription,
		[Status] = @retSalHiringTermsComBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsComBCondBGuid
END*/
-- #################################### 13a-b Hours of Work

---- Hours of Work A
DECLARE @retSalHiringTermsHWAGuid UNIQUEIDENTIFIER = '19E0C4DA-0FC7-49FD-8E91-78F98AA20EA3',
	@retSalHiringTermsHWAName NVARCHAR(MAX) = @prefix + ' - Hours of Work - Full time',
	@retSalHiringTermsHWADescription NVARCHAR(MAX) = '',
	@retSalHiringTermsHWAText NVARCHAR(MAX) = 
	'You will be rostered to work 76 ordinary hours per fortnight.  Such details of your initial roster will be discussed with you upon your commencement.  However, where there is a change in the business’ needs, your hours may also be subject to change with appropriate notice.
You should note that ordinary hours in the Distribution Centre include Saturday’s and you have mutually agreed to work more than one in three Saturdays as part of your contracted ordinary hours.',
	@retSalHiringTermsHWAHeadline NVARCHAR(MAX) = 'Hours of Work',
	@retSalHiringTermsHWASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsHWAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsHWAName, 
		@retSalHiringTermsHWADescription,
		@retSalHiringTermsHWAText, 
		@retSalHiringTermsHWAHeadline,
		@retSalHiringTermsHWASortOrder,
		@retSalHiringTermsHWAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsHWAName, 
		[Description] = @retSalHiringTermsHWADescription, 
		[Text] = @retSalHiringTermsHWAText,
		[Headline] = @retSalHiringTermsHWAHeadline,
		SortOrder = @retSalHiringTermsHWASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsHWAGuid
END
DECLARE @retSalHiringTermsHWAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsHWAGuid)

-- Create condition for Hours of Work A
DECLARE @retSalHiringTermsHWACondGuid UNIQUEIDENTIFIER = '22CCD872-5C39-48F8-8D63-4A5EFB9165F3',
	@retSalHiringTermsHWACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringTermsHWACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalHiringTermsHWACondValues NVARCHAR(MAX) = '76',
	@retSalHiringTermsHWACondDescription NVARCHAR(MAX) = 'Is full time',
	@retSalHiringTermsHWACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsHWACondGuid)
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
		@retSalHiringTermsHWACondGuid,
		@retSalHiringTermsHWAID,
		@retSalHiringTermsHWACondPropertyName,
		@retSalHiringTermsHWACondOperator,
		@retSalHiringTermsHWACondValues,
		@retSalHiringTermsHWACondDescription,
		@retSalHiringTermsHWACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsHWAID,
		Property_Name = @retSalHiringTermsHWACondPropertyName,
		Operator = @retSalHiringTermsHWACondOperator,
		[Values] = @retSalHiringTermsHWACondValues,
		[Description] = @retSalHiringTermsHWACondDescription,
		[Status] = @retSalHiringTermsHWACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsHWACondGuid
END

---- Hours of Work B
DECLARE @retSalHiringTermsHWBGuid UNIQUEIDENTIFIER = '9A40A689-16CB-423E-8A96-E75882AC099C',
	@retSalHiringTermsHWBName NVARCHAR(MAX) = @prefix + ' - Hours of Work - Part time',
	@retSalHiringTermsHWBDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsHWBText NVARCHAR(MAX) = 
	'Your contracted hours are <Contracted Hours> per fortnight, you may be offered additional ‘varied hours’ paid at your ordinary rate of pay.
	You will be rostered in accordance to your availability schedule which you filled out at the time of your employment. Your availability schedule forms part of your employment contract.',
	@retSalHiringTermsHWBHeadline NVARCHAR(MAX) = 'Hours of Work',
	@retSalHiringTermsHWBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsHWBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsHWBName, 
		@retSalHiringTermsHWBDescription,
		@retSalHiringTermsHWBText, 
		@retSalHiringTermsHWBHeadline,
		@retSalHiringTermsHWBSortOrder,
		@retSalHiringTermsHWBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsHWBName, 
		[Description] = @retSalHiringTermsHWBDescription, 
		[Text] = @retSalHiringTermsHWBText,
		[Headline] = @retSalHiringTermsHWBHeadline,
		SortOrder = @retSalHiringTermsHWBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsHWBGuid
END
DECLARE @retSalHiringTermsHWBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsHWBGuid)

-- Create condition for hours of work B
DECLARE @retSalHiringTermsHWBCondGuid UNIQUEIDENTIFIER = '55423447-FA0B-4406-8207-14D54FC89F1D',
	@retSalHiringTermsHWBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringTermsHWBCondOperator NVARCHAR(MAX) = 'LessThan',
	@retSalHiringTermsHWBCondValues NVARCHAR(MAX) = '76',
	@retSalHiringTermsHWBCondDescription NVARCHAR(MAX) = 'Is part time',
	@retSalHiringTermsHWBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsHWBCondGuid)
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
		@retSalHiringTermsHWBCondGuid,
		@retSalHiringTermsHWBID,
		@retSalHiringTermsHWBCondPropertyName,
		@retSalHiringTermsHWBCondOperator,
		@retSalHiringTermsHWBCondValues,
		@retSalHiringTermsHWBCondDescription,
		@retSalHiringTermsHWBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsHWBID,
		Property_Name = @retSalHiringTermsHWBCondPropertyName,
		Operator = @retSalHiringTermsHWBCondOperator,
		[Values] = @retSalHiringTermsHWBCondValues,
		[Description] = @retSalHiringTermsHWBCondDescription,
		[Status] = @retSalHiringTermsHWBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsHWBCondGuid
END


-- #################################### 14 Probationary Period

---- Probationary Period
DECLARE @retSalHiringTermsProbTimeGuid UNIQUEIDENTIFIER = 'D33DCA76-8810-4163-B180-6285D9F055D2',
	@retSalHiringTermsProbTimeName NVARCHAR(MAX) = @prefix + ' - Probationary Period',
	@retSalHiringTermsProbTimeDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsProbTimeText NVARCHAR(MAX) = 'IKEA offers this employment to you on a probationary basis for a period of six (6) months, during which time your performance standards will be subject to regular review and assessment.  In the six (6)-month period, if either you or IKEA wishes to terminate the employment relationship, then either party can effect that termination with one week’s notice in writing.',
	@retSalHiringTermsProbTimeHeadline NVARCHAR(MAX) = 'Probationary Period',
	@retSalHiringTermsProbTimeSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsProbTimeGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsProbTimeName, 
		@retSalHiringTermsProbTimeDescription,
		@retSalHiringTermsProbTimeText, 
		@retSalHiringTermsProbTimeHeadline,
		@retSalHiringTermsProbTimeSortOrder,
		@retSalHiringTermsProbTimeGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsProbTimeName, 
		[Description] = @retSalHiringTermsProbTimeDescription, 
		[Text] = @retSalHiringTermsProbTimeText,
		[Headline] = @retSalHiringTermsProbTimeHeadline,
		SortOrder = @retSalHiringTermsProbTimeSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsProbTimeGuid
END

-- #################################### 15 Remuneration
DECLARE @retSalHiringTermsRemunGuid UNIQUEIDENTIFIER = '00149C22-1C23-48B1-A246-51274F3AB8BC',
	@retSalHiringTermsRemunName NVARCHAR(MAX) = @prefix + ' - Remuneration',
	@retSalHiringTermsRemunDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsRemunText NVARCHAR(MAX) = 'Upon commencement, your base hourly rate will be as per the <b>IKEA Distributions Services Australia Pty Ltd Enterprise Agreement 2016</b>.  This amount will be paid directly into your nominated bank account on a fortnightly basis.',
	@retSalHiringTermsRemunHeadline NVARCHAR(MAX) = 'Remuneration',
	@retSalHiringTermsRemunSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsRemunGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsRemunName, 
		@retSalHiringTermsRemunDescription,
		@retSalHiringTermsRemunText, 
		@retSalHiringTermsRemunHeadline,
		@retSalHiringTermsRemunSortOrder,
		@retSalHiringTermsRemunGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsRemunName, 
		[Description] = @retSalHiringTermsRemunDescription, 
		[Text] = @retSalHiringTermsRemunText,
		[Headline] = @retSalHiringTermsRemunHeadline,
		SortOrder = @retSalHiringTermsRemunSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsRemunGuid
END

-- #################################### 16 Superannuation
DECLARE @retSalHiringTermsSuperGuid UNIQUEIDENTIFIER = 'A47D9626-268F-4B1E-A584-6ACE4757CF1D',
	@retSalHiringTermsSuperName NVARCHAR(MAX) = @prefix + ' - Superannuation',
	@retSalHiringTermsSuperDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsSuperText NVARCHAR(MAX) = 'IKEA will make superannuation contributions, on your behalf, to a government approved Superannuation Fund, at the rate payable under the Superannuation Guarantee Legislation (SGL). This rate is currently 9.5% of your wage.
<br>
IKEA’s current employer superannuation fund is the Labour Union Co-operative Retirement Fund (LUCRF), which is the fund into which the superannuation contributions will be made unless an alternate fund is nominated by you in writing, in accordance with the SGL.
<br>
It is your responsibility to nominate a Super Fund for your contributions to be made to, and to ensure that you complete the necessary paperwork for enrolment into your nominated fund.  IKEA will supply you with a LUCRF Member Guide, including an application form.',
	@retSalHiringTermsSuperHeadline NVARCHAR(MAX) = 'Superannuation',
	@retSalHiringTermsSuperSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsSuperGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsSuperName, 
		@retSalHiringTermsSuperDescription,
		@retSalHiringTermsSuperText, 
		@retSalHiringTermsSuperHeadline,
		@retSalHiringTermsSuperSortOrder,
		@retSalHiringTermsSuperGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsSuperName, 
		[Description] = @retSalHiringTermsSuperDescription, 
		[Text] = @retSalHiringTermsSuperText,
		[Headline] = @retSalHiringTermsSuperHeadline,
		SortOrder = @retSalHiringTermsSuperSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsSuperGuid
END


-- #################################### 17 Confidential Information
DECLARE @retSalHiringTermsConfGuid UNIQUEIDENTIFIER = '6FDBEA72-1B45-46F0-B6C9-C1FC6D921AB4',
	@retSalHiringTermsConfName NVARCHAR(MAX) = @prefix + ' - Confidential Information',
	@retSalHiringTermsConfDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsConfText NVARCHAR(MAX) = 'In the course of your employment, you may be exposed to “Confidential Information” concerning IKEA. Confidential Information means any information obtained by you in the course of your employment, including:
<ul>
<li>trade secrets;</li>
<li>technical information and technical drawings;</li>
<li>commercial information about IKEA and persons with whom IKEA deals;</li>
<li>Product and market information;</li>
<li>this letter of appointment;</li>
<li>any information marked “confidential” or which IKEA informs you is confidential or a trade secret; and</li>
<li>Co-worker and customer personal details.</li>
</ul>
<br>
but excluding:
<ul>
<li>information available to the public; and</li>
<li>information which you can prove you lawfully possessed before obtaining it in the course of your employment (other than this letter of appointment)</li>
</ul>
<br>
During and after your employment, you must not use or disclose Confidential Information to any person (including an employee of IKEA) other than:
<ul>
<li>to perform your duties;</li>
<li>if IKEA has consented in writing; or</li>
<li>if required by law.</li>
</ul>
<br>
As an IKEA co-worker, you must keep Confidential Information in a secure manner and treat such information with appropriate sensitivity. On demand by IKEA and at the end of your employment, you must deliver to IKEA all copies of Confidential Information in your possession or control (including all Confidential Information held electronically in any medium) and then delete all Confidential Information held electronically in any medium in your possession or control.',
	@retSalHiringTermsConfHeadline NVARCHAR(MAX) = 'Confidential Information',
	@retSalHiringTermsConfSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsConfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsConfName, 
		@retSalHiringTermsConfDescription,
		@retSalHiringTermsConfText, 
		@retSalHiringTermsConfHeadline,
		@retSalHiringTermsConfSortOrder,
		@retSalHiringTermsConfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsConfName, 
		[Description] = @retSalHiringTermsConfDescription, 
		[Text] = @retSalHiringTermsConfText,
		[Headline] = @retSalHiringTermsConfHeadline,
		SortOrder = @retSalHiringTermsConfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsConfGuid
END

-- #################################### 18a-b Leave Entitlements 

---- Leave Entitlements  A
DECLARE @retSalHiringTermsLeaveAGuid UNIQUEIDENTIFIER = 'D162CA85-7670-4CA8-B619-3A358F41B42F',
	@retSalHiringTermsLeaveAName NVARCHAR(MAX) = @prefix + ' - Leave - Full time',
	@retSalHiringTermsLeaveADescription NVARCHAR(MAX) = '',
	@retSalHiringTermsLeaveAText NVARCHAR(MAX) = 'From your commencement date, you are entitled to leave in accordance with the relevant legislation and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016. These entitlements are processed as detailed in the IKEA policies.',
	@retSalHiringTermsLeaveAHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@retSalHiringTermsLeaveASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsLeaveAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsLeaveAName, 
		@retSalHiringTermsLeaveADescription,
		@retSalHiringTermsLeaveAText, 
		@retSalHiringTermsLeaveAHeadline,
		@retSalHiringTermsLeaveASortOrder,
		@retSalHiringTermsLeaveAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsLeaveAName, 
		[Description] = @retSalHiringTermsLeaveADescription, 
		[Text] = @retSalHiringTermsLeaveAText,
		[Headline] = @retSalHiringTermsLeaveAHeadline,
		SortOrder = @retSalHiringTermsLeaveASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsLeaveAGuid
END

DECLARE @retSalHiringTermsLeaveAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsLeaveAGuid)


-- Create condition for Leave Entitlements A
DECLARE @retSalHiringTermsLeaveACondGuid UNIQUEIDENTIFIER = '255C5482-EDEC-4FE1-BED5-D9ECBB20203B',
	@retSalHiringTermsLeaveACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringTermsLeaveACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalHiringTermsLeaveACondValues NVARCHAR(MAX) = '76',
	@retSalHiringTermsLeaveACondDescription NVARCHAR(MAX) = 'Is full time',
	@retSalHiringTermsLeaveACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsLeaveACondGuid)
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
		@retSalHiringTermsLeaveACondGuid,
		@retSalHiringTermsLeaveAID,
		@retSalHiringTermsLeaveACondPropertyName,
		@retSalHiringTermsLeaveACondOperator,
		@retSalHiringTermsLeaveACondValues,
		@retSalHiringTermsLeaveACondDescription,
		@retSalHiringTermsLeaveACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsLeaveAID,
		Property_Name = @retSalHiringTermsLeaveACondPropertyName,
		Operator = @retSalHiringTermsLeaveACondOperator,
		[Values] = @retSalHiringTermsLeaveACondValues,
		[Description] = @retSalHiringTermsLeaveACondDescription,
		[Status] = @retSalHiringTermsLeaveACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsLeaveACondGuid
END

---- Leave Entitlements  B
DECLARE @retSalHiringTermsLeaveBGuid UNIQUEIDENTIFIER = '896C5182-6B30-4E8A-BF19-86699F9672E6',
	@retSalHiringTermsLeaveBName NVARCHAR(MAX) = @prefix + ' - Leave - Part time',
	@retSalHiringTermsLeaveBDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsLeaveBText NVARCHAR(MAX) = 'You will accrue entitlements to leave in accordance with relevant legislation and company policy on a pro rata basis (compared to a full-time employee). As stated in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  These entitlements are processed as detailed in the IKEA policies.  This includes personal leave, annual leave, parental leave and long service leave.',
	@retSalHiringTermsLeaveBHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@retSalHiringTermsLeaveBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsLeaveBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsLeaveBName, 
		@retSalHiringTermsLeaveBDescription,
		@retSalHiringTermsLeaveBText, 
		@retSalHiringTermsLeaveBHeadline,
		@retSalHiringTermsLeaveBSortOrder,
		@retSalHiringTermsLeaveBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsLeaveBName, 
		[Description] = @retSalHiringTermsLeaveBDescription, 
		[Text] = @retSalHiringTermsLeaveBText,
		[Headline] = @retSalHiringTermsLeaveBHeadline,
		SortOrder = @retSalHiringTermsLeaveBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsLeaveBGuid
END
DECLARE @retSalHiringTermsLeaveBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsLeaveBGuid)

-- Create condition for leave entitlements B
DECLARE @retSalHiringTermsLeaveBCondGuid UNIQUEIDENTIFIER = 'E3334046-90F1-4599-82E4-39070FD873CF',
	@retSalHiringTermsLeaveBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalHiringTermsLeaveBCondOperator NVARCHAR(MAX) = 'LessThan',
	@retSalHiringTermsLeaveBCondValues NVARCHAR(MAX) = '76',
	@retSalHiringTermsLeaveBCondDescription NVARCHAR(MAX) = 'Is part time',
	@retSalHiringTermsLeaveBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalHiringTermsLeaveBCondGuid)
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
		@retSalHiringTermsLeaveBCondGuid,
		@retSalHiringTermsLeaveBID,
		@retSalHiringTermsLeaveBCondPropertyName,
		@retSalHiringTermsLeaveBCondOperator,
		@retSalHiringTermsLeaveBCondValues,
		@retSalHiringTermsLeaveBCondDescription,
		@retSalHiringTermsLeaveBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalHiringTermsLeaveBID,
		Property_Name = @retSalHiringTermsLeaveBCondPropertyName,
		Operator = @retSalHiringTermsLeaveBCondOperator,
		[Values] = @retSalHiringTermsLeaveBCondValues,
		[Description] = @retSalHiringTermsLeaveBCondDescription,
		[Status] = @retSalHiringTermsLeaveBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalHiringTermsLeaveBCondGuid
END

-- #################################### Issues Resolution
DECLARE @retSalHiringTermsIssuesGuid UNIQUEIDENTIFIER = 'EE681C14-64B3-42D2-90D8-70EE880A80DB',
	@retSalHiringTermsIssuesName NVARCHAR(MAX) = @prefix + ' - Issues Resolution',
	@retSalHiringTermsIssuesDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsIssuesText NVARCHAR(MAX) = 'If any issues arise during your employment with IKEA, the matter should initially be discussed with your immediate manager, in accordance with IKEA’s Issue Resolution Procedure.  If the problem remains unresolved, you may refer it to more senior levels of management for further discussion in accordance with the Issue Resolution Procedure and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.',
	@retSalHiringTermsIssuesHeadline NVARCHAR(MAX) = 'Issues Resolution',
	@retSalHiringTermsIssuesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsIssuesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsIssuesName, 
		@retSalHiringTermsIssuesDescription,
		@retSalHiringTermsIssuesText, 
		@retSalHiringTermsIssuesHeadline,
		@retSalHiringTermsIssuesSortOrder,
		@retSalHiringTermsIssuesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsIssuesName, 
		[Description] = @retSalHiringTermsIssuesDescription, 
		[Text] = @retSalHiringTermsIssuesText,
		[Headline] = @retSalHiringTermsIssuesHeadline,
		SortOrder = @retSalHiringTermsIssuesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsIssuesGuid
END

-- #################################### 20 Equal Employment Opportunity 
DECLARE @retSalHiringTermsEqualGuid UNIQUEIDENTIFIER = '1C24FE44-8A51-45E9-96F9-6F5CB8C305D3',
	@retSalHiringTermsEqualName NVARCHAR(MAX) = @prefix + ' - Equal Employment',
	@retSalHiringTermsEqualDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsEqualText NVARCHAR(MAX) = 'IKEA''s policy is to provide all co-workers with equal opportunity.  This policy precludes discrimination and harassment based on, but not limited to, race, colour, religion, gender, age, marital status and disability.  You are required to familiarise yourself with this policy and comply with it at all times.',
	@retSalHiringTermsEqualHeadline NVARCHAR(MAX) = 'Equal Employment Opportunity ',
	@retSalHiringTermsEqualSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsEqualGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsEqualName, 
		@retSalHiringTermsEqualDescription,
		@retSalHiringTermsEqualText, 
		@retSalHiringTermsEqualHeadline,
		@retSalHiringTermsEqualSortOrder,
		@retSalHiringTermsEqualGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsEqualName, 
		[Description] = @retSalHiringTermsEqualDescription, 
		[Text] = @retSalHiringTermsEqualText,
		[Headline] = @retSalHiringTermsEqualHeadline,
		SortOrder = @retSalHiringTermsEqualSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsEqualGuid
END

-- #################################### Uniform & Conduct
DECLARE @retSalHiringTermsUniformGuid UNIQUEIDENTIFIER = '1B7CAAF4-3606-442C-8CB8-4D98FA79BDE8',
	@retSalHiringTermsUniformName NVARCHAR(MAX) = @prefix + ' - Uniform & Conduct',
	@retSalHiringTermsUniformDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsUniformText NVARCHAR(MAX) = 'IKEA has established guidelines necessary for the professional appearance that IKEA expects all co-workers to present, and as such co-workers are supplied with uniforms and name badges within these guidelines.
<br><br>
Co-workers are expected to project a favourable and professional image for IKEA, and are to be courteous, efficient and reliable in their dealings with colleagues, existing and potential customers and suppliers to IKEA.',
	@retSalHiringTermsUniformHeadline NVARCHAR(MAX) = 'Uniform & Conduct',
	@retSalHiringTermsUniformSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsUniformGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsUniformName, 
		@retSalHiringTermsUniformDescription,
		@retSalHiringTermsUniformText, 
		@retSalHiringTermsUniformHeadline,
		@retSalHiringTermsUniformSortOrder,
		@retSalHiringTermsUniformGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsUniformName, 
		[Description] = @retSalHiringTermsUniformDescription, 
		[Text] = @retSalHiringTermsUniformText,
		[Headline] = @retSalHiringTermsUniformHeadline,
		SortOrder = @retSalHiringTermsUniformSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsUniformGuid
END

-- #################################### Induction & Ongoing Learning & Development
DECLARE @retSalHiringTermsInductionGuid UNIQUEIDENTIFIER = 'F953A7C5-92AD-4ECD-8441-5F09814C9EB7',
	@retSalHiringTermsInductionName NVARCHAR(MAX) = @prefix + ' - Induction',
	@retSalHiringTermsInductionDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsInductionText NVARCHAR(MAX) = 'IKEA is committed to your induction and ongoing development and as such, has a requirement that you undertake and are committed to training as offered by the company.  Whilst the majority of training is conducted on the job, you may be required from time to time to attend external training programs at different locations as organised by IKEA.
<br><br>
IKEA also encourages its co-workers to take responsibility for their own learning and development.',
	@retSalHiringTermsInductionHeadline NVARCHAR(MAX) = 'Induction & Ongoing Learning & Development',
	@retSalHiringTermsInductionSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsInductionGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsInductionName, 
		@retSalHiringTermsInductionDescription,
		@retSalHiringTermsInductionText, 
		@retSalHiringTermsInductionHeadline,
		@retSalHiringTermsInductionSortOrder,
		@retSalHiringTermsInductionGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsInductionName, 
		[Description] = @retSalHiringTermsInductionDescription, 
		[Text] = @retSalHiringTermsInductionText,
		[Headline] = @retSalHiringTermsInductionHeadline,
		SortOrder = @retSalHiringTermsInductionSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsInductionGuid
END

-- #################################### Occupational Health & Safety
DECLARE @retSalHiringTermsSafetyGuid UNIQUEIDENTIFIER = '068FED85-1E7F-4D2C-A3F9-F9D281188A70',
	@retSalHiringTermsSafetyName NVARCHAR(MAX) = @prefix + ' - Safety',
	@retSalHiringTermsSafetyDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsSafetyText NVARCHAR(MAX) = 'IKEA understands the requirement of ensuring a safe and healthy working environment for all co-workers in its offices, warehouses and stores, and a safe and healthy shopping environment for customers.  In fulfilling this aim, we undertake regular consultation with co-workers on health and safety issues and concerns.',
	@retSalHiringTermsSafetyHeadline NVARCHAR(MAX) = 'Occupational Health & Safety',
	@retSalHiringTermsSafetySortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsSafetyGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsSafetyName, 
		@retSalHiringTermsSafetyDescription,
		@retSalHiringTermsSafetyText, 
		@retSalHiringTermsSafetyHeadline,
		@retSalHiringTermsSafetySortOrder,
		@retSalHiringTermsSafetyGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsSafetyName, 
		[Description] = @retSalHiringTermsSafetyDescription, 
		[Text] = @retSalHiringTermsSafetyText,
		[Headline] = @retSalHiringTermsSafetyHeadline,
		SortOrder = @retSalHiringTermsSafetySortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsSafetyGuid
END

-- #################################### 24 Termination
DECLARE @retSalHiringTermsTerminationGuid UNIQUEIDENTIFIER = '62896AB1-1E21-4476-B403-2C08E9297DAF',
	@retSalHiringTermsTerminationName NVARCHAR(MAX) = @prefix + ' - Termination',
	@retSalHiringTermsTerminationDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsTerminationText NVARCHAR(MAX) = 'Either party may terminate the employment relationship with the appropriate notice as prescribed in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  Notice provisions do not apply in the case of summary dismissal.
<br><br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee.
<br><br>
IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.',
	@retSalHiringTermsTerminationHeadline NVARCHAR(MAX) = 'Termination',
	@retSalHiringTermsTerminationSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsTerminationGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsTerminationName, 
		@retSalHiringTermsTerminationDescription,
		@retSalHiringTermsTerminationText, 
		@retSalHiringTermsTerminationHeadline,
		@retSalHiringTermsTerminationSortOrder,
		@retSalHiringTermsTerminationGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsTerminationName, 
		[Description] = @retSalHiringTermsTerminationDescription, 
		[Text] = @retSalHiringTermsTerminationText,
		[Headline] = @retSalHiringTermsTerminationHeadline,
		SortOrder = @retSalHiringTermsTerminationSortOrder 
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsTerminationGuid
END

-- #################################### Company Policies & Procedures 
DECLARE @retSalHiringTermsPoliciesGuid UNIQUEIDENTIFIER = 'A4BE12B8-036B-4BFC-8368-51C26E837752',
	@retSalHiringTermsPoliciesName NVARCHAR(MAX) = @prefix + ' - Policies',
	@retSalHiringTermsPoliciesDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsPoliciesText NVARCHAR(MAX) = 'You shall be required to comply with all IKEA Policies and Procedures as amended from time to time and as outlined in IKEA’s Policy Guidelines and Welcome Program. The IKEA Policies and Procedures form part of your contract of employment.',
	@retSalHiringTermsPoliciesHeadline NVARCHAR(MAX) = 'Company Policies & Procedures',
	@retSalHiringTermsPoliciesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsPoliciesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsPoliciesName, 
		@retSalHiringTermsPoliciesDescription,
		@retSalHiringTermsPoliciesText, 
		@retSalHiringTermsPoliciesHeadline,
		@retSalHiringTermsPoliciesSortOrder,
		@retSalHiringTermsPoliciesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsPoliciesName, 
		[Description] = @retSalHiringTermsPoliciesDescription, 
		[Text] = @retSalHiringTermsPoliciesText,
		[Headline] = @retSalHiringTermsPoliciesHeadline,
		SortOrder = @retSalHiringTermsPoliciesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPoliciesGuid
END

-- #################################### 26 Other Terms and Conditions
DECLARE @retSalHiringTermsOtherTermsGuid UNIQUEIDENTIFIER = '32AD2BDD-8B69-49E7-9975-773F45DF856F',
	@retSalHiringTermsOtherTermsName NVARCHAR(MAX) = @prefix + ' - Other T&C',
	@retSalHiringTermsOtherTermsDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsOtherTermsText NVARCHAR(MAX) = 'The terms and conditions contained within the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016 (and any subsequent statutory agreement binding you and IKEA) also apply to your employment.  A copy of this Agreement is available for your perusal at all times.',
	@retSalHiringTermsOtherTermsHeadline NVARCHAR(MAX) = 'Other Terms and Conditions',
	@retSalHiringTermsOtherTermsSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsOtherTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsOtherTermsName, 
		@retSalHiringTermsOtherTermsDescription,
		@retSalHiringTermsOtherTermsText, 
		@retSalHiringTermsOtherTermsHeadline,
		@retSalHiringTermsOtherTermsSortOrder,
		@retSalHiringTermsOtherTermsGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsOtherTermsName, 
		[Description] = @retSalHiringTermsOtherTermsDescription, 
		[Text] = @retSalHiringTermsOtherTermsText,
		[Headline] = @retSalHiringTermsOtherTermsHeadline,
		SortOrder = @retSalHiringTermsOtherTermsSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsOtherTermsGuid
END

-- #################################### Police Checks
DECLARE @retSalHiringTermsPoliceGuid UNIQUEIDENTIFIER = '5FC0E186-B8C8-4A50-A32D-66F944BB3067',
	@retSalHiringTermsPoliceName NVARCHAR(MAX) = @prefix + ' - Police',
	@retSalHiringTermsPoliceDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsPoliceText NVARCHAR(MAX) = 'Some positions at IKEA require evidence of good character (for example - positions that deal with children or cash).  Obtaining details of your criminal history via a police check/s is an integral part of the assessment of your suitability for such positions.
<br><br>
You may be required to provide a police check/s at the time you are given this offer of employment.  Alternatively, you may be required to provide a police check/s during your employment (for instance, when it is suspected that you have incurred a criminal record since the commencement of your employment, or where you begin working in a position requiring evidence of good character).  By signing this offer of employment, you consent to complete, sign and lodge the relevant police check application documentation (which will be provided to you by IKEA), and to direct that the corresponding police check record/s be forwarded directly to IKEA (where permitted) or (otherwise) to provide IKEA with the original police check record/s immediately on receipt.
<br><br>
If you are required to provide the police check/s at the time you are given this offer of employment, you acknowledge that the offer of employment will be subject to the check being satisfactory to IKEA.
<br><br>
If you are required to provide the police check/s at any other time during your employment and the check is not satisfactory to IKEA, it may give grounds for dismissal.
<br><br>
Please note that the existence of a criminal record does not mean that the check will automatically be unsatisfactory, or that you will be assessed automatically as being unsuitable.  Each case will be assessed on its merits and will depend upon the inherent requirements of the position.',
	@retSalHiringTermsPoliceHeadline NVARCHAR(MAX) = 'Police Checks',
	@retSalHiringTermsPoliceSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsPoliceGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsPoliceName, 
		@retSalHiringTermsPoliceDescription,
		@retSalHiringTermsPoliceText, 
		@retSalHiringTermsPoliceHeadline,
		@retSalHiringTermsPoliceSortOrder,
		@retSalHiringTermsPoliceGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsPoliceName, 
		[Description] = @retSalHiringTermsPoliceDescription, 
		[Text] = @retSalHiringTermsPoliceText,
		[Headline] = @retSalHiringTermsPoliceHeadline,
		SortOrder = @retSalHiringTermsPoliceSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPoliceGuid
END

-- #################################### Performance Management
DECLARE @retSalHiringTermsPerfGuid UNIQUEIDENTIFIER = '5E663ED4-F484-4919-BC78-AAF1DD219076',
	@retSalHiringTermsPerfName NVARCHAR(MAX) = @prefix + ' - Performance',
	@retSalHiringTermsPerfDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsPerfText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November.  However, your first review will be conducted during your 6-month probationary period.  This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@retSalHiringTermsPerfHeadline NVARCHAR(MAX) = 'Performance Management',
	@retSalHiringTermsPerfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsPerfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsPerfName, 
		@retSalHiringTermsPerfDescription,
		@retSalHiringTermsPerfText, 
		@retSalHiringTermsPerfHeadline,
		@retSalHiringTermsPerfSortOrder,
		@retSalHiringTermsPerfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsPerfName, 
		[Description] = @retSalHiringTermsPerfDescription, 
		[Text] = @retSalHiringTermsPerfText,
		[Headline] = @retSalHiringTermsPerfHeadline,
		SortOrder = @retSalHiringTermsPerfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsPerfGuid
END

-- #################################### Communications with Media
DECLARE @retSalHiringTermsMediaGuid UNIQUEIDENTIFIER = '65542777-90DF-4329-8D91-EE885E4E431D',
	@retSalHiringTermsMediaName NVARCHAR(MAX) = @prefix + ' - Media',
	@retSalHiringTermsMediaDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsMediaText NVARCHAR(MAX) = 'You shall not provide information or speak on behalf of IKEA or otherwise to the media on any matters concerning IKEA’s business or activities.  You must refer all requests from the media for information and/or interviews to the DC Manager.',
	@retSalHiringTermsMediaHeadline NVARCHAR(MAX) = 'Communications with Media',
	@retSalHiringTermsMediaSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsMediaGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsMediaName, 
		@retSalHiringTermsMediaDescription,
		@retSalHiringTermsMediaText, 
		@retSalHiringTermsMediaHeadline,
		@retSalHiringTermsMediaSortOrder,
		@retSalHiringTermsMediaGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsMediaName, 
		[Description] = @retSalHiringTermsMediaDescription, 
		[Text] = @retSalHiringTermsMediaText,
		[Headline] = @retSalHiringTermsMediaHeadline,
		SortOrder = @retSalHiringTermsMediaSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsMediaGuid
END

-- #################################### Obligation to report unlawful activities
DECLARE @retSalHiringTermsUnlawGuid UNIQUEIDENTIFIER = '401FA0BD-0400-47E0-9972-AEA03B23B20F',
	@retSalHiringTermsUnlawName NVARCHAR(MAX) = @prefix + ' - Unlawful',
	@retSalHiringTermsUnlawDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsUnlawText NVARCHAR(MAX) = 'If you become aware of or suspect any unlawful act or omission by any IKEA employee, you must advise IKEA immediately.',
	@retSalHiringTermsUnlawHeadline NVARCHAR(MAX) = 'Obligation to report unlawful activities',
	@retSalHiringTermsUnlawSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsUnlawGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsUnlawName, 
		@retSalHiringTermsUnlawDescription,
		@retSalHiringTermsUnlawText, 
		@retSalHiringTermsUnlawHeadline,
		@retSalHiringTermsUnlawSortOrder,
		@retSalHiringTermsUnlawGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsUnlawName, 
		[Description] = @retSalHiringTermsUnlawDescription, 
		[Text] = @retSalHiringTermsUnlawText,
		[Headline] = @retSalHiringTermsUnlawHeadline,
		SortOrder = @retSalHiringTermsUnlawSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsUnlawGuid
END

-- #################################### Intellectual Property
DECLARE @retSalHiringTermsIntelPropGuid UNIQUEIDENTIFIER = 'A8515394-D0D9-4E37-8CCF-ADFF9EEB1866',
	@retSalHiringTermsIntelPropName NVARCHAR(MAX) = @prefix + ' - Int. Property',
	@retSalHiringTermsIntelPropDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsIntelPropText NVARCHAR(MAX) = 'IKEA owns all copyright in any works and all inventions, discoveries, novel designs, improvements or modifications, computer program material and trademarks which you write or develop in the course of your employment (in or out of working hours) (“Intellectual Property”).<br>
<br>
You assign to IKEA any interest you have in the Intellectual Property, and you must disclose any Intellectual Property to IKEA.<br>
<br>
During and after your employment, you must do anything IKEA reasonably requires (at IKEA''s cost) to:
<ul>
<li>obtain statutory protection (including by patent, design registration, trade mark registration or copyright) for the Intellectual Property for IKEA in any country; or</li>
<li>Perfect or evidence IKEA’s ownership of the Intellectual Property.</li>
</ul>',

	@retSalHiringTermsIntelPropHeadline NVARCHAR(MAX) = 'Intellectual Property',
	@retSalHiringTermsIntelPropSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsIntelPropGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsIntelPropName, 
		@retSalHiringTermsIntelPropDescription,
		@retSalHiringTermsIntelPropText, 
		@retSalHiringTermsIntelPropHeadline,
		@retSalHiringTermsIntelPropSortOrder,
		@retSalHiringTermsIntelPropGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsIntelPropName, 
		[Description] = @retSalHiringTermsIntelPropDescription, 
		[Text] = @retSalHiringTermsIntelPropText,
		[Headline] = @retSalHiringTermsIntelPropHeadline,
		SortOrder = @retSalHiringTermsIntelPropSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsIntelPropGuid
END

-- #################################### Variation
DECLARE @retSalHiringTermsVarGuid UNIQUEIDENTIFIER = 'B36C5152-9AA9-40EF-8721-5087518C67C4',
	@retSalHiringTermsVarName NVARCHAR(MAX) = @prefix + ' - Variation',
	@retSalHiringTermsVarDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsVarText NVARCHAR(MAX) = 'This Agreement may only be varied by a written agreement signed by yourself and IKEA.',
	@retSalHiringTermsVarHeadline NVARCHAR(MAX) = 'Variation',
	@retSalHiringTermsVarSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsVarGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsVarName, 
		@retSalHiringTermsVarDescription,
		@retSalHiringTermsVarText, 
		@retSalHiringTermsVarHeadline,
		@retSalHiringTermsVarSortOrder,
		@retSalHiringTermsVarGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsVarName, 
		[Description] = @retSalHiringTermsVarDescription, 
		[Text] = @retSalHiringTermsVarText,
		[Headline] = @retSalHiringTermsVarHeadline,
		SortOrder = @retSalHiringTermsVarSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsVarGuid
END

-- #################################### Suspension
DECLARE @retSalHiringTermsSuspGuid UNIQUEIDENTIFIER = '340560CC-8FF4-4720-A51F-B8E632ABBF1C',
	@retSalHiringTermsSuspName NVARCHAR(MAX) = @prefix + ' - Suspension',
	@retSalHiringTermsSuspDescription NVARCHAR(MAX) = '',
	@retSalHiringTermsSuspText NVARCHAR(MAX) = 'If we have reason to believe that you may have engaged in a serious breach of your employment obligations, IKEA may at its discretion suspend you from your duties, either with or without pay, while and investigation is conducted.',
	@retSalHiringTermsSuspHeadline NVARCHAR(MAX) = 'Suspension',
	@retSalHiringTermsSuspSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringTermsSuspGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringTermsID, 
		@retSalHiringTermsSuspName, 
		@retSalHiringTermsSuspDescription,
		@retSalHiringTermsSuspText, 
		@retSalHiringTermsSuspHeadline,
		@retSalHiringTermsSuspSortOrder,
		@retSalHiringTermsSuspGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringTermsID,
		[Name] = @retSalHiringTermsSuspName, 
		[Description] = @retSalHiringTermsSuspDescription, 
		[Text] = @retSalHiringTermsSuspText,
		[Headline] = @retSalHiringTermsSuspHeadline,
		SortOrder = @retSalHiringTermsSuspSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringTermsSuspGuid
END

-- Add terms paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @retSalHiringTermsID, @counter
SET @counter = @counter + 1


-- #################################### End Text
---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalHiringEndTextParagraphGuid UNIQUEIDENTIFIER = '8A22C400-407A-49E4-9D4B-6BBD7AC2FF17',
	@retSalHiringEndTextParagraphName NVARCHAR(MAX) = @prefix + ' - End Text',
	@retSalHiringEndTextParagraphType INT = @ParagraphTypeText,
	@retSalHiringEndTextParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalHiringEndTextParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalHiringEndTextParagraphName, @retSalHiringEndTextParagraphDescription, @retSalHiringEndTextParagraphType, @retSalHiringEndTextParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalHiringEndTextParagraphName, [Description] = @retSalHiringEndTextParagraphDescription, ParagraphType = @retSalHiringEndTextParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalHiringEndTextParagraphGuid
END
DECLARE @retSalHiringEndTextParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalHiringEndTextParagraphGuid)

-- Create a text field
DECLARE @retSalHiringEndTextGuid UNIQUEIDENTIFIER = '077A4ED8-39CA-4D9A-9968-A0AE0CEFD28E',
	@retSalHiringEndTextName NVARCHAR(MAX) = @prefix + ' - End Text',
	@retSalHiringEndTextDescription NVARCHAR(MAX) = '',
	@retSalHiringEndTextText NVARCHAR(MAX) = 'IKEA recognises that its co-workers are essential to the success of the company’s operations.  IKEA remains committed to ensuring that all co-workers are treated fairly and equitably and encourages co-workers to reach their full potential.  We believe that the basis of your employment outlined above, will achieve these objectives and greatly benefit those co-workers willing to develop themselves.
	<br><br>
	As an indication of your understanding and acceptance of these conditions, please sign this letter of offer, and return to the undersigned within seven (7) days.  Please retain the second copy for your records.
	<br><br>
	If you have any questions pertaining to this offer of employment or any of the information contained herein, please do not hesitate to contact me before signing this letter.',
	@retSalHiringEndTextHeadline NVARCHAR(MAX) = '',
	@retSalHiringEndTextSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringEndTextGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringEndTextParagraphID, 
		@retSalHiringEndTextName, 
		@retSalHiringEndTextDescription,
		@retSalHiringEndTextText, 
		@retSalHiringEndTextHeadline,
		@retSalHiringEndTextSortOrder,
		@retSalHiringEndTextGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringEndTextParagraphID,
		[Name] = @retSalHiringEndTextName, 
		[Description] = @retSalHiringEndTextDescription, 
		[Text] = @retSalHiringEndTextText,
		[Headline] = @retSalHiringEndTextHeadline,
		SortOrder = @retSalHiringEndTextSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringEndTextGuid
END

-- Add end text paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @retSalHiringEndTextParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Contractor Signature
---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalHiringConSignParagraphGuid UNIQUEIDENTIFIER = '7675D323-18F5-46FE-B2C4-6C25FFD33A7A',
	@retSalHiringConSignParagraphName NVARCHAR(MAX) = @prefix + ' - Con. Sign.',
	@retSalHiringConSignParagraphType INT = @ParagraphTypeText,
	@retSalHiringConSignParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalHiringConSignParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalHiringConSignParagraphName, @retSalHiringConSignParagraphDescription, @retSalHiringConSignParagraphType, @retSalHiringConSignParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalHiringConSignParagraphName, [Description] = @retSalHiringConSignParagraphDescription, ParagraphType = @retSalHiringConSignParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalHiringConSignParagraphGuid
END
DECLARE @retSalHiringConSignParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalHiringConSignParagraphGuid)

-- Create a text field
DECLARE @retSalHiringConSignGuid UNIQUEIDENTIFIER = 'E0D4CACD-C0AC-44D6-BE61-067A74D75D24',
	@retSalHiringConSignName NVARCHAR(MAX) = @prefix + ' - Con. Sign.',
	@retSalHiringConSignDescription NVARCHAR(MAX) = '',
	@retSalHiringConSignText NVARCHAR(MAX) = 'Yours sincerely<br>
	<Reports to Line Manager><br>
	<Position Title (Local Job Name)> of <Reports To Line Manager><br>
	<strong>IKEA Distribution Services Australia Pty Ltd</strong>',
	@retSalHiringConSignHeadline NVARCHAR(MAX) = '',
	@retSalHiringConSignSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringConSignGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringConSignParagraphID, 
		@retSalHiringConSignName, 
		@retSalHiringConSignDescription,
		@retSalHiringConSignText, 
		@retSalHiringConSignHeadline,
		@retSalHiringConSignSortOrder,
		@retSalHiringConSignGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringConSignParagraphID,
		[Name] = @retSalHiringConSignName, 
		[Description] = @retSalHiringConSignDescription, 
		[Text] = @retSalHiringConSignText,
		[Headline] = @retSalHiringConSignHeadline,
		SortOrder = @retSalHiringConSignSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringConSignGuid
END

-- Add contractor signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @retSalHiringConSignParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Acceptance
---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalHiringAcceptParagraphGuid UNIQUEIDENTIFIER = '22909EDC-ACE0-496E-8A24-6E0D25DD97BA',
	@retSalHiringAcceptParagraphName NVARCHAR(MAX) = @prefix + ' - Acceptance',
	@retSalHiringAcceptParagraphType INT = @ParagraphTypeText,
	@retSalHiringAcceptParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalHiringAcceptParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalHiringAcceptParagraphName, @retSalHiringAcceptParagraphDescription, @retSalHiringAcceptParagraphType, @retSalHiringAcceptParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalHiringAcceptParagraphName, [Description] = @retSalHiringAcceptParagraphDescription, ParagraphType = @retSalHiringAcceptParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalHiringAcceptParagraphGuid
END
DECLARE @retSalHiringAcceptParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalHiringAcceptParagraphGuid)

-- Create a text field
DECLARE @retSalHiringAcceptGuid UNIQUEIDENTIFIER = '746561EF-0FD3-46FA-A485-312E1C73D568',
	@retSalHiringAcceptName NVARCHAR(MAX) = @prefix + ' - Acceptance',
	@retSalHiringAcceptDescription NVARCHAR(MAX) = '',
	@retSalHiringAcceptText NVARCHAR(MAX) = '<table style="border: 1px solid black">
<tr><th align="center">ACCEPTANCE</th></tr>
<tr><td>I accept the terms and conditions of employment as detailed above.</td></tr>
<tr><td style="height:100px;vertical-align: bottom;"><Co-worker First Name> <Co-worker Last Name></td></tr>
<tr><td style="height:5px">.......................................</td></tr>
<tr><td style="vertical-align: top;">Name</td></tr>
<tr><td></td></tr>
<tr><td>.......................................</td></tr>
<tr><td style="vertical-align: top;">Signature</td></tr>
<tr><td></td></tr>
<tr><td>.......................................</td></tr>
<tr><td style="vertical-align: top;">Date</td></tr>
</table>',
	@retSalHiringAcceptHeadline NVARCHAR(MAX) = '',
	@retSalHiringAcceptSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalHiringAcceptGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalHiringAcceptParagraphID, 
		@retSalHiringAcceptName, 
		@retSalHiringAcceptDescription,
		@retSalHiringAcceptText, 
		@retSalHiringAcceptHeadline,
		@retSalHiringAcceptSortOrder,
		@retSalHiringAcceptGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalHiringAcceptParagraphID,
		[Name] = @retSalHiringAcceptName, 
		[Description] = @retSalHiringAcceptDescription, 
		[Text] = @retSalHiringAcceptText,
		[Headline] = @retSalHiringAcceptHeadline,
		SortOrder = @retSalHiringAcceptSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalHiringAcceptGuid
END

-- Add acceptance paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalHiringID, @retSalHiringAcceptParagraphID, @counter
SET @counter = @counter + 1




-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @retSalHiringGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



COMMIT

