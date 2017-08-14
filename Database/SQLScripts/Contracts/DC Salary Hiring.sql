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
DECLARE @prefix NVARCHAR(MAX) = 'DC SAL HIR'

-- #################################### Contract Clusters – DC Salary Hiring ####################################

-- Get the form
DECLARE @dcSalHirGuid UNIQUEIDENTIFIER = 'D91F9293-C9C0-4F0F-88CA-AE30F04451CF'
DECLARE @dcSalHirID INT, @counter INT = 0
SELECT @dcSalHirID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcSalHirGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @dcSalHirGuid


-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @footerID, @counter
SET @counter = @counter + 1

-- #################################### 1. Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### 2-8. Address and company info
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @addressInfoID, @counter
SET @counter = @counter + 1

-- #################################### 10a-b. Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @dcSalHirEmployGreetingGuid UNIQUEIDENTIFIER = 'ED67B092-7BDA-42D1-93A8-74A6C65208CF',
	@cdpName NVARCHAR(MAX) = @prefix + ' Employment greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalHirEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @dcSalHirEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalHirEmployGreetingGuid
END
DECLARE @dcSalHirEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalHirEmployGreetingGuid)

---- Create or update text A, Full Time
DECLARE @dcSalHirEmployGreetingTextAGuid UNIQUEIDENTIFIER = '0C4283EA-FBF2-456C-9216-FB3ADD8D76EB',
	@dcSalHirEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Employment greeting, Text A - Full time',
	@dcSalHirEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@dcSalHirEmployGreetingTextAText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Full Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcSalHirEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@dcSalHirEmployGreetingTextASortOrder INT = 0


IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirEmployGreetingID, 
		@dcSalHirEmployGreetingTextAName, 
		@dcSalHirEmployGreetingTextADescription,
		@dcSalHirEmployGreetingTextAText, 
		@dcSalHirEmployGreetingTextAHeadline,
		@dcSalHirEmployGreetingTextASortOrder,
		@dcSalHirEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirEmployGreetingID,
		[Name] = @dcSalHirEmployGreetingTextAName, 
		[Description] = @dcSalHirEmployGreetingTextADescription, 
		[Text] = @dcSalHirEmployGreetingTextAText,
		[Headline] = @dcSalHirEmployGreetingTextAHeadline,
		SortOrder = @dcSalHirEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirEmployGreetingTextAGuid
END
DECLARE @dcSalHirEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @dcSalHirEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = 'D397463C-4492-4382-A7B3-9AA203C6E212',
	@dcSalHirEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalHirEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'Equal',
	@dcSalHirEmployGreetingTextACondAValues NVARCHAR(MAX) = '76',
	@dcSalHirEmployGreetingTextACondADescription NVARCHAR(MAX) = 'Is full time',
	@dcSalHirEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirEmployGreetingTextACondAGuid)
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
		@dcSalHirEmployGreetingTextACondAGuid,
		@dcSalHirEmployGreetingTextAID,
		@dcSalHirEmployGreetingTextACondAPropertyName,
		@dcSalHirEmployGreetingTextACondAOperator,
		@dcSalHirEmployGreetingTextACondAValues,
		@dcSalHirEmployGreetingTextACondADescription,
		@dcSalHirEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirEmployGreetingTextAID,
		Property_Name = @dcSalHirEmployGreetingTextACondAPropertyName,
		Operator = @dcSalHirEmployGreetingTextACondAOperator,
		[Values] = @dcSalHirEmployGreetingTextACondAValues,
		[Description] = @dcSalHirEmployGreetingTextACondADescription,
		[Status] = @dcSalHirEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @dcSalHirEmployGreetingTextBGuid UNIQUEIDENTIFIER = 'B3402432-AA9D-43BF-97D1-FFE53D38F627',
	@dcSalHirEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Employment greeting, Text B - Part time',
	@dcSalHirEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@dcSalHirEmployGreetingTextBText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Part Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcSalHirEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@dcSalHirEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirEmployGreetingID, 
		@dcSalHirEmployGreetingTextBName, 
		@dcSalHirEmployGreetingTextBDescription,
		@dcSalHirEmployGreetingTextBText, 
		@dcSalHirEmployGreetingTextBHeadline,
		@dcSalHirEmployGreetingTextBSortOrder,
		@dcSalHirEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirEmployGreetingID,
		[Name] = @dcSalHirEmployGreetingTextBName, 
		[Description] = @dcSalHirEmployGreetingTextBDescription, 
		[Text] = @dcSalHirEmployGreetingTextBText,
		[Headline] = @dcSalHirEmployGreetingTextBHeadline,
		SortOrder = @dcSalHirEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirEmployGreetingTextBGuid
END
DECLARE @dcSalHirEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @dcSalHirEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = '45DDF8BD-FC24-4DFA-AE8B-16A0A5ADB92B',
	@dcSalHirEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalHirEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@dcSalHirEmployGreetingTextBCondAValues NVARCHAR(MAX) = '76',
	@dcSalHirEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Is part time',
	@dcSalHirEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirEmployGreetingTextBCondAGuid)
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
		@dcSalHirEmployGreetingTextBCondAGuid,
		@dcSalHirEmployGreetingTextBID,
		@dcSalHirEmployGreetingTextBCondAPropertyName,
		@dcSalHirEmployGreetingTextBCondAOperator,
		@dcSalHirEmployGreetingTextBCondAValues,
		@dcSalHirEmployGreetingTextBCondADescription,
		@dcSalHirEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirEmployGreetingTextBID,
		Property_Name = @dcSalHirEmployGreetingTextBCondAPropertyName,
		Operator = @dcSalHirEmployGreetingTextBCondAOperator,
		[Values] = @dcSalHirEmployGreetingTextBCondAValues,
		[Description] = @dcSalHirEmployGreetingTextBCondADescription,
		[Status] = @dcSalHirEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirEmployGreetingTextBCondAGuid
END


-- Add paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @dcSalHirEmployGreetingID, @counter
SET @counter = @counter + 1

-- #################################### 11a-33 Terms

DECLARE @termsCounter INT = 0
---- Create or update a terms paragraph
-- Paragraph guid
DECLARE @dcSalHirTermsGuid UNIQUEIDENTIFIER = '1180154C-361F-4D09-85F1-7F856608505B',
	@dcSalHirTermsName NVARCHAR(MAX) = @prefix + ' Terms',
	@dcSalHirTermsParagraphType INT = @ParagraphTypeTableNumeric,
	@termsDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalHirTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalHirTermsName, @termsDescription, @dcSalHirTermsParagraphType, @dcSalHirTermsGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalHirTermsName, [Description] = @termsDescription, ParagraphType = @dcSalHirTermsParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalHirTermsGuid
END
DECLARE @dcSalHirTermsID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalHirTermsGuid)

-- #################################### 11a-b Position

---- Position A
DECLARE @dcSalHirTermsPositionAGuid UNIQUEIDENTIFIER = '5F717D6F-2F83-4E66-AB5D-EEE48F5FD742',
	@dcSalHirTermsPositionAName NVARCHAR(MAX) = @prefix + ' Employment - Position - Full time',
	@dcSalHirTermsPositionADescription NVARCHAR(MAX) = '',
	@dcSalHirTermsPositionAText NVARCHAR(MAX) = 'Your position is Full Time <Position Title (Local Job Name)>, located at <Business Unit>, reporting to the <Position Title (Local Job Name)> of <Reports To Line Manager>. Your position (in terms of your duties and responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcSalHirTermsPositionAHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcSalHirTermsPositionASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsPositionAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsPositionAName, 
		@dcSalHirTermsPositionADescription,
		@dcSalHirTermsPositionAText, 
		@dcSalHirTermsPositionAHeadline,
		@dcSalHirTermsPositionASortOrder,
		@dcSalHirTermsPositionAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsPositionAName, 
		[Description] = @dcSalHirTermsPositionADescription, 
		[Text] = @dcSalHirTermsPositionAText,
		[Headline] = @dcSalHirTermsPositionAHeadline,
		SortOrder = @dcSalHirTermsPositionASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPositionAGuid
END
DECLARE @dcSalHirTermsPositionAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPositionAGuid)

-- Create condition for position A
DECLARE @dcSalHirTermsPositionACondGuid UNIQUEIDENTIFIER = '5EC4CFBB-8D8A-4604-933B-6D85C30A5B4E',
	@dcSalHirTermsPositionACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalHirTermsPositionACondOperator NVARCHAR(MAX) = 'Equal',
	@dcSalHirTermsPositionACondValues NVARCHAR(MAX) = '76',
	@dcSalHirTermsPositionACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcSalHirTermsPositionACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsPositionACondGuid)
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
		@dcSalHirTermsPositionACondGuid,
		@dcSalHirTermsPositionAID,
		@dcSalHirTermsPositionACondPropertyName,
		@dcSalHirTermsPositionACondOperator,
		@dcSalHirTermsPositionACondValues,
		@dcSalHirTermsPositionACondDescription,
		@dcSalHirTermsPositionACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsPositionAID,
		Property_Name = @dcSalHirTermsPositionACondPropertyName,
		Operator = @dcSalHirTermsPositionACondOperator,
		[Values] = @dcSalHirTermsPositionACondValues,
		[Description] = @dcSalHirTermsPositionACondDescription,
		[Status] = @dcSalHirTermsPositionACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsPositionACondGuid
END

---- Position B
DECLARE @dcSalHirTermsPositionBGuid UNIQUEIDENTIFIER = 'E11537F9-D94B-41BA-95F3-C99C7E4138AF',
	@dcSalHirTermsPositionBName NVARCHAR(MAX) = @prefix + ' Employment - Position - Part time',
	@dcSalHirTermsPositionBDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsPositionBText NVARCHAR(MAX) = 'Your position is Part Time <Position Title (Local Job Name)> <Shift Type> Shift, reporting to  <Position Title (Local Job Name)> of <Reports To Line Manager>, which will be based at <Business Unit>. Your position (in terms of your duties & responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcSalHirTermsPositionBHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcSalHirTermsPositionBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsPositionBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsPositionBName, 
		@dcSalHirTermsPositionBDescription,
		@dcSalHirTermsPositionBText, 
		@dcSalHirTermsPositionBHeadline,
		@dcSalHirTermsPositionBSortOrder,
		@dcSalHirTermsPositionBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsPositionBName, 
		[Description] = @dcSalHirTermsPositionBDescription, 
		[Text] = @dcSalHirTermsPositionBText,
		[Headline] = @dcSalHirTermsPositionBHeadline,
		SortOrder = @dcSalHirTermsPositionBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPositionBGuid
END
DECLARE @dcSalHirTermsPositionBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPositionBGuid)

-- Create condition for position A
DECLARE @dcSalHirTermsPositionBCondGuid UNIQUEIDENTIFIER = 'E129AEE4-1F0C-4DFD-8C62-AA28FA4C3EC9',
	@dcSalHirTermsPositionBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalHirTermsPositionBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcSalHirTermsPositionBCondValues NVARCHAR(MAX) = '76',
	@dcSalHirTermsPositionBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcSalHirTermsPositionBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsPositionBCondGuid)
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
		@dcSalHirTermsPositionBCondGuid,
		@dcSalHirTermsPositionBID,
		@dcSalHirTermsPositionBCondPropertyName,
		@dcSalHirTermsPositionBCondOperator,
		@dcSalHirTermsPositionBCondValues,
		@dcSalHirTermsPositionBCondDescription,
		@dcSalHirTermsPositionBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsPositionBID,
		Property_Name = @dcSalHirTermsPositionBCondPropertyName,
		Operator = @dcSalHirTermsPositionBCondOperator,
		[Values] = @dcSalHirTermsPositionBCondValues,
		[Description] = @dcSalHirTermsPositionBCondDescription,
		[Status] = @dcSalHirTermsPositionBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsPositionBCondGuid
END

-- #################################### 12a-b Commencement Date

---- Commencement A
DECLARE @dcSalHirTermsComAGuid UNIQUEIDENTIFIER = '31D29CB8-6E01-4523-B125-B6DD41165CD9',
	@dcSalHirTermsComAName NVARCHAR(MAX) = @prefix + ' Employment - Commencement - No date',
	@dcSalHirTermsComADescription NVARCHAR(MAX) = '',
	@dcSalHirTermsComAText NVARCHAR(MAX) = 'Your commencement date is <Change Valid From>, unless otherwise terminated in accordance with this contract.',
	@dcSalHirTermsComAHeadline NVARCHAR(MAX) = 'Commencement Date',
	@dcSalHirTermsComASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsComAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsComAName, 
		@dcSalHirTermsComADescription,
		@dcSalHirTermsComAText, 
		@dcSalHirTermsComAHeadline,
		@dcSalHirTermsComASortOrder,
		@dcSalHirTermsComAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsComAName, 
		[Description] = @dcSalHirTermsComADescription, 
		[Text] = @dcSalHirTermsComAText,
		[Headline] = @dcSalHirTermsComAHeadline,
		SortOrder = @dcSalHirTermsComASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsComAGuid
END
DECLARE @dcSalHirTermsComAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsComAGuid)

-- Create condition for Commencement A
DECLARE @dcSalHirTermsComACondAGuid UNIQUEIDENTIFIER = '3E9337BF-859B-4B20-87B3-D07A38BF2AFB',
	@dcSalHirTermsComACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@dcSalHirTermsComACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@dcSalHirTermsComACondAValues NVARCHAR(MAX) = '',
	@dcSalHirTermsComACondADescription NVARCHAR(MAX) = 'Has no end date',
	@dcSalHirTermsComACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsComACondAGuid)
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
		@dcSalHirTermsComACondAGuid,
		@dcSalHirTermsComAID,
		@dcSalHirTermsComACondAPropertyName,
		@dcSalHirTermsComACondAOperator,
		@dcSalHirTermsComACondAValues,
		@dcSalHirTermsComACondADescription,
		@dcSalHirTermsComACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsComAID,
		Property_Name = @dcSalHirTermsComACondAPropertyName,
		Operator = @dcSalHirTermsComACondAOperator,
		[Values] = @dcSalHirTermsComACondAValues,
		[Description] = @dcSalHirTermsComACondADescription,
		[Status] = @dcSalHirTermsComACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsComACondAGuid
END

-- No support for 31.12.9999 yet
--DECLARE @dcSalHirTermsComACondBGuid UNIQUEIDENTIFIER = 'e68e3e7c-52b0-4018-964b-99a1d9d471b9',
--	@dcSalHirTermsComACondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
--	@dcSalHirTermsComACondBOperator NVARCHAR(MAX) = 'Equal',
--	@dcSalHirTermsComACondBValues NVARCHAR(MAX) = '31.12.9999',
--	@dcSalHirTermsComACondBDescription NVARCHAR(MAX) = 'Equal 31.12.9999',
--	@dcSalHirTermsComACondBStatus INT = 1

--IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsComACondBGuid)
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
--		@dcSalHirTermsComACondBGuid,
--		@dcSalHirTermsComAID,
--		@dcSalHirTermsComACondBPropertyName,
--		@dcSalHirTermsComACondBOperator,
--		@dcSalHirTermsComACondBValues,
--		@dcSalHirTermsComACondBDescription,
--		@dcSalHirTermsComACondBStatus,
--		@now, 
--		@userID,
--		@now,
--		@userID
--	)
--END
--ELSE
--BEGIN
--	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsComAID,
--		Property_Name = @dcSalHirTermsComACondBPropertyName,
--		Operator = @dcSalHirTermsComACondBOperator,
--		[Values] = @dcSalHirTermsComACondBValues,
--		[Description] = @dcSalHirTermsComACondBDescription,
--		[Status] = @dcSalHirTermsComACondBStatus,
--		CreatedDate = @now,
--		CreatedByUser_Id = @userID,
--		ChangedDate = @now,
--		ChangedByUser_Id = @userID
--	FROM tblCaseDocumentTextCondition CDTC
--	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsComACondBGuid
--END

---- Commencement B
DECLARE @dcSalHirTermsComBGuid UNIQUEIDENTIFIER = 'C8A45005-DB67-456C-B8D0-31976EE668C9',
	@dcSalHirTermsComBName NVARCHAR(MAX) = @prefix + ' - Commencement - Has end date',
	@dcSalHirTermsComBDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsComBText NVARCHAR(MAX) = 'Your commencement date is <Change Valid From> and will cease on <Change Valid To>, unless otherwise terminated in accordance with this contract.',
	@dcSalHirTermsComBHeadline NVARCHAR(MAX) = 'Commencement date',
	@dcSalHirTermsComBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsComBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsComBName, 
		@dcSalHirTermsComBDescription,
		@dcSalHirTermsComBText, 
		@dcSalHirTermsComBHeadline,
		@dcSalHirTermsComBSortOrder,
		@dcSalHirTermsComBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsComBName, 
		[Description] = @dcSalHirTermsComBDescription, 
		[Text] = @dcSalHirTermsComBText,
		[Headline] = @dcSalHirTermsComBHeadline,
		SortOrder = @dcSalHirTermsComBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsComBGuid
END
DECLARE @dcSalHirTermsComBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsComBGuid)

-- Create condition for Commence B
DECLARE @dcSalHirTermsComBCondAGuid UNIQUEIDENTIFIER = 'D71A75F7-1479-4992-9B98-AC8787A44EEA',
	@dcSalHirTermsComBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@dcSalHirTermsComBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@dcSalHirTermsComBCondAValues NVARCHAR(MAX) = '',
	@dcSalHirTermsComBCondADescription NVARCHAR(MAX) = 'Has end date',
	@dcSalHirTermsComBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsComBCondAGuid)
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
		@dcSalHirTermsComBCondAGuid,
		@dcSalHirTermsComBID,
		@dcSalHirTermsComBCondAPropertyName,
		@dcSalHirTermsComBCondAOperator,
		@dcSalHirTermsComBCondAValues,
		@dcSalHirTermsComBCondADescription,
		@dcSalHirTermsComBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsComBID,
		Property_Name = @dcSalHirTermsComBCondAPropertyName,
		Operator = @dcSalHirTermsComBCondAOperator,
		[Values] = @dcSalHirTermsComBCondAValues,
		[Description] = @dcSalHirTermsComBCondADescription,
		[Status] = @dcSalHirTermsComBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsComBCondAGuid
END

-- No support for 31.12.9999 yet
/*DECLARE @dcSalHirTermsComBCondBGuid UNIQUEIDENTIFIER = '43c72a21-96c1-4d3c-a44a-5279593332c7',
	@dcSalHirTermsComBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcSalHirTermsComBCondBOperator NVARCHAR(MAX) = 'NotEqual',
	@dcSalHirTermsComBCondBValues NVARCHAR(MAX) = '31.12.9999',
	@dcSalHirTermsComBCondBDescription NVARCHAR(MAX) = 'Not equals 31.12.9999',
	@dcSalHirTermsComBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsComBCondBGuid)
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
		@dcSalHirTermsComBCondBGuid,
		@dcSalHirTermsComBID,
		@dcSalHirTermsComBCondBPropertyName,
		@dcSalHirTermsComBCondBOperator,
		@dcSalHirTermsComBCondBValues,
		@dcSalHirTermsComBCondBDescription,
		@dcSalHirTermsComBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsComBID,
		Property_Name = @dcSalHirTermsComBCondBPropertyName,
		Operator = @dcSalHirTermsComBCondBOperator,
		[Values] = @dcSalHirTermsComBCondBValues,
		[Description] = @dcSalHirTermsComBCondBDescription,
		[Status] = @dcSalHirTermsComBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsComBCondBGuid
END*/

-- #################################### 13 Remuneration
DECLARE @dcSalHirTermsRemunGuid UNIQUEIDENTIFIER = '4AF86F83-273C-44D7-A081-2894032D8C8A',
	@dcSalHirTermsRemunName NVARCHAR(MAX) = @prefix + ' Employment - Remuneration',
	@dcSalHirTermsRemunDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsRemunText NVARCHAR(MAX) = 'Upon commencement, your base hourly rate will be as per the <b>IKEA Distributions Services Australia Pty Ltd Enterprise Agreement 2016</b>.  This amount will be paid directly into your nominated bank account on a fortnightly basis.',
	@dcSalHirTermsRemunHeadline NVARCHAR(MAX) = 'Remuneration',
	@dcSalHirTermsRemunSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsRemunGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsRemunName, 
		@dcSalHirTermsRemunDescription,
		@dcSalHirTermsRemunText, 
		@dcSalHirTermsRemunHeadline,
		@dcSalHirTermsRemunSortOrder,
		@dcSalHirTermsRemunGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsRemunName, 
		[Description] = @dcSalHirTermsRemunDescription, 
		[Text] = @dcSalHirTermsRemunText,
		[Headline] = @dcSalHirTermsRemunHeadline,
		SortOrder = @dcSalHirTermsRemunSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsRemunGuid
END

-- #################################### 16 Superannuation
DECLARE @dcSalHirTermsSuperGuid UNIQUEIDENTIFIER = '79D00B1E-741B-4FF5-8D5D-2F3FFF8D9093',
	@dcSalHirTermsSuperName NVARCHAR(MAX) = @prefix + ' Employment - Superannuation',
	@dcSalHirTermsSuperDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsSuperText NVARCHAR(MAX) = 'IKEA will make superannuation contributions, on your behalf, to a government approved Superannuation Fund, at the rate payable under the Superannuation Guarantee Legislation (SGL). This rate is currently 9.5% of your wage.
<br>
IKEA’s current employer superannuation fund is the Labour Union Co-operative Retirement Fund (LUCRF), which is the fund into which the superannuation contributions will be made unless an alternate fund is nominated by you in writing, in accordance with the SGL.
<br>
It is your responsibility to nominate a Super Fund for your contributions to be made to, and to ensure that you complete the necessary paperwork for enrolment into your nominated fund.  IKEA will supply you with a LUCRF Member Guide, including an application form.',
	@dcSalHirTermsSuperHeadline NVARCHAR(MAX) = 'Superannuation',
	@dcSalHirTermsSuperSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsSuperGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsSuperName, 
		@dcSalHirTermsSuperDescription,
		@dcSalHirTermsSuperText, 
		@dcSalHirTermsSuperHeadline,
		@dcSalHirTermsSuperSortOrder,
		@dcSalHirTermsSuperGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsSuperName, 
		[Description] = @dcSalHirTermsSuperDescription, 
		[Text] = @dcSalHirTermsSuperText,
		[Headline] = @dcSalHirTermsSuperHeadline,
		SortOrder = @dcSalHirTermsSuperSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsSuperGuid
END

-- #################################### 13a-b Hours of Work

---- Hours of Work A
DECLARE @dcSalHirTermsHWAGuid UNIQUEIDENTIFIER = '76C14D99-C2E0-4853-A27D-6DCBB15336AE',
	@dcSalHirTermsHWAName NVARCHAR(MAX) = @prefix + ' - Hours of Work - Full time',
	@dcSalHirTermsHWADescription NVARCHAR(MAX) = '',
	@dcSalHirTermsHWAText NVARCHAR(MAX) = 
	'You will be rostered to work 76 ordinary hours per fortnight.  Such details of your initial roster will be discussed with you upon your commencement.  However, where there is a change in the business’ needs, your hours may also be subject to change with appropriate notice.
You should note that ordinary hours in the Distribution Centre include Saturday’s and you have mutually agreed to work more than one in three Saturdays as part of your contracted ordinary hours.',
	@dcSalHirTermsHWAHeadline NVARCHAR(MAX) = 'Position',
	@dcSalHirTermsHWASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsHWAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsHWAName, 
		@dcSalHirTermsHWADescription,
		@dcSalHirTermsHWAText, 
		@dcSalHirTermsHWAHeadline,
		@dcSalHirTermsHWASortOrder,
		@dcSalHirTermsHWAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsHWAName, 
		[Description] = @dcSalHirTermsHWADescription, 
		[Text] = @dcSalHirTermsHWAText,
		[Headline] = @dcSalHirTermsHWAHeadline,
		SortOrder = @dcSalHirTermsHWASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsHWAGuid
END
DECLARE @dcSalHirTermsHWAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsHWAGuid)


-- #################################### 14 Probationary Period

---- Probationary Period
DECLARE @dcSalHirTermsProbTimeGuid UNIQUEIDENTIFIER = 'C7664560-0FAB-4D77-804B-F76471D7D9BA',
	@dcSalHirTermsProbTimeName NVARCHAR(MAX) = @prefix + ' Employment - Probationary Period',
	@dcSalHirTermsProbTimeDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsProbTimeText NVARCHAR(MAX) = 'IKEA offers this employment to you on a probationary basis for a period of six (6) months, during which time your performance standards will be subject to regular review and assessment.  In the six (6)-month period, if either you or IKEA wishes to terminate the employment relationship, then either party can effect that termination with one week’s notice in writing.',
	@dcSalHirTermsProbTimeHeadline NVARCHAR(MAX) = 'Probationary Period',
	@dcSalHirTermsProbTimeSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsProbTimeGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsProbTimeName, 
		@dcSalHirTermsProbTimeDescription,
		@dcSalHirTermsProbTimeText, 
		@dcSalHirTermsProbTimeHeadline,
		@dcSalHirTermsProbTimeSortOrder,
		@dcSalHirTermsProbTimeGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsProbTimeName, 
		[Description] = @dcSalHirTermsProbTimeDescription, 
		[Text] = @dcSalHirTermsProbTimeText,
		[Headline] = @dcSalHirTermsProbTimeHeadline,
		SortOrder = @dcSalHirTermsProbTimeSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsProbTimeGuid
END

DECLARE @dcSalHirTermsProbID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsProbTimeGuid)

-- #################################### 28 Performance Management
DECLARE @dcSalHirTermsPerfAGuid UNIQUEIDENTIFIER = '7673FB67-8DDF-40BE-B999-D4C0804FEC7B',
	@dcSalHirTermsPerfAName NVARCHAR(MAX) = @prefix + ' Employment - Performance',
	@dcSalHirTermsPerfADescription NVARCHAR(MAX) = '',
	@dcSalHirTermsPerfAText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November.  However, your first review will be conducted during your 6-month probationary period.  This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@dcSalHirTermsPerfAHeadline NVARCHAR(MAX) = 'Performance Management',
	@dcSalHirTermsPerfASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsPerfAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsPerfAName, 
		@dcSalHirTermsPerfADescription,
		@dcSalHirTermsPerfAText, 
		@dcSalHirTermsPerfAHeadline,
		@dcSalHirTermsPerfASortOrder,
		@dcSalHirTermsPerfAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsPerfAName, 
		[Description] = @dcSalHirTermsPerfADescription, 
		[Text] = @dcSalHirTermsPerfAText,
		[Headline] = @dcSalHirTermsPerfAHeadline,
		SortoRder = @dcSalHirTermsPerfASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPerfAGuid
END


-- #################################### Remuneration Review

DECLARE @dcSalHirTermsRemunRevAGuid UNIQUEIDENTIFIER = '0259B029-DB44-46F8-83C0-8C24C6D1A5A9',
	@dcSalHirTermsRemunRevAName NVARCHAR(MAX) = @prefix + ' Remuneration Review',
	@dcSalHirTermsRemunRevADescription NVARCHAR(MAX) = '',
	@dcSalHirTermsRemunRevAText NVARCHAR(MAX) = 'In line with IKEA’s Remuneration Policy, your Total Remuneration package will be reviewed annually following your performance review.  Any increase in your total remuneration package will take effect from the next pay cycle.<br>
	<br>
The earliest your Total Remuneration package will be reviewed will be in January <Next Salary Review Year>.',
	@dcSalHirTermsRemunRevAHeadline NVARCHAR(MAX) = 'Remuneration Review',
	@dcSalHirTermsRemunRevASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsRemunRevAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsRemunRevAName, 
		@dcSalHirTermsRemunRevADescription,
		@dcSalHirTermsRemunRevAText, 
		@dcSalHirTermsRemunRevAHeadline,
		@dcSalHirTermsRemunRevASortOrder,
		@dcSalHirTermsRemunRevAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsRemunRevAName, 
		[Description] = @dcSalHirTermsRemunRevADescription, 
		[Text] = @dcSalHirTermsRemunRevAText,
		[Headline] = @dcSalHirTermsRemunRevAHeadline,
		SortoRder = @dcSalHirTermsRemunRevASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsRemunRevAGuid
END

DECLARE @dcSalHirTermsRemunRevAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsRemunRevAGuid)
-- #################################### 17 Confidential Information
DECLARE @dcSalHirTermsConfGuid UNIQUEIDENTIFIER = '43928C15-8F1B-443B-98CD-E78E2241E2BF',
	@dcSalHirTermsConfName NVARCHAR(MAX) = @prefix + ' Employment - Confidential Information',
	@dcSalHirTermsConfDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsConfText NVARCHAR(MAX) = '',
	@dcSalHirTermsConfHeadline NVARCHAR(MAX) = 'Confidential Information',
	@dcSalHirTermsConfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsConfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsConfName, 
		@dcSalHirTermsConfDescription,
		@dcSalHirTermsConfText, 
		@dcSalHirTermsConfHeadline,
		@dcSalHirTermsConfSortOrder,
		@dcSalHirTermsConfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsConfName, 
		[Description] = @dcSalHirTermsConfDescription, 
		[Text] = @dcSalHirTermsConfText,
		[Headline] = @dcSalHirTermsConfHeadline,
		SortOrder = @dcSalHirTermsConfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsConfGuid
END

-- #################################### 18a-b Leave Entitlements 

---- Leave Entitlements
DECLARE @dcSalHirTermsLeaveAGuid UNIQUEIDENTIFIER = 'C8D02802-F6D3-43EB-A05B-74CB01ED0412',
	@dcSalHirTermsLeaveAName NVARCHAR(MAX) = @prefix + ' Employment - Leave - Full time',
	@dcSalHirTermsLeaveADescription NVARCHAR(MAX) = '',
	@dcSalHirTermsLeaveAText NVARCHAR(MAX) = 'You will accrue entitlements to leave in accordance with relevant legislation and company policy.  This currently includes annual leave (4 weeks per annum, excluding annual leave loading), personal leave (10 days per annum) to be used for absence due to personal illness or to care for a member of your immediate family, parental leave and long service leave.  Company policy may change at any time at IKEA’s sole discretion.<br>
	<br>
Annual leave is ordinarily to be taken within the year it is accrued, or within 12 months from the date it becomes due.  Annual leave is to be taken at times mutually agreed to, taking into consideration peak periods in business operations, which may vary from year to year.  Peak periods may be such that no annual leave will be authorised during those periods.  Peak periods can be identified in consultation with your manager.',
	@dcSalHirTermsLeaveAHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@dcSalHirTermsLeaveASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsLeaveAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsLeaveAName, 
		@dcSalHirTermsLeaveADescription,
		@dcSalHirTermsLeaveAText, 
		@dcSalHirTermsLeaveAHeadline,
		@dcSalHirTermsLeaveASortOrder,
		@dcSalHirTermsLeaveAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsLeaveAName, 
		[Description] = @dcSalHirTermsLeaveADescription, 
		[Text] = @dcSalHirTermsLeaveAText,
		[Headline] = @dcSalHirTermsLeaveAHeadline,
		SortOrder = @dcSalHirTermsLeaveASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsLeaveAGuid
END 

DECLARE @dcSalHirTermsLeaveAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsLeaveAGuid)

-- #################################### 19 Issues Resolution
DECLARE @dcSalHirTermsIssuesGuid UNIQUEIDENTIFIER = '93D2EA1A-A8A0-4A8E-A386-63354E414AA4',
	@dcSalHirTermsIssuesName NVARCHAR(MAX) = @prefix + ' Employment - Issues Resolution',
	@dcSalHirTermsIssuesDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsIssuesText NVARCHAR(MAX) = 'If any issues arise during your employment with IKEA, the matter should initially be discussed with your immediate manager, in accordance with IKEA’s Issue Resolution Procedure.  If the problem remains unresolved, you may refer it to more senior levels of management for further discussion in accordance with the Issue Resolution Procedure and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.',
	@dcSalHirTermsIssuesHeadline NVARCHAR(MAX) = 'Issues Resolution',
	@dcSalHirTermsIssuesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsIssuesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsIssuesName, 
		@dcSalHirTermsIssuesDescription,
		@dcSalHirTermsIssuesText, 
		@dcSalHirTermsIssuesHeadline,
		@dcSalHirTermsIssuesSortOrder,
		@dcSalHirTermsIssuesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsIssuesName, 
		[Description] = @dcSalHirTermsIssuesDescription, 
		[Text] = @dcSalHirTermsIssuesText,
		[Headline] = @dcSalHirTermsIssuesHeadline,
		SortOrder = @dcSalHirTermsIssuesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsIssuesGuid
END

-- #################################### 20 Equal Employment Opportunity 
DECLARE @dcSalHirTermsEqualGuid UNIQUEIDENTIFIER = '22E63997-2EE8-417F-B2B2-AC213D2EC2F7',
	@dcSalHirTermsEqualName NVARCHAR(MAX) = @prefix + ' Employment - Equal Employment',
	@dcSalHirTermsEqualDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsEqualText NVARCHAR(MAX) = 'IKEA''s policy is to provide all co-workers with equal opportunity.  This policy precludes discrimination and harassment based on, but not limited to, race, colour, religion, gender, age, marital status and disability.  You are required to familiarise yourself with this policy and comply with it at all times.',
	@dcSalHirTermsEqualHeadline NVARCHAR(MAX) = 'Equal Employment Opportunity ',
	@dcSalHirTermsEqualSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsEqualGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsEqualName, 
		@dcSalHirTermsEqualDescription,
		@dcSalHirTermsEqualText, 
		@dcSalHirTermsEqualHeadline,
		@dcSalHirTermsEqualSortOrder,
		@dcSalHirTermsEqualGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsEqualName, 
		[Description] = @dcSalHirTermsEqualDescription, 
		[Text] = @dcSalHirTermsEqualText,
		[Headline] = @dcSalHirTermsEqualHeadline,
		SortOrder = @dcSalHirTermsEqualSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsEqualGuid
END

-- #################################### 21 Appearance & Conduct
DECLARE @dcSalHirTermsUniformGuid UNIQUEIDENTIFIER = '675E5D06-86CA-45EC-AA36-61EC5971CA16',
	@dcSalHirTermsUniformName NVARCHAR(MAX) = @prefix + ' Terms - Appearance & Conduct',
	@dcSalHirTermsUniformDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsUniformText NVARCHAR(MAX) = 'IKEA has established guidelines necessary for the professional appearance that the company expects all co-workers to present, and as such co-workers are supplied with uniforms and name badges within these guidelines.<br>
	<br>
Co-workers are expected to project a favorable and professional image for IKEA, and are to be courteous, efficient and reliable in their dealings with colleagues, existing and potential customers and suppliers to IKEA.',
	@dcSalHirTermsUniformHeadline NVARCHAR(MAX) = 'Appearance & Conduct',
	@dcSalHirTermsUniformSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsUniformGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsUniformName, 
		@dcSalHirTermsUniformDescription,
		@dcSalHirTermsUniformText, 
		@dcSalHirTermsUniformHeadline,
		@dcSalHirTermsUniformSortOrder,
		@dcSalHirTermsUniformGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsUniformName, 
		[Description] = @dcSalHirTermsUniformDescription, 
		[Text] = @dcSalHirTermsUniformText,
		[Headline] = @dcSalHirTermsUniformHeadline,
		SortOrder = @dcSalHirTermsUniformSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsUniformGuid
END

-- #################################### 22 Induction & Ongoing Learning & Development
DECLARE @dcSalHirTermsInductionGuid UNIQUEIDENTIFIER = '68D3B896-744D-4D17-933B-CE309154AC0A',
	@dcSalHirTermsInductionName NVARCHAR(MAX) = @prefix + ' Employment - Induction',
	@dcSalHirTermsInductionDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsInductionText NVARCHAR(MAX) = 'IKEA is committed to your induction and ongoing development and as such, has a requirement that you undertake and are committed to training as offered by the company.  Whilst the majority of training is conducted on the job, you may be required from time to time to attend external training programs at different locations as organised by IKEA.
<br><br>
IKEA also encourages its co-workers to take responsibility for their own learning and development.',
	@dcSalHirTermsInductionHeadline NVARCHAR(MAX) = 'Induction & Ongoing Learning & Development',
	@dcSalHirTermsInductionSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsInductionGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsInductionName, 
		@dcSalHirTermsInductionDescription,
		@dcSalHirTermsInductionText, 
		@dcSalHirTermsInductionHeadline,
		@dcSalHirTermsInductionSortOrder,
		@dcSalHirTermsInductionGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsInductionName, 
		[Description] = @dcSalHirTermsInductionDescription, 
		[Text] = @dcSalHirTermsInductionText,
		[Headline] = @dcSalHirTermsInductionHeadline,
		SortOrder = @dcSalHirTermsInductionSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsInductionGuid
END

-- #################################### 23 Occupational Health & Safety
DECLARE @dcSalHirTermsSafetyGuid UNIQUEIDENTIFIER = '7E6D73FD-80DA-45A3-B367-6BA89C764E56',
	@dcSalHirTermsSafetyName NVARCHAR(MAX) = @prefix + ' Employment - Safety',
	@dcSalHirTermsSafetyDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsSafetyText NVARCHAR(MAX) = 'IKEA understands the requirement of ensuring a safe and healthy working environment for all co-workers in its offices, warehouses and stores, and a safe and healthy shopping environment for customers.  In fulfilling this aim, we undertake regular consultation with co-workers on health and safety issues and concerns.',
	@dcSalHirTermsSafetyHeadline NVARCHAR(MAX) = 'Occupational Health & Safety',
	@dcSalHirTermsSafetySortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsSafetyGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsSafetyName, 
		@dcSalHirTermsSafetyDescription,
		@dcSalHirTermsSafetyText, 
		@dcSalHirTermsSafetyHeadline,
		@dcSalHirTermsSafetySortOrder,
		@dcSalHirTermsSafetyGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsSafetyName, 
		[Description] = @dcSalHirTermsSafetyDescription, 
		[Text] = @dcSalHirTermsSafetyText,
		[Headline] = @dcSalHirTermsSafetyHeadline,
		SortOrder = @dcSalHirTermsSafetySortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsSafetyGuid
END

-- #################################### 24 Termination
DECLARE @dcSalHirTermsTerminationGuid UNIQUEIDENTIFIER = 'EA08282D-87DB-4DE1-8146-6004329663F3',
	@dcSalHirTermsTerminationName NVARCHAR(MAX) = @prefix + ' Employment - Termination',
	@dcSalHirTermsTerminationDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsTerminationText NVARCHAR(MAX) = 'Either party may terminate the employment relationship with the appropriate notice as prescribed in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  Notice provisions do not apply in the case of summary dismissal.
<br><br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee.
<br><br>
IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.',
	@dcSalHirTermsTerminationHeadline NVARCHAR(MAX) = 'Termination',
	@dcSalHirTermsTerminationSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsTerminationGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsTerminationName, 
		@dcSalHirTermsTerminationDescription,
		@dcSalHirTermsTerminationText, 
		@dcSalHirTermsTerminationHeadline,
		@dcSalHirTermsTerminationSortOrder,
		@dcSalHirTermsTerminationGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsTerminationName, 
		[Description] = @dcSalHirTermsTerminationDescription, 
		[Text] = @dcSalHirTermsTerminationText,
		[Headline] = @dcSalHirTermsTerminationHeadline,
		SortOrder = @dcSalHirTermsTerminationSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsTerminationGuid
END

-- #################################### 25 Company Policies & Procedures 
DECLARE @dcSalHirTermsPoliciesGuid UNIQUEIDENTIFIER = 'F62963CD-B709-436B-997B-8D41FFF60E3B',
	@dcSalHirTermsPoliciesName NVARCHAR(MAX) = @prefix + ' Employment - Policies',
	@dcSalHirTermsPoliciesDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsPoliciesText NVARCHAR(MAX) = 'You shall be required to comply with all IKEA Policies and Procedures as amended from time to time and as outlined in IKEA’s Policy Guidelines and Welcome Program. The IKEA Policies and Procedures form part of your contract of employment.',
	@dcSalHirTermsPoliciesHeadline NVARCHAR(MAX) = 'Company Policies & Procedures',
	@dcSalHirTermsPoliciesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsPoliciesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsPoliciesName, 
		@dcSalHirTermsPoliciesDescription,
		@dcSalHirTermsPoliciesText, 
		@dcSalHirTermsPoliciesHeadline,
		@dcSalHirTermsPoliciesSortOrder,
		@dcSalHirTermsPoliciesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsPoliciesName, 
		[Description] = @dcSalHirTermsPoliciesDescription, 
		[Text] = @dcSalHirTermsPoliciesText,
		[Headline] = @dcSalHirTermsPoliciesHeadline,
		SortOrder = @dcSalHirTermsPoliciesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPoliciesGuid
END

-- #################################### 26 Other Terms and Conditions
DECLARE @dcSalHirTermsOtherTermsGuid UNIQUEIDENTIFIER = 'B6A4C2A9-C14E-4B0C-9874-8AE9BC2B9F3B',
	@dcSalHirTermsOtherTermsName NVARCHAR(MAX) = @prefix + ' Employment - Other T&C',
	@dcSalHirTermsOtherTermsDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsOtherTermsText NVARCHAR(MAX) = 'The terms and conditions contained within the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016 (and any subsequent statutory agreement binding you and IKEA) also apply to your employment.  A copy of this Agreement is available for your perusal at all times.',
	@dcSalHirTermsOtherTermsHeadline NVARCHAR(MAX) = 'Other Terms and Conditions',
	@dcSalHirTermsOtherTermsSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsOtherTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsOtherTermsName, 
		@dcSalHirTermsOtherTermsDescription,
		@dcSalHirTermsOtherTermsText, 
		@dcSalHirTermsOtherTermsHeadline,
		@dcSalHirTermsOtherTermsSortOrder,
		@dcSalHirTermsOtherTermsGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsOtherTermsName, 
		[Description] = @dcSalHirTermsOtherTermsDescription, 
		[Text] = @dcSalHirTermsOtherTermsText,
		[Headline] = @dcSalHirTermsOtherTermsHeadline,
		SortOrder = @dcSalHirTermsOtherTermsSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsOtherTermsGuid
END

-- #################################### 27 Police Checks
DECLARE @dcSalHirTermsPoliceGuid UNIQUEIDENTIFIER = '40344DB8-F736-4D70-89AF-E7B20E6E2C1E',
	@dcSalHirTermsPoliceName NVARCHAR(MAX) = @prefix + ' Employment - Police',
	@dcSalHirTermsPoliceDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsPoliceText NVARCHAR(MAX) = 'Some positions at IKEA require evidence of good character (for example - positions that deal with children or cash).  Obtaining details of your criminal history via a police check/s is an integral part of the assessment of your suitability for such positions.
<br><br>
You may be required to provide a police check/s at the time you are given this offer of employment.  Alternatively, you may be required to provide a police check/s during your employment (for instance, when it is suspected that you have incurred a criminal record since the commencement of your employment, or where you begin working in a position requiring evidence of good character).  By signing this offer of employment, you consent to complete, sign and lodge the relevant police check application documentation (which will be provided to you by IKEA), and to direct that the corresponding police check record/s be forwarded directly to IKEA (where permitted) or (otherwise) to provide IKEA with the original police check record/s immediately on receipt.
<br><br>
If you are required to provide the police check/s at the time you are given this offer of employment, you acknowledge that the offer of employment will be subject to the check being satisfactory to IKEA.
<br><br>
If you are required to provide the police check/s at any other time during your employment and the check is not satisfactory to IKEA, it may give grounds for dismissal.
<br><br>
Please note that the existence of a criminal record does not mean that the check will automatically be unsatisfactory, or that you will be assessed automatically as being unsuitable.  Each case will be assessed on its merits and will depend upon the inherent requirements of the position.',
	@dcSalHirTermsPoliceHeadline NVARCHAR(MAX) = 'Police Checks',
	@dcSalHirTermsPoliceSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsPoliceGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsPoliceName, 
		@dcSalHirTermsPoliceDescription,
		@dcSalHirTermsPoliceText, 
		@dcSalHirTermsPoliceHeadline,
		@dcSalHirTermsPoliceSortOrder,
		@dcSalHirTermsPoliceGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsPoliceName, 
		[Description] = @dcSalHirTermsPoliceDescription, 
		[Text] = @dcSalHirTermsPoliceText,
		[Headline] = @dcSalHirTermsPoliceHeadline,
		SortOrder = @dcSalHirTermsPoliceSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPoliceGuid
END



-- Con B
DECLARE @dcSalHirTermsPerfBGuid UNIQUEIDENTIFIER = '18BF9520-1844-4FE7-8F67-6EB6D863B007',
	@dcSalHirTermsPerfBName NVARCHAR(MAX) = @prefix + ' Employment - Performance',
	@dcSalHirTermsPerfBDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsPerfBText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November. This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@dcSalHirTermsPerfBHeadline NVARCHAR(MAX) = 'Performance Management',
	@dcSalHirTermsPerfBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsPerfBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsPerfBName, 
		@dcSalHirTermsPerfBDescription,
		@dcSalHirTermsPerfBText, 
		@dcSalHirTermsPerfBHeadline,
		@dcSalHirTermsPerfBSortOrder,
		@dcSalHirTermsPerfBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsPerfBName, 
		[Description] = @dcSalHirTermsPerfBDescription, 
		[Text] = @dcSalHirTermsPerfBText,
		[Headline] = @dcSalHirTermsPerfBHeadline,
		SortoRder = @dcSalHirTermsPerfBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPerfBGuid
END

DECLARE @dcSalHirTermsPerfBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsPerfBGuid)

-- Create condition for probation period 
DECLARE @dcSalHirTermsPerfBCondGuid UNIQUEIDENTIFIER = 'A337468A-39F2-4FE8-BEB4-3B8D56CF8449',
	@dcSalHirTermsPerfBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ProbationPeriod',
	@dcSalHirTermsPerfBCondOperator NVARCHAR(MAX) = 'Equal',
	@dcSalHirTermsPerfBCondValues NVARCHAR(MAX) = 'No',
	@dcSalHirTermsPerfBCondDescription NVARCHAR(MAX) = 'Has not probation period',
	@dcSalHirTermsPerfBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalHirTermsPerfBCondGuid)
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
		@dcSalHirTermsPerfBCondGuid,
		@dcSalHirTermsPerfBID,
		@dcSalHirTermsPerfBCondPropertyName,
		@dcSalHirTermsPerfBCondOperator,
		@dcSalHirTermsPerfBCondValues,
		@dcSalHirTermsPerfBCondDescription,
		@dcSalHirTermsPerfBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalHirTermsPerfBID,
		Property_Name = @dcSalHirTermsPerfBCondPropertyName,
		Operator = @dcSalHirTermsPerfBCondOperator,
		[Values] = @dcSalHirTermsPerfBCondValues,
		[Description] = @dcSalHirTermsPerfBCondDescription,
		[Status] = @dcSalHirTermsPerfBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalHirTermsPerfBCondGuid
END


-- #################################### 29 Communications with Media
DECLARE @dcSalHirTermsMediaGuid UNIQUEIDENTIFIER = '6DBF6539-F1B0-4DC9-BEE4-B31996A9E7A2',
	@dcSalHirTermsMediaName NVARCHAR(MAX) = @prefix + ' Employment - Media',
	@dcSalHirTermsMediaDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsMediaText NVARCHAR(MAX) = 'You shall not provide information or speak on behalf of IKEA or otherwise to the media on any matters concerning IKEA’s business or activities.  You must refer all requests from the media for information and/or interviews to the DC Manager.',
	@dcSalHirTermsMediaHeadline NVARCHAR(MAX) = 'Communications with Media',
	@dcSalHirTermsMediaSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsMediaGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsMediaName, 
		@dcSalHirTermsMediaDescription,
		@dcSalHirTermsMediaText, 
		@dcSalHirTermsMediaHeadline,
		@dcSalHirTermsMediaSortOrder,
		@dcSalHirTermsMediaGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsMediaName, 
		[Description] = @dcSalHirTermsMediaDescription, 
		[Text] = @dcSalHirTermsMediaText,
		[Headline] = @dcSalHirTermsMediaHeadline,
		SortOrder = @dcSalHirTermsMediaSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsMediaGuid
END

-- #################################### 30 Obligation to report unlawful activities
DECLARE @dcSalHirTermsUnlawGuid UNIQUEIDENTIFIER = '276C55C5-7383-4B05-8D21-5ADFA82340A5',
	@dcSalHirTermsUnlawName NVARCHAR(MAX) = @prefix + ' Employment - Unlawful',
	@dcSalHirTermsUnlawDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsUnlawText NVARCHAR(MAX) = 'If you become aware of or suspect any unlawful act or omission by any IKEA employee, you must advise IKEA immediately.',
	@dcSalHirTermsUnlawHeadline NVARCHAR(MAX) = 'Obligation to report unlawful activities',
	@dcSalHirTermsUnlawSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsUnlawGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsUnlawName, 
		@dcSalHirTermsUnlawDescription,
		@dcSalHirTermsUnlawText, 
		@dcSalHirTermsUnlawHeadline,
		@dcSalHirTermsUnlawSortOrder,
		@dcSalHirTermsUnlawGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsUnlawName, 
		[Description] = @dcSalHirTermsUnlawDescription, 
		[Text] = @dcSalHirTermsUnlawText,
		[Headline] = @dcSalHirTermsUnlawHeadline,
		SortOrder = @dcSalHirTermsUnlawSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsUnlawGuid
END

-- #################################### 31 Intellectual Property
DECLARE @dcSalHirTermsIntelPropGuid UNIQUEIDENTIFIER = '18E2F0ED-36A0-4B42-8A68-F5AF0AE575BB',
	@dcSalHirTermsIntelPropName NVARCHAR(MAX) = @prefix + ' Employment - Int. Property',
	@dcSalHirTermsIntelPropDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsIntelPropText NVARCHAR(MAX) = 'IKEA owns all copyright in any works and all inventions, discoveries, novel designs, improvements or modifications, computer program material and trademarks which you write or develop in the course of your employment (in or out of working hours) (“Intellectual Property”).<br>
<br>
You assign to IKEA any interest you have in the Intellectual Property, and you must disclose any Intellectual Property to IKEA.<br>
<br>
During and after your employment, you must do anything IKEA reasonably requires (at IKEA''s cost) to:
<ul>
<li>obtain statutory protection (including by patent, design registration, trade mark registration or copyright) for the Intellectual Property for IKEA in any country; or</li>
<li>Perfect or evidence IKEA’s ownership of the Intellectual Property.</li>
</ul>',

	@dcSalHirTermsIntelPropHeadline NVARCHAR(MAX) = 'Intellectual Property',
	@dcSalHirTermsIntelPropSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsIntelPropGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsIntelPropName, 
		@dcSalHirTermsIntelPropDescription,
		@dcSalHirTermsIntelPropText, 
		@dcSalHirTermsIntelPropHeadline,
		@dcSalHirTermsIntelPropSortOrder,
		@dcSalHirTermsIntelPropGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsIntelPropName, 
		[Description] = @dcSalHirTermsIntelPropDescription, 
		[Text] = @dcSalHirTermsIntelPropText,
		[Headline] = @dcSalHirTermsIntelPropHeadline,
		SortOrder = @dcSalHirTermsIntelPropSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsIntelPropGuid
END

-- #################################### 32 Variation
DECLARE @dcSalHirTermsVarGuid UNIQUEIDENTIFIER = 'E1B62EF7-34FC-4B53-828B-2805A5B7C18F',
	@dcSalHirTermsVarName NVARCHAR(MAX) = @prefix + ' Employment - Variation',
	@dcSalHirTermsVarDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsVarText NVARCHAR(MAX) = 'This Agreement may only be varied by a written agreement signed by yourself and IKEA.',
	@dcSalHirTermsVarHeadline NVARCHAR(MAX) = 'Variation',
	@dcSalHirTermsVarSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsVarGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsVarName, 
		@dcSalHirTermsVarDescription,
		@dcSalHirTermsVarText, 
		@dcSalHirTermsVarHeadline,
		@dcSalHirTermsVarSortOrder,
		@dcSalHirTermsVarGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsVarName, 
		[Description] = @dcSalHirTermsVarDescription, 
		[Text] = @dcSalHirTermsVarText,
		[Headline] = @dcSalHirTermsVarHeadline,
		SortOrder = @dcSalHirTermsVarSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsVarGuid
END

-- #################################### 33 Suspension
DECLARE @dcSalHirTermsSuspGuid UNIQUEIDENTIFIER = 'A555CCC0-24DA-4669-849E-7596BDFD83EF',
	@dcSalHirTermsSuspName NVARCHAR(MAX) = @prefix + ' Employment - Suspension',
	@dcSalHirTermsSuspDescription NVARCHAR(MAX) = '',
	@dcSalHirTermsSuspText NVARCHAR(MAX) = 'If we have reason to believe that you may have engaged in a serious breach of your employment obligations, IKEA may at its discretion suspend you from your duties, either with or without pay, while and investigation is conducted.',
	@dcSalHirTermsSuspHeadline NVARCHAR(MAX) = 'Suspension',
	@dcSalHirTermsSuspSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirTermsSuspGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirTermsID, 
		@dcSalHirTermsSuspName, 
		@dcSalHirTermsSuspDescription,
		@dcSalHirTermsSuspText, 
		@dcSalHirTermsSuspHeadline,
		@dcSalHirTermsSuspSortOrder,
		@dcSalHirTermsSuspGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirTermsID,
		[Name] = @dcSalHirTermsSuspName, 
		[Description] = @dcSalHirTermsSuspDescription, 
		[Text] = @dcSalHirTermsSuspText,
		[Headline] = @dcSalHirTermsSuspHeadline,
		SortOrder = @dcSalHirTermsSuspSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirTermsSuspGuid
END

-- Add terms paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @dcSalHirTermsID, @counter
SET @counter = @counter + 1


-- #################################### 34-36 End Text
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcSalHirEndTextParagraphGuid UNIQUEIDENTIFIER = '3CC01BED-A4CC-4464-83D5-C3A12790746B',
	@dcSalHirEndTextParagraphName NVARCHAR(MAX) = @prefix + ' Employment - End Text',
	@dcSalHirEndTextParagraphType INT = @ParagraphTypeText,
	@dcSalHirEndTextParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalHirEndTextParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalHirEndTextParagraphName, @dcSalHirEndTextParagraphDescription, @dcSalHirEndTextParagraphType, @dcSalHirEndTextParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalHirEndTextParagraphName, [Description] = @dcSalHirEndTextParagraphDescription, ParagraphType = @dcSalHirEndTextParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalHirEndTextParagraphGuid
END
DECLARE @dcSalHirEndTextParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalHirEndTextParagraphGuid)

-- Create a text field
DECLARE @dcSalHirEndTextGuid UNIQUEIDENTIFIER = 'E82C5361-3148-4962-8191-CB298F43B11D',
	@dcSalHirEndTextName NVARCHAR(MAX) = @prefix + ' Employment - End Text',
	@dcSalHirEndTextDescription NVARCHAR(MAX) = '',
	@dcSalHirEndTextText NVARCHAR(MAX) = 'IKEA recognises that its co-workers are essential to the success of the company’s operations.  IKEA remains committed to ensuring that all co-workers are treated fairly and equitably and encourages co-workers to reach their full potential.  We believe that the basis of your employment outlined above, will achieve these objectives and greatly benefit those co-workers willing to develop themselves.
	<br><br>
	As an indication of your understanding and acceptance of these conditions, please sign this letter of offer, and return to the undersigned within seven (7) days.  Please retain the second copy for your records.
	<br><br>
	If you have any questions pertaining to this offer of employment or any of the information contained herein, please do not hesitate to contact me before signing this letter.',
	@dcSalHirEndTextHeadline NVARCHAR(MAX) = '',
	@dcSalHirEndTextSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirEndTextGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirEndTextParagraphID, 
		@dcSalHirEndTextName, 
		@dcSalHirEndTextDescription,
		@dcSalHirEndTextText, 
		@dcSalHirEndTextHeadline,
		@dcSalHirEndTextSortOrder,
		@dcSalHirEndTextGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirEndTextParagraphID,
		[Name] = @dcSalHirEndTextName, 
		[Description] = @dcSalHirEndTextDescription, 
		[Text] = @dcSalHirEndTextText,
		[Headline] = @dcSalHirEndTextHeadline,
		SortOrder = @dcSalHirEndTextSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirEndTextGuid
END

-- Add end text paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @dcSalHirEndTextParagraphID, @counter
SET @counter = @counter + 1

-- #################################### 37-40 Contractor Signature
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcSalHirConSignParagraphGuid UNIQUEIDENTIFIER = 'C1E8ACDF-508E-4EF0-AC39-F47D92B12489',
	@dcSalHirConSignParagraphName NVARCHAR(MAX) = @prefix + ' Employment - Con. Sign.',
	@dcSalHirConSignParagraphType INT = @ParagraphTypeText,
	@dcSalHirConSignParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalHirConSignParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalHirConSignParagraphName, @dcSalHirConSignParagraphDescription, @dcSalHirConSignParagraphType, @dcSalHirConSignParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalHirConSignParagraphName, [Description] = @dcSalHirConSignParagraphDescription, ParagraphType = @dcSalHirConSignParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalHirConSignParagraphGuid
END
DECLARE @dcSalHirConSignParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalHirConSignParagraphGuid)

-- Create a text field
DECLARE @dcSalHirConSignGuid UNIQUEIDENTIFIER = 'FA6C49A6-4483-4C23-913A-45D159557523',
	@dcSalHirConSignName NVARCHAR(MAX) = @prefix + ' Employment - Con. Sign.',
	@dcSalHirConSignDescription NVARCHAR(MAX) = '',
	@dcSalHirConSignText NVARCHAR(MAX) = 'Yours sincerely<br>
	<Reports to Line Manager><br>
	<Position Title (Local Job Name)> of <Reports To Line Manager><br>
	<strong>IKEA Distribution Services Australia Pty Ltd</strong>',
	@dcSalHirConSignHeadline NVARCHAR(MAX) = '',
	@dcSalHirConSignSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirConSignGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirConSignParagraphID, 
		@dcSalHirConSignName, 
		@dcSalHirConSignDescription,
		@dcSalHirConSignText, 
		@dcSalHirConSignHeadline,
		@dcSalHirConSignSortOrder,
		@dcSalHirConSignGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirConSignParagraphID,
		[Name] = @dcSalHirConSignName, 
		[Description] = @dcSalHirConSignDescription, 
		[Text] = @dcSalHirConSignText,
		[Headline] = @dcSalHirConSignHeadline,
		SortOrder = @dcSalHirConSignSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirConSignGuid
END

-- Add contractor signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @dcSalHirConSignParagraphID, @counter
SET @counter = @counter + 1

-- #################################### 41-48 Acceptance
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcSalHirAcceptParagraphGuid UNIQUEIDENTIFIER = '95419F05-9B86-479F-A22C-810E3A27D130',
	@dcSalHirAcceptParagraphName NVARCHAR(MAX) = @prefix + ' Employment - Acceptance',
	@dcSalHirAcceptParagraphType INT = @ParagraphTypeText,
	@dcSalHirAcceptParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalHirAcceptParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalHirAcceptParagraphName, @dcSalHirAcceptParagraphDescription, @dcSalHirAcceptParagraphType, @dcSalHirAcceptParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalHirAcceptParagraphName, [Description] = @dcSalHirAcceptParagraphDescription, ParagraphType = @dcSalHirAcceptParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalHirAcceptParagraphGuid
END
DECLARE @dcSalHirAcceptParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalHirAcceptParagraphGuid)

-- Create a text field
DECLARE @dcSalHirAcceptGuid UNIQUEIDENTIFIER = '08EADA4B-5C0F-4816-9650-EDB2784FDF01',
	@dcSalHirAcceptName NVARCHAR(MAX) = @prefix + ' Employment - Acceptance',
	@dcSalHirAcceptDescription NVARCHAR(MAX) = '',
	@dcSalHirAcceptText NVARCHAR(MAX) = '<table style="border: 1px solid black">
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
	@dcSalHirAcceptHeadline NVARCHAR(MAX) = '',
	@dcSalHirAcceptSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalHirAcceptGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalHirAcceptParagraphID, 
		@dcSalHirAcceptName, 
		@dcSalHirAcceptDescription,
		@dcSalHirAcceptText, 
		@dcSalHirAcceptHeadline,
		@dcSalHirAcceptSortOrder,
		@dcSalHirAcceptGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalHirAcceptParagraphID,
		[Name] = @dcSalHirAcceptName, 
		[Description] = @dcSalHirAcceptDescription, 
		[Text] = @dcSalHirAcceptText,
		[Headline] = @dcSalHirAcceptHeadline,
		SortOrder = @dcSalHirAcceptSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalHirAcceptGuid
END

-- Add acceptance paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalHirID, @dcSalHirAcceptParagraphID, @counter
SET @counter = @counter + 1




-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @dcSalHirGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



COMMIT

