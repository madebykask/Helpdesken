--########################################
--########## DC Salary T&C ###################
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
DECLARE @prefix NVARCHAR(MAX) = 'DC SAL T&C'

-- #################################### Contract Clusters – DC EA T&C ####################################

-- Get the form
DECLARE @dcSalTcGuid UNIQUEIDENTIFIER = 'ED5D9C91-69A7-4564-8A4B-FB141CA4C007'
DECLARE @dcSalTcID INT, @counter INT = 0
SELECT @dcSalTcID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcSalTcGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @dcSalTcGuid


-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @footerID, @counter
SET @counter = @counter + 1

-- #################################### Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### Address and company info
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @addressInfoID, @counter
SET @counter = @counter + 1

-- #################################### Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @dcSalTcEmployGreetingGuid UNIQUEIDENTIFIER = '805476D1-42A1-4CED-8C85-53DBF80C63D9',
	@cdpName NVARCHAR(MAX) = @prefix + ' Employment greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalTcEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @dcSalTcEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalTcEmployGreetingGuid
END
DECLARE @dcSalTcEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalTcEmployGreetingGuid)

---- Create or update text A, Full Time
DECLARE @dcSalTcEmployGreetingTextAGuid UNIQUEIDENTIFIER = '1A53250B-E28E-47C9-96BA-95339F18A804',
	@dcSalTcEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Greeting, full time',
	@dcSalTcEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@dcSalTcEmployGreetingTextAText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Full Time <Position Title (Local Job Name)> at IKEA has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcSalTcEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@dcSalTcEmployGreetingTextASortOrder INT = 0


IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcEmployGreetingID, 
		@dcSalTcEmployGreetingTextAName, 
		@dcSalTcEmployGreetingTextADescription,
		@dcSalTcEmployGreetingTextAText, 
		@dcSalTcEmployGreetingTextAHeadline,
		@dcSalTcEmployGreetingTextASortOrder,
		@dcSalTcEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcEmployGreetingID,
		[Name] = @dcSalTcEmployGreetingTextAName, 
		[Description] = @dcSalTcEmployGreetingTextADescription, 
		[Text] = @dcSalTcEmployGreetingTextAText,
		[Headline] = @dcSalTcEmployGreetingTextAHeadline,
		SortOrder = @dcSalTcEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcEmployGreetingTextAGuid
END
DECLARE @dcSalTcEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @dcSalTcEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = 'D4408C19-C306-4954-BBF9-86D843A7C806',
	@dcSalTcEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalTcEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'Equal',
	@dcSalTcEmployGreetingTextACondAValues NVARCHAR(MAX) = '76',
	@dcSalTcEmployGreetingTextACondADescription NVARCHAR(MAX) = 'Is full time',
	@dcSalTcEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcEmployGreetingTextACondAGuid)
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
		@dcSalTcEmployGreetingTextACondAGuid,
		@dcSalTcEmployGreetingTextAID,
		@dcSalTcEmployGreetingTextACondAPropertyName,
		@dcSalTcEmployGreetingTextACondAOperator,
		@dcSalTcEmployGreetingTextACondAValues,
		@dcSalTcEmployGreetingTextACondADescription,
		@dcSalTcEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcEmployGreetingTextAID,
		Property_Name = @dcSalTcEmployGreetingTextACondAPropertyName,
		Operator = @dcSalTcEmployGreetingTextACondAOperator,
		[Values] = @dcSalTcEmployGreetingTextACondAValues,
		[Description] = @dcSalTcEmployGreetingTextACondADescription,
		[Status] = @dcSalTcEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @dcSalTcEmployGreetingTextBGuid UNIQUEIDENTIFIER = '9EBEA2D0-85E1-429B-B193-89B866F3BD4C',
	@dcSalTcEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Greeting, part time',
	@dcSalTcEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@dcSalTcEmployGreetingTextBText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Part Time <Position Title (Local Job Name)> at IKEA has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcSalTcEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@dcSalTcEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcEmployGreetingID, 
		@dcSalTcEmployGreetingTextBName, 
		@dcSalTcEmployGreetingTextBDescription,
		@dcSalTcEmployGreetingTextBText, 
		@dcSalTcEmployGreetingTextBHeadline,
		@dcSalTcEmployGreetingTextBSortOrder,
		@dcSalTcEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcEmployGreetingID,
		[Name] = @dcSalTcEmployGreetingTextBName, 
		[Description] = @dcSalTcEmployGreetingTextBDescription, 
		[Text] = @dcSalTcEmployGreetingTextBText,
		[Headline] = @dcSalTcEmployGreetingTextBHeadline,
		SortOrder = @dcSalTcEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcEmployGreetingTextBGuid
END
DECLARE @dcSalTcEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @dcSalTcEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = '14FA15C4-2CD8-4285-960C-CB8B8B8EC5FA',
	@dcSalTcEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalTcEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@dcSalTcEmployGreetingTextBCondAValues NVARCHAR(MAX) = '76',
	@dcSalTcEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Is part time',
	@dcSalTcEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcEmployGreetingTextBCondAGuid)
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
		@dcSalTcEmployGreetingTextBCondAGuid,
		@dcSalTcEmployGreetingTextBID,
		@dcSalTcEmployGreetingTextBCondAPropertyName,
		@dcSalTcEmployGreetingTextBCondAOperator,
		@dcSalTcEmployGreetingTextBCondAValues,
		@dcSalTcEmployGreetingTextBCondADescription,
		@dcSalTcEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcEmployGreetingTextBID,
		Property_Name = @dcSalTcEmployGreetingTextBCondAPropertyName,
		Operator = @dcSalTcEmployGreetingTextBCondAOperator,
		[Values] = @dcSalTcEmployGreetingTextBCondAValues,
		[Description] = @dcSalTcEmployGreetingTextBCondADescription,
		[Status] = @dcSalTcEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcEmployGreetingTextBCondAGuid
END


-- Add paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @dcSalTcEmployGreetingID, @counter
SET @counter = @counter + 1

-- #################################### Terms

DECLARE @termsCounter INT = 0
---- Create or update a terms paragraph
-- Paragraph guid
DECLARE @dcSalTcTermsGuid UNIQUEIDENTIFIER = 'BA38DE18-F7C0-466F-BC1E-734F23F80CE9',
	@dcSalTcTermsName NVARCHAR(MAX) = @prefix + ' Terms',
	@dcSalTcTermsParagraphType INT = @ParagraphTypeTableNumeric,
	@termsDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalTcTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalTcTermsName, @termsDescription, @dcSalTcTermsParagraphType, @dcSalTcTermsGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalTcTermsName, [Description] = @termsDescription, ParagraphType = @dcSalTcTermsParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalTcTermsGuid
END
DECLARE @dcSalTcTermsID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalTcTermsGuid)

-- #################################### Position

---- Position A
DECLARE @dcSalTcTermsPositionAGuid UNIQUEIDENTIFIER = '09E1B35C-6B55-43F0-9CD8-E4BEFF0603ED',
	@dcSalTcTermsPositionAName NVARCHAR(MAX) = @prefix + ' Position, full time',
	@dcSalTcTermsPositionADescription NVARCHAR(MAX) = '',
	@dcSalTcTermsPositionAText NVARCHAR(MAX) = 'Your position is Full Time <Position Title (Local Job Name)>, located at <Business Unit>, reporting to the <Position Title (Local Job Name) of Reports To Line Manager>. Your position (in terms of your duties and responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcSalTcTermsPositionAHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcSalTcTermsPositionASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsPositionAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsPositionAName, 
		@dcSalTcTermsPositionADescription,
		@dcSalTcTermsPositionAText, 
		@dcSalTcTermsPositionAHeadline,
		@dcSalTcTermsPositionASortOrder,
		@dcSalTcTermsPositionAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsPositionAName, 
		[Description] = @dcSalTcTermsPositionADescription, 
		[Text] = @dcSalTcTermsPositionAText,
		[Headline] = @dcSalTcTermsPositionAHeadline,
		SortOrder = @dcSalTcTermsPositionASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsPositionAGuid
END
DECLARE @dcSalTcTermsPositionAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsPositionAGuid)

-- Create condition for position A
DECLARE @dcSalTcTermsPositionACondGuid UNIQUEIDENTIFIER = '06317670-6BE9-45F3-9089-4CEF430A9CB1',
	@dcSalTcTermsPositionACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalTcTermsPositionACondOperator NVARCHAR(MAX) = 'Equal',
	@dcSalTcTermsPositionACondValues NVARCHAR(MAX) = '76',
	@dcSalTcTermsPositionACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcSalTcTermsPositionACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsPositionACondGuid)
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
		@dcSalTcTermsPositionACondGuid,
		@dcSalTcTermsPositionAID,
		@dcSalTcTermsPositionACondPropertyName,
		@dcSalTcTermsPositionACondOperator,
		@dcSalTcTermsPositionACondValues,
		@dcSalTcTermsPositionACondDescription,
		@dcSalTcTermsPositionACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsPositionAID,
		Property_Name = @dcSalTcTermsPositionACondPropertyName,
		Operator = @dcSalTcTermsPositionACondOperator,
		[Values] = @dcSalTcTermsPositionACondValues,
		[Description] = @dcSalTcTermsPositionACondDescription,
		[Status] = @dcSalTcTermsPositionACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsPositionACondGuid
END

---- Position B
DECLARE @dcSalTcTermsPositionBGuid UNIQUEIDENTIFIER = '5C3B5B89-6EF6-4576-A0FC-A46D47E0140B',
	@dcSalTcTermsPositionBName NVARCHAR(MAX) = @prefix + ' Position, part time',
	@dcSalTcTermsPositionBDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsPositionBText NVARCHAR(MAX) = 'Your position is Part Time <Position Title (Local Job Name)>, located at <Business Unit>, reporting to the <Position Title (Local Job Name) of Reports To Line Manager>. Your position (in terms of your duties and responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcSalTcTermsPositionBHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcSalTcTermsPositionBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsPositionBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsPositionBName, 
		@dcSalTcTermsPositionBDescription,
		@dcSalTcTermsPositionBText, 
		@dcSalTcTermsPositionBHeadline,
		@dcSalTcTermsPositionBSortOrder,
		@dcSalTcTermsPositionBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsPositionBName, 
		[Description] = @dcSalTcTermsPositionBDescription, 
		[Text] = @dcSalTcTermsPositionBText,
		[Headline] = @dcSalTcTermsPositionBHeadline,
		SortOrder = @dcSalTcTermsPositionBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsPositionBGuid
END
DECLARE @dcSalTcTermsPositionBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsPositionBGuid)

-- Create condition for position A
DECLARE @dcSalTcTermsPositionBCondGuid UNIQUEIDENTIFIER = '6EA54E0A-FF30-44CC-8C0D-1163825EA105',
	@dcSalTcTermsPositionBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcSalTcTermsPositionBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcSalTcTermsPositionBCondValues NVARCHAR(MAX) = '76',
	@dcSalTcTermsPositionBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcSalTcTermsPositionBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsPositionBCondGuid)
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
		@dcSalTcTermsPositionBCondGuid,
		@dcSalTcTermsPositionBID,
		@dcSalTcTermsPositionBCondPropertyName,
		@dcSalTcTermsPositionBCondOperator,
		@dcSalTcTermsPositionBCondValues,
		@dcSalTcTermsPositionBCondDescription,
		@dcSalTcTermsPositionBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsPositionBID,
		Property_Name = @dcSalTcTermsPositionBCondPropertyName,
		Operator = @dcSalTcTermsPositionBCondOperator,
		[Values] = @dcSalTcTermsPositionBCondValues,
		[Description] = @dcSalTcTermsPositionBCondDescription,
		[Status] = @dcSalTcTermsPositionBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsPositionBCondGuid
END

-- #################################### Commencement Date

---- Commencement A
DECLARE @dcSalTcTermsComAGuid UNIQUEIDENTIFIER = '7ACE12E4-2FA4-43D8-AC8D-CDAFF36A7756',
	@dcSalTcTermsComAName NVARCHAR(MAX) = @prefix + ' Employment - Commencement - No date',
	@dcSalTcTermsComADescription NVARCHAR(MAX) = '',
	@dcSalTcTermsComAText NVARCHAR(MAX) = 'Your commencement date is <Change Valid From>, unless otherwise terminated in accordance with this contract.',
	@dcSalTcTermsComAHeadline NVARCHAR(MAX) = 'Commencement Date',
	@dcSalTcTermsComASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsComAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsComAName, 
		@dcSalTcTermsComADescription,
		@dcSalTcTermsComAText, 
		@dcSalTcTermsComAHeadline,
		@dcSalTcTermsComASortOrder,
		@dcSalTcTermsComAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsComAName, 
		[Description] = @dcSalTcTermsComADescription, 
		[Text] = @dcSalTcTermsComAText,
		[Headline] = @dcSalTcTermsComAHeadline,
		SortOrder = @dcSalTcTermsComASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsComAGuid
END
DECLARE @dcSalTcTermsComAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsComAGuid)

-- Create condition for Commencement A
DECLARE @dcSalTcTermsComACondAGuid UNIQUEIDENTIFIER = 'D44099CA-6B08-49DB-858F-18C0B0DD7CA9',
	@dcSalTcTermsComACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@dcSalTcTermsComACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@dcSalTcTermsComACondAValues NVARCHAR(MAX) = '',
	@dcSalTcTermsComACondADescription NVARCHAR(MAX) = 'Has no end date',
	@dcSalTcTermsComACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsComACondAGuid)
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
		@dcSalTcTermsComACondAGuid,
		@dcSalTcTermsComAID,
		@dcSalTcTermsComACondAPropertyName,
		@dcSalTcTermsComACondAOperator,
		@dcSalTcTermsComACondAValues,
		@dcSalTcTermsComACondADescription,
		@dcSalTcTermsComACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsComAID,
		Property_Name = @dcSalTcTermsComACondAPropertyName,
		Operator = @dcSalTcTermsComACondAOperator,
		[Values] = @dcSalTcTermsComACondAValues,
		[Description] = @dcSalTcTermsComACondADescription,
		[Status] = @dcSalTcTermsComACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsComACondAGuid
END

-- No support for 31.12.9999 yet
--DECLARE @dcSalTcTermsComACondBGuid UNIQUEIDENTIFIER = 'e68e3e7c-52b0-4018-964b-99a1d9d471b9',
--	@dcSalTcTermsComACondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
--	@dcSalTcTermsComACondBOperator NVARCHAR(MAX) = 'Equal',
--	@dcSalTcTermsComACondBValues NVARCHAR(MAX) = '31.12.9999',
--	@dcSalTcTermsComACondBDescription NVARCHAR(MAX) = 'Equal 31.12.9999',
--	@dcSalTcTermsComACondBStatus INT = 1

--IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsComACondBGuid)
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
--		@dcSalTcTermsComACondBGuid,
--		@dcSalTcTermsComAID,
--		@dcSalTcTermsComACondBPropertyName,
--		@dcSalTcTermsComACondBOperator,
--		@dcSalTcTermsComACondBValues,
--		@dcSalTcTermsComACondBDescription,
--		@dcSalTcTermsComACondBStatus,
--		@now, 
--		@userID,
--		@now,
--		@userID
--	)
--END
--ELSE
--BEGIN
--	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsComAID,
--		Property_Name = @dcSalTcTermsComACondBPropertyName,
--		Operator = @dcSalTcTermsComACondBOperator,
--		[Values] = @dcSalTcTermsComACondBValues,
--		[Description] = @dcSalTcTermsComACondBDescription,
--		[Status] = @dcSalTcTermsComACondBStatus,
--		CreatedDate = @now,
--		CreatedByUser_Id = @userID,
--		ChangedDate = @now,
--		ChangedByUser_Id = @userID
--	FROM tblCaseDocumentTextCondition CDTC
--	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsComACondBGuid
--END

---- Commencement B
DECLARE @dcSalTcTermsComBGuid UNIQUEIDENTIFIER = '7C948166-2E52-40D0-827D-FE5F054A61AB',
	@dcSalTcTermsComBName NVARCHAR(MAX) = @prefix + ' Employment - Commencement - Has end date',
	@dcSalTcTermsComBDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsComBText NVARCHAR(MAX) = 'Your commencement date is <Change Valid From> and will cease on <Change Valid To>, unless otherwise terminated in accordance with this contract.',
	@dcSalTcTermsComBHeadline NVARCHAR(MAX) = 'Commencement date',
	@dcSalTcTermsComBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsComBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsComBName, 
		@dcSalTcTermsComBDescription,
		@dcSalTcTermsComBText, 
		@dcSalTcTermsComBHeadline,
		@dcSalTcTermsComBSortOrder,
		@dcSalTcTermsComBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsComBName, 
		[Description] = @dcSalTcTermsComBDescription, 
		[Text] = @dcSalTcTermsComBText,
		[Headline] = @dcSalTcTermsComBHeadline,
		SortOrder = @dcSalTcTermsComBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsComBGuid
END
DECLARE @dcSalTcTermsComBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsComBGuid)

-- Create condition for Commence B
DECLARE @dcSalTcTermsComBCondAGuid UNIQUEIDENTIFIER = '46303E6E-55DC-4091-B742-D2A290D888B1',
	@dcSalTcTermsComBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@dcSalTcTermsComBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@dcSalTcTermsComBCondAValues NVARCHAR(MAX) = '',
	@dcSalTcTermsComBCondADescription NVARCHAR(MAX) = 'Has end date',
	@dcSalTcTermsComBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsComBCondAGuid)
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
		@dcSalTcTermsComBCondAGuid,
		@dcSalTcTermsComBID,
		@dcSalTcTermsComBCondAPropertyName,
		@dcSalTcTermsComBCondAOperator,
		@dcSalTcTermsComBCondAValues,
		@dcSalTcTermsComBCondADescription,
		@dcSalTcTermsComBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsComBID,
		Property_Name = @dcSalTcTermsComBCondAPropertyName,
		Operator = @dcSalTcTermsComBCondAOperator,
		[Values] = @dcSalTcTermsComBCondAValues,
		[Description] = @dcSalTcTermsComBCondADescription,
		[Status] = @dcSalTcTermsComBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsComBCondAGuid
END

-- No support for 31.12.9999 yet
/*DECLARE @dcSalTcTermsComBCondBGuid UNIQUEIDENTIFIER = '43c72a21-96c1-4d3c-a44a-5279593332c7',
	@dcSalTcTermsComBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcSalTcTermsComBCondBOperator NVARCHAR(MAX) = 'NotEqual',
	@dcSalTcTermsComBCondBValues NVARCHAR(MAX) = '31.12.9999',
	@dcSalTcTermsComBCondBDescription NVARCHAR(MAX) = 'Not equals 31.12.9999',
	@dcSalTcTermsComBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsComBCondBGuid)
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
		@dcSalTcTermsComBCondBGuid,
		@dcSalTcTermsComBID,
		@dcSalTcTermsComBCondBPropertyName,
		@dcSalTcTermsComBCondBOperator,
		@dcSalTcTermsComBCondBValues,
		@dcSalTcTermsComBCondBDescription,
		@dcSalTcTermsComBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsComBID,
		Property_Name = @dcSalTcTermsComBCondBPropertyName,
		Operator = @dcSalTcTermsComBCondBOperator,
		[Values] = @dcSalTcTermsComBCondBValues,
		[Description] = @dcSalTcTermsComBCondBDescription,
		[Status] = @dcSalTcTermsComBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsComBCondBGuid
END*/


-- #################################### Remuneration
DECLARE @dcSalTcTermsRemunGuid UNIQUEIDENTIFIER = 'D5BF811E-3F66-4686-860F-E6FB557607D3',
	@dcSalTcTermsRemunName NVARCHAR(MAX) = @prefix + ' Remuneration',
	@dcSalTcTermsRemunDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsRemunText NVARCHAR(MAX) = 'Upon commencement, your Total Remuneration package will be $<Basic Pay Amount> per annum.  Attached is a Remuneration Statement, which outlines the break-up of your Total Remuneration package.<br>
	<br>
Your salary will be paid directly into your nominated bank account on a fortnightly basis.',
	@dcSalTcTermsRemunHeadline NVARCHAR(MAX) = 'Remuneration',
	@dcSalTcTermsRemunSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsRemunGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsRemunName, 
		@dcSalTcTermsRemunDescription,
		@dcSalTcTermsRemunText, 
		@dcSalTcTermsRemunHeadline,
		@dcSalTcTermsRemunSortOrder,
		@dcSalTcTermsRemunGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsRemunName, 
		[Description] = @dcSalTcTermsRemunDescription, 
		[Text] = @dcSalTcTermsRemunText,
		[Headline] = @dcSalTcTermsRemunHeadline,
		SortOrder = @dcSalTcTermsRemunSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsRemunGuid
END


-- #################################### Superannuation
DECLARE @dcSalTcTermsSuperGuid UNIQUEIDENTIFIER = '33484D30-F7F7-4294-AA62-24372A24A8FA',
	@dcSalTcTermsSuperName NVARCHAR(MAX) = @prefix + ' Superannuation',
	@dcSalTcTermsSuperDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsSuperText NVARCHAR(MAX) = 'IKEA will make superannuation contributions, on your behalf, to the IKEA default Superannuation Fund, at the rate payable under the Superannuation Guarantee Legislation (SGL). This rate is currently 9.5% of your wage/salary.<br>
<br>
IKEA’s current employer superannuation fund is the Labour Union Co-operative Retirement Fund (LUCRF), which is the fund into which the superannuation contributions will be made unless an alternate fund is nominated by you in writing, in accordance with the SGL.<br>
<br>
It is your responsibility to nominate a Super Fund for your contributions to be made to, and to ensure that you complete the necessary paperwork for enrolment into your nominated fund.  IKEA will supply you with a LUCRF Member Guide, including an application form.',
	@dcSalTcTermsSuperHeadline NVARCHAR(MAX) = 'Superannuation',
	@dcSalTcTermsSuperSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsSuperGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsSuperName, 
		@dcSalTcTermsSuperDescription,
		@dcSalTcTermsSuperText, 
		@dcSalTcTermsSuperHeadline,
		@dcSalTcTermsSuperSortOrder,
		@dcSalTcTermsSuperGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsSuperName, 
		[Description] = @dcSalTcTermsSuperDescription, 
		[Text] = @dcSalTcTermsSuperText,
		[Headline] = @dcSalTcTermsSuperHeadline,
		SortOrder = @dcSalTcTermsSuperSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsSuperGuid
END

-- #################################### Hours of Work

DECLARE @dcSalTcTermsHWGuid UNIQUEIDENTIFIER = '9BED8CAD-0255-4FF7-9B49-23DD7247BD5B',
	@dcSalTcTermsHWName NVARCHAR(MAX) = @prefix + ' Hours of Work',
	@dcSalTcTermsHWDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsHWText NVARCHAR(MAX) = 'Your normal working hours are <Contracted Hours> hours between Monday to Sunday per fortnight.  However, your position will require you to work beyond these hours from time to time and on weekends.  Your level of salary takes into account these additional hours, which may be required from time to time to fulfill the responsibilities of your role.',
	@dcSalTcTermsHWHeadline NVARCHAR(MAX) = 'Hours of Work',
	@dcSalTcTermsHWSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsHWGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsHWName, 
		@dcSalTcTermsHWDescription,
		@dcSalTcTermsHWText, 
		@dcSalTcTermsHWHeadline,
		@dcSalTcTermsHWSortOrder,
		@dcSalTcTermsHWGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsHWName, 
		[Description] = @dcSalTcTermsHWDescription, 
		[Text] = @dcSalTcTermsHWText,
		[Headline] = @dcSalTcTermsHWHeadline,
		SortOrder = @dcSalTcTermsHWSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsHWGuid
END

-- #################################### Probationary Period

---- Probationary Period
DECLARE @dcSalTcTermsProbTimeGuid UNIQUEIDENTIFIER = 'C3E1599B-F185-4DBB-BCF8-38BF73B543B0',
	@dcSalTcTermsProbTimeName NVARCHAR(MAX) = @prefix + ' Probationary Period',
	@dcSalTcTermsProbTimeDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsProbTimeText NVARCHAR(MAX) = 'IKEA offers this employment to you on a probationary basis for a period of six (6) months, during which time your performance standards will be subject to regular review and assessment.  In the six (6)-month period, if either you or IKEA wishes to terminate the employment relationship, then either party can effect that termination with one week’s notice in writing.',
	@dcSalTcTermsProbTimeHeadline NVARCHAR(MAX) = 'Probationary Period',
	@dcSalTcTermsProbTimeSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsProbTimeGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsProbTimeName, 
		@dcSalTcTermsProbTimeDescription,
		@dcSalTcTermsProbTimeText, 
		@dcSalTcTermsProbTimeHeadline,
		@dcSalTcTermsProbTimeSortOrder,
		@dcSalTcTermsProbTimeGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsProbTimeName, 
		[Description] = @dcSalTcTermsProbTimeDescription, 
		[Text] = @dcSalTcTermsProbTimeText,
		[Headline] = @dcSalTcTermsProbTimeHeadline,
		SortOrder = @dcSalTcTermsProbTimeSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsProbTimeGuid
END

DECLARE @dcSalTcTermsProbID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsProbTimeGuid)

-- Create condition for probation period 
DECLARE @dcSalTcTermsProbCondGuid UNIQUEIDENTIFIER = '410B747D-9FCB-440F-98FE-981389DE33B3',
	@dcSalTcTermsProbCondPropertyName NVARCHAR(MAX) = 'extendedcase_ProbationPeriod',
	@dcSalTcTermsProbCondOperator NVARCHAR(MAX) = 'Equal',
	@dcSalTcTermsProbCondValues NVARCHAR(MAX) = 'Yes',
	@dcSalTcTermsProbCondDescription NVARCHAR(MAX) = 'Has probation period',
	@dcSalTcTermsProbCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcSalTcTermsProbCondGuid)
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
		@dcSalTcTermsProbCondGuid,
		@dcSalTcTermsProbID,
		@dcSalTcTermsProbCondPropertyName,
		@dcSalTcTermsProbCondOperator,
		@dcSalTcTermsProbCondValues,
		@dcSalTcTermsProbCondDescription,
		@dcSalTcTermsProbCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcSalTcTermsProbID,
		Property_Name = @dcSalTcTermsProbCondPropertyName,
		Operator = @dcSalTcTermsProbCondOperator,
		[Values] = @dcSalTcTermsProbCondValues,
		[Description] = @dcSalTcTermsProbCondDescription,
		[Status] = @dcSalTcTermsProbCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcSalTcTermsProbCondGuid
END


-- #################################### Performance Management
DECLARE @dcSalTcTermsPerfGuid UNIQUEIDENTIFIER = '7B555926-E16C-4577-A2CB-5910351298D5',
	@dcSalTcTermsPerfName NVARCHAR(MAX) = @prefix + ' Performance',
	@dcSalTcTermsPerfDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsPerfText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November.  However, your first review will be conducted during your six (6)-month probationary period.  This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@dcSalTcTermsPerfHeadline NVARCHAR(MAX) = 'Performance Management',
	@dcSalTcTermsPerfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsPerfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsPerfName, 
		@dcSalTcTermsPerfDescription,
		@dcSalTcTermsPerfText, 
		@dcSalTcTermsPerfHeadline,
		@dcSalTcTermsPerfSortOrder,
		@dcSalTcTermsPerfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsPerfName, 
		[Description] = @dcSalTcTermsPerfDescription, 
		[Text] = @dcSalTcTermsPerfText,
		[Headline] = @dcSalTcTermsPerfHeadline,
		SortoRder = @dcSalTcTermsPerfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsPerfGuid
END

-- #################################### Remuneration Review

DECLARE @dcSalTcTermsRemunRevAGuid UNIQUEIDENTIFIER = 'bfc9980b-fee7-4d92-9399-5f25bea27130',
	@dcSalTcTermsRemunRevAName NVARCHAR(MAX) = @prefix + ' Remuneration Review',
	@dcSalTcTermsRemunRevADescription NVARCHAR(MAX) = '',
	@dcSalTcTermsRemunRevAText NVARCHAR(MAX) = 'In line with IKEA’s Remuneration Policy, your Total Remuneration package will be reviewed annually following your performance review.  Any increase in your total remuneration package will take effect from the next pay cycle.<br>
<br>
The earliest your Total Remuneration package will be reviewed will be in January <Next Salary Review Year>.',
	@dcSalTcTermsRemunRevAHeadline NVARCHAR(MAX) = 'Remuneration Review',
	@dcSalTcTermsRemunRevASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsRemunRevAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsRemunRevAName, 
		@dcSalTcTermsRemunRevADescription,
		@dcSalTcTermsRemunRevAText, 
		@dcSalTcTermsRemunRevAHeadline,
		@dcSalTcTermsRemunRevASortOrder,
		@dcSalTcTermsRemunRevAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsRemunRevAName, 
		[Description] = @dcSalTcTermsRemunRevADescription, 
		[Text] = @dcSalTcTermsRemunRevAText,
		[Headline] = @dcSalTcTermsRemunRevAHeadline,
		SortoRder = @dcSalTcTermsRemunRevASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsRemunRevAGuid
END


-- #################################### Confidential Information
DECLARE @dcSalTcTermsConfGuid UNIQUEIDENTIFIER = 'A7BFF483-332F-4B00-A4BA-D5B55C0E28F2',
	@dcSalTcTermsConfName NVARCHAR(MAX) = @prefix + ' Confidential Information',
	@dcSalTcTermsConfDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsConfText NVARCHAR(MAX) = 'In the course of your employment, you may be exposed to “Confidential Information” concerning IKEA. Confidential Information means any information obtained by you in the course of your employment, including:
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
	@dcSalTcTermsConfHeadline NVARCHAR(MAX) = 'Confidential Information',
	@dcSalTcTermsConfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsConfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsConfName, 
		@dcSalTcTermsConfDescription,
		@dcSalTcTermsConfText, 
		@dcSalTcTermsConfHeadline,
		@dcSalTcTermsConfSortOrder,
		@dcSalTcTermsConfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsConfName, 
		[Description] = @dcSalTcTermsConfDescription, 
		[Text] = @dcSalTcTermsConfText,
		[Headline] = @dcSalTcTermsConfHeadline,
		SortOrder = @dcSalTcTermsConfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsConfGuid
END

-- #################################### Leave Entitlements 

DECLARE @dcSalTcTermsLeaveGuid UNIQUEIDENTIFIER = '81A42F7D-D872-467F-8A73-FB2E6E84E0EC',
	@dcSalTcTermsLeaveName NVARCHAR(MAX) = @prefix + ' Leave',
	@dcSalTcTermsLeaveDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsLeaveText NVARCHAR(MAX) = 'You will accrue entitlements to leave in accordance with relevant legislation and company policy.  This currently includes annual leave (4 weeks per annum, excluding annual leave loading), personal leave (10 days per annum) to be used for absence due to personal illness or to care for a member of your immediate family, parental leave and long service leave.  Company policy may change at any time at IKEA’s sole discretion.<br>
<br>
Annual leave is ordinarily to be taken within the year it is accrued, or within 12 months from the date it becomes due.  Annual leave is to be taken at times mutually agreed to, taking into consideration peak periods in business operations, which may vary from year to year.  Peak periods may be such that no annual leave will be authorised during those periods.  Peak periods can be identified in consultation with your manager.',
	@dcSalTcTermsLeaveHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@dcSalTcTermsLeaveSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsLeaveGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsLeaveName, 
		@dcSalTcTermsLeaveDescription,
		@dcSalTcTermsLeaveText, 
		@dcSalTcTermsLeaveHeadline,
		@dcSalTcTermsLeaveSortOrder,
		@dcSalTcTermsLeaveGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsLeaveName, 
		[Description] = @dcSalTcTermsLeaveDescription, 
		[Text] = @dcSalTcTermsLeaveText,
		[Headline] = @dcSalTcTermsLeaveHeadline,
		SortOrder = @dcSalTcTermsLeaveSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsLeaveGuid
END

-- #################################### Issues Resolution
DECLARE @dcSalTcTermsIssuesGuid UNIQUEIDENTIFIER = '5E11F757-4703-46E1-964D-D40EBD8FCF95',
	@dcSalTcTermsIssuesName NVARCHAR(MAX) = @prefix + ' Issues Resolution',
	@dcSalTcTermsIssuesDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsIssuesText NVARCHAR(MAX) = 'If any issues arise during your employment with IKEA, the matter should initially be discussed with your immediate manager, in accordance with IKEA’s Issue Resolution Procedure.  If the problem remains unresolved, you may refer it to more senior levels of management for further discussion in accordance with the Issue Resolution Procedure.',
	@dcSalTcTermsIssuesHeadline NVARCHAR(MAX) = 'Issues Resolution',
	@dcSalTcTermsIssuesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsIssuesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsIssuesName, 
		@dcSalTcTermsIssuesDescription,
		@dcSalTcTermsIssuesText, 
		@dcSalTcTermsIssuesHeadline,
		@dcSalTcTermsIssuesSortOrder,
		@dcSalTcTermsIssuesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsIssuesName, 
		[Description] = @dcSalTcTermsIssuesDescription, 
		[Text] = @dcSalTcTermsIssuesText,
		[Headline] = @dcSalTcTermsIssuesHeadline,
		SortOrder = @dcSalTcTermsIssuesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsIssuesGuid
END

-- #################################### Equal Employment Opportunity 
DECLARE @dcSalTcTermsEqualGuid UNIQUEIDENTIFIER = 'F21C5B1D-B7D4-47B6-AC3F-10B1D5AA93A4',
	@dcSalTcTermsEqualName NVARCHAR(MAX) = @prefix + ' Equal Employment',
	@dcSalTcTermsEqualDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsEqualText NVARCHAR(MAX) = 'IKEA''s policy is to provide all co-workers with equal opportunity.  This policy precludes discrimination and harassment based on, but not limited to, race, colour, religion, gender, age, marital status and disability.  You are required to familiarise yourself with this policy and comply with it at all times.',
	@dcSalTcTermsEqualHeadline NVARCHAR(MAX) = 'Equal Employment Opportunity ',
	@dcSalTcTermsEqualSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsEqualGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsEqualName, 
		@dcSalTcTermsEqualDescription,
		@dcSalTcTermsEqualText, 
		@dcSalTcTermsEqualHeadline,
		@dcSalTcTermsEqualSortOrder,
		@dcSalTcTermsEqualGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsEqualName, 
		[Description] = @dcSalTcTermsEqualDescription, 
		[Text] = @dcSalTcTermsEqualText,
		[Headline] = @dcSalTcTermsEqualHeadline,
		SortOrder = @dcSalTcTermsEqualSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsEqualGuid
END

-- #################################### Appearance & Conduct
DECLARE @dcSalTcTermsAppearGuid UNIQUEIDENTIFIER = '2516E05A-08CD-46C7-B473-84376ADCDD0B',
	@dcSalTcTermsAppearName NVARCHAR(MAX) = @prefix + ' Appearance & Conduct',
	@dcSalTcTermsAppearDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsAppearText NVARCHAR(MAX) = 'IKEA has established guidelines necessary for the professional appearance that the company expects all co-workers to present, and as such co-workers are supplied with uniforms and name badges within these guidelines.<br>
	<br>
Co-workers are expected to project a favorable and professional image for IKEA, and are to be courteous, efficient and reliable in their dealings with colleagues, existing and potential customers and suppliers to IKEA.',
	@dcSalTcTermsAppearHeadline NVARCHAR(MAX) = 'Appearance & Conduct',
	@dcSalTcTermsAppearSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsAppearGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsAppearName, 
		@dcSalTcTermsAppearDescription,
		@dcSalTcTermsAppearText, 
		@dcSalTcTermsAppearHeadline,
		@dcSalTcTermsAppearSortOrder,
		@dcSalTcTermsAppearGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsAppearName, 
		[Description] = @dcSalTcTermsAppearDescription, 
		[Text] = @dcSalTcTermsAppearText,
		[Headline] = @dcSalTcTermsAppearHeadline,
		SortOrder = @dcSalTcTermsAppearSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsAppearGuid
END

-- #################################### Induction & Ongoing Learning & Development
DECLARE @dcSalTcTermsInductionGuid UNIQUEIDENTIFIER = 'D0A8DF43-C0FA-47B7-AAD5-F078C30B3FFB',
	@dcSalTcTermsInductionName NVARCHAR(MAX) = @prefix + ' Induction',
	@dcSalTcTermsInductionDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsInductionText NVARCHAR(MAX) = 'IKEA is committed to your induction and ongoing development and as such, has a requirement that you undertake and are committed to training as offered by the company.  Whilst the majority of training is conducted on the job, you may be required from time to time to attend external training programs at different locations as organised by the company.<br>
<br>
IKEA encourages its co-workers to take responsibility for their own learning and development.',
	@dcSalTcTermsInductionHeadline NVARCHAR(MAX) = 'Induction & Ongoing Learning & Development',
	@dcSalTcTermsInductionSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsInductionGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsInductionName, 
		@dcSalTcTermsInductionDescription,
		@dcSalTcTermsInductionText, 
		@dcSalTcTermsInductionHeadline,
		@dcSalTcTermsInductionSortOrder,
		@dcSalTcTermsInductionGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsInductionName, 
		[Description] = @dcSalTcTermsInductionDescription, 
		[Text] = @dcSalTcTermsInductionText,
		[Headline] = @dcSalTcTermsInductionHeadline,
		SortOrder = @dcSalTcTermsInductionSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsInductionGuid
END

-- #################################### Occupational Health & Safety
DECLARE @dcSalTcTermsSafetyGuid UNIQUEIDENTIFIER = 'BC958CF2-2280-407F-9647-89B553E992A8',
	@dcSalTcTermsSafetyName NVARCHAR(MAX) = @prefix + ' Safety',
	@dcSalTcTermsSafetyDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsSafetyText NVARCHAR(MAX) = 'IKEA understands the requirement of ensuring a safe and healthy working environment for all co-workers in its offices, warehouses and stores, and a safe and healthy shopping environment for customers.  In fulfilling this aim, we undertake regular consultation with co-workers on health and safety issues and concerns.',
	@dcSalTcTermsSafetyHeadline NVARCHAR(MAX) = 'Occupational Health & Safety',
	@dcSalTcTermsSafetySortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsSafetyGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsSafetyName, 
		@dcSalTcTermsSafetyDescription,
		@dcSalTcTermsSafetyText, 
		@dcSalTcTermsSafetyHeadline,
		@dcSalTcTermsSafetySortOrder,
		@dcSalTcTermsSafetyGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsSafetyName, 
		[Description] = @dcSalTcTermsSafetyDescription, 
		[Text] = @dcSalTcTermsSafetyText,
		[Headline] = @dcSalTcTermsSafetyHeadline,
		SortOrder = @dcSalTcTermsSafetySortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsSafetyGuid
END

-- #################################### Termination
DECLARE @dcSalTcTermsTerminationGuid UNIQUEIDENTIFIER = '9D38D202-8472-4EFF-9E63-C0DDA69BB705',
	@dcSalTcTermsTerminationName NVARCHAR(MAX) = @prefix + ' Termination',
	@dcSalTcTermsTerminationDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsTerminationText NVARCHAR(MAX) = 'IKEA may terminate your employment by giving four (4) weeks’ notice or payment in lieu at your ordinary rate of pay.  If you are over 45 years of age and have at least two years’ continuous employment with IKEA, you will be entitled to an additional week’s notice.<br>
<br>
If you wish to resign, you must provide IKEA with four (4) weeks’ notice.  If you fail to give the appropriate notice to IKEA, IKEA shall have the right to withhold any monies due to you up to a maximum of your ordinary rate of pay for the shortfall in period of notice not served. IKEA may at its election not require you to attend the workplace during the notice period.<br>
<br>
Notices of resignation or termination must be supplied in writing, and must comply with the above named notice periods unless a new period is agreed to in writing between you and IKEA.  A failure on your part to resign in writing will not affect the validity of your resignation.<br>
<br>
IKEA retains the right to terminate your employment without notice in the case of summary dismissal.<br>
<br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee. IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.<br>',
	@dcSalTcTermsTerminationHeadline NVARCHAR(MAX) = 'Termination',
	@dcSalTcTermsTerminationSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsTerminationGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsTerminationName, 
		@dcSalTcTermsTerminationDescription,
		@dcSalTcTermsTerminationText, 
		@dcSalTcTermsTerminationHeadline,
		@dcSalTcTermsTerminationSortOrder,
		@dcSalTcTermsTerminationGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsTerminationName, 
		[Description] = @dcSalTcTermsTerminationDescription, 
		[Text] = @dcSalTcTermsTerminationText,
		[Headline] = @dcSalTcTermsTerminationHeadline,
		SortOrder = @dcSalTcTermsTerminationSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsTerminationGuid
END

-- #################################### Company Policies & Procedures 
DECLARE @dcSalTcTermsPoliciesGuid UNIQUEIDENTIFIER = '2ABFDEDD-2522-4F1B-A0DC-5A6D86A402E3',
	@dcSalTcTermsPoliciesName NVARCHAR(MAX) = @prefix + ' Policies',
	@dcSalTcTermsPoliciesDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsPoliciesText NVARCHAR(MAX) = 'You shall be required to comply with all Company Policies and Procedures as advised to you and as outlined in IKEA’s Policy Guidelines. These Policies and Procedures may be subject to change/amendment from time to time, and form part of your contract of employment.',
	@dcSalTcTermsPoliciesHeadline NVARCHAR(MAX) = 'Company Policies & Procedures',
	@dcSalTcTermsPoliciesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsPoliciesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsPoliciesName, 
		@dcSalTcTermsPoliciesDescription,
		@dcSalTcTermsPoliciesText, 
		@dcSalTcTermsPoliciesHeadline,
		@dcSalTcTermsPoliciesSortOrder,
		@dcSalTcTermsPoliciesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsPoliciesName, 
		[Description] = @dcSalTcTermsPoliciesDescription, 
		[Text] = @dcSalTcTermsPoliciesText,
		[Headline] = @dcSalTcTermsPoliciesHeadline,
		SortOrder = @dcSalTcTermsPoliciesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsPoliciesGuid
END


-- #################################### Communications with Media
DECLARE @dcSalTcTermsMediaGuid UNIQUEIDENTIFIER = '5253CC1A-B10C-49AB-8B4F-4F94FAD1E685',
	@dcSalTcTermsMediaName NVARCHAR(MAX) = @prefix + ' Media',
	@dcSalTcTermsMediaDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsMediaText NVARCHAR(MAX) = 'You shall not provide information or speak on behalf of IKEA or otherwise to the media on any matters concerning IKEA’s business or activities.  You must refer all requests from the media for information and/or interviews to your immediate Manager.',
	@dcSalTcTermsMediaHeadline NVARCHAR(MAX) = 'Communications with Media',
	@dcSalTcTermsMediaSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsMediaGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsMediaName, 
		@dcSalTcTermsMediaDescription,
		@dcSalTcTermsMediaText, 
		@dcSalTcTermsMediaHeadline,
		@dcSalTcTermsMediaSortOrder,
		@dcSalTcTermsMediaGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsMediaName, 
		[Description] = @dcSalTcTermsMediaDescription, 
		[Text] = @dcSalTcTermsMediaText,
		[Headline] = @dcSalTcTermsMediaHeadline,
		SortOrder = @dcSalTcTermsMediaSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsMediaGuid
END


-- #################################### Obligation to report unlawful activities
DECLARE @dcSalTcTermsUnlawGuid UNIQUEIDENTIFIER = '53164802-5B17-4E23-ABBC-A14AF1D70F80',
	@dcSalTcTermsUnlawName NVARCHAR(MAX) = @prefix + ' Unlawful',
	@dcSalTcTermsUnlawDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsUnlawText NVARCHAR(MAX) = 'If you become aware of or suspect any unlawful act or omission by any IKEA employee, you must advise IKEA immediately.',
	@dcSalTcTermsUnlawHeadline NVARCHAR(MAX) = 'Obligation to report unlawful activities',
	@dcSalTcTermsUnlawSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsUnlawGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsUnlawName, 
		@dcSalTcTermsUnlawDescription,
		@dcSalTcTermsUnlawText, 
		@dcSalTcTermsUnlawHeadline,
		@dcSalTcTermsUnlawSortOrder,
		@dcSalTcTermsUnlawGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsUnlawName, 
		[Description] = @dcSalTcTermsUnlawDescription, 
		[Text] = @dcSalTcTermsUnlawText,
		[Headline] = @dcSalTcTermsUnlawHeadline,
		SortOrder = @dcSalTcTermsUnlawSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsUnlawGuid
END

-- #################################### Intellectual Property
DECLARE @dcSalTcTermsIntelPropGuid UNIQUEIDENTIFIER = '3FFEE114-B1C0-4459-8172-0698AD0CDB53',
	@dcSalTcTermsIntelPropName NVARCHAR(MAX) = @prefix + ' Int. Property',
	@dcSalTcTermsIntelPropDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsIntelPropText NVARCHAR(MAX) = 'IKEA owns all copyright in any works and all inventions, discoveries, novel designs, improvements or modifications, computer program material and trademarks which you write or develop in the course of your employment (in or out of working hours) (“Intellectual Property”).<br>
<br>
You assign to IKEA any interest you have in the Intellectual Property, and you must disclose any Intellectual Property to IKEA.<br>
<br>
During and after your employment, you must do anything IKEA reasonably requires (at IKEA''s cost) to:
<ul>
<li>obtain statutory protection (including by patent, design registration, trade mark registration or copyright) for the Intellectual Property for IKEA in any country; or</li>
<li>Perfect or evidence IKEA’s ownership of the Intellectual Property.</li>
</ul>',

	@dcSalTcTermsIntelPropHeadline NVARCHAR(MAX) = 'Intellectual Property',
	@dcSalTcTermsIntelPropSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsIntelPropGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsIntelPropName, 
		@dcSalTcTermsIntelPropDescription,
		@dcSalTcTermsIntelPropText, 
		@dcSalTcTermsIntelPropHeadline,
		@dcSalTcTermsIntelPropSortOrder,
		@dcSalTcTermsIntelPropGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsIntelPropName, 
		[Description] = @dcSalTcTermsIntelPropDescription, 
		[Text] = @dcSalTcTermsIntelPropText,
		[Headline] = @dcSalTcTermsIntelPropHeadline,
		SortOrder = @dcSalTcTermsIntelPropSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsIntelPropGuid
END

-- #################################### Suspension
DECLARE @dcSalTcTermsSuspGuid UNIQUEIDENTIFIER = '0553635E-0832-4ECA-AD53-20D6D1E2DEB2',
	@dcSalTcTermsSuspName NVARCHAR(MAX) = @prefix + ' Suspension',
	@dcSalTcTermsSuspDescription NVARCHAR(MAX) = '',
	@dcSalTcTermsSuspText NVARCHAR(MAX) = 'If we have reason to believe that you may have engaged in a serious breach of your employment obligations, IKEA may at its discretion suspend you from your duties, either with or without pay, while an investigation is conducted.',
	@dcSalTcTermsSuspHeadline NVARCHAR(MAX) = 'Suspension',
	@dcSalTcTermsSuspSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcTermsSuspGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcTermsID, 
		@dcSalTcTermsSuspName, 
		@dcSalTcTermsSuspDescription,
		@dcSalTcTermsSuspText, 
		@dcSalTcTermsSuspHeadline,
		@dcSalTcTermsSuspSortOrder,
		@dcSalTcTermsSuspGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcTermsID,
		[Name] = @dcSalTcTermsSuspName, 
		[Description] = @dcSalTcTermsSuspDescription, 
		[Text] = @dcSalTcTermsSuspText,
		[Headline] = @dcSalTcTermsSuspHeadline,
		SortOrder = @dcSalTcTermsSuspSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcTermsSuspGuid
END

-- Add terms paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @dcSalTcTermsID, @counter
SET @counter = @counter + 1


-- #################################### End Text
DECLARE @dcSalTcEndTextParagraphGuid UNIQUEIDENTIFIER = '8327AE93-C886-4E99-B9B7-835C6B149EC1',
	@dcSalTcEndTextParagraphName NVARCHAR(MAX) = @prefix + ' End Text',
	@dcSalTcEndTextParagraphType INT = @ParagraphTypeText,
	@dcSalTcEndTextParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalTcEndTextParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalTcEndTextParagraphName, @dcSalTcEndTextParagraphDescription, @dcSalTcEndTextParagraphType, @dcSalTcEndTextParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalTcEndTextParagraphName, [Description] = @dcSalTcEndTextParagraphDescription, ParagraphType = @dcSalTcEndTextParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalTcEndTextParagraphGuid
END
DECLARE @dcSalTcEndTextParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalTcEndTextParagraphGuid)

-- Create a text field
DECLARE @dcSalTcEndTextGuid UNIQUEIDENTIFIER = 'CAC83FC1-C961-4A3E-986D-E0A3B0FB17AC',
	@dcSalTcEndTextName NVARCHAR(MAX) = @prefix + ' End Text',
	@dcSalTcEndTextDescription NVARCHAR(MAX) = '',
	@dcSalTcEndTextText NVARCHAR(MAX) = 'IKEA recognises that its co-workers are essential to the success of the company’s operations.  IKEA remains committed to ensuring that all co-workers are treated fairly and equitably and encourages co-workers to reach their full potential.  We believe that the basis of your employment outlined above, will achieve these objectives and greatly benefit those co-workers willing to develop themselves.
	<br><br>
	As an indication of your understanding and acceptance of these conditions, please sign this letter of offer, and return to the undersigned within seven (7) days.  Please retain the second copy for your records.
	<br><br>
	If you have any questions pertaining to this offer of employment or any of the information contained herein, please do not hesitate to contact me before signing this letter.',
	@dcSalTcEndTextHeadline NVARCHAR(MAX) = '',
	@dcSalTcEndTextSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcEndTextGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcEndTextParagraphID, 
		@dcSalTcEndTextName, 
		@dcSalTcEndTextDescription,
		@dcSalTcEndTextText, 
		@dcSalTcEndTextHeadline,
		@dcSalTcEndTextSortOrder,
		@dcSalTcEndTextGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcEndTextParagraphID,
		[Name] = @dcSalTcEndTextName, 
		[Description] = @dcSalTcEndTextDescription, 
		[Text] = @dcSalTcEndTextText,
		[Headline] = @dcSalTcEndTextHeadline,
		SortOrder = @dcSalTcEndTextSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcEndTextGuid
END

-- Add end text paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @dcSalTcEndTextParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Contractor Signature
DECLARE @dcSalTcConSignParagraphGuid UNIQUEIDENTIFIER = 'A2B87461-EB4D-4FBC-8D41-B5243AB67944',
	@dcSalTcConSignParagraphName NVARCHAR(MAX) = @prefix + ' Con. Sign.',
	@dcSalTcConSignParagraphType INT = @ParagraphTypeText,
	@dcSalTcConSignParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalTcConSignParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalTcConSignParagraphName, @dcSalTcConSignParagraphDescription, @dcSalTcConSignParagraphType, @dcSalTcConSignParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalTcConSignParagraphName, [Description] = @dcSalTcConSignParagraphDescription, ParagraphType = @dcSalTcConSignParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalTcConSignParagraphGuid
END
DECLARE @dcSalTcConSignParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalTcConSignParagraphGuid)

-- Create a text field
DECLARE @dcSalTcConSignGuid UNIQUEIDENTIFIER = '877E3C5B-141B-46E2-9A56-B1259817B833',
	@dcSalTcConSignName NVARCHAR(MAX) = @prefix + ' Con. Sign.',
	@dcSalTcConSignDescription NVARCHAR(MAX) = '',
	@dcSalTcConSignText NVARCHAR(MAX) = 'Yours sincerely<br>
	<Reports to Line Manager><br>
	<Position Title (Local Job Name) of Reports To Line Manager><br>
	<strong>IKEA Distribution Services Australia Pty Ltd</strong>',
	@dcSalTcConSignHeadline NVARCHAR(MAX) = '',
	@dcSalTcConSignSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcConSignGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcConSignParagraphID, 
		@dcSalTcConSignName, 
		@dcSalTcConSignDescription,
		@dcSalTcConSignText, 
		@dcSalTcConSignHeadline,
		@dcSalTcConSignSortOrder,
		@dcSalTcConSignGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcConSignParagraphID,
		[Name] = @dcSalTcConSignName, 
		[Description] = @dcSalTcConSignDescription, 
		[Text] = @dcSalTcConSignText,
		[Headline] = @dcSalTcConSignHeadline,
		SortOrder = @dcSalTcConSignSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcConSignGuid
END

-- Add contractor signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @dcSalTcConSignParagraphID, @counter
SET @counter = @counter + 1

-- #################################### 41-48 Acceptance
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcSalTcAcceptParagraphGuid UNIQUEIDENTIFIER = '5F3474F9-C991-4E07-B386-7C02B6D73DF0',
	@dcSalTcAcceptParagraphName NVARCHAR(MAX) = @prefix + ' Employment - Acceptance',
	@dcSalTcAcceptParagraphType INT = @ParagraphTypeText,
	@dcSalTcAcceptParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcSalTcAcceptParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcSalTcAcceptParagraphName, @dcSalTcAcceptParagraphDescription, @dcSalTcAcceptParagraphType, @dcSalTcAcceptParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcSalTcAcceptParagraphName, [Description] = @dcSalTcAcceptParagraphDescription, ParagraphType = @dcSalTcAcceptParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcSalTcAcceptParagraphGuid
END
DECLARE @dcSalTcAcceptParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcSalTcAcceptParagraphGuid)

-- Create a text field
DECLARE @dcSalTcAcceptGuid UNIQUEIDENTIFIER = '224E290B-E833-4823-B0A6-DDCFF34C65A1',
	@dcSalTcAcceptName NVARCHAR(MAX) = @prefix + ' Employment - Acceptance',
	@dcSalTcAcceptDescription NVARCHAR(MAX) = '',
	@dcSalTcAcceptText NVARCHAR(MAX) = '<table style="border: 1px solid black">
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
	@dcSalTcAcceptHeadline NVARCHAR(MAX) = '',
	@dcSalTcAcceptSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcSalTcAcceptGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcSalTcAcceptParagraphID, 
		@dcSalTcAcceptName, 
		@dcSalTcAcceptDescription,
		@dcSalTcAcceptText, 
		@dcSalTcAcceptHeadline,
		@dcSalTcAcceptSortOrder,
		@dcSalTcAcceptGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcSalTcAcceptParagraphID,
		[Name] = @dcSalTcAcceptName, 
		[Description] = @dcSalTcAcceptDescription, 
		[Text] = @dcSalTcAcceptText,
		[Headline] = @dcSalTcAcceptHeadline,
		SortOrder = @dcSalTcAcceptSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcSalTcAcceptGuid
END

-- Add acceptance paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcSalTcID, @dcSalTcAcceptParagraphID, @counter
SET @counter = @counter + 1




-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @dcSalTcGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



ROLLBACK

