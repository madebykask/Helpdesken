--########################################
--########## DC EA Hiring ###################
--########################################

BEGIN TRAN


-- #################################### General paragraphs & values ####################################

-- Get Logo info
DECLARE @logoGuid UNIQUEIDENTIFIER = 'EB0434AA-0BBF-4CA8-AB0A-BF853129FB9D'
DECLARE @logoID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @logoGuid)

-- Get footer info with initials
DECLARE @footerWithInitialsGuid UNIQUEIDENTIFIER = 'A7626F89-C428-475C-8E10-160CCE0F2B5D'
DECLARE @footerWithInitialsID INT = (SELECT ID FROM tblCaseDocumentParagraph CP WHERE CP.CaseDocumentParagraphGUID = @footerWithInitialsGuid)

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

DECLARE @prefix NVARCHAR(MAX) = 'DC EA HIR'

-- #################################### Contract Clusters – DC EA (Hiring) ####################################

-- Get the form
DECLARE @dcHiringGuid UNIQUEIDENTIFIER = '33216A30-3CE1-40CE-8C15-40D2F364B547'
DECLARE @dcHiringID INT, @counter INT = 0
SELECT @dcHiringID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @dcHiringGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @dcHiringGuid

-- #################################### Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer with initials
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @footerWithInitialsID, @counter
SET @counter = @counter + 1

-- #################################### Header

---- Create or update paragraph
-- Paragraph guid
DECLARE @dcHiringHeaderGuid UNIQUEIDENTIFIER = '6D4E591C-92D5-4ED5-B4BE-5108F2405F91',
	@dcHiringHeaderName NVARCHAR(MAX) = @prefix + ' Header',
	@dcHiringHeaderParagraphType INT = @ParagraphTypeText,
	@dcHiringHeaderDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcHiringHeaderGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcHiringHeaderName, @dcHiringHeaderDescription, @dcHiringHeaderParagraphType, @dcHiringHeaderGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcHiringHeaderName, [Description] = @dcHiringHeaderDescription, ParagraphType = @dcHiringHeaderParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcHiringHeaderGuid
END
DECLARE @dcHiringHeaderID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcHiringHeaderGuid)

---- Create or update text A. Company info
DECLARE @dcHiringHeaderTextAGuid UNIQUEIDENTIFIER = 'C4BC8899-4704-47C2-88ED-120DDFC36DE3',
	@dcHiringHeaderTextAName NVARCHAR(MAX) = @prefix + ' Header, Company',
	@dcHiringHeaderTextADescription NVARCHAR(MAX) = '',
	@dcHiringHeaderTextAText NVARCHAR(MAX) = '<p style="text-align:left;">IKEA Distribution Services Australia Pty Ltd</p>	
<p>ABN 96 001 264 179</p>',
	@dcHiringHeaderTextAHeadline NVARCHAR(MAX) = '',
	@dcHiringHeaderTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringHeaderTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringHeaderID, 
		@dcHiringHeaderTextAName, 
		@dcHiringHeaderTextADescription,
		@dcHiringHeaderTextAText, 
		@dcHiringHeaderTextAHeadline,
		@dcHiringHeaderTextASortOrder,
		@dcHiringHeaderTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringHeaderID,
		[Name] = @dcHiringHeaderTextAName, 
		[Description] = @dcHiringHeaderTextADescription, 
		[Text] = @dcHiringHeaderTextAText,
		[Headline] = @dcHiringHeaderTextAHeadline,
		SortOrder = @dcHiringHeaderTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringHeaderTextAGuid
END
---- Create or update text B. Co-worker info
DECLARE @dcHiringHeaderTextBGuid UNIQUEIDENTIFIER = 'E79BBDDB-E17B-4B4D-BD36-56FE5C7C1E00',
	@dcHiringHeaderTextBName NVARCHAR(MAX) = @prefix + ' Header, Co-worker',
	@dcHiringHeaderTextBDescription NVARCHAR(MAX) = '',
	@dcHiringHeaderTextBText NVARCHAR(MAX) = '<p><Todays Date - Long></p>
		<p><strong><Co-worker First Name> <Co-worker Last Name></strong></p>
		<p><Address Line 1><br />
		<Address Line 2> <State> <Postal Code><br />
		<Address Line 3><br />
		<br /><br />
		Dear <Co-worker First Name>,</p>',
	@dcHiringHeaderTextBHeadline NVARCHAR(MAX) = '',
	@dcHiringHeaderTextBSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringHeaderTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringHeaderID, 
		@dcHiringHeaderTextBName, 
		@dcHiringHeaderTextBDescription,
		@dcHiringHeaderTextBText, 
		@dcHiringHeaderTextBHeadline,
		@dcHiringHeaderTextBSortOrder,
		@dcHiringHeaderTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringHeaderID,
		[Name] = @dcHiringHeaderTextBName, 
		[Description] = @dcHiringHeaderTextBDescription, 
		[Text] = @dcHiringHeaderTextBText,
		[Headline] = @dcHiringHeaderTextBHeadline,
		SortOrder = @dcHiringHeaderTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringHeaderTextBGuid
END

-- Add header paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @dcHiringHeaderID, @counter
SET @counter = @counter + 1



-- #################################### Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @dcHiringEmployGreetingGuid UNIQUEIDENTIFIER = 'ac3e675e-79dc-4045-87d3-69e52c07aee5',
	@cdpName NVARCHAR(MAX) = @prefix + ' Greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcHiringEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @dcHiringEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcHiringEmployGreetingGuid
END
DECLARE @dcHiringEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcHiringEmployGreetingGuid)

---- Create or update text A, Full Time
DECLARE @dcHiringEmployGreetingTextAGuid UNIQUEIDENTIFIER = 'e5941d9d-8de3-4472-8409-f1d3e25797fa',
	@dcHiringEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Greeting, full time',
	@dcHiringEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@dcHiringEmployGreetingTextAText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Full Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcHiringEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@dcHiringEmployGreetingTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringEmployGreetingID, 
		@dcHiringEmployGreetingTextAName, 
		@dcHiringEmployGreetingTextADescription,
		@dcHiringEmployGreetingTextAText, 
		@dcHiringEmployGreetingTextAHeadline,
		@dcHiringEmployGreetingTextASortOrder,
		@dcHiringEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringEmployGreetingID,
		[Name] = @dcHiringEmployGreetingTextAName, 
		[Description] = @dcHiringEmployGreetingTextADescription, 
		[Text] = @dcHiringEmployGreetingTextAText,
		[Headline] = @dcHiringEmployGreetingTextAHeadline,
		SortOrder = @dcHiringEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringEmployGreetingTextAGuid
END
DECLARE @dcHiringEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @dcHiringEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = '2247139F-6294-4CAB-BF62-5C2F828DAC77',
	@dcHiringEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'Equal',
	@dcHiringEmployGreetingTextACondAValues NVARCHAR(MAX) = '76',
	@dcHiringEmployGreetingTextACondADescription NVARCHAR(MAX) = 'Is full time',
	@dcHiringEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringEmployGreetingTextACondAGuid)
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
		@dcHiringEmployGreetingTextACondAGuid,
		@dcHiringEmployGreetingTextAID,
		@dcHiringEmployGreetingTextACondAPropertyName,
		@dcHiringEmployGreetingTextACondAOperator,
		@dcHiringEmployGreetingTextACondAValues,
		@dcHiringEmployGreetingTextACondADescription,
		@dcHiringEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringEmployGreetingTextAID,
		Property_Name = @dcHiringEmployGreetingTextACondAPropertyName,
		Operator = @dcHiringEmployGreetingTextACondAOperator,
		[Values] = @dcHiringEmployGreetingTextACondAValues,
		[Description] = @dcHiringEmployGreetingTextACondADescription,
		[Status] = @dcHiringEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @dcHiringEmployGreetingTextBGuid UNIQUEIDENTIFIER = '6F56B42A-A052-4B98-A438-ECA5C30E0A3E',
	@dcHiringEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Greeting, part time',
	@dcHiringEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@dcHiringEmployGreetingTextBText NVARCHAR(MAX) = 'We are delighted to confirm that your application for the position of Part Time <Position Title (Local Job Name)> <Shift Type> Shift has been successful, and wish to confirm the terms and conditions of your employment.',
	@dcHiringEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@dcHiringEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringEmployGreetingID, 
		@dcHiringEmployGreetingTextBName, 
		@dcHiringEmployGreetingTextBDescription,
		@dcHiringEmployGreetingTextBText, 
		@dcHiringEmployGreetingTextBHeadline,
		@dcHiringEmployGreetingTextBSortOrder,
		@dcHiringEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringEmployGreetingID,
		[Name] = @dcHiringEmployGreetingTextBName, 
		[Description] = @dcHiringEmployGreetingTextBDescription, 
		[Text] = @dcHiringEmployGreetingTextBText,
		[Headline] = @dcHiringEmployGreetingTextBHeadline,
		SortOrder = @dcHiringEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringEmployGreetingTextBGuid
END
DECLARE @dcHiringEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @dcHiringEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = '518c7cbc-1a47-47cd-bf27-816f8b31c45e',
	@dcHiringEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@dcHiringEmployGreetingTextBCondAValues NVARCHAR(MAX) = '76',
	@dcHiringEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Is part time',
	@dcHiringEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringEmployGreetingTextBCondAGuid)
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
		@dcHiringEmployGreetingTextBCondAGuid,
		@dcHiringEmployGreetingTextBID,
		@dcHiringEmployGreetingTextBCondAPropertyName,
		@dcHiringEmployGreetingTextBCondAOperator,
		@dcHiringEmployGreetingTextBCondAValues,
		@dcHiringEmployGreetingTextBCondADescription,
		@dcHiringEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringEmployGreetingTextBID,
		Property_Name = @dcHiringEmployGreetingTextBCondAPropertyName,
		Operator = @dcHiringEmployGreetingTextBCondAOperator,
		[Values] = @dcHiringEmployGreetingTextBCondAValues,
		[Description] = @dcHiringEmployGreetingTextBCondADescription,
		[Status] = @dcHiringEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringEmployGreetingTextBCondAGuid
END


-- Add paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @dcHiringEmployGreetingID, @counter
SET @counter = @counter + 1

-- #################################### Terms

DECLARE @termsCounter INT = 0

---- Create or update a terms paragraph
-- Paragraph guid
DECLARE @dcHiringTermsGuid UNIQUEIDENTIFIER = '43375eaa-2efa-4646-9a22-d31983bf345c',
	@dcHiringTermsName NVARCHAR(MAX) = @prefix + ' Terms',
	@dcHiringTermsParagraphType INT = @ParagraphTypeTableNumeric,
	@termsDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcHiringTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcHiringTermsName, @termsDescription, @dcHiringTermsParagraphType, @dcHiringTermsGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcHiringTermsName, [Description] = @termsDescription, ParagraphType = @dcHiringTermsParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcHiringTermsGuid
END
DECLARE @dcHiringTermsID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcHiringTermsGuid)

-- #################################### Position

---- Position A
DECLARE @dcHiringTermsPositionAGuid UNIQUEIDENTIFIER = '7f5dc325-98b0-44b7-a812-562b0b368954',
	@dcHiringTermsPositionAName NVARCHAR(MAX) = @prefix + ' Position, full time',
	@dcHiringTermsPositionADescription NVARCHAR(MAX) = '',
	@dcHiringTermsPositionAText NVARCHAR(MAX) = 'Your position is Full Time <Position Title (Local Job Name)> <Shift Type> Shift, reporting to <Position Title (Local Job Name) of Reports To Line Manager>, which will be based at <Business Unit>. Your position (in terms of your duties & responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcHiringTermsPositionAHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcHiringTermsPositionASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsPositionAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsPositionAName, 
		@dcHiringTermsPositionADescription,
		@dcHiringTermsPositionAText, 
		@dcHiringTermsPositionAHeadline,
		@dcHiringTermsPositionASortOrder,
		@dcHiringTermsPositionAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsPositionAName, 
		[Description] = @dcHiringTermsPositionADescription, 
		[Text] = @dcHiringTermsPositionAText,
		[Headline] = @dcHiringTermsPositionAHeadline,
		SortOrder = @dcHiringTermsPositionASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPositionAGuid
END
DECLARE @dcHiringTermsPositionAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPositionAGuid)
SET @termsCounter = @termsCounter + 1

-- Create condition for position A
DECLARE @dcHiringTermsPositionACondGuid UNIQUEIDENTIFIER = 'a6340e59-a351-4323-ad34-023f20ff3427',
	@dcHiringTermsPositionACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringTermsPositionACondOperator NVARCHAR(MAX) = 'Equal',
	@dcHiringTermsPositionACondValues NVARCHAR(MAX) = '76',
	@dcHiringTermsPositionACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcHiringTermsPositionACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsPositionACondGuid)
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
		@dcHiringTermsPositionACondGuid,
		@dcHiringTermsPositionAID,
		@dcHiringTermsPositionACondPropertyName,
		@dcHiringTermsPositionACondOperator,
		@dcHiringTermsPositionACondValues,
		@dcHiringTermsPositionACondDescription,
		@dcHiringTermsPositionACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsPositionAID,
		Property_Name = @dcHiringTermsPositionACondPropertyName,
		Operator = @dcHiringTermsPositionACondOperator,
		[Values] = @dcHiringTermsPositionACondValues,
		[Description] = @dcHiringTermsPositionACondDescription,
		[Status] = @dcHiringTermsPositionACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsPositionACondGuid
END

---- Position B
DECLARE @dcHiringTermsPositionBGuid UNIQUEIDENTIFIER = 'fbec3308-d3bf-47d1-8dcc-48acb82b6b18',
	@dcHiringTermsPositionBName NVARCHAR(MAX) = @prefix + ' Position, part time',
	@dcHiringTermsPositionBDescription NVARCHAR(MAX) = '',
	@dcHiringTermsPositionBText NVARCHAR(MAX) = 'Your position is Part Time <Position Title (Local Job Name)> <Shift Type> Shift, reporting to <Position Title (Local Job Name) of Reports To Line Manager>, which will be based at <Business Unit>. Your position (in terms of your duties & responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.',
	@dcHiringTermsPositionBHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@dcHiringTermsPositionBSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsPositionBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsPositionBName, 
		@dcHiringTermsPositionBDescription,
		@dcHiringTermsPositionBText, 
		@dcHiringTermsPositionBHeadline,
		@dcHiringTermsPositionBSortOrder,
		@dcHiringTermsPositionBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsPositionBName, 
		[Description] = @dcHiringTermsPositionBDescription, 
		[Text] = @dcHiringTermsPositionBText,
		[Headline] = @dcHiringTermsPositionBHeadline,
		SortOrder = @dcHiringTermsPositionBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPositionBGuid
END
DECLARE @dcHiringTermsPositionBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPositionBGuid)

-- Create condition for position A
DECLARE @dcHiringTermsPositionBCondGuid UNIQUEIDENTIFIER = '9359e3cf-bc3f-44a4-8ee3-783e30574ee0',
	@dcHiringTermsPositionBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringTermsPositionBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcHiringTermsPositionBCondValues NVARCHAR(MAX) = '76',
	@dcHiringTermsPositionBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcHiringTermsPositionBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsPositionBCondGuid)
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
		@dcHiringTermsPositionBCondGuid,
		@dcHiringTermsPositionBID,
		@dcHiringTermsPositionBCondPropertyName,
		@dcHiringTermsPositionBCondOperator,
		@dcHiringTermsPositionBCondValues,
		@dcHiringTermsPositionBCondDescription,
		@dcHiringTermsPositionBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsPositionBID,
		Property_Name = @dcHiringTermsPositionBCondPropertyName,
		Operator = @dcHiringTermsPositionBCondOperator,
		[Values] = @dcHiringTermsPositionBCondValues,
		[Description] = @dcHiringTermsPositionBCondDescription,
		[Status] = @dcHiringTermsPositionBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsPositionBCondGuid
END

-- #################################### Commencement Date

---- Commencement A
DECLARE @dcHiringTermsComAGuid UNIQUEIDENTIFIER = 'a5af4973-c5b3-4993-92d3-5adbdccba9f7',
	@dcHiringTermsComAName NVARCHAR(MAX) = @prefix + ' Commencement, no date',
	@dcHiringTermsComADescription NVARCHAR(MAX) = '',
	@dcHiringTermsComAText NVARCHAR(MAX) = 'Your commencement date is <Contract Start Date>, unless otherwise terminated in accordance with this contract.',
	@dcHiringTermsComAHeadline NVARCHAR(MAX) = 'Commencement Date',
	@dcHiringTermsComASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsComAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsComAName, 
		@dcHiringTermsComADescription,
		@dcHiringTermsComAText, 
		@dcHiringTermsComAHeadline,
		@dcHiringTermsComASortOrder,
		@dcHiringTermsComAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsComAName, 
		[Description] = @dcHiringTermsComADescription, 
		[Text] = @dcHiringTermsComAText,
		[Headline] = @dcHiringTermsComAHeadline,
		SortOrder = @dcHiringTermsComASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsComAGuid
END
DECLARE @dcHiringTermsComAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsComAGuid)

-- Create condition for Commencement A
DECLARE @dcHiringTermsComACondAGuid UNIQUEIDENTIFIER = '26cd95bd-56ac-4f40-8c93-f14e6e2144e4',
	@dcHiringTermsComACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcHiringTermsComACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@dcHiringTermsComACondAValues NVARCHAR(MAX) = '',
	@dcHiringTermsComACondADescription NVARCHAR(MAX) = 'Has no end date',
	@dcHiringTermsComACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsComACondAGuid)
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
		@dcHiringTermsComACondAGuid,
		@dcHiringTermsComAID,
		@dcHiringTermsComACondAPropertyName,
		@dcHiringTermsComACondAOperator,
		@dcHiringTermsComACondAValues,
		@dcHiringTermsComACondADescription,
		@dcHiringTermsComACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsComAID,
		Property_Name = @dcHiringTermsComACondAPropertyName,
		Operator = @dcHiringTermsComACondAOperator,
		[Values] = @dcHiringTermsComACondAValues,
		[Description] = @dcHiringTermsComACondADescription,
		[Status] = @dcHiringTermsComACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsComACondAGuid
END

-- No support for 31.12.9999 yet
--DECLARE @dcHiringTermsComACondBGuid UNIQUEIDENTIFIER = 'e68e3e7c-52b0-4018-964b-99a1d9d471b9',
--	@dcHiringTermsComACondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
--	@dcHiringTermsComACondBOperator NVARCHAR(MAX) = 'Equal',
--	@dcHiringTermsComACondBValues NVARCHAR(MAX) = '31.12.9999',
--	@dcHiringTermsComACondBDescription NVARCHAR(MAX) = 'Equal 31.12.9999',
--	@dcHiringTermsComACondBStatus INT = 1

--IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsComACondBGuid)
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
--		@dcHiringTermsComACondBGuid,
--		@dcHiringTermsComAID,
--		@dcHiringTermsComACondBPropertyName,
--		@dcHiringTermsComACondBOperator,
--		@dcHiringTermsComACondBValues,
--		@dcHiringTermsComACondBDescription,
--		@dcHiringTermsComACondBStatus,
--		@now, 
--		@userID,
--		@now,
--		@userID
--	)
--END
--ELSE
--BEGIN
--	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsComAID,
--		Property_Name = @dcHiringTermsComACondBPropertyName,
--		Operator = @dcHiringTermsComACondBOperator,
--		[Values] = @dcHiringTermsComACondBValues,
--		[Description] = @dcHiringTermsComACondBDescription,
--		[Status] = @dcHiringTermsComACondBStatus,
--		CreatedDate = @now,
--		CreatedByUser_Id = @userID,
--		ChangedDate = @now,
--		ChangedByUser_Id = @userID
--	FROM tblCaseDocumentTextCondition CDTC
--	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsComACondBGuid
--END

---- Position B
DECLARE @dcHiringTermsComBGuid UNIQUEIDENTIFIER = '1292d3ea-7481-4a5f-b0f3-cb4f41bc4654',
	@dcHiringTermsComBName NVARCHAR(MAX) = @prefix + ' Commencement, has end date',
	@dcHiringTermsComBDescription NVARCHAR(MAX) = '',
	@dcHiringTermsComBText NVARCHAR(MAX) = 'Your commencement date is <Contract Start Date> and will cease on <Contract End Date>, unless otherwise terminated in accordance with this contract.',
	@dcHiringTermsComBHeadline NVARCHAR(MAX) = 'Commencement date',
	@dcHiringTermsComBSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsComBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsComBName, 
		@dcHiringTermsComBDescription,
		@dcHiringTermsComBText, 
		@dcHiringTermsComBHeadline,
		@dcHiringTermsComBSortOrder,
		@dcHiringTermsComBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsComBName, 
		[Description] = @dcHiringTermsComBDescription, 
		[Text] = @dcHiringTermsComBText,
		[Headline] = @dcHiringTermsComBHeadline,
		SortOrder = @dcHiringTermsComBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsComBGuid
END
DECLARE @dcHiringTermsComBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsComBGuid)

-- Create condition for Commence B
DECLARE @dcHiringTermsComBCondAGuid UNIQUEIDENTIFIER = '9d36be42-5689-49ec-843b-382650b592ad',
	@dcHiringTermsComBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcHiringTermsComBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@dcHiringTermsComBCondAValues NVARCHAR(MAX) = '',
	@dcHiringTermsComBCondADescription NVARCHAR(MAX) = 'Has end date',
	@dcHiringTermsComBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsComBCondAGuid)
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
		@dcHiringTermsComBCondAGuid,
		@dcHiringTermsComBID,
		@dcHiringTermsComBCondAPropertyName,
		@dcHiringTermsComBCondAOperator,
		@dcHiringTermsComBCondAValues,
		@dcHiringTermsComBCondADescription,
		@dcHiringTermsComBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsComBID,
		Property_Name = @dcHiringTermsComBCondAPropertyName,
		Operator = @dcHiringTermsComBCondAOperator,
		[Values] = @dcHiringTermsComBCondAValues,
		[Description] = @dcHiringTermsComBCondADescription,
		[Status] = @dcHiringTermsComBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsComBCondAGuid
END

-- No support for 31.12.9999 yet
/*DECLARE @dcHiringTermsComBCondBGuid UNIQUEIDENTIFIER = '43c72a21-96c1-4d3c-a44a-5279593332c7',
	@dcHiringTermsComBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractEndDate',
	@dcHiringTermsComBCondBOperator NVARCHAR(MAX) = 'NotEqual',
	@dcHiringTermsComBCondBValues NVARCHAR(MAX) = '31.12.9999',
	@dcHiringTermsComBCondBDescription NVARCHAR(MAX) = 'Not equals 31.12.9999',
	@dcHiringTermsComBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsComBCondBGuid)
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
		@dcHiringTermsComBCondBGuid,
		@dcHiringTermsComBID,
		@dcHiringTermsComBCondBPropertyName,
		@dcHiringTermsComBCondBOperator,
		@dcHiringTermsComBCondBValues,
		@dcHiringTermsComBCondBDescription,
		@dcHiringTermsComBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsComBID,
		Property_Name = @dcHiringTermsComBCondBPropertyName,
		Operator = @dcHiringTermsComBCondBOperator,
		[Values] = @dcHiringTermsComBCondBValues,
		[Description] = @dcHiringTermsComBCondBDescription,
		[Status] = @dcHiringTermsComBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsComBCondBGuid
END*/
-- #################################### Hours of Work

---- Hours of Work A
DECLARE @dcHiringTermsHWAGuid UNIQUEIDENTIFIER = '4a800b05-7d0e-4227-9724-8bce8cd243b3',
	@dcHiringTermsHWAName NVARCHAR(MAX) = @prefix + ' Hours of Work, full time',
	@dcHiringTermsHWADescription NVARCHAR(MAX) = '',
	@dcHiringTermsHWAText NVARCHAR(MAX) = 
	'You will be rostered to work 76 ordinary hours per fortnight.  Such details of your initial roster will be discussed with you upon your commencement.  However, where there is a change in the business’ needs, your hours may also be subject to change with appropriate notice.
You should note that ordinary hours in the Distribution Centre include Saturday’s and you have mutually agreed to work more than one in three Saturdays as part of your contracted ordinary hours.',
	@dcHiringTermsHWAHeadline NVARCHAR(MAX) = 'Hours of Work',
	@dcHiringTermsHWASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsHWAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsHWAName, 
		@dcHiringTermsHWADescription,
		@dcHiringTermsHWAText, 
		@dcHiringTermsHWAHeadline,
		@dcHiringTermsHWASortOrder,
		@dcHiringTermsHWAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsHWAName, 
		[Description] = @dcHiringTermsHWADescription, 
		[Text] = @dcHiringTermsHWAText,
		[Headline] = @dcHiringTermsHWAHeadline,
		SortOrder = @dcHiringTermsHWASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsHWAGuid
END
DECLARE @dcHiringTermsHWAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsHWAGuid)

-- Create condition for Hours of Work A
DECLARE @dcHiringTermsHWACondGuid UNIQUEIDENTIFIER = '10a71618-1ee7-4ad0-9a62-37ddae3a6009',
	@dcHiringTermsHWACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringTermsHWACondOperator NVARCHAR(MAX) = 'Equal',
	@dcHiringTermsHWACondValues NVARCHAR(MAX) = '76',
	@dcHiringTermsHWACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcHiringTermsHWACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsHWACondGuid)
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
		@dcHiringTermsHWACondGuid,
		@dcHiringTermsHWAID,
		@dcHiringTermsHWACondPropertyName,
		@dcHiringTermsHWACondOperator,
		@dcHiringTermsHWACondValues,
		@dcHiringTermsHWACondDescription,
		@dcHiringTermsHWACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsHWAID,
		Property_Name = @dcHiringTermsHWACondPropertyName,
		Operator = @dcHiringTermsHWACondOperator,
		[Values] = @dcHiringTermsHWACondValues,
		[Description] = @dcHiringTermsHWACondDescription,
		[Status] = @dcHiringTermsHWACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsHWACondGuid
END

---- Hours of Work B
DECLARE @dcHiringTermsHWBGuid UNIQUEIDENTIFIER = '01c03f18-01fd-4718-bfab-1e5ec78a0bb2',
	@dcHiringTermsHWBName NVARCHAR(MAX) = @prefix + ' Hours of Work, part time',
	@dcHiringTermsHWBDescription NVARCHAR(MAX) = '',
	@dcHiringTermsHWBText NVARCHAR(MAX) = 
	'Your contracted hours are <Contracted Hours> per fortnight, you may be offered additional ‘varied hours’ paid at your ordinary rate of pay.
	You will be rostered in accordance to your availability schedule which you filled out at the time of your employment. Your availability schedule forms part of your employment contract.',
	@dcHiringTermsHWBHeadline NVARCHAR(MAX) = 'Hours of Work',
	@dcHiringTermsHWBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsHWBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsHWBName, 
		@dcHiringTermsHWBDescription,
		@dcHiringTermsHWBText, 
		@dcHiringTermsHWBHeadline,
		@dcHiringTermsHWBSortOrder,
		@dcHiringTermsHWBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsHWBName, 
		[Description] = @dcHiringTermsHWBDescription, 
		[Text] = @dcHiringTermsHWBText,
		[Headline] = @dcHiringTermsHWBHeadline,
		SortOrder = @dcHiringTermsHWBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsHWBGuid
END
DECLARE @dcHiringTermsHWBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsHWBGuid)

-- Create condition for hours of work B
DECLARE @dcHiringTermsHWBCondGuid UNIQUEIDENTIFIER = '7bb07d5b-1787-4150-81e1-6c8fbade8bd0',
	@dcHiringTermsHWBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringTermsHWBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcHiringTermsHWBCondValues NVARCHAR(MAX) = '76',
	@dcHiringTermsHWBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcHiringTermsHWBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsHWBCondGuid)
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
		@dcHiringTermsHWBCondGuid,
		@dcHiringTermsHWBID,
		@dcHiringTermsHWBCondPropertyName,
		@dcHiringTermsHWBCondOperator,
		@dcHiringTermsHWBCondValues,
		@dcHiringTermsHWBCondDescription,
		@dcHiringTermsHWBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsHWBID,
		Property_Name = @dcHiringTermsHWBCondPropertyName,
		Operator = @dcHiringTermsHWBCondOperator,
		[Values] = @dcHiringTermsHWBCondValues,
		[Description] = @dcHiringTermsHWBCondDescription,
		[Status] = @dcHiringTermsHWBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsHWBCondGuid
END


-- #################################### Probationary Period

---- Probationary Period
DECLARE @dcHiringTermsProbTimeGuid UNIQUEIDENTIFIER = 'ab481264-fce1-4f8c-bdea-b5f4460e4688',
	@dcHiringTermsProbTimeName NVARCHAR(MAX) = @prefix + ' Probationary Period',
	@dcHiringTermsProbTimeDescription NVARCHAR(MAX) = '',
	@dcHiringTermsProbTimeText NVARCHAR(MAX) = 'IKEA offers this employment to you on a probationary basis for a period of six (6) months, during which time your performance standards will be subject to regular review and assessment.  In the six (6)-month period, if either you or IKEA wishes to terminate the employment relationship, then either party can effect that termination with one week’s notice in writing.',
	@dcHiringTermsProbTimeHeadline NVARCHAR(MAX) = 'Probationary Period',
	@dcHiringTermsProbTimeSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsProbTimeGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsProbTimeName, 
		@dcHiringTermsProbTimeDescription,
		@dcHiringTermsProbTimeText, 
		@dcHiringTermsProbTimeHeadline,
		@dcHiringTermsProbTimeSortOrder,
		@dcHiringTermsProbTimeGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsProbTimeName, 
		[Description] = @dcHiringTermsProbTimeDescription, 
		[Text] = @dcHiringTermsProbTimeText,
		[Headline] = @dcHiringTermsProbTimeHeadline,
		SortOrder = @dcHiringTermsProbTimeSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsProbTimeGuid
END

-- #################################### Remuneration
DECLARE @dcHiringTermsRemunGuid UNIQUEIDENTIFIER = 'a5452cb7-54f9-4f0e-acd9-b27da4e95a50',
	@dcHiringTermsRemunName NVARCHAR(MAX) = @prefix + ' Remuneration',
	@dcHiringTermsRemunDescription NVARCHAR(MAX) = '',
	@dcHiringTermsRemunText NVARCHAR(MAX) = 'Upon commencement, your base hourly rate will be as per the <b>IKEA Distributions Services Australia Pty Ltd Enterprise Agreement 2016</b>.  This amount will be paid directly into your nominated bank account on a fortnightly basis.
<p style="page-break-after: always;"></p>', -- New PDF page after this',
	@dcHiringTermsRemunHeadline NVARCHAR(MAX) = 'Remuneration',
	@dcHiringTermsRemunSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsRemunGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsRemunName, 
		@dcHiringTermsRemunDescription,
		@dcHiringTermsRemunText, 
		@dcHiringTermsRemunHeadline,
		@dcHiringTermsRemunSortOrder,
		@dcHiringTermsRemunGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsRemunName, 
		[Description] = @dcHiringTermsRemunDescription, 
		[Text] = @dcHiringTermsRemunText,
		[Headline] = @dcHiringTermsRemunHeadline,
		SortOrder = @dcHiringTermsRemunSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsRemunGuid
END

-- #################################### Superannuation
DECLARE @dcHiringTermsSuperGuid UNIQUEIDENTIFIER = '0fa41e7e-a54d-47f2-83fe-2c92817d006c',
	@dcHiringTermsSuperName NVARCHAR(MAX) = @prefix + ' Superannuation',
	@dcHiringTermsSuperDescription NVARCHAR(MAX) = '',
	@dcHiringTermsSuperText NVARCHAR(MAX) = 'IKEA will make superannuation contributions, on your behalf, to a government approved Superannuation Fund, at the rate payable under the Superannuation Guarantee Legislation (SGL). This rate is currently 9.5% of your wage.
<br>
IKEA’s current employer superannuation fund is the Labour Union Co-operative Retirement Fund (LUCRF), which is the fund into which the superannuation contributions will be made unless an alternate fund is nominated by you in writing, in accordance with the SGL.
<br>
It is your responsibility to nominate a Super Fund for your contributions to be made to, and to ensure that you complete the necessary paperwork for enrolment into your nominated fund.  IKEA will supply you with a LUCRF Member Guide, including an application form.',
	@dcHiringTermsSuperHeadline NVARCHAR(MAX) = 'Superannuation',
	@dcHiringTermsSuperSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsSuperGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsSuperName, 
		@dcHiringTermsSuperDescription,
		@dcHiringTermsSuperText, 
		@dcHiringTermsSuperHeadline,
		@dcHiringTermsSuperSortOrder,
		@dcHiringTermsSuperGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsSuperName, 
		[Description] = @dcHiringTermsSuperDescription, 
		[Text] = @dcHiringTermsSuperText,
		[Headline] = @dcHiringTermsSuperHeadline,
		SortOrder = @dcHiringTermsSuperSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsSuperGuid
END


-- #################################### Confidential Information
DECLARE @dcHiringTermsConfGuid UNIQUEIDENTIFIER = '38786f09-515d-4cb5-9fba-921fc738e630',
	@dcHiringTermsConfName NVARCHAR(MAX) = @prefix + ' Confidential Information',
	@dcHiringTermsConfDescription NVARCHAR(MAX) = '',
	@dcHiringTermsConfText NVARCHAR(MAX) = 'In the course of your employment, you may be exposed to “Confidential Information” concerning IKEA. Confidential Information means any information obtained by you in the course of your employment, including:
<br>
<br>
<ul>
<li>trade secrets;</li>
<li>technical information and technical drawings;</li>
<li>commercial information about IKEA and persons with whom IKEA deals;</li>
<li>Product and market information;</li>
<li>this letter of appointment;</li>
<li>any information marked “confidential” or which IKEA informs you is confidential or a trade secret; and</li>
<li>Co-worker and customer personal details.</li>
</ul>
but excluding:
<br>
<br>
<ul>
<li>information available to the public; and</li>
<li>information which you can prove you lawfully possessed before obtaining it in the course of your employment (other than this letter of appointment)</li>
</ul>
During and after your employment, you must not use or disclose Confidential Information to any person (including an employee of IKEA) other than:
<br>
<br>
<ul>
<li>to perform your duties;</li>
<li>if IKEA has consented in writing; or</li>
<li>if required by law.</li>
</ul>
As an IKEA co-worker, you must keep Confidential Information in a secure manner and treat such information with appropriate sensitivity. On demand by IKEA and at the end of your employment, you must deliver to IKEA all copies of Confidential Information in your possession or control (including all Confidential Information held electronically in any medium) and then delete all Confidential Information held electronically in any medium in your possession or control.
<p style="page-break-after: always;"></p>', -- New PDF page after this',
	@dcHiringTermsConfHeadline NVARCHAR(MAX) = 'Confidential Information',
	@dcHiringTermsConfSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsConfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsConfName, 
		@dcHiringTermsConfDescription,
		@dcHiringTermsConfText, 
		@dcHiringTermsConfHeadline,
		@dcHiringTermsConfSortOrder,
		@dcHiringTermsConfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsConfName, 
		[Description] = @dcHiringTermsConfDescription, 
		[Text] = @dcHiringTermsConfText,
		[Headline] = @dcHiringTermsConfHeadline,
		SortOrder = @dcHiringTermsConfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsConfGuid
END

-- #################################### Leave Entitlements 

---- Leave Entitlements  A
DECLARE @dcHiringTermsLeaveAGuid UNIQUEIDENTIFIER = '095c615c-a482-4ea3-aa3a-8e992ab6c88b',
	@dcHiringTermsLeaveAName NVARCHAR(MAX) = @prefix + ' Leave, full time',
	@dcHiringTermsLeaveADescription NVARCHAR(MAX) = '',
	@dcHiringTermsLeaveAText NVARCHAR(MAX) = 'From your commencement date, you are entitled to leave in accordance with the relevant legislation and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016. These entitlements are processed as detailed in the IKEA policies.',
	@dcHiringTermsLeaveAHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@dcHiringTermsLeaveASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsLeaveAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsLeaveAName, 
		@dcHiringTermsLeaveADescription,
		@dcHiringTermsLeaveAText, 
		@dcHiringTermsLeaveAHeadline,
		@dcHiringTermsLeaveASortOrder,
		@dcHiringTermsLeaveAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsLeaveAName, 
		[Description] = @dcHiringTermsLeaveADescription, 
		[Text] = @dcHiringTermsLeaveAText,
		[Headline] = @dcHiringTermsLeaveAHeadline,
		SortOrder = @dcHiringTermsLeaveASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsLeaveAGuid
END

DECLARE @dcHiringTermsLeaveAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsLeaveAGuid)


-- Create condition for Leave Entitlements A
DECLARE @dcHiringTermsLeaveACondGuid UNIQUEIDENTIFIER = '9c360b86-beec-4de1-9ee8-9ddd9c1fc87b',
	@dcHiringTermsLeaveACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringTermsLeaveACondOperator NVARCHAR(MAX) = 'Equal',
	@dcHiringTermsLeaveACondValues NVARCHAR(MAX) = '76',
	@dcHiringTermsLeaveACondDescription NVARCHAR(MAX) = 'Is full time',
	@dcHiringTermsLeaveACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsLeaveACondGuid)
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
		@dcHiringTermsLeaveACondGuid,
		@dcHiringTermsLeaveAID,
		@dcHiringTermsLeaveACondPropertyName,
		@dcHiringTermsLeaveACondOperator,
		@dcHiringTermsLeaveACondValues,
		@dcHiringTermsLeaveACondDescription,
		@dcHiringTermsLeaveACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsLeaveAID,
		Property_Name = @dcHiringTermsLeaveACondPropertyName,
		Operator = @dcHiringTermsLeaveACondOperator,
		[Values] = @dcHiringTermsLeaveACondValues,
		[Description] = @dcHiringTermsLeaveACondDescription,
		[Status] = @dcHiringTermsLeaveACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsLeaveACondGuid
END

---- Leave Entitlements  B
DECLARE @dcHiringTermsLeaveBGuid UNIQUEIDENTIFIER = '1db91d5f-1e64-4d18-af52-17f004a032c1',
	@dcHiringTermsLeaveBName NVARCHAR(MAX) = @prefix + ' Leave, part time',
	@dcHiringTermsLeaveBDescription NVARCHAR(MAX) = '',
	@dcHiringTermsLeaveBText NVARCHAR(MAX) = 'You will accrue entitlements to leave in accordance with relevant legislation and company policy on a pro rata basis (compared to a full-time employee). As stated in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  These entitlements are processed as detailed in the IKEA policies.  This includes personal leave, annual leave, parental leave and long service leave.',
	@dcHiringTermsLeaveBHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@dcHiringTermsLeaveBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsLeaveBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsLeaveBName, 
		@dcHiringTermsLeaveBDescription,
		@dcHiringTermsLeaveBText, 
		@dcHiringTermsLeaveBHeadline,
		@dcHiringTermsLeaveBSortOrder,
		@dcHiringTermsLeaveBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsLeaveBName, 
		[Description] = @dcHiringTermsLeaveBDescription, 
		[Text] = @dcHiringTermsLeaveBText,
		[Headline] = @dcHiringTermsLeaveBHeadline,
		SortOrder = @dcHiringTermsLeaveBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsLeaveBGuid
END
DECLARE @dcHiringTermsLeaveBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsLeaveBGuid)

-- Create condition for leave entitlements B
DECLARE @dcHiringTermsLeaveBCondGuid UNIQUEIDENTIFIER = 'da303f7b-9bfc-423a-b326-716bbc52e194',
	@dcHiringTermsLeaveBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@dcHiringTermsLeaveBCondOperator NVARCHAR(MAX) = 'LessThan',
	@dcHiringTermsLeaveBCondValues NVARCHAR(MAX) = '76',
	@dcHiringTermsLeaveBCondDescription NVARCHAR(MAX) = 'Is part time',
	@dcHiringTermsLeaveBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @dcHiringTermsLeaveBCondGuid)
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
		@dcHiringTermsLeaveBCondGuid,
		@dcHiringTermsLeaveBID,
		@dcHiringTermsLeaveBCondPropertyName,
		@dcHiringTermsLeaveBCondOperator,
		@dcHiringTermsLeaveBCondValues,
		@dcHiringTermsLeaveBCondDescription,
		@dcHiringTermsLeaveBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @dcHiringTermsLeaveBID,
		Property_Name = @dcHiringTermsLeaveBCondPropertyName,
		Operator = @dcHiringTermsLeaveBCondOperator,
		[Values] = @dcHiringTermsLeaveBCondValues,
		[Description] = @dcHiringTermsLeaveBCondDescription,
		[Status] = @dcHiringTermsLeaveBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @dcHiringTermsLeaveBCondGuid
END

-- #################################### Issues Resolution
DECLARE @dcHiringTermsIssuesGuid UNIQUEIDENTIFIER = '6e0427ae-fe4d-492d-9319-5f047223df3d',
	@dcHiringTermsIssuesName NVARCHAR(MAX) = @prefix + ' Issues Resolution',
	@dcHiringTermsIssuesDescription NVARCHAR(MAX) = '',
	@dcHiringTermsIssuesText NVARCHAR(MAX) = 'If any issues arise during your employment with IKEA, the matter should initially be discussed with your immediate manager, in accordance with IKEA’s Issue Resolution Procedure.  If the problem remains unresolved, you may refer it to more senior levels of management for further discussion in accordance with the Issue Resolution Procedure and the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.',
	@dcHiringTermsIssuesHeadline NVARCHAR(MAX) = 'Issues Resolution',
	@dcHiringTermsIssuesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsIssuesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsIssuesName, 
		@dcHiringTermsIssuesDescription,
		@dcHiringTermsIssuesText, 
		@dcHiringTermsIssuesHeadline,
		@dcHiringTermsIssuesSortOrder,
		@dcHiringTermsIssuesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsIssuesName, 
		[Description] = @dcHiringTermsIssuesDescription, 
		[Text] = @dcHiringTermsIssuesText,
		[Headline] = @dcHiringTermsIssuesHeadline,
		SortOrder = @dcHiringTermsIssuesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsIssuesGuid
END

-- #################################### Equal Employment Opportunity 
DECLARE @dcHiringTermsEqualGuid UNIQUEIDENTIFIER = 'bc3a4308-8d47-4d31-bf86-3699df111d9f',
	@dcHiringTermsEqualName NVARCHAR(MAX) = @prefix + ' Equal Employment',
	@dcHiringTermsEqualDescription NVARCHAR(MAX) = '',
	@dcHiringTermsEqualText NVARCHAR(MAX) = 'IKEA''s policy is to provide all co-workers with equal opportunity.  This policy precludes discrimination and harassment based on, but not limited to, race, colour, religion, gender, age, marital status and disability.  You are required to familiarise yourself with this policy and comply with it at all times.',
	@dcHiringTermsEqualHeadline NVARCHAR(MAX) = 'Equal Employment Opportunity',
	@dcHiringTermsEqualSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsEqualGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsEqualName, 
		@dcHiringTermsEqualDescription,
		@dcHiringTermsEqualText, 
		@dcHiringTermsEqualHeadline,
		@dcHiringTermsEqualSortOrder,
		@dcHiringTermsEqualGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsEqualName, 
		[Description] = @dcHiringTermsEqualDescription, 
		[Text] = @dcHiringTermsEqualText,
		[Headline] = @dcHiringTermsEqualHeadline,
		SortOrder = @dcHiringTermsEqualSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsEqualGuid
END

-- #################################### Uniform & Conduct
DECLARE @dcHiringTermsUniformGuid UNIQUEIDENTIFIER = '9c93cf51-80fa-4ff9-9c9b-4d552817479a',
	@dcHiringTermsUniformName NVARCHAR(MAX) = @prefix + ' Uniform & Conduct',
	@dcHiringTermsUniformDescription NVARCHAR(MAX) = '',
	@dcHiringTermsUniformText NVARCHAR(MAX) = 'IKEA has established guidelines necessary for the professional appearance that IKEA expects all co-workers to present, and as such co-workers are supplied with uniforms and name badges within these guidelines.
<br><br>
Co-workers are expected to project a favourable and professional image for IKEA, and are to be courteous, efficient and reliable in their dealings with colleagues, existing and potential customers and suppliers to IKEA.',
	@dcHiringTermsUniformHeadline NVARCHAR(MAX) = 'Uniform & Conduct',
	@dcHiringTermsUniformSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsUniformGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsUniformName, 
		@dcHiringTermsUniformDescription,
		@dcHiringTermsUniformText, 
		@dcHiringTermsUniformHeadline,
		@dcHiringTermsUniformSortOrder,
		@dcHiringTermsUniformGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsUniformName, 
		[Description] = @dcHiringTermsUniformDescription, 
		[Text] = @dcHiringTermsUniformText,
		[Headline] = @dcHiringTermsUniformHeadline,
		SortOrder = @dcHiringTermsUniformSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsUniformGuid
END

-- #################################### Induction & Ongoing Learning & Development
DECLARE @dcHiringTermsInductionGuid UNIQUEIDENTIFIER = '19ca06b0-6183-4da9-994a-ae5b44165a72',
	@dcHiringTermsInductionName NVARCHAR(MAX) = @prefix + ' Induction',
	@dcHiringTermsInductionDescription NVARCHAR(MAX) = '',
	@dcHiringTermsInductionText NVARCHAR(MAX) = 'IKEA is committed to your induction and ongoing development and as such, has a requirement that you undertake and are committed to training as offered by the company.  Whilst the majority of training is conducted on the job, you may be required from time to time to attend external training programs at different locations as organised by IKEA.
<br><br>
IKEA also encourages its co-workers to take responsibility for their own learning and development.',
	@dcHiringTermsInductionHeadline NVARCHAR(MAX) = 'Induction & Ongoing Learning & Development',
	@dcHiringTermsInductionSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsInductionGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsInductionName, 
		@dcHiringTermsInductionDescription,
		@dcHiringTermsInductionText, 
		@dcHiringTermsInductionHeadline,
		@dcHiringTermsInductionSortOrder,
		@dcHiringTermsInductionGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsInductionName, 
		[Description] = @dcHiringTermsInductionDescription, 
		[Text] = @dcHiringTermsInductionText,
		[Headline] = @dcHiringTermsInductionHeadline,
		SortOrder = @dcHiringTermsInductionSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsInductionGuid
END

-- #################################### Occupational Health & Safety
DECLARE @dcHiringTermsSafetyGuid UNIQUEIDENTIFIER = '82bbb059-74ce-4a9f-be52-ecf806ba9dee',
	@dcHiringTermsSafetyName NVARCHAR(MAX) = @prefix + ' Safety',
	@dcHiringTermsSafetyDescription NVARCHAR(MAX) = '',
	@dcHiringTermsSafetyText NVARCHAR(MAX) = 'IKEA understands the requirement of ensuring a safe and healthy working environment for all co-workers in its offices, warehouses and stores, and a safe and healthy shopping environment for customers.  In fulfilling this aim, we undertake regular consultation with co-workers on health and safety issues and concerns.',
	@dcHiringTermsSafetyHeadline NVARCHAR(MAX) = 'Occupational Health & Safety',
	@dcHiringTermsSafetySortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsSafetyGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsSafetyName, 
		@dcHiringTermsSafetyDescription,
		@dcHiringTermsSafetyText, 
		@dcHiringTermsSafetyHeadline,
		@dcHiringTermsSafetySortOrder,
		@dcHiringTermsSafetyGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsSafetyName, 
		[Description] = @dcHiringTermsSafetyDescription, 
		[Text] = @dcHiringTermsSafetyText,
		[Headline] = @dcHiringTermsSafetyHeadline,
		SortOrder = @dcHiringTermsSafetySortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsSafetyGuid
END

-- #################################### Termination
DECLARE @dcHiringTermsTerminationGuid UNIQUEIDENTIFIER = '70cf8691-6792-4ddd-9743-3c83fcebe1a2',
	@dcHiringTermsTerminationName NVARCHAR(MAX) = @prefix + ' Termination',
	@dcHiringTermsTerminationDescription NVARCHAR(MAX) = '',
	@dcHiringTermsTerminationText NVARCHAR(MAX) = 'Either party may terminate the employment relationship with the appropriate notice as prescribed in the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016.  Notice provisions do not apply in the case of summary dismissal.
<br><br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee.
<br><br>
IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.
<p style="page-break-after: always;"></p>', -- New PDF page after this',
	@dcHiringTermsTerminationHeadline NVARCHAR(MAX) = 'Termination',
	@dcHiringTermsTerminationSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsTerminationGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsTerminationName, 
		@dcHiringTermsTerminationDescription,
		@dcHiringTermsTerminationText, 
		@dcHiringTermsTerminationHeadline,
		@dcHiringTermsTerminationSortOrder,
		@dcHiringTermsTerminationGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsTerminationName, 
		[Description] = @dcHiringTermsTerminationDescription, 
		[Text] = @dcHiringTermsTerminationText,
		[Headline] = @dcHiringTermsTerminationHeadline,
		SortOrder = @dcHiringTermsTerminationSortOrder 
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsTerminationGuid
END

-- #################################### Company Policies & Procedures 
DECLARE @dcHiringTermsPoliciesGuid UNIQUEIDENTIFIER = 'ee5d4723-4bc3-436c-9e9a-3faa079715a1',
	@dcHiringTermsPoliciesName NVARCHAR(MAX) = @prefix + ' Policies',
	@dcHiringTermsPoliciesDescription NVARCHAR(MAX) = '',
	@dcHiringTermsPoliciesText NVARCHAR(MAX) = 'You shall be required to comply with all IKEA Policies and Procedures as amended from time to time and as outlined in IKEA’s Policy Guidelines and Welcome Program. The IKEA Policies and Procedures form part of your contract of employment.',
	@dcHiringTermsPoliciesHeadline NVARCHAR(MAX) = 'Company Policies & Procedures',
	@dcHiringTermsPoliciesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsPoliciesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsPoliciesName, 
		@dcHiringTermsPoliciesDescription,
		@dcHiringTermsPoliciesText, 
		@dcHiringTermsPoliciesHeadline,
		@dcHiringTermsPoliciesSortOrder,
		@dcHiringTermsPoliciesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsPoliciesName, 
		[Description] = @dcHiringTermsPoliciesDescription, 
		[Text] = @dcHiringTermsPoliciesText,
		[Headline] = @dcHiringTermsPoliciesHeadline,
		SortOrder = @dcHiringTermsPoliciesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPoliciesGuid
END

-- #################################### Other Terms and Conditions
DECLARE @dcHiringTermsOtherTermsGuid UNIQUEIDENTIFIER = '10702b8d-eff8-4df4-897c-5150349d3b91',
	@dcHiringTermsOtherTermsName NVARCHAR(MAX) = @prefix + ' Other T&C',
	@dcHiringTermsOtherTermsDescription NVARCHAR(MAX) = '',
	@dcHiringTermsOtherTermsText NVARCHAR(MAX) = 'The terms and conditions contained within the IKEA Distribution Services Australia Pty Ltd Enterprise Agreement 2016 (and any subsequent statutory agreement binding you and IKEA) also apply to your employment.  A copy of this Agreement is available for your perusal at all times.',
	@dcHiringTermsOtherTermsHeadline NVARCHAR(MAX) = 'Other Terms and Conditions',
	@dcHiringTermsOtherTermsSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsOtherTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsOtherTermsName, 
		@dcHiringTermsOtherTermsDescription,
		@dcHiringTermsOtherTermsText, 
		@dcHiringTermsOtherTermsHeadline,
		@dcHiringTermsOtherTermsSortOrder,
		@dcHiringTermsOtherTermsGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsOtherTermsName, 
		[Description] = @dcHiringTermsOtherTermsDescription, 
		[Text] = @dcHiringTermsOtherTermsText,
		[Headline] = @dcHiringTermsOtherTermsHeadline,
		SortOrder = @dcHiringTermsOtherTermsSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsOtherTermsGuid
END

-- #################################### Police Checks
DECLARE @dcHiringTermsPoliceGuid UNIQUEIDENTIFIER = 'ea1df328-cadb-435d-b169-5b0530a6f2d8',
	@dcHiringTermsPoliceName NVARCHAR(MAX) = @prefix + ' Police',
	@dcHiringTermsPoliceDescription NVARCHAR(MAX) = '',
	@dcHiringTermsPoliceText NVARCHAR(MAX) = 'Some positions at IKEA require evidence of good character (for example - positions that deal with children or cash).  Obtaining details of your criminal history via a police check/s is an integral part of the assessment of your suitability for such positions.
<br><br>
You may be required to provide a police check/s at the time you are given this offer of employment.  Alternatively, you may be required to provide a police check/s during your employment (for instance, when it is suspected that you have incurred a criminal record since the commencement of your employment, or where you begin working in a position requiring evidence of good character).  By signing this offer of employment, you consent to complete, sign and lodge the relevant police check application documentation (which will be provided to you by IKEA), and to direct that the corresponding police check record/s be forwarded directly to IKEA (where permitted) or (otherwise) to provide IKEA with the original police check record/s immediately on receipt.
<br><br>
If you are required to provide the police check/s at the time you are given this offer of employment, you acknowledge that the offer of employment will be subject to the check being satisfactory to IKEA.
<br><br>
If you are required to provide the police check/s at any other time during your employment and the check is not satisfactory to IKEA, it may give grounds for dismissal.
<br><br>
Please note that the existence of a criminal record does not mean that the check will automatically be unsatisfactory, or that you will be assessed automatically as being unsuitable.  Each case will be assessed on its merits and will depend upon the inherent requirements of the position.',
	@dcHiringTermsPoliceHeadline NVARCHAR(MAX) = 'Police Checks',
	@dcHiringTermsPoliceSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsPoliceGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsPoliceName, 
		@dcHiringTermsPoliceDescription,
		@dcHiringTermsPoliceText, 
		@dcHiringTermsPoliceHeadline,
		@dcHiringTermsPoliceSortOrder,
		@dcHiringTermsPoliceGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsPoliceName, 
		[Description] = @dcHiringTermsPoliceDescription, 
		[Text] = @dcHiringTermsPoliceText,
		[Headline] = @dcHiringTermsPoliceHeadline,
		SortOrder = @dcHiringTermsPoliceSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPoliceGuid
END

-- #################################### Performance Management
DECLARE @dcHiringTermsPerfGuid UNIQUEIDENTIFIER = '1e834bbd-cf8f-40d1-a4fb-c55041c3dc1a',
	@dcHiringTermsPerfName NVARCHAR(MAX) = @prefix + ' Performance',
	@dcHiringTermsPerfDescription NVARCHAR(MAX) = '',
	@dcHiringTermsPerfText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November.  However, your first review will be conducted during your 6-month probationary period.  This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@dcHiringTermsPerfHeadline NVARCHAR(MAX) = 'Performance Management',
	@dcHiringTermsPerfSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsPerfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsPerfName, 
		@dcHiringTermsPerfDescription,
		@dcHiringTermsPerfText, 
		@dcHiringTermsPerfHeadline,
		@dcHiringTermsPerfSortOrder,
		@dcHiringTermsPerfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsPerfName, 
		[Description] = @dcHiringTermsPerfDescription, 
		[Text] = @dcHiringTermsPerfText,
		[Headline] = @dcHiringTermsPerfHeadline,
		SortOrder = @dcHiringTermsPerfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsPerfGuid
END

-- #################################### Communications with Media
DECLARE @dcHiringTermsMediaGuid UNIQUEIDENTIFIER = '3825d63e-4075-4d61-99f2-cc4807220d7c',
	@dcHiringTermsMediaName NVARCHAR(MAX) = @prefix + ' Media',
	@dcHiringTermsMediaDescription NVARCHAR(MAX) = '',
	@dcHiringTermsMediaText NVARCHAR(MAX) = 'You shall not provide information or speak on behalf of IKEA or otherwise to the media on any matters concerning IKEA’s business or activities.  You must refer all requests from the media for information and/or interviews to the DC Manager.',
	@dcHiringTermsMediaHeadline NVARCHAR(MAX) = 'Communications with Media',
	@dcHiringTermsMediaSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsMediaGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsMediaName, 
		@dcHiringTermsMediaDescription,
		@dcHiringTermsMediaText, 
		@dcHiringTermsMediaHeadline,
		@dcHiringTermsMediaSortOrder,
		@dcHiringTermsMediaGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsMediaName, 
		[Description] = @dcHiringTermsMediaDescription, 
		[Text] = @dcHiringTermsMediaText,
		[Headline] = @dcHiringTermsMediaHeadline,
		SortOrder = @dcHiringTermsMediaSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsMediaGuid
END

-- #################################### Obligation to report unlawful activities
DECLARE @dcHiringTermsUnlawGuid UNIQUEIDENTIFIER = 'dd43215f-70a6-4d8d-b0e5-eb89632f2291',
	@dcHiringTermsUnlawName NVARCHAR(MAX) = @prefix + ' Unlawful',
	@dcHiringTermsUnlawDescription NVARCHAR(MAX) = '',
	@dcHiringTermsUnlawText NVARCHAR(MAX) = 'If you become aware of or suspect any unlawful act or omission by any IKEA employee, you must advise IKEA immediately.',
	@dcHiringTermsUnlawHeadline NVARCHAR(MAX) = 'Obligation to report unlawful activities',
	@dcHiringTermsUnlawSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsUnlawGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsUnlawName, 
		@dcHiringTermsUnlawDescription,
		@dcHiringTermsUnlawText, 
		@dcHiringTermsUnlawHeadline,
		@dcHiringTermsUnlawSortOrder,
		@dcHiringTermsUnlawGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsUnlawName, 
		[Description] = @dcHiringTermsUnlawDescription, 
		[Text] = @dcHiringTermsUnlawText,
		[Headline] = @dcHiringTermsUnlawHeadline,
		SortOrder = @dcHiringTermsUnlawSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsUnlawGuid
END

-- #################################### Intellectual Property
DECLARE @dcHiringTermsIntelPropGuid UNIQUEIDENTIFIER = '05a82051-5e5e-47df-ae38-469375a4e135',
	@dcHiringTermsIntelPropName NVARCHAR(MAX) = @prefix + ' Int. Property',
	@dcHiringTermsIntelPropDescription NVARCHAR(MAX) = '',
	@dcHiringTermsIntelPropText NVARCHAR(MAX) = 'IKEA owns all copyright in any works and all inventions, discoveries, novel designs, improvements or modifications, computer program material and trademarks which you write or develop in the course of your employment (in or out of working hours) (“Intellectual Property”).<br>
<br>
You assign to IKEA any interest you have in the Intellectual Property, and you must disclose any Intellectual Property to IKEA.<br>
<br>
During and after your employment, you must do anything IKEA reasonably requires (at IKEA''s cost) to:
<ul>
<li>obtain statutory protection (including by patent, design registration, trade mark registration or copyright) for the Intellectual Property for IKEA in any country; or</li>
<li>Perfect or evidence IKEA’s ownership of the Intellectual Property.</li>
</ul>',

	@dcHiringTermsIntelPropHeadline NVARCHAR(MAX) = 'Intellectual Property',
	@dcHiringTermsIntelPropSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsIntelPropGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsIntelPropName, 
		@dcHiringTermsIntelPropDescription,
		@dcHiringTermsIntelPropText, 
		@dcHiringTermsIntelPropHeadline,
		@dcHiringTermsIntelPropSortOrder,
		@dcHiringTermsIntelPropGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsIntelPropName, 
		[Description] = @dcHiringTermsIntelPropDescription, 
		[Text] = @dcHiringTermsIntelPropText,
		[Headline] = @dcHiringTermsIntelPropHeadline,
		SortOrder = @dcHiringTermsIntelPropSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsIntelPropGuid
END

-- #################################### Variation
DECLARE @dcHiringTermsVarGuid UNIQUEIDENTIFIER = '99ab359a-d4c9-4fba-a1ce-9ae064b7251b',
	@dcHiringTermsVarName NVARCHAR(MAX) = @prefix + ' Variation',
	@dcHiringTermsVarDescription NVARCHAR(MAX) = '',
	@dcHiringTermsVarText NVARCHAR(MAX) = 'This Agreement may only be varied by a written agreement signed by yourself and IKEA.',
	@dcHiringTermsVarHeadline NVARCHAR(MAX) = 'Variation',
	@dcHiringTermsVarSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsVarGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsVarName, 
		@dcHiringTermsVarDescription,
		@dcHiringTermsVarText, 
		@dcHiringTermsVarHeadline,
		@dcHiringTermsVarSortOrder,
		@dcHiringTermsVarGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsVarName, 
		[Description] = @dcHiringTermsVarDescription, 
		[Text] = @dcHiringTermsVarText,
		[Headline] = @dcHiringTermsVarHeadline,
		SortOrder = @dcHiringTermsVarSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsVarGuid
END

-- #################################### Suspension
DECLARE @dcHiringTermsSuspGuid UNIQUEIDENTIFIER = '73970de6-40d8-4048-a409-539d4dc2ba34',
	@dcHiringTermsSuspName NVARCHAR(MAX) = @prefix + ' Suspension',
	@dcHiringTermsSuspDescription NVARCHAR(MAX) = '',
	@dcHiringTermsSuspText NVARCHAR(MAX) = 'If we have reason to believe that you may have engaged in a serious breach of your employment obligations, IKEA may at its discretion suspend you from your duties, either with or without pay, while and investigation is conducted.',
	@dcHiringTermsSuspHeadline NVARCHAR(MAX) = 'Suspension',
	@dcHiringTermsSuspSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringTermsSuspGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringTermsID, 
		@dcHiringTermsSuspName, 
		@dcHiringTermsSuspDescription,
		@dcHiringTermsSuspText, 
		@dcHiringTermsSuspHeadline,
		@dcHiringTermsSuspSortOrder,
		@dcHiringTermsSuspGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringTermsID,
		[Name] = @dcHiringTermsSuspName, 
		[Description] = @dcHiringTermsSuspDescription, 
		[Text] = @dcHiringTermsSuspText,
		[Headline] = @dcHiringTermsSuspHeadline,
		SortOrder = @dcHiringTermsSuspSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringTermsSuspGuid
END

-- Add terms paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @dcHiringTermsID, @counter
SET @counter = @counter + 1


-- #################################### End Text
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcHiringEndTextParagraphGuid UNIQUEIDENTIFIER = '17f11260-0b32-4bc2-a16c-b36740cc30e6',
	@dcHiringEndTextParagraphName NVARCHAR(MAX) = @prefix + ' End Text',
	@dcHiringEndTextParagraphType INT = @ParagraphTypeText,
	@dcHiringEndTextParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcHiringEndTextParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcHiringEndTextParagraphName, @dcHiringEndTextParagraphDescription, @dcHiringEndTextParagraphType, @dcHiringEndTextParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcHiringEndTextParagraphName, [Description] = @dcHiringEndTextParagraphDescription, ParagraphType = @dcHiringEndTextParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcHiringEndTextParagraphGuid
END
DECLARE @dcHiringEndTextParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcHiringEndTextParagraphGuid)

-- Create a text field
DECLARE @dcHiringEndTextGuid UNIQUEIDENTIFIER = '8aebf89f-11b1-4231-bfce-a05e10f85f7d',
	@dcHiringEndTextName NVARCHAR(MAX) = @prefix + ' End Text',
	@dcHiringEndTextDescription NVARCHAR(MAX) = '',
	@dcHiringEndTextText NVARCHAR(MAX) = 'IKEA recognises that its co-workers are essential to the success of the company’s operations.  IKEA remains committed to ensuring that all co-workers are treated fairly and equitably and encourages co-workers to reach their full potential.  We believe that the basis of your employment outlined above, will achieve these objectives and greatly benefit those co-workers willing to develop themselves.
	<br><br>
	As an indication of your understanding and acceptance of these conditions, please sign this letter of offer, and return to the undersigned within seven (7) days.  Please retain the second copy for your records.
	<br><br>
	<p style="page-break-after: always;"></p> <!-- New PDF page after this -->
	If you have any questions pertaining to this offer of employment or any of the information contained herein, please do not hesitate to contact me before signing this letter.',
	@dcHiringEndTextHeadline NVARCHAR(MAX) = '',
	@dcHiringEndTextSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringEndTextGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringEndTextParagraphID, 
		@dcHiringEndTextName, 
		@dcHiringEndTextDescription,
		@dcHiringEndTextText, 
		@dcHiringEndTextHeadline,
		@dcHiringEndTextSortOrder,
		@dcHiringEndTextGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringEndTextParagraphID,
		[Name] = @dcHiringEndTextName, 
		[Description] = @dcHiringEndTextDescription, 
		[Text] = @dcHiringEndTextText,
		[Headline] = @dcHiringEndTextHeadline,
		SortOrder = @dcHiringEndTextSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringEndTextGuid
END

-- Add end text paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @dcHiringEndTextParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Contractor Signature
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcHiringConSignParagraphGuid UNIQUEIDENTIFIER = '44659449-5e45-41e8-bccd-f8f2da46abd2',
	@dcHiringConSignParagraphName NVARCHAR(MAX) = @prefix + ' Con. Sign.',
	@dcHiringConSignParagraphType INT = @ParagraphTypeText,
	@dcHiringConSignParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcHiringConSignParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcHiringConSignParagraphName, @dcHiringConSignParagraphDescription, @dcHiringConSignParagraphType, @dcHiringConSignParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcHiringConSignParagraphName, [Description] = @dcHiringConSignParagraphDescription, ParagraphType = @dcHiringConSignParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcHiringConSignParagraphGuid
END
DECLARE @dcHiringConSignParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcHiringConSignParagraphGuid)

-- Create a text field
DECLARE @dcHiringConSignGuid UNIQUEIDENTIFIER = 'f0682a33-53e2-4bd0-9a1c-5a57987697d6',
	@dcHiringConSignName NVARCHAR(MAX) = @prefix + ' Con. Sign.',
	@dcHiringConSignDescription NVARCHAR(MAX) = '',
	@dcHiringConSignText NVARCHAR(MAX) = '<br><br><br>
	Yours sincerely,<br>
	<Reports To Line Manager><br>
	<Position Title (Local Job Name) of Reports To Line Manager><br>
	<strong>IKEA Distribution Services Australia Pty Ltd</strong>',
	@dcHiringConSignHeadline NVARCHAR(MAX) = '',
	@dcHiringConSignSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringConSignGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringConSignParagraphID, 
		@dcHiringConSignName, 
		@dcHiringConSignDescription,
		@dcHiringConSignText, 
		@dcHiringConSignHeadline,
		@dcHiringConSignSortOrder,
		@dcHiringConSignGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringConSignParagraphID,
		[Name] = @dcHiringConSignName, 
		[Description] = @dcHiringConSignDescription, 
		[Text] = @dcHiringConSignText,
		[Headline] = @dcHiringConSignHeadline,
		SortOrder = @dcHiringConSignSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringConSignGuid
END

-- Add contractor signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @dcHiringConSignParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Acceptance
---- Create or update paragraph
-- Paragraph guid
DECLARE @dcHiringAcceptParagraphGuid UNIQUEIDENTIFIER = '56027625-3032-481b-92bd-8f3bc651081f',
	@dcHiringAcceptParagraphName NVARCHAR(MAX) = @prefix + ' Acceptance',
	@dcHiringAcceptParagraphType INT = @ParagraphTypeText,
	@dcHiringAcceptParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @dcHiringAcceptParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@dcHiringAcceptParagraphName, @dcHiringAcceptParagraphDescription, @dcHiringAcceptParagraphType, @dcHiringAcceptParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @dcHiringAcceptParagraphName, [Description] = @dcHiringAcceptParagraphDescription, ParagraphType = @dcHiringAcceptParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @dcHiringAcceptParagraphGuid
END
DECLARE @dcHiringAcceptParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @dcHiringAcceptParagraphGuid)

-- Create a text field
DECLARE @dcHiringAcceptGuid UNIQUEIDENTIFIER = 'c7caffaf-1e22-46fe-a5b3-e301660b9cdb',
	@dcHiringAcceptName NVARCHAR(MAX) = @prefix + ' Acceptance',
	@dcHiringAcceptDescription NVARCHAR(MAX) = '',
	@dcHiringAcceptText NVARCHAR(MAX) = '
<style>

#acceptance {
	font-family: Verdana; 
	border: 1px solid black; 
	width: 5000px;
}


#acceptance th {
	font-size: 16pt; 
	text-decoration: underline;
	text-align: center;
	padding-top: 20px;
	width: 5000px;
}
#acceptance td {

	padding-left: 20px;
}

#acceptance td .signHeader {
	vertical-align: top;
}

#acceptance  td .signLine {
	height:5px
}

#acceptance td .sign {
	height:100px;
	vertical-align: bottom;
}

</style>
<br><br>
<table id="acceptance">
<tr><th>ACCEPTANCE</th></tr>
<tr><td><br>I accept the terms and conditions of employment as detailed above.</td></tr>
<tr><td class="sign"><br><br><Co-worker First Name> <Co-worker Last Name></td></tr>
<tr><td class="signLine">.................................................................</td></tr>
<tr><td class="signHeader">Name</td></tr>
<tr><td><br><br></td></tr>
<tr><td>.................................................................</td></tr>
<tr><td class="signHeader">Signature</td></tr>
<tr><td><br><br></td></tr>
<tr><td>.................................................................</td></tr>
<tr><td class="signHeader">Date<br><br></td></tr>
</table>',
	@dcHiringAcceptHeadline NVARCHAR(MAX) = '',
	@dcHiringAcceptSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @dcHiringAcceptGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@dcHiringAcceptParagraphID, 
		@dcHiringAcceptName, 
		@dcHiringAcceptDescription,
		@dcHiringAcceptText, 
		@dcHiringAcceptHeadline,
		@dcHiringAcceptSortOrder,
		@dcHiringAcceptGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @dcHiringAcceptParagraphID,
		[Name] = @dcHiringAcceptName, 
		[Description] = @dcHiringAcceptDescription, 
		[Text] = @dcHiringAcceptText,
		[Headline] = @dcHiringAcceptHeadline,
		SortOrder = @dcHiringAcceptSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @dcHiringAcceptGuid
END

-- Add acceptance paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @dcHiringID, @dcHiringAcceptParagraphID, @counter
SET @counter = @counter + 1




-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @dcHiringGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



COMMIT

