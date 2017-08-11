--########################################
--########## DC EA T&C ###################
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

-- Prefix to names
DECLARE @prefix NVARCHAR(MAX) = 'DC EA T&C'

-- #################################### Contract Clusters – DC EA T&C ####################################

-- Get the form
DECLARE @dcTcGuid UNIQUEIDENTIFIER = 'AB07E815-2578-444E-8C58-BD28FE0CD502'
DECLARE @dcTcID INT, @counter INT = 0
SELECT @dcTcID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcTcGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @dcTcGuid


-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @footerID, @counter
SET @counter = @counter + 1

-- #################################### 1. Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### 2-8. Address and company info
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @addressInfoID, @counter
SET @counter = @counter + 1

-- #################################### 10a-b. Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @dcTcEmployGreetingGuid UNIQUEIDENTIFIER = 'bac42e4f-f520-449d-9e23-103ac6ac8591',
	@cdpName NVARCHAR(MAX) = @prefix + ' Employment greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcTcEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @dcTcEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcTcEmployGreetingGuid
END
DECLARE @dcTcEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcTcEmployGreetingGuid)

---- Create or update text A, Full Time
DECLARE @dcTcEmployGreetingTextAGuid UNIQUEIDENTIFIER = '224a94ef-a37d-4693-ad30-c7b79354c631',
	@dcTcEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Employment greeting, Text A - Full time',
	@dcTcEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@dcTcEmployGreetingTextAText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Full Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcTcEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@dcTcEmployGreetingTextASortOrder INT = 0


IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcEmployGreetingID, 
		@dcTcEmployGreetingTextAName, 
		@dcTcEmployGreetingTextADescription,
		@dcTcEmployGreetingTextAText, 
		@dcTcEmployGreetingTextAHeadline,
		@dcTcEmployGreetingTextASortOrder,
		@dcTcEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcEmployGreetingID,
		[Name] = @dcTcEmployGreetingTextAName, 
		[Description] = @dcTcEmployGreetingTextADescription, 
		[Text] = @dcTcEmployGreetingTextAText,
		[Headline] = @dcTcEmployGreetingTextAHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcEmployGreetingTextAGuid
END
DECLARE @dcTcEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @dcTcEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = '25e8cbac-b15c-44f8-82ca-a2de88b9b04b',
	@dcTcEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'Equal',
	@dcTcEmployGreetingTextACondAValues NVARCHAR(MAX) = '76',
	@dcTcEmployGreetingTextACondADescription NVARCHAR(MAX) = 'Is full time',
	@dcTcEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcEmployGreetingTextACondAGuid)
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
		@dcTcEmployGreetingTextACondAGuid,
		@dcTcEmployGreetingTextAID,
		@dcTcEmployGreetingTextACondAPropertyName,
		@dcTcEmployGreetingTextACondAOperator,
		@dcTcEmployGreetingTextACondAValues,
		@dcTcEmployGreetingTextACondADescription,
		@dcTcEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcEmployGreetingTextAID,
		Property_Name = @dcTcEmployGreetingTextACondAPropertyName,
		Operator = @dcTcEmployGreetingTextACondAOperator,
		[Values] = @dcTcEmployGreetingTextACondAValues,
		[Description] = @dcTcEmployGreetingTextACondADescription,
		[Status] = @dcTcEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @dcTcEmployGreetingTextBGuid UNIQUEIDENTIFIER = '9122a18f-4352-4396-99db-9be6fc0e0a2e',
	@dcTcEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Employment greeting, Text B - Part time',
	@dcTcEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@dcTcEmployGreetingTextBText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Part Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcTcEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@dcTcEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcEmployGreetingID, 
		@dcTcEmployGreetingTextBName, 
		@dcTcEmployGreetingTextBDescription,
		@dcTcEmployGreetingTextBText, 
		@dcTcEmployGreetingTextBHeadline,
		@dcTcEmployGreetingTextBSortOrder,
		@dcTcEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcEmployGreetingID,
		[Name] = @dcTcEmployGreetingTextBName, 
		[Description] = @dcTcEmployGreetingTextBDescription, 
		[Text] = @dcTcEmployGreetingTextBText,
		[Headline] = @dcTcEmployGreetingTextBHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcEmployGreetingTextBGuid
END
DECLARE @dcTcEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @dcTcEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = 'b23ebc8a-3a7c-4b93-9b7f-0a31cb08f797',
	@dcTcEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@dcTcEmployGreetingTextBCondAValues NVARCHAR(MAX) = '76',
	@dcTcEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Is part time',
	@dcTcEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcEmployGreetingTextBCondAGuid)
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
		@dcTcEmployGreetingTextBCondAGuid,
		@dcTcEmployGreetingTextBID,
		@dcTcEmployGreetingTextBCondAPropertyName,
		@dcTcEmployGreetingTextBCondAOperator,
		@dcTcEmployGreetingTextBCondAValues,
		@dcTcEmployGreetingTextBCondADescription,
		@dcTcEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcEmployGreetingTextBID,
		Property_Name = @dcTcEmployGreetingTextBCondAPropertyName,
		Operator = @dcTcEmployGreetingTextBCondAOperator,
		[Values] = @dcTcEmployGreetingTextBCondAValues,
		[Description] = @dcTcEmployGreetingTextBCondADescription,
		[Status] = @dcTcEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcEmployGreetingTextBCondAGuid
END


-- Add paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @dcTcEmployGreetingID, @counter
SET @counter = @counter + 1

-- #################################### 11a-33 Terms

DECLARE @termsCounter INT = 0
---- Create or update a terms paragraph
-- Paragraph guid
DECLARE @dcTcTermsGuid UNIQUEIDENTIFIER = 'a38eea6a-7794-4cd7-ad1a-dba7f59335b8',
	@dcTcTermsName NVARCHAR(MAX) = @prefix + ' Terms',
	@dcTcTermsParagraphType INT = @ParagraphTypeTableNumeric,
	@termsDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcTcTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcTcTermsName, @termsDescription, @dcTcTermsParagraphType, @dcTcTermsGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcTcTermsName, [Description] = @termsDescription, ParagraphType = @dcTcTermsParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcTcTermsGuid
END
DECLARE @dcTcTermsID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcTcTermsGuid)

-- #################################### 11a-b Position

---- Position A
DECLARE @dcTcTermsPositionAGuid UNIQUEIDENTIFIER = 'd84526d7-6791-4f2c-ab1a-78e38f63f616',
	@dcTcTermsPositionAName NVARCHAR(MAX) = @prefix + ' Employment - Position - Full time',
	@dcTcTermsPositionADescription NVARCHAR(MAX) = '',
	@dcTcTermsPositionAText NVARCHAR(MAX) = 'Your position is Full Time <Position Title (Local Job Name)>, located at <Business Unit>, reporting to the <Position Title (Local Job Name)> of <Reports To Line Manager>. Your position (in terms of your duties and responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcTcTermsPositionAHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcTcTermsPositionASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsPositionAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsPositionAName, 
		@dcTcTermsPositionADescription,
		@dcTcTermsPositionAText, 
		@dcTcTermsPositionAHeadline,
		@dcTcTermsPositionASortOrder,
		@dcTcTermsPositionAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsPositionAName, 
		[Description] = @dcTcTermsPositionADescription, 
		[Text] = @dcTcTermsPositionAText,
		[Headline] = @dcTcTermsPositionAHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPositionAGuid
END
DECLARE @dcTcTermsPositionAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPositionAGuid)

-- Create condition for position A
DECLARE @dcTcTermsPositionACondGuid UNIQUEIDENTIFIER = '5ecbbbd3-075d-4749-bd15-9ee4510ec364',
	@dcTcTermsPositionACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcTermsPositionACondOperator NVARCHAR(MAX) = 'Equal',
	@dcTcTermsPositionACondValues NVARCHAR(MAX) = '76',
	@dcTcTermsPositionACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcTcTermsPositionACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsPositionACondGuid)
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
		@dcTcTermsPositionACondGuid,
		@dcTcTermsPositionAID,
		@dcTcTermsPositionACondPropertyName,
		@dcTcTermsPositionACondOperator,
		@dcTcTermsPositionACondValues,
		@dcTcTermsPositionACondDescription,
		@dcTcTermsPositionACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsPositionAID,
		Property_Name = @dcTcTermsPositionACondPropertyName,
		Operator = @dcTcTermsPositionACondOperator,
		[Values] = @dcTcTermsPositionACondValues,
		[Description] = @dcTcTermsPositionACondDescription,
		[Status] = @dcTcTermsPositionACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsPositionACondGuid
END

---- Position B
DECLARE @dcTcTermsPositionBGuid UNIQUEIDENTIFIER = '5716b2bc-72f3-427c-82bc-e482f2fbd616',
	@dcTcTermsPositionBName NVARCHAR(MAX) = @prefix + ' Employment - Position - Part time',
	@dcTcTermsPositionBDescription NVARCHAR(MAX) = '',
	@dcTcTermsPositionBText NVARCHAR(MAX) = 'Your position is Part Time <Position Title (Local Job Name)> <Shift Type> Shift, reporting to  <Position Title (Local Job Name)> of <Reports To Line Manager>, which will be based at <Business Unit>. Your position (in terms of your duties & responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcTcTermsPositionBHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcTcTermsPositionBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsPositionBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsPositionBName, 
		@dcTcTermsPositionBDescription,
		@dcTcTermsPositionBText, 
		@dcTcTermsPositionBHeadline,
		@dcTcTermsPositionBSortOrder,
		@dcTcTermsPositionBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsPositionBName, 
		[Description] = @dcTcTermsPositionBDescription, 
		[Text] = @dcTcTermsPositionBText,
		[Headline] = @dcTcTermsPositionBHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPositionBGuid
END
DECLARE @dcTcTermsPositionBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPositionBGuid)

-- Create condition for position A
DECLARE @dcTcTermsPositionBCondGuid UNIQUEIDENTIFIER = '04a12eb0-859b-42b1-84b9-8b5ea9abed1a',
	@dcTcTermsPositionBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcTermsPositionBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcTcTermsPositionBCondValues NVARCHAR(MAX) = '76',
	@dcTcTermsPositionBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcTcTermsPositionBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsPositionBCondGuid)
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
		@dcTcTermsPositionBCondGuid,
		@dcTcTermsPositionBID,
		@dcTcTermsPositionBCondPropertyName,
		@dcTcTermsPositionBCondOperator,
		@dcTcTermsPositionBCondValues,
		@dcTcTermsPositionBCondDescription,
		@dcTcTermsPositionBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsPositionBID,
		Property_Name = @dcTcTermsPositionBCondPropertyName,
		Operator = @dcTcTermsPositionBCondOperator,
		[Values] = @dcTcTermsPositionBCondValues,
		[Description] = @dcTcTermsPositionBCondDescription,
		[Status] = @dcTcTermsPositionBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsPositionBCondGuid
END

-- DC EA T&C has no Contract start date, avoid this field for now
-- #################################### 12a-b Commencement Date
/*
---- Commencement A
DECLARE @dcTcTermsComAGuid UNIQUEIDENTIFIER = '93803913-df3b-4477-a545-cb0b15111444',
	@dcTcTermsComAName NVARCHAR(MAX) = @prefix + ' Employment - Commencement - No date',
	@dcTcTermsComADescription NVARCHAR(MAX) = '',
	@dcTcTermsComAText NVARCHAR(MAX) = 'Your commencement date is <Contract Start Date>, unless otherwise terminated in accordance with this contract.',
	@dcTcTermsComAHeadline NVARCHAR(MAX) = 'Commencement Date',
	@dcTcTermsComASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsComAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsComAName, 
		@dcTcTermsComADescription,
		@dcTcTermsComAText, 
		@dcTcTermsComAHeadline,
		@dcTcTermsComASortOrder,
		@dcTcTermsComAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsComAName, 
		[Description] = @dcTcTermsComADescription, 
		[Text] = @dcTcTermsComAText,
		[Headline] = @dcTcTermsComAHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsComAGuid
END
DECLARE @dcTcTermsComAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsComAGuid)

-- Create condition for Commencement A
DECLARE @dcTcTermsComACondAGuid UNIQUEIDENTIFIER = '471bdf02-2355-4986-94a7-8d01afffbb62',
	@dcTcTermsComACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcTcTermsComACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@dcTcTermsComACondAValues NVARCHAR(MAX) = '',
	@dcTcTermsComACondADescription NVARCHAR(MAX) = 'Has no end date',
	@dcTcTermsComACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsComACondAGuid)
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
		@dcTcTermsComACondAGuid,
		@dcTcTermsComAID,
		@dcTcTermsComACondAPropertyName,
		@dcTcTermsComACondAOperator,
		@dcTcTermsComACondAValues,
		@dcTcTermsComACondADescription,
		@dcTcTermsComACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsComAID,
		Property_Name = @dcTcTermsComACondAPropertyName,
		Operator = @dcTcTermsComACondAOperator,
		[Values] = @dcTcTermsComACondAValues,
		[Description] = @dcTcTermsComACondADescription,
		[Status] = @dcTcTermsComACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsComACondAGuid
END

-- No support for 31.12.9999 yet
--DECLARE @dcTcTermsComACondBGuid UNIQUEIDENTIFIER = 'e68e3e7c-52b0-4018-964b-99a1d9d471b9',
--	@dcTcTermsComACondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
--	@dcTcTermsComACondBOperator NVARCHAR(MAX) = 'Equal',
--	@dcTcTermsComACondBValues NVARCHAR(MAX) = '31.12.9999',
--	@dcTcTermsComACondBDescription NVARCHAR(MAX) = 'Equal 31.12.9999',
--	@dcTcTermsComACondBStatus INT = 1

--IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsComACondBGuid)
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
--		@dcTcTermsComACondBGuid,
--		@dcTcTermsComAID,
--		@dcTcTermsComACondBPropertyName,
--		@dcTcTermsComACondBOperator,
--		@dcTcTermsComACondBValues,
--		@dcTcTermsComACondBDescription,
--		@dcTcTermsComACondBStatus,
--		@now, 
--		@userID,
--		@now,
--		@userID
--	)
--END
--ELSE
--BEGIN
--	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsComAID,
--		Property_Name = @dcTcTermsComACondBPropertyName,
--		Operator = @dcTcTermsComACondBOperator,
--		[Values] = @dcTcTermsComACondBValues,
--		[Description] = @dcTcTermsComACondBDescription,
--		[Status] = @dcTcTermsComACondBStatus,
--		CreatedDate = @now,
--		CreatedByUser_Id = @userID,
--		ChangedDate = @now,
--		ChangedByUser_Id = @userID
--	FROM tblCaseDocumentTextCondition CDTC
--	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsComACondBGuid
--END

---- Position B
DECLARE @dcTcTermsComBGuid UNIQUEIDENTIFIER = 'e367cd79-7c5c-40d5-b334-3c1991b735dc',
	@dcTcTermsComBName NVARCHAR(MAX) = @prefix + ' Employment - Commencement - Has end date',
	@dcTcTermsComBDescription NVARCHAR(MAX) = '',
	@dcTcTermsComBText NVARCHAR(MAX) = 'Your commencement date is <Contract Start Date> and will cease on <Contract End Date>, unless otherwise terminated in accordance with this contract.',
	@dcTcTermsComBHeadline NVARCHAR(MAX) = 'Commencement date',
	@dcTcTermsComBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsComBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsComBName, 
		@dcTcTermsComBDescription,
		@dcTcTermsComBText, 
		@dcTcTermsComBHeadline,
		@dcTcTermsComBSortOrder,
		@dcTcTermsComBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsComBName, 
		[Description] = @dcTcTermsComBDescription, 
		[Text] = @dcTcTermsComBText,
		[Headline] = @dcTcTermsComBHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsComBGuid
END
DECLARE @dcTcTermsComBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsComBGuid)

-- Create condition for Commence B
DECLARE @dcTcTermsComBCondAGuid UNIQUEIDENTIFIER = '5b78476f-b1c8-438d-b7ed-b3d5c8c0dd77',
	@dcTcTermsComBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcTcTermsComBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@dcTcTermsComBCondAValues NVARCHAR(MAX) = '',
	@dcTcTermsComBCondADescription NVARCHAR(MAX) = 'Has end date',
	@dcTcTermsComBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsComBCondAGuid)
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
		@dcTcTermsComBCondAGuid,
		@dcTcTermsComBID,
		@dcTcTermsComBCondAPropertyName,
		@dcTcTermsComBCondAOperator,
		@dcTcTermsComBCondAValues,
		@dcTcTermsComBCondADescription,
		@dcTcTermsComBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsComBID,
		Property_Name = @dcTcTermsComBCondAPropertyName,
		Operator = @dcTcTermsComBCondAOperator,
		[Values] = @dcTcTermsComBCondAValues,
		[Description] = @dcTcTermsComBCondADescription,
		[Status] = @dcTcTermsComBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsComBCondAGuid
END

-- No support for 31.12.9999 yet
/*DECLARE @dcTcTermsComBCondBGuid UNIQUEIDENTIFIER = '43c72a21-96c1-4d3c-a44a-5279593332c7',
	@dcTcTermsComBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcTcTermsComBCondBOperator NVARCHAR(MAX) = 'NotEqual',
	@dcTcTermsComBCondBValues NVARCHAR(MAX) = '31.12.9999',
	@dcTcTermsComBCondBDescription NVARCHAR(MAX) = 'Not equals 31.12.9999',
	@dcTcTermsComBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsComBCondBGuid)
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
		@dcTcTermsComBCondBGuid,
		@dcTcTermsComBID,
		@dcTcTermsComBCondBPropertyName,
		@dcTcTermsComBCondBOperator,
		@dcTcTermsComBCondBValues,
		@dcTcTermsComBCondBDescription,
		@dcTcTermsComBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsComBID,
		Property_Name = @dcTcTermsComBCondBPropertyName,
		Operator = @dcTcTermsComBCondBOperator,
		[Values] = @dcTcTermsComBCondBValues,
		[Description] = @dcTcTermsComBCondBDescription,
		[Status] = @dcTcTermsComBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsComBCondBGuid
END*/

*/
-- #################################### 13a-b Hours of Work

---- Hours of Work A
DECLARE @dcTcTermsHWAGuid UNIQUEIDENTIFIER = 'a5f8db11-a408-4aac-af4c-0f9cfd7a55dc',
	@dcTcTermsHWAName NVARCHAR(MAX) = @prefix + ' Employment - Hours of Work - Full time',
	@dcTcTermsHWADescription NVARCHAR(MAX) = '',
	@dcTcTermsHWAText NVARCHAR(MAX) = 
	'You will be rostered to work 76 ordinary hours per fortnight.  Such details of your initial roster will be discussed with you upon your commencement.  However, where there is a change in the business’ needs, your hours may also be subject to change with appropriate notice.
You should note that ordinary hours in the Distribution Centre include Saturday’s and you have mutually agreed to work more than one in three Saturdays as part of your contracted ordinary hours.',
	@dcTcTermsHWAHeadline NVARCHAR(MAX) = 'Position',
	@dcTcTermsHWASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsHWAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsHWAName, 
		@dcTcTermsHWADescription,
		@dcTcTermsHWAText, 
		@dcTcTermsHWAHeadline,
		@dcTcTermsHWASortOrder,
		@dcTcTermsHWAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsHWAName, 
		[Description] = @dcTcTermsHWADescription, 
		[Text] = @dcTcTermsHWAText,
		[Headline] = @dcTcTermsHWAHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsHWAGuid
END
DECLARE @dcTcTermsHWAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsHWAGuid)

-- Create condition for Hours of Work A
DECLARE @dcTcTermsHWACondGuid UNIQUEIDENTIFIER = 'c006898f-24ca-4ab9-a900-28e3032f260b',
	@dcTcTermsHWACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcTermsHWACondOperator NVARCHAR(MAX) = 'Equal',
	@dcTcTermsHWACondValues NVARCHAR(MAX) = '76',
	@dcTcTermsHWACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcTcTermsHWACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsHWACondGuid)
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
		@dcTcTermsHWACondGuid,
		@dcTcTermsHWAID,
		@dcTcTermsHWACondPropertyName,
		@dcTcTermsHWACondOperator,
		@dcTcTermsHWACondValues,
		@dcTcTermsHWACondDescription,
		@dcTcTermsHWACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsHWAID,
		Property_Name = @dcTcTermsHWACondPropertyName,
		Operator = @dcTcTermsHWACondOperator,
		[Values] = @dcTcTermsHWACondValues,
		[Description] = @dcTcTermsHWACondDescription,
		[Status] = @dcTcTermsHWACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsHWACondGuid
END

---- Hours of Work B
DECLARE @dcTcTermsHWBGuid UNIQUEIDENTIFIER = 'b055c31a-16e4-4544-b542-30ca4204d2a2',
	@dcTcTermsHWBName NVARCHAR(MAX) = @prefix + ' Employment - Hours of Work - Part time',
	@dcTcTermsHWBDescription NVARCHAR(MAX) = '',
	@dcTcTermsHWBText NVARCHAR(MAX) = 
	'Your contracted hours are <Contracted Hours> per fortnight, you may be offered additional ‘varied hours’ paid at your ordinary rate of pay.
	You will be rostered in accordance to your availability schedule which you filled out at the time of your employment. Your availability schedule forms part of your employment contract.',
	@dcTcTermsHWBHeadline NVARCHAR(MAX) = 'Hours of Work',
	@dcTcTermsHWBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsHWBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsHWBName, 
		@dcTcTermsHWBDescription,
		@dcTcTermsHWBText, 
		@dcTcTermsHWBHeadline,
		@dcTcTermsHWBSortOrder,
		@dcTcTermsHWBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsHWBName, 
		[Description] = @dcTcTermsHWBDescription, 
		[Text] = @dcTcTermsHWBText,
		[Headline] = @dcTcTermsHWBHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsHWBGuid
END
DECLARE @dcTcTermsHWBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsHWBGuid)

-- Create condition for hours of work B
DECLARE @dcTcTermsHWBCondGuid UNIQUEIDENTIFIER = '32ab33cf-2f93-4bf5-a371-978fe20f3ed3',
	@dcTcTermsHWBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcTermsHWBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcTcTermsHWBCondValues NVARCHAR(MAX) = '76',
	@dcTcTermsHWBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcTcTermsHWBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsHWBCondGuid)
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
		@dcTcTermsHWBCondGuid,
		@dcTcTermsHWBID,
		@dcTcTermsHWBCondPropertyName,
		@dcTcTermsHWBCondOperator,
		@dcTcTermsHWBCondValues,
		@dcTcTermsHWBCondDescription,
		@dcTcTermsHWBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsHWBID,
		Property_Name = @dcTcTermsHWBCondPropertyName,
		Operator = @dcTcTermsHWBCondOperator,
		[Values] = @dcTcTermsHWBCondValues,
		[Description] = @dcTcTermsHWBCondDescription,
		[Status] = @dcTcTermsHWBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsHWBCondGuid
END


-- #################################### 14 Probationary Period

---- Probationary Period
DECLARE @dcTcTermsProbTimeGuid UNIQUEIDENTIFIER = '63913abd-c604-4a0f-a380-1838ceb23bf2',
	@dcTcTermsProbTimeName NVARCHAR(MAX) = @prefix + ' Employment - Probationary Period',
	@dcTcTermsProbTimeDescription NVARCHAR(MAX) = '',
	@dcTcTermsProbTimeText NVARCHAR(MAX) = 'IKEA offers this employment to you on a probationary basis for a period of six (6) months, during which time your performance standards will be subject to regular review and assessment.  In the six (6)-month period, if either you or IKEA wishes to terminate the employment relationship, then either party can effect that termination with one week’s notice in writing.',
	@dcTcTermsProbTimeHeadline NVARCHAR(MAX) = 'Probationary Period',
	@dcTcTermsProbTimeSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsProbTimeGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsProbTimeName, 
		@dcTcTermsProbTimeDescription,
		@dcTcTermsProbTimeText, 
		@dcTcTermsProbTimeHeadline,
		@dcTcTermsProbTimeSortOrder,
		@dcTcTermsProbTimeGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsProbTimeName, 
		[Description] = @dcTcTermsProbTimeDescription, 
		[Text] = @dcTcTermsProbTimeText,
		[Headline] = @dcTcTermsProbTimeHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsProbTimeGuid
END

DECLARE @dcTcTermsProbID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsProbTimeGuid)

-- Create condition for probation period 
DECLARE @dcTcTermsProbCondGuid UNIQUEIDENTIFIER = '32ab33cf-2f93-4bf5-a371-978fe20f3ed3',
	@dcTcTermsProbCondPropertyName NVARCHAR(MAX) = 'extendedcase_ProbationPeriod',
	@dcTcTermsProbCondOperator NVARCHAR(MAX) = 'Equal',
	@dcTcTermsProbCondValues NVARCHAR(MAX) = 'Yes',
	@dcTcTermsProbCondDescription NVARCHAR(MAX) = 'Has probation period',
	@dcTcTermsProbCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsProbCondGuid)
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
		@dcTcTermsProbCondGuid,
		@dcTcTermsProbID,
		@dcTcTermsProbCondPropertyName,
		@dcTcTermsProbCondOperator,
		@dcTcTermsProbCondValues,
		@dcTcTermsProbCondDescription,
		@dcTcTermsProbCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsProbID,
		Property_Name = @dcTcTermsProbCondPropertyName,
		Operator = @dcTcTermsProbCondOperator,
		[Values] = @dcTcTermsProbCondValues,
		[Description] = @dcTcTermsProbCondDescription,
		[Status] = @dcTcTermsProbCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsProbCondGuid
END



-- #################################### 15 Remuneration
DECLARE @dcTcTermsRemunGuid UNIQUEIDENTIFIER = '91714198-1a98-4040-be64-207db37023e2',
	@dcTcTermsRemunName NVARCHAR(MAX) = @prefix + ' Employment - Remuneration',
	@dcTcTermsRemunDescription NVARCHAR(MAX) = '',
	@dcTcTermsRemunText NVARCHAR(MAX) = 'Upon commencement, your base hourly rate will be as per the <b>IKEA Distributions Services Australia Pty Ltd Enterprise Agreement 2016</b>.  This amount will be paid directly into your nominated bank account on a fortnightly basis.',
	@dcTcTermsRemunHeadline NVARCHAR(MAX) = 'Remuneration',
	@dcTcTermsRemunSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsRemunGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsRemunName, 
		@dcTcTermsRemunDescription,
		@dcTcTermsRemunText, 
		@dcTcTermsRemunHeadline,
		@dcTcTermsRemunSortOrder,
		@dcTcTermsRemunGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsRemunName, 
		[Description] = @dcTcTermsRemunDescription, 
		[Text] = @dcTcTermsRemunText,
		[Headline] = @dcTcTermsRemunHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsRemunGuid
END

-- #################################### 16 Superannuation
DECLARE @dcTcTermsSuperGuid UNIQUEIDENTIFIER = 'fa1b4355-4524-4bc1-ade6-0a9431ef2b10',
	@dcTcTermsSuperName NVARCHAR(MAX) = @prefix + ' Employment - Superannuation',
	@dcTcTermsSuperDescription NVARCHAR(MAX) = '',
	@dcTcTermsSuperText NVARCHAR(MAX) = 'IKEA will make superannuation contributions, on your behalf, to a government approved Superannuation Fund, at the rate payable under the Superannuation Guarantee Legislation (SGL). This rate is currently 9.5% of your wage.
<br>
IKEA’s current employer superannuation fund is the Labour Union Co-operative Retirement Fund (LUCRF), which is the fund into which the superannuation contributions will be made unless an alternate fund is nominated by you in writing, in accordance with the SGL.
<br>
It is your responsibility to nominate a Super Fund for your contributions to be made to, and to ensure that you complete the necessary paperwork for enrolment into your nominated fund.  IKEA will supply you with a LUCRF Member Guide, including an application form.',
	@dcTcTermsSuperHeadline NVARCHAR(MAX) = 'Superannuation',
	@dcTcTermsSuperSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsSuperGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsSuperName, 
		@dcTcTermsSuperDescription,
		@dcTcTermsSuperText, 
		@dcTcTermsSuperHeadline,
		@dcTcTermsSuperSortOrder,
		@dcTcTermsSuperGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsSuperName, 
		[Description] = @dcTcTermsSuperDescription, 
		[Text] = @dcTcTermsSuperText,
		[Headline] = @dcTcTermsSuperHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsSuperGuid
END


-- #################################### 17 Confidential Information
DECLARE @dcTcTermsConfGuid UNIQUEIDENTIFIER = '53f40d5e-a14e-4b0b-afbe-eedbbe2a07ad',
	@dcTcTermsConfName NVARCHAR(MAX) = @prefix + ' Employment - Confidential Information',
	@dcTcTermsConfDescription NVARCHAR(MAX) = '',
	@dcTcTermsConfText NVARCHAR(MAX) = 'In the course of your employment, you may be exposed to “Confidential Information” concerning IKEA. Confidential Information means any information obtained by you in the course of your employment, including:
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
	@dcTcTermsConfHeadline NVARCHAR(MAX) = 'Confidential Information',
	@dcTcTermsConfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsConfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsConfName, 
		@dcTcTermsConfDescription,
		@dcTcTermsConfText, 
		@dcTcTermsConfHeadline,
		@dcTcTermsConfSortOrder,
		@dcTcTermsConfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsConfName, 
		[Description] = @dcTcTermsConfDescription, 
		[Text] = @dcTcTermsConfText,
		[Headline] = @dcTcTermsConfHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsConfGuid
END

-- #################################### 18a-b Leave Entitlements 

---- Leave Entitlements  A
DECLARE @dcTcTermsLeaveAGuid UNIQUEIDENTIFIER = 'ec1d8852-2d25-4440-b051-f3022f343526',
	@dcTcTermsLeaveAName NVARCHAR(MAX) = @prefix + ' Employment - Leave - Full time',
	@dcTcTermsLeaveADescription NVARCHAR(MAX) = '',
	@dcTcTermsLeaveAText NVARCHAR(MAX) = 'From your commencement date, you are entitled to leave in accordance with the relevant legislation and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016. These entitlements are processed as detailed in the IKEA policies.',
	@dcTcTermsLeaveAHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@dcTcTermsLeaveASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsLeaveAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsLeaveAName, 
		@dcTcTermsLeaveADescription,
		@dcTcTermsLeaveAText, 
		@dcTcTermsLeaveAHeadline,
		@dcTcTermsLeaveASortOrder,
		@dcTcTermsLeaveAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsLeaveAName, 
		[Description] = @dcTcTermsLeaveADescription, 
		[Text] = @dcTcTermsLeaveAText,
		[Headline] = @dcTcTermsLeaveAHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsLeaveAGuid
END

DECLARE @dcTcTermsLeaveAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsLeaveAGuid)


-- Create condition for Leave Entitlements A
DECLARE @dcTcTermsLeaveACondGuid UNIQUEIDENTIFIER = '443f9b30-d3a1-4129-9cd1-4108fa64755e',
	@dcTcTermsLeaveACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcTermsLeaveACondOperator NVARCHAR(MAX) = 'Equal',
	@dcTcTermsLeaveACondValues NVARCHAR(MAX) = '76',
	@dcTcTermsLeaveACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcTcTermsLeaveACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsLeaveACondGuid)
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
		@dcTcTermsLeaveACondGuid,
		@dcTcTermsLeaveAID,
		@dcTcTermsLeaveACondPropertyName,
		@dcTcTermsLeaveACondOperator,
		@dcTcTermsLeaveACondValues,
		@dcTcTermsLeaveACondDescription,
		@dcTcTermsLeaveACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsLeaveAID,
		Property_Name = @dcTcTermsLeaveACondPropertyName,
		Operator = @dcTcTermsLeaveACondOperator,
		[Values] = @dcTcTermsLeaveACondValues,
		[Description] = @dcTcTermsLeaveACondDescription,
		[Status] = @dcTcTermsLeaveACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsLeaveACondGuid
END

---- Leave Entitlements  B
DECLARE @dcTcTermsLeaveBGuid UNIQUEIDENTIFIER = 'c8417748-c47b-419d-8eea-2d36e57bccbb',
	@dcTcTermsLeaveBName NVARCHAR(MAX) = @prefix + ' Employment - Leave - Part time',
	@dcTcTermsLeaveBDescription NVARCHAR(MAX) = '',
	@dcTcTermsLeaveBText NVARCHAR(MAX) = 'You will accrue entitlements to leave in accordance with relevant legislation and company policy on a pro rata basis (compared to a full-time employee). As stated in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  These entitlements are processed as detailed in the IKEA policies.  This includes personal leave, annual leave, parental leave and long service leave.',
	@dcTcTermsLeaveBHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@dcTcTermsLeaveBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsLeaveBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsLeaveBName, 
		@dcTcTermsLeaveBDescription,
		@dcTcTermsLeaveBText, 
		@dcTcTermsLeaveBHeadline,
		@dcTcTermsLeaveBSortOrder,
		@dcTcTermsLeaveBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsLeaveBName, 
		[Description] = @dcTcTermsLeaveBDescription, 
		[Text] = @dcTcTermsLeaveBText,
		[Headline] = @dcTcTermsLeaveBHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsLeaveBGuid
END
DECLARE @dcTcTermsLeaveBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcTcTermsLeaveBGuid)

-- Create condition for leave entitlements B
DECLARE @dcTcTermsLeaveBCondGuid UNIQUEIDENTIFIER = '4f2c7207-adf5-430e-9207-3e0596cef7fb',
	@dcTcTermsLeaveBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcTcTermsLeaveBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcTcTermsLeaveBCondValues NVARCHAR(MAX) = '76',
	@dcTcTermsLeaveBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcTcTermsLeaveBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcTcTermsLeaveBCondGuid)
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
		@dcTcTermsLeaveBCondGuid,
		@dcTcTermsLeaveBID,
		@dcTcTermsLeaveBCondPropertyName,
		@dcTcTermsLeaveBCondOperator,
		@dcTcTermsLeaveBCondValues,
		@dcTcTermsLeaveBCondDescription,
		@dcTcTermsLeaveBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcTcTermsLeaveBID,
		Property_Name = @dcTcTermsLeaveBCondPropertyName,
		Operator = @dcTcTermsLeaveBCondOperator,
		[Values] = @dcTcTermsLeaveBCondValues,
		[Description] = @dcTcTermsLeaveBCondDescription,
		[Status] = @dcTcTermsLeaveBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcTcTermsLeaveBCondGuid
END

-- #################################### 19 Issues Resolution
DECLARE @dcTcTermsIssuesGuid UNIQUEIDENTIFIER = 'c55a5c5f-9271-46f0-9fb5-b43c1e43b0d6',
	@dcTcTermsIssuesName NVARCHAR(MAX) = @prefix + ' Employment - Issues Resolution',
	@dcTcTermsIssuesDescription NVARCHAR(MAX) = '',
	@dcTcTermsIssuesText NVARCHAR(MAX) = 'If any issues arise during your employment with IKEA, the matter should initially be discussed with your immediate manager, in accordance with IKEA’s Issue Resolution Procedure.  If the problem remains unresolved, you may refer it to more senior levels of management for further discussion in accordance with the Issue Resolution Procedure and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.',
	@dcTcTermsIssuesHeadline NVARCHAR(MAX) = 'Issues Resolution',
	@dcTcTermsIssuesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsIssuesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsIssuesName, 
		@dcTcTermsIssuesDescription,
		@dcTcTermsIssuesText, 
		@dcTcTermsIssuesHeadline,
		@dcTcTermsIssuesSortOrder,
		@dcTcTermsIssuesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsIssuesName, 
		[Description] = @dcTcTermsIssuesDescription, 
		[Text] = @dcTcTermsIssuesText,
		[Headline] = @dcTcTermsIssuesHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsIssuesGuid
END

-- #################################### 20 Equal Employment Opportunity 
DECLARE @dcTcTermsEqualGuid UNIQUEIDENTIFIER = 'a8c2b37f-051e-4d32-b87c-ba9930f599a1',
	@dcTcTermsEqualName NVARCHAR(MAX) = @prefix + ' Employment - Equal Employment',
	@dcTcTermsEqualDescription NVARCHAR(MAX) = '',
	@dcTcTermsEqualText NVARCHAR(MAX) = 'IKEA''s policy is to provide all co-workers with equal opportunity.  This policy precludes discrimination and harassment based on, but not limited to, race, colour, religion, gender, age, marital status and disability.  You are required to familiarise yourself with this policy and comply with it at all times.',
	@dcTcTermsEqualHeadline NVARCHAR(MAX) = 'Equal Employment Opportunity ',
	@dcTcTermsEqualSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsEqualGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsEqualName, 
		@dcTcTermsEqualDescription,
		@dcTcTermsEqualText, 
		@dcTcTermsEqualHeadline,
		@dcTcTermsEqualSortOrder,
		@dcTcTermsEqualGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsEqualName, 
		[Description] = @dcTcTermsEqualDescription, 
		[Text] = @dcTcTermsEqualText,
		[Headline] = @dcTcTermsEqualHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsEqualGuid
END

-- #################################### 21 Uniform & Conduct
DECLARE @dcTcTermsUniformGuid UNIQUEIDENTIFIER = '8661a234-5724-41b6-9ca5-16dd672dcf04',
	@dcTcTermsUniformName NVARCHAR(MAX) = @prefix + ' Employment - Uniform & Conduct',
	@dcTcTermsUniformDescription NVARCHAR(MAX) = '',
	@dcTcTermsUniformText NVARCHAR(MAX) = 'IKEA has established guidelines necessary for the professional appearance that IKEA expects all co-workers to present, and as such co-workers are supplied with uniforms and name badges within these guidelines.
<br><br>
Co-workers are expected to project a favourable and professional image for IKEA, and are to be courteous, efficient and reliable in their dealings with colleagues, existing and potential customers and suppliers to IKEA.',
	@dcTcTermsUniformHeadline NVARCHAR(MAX) = 'Uniform & Conduct',
	@dcTcTermsUniformSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsUniformGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsUniformName, 
		@dcTcTermsUniformDescription,
		@dcTcTermsUniformText, 
		@dcTcTermsUniformHeadline,
		@dcTcTermsUniformSortOrder,
		@dcTcTermsUniformGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsUniformName, 
		[Description] = @dcTcTermsUniformDescription, 
		[Text] = @dcTcTermsUniformText,
		[Headline] = @dcTcTermsUniformHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsUniformGuid
END

-- #################################### 22 Induction & Ongoing Learning & Development
DECLARE @dcTcTermsInductionGuid UNIQUEIDENTIFIER = '03a639f7-eab2-4bd2-abed-e930d3894a67',
	@dcTcTermsInductionName NVARCHAR(MAX) = @prefix + ' Employment - Induction',
	@dcTcTermsInductionDescription NVARCHAR(MAX) = '',
	@dcTcTermsInductionText NVARCHAR(MAX) = 'IKEA is committed to your induction and ongoing development and as such, has a requirement that you undertake and are committed to training as offered by the company.  Whilst the majority of training is conducted on the job, you may be required from time to time to attend external training programs at different locations as organised by IKEA.
<br><br>
IKEA also encourages its co-workers to take responsibility for their own learning and development.',
	@dcTcTermsInductionHeadline NVARCHAR(MAX) = 'Induction & Ongoing Learning & Development',
	@dcTcTermsInductionSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsInductionGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsInductionName, 
		@dcTcTermsInductionDescription,
		@dcTcTermsInductionText, 
		@dcTcTermsInductionHeadline,
		@dcTcTermsInductionSortOrder,
		@dcTcTermsInductionGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsInductionName, 
		[Description] = @dcTcTermsInductionDescription, 
		[Text] = @dcTcTermsInductionText,
		[Headline] = @dcTcTermsInductionHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsInductionGuid
END

-- #################################### 23 Occupational Health & Safety
DECLARE @dcTcTermsSafetyGuid UNIQUEIDENTIFIER = 'd77f8309-6bb0-41ab-b93c-053f73a2b34d',
	@dcTcTermsSafetyName NVARCHAR(MAX) = @prefix + ' Employment - Safety',
	@dcTcTermsSafetyDescription NVARCHAR(MAX) = '',
	@dcTcTermsSafetyText NVARCHAR(MAX) = 'IKEA understands the requirement of ensuring a safe and healthy working environment for all co-workers in its offices, warehouses and stores, and a safe and healthy shopping environment for customers.  In fulfilling this aim, we undertake regular consultation with co-workers on health and safety issues and concerns.',
	@dcTcTermsSafetyHeadline NVARCHAR(MAX) = 'Occupational Health & Safety',
	@dcTcTermsSafetySortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsSafetyGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsSafetyName, 
		@dcTcTermsSafetyDescription,
		@dcTcTermsSafetyText, 
		@dcTcTermsSafetyHeadline,
		@dcTcTermsSafetySortOrder,
		@dcTcTermsSafetyGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsSafetyName, 
		[Description] = @dcTcTermsSafetyDescription, 
		[Text] = @dcTcTermsSafetyText,
		[Headline] = @dcTcTermsSafetyHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsSafetyGuid
END

-- #################################### 24 Termination
DECLARE @dcTcTermsTerminationGuid UNIQUEIDENTIFIER = '43393eed-c718-4bf4-a588-e3c2945f1ccc',
	@dcTcTermsTerminationName NVARCHAR(MAX) = @prefix + ' Employment - Termination',
	@dcTcTermsTerminationDescription NVARCHAR(MAX) = '',
	@dcTcTermsTerminationText NVARCHAR(MAX) = 'Either party may terminate the employment relationship with the appropriate notice as prescribed in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  Notice provisions do not apply in the case of summary dismissal.
<br><br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee.
<br><br>
IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.',
	@dcTcTermsTerminationHeadline NVARCHAR(MAX) = 'Termination',
	@dcTcTermsTerminationSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsTerminationGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsTerminationName, 
		@dcTcTermsTerminationDescription,
		@dcTcTermsTerminationText, 
		@dcTcTermsTerminationHeadline,
		@dcTcTermsTerminationSortOrder,
		@dcTcTermsTerminationGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsTerminationName, 
		[Description] = @dcTcTermsTerminationDescription, 
		[Text] = @dcTcTermsTerminationText,
		[Headline] = @dcTcTermsTerminationHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsTerminationGuid
END

-- #################################### 25 Company Policies & Procedures 
DECLARE @dcTcTermsPoliciesGuid UNIQUEIDENTIFIER = 'a53f323b-545c-4fd7-b474-c2a933fc5243',
	@dcTcTermsPoliciesName NVARCHAR(MAX) = @prefix + ' Employment - Policies',
	@dcTcTermsPoliciesDescription NVARCHAR(MAX) = '',
	@dcTcTermsPoliciesText NVARCHAR(MAX) = 'You shall be required to comply with all IKEA Policies and Procedures as amended from time to time and as outlined in IKEA’s Policy Guidelines and Welcome Program. The IKEA Policies and Procedures form part of your contract of employment.',
	@dcTcTermsPoliciesHeadline NVARCHAR(MAX) = 'Company Policies & Procedures',
	@dcTcTermsPoliciesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsPoliciesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsPoliciesName, 
		@dcTcTermsPoliciesDescription,
		@dcTcTermsPoliciesText, 
		@dcTcTermsPoliciesHeadline,
		@dcTcTermsPoliciesSortOrder,
		@dcTcTermsPoliciesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsPoliciesName, 
		[Description] = @dcTcTermsPoliciesDescription, 
		[Text] = @dcTcTermsPoliciesText,
		[Headline] = @dcTcTermsPoliciesHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPoliciesGuid
END

-- #################################### 26 Other Terms and Conditions
DECLARE @dcTcTermsOtherTermsGuid UNIQUEIDENTIFIER = '4e4af9b2-68e2-4376-b269-c5018646511a',
	@dcTcTermsOtherTermsName NVARCHAR(MAX) = @prefix + ' Employment - Other T&C',
	@dcTcTermsOtherTermsDescription NVARCHAR(MAX) = '',
	@dcTcTermsOtherTermsText NVARCHAR(MAX) = 'The terms and conditions contained within the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016 (and any subsequent statutory agreement binding you and IKEA) also apply to your employment.  A copy of this Agreement is available for your perusal at all times.',
	@dcTcTermsOtherTermsHeadline NVARCHAR(MAX) = 'Other Terms and Conditions',
	@dcTcTermsOtherTermsSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsOtherTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsOtherTermsName, 
		@dcTcTermsOtherTermsDescription,
		@dcTcTermsOtherTermsText, 
		@dcTcTermsOtherTermsHeadline,
		@dcTcTermsOtherTermsSortOrder,
		@dcTcTermsOtherTermsGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsOtherTermsName, 
		[Description] = @dcTcTermsOtherTermsDescription, 
		[Text] = @dcTcTermsOtherTermsText,
		[Headline] = @dcTcTermsOtherTermsHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsOtherTermsGuid
END

-- #################################### 27 Police Checks
DECLARE @dcTcTermsPoliceGuid UNIQUEIDENTIFIER = 'b7d588ef-304c-43ac-ba82-cd0c8933bf9a',
	@dcTcTermsPoliceName NVARCHAR(MAX) = @prefix + ' Employment - Police',
	@dcTcTermsPoliceDescription NVARCHAR(MAX) = '',
	@dcTcTermsPoliceText NVARCHAR(MAX) = 'Some positions at IKEA require evidence of good character (for example - positions that deal with children or cash).  Obtaining details of your criminal history via a police check/s is an integral part of the assessment of your suitability for such positions.
<br><br>
You may be required to provide a police check/s at the time you are given this offer of employment.  Alternatively, you may be required to provide a police check/s during your employment (for instance, when it is suspected that you have incurred a criminal record since the commencement of your employment, or where you begin working in a position requiring evidence of good character).  By signing this offer of employment, you consent to complete, sign and lodge the relevant police check application documentation (which will be provided to you by IKEA), and to direct that the corresponding police check record/s be forwarded directly to IKEA (where permitted) or (otherwise) to provide IKEA with the original police check record/s immediately on receipt.
<br><br>
If you are required to provide the police check/s at the time you are given this offer of employment, you acknowledge that the offer of employment will be subject to the check being satisfactory to IKEA.
<br><br>
If you are required to provide the police check/s at any other time during your employment and the check is not satisfactory to IKEA, it may give grounds for dismissal.
<br><br>
Please note that the existence of a criminal record does not mean that the check will automatically be unsatisfactory, or that you will be assessed automatically as being unsuitable.  Each case will be assessed on its merits and will depend upon the inherent requirements of the position.',
	@dcTcTermsPoliceHeadline NVARCHAR(MAX) = 'Police Checks',
	@dcTcTermsPoliceSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsPoliceGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsPoliceName, 
		@dcTcTermsPoliceDescription,
		@dcTcTermsPoliceText, 
		@dcTcTermsPoliceHeadline,
		@dcTcTermsPoliceSortOrder,
		@dcTcTermsPoliceGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsPoliceName, 
		[Description] = @dcTcTermsPoliceDescription, 
		[Text] = @dcTcTermsPoliceText,
		[Headline] = @dcTcTermsPoliceHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPoliceGuid
END

-- #################################### 28 Performance Management
DECLARE @dcTcTermsPerfGuid UNIQUEIDENTIFIER = 'f262417d-f0a8-4b17-8ef8-2757166ed84f',
	@dcTcTermsPerfName NVARCHAR(MAX) = @prefix + ' Employment - Performance',
	@dcTcTermsPerfDescription NVARCHAR(MAX) = '',
	@dcTcTermsPerfText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November.  However, your first review will be conducted during your 6-month probationary period.  This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@dcTcTermsPerfHeadline NVARCHAR(MAX) = 'Performance Management',
	@dcTcTermsPerfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsPerfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsPerfName, 
		@dcTcTermsPerfDescription,
		@dcTcTermsPerfText, 
		@dcTcTermsPerfHeadline,
		@dcTcTermsPerfSortOrder,
		@dcTcTermsPerfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsPerfName, 
		[Description] = @dcTcTermsPerfDescription, 
		[Text] = @dcTcTermsPerfText,
		[Headline] = @dcTcTermsPerfHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsPerfGuid
END

-- #################################### 29 Communications with Media
DECLARE @dcTcTermsMediaGuid UNIQUEIDENTIFIER = '9b691976-4767-496f-b394-33ca4c0087b3',
	@dcTcTermsMediaName NVARCHAR(MAX) = @prefix + ' Employment - Media',
	@dcTcTermsMediaDescription NVARCHAR(MAX) = '',
	@dcTcTermsMediaText NVARCHAR(MAX) = 'You shall not provide information or speak on behalf of IKEA or otherwise to the media on any matters concerning IKEA’s business or activities.  You must refer all requests from the media for information and/or interviews to the DC Manager.',
	@dcTcTermsMediaHeadline NVARCHAR(MAX) = 'Communications with Media',
	@dcTcTermsMediaSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsMediaGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsMediaName, 
		@dcTcTermsMediaDescription,
		@dcTcTermsMediaText, 
		@dcTcTermsMediaHeadline,
		@dcTcTermsMediaSortOrder,
		@dcTcTermsMediaGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsMediaName, 
		[Description] = @dcTcTermsMediaDescription, 
		[Text] = @dcTcTermsMediaText,
		[Headline] = @dcTcTermsMediaHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsMediaGuid
END

-- #################################### 30 Obligation to report unlawful activities
DECLARE @dcTcTermsUnlawGuid UNIQUEIDENTIFIER = '58feef2e-241f-491e-b315-cecd82be4518',
	@dcTcTermsUnlawName NVARCHAR(MAX) = @prefix + ' Employment - Unlawful',
	@dcTcTermsUnlawDescription NVARCHAR(MAX) = '',
	@dcTcTermsUnlawText NVARCHAR(MAX) = 'If you become aware of or suspect any unlawful act or omission by any IKEA employee, you must advise IKEA immediately.',
	@dcTcTermsUnlawHeadline NVARCHAR(MAX) = 'Obligation to report unlawful activities',
	@dcTcTermsUnlawSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsUnlawGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsUnlawName, 
		@dcTcTermsUnlawDescription,
		@dcTcTermsUnlawText, 
		@dcTcTermsUnlawHeadline,
		@dcTcTermsUnlawSortOrder,
		@dcTcTermsUnlawGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsUnlawName, 
		[Description] = @dcTcTermsUnlawDescription, 
		[Text] = @dcTcTermsUnlawText,
		[Headline] = @dcTcTermsUnlawHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsUnlawGuid
END

-- #################################### 31 Intellectual Property
DECLARE @dcTcTermsIntelPropGuid UNIQUEIDENTIFIER = '1868d234-a7a5-42f1-af23-2a3a2a6300b0',
	@dcTcTermsIntelPropName NVARCHAR(MAX) = @prefix + ' Employment - Int. Property',
	@dcTcTermsIntelPropDescription NVARCHAR(MAX) = '',
	@dcTcTermsIntelPropText NVARCHAR(MAX) = 'IKEA owns all copyright in any works and all inventions, discoveries, novel designs, improvements or modifications, computer program material and trademarks which you write or develop in the course of your employment (in or out of working hours) (“Intellectual Property”).<br>
<br>
You assign to IKEA any interest you have in the Intellectual Property, and you must disclose any Intellectual Property to IKEA.<br>
<br>
During and after your employment, you must do anything IKEA reasonably requires (at IKEA''s cost) to:
<ul>
<li>obtain statutory protection (including by patent, design registration, trade mark registration or copyright) for the Intellectual Property for IKEA in any country; or</li>
<li>Perfect or evidence IKEA’s ownership of the Intellectual Property.</li>
</ul>',

	@dcTcTermsIntelPropHeadline NVARCHAR(MAX) = 'Intellectual Property',
	@dcTcTermsIntelPropSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsIntelPropGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsIntelPropName, 
		@dcTcTermsIntelPropDescription,
		@dcTcTermsIntelPropText, 
		@dcTcTermsIntelPropHeadline,
		@dcTcTermsIntelPropSortOrder,
		@dcTcTermsIntelPropGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsIntelPropName, 
		[Description] = @dcTcTermsIntelPropDescription, 
		[Text] = @dcTcTermsIntelPropText,
		[Headline] = @dcTcTermsIntelPropHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsIntelPropGuid
END

-- #################################### 32 Variation
DECLARE @dcTcTermsVarGuid UNIQUEIDENTIFIER = '0ea70ddd-53ee-4eec-aeab-7cf3821af6df',
	@dcTcTermsVarName NVARCHAR(MAX) = @prefix + ' Employment - Variation',
	@dcTcTermsVarDescription NVARCHAR(MAX) = '',
	@dcTcTermsVarText NVARCHAR(MAX) = 'This Agreement may only be varied by a written agreement signed by yourself and IKEA.',
	@dcTcTermsVarHeadline NVARCHAR(MAX) = 'Variation',
	@dcTcTermsVarSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsVarGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsVarName, 
		@dcTcTermsVarDescription,
		@dcTcTermsVarText, 
		@dcTcTermsVarHeadline,
		@dcTcTermsVarSortOrder,
		@dcTcTermsVarGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsVarName, 
		[Description] = @dcTcTermsVarDescription, 
		[Text] = @dcTcTermsVarText,
		[Headline] = @dcTcTermsVarHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsVarGuid
END

-- #################################### 33 Suspension
DECLARE @dcTcTermsSuspGuid UNIQUEIDENTIFIER = 'f0667d2f-d9c2-4e9f-a474-a112dd724a16',
	@dcTcTermsSuspName NVARCHAR(MAX) = @prefix + ' Employment - Suspension',
	@dcTcTermsSuspDescription NVARCHAR(MAX) = '',
	@dcTcTermsSuspText NVARCHAR(MAX) = 'If we have reason to believe that you may have engaged in a serious breach of your employment obligations, IKEA may at its discretion suspend you from your duties, either with or without pay, while and investigation is conducted.',
	@dcTcTermsSuspHeadline NVARCHAR(MAX) = 'Suspension',
	@dcTcTermsSuspSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcTermsSuspGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcTermsID, 
		@dcTcTermsSuspName, 
		@dcTcTermsSuspDescription,
		@dcTcTermsSuspText, 
		@dcTcTermsSuspHeadline,
		@dcTcTermsSuspSortOrder,
		@dcTcTermsSuspGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcTermsID,
		[Name] = @dcTcTermsSuspName, 
		[Description] = @dcTcTermsSuspDescription, 
		[Text] = @dcTcTermsSuspText,
		[Headline] = @dcTcTermsSuspHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcTermsSuspGuid
END

-- Add terms paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @dcTcTermsID, @counter
SET @counter = @counter + 1


-- #################################### 34-36 End Text
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcTcEndTextParagraphGuid UNIQUEIDENTIFIER = '2bc81adb-bee8-4ae8-a95b-8496d15af89d',
	@dcTcEndTextParagraphName NVARCHAR(MAX) = @prefix + ' Employment - End Text',
	@dcTcEndTextParagraphType INT = @ParagraphTypeText,
	@dcTcEndTextParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcTcEndTextParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcTcEndTextParagraphName, @dcTcEndTextParagraphDescription, @dcTcEndTextParagraphType, @dcTcEndTextParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcTcEndTextParagraphName, [Description] = @dcTcEndTextParagraphDescription, ParagraphType = @dcTcEndTextParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcTcEndTextParagraphGuid
END
DECLARE @dcTcEndTextParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcTcEndTextParagraphGuid)

-- Create a text field
DECLARE @dcTcEndTextGuid UNIQUEIDENTIFIER = 'da2d2c05-5c77-4aef-8821-148a8398c61a',
	@dcTcEndTextName NVARCHAR(MAX) = @prefix + ' Employment - End Text',
	@dcTcEndTextDescription NVARCHAR(MAX) = '',
	@dcTcEndTextText NVARCHAR(MAX) = 'IKEA recognises that its co-workers are essential to the success of the company’s operations.  IKEA remains committed to ensuring that all co-workers are treated fairly and equitably and encourages co-workers to reach their full potential.  We believe that the basis of your employment outlined above, will achieve these objectives and greatly benefit those co-workers willing to develop themselves.
	<br><br>
	As an indication of your understanding and acceptance of these conditions, please sign this letter of offer, and return to the undersigned within seven (7) days.  Please retain the second copy for your records.
	<br><br>
	If you have any questions pertaining to this offer of employment or any of the information contained herein, please do not hesitate to contact me before signing this letter.',
	@dcTcEndTextHeadline NVARCHAR(MAX) = '',
	@dcTcEndTextSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcEndTextGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcEndTextParagraphID, 
		@dcTcEndTextName, 
		@dcTcEndTextDescription,
		@dcTcEndTextText, 
		@dcTcEndTextHeadline,
		@dcTcEndTextSortOrder,
		@dcTcEndTextGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcEndTextParagraphID,
		[Name] = @dcTcEndTextName, 
		[Description] = @dcTcEndTextDescription, 
		[Text] = @dcTcEndTextText,
		[Headline] = @dcTcEndTextHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcEndTextGuid
END

-- Add end text paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @dcTcEndTextParagraphID, @counter
SET @counter = @counter + 1

-- #################################### 37-40 Contractor Signature
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcTcConSignParagraphGuid UNIQUEIDENTIFIER = '0ae9381f-c4e2-46c0-b64b-d8eac5b8cfb7',
	@dcTcConSignParagraphName NVARCHAR(MAX) = @prefix + ' Employment - Con. Sign.',
	@dcTcConSignParagraphType INT = @ParagraphTypeText,
	@dcTcConSignParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcTcConSignParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcTcConSignParagraphName, @dcTcConSignParagraphDescription, @dcTcConSignParagraphType, @dcTcConSignParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcTcConSignParagraphName, [Description] = @dcTcConSignParagraphDescription, ParagraphType = @dcTcConSignParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcTcConSignParagraphGuid
END
DECLARE @dcTcConSignParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcTcConSignParagraphGuid)

-- Create a text field
DECLARE @dcTcConSignGuid UNIQUEIDENTIFIER = '52d9e99f-a51f-4d02-9fac-c1523053ed64',
	@dcTcConSignName NVARCHAR(MAX) = @prefix + ' Employment - Con. Sign.',
	@dcTcConSignDescription NVARCHAR(MAX) = '',
	@dcTcConSignText NVARCHAR(MAX) = 'Yours sincerely<br>
	<Reports to Line Manager><br>
	<Position Title (Local Job Name)> of <Reports To Line Manager><br>
	<strong>IKEA Distribution Services Australia Pty Ltd</strong>',
	@dcTcConSignHeadline NVARCHAR(MAX) = '',
	@dcTcConSignSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcConSignGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcConSignParagraphID, 
		@dcTcConSignName, 
		@dcTcConSignDescription,
		@dcTcConSignText, 
		@dcTcConSignHeadline,
		@dcTcConSignSortOrder,
		@dcTcConSignGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcConSignParagraphID,
		[Name] = @dcTcConSignName, 
		[Description] = @dcTcConSignDescription, 
		[Text] = @dcTcConSignText,
		[Headline] = @dcTcConSignHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcConSignGuid
END

-- Add contractor signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @dcTcConSignParagraphID, @counter
SET @counter = @counter + 1

-- #################################### 41-48 Acceptance
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcTcAcceptParagraphGuid UNIQUEIDENTIFIER = '34779b4c-2a06-473f-912a-e03e367dbe99',
	@dcTcAcceptParagraphName NVARCHAR(MAX) = @prefix + ' Employment - Acceptance',
	@dcTcAcceptParagraphType INT = @ParagraphTypeText,
	@dcTcAcceptParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcTcAcceptParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcTcAcceptParagraphName, @dcTcAcceptParagraphDescription, @dcTcAcceptParagraphType, @dcTcAcceptParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcTcAcceptParagraphName, [Description] = @dcTcAcceptParagraphDescription, ParagraphType = @dcTcAcceptParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcTcAcceptParagraphGuid
END
DECLARE @dcTcAcceptParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcTcAcceptParagraphGuid)

-- Create a text field
DECLARE @dcTcAcceptGuid UNIQUEIDENTIFIER = 'dc225aae-e266-4621-b4ea-f23263027947',
	@dcTcAcceptName NVARCHAR(MAX) = @prefix + ' Employment - Acceptance',
	@dcTcAcceptDescription NVARCHAR(MAX) = '',
	@dcTcAcceptText NVARCHAR(MAX) = '<table style="border: 1px solid black">
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
	@dcTcAcceptHeadline NVARCHAR(MAX) = '',
	@dcTcAcceptSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcTcAcceptGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcTcAcceptParagraphID, 
		@dcTcAcceptName, 
		@dcTcAcceptDescription,
		@dcTcAcceptText, 
		@dcTcAcceptHeadline,
		@dcTcAcceptSortOrder,
		@dcTcAcceptGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcTcAcceptParagraphID,
		[Name] = @dcTcAcceptName, 
		[Description] = @dcTcAcceptDescription, 
		[Text] = @dcTcAcceptText,
		[Headline] = @dcTcAcceptHeadline
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcTcAcceptGuid
END

-- Add acceptance paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcTcID, @dcTcAcceptParagraphID, @counter
SET @counter = @counter + 1




-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @dcTcGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



COMMIT

