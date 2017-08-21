--########################################
--########## RET SAL TC ##################
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

DECLARE @prefix NVARCHAR(MAX) = 'RET SAL TC'

-- #################################### Contract Clusters – Retail Salary T&C) ####################################

-- Get the form
DECLARE @retSalTcGuid UNIQUEIDENTIFIER = 'EA1D92DF-AF56-4D3E-BA70-C45E4C3C30DA'
DECLARE @retSalTcID INT, @counter INT = 0
SELECT @retSalTcID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retSalTcGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @retSalTcGuid

-- #################################### Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer with initials
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @footerWithInitialsID, @counter
SET @counter = @counter + 1

-- #################################### Header

---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalTcHeaderGuid UNIQUEIDENTIFIER = 'F9DFA613-9D0B-4C06-AB14-6577DA9184DC',
	@retSalTcHeaderName NVARCHAR(MAX) = @prefix + ' Header',
	@retSalTcHeaderParagraphType INT = @ParagraphTypeText,
	@retSalTcHeaderDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalTcHeaderGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalTcHeaderName, @retSalTcHeaderDescription, @retSalTcHeaderParagraphType, @retSalTcHeaderGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalTcHeaderName, [Description] = @retSalTcHeaderDescription, ParagraphType = @retSalTcHeaderParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalTcHeaderGuid
END
DECLARE @retSalTcHeaderID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalTcHeaderGuid)

---- Create or update text A. Company info
DECLARE @retSalTcHeaderTextAGuid UNIQUEIDENTIFIER = '6895874E-7FAE-42F9-9624-9D285CCD6C17',
	@retSalTcHeaderTextAName NVARCHAR(MAX) = @prefix + ' Header, Company',
	@retSalTcHeaderTextADescription NVARCHAR(MAX) = '',
	@retSalTcHeaderTextAText NVARCHAR(MAX) = '<p style="text-align:left;">IKEA Pty Limited ABN 84 006 270 757</p>',
	@retSalTcHeaderTextAHeadline NVARCHAR(MAX) = '',
	@retSalTcHeaderTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcHeaderTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcHeaderID, 
		@retSalTcHeaderTextAName, 
		@retSalTcHeaderTextADescription,
		@retSalTcHeaderTextAText, 
		@retSalTcHeaderTextAHeadline,
		@retSalTcHeaderTextASortOrder,
		@retSalTcHeaderTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcHeaderID,
		[Name] = @retSalTcHeaderTextAName, 
		[Description] = @retSalTcHeaderTextADescription, 
		[Text] = @retSalTcHeaderTextAText,
		[Headline] = @retSalTcHeaderTextAHeadline,
		SortOrder = @retSalTcHeaderTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcHeaderTextAGuid
END
---- Create or update text B. Co-worker info
DECLARE @retSalTcHeaderTextBGuid UNIQUEIDENTIFIER = '1AF3A6D8-7A5E-4B0F-900B-C344EA001C91',
	@retSalTcHeaderTextBName NVARCHAR(MAX) = @prefix + ' Header, Co-worker',
	@retSalTcHeaderTextBDescription NVARCHAR(MAX) = '',
	@retSalTcHeaderTextBText NVARCHAR(MAX) = '<p><Todays Date - Long></p>
		<p><Co-worker First Name> <Co-worker Last Name></p>
		<p><Address Line 1><br />
		<Address Line 2> <State> <Postal Code><br />
		<Address Line 3><br />
		<br /><br />
		Dear <Co-worker First Name></p>',
	@retSalTcHeaderTextBHeadline NVARCHAR(MAX) = '',
	@retSalTcHeaderTextBSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcHeaderTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcHeaderID, 
		@retSalTcHeaderTextBName, 
		@retSalTcHeaderTextBDescription,
		@retSalTcHeaderTextBText, 
		@retSalTcHeaderTextBHeadline,
		@retSalTcHeaderTextBSortOrder,
		@retSalTcHeaderTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcHeaderID,
		[Name] = @retSalTcHeaderTextBName, 
		[Description] = @retSalTcHeaderTextBDescription, 
		[Text] = @retSalTcHeaderTextBText,
		[Headline] = @retSalTcHeaderTextBHeadline,
		SortOrder = @retSalTcHeaderTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcHeaderTextBGuid
END

-- Add header paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @retSalTcHeaderID, @counter
SET @counter = @counter + 1

-- #################################### Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalTcEmployGreetingGuid UNIQUEIDENTIFIER = 'A0BD0008-B463-4853-8A07-C0B19DA9EEAA',
	@cdpName NVARCHAR(MAX) = @prefix + ' Employment greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalTcEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @retSalTcEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalTcEmployGreetingGuid
END
DECLARE @retSalTcEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalTcEmployGreetingGuid)

---- Create or update text A, No contract end date
DECLARE @retSalTcEmployGreetingTextAGuid UNIQUEIDENTIFIER = 'BA6D3614-9C2D-4FE5-9B19-BB757885E1ED',
	@retSalTcEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Greeting, no change valid to date',
	@retSalTcEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextAText NVARCHAR(MAX) = 'We are delighted to confirm your appointment to the position of <Position Title (Local Job Name)> at IKEA and wish to confirm the terms and conditions of your employment.',
	@retSalTcEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcEmployGreetingID, 
		@retSalTcEmployGreetingTextAName, 
		@retSalTcEmployGreetingTextADescription,
		@retSalTcEmployGreetingTextAText, 
		@retSalTcEmployGreetingTextAHeadline,
		@retSalTcEmployGreetingTextASortOrder,
		@retSalTcEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcEmployGreetingID,
		[Name] = @retSalTcEmployGreetingTextAName, 
		[Description] = @retSalTcEmployGreetingTextADescription, 
		[Text] = @retSalTcEmployGreetingTextAText,
		[Headline] = @retSalTcEmployGreetingTextAHeadline,
		SortOrder = @retSalTcEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextAGuid
END
DECLARE @retSalTcEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @retSalTcEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = '888B07D9-353A-49C6-B2CB-AAA6DA6A0433',
	@retSalTcEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retSalTcEmployGreetingTextACondAValues NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextACondADescription NVARCHAR(MAX) = 'No change valid to date',
	@retSalTcEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextACondAGuid)
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
		@retSalTcEmployGreetingTextACondAGuid,
		@retSalTcEmployGreetingTextAID,
		@retSalTcEmployGreetingTextACondAPropertyName,
		@retSalTcEmployGreetingTextACondAOperator,
		@retSalTcEmployGreetingTextACondAValues,
		@retSalTcEmployGreetingTextACondADescription,
		@retSalTcEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcEmployGreetingTextAID,
		Property_Name = @retSalTcEmployGreetingTextACondAPropertyName,
		Operator = @retSalTcEmployGreetingTextACondAOperator,
		[Values] = @retSalTcEmployGreetingTextACondAValues,
		[Description] = @retSalTcEmployGreetingTextACondADescription,
		[Status] = @retSalTcEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextACondAGuid
END

---- Create or update text B, Change valid to date and has no extension to fixed term
DECLARE @retSalTcEmployGreetingTextBGuid UNIQUEIDENTIFIER = 'CCE45E91-9A72-4D39-9DB7-FF75DF8655F5',
	@retSalTcEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Greeting, B',
	@retSalTcEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextBText NVARCHAR(MAX) = 'We are delighted to confirm your appointment to the fixed term position of <Position Title (Local Job Name)> at IKEA, and wish to confirm the terms and conditions of your employment.',
	@retSalTcEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextBSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcEmployGreetingID, 
		@retSalTcEmployGreetingTextBName, 
		@retSalTcEmployGreetingTextBDescription,
		@retSalTcEmployGreetingTextBText, 
		@retSalTcEmployGreetingTextBHeadline,
		@retSalTcEmployGreetingTextBSortOrder,
		@retSalTcEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcEmployGreetingID,
		[Name] = @retSalTcEmployGreetingTextBName, 
		[Description] = @retSalTcEmployGreetingTextBDescription, 
		[Text] = @retSalTcEmployGreetingTextBText,
		[Headline] = @retSalTcEmployGreetingTextBHeadline,
		SortOrder = @retSalTcEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextBGuid
END
DECLARE @retSalTcEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextBGuid)

-- Create condition A for Text B, Has change valid to end date
DECLARE @retSalTcEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = 'E22A9E8C-983D-4839-A753-BC26CD7E3DC4',
	@retSalTcEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcEmployGreetingTextBCondAValues NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Has change valid to date',
	@retSalTcEmployGreetingTextBCondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextBCondAGuid)
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
		@retSalTcEmployGreetingTextBCondAGuid,
		@retSalTcEmployGreetingTextBID,
		@retSalTcEmployGreetingTextBCondAPropertyName,
		@retSalTcEmployGreetingTextBCondAOperator,
		@retSalTcEmployGreetingTextBCondAValues,
		@retSalTcEmployGreetingTextBCondADescription,
		@retSalTcEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcEmployGreetingTextBID,
		Property_Name = @retSalTcEmployGreetingTextBCondAPropertyName,
		Operator = @retSalTcEmployGreetingTextBCondAOperator,
		[Values] = @retSalTcEmployGreetingTextBCondAValues,
		[Description] = @retSalTcEmployGreetingTextBCondADescription,
		[Status] = @retSalTcEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextBCondAGuid
END


-- Create condition B for Text B, Has no extension to fixed term
DECLARE @retSalTcEmployGreetingTextBCondBGuid UNIQUEIDENTIFIER = '1F2E2F8C-A658-4C04-9859-27B80D5C6111',
	@retSalTcEmployGreetingTextBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ExtensionFixedTerm',
	@retSalTcEmployGreetingTextBCondBOperator NVARCHAR(MAX) = 'EqualOrEmpty',
	@retSalTcEmployGreetingTextBCondBValues NVARCHAR(MAX) = 'No',
	@retSalTcEmployGreetingTextBCondBDescription NVARCHAR(MAX) = 'No extension to fixed term',
	@retSalTcEmployGreetingTextBCondBStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextBCondBGuid)
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
		@retSalTcEmployGreetingTextBCondBGuid,
		@retSalTcEmployGreetingTextBID,
		@retSalTcEmployGreetingTextBCondBPropertyName,
		@retSalTcEmployGreetingTextBCondBOperator,
		@retSalTcEmployGreetingTextBCondBValues,
		@retSalTcEmployGreetingTextBCondBDescription,
		@retSalTcEmployGreetingTextBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcEmployGreetingTextBID,
		Property_Name = @retSalTcEmployGreetingTextBCondBPropertyName,
		Operator = @retSalTcEmployGreetingTextBCondBOperator,
		[Values] = @retSalTcEmployGreetingTextBCondBValues,
		[Description] = @retSalTcEmployGreetingTextBCondBDescription,
		[Status] = @retSalTcEmployGreetingTextBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextBCondBGuid
END


--Text C
DECLARE @retSalTcEmployGreetingTextCGuid UNIQUEIDENTIFIER = '1CD7DB6A-12B6-4808-AA0D-783CA8E1548D',
	@retSalTcEmployGreetingTextCName NVARCHAR(MAX) = @prefix + ' Greeting, C',
	@retSalTcEmployGreetingTextCDescription NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextCText NVARCHAR(MAX) = 'We are delighted to confirm the extension of your fixed term position of <Position Title (Local Job Name)> at IKEA, and wish to confirm the terms and conditions of your employment.',
	@retSalTcEmployGreetingTextCHeadline NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextCSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextCGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcEmployGreetingID, 
		@retSalTcEmployGreetingTextCName, 
		@retSalTcEmployGreetingTextCDescription,
		@retSalTcEmployGreetingTextCText, 
		@retSalTcEmployGreetingTextCHeadline,
		@retSalTcEmployGreetingTextCSortOrder,
		@retSalTcEmployGreetingTextCGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcEmployGreetingID,
		[Name] = @retSalTcEmployGreetingTextCName, 
		[Description] = @retSalTcEmployGreetingTextCDescription, 
		[Text] = @retSalTcEmployGreetingTextCText,
		[Headline] = @retSalTcEmployGreetingTextCHeadline,
		SortOrder = @retSalTcEmployGreetingTextCSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextCGuid
END
DECLARE @retSalTcEmployGreetingTextCID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcEmployGreetingTextCGuid)


-- Create condition A for Text C, Has change valid to end date
DECLARE @retSalTcEmployGreetingTextCCondAGuid UNIQUEIDENTIFIER = '6300EE79-CF90-4E6A-ACDA-350B006C9B1D',
	@retSalTcEmployGreetingTextCCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcEmployGreetingTextCCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcEmployGreetingTextCCondAValues NVARCHAR(MAX) = '',
	@retSalTcEmployGreetingTextCCondADescription NVARCHAR(MAX) = 'Has change valid to date',
	@retSalTcEmployGreetingTextCCondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextCCondAGuid)
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
		@retSalTcEmployGreetingTextCCondAGuid,
		@retSalTcEmployGreetingTextCID,
		@retSalTcEmployGreetingTextCCondAPropertyName,
		@retSalTcEmployGreetingTextCCondAOperator,
		@retSalTcEmployGreetingTextCCondAValues,
		@retSalTcEmployGreetingTextCCondADescription,
		@retSalTcEmployGreetingTextCCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcEmployGreetingTextCID,
		Property_Name = @retSalTcEmployGreetingTextCCondAPropertyName,
		Operator = @retSalTcEmployGreetingTextCCondAOperator,
		[Values] = @retSalTcEmployGreetingTextCCondAValues,
		[Description] = @retSalTcEmployGreetingTextCCondADescription,
		[Status] = @retSalTcEmployGreetingTextCCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextCCondAGuid
END


-- Create condition B for Text C, Has extension to fixed term
DECLARE @retSalTcEmployGreetingTextCCondBGuid UNIQUEIDENTIFIER = '2D0E9F79-735B-489C-8B17-00AE0E819652',
	@retSalTcEmployGreetingTextCCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ExtensionFixedTerm',
	@retSalTcEmployGreetingTextCCondBOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcEmployGreetingTextCCondBValues NVARCHAR(MAX) = 'Yes',
	@retSalTcEmployGreetingTextCCondBDescription NVARCHAR(MAX) = 'Has extension to fixed term',
	@retSalTcEmployGreetingTextCCondBStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextCCondBGuid)
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
		@retSalTcEmployGreetingTextCCondBGuid,
		@retSalTcEmployGreetingTextCID,
		@retSalTcEmployGreetingTextCCondBPropertyName,
		@retSalTcEmployGreetingTextCCondBOperator,
		@retSalTcEmployGreetingTextCCondBValues,
		@retSalTcEmployGreetingTextCCondBDescription,
		@retSalTcEmployGreetingTextCCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcEmployGreetingTextCID,
		Property_Name = @retSalTcEmployGreetingTextCCondBPropertyName,
		Operator = @retSalTcEmployGreetingTextCCondBOperator,
		[Values] = @retSalTcEmployGreetingTextCCondBValues,
		[Description] = @retSalTcEmployGreetingTextCCondBDescription,
		[Status] = @retSalTcEmployGreetingTextCCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcEmployGreetingTextCCondBGuid
END



-- Add paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @retSalTcEmployGreetingID, @counter
SET @counter = @counter + 1

-- #################################### Terms

DECLARE @termsCounter INT = 0

---- Create or update a terms paragraph
-- Paragraph guid
DECLARE @retSalTcTermsGuid UNIQUEIDENTIFIER = '7E6EF4C9-85BF-42E0-9712-E2FCE2E2C06E',
	@retSalTcTermsName NVARCHAR(MAX) = @prefix + ' Terms',
	@retSalTcTermsParagraphType INT = @ParagraphTypeTableNumeric,
	@termsDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalTcTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalTcTermsName, @termsDescription, @retSalTcTermsParagraphType, @retSalTcTermsGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalTcTermsName, [Description] = @termsDescription, ParagraphType = @retSalTcTermsParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalTcTermsGuid
END
DECLARE @retSalTcTermsID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalTcTermsGuid)

-- #################################### Position

---- Position A
DECLARE @retSalTcTermsPositionAGuid UNIQUEIDENTIFIER = '413683D9-1521-45A6-9687-6C65CA7CC698',
	@retSalTcTermsPositionAName NVARCHAR(MAX) = @prefix + ' Position, full time',
	@retSalTcTermsPositionADescription NVARCHAR(MAX) = '',
	@retSalTcTermsPositionAText NVARCHAR(MAX) = 'Your position is Full Time <Position Title (Local Job Name)>, located at <Business Unit>, reporting to  <Position Title (Local Job Name) of Reports To Line Manager>.  Your position (in terms of your duties and responsibilities), and location may be varied from time to time in accordance with IKEA’s needs.  ',
	@retSalTcTermsPositionAHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@retSalTcTermsPositionASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsPositionAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsPositionAName, 
		@retSalTcTermsPositionADescription,
		@retSalTcTermsPositionAText, 
		@retSalTcTermsPositionAHeadline,
		@retSalTcTermsPositionASortOrder,
		@retSalTcTermsPositionAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsPositionAName, 
		[Description] = @retSalTcTermsPositionADescription, 
		[Text] = @retSalTcTermsPositionAText,
		[Headline] = @retSalTcTermsPositionAHeadline,
		SortOrder = @retSalTcTermsPositionASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPositionAGuid
END
DECLARE @retSalTcTermsPositionAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPositionAGuid)
SET @termsCounter = @termsCounter + 1

-- Create condition for position A
DECLARE @retSalTcTermsPositionACondGuid UNIQUEIDENTIFIER = '865AFB25-06DE-42BA-AB21-02220A9FE83D',
	@retSalTcTermsPositionACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalTcTermsPositionACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsPositionACondValues NVARCHAR(MAX) = '76',
	@retSalTcTermsPositionACondDescription NVARCHAR(MAX) = 'Is full time',
	@retSalTcTermsPositionACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsPositionACondGuid)
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
		@retSalTcTermsPositionACondGuid,
		@retSalTcTermsPositionAID,
		@retSalTcTermsPositionACondPropertyName,
		@retSalTcTermsPositionACondOperator,
		@retSalTcTermsPositionACondValues,
		@retSalTcTermsPositionACondDescription,
		@retSalTcTermsPositionACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsPositionAID,
		Property_Name = @retSalTcTermsPositionACondPropertyName,
		Operator = @retSalTcTermsPositionACondOperator,
		[Values] = @retSalTcTermsPositionACondValues,
		[Description] = @retSalTcTermsPositionACondDescription,
		[Status] = @retSalTcTermsPositionACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsPositionACondGuid
END

---- Position B
DECLARE @retSalTcTermsPositionBGuid UNIQUEIDENTIFIER = '186EFA0E-BEA7-432E-BB73-4D48229D20DA',
	@retSalTcTermsPositionBName NVARCHAR(MAX) = @prefix + ' Position, part time',
	@retSalTcTermsPositionBDescription NVARCHAR(MAX) = '',
	@retSalTcTermsPositionBText NVARCHAR(MAX) = 'Your position is Part Time <Position Title (Local Job Name)>, located at <Business Unit>, reporting to  <Position Title (Local Job Name) of Reports To Line Manager>.  Your position (in terms of your duties and responsibilities), and location may be varied from time to time in accordance with IKEA’s needs',
	@retSalTcTermsPositionBHeadline NVARCHAR(MAX) = '<i>Position</i>',
	@retSalTcTermsPositionBSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsPositionBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsPositionBName, 
		@retSalTcTermsPositionBDescription,
		@retSalTcTermsPositionBText, 
		@retSalTcTermsPositionBHeadline,
		@retSalTcTermsPositionBSortOrder,
		@retSalTcTermsPositionBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsPositionBName, 
		[Description] = @retSalTcTermsPositionBDescription, 
		[Text] = @retSalTcTermsPositionBText,
		[Headline] = @retSalTcTermsPositionBHeadline,
		SortOrder = @retSalTcTermsPositionBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPositionBGuid
END
DECLARE @retSalTcTermsPositionBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPositionBGuid)

-- Create condition for position A
DECLARE @retSalTcTermsPositionBCondGuid UNIQUEIDENTIFIER = 'A1267D10-CE89-4725-AEAF-102DB6FD3EC0',
	@retSalTcTermsPositionBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalTcTermsPositionBCondOperator NVARCHAR(MAX) = 'LessThan',
	@retSalTcTermsPositionBCondValues NVARCHAR(MAX) = '76',
	@retSalTcTermsPositionBCondDescription NVARCHAR(MAX) = 'Is part time',
	@retSalTcTermsPositionBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsPositionBCondGuid)
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
		@retSalTcTermsPositionBCondGuid,
		@retSalTcTermsPositionBID,
		@retSalTcTermsPositionBCondPropertyName,
		@retSalTcTermsPositionBCondOperator,
		@retSalTcTermsPositionBCondValues,
		@retSalTcTermsPositionBCondDescription,
		@retSalTcTermsPositionBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsPositionBID,
		Property_Name = @retSalTcTermsPositionBCondPropertyName,
		Operator = @retSalTcTermsPositionBCondOperator,
		[Values] = @retSalTcTermsPositionBCondValues,
		[Description] = @retSalTcTermsPositionBCondDescription,
		[Status] = @retSalTcTermsPositionBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsPositionBCondGuid
END

-- #################################### Commencement Date

---- Commencement A,  No change valid to date
DECLARE @retSalTcTermsComAGuid UNIQUEIDENTIFIER = '9B15024D-F436-4923-978C-25E6FCDAD07D',
	@retSalTcTermsComAName NVARCHAR(MAX) = @prefix + ' Commencement, A',
	@retSalTcTermsComADescription NVARCHAR(MAX) = '',
	@retSalTcTermsComAText NVARCHAR(MAX) = 'Your date of commencement will be <Change Valid From>.',
	@retSalTcTermsComAHeadline NVARCHAR(MAX) = 'Commencement Date',
	@retSalTcTermsComASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsComAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsComAName, 
		@retSalTcTermsComADescription,
		@retSalTcTermsComAText, 
		@retSalTcTermsComAHeadline,
		@retSalTcTermsComASortOrder,
		@retSalTcTermsComAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsComAName, 
		[Description] = @retSalTcTermsComADescription, 
		[Text] = @retSalTcTermsComAText,
		[Headline] = @retSalTcTermsComAHeadline,
		SortOrder = @retSalTcTermsComASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComAGuid
END
DECLARE @retSalTcTermsComAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComAGuid)

-- Create condition for Commencement A, Condition A, No change valid to date
DECLARE @retSalTcTermsComACondAGuid UNIQUEIDENTIFIER = '3334A6E7-0949-49F0-9BD0-F16C0DE402F3',
	@retSalTcTermsComACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsComACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retSalTcTermsComACondAValues NVARCHAR(MAX) = '',
	@retSalTcTermsComACondADescription NVARCHAR(MAX) = 'No change ValidTo date',
	@retSalTcTermsComACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComACondAGuid)
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
		@retSalTcTermsComACondAGuid,
		@retSalTcTermsComAID,
		@retSalTcTermsComACondAPropertyName,
		@retSalTcTermsComACondAOperator,
		@retSalTcTermsComACondAValues,
		@retSalTcTermsComACondADescription,
		@retSalTcTermsComACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComAID,
		Property_Name = @retSalTcTermsComACondAPropertyName,
		Operator = @retSalTcTermsComACondAOperator,
		[Values] = @retSalTcTermsComACondAValues,
		[Description] = @retSalTcTermsComACondADescription,
		[Status] = @retSalTcTermsComACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComACondAGuid
END

---- Commencement B,  Has change valid to date, no additional clause
DECLARE @retSalTcTermsComBGuid UNIQUEIDENTIFIER = '6B40FCB6-78AE-4ECF-B644-546030FA2FCF',
	@retSalTcTermsComBName NVARCHAR(MAX) = @prefix + ' Commencement, B',
	@retSalTcTermsComBDescription NVARCHAR(MAX) = '',
	@retSalTcTermsComBText NVARCHAR(MAX) = 'This is a limited tenure contract. The contract is effective from <Change Valid From> and will cease on <Change Valid To>, unless otherwise terminated in accordance with this contract.',
	@retSalTcTermsComBHeadline NVARCHAR(MAX) = 'Commencement date',
	@retSalTcTermsComBSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsComBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsComBName, 
		@retSalTcTermsComBDescription,
		@retSalTcTermsComBText, 
		@retSalTcTermsComBHeadline,
		@retSalTcTermsComBSortOrder,
		@retSalTcTermsComBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsComBName, 
		[Description] = @retSalTcTermsComBDescription, 
		[Text] = @retSalTcTermsComBText,
		[Headline] = @retSalTcTermsComBHeadline,
		SortOrder = @retSalTcTermsComBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComBGuid
END
DECLARE @retSalTcTermsComBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComBGuid)

-- Create condition A for Commence B, has change valid to
DECLARE @retSalTcTermsComBCondAGuid UNIQUEIDENTIFIER = '0F77A915-6609-4F1F-91CA-570B509456FF',
	@retSalTcTermsComBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsComBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsComBCondAValues NVARCHAR(MAX) = '',
	@retSalTcTermsComBCondADescription NVARCHAR(MAX) = 'Has change valid to',
	@retSalTcTermsComBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComBCondAGuid)
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
		@retSalTcTermsComBCondAGuid,
		@retSalTcTermsComBID,
		@retSalTcTermsComBCondAPropertyName,
		@retSalTcTermsComBCondAOperator,
		@retSalTcTermsComBCondAValues,
		@retSalTcTermsComBCondADescription,
		@retSalTcTermsComBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComBID,
		Property_Name = @retSalTcTermsComBCondAPropertyName,
		Operator = @retSalTcTermsComBCondAOperator,
		[Values] = @retSalTcTermsComBCondAValues,
		[Description] = @retSalTcTermsComBCondADescription,
		[Status] = @retSalTcTermsComBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComBCondAGuid
END

-- Create condition B for Commence B, has no additional clause
DECLARE @retSalTcTermsComBCondBGuid UNIQUEIDENTIFIER = 'DF8F2BB4-EF15-426C-8C6E-88C0542C6711',
	@retSalTcTermsComBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_AdditionalClause',
	@retSalTcTermsComBCondBOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsComBCondBValues NVARCHAR(MAX) = 'None',
	@retSalTcTermsComBCondBDescription NVARCHAR(MAX) = 'Has no additional clause',
	@retSalTcTermsComBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComBCondBGuid)
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
		@retSalTcTermsComBCondBGuid,
		@retSalTcTermsComBID,
		@retSalTcTermsComBCondBPropertyName,
		@retSalTcTermsComBCondBOperator,
		@retSalTcTermsComBCondBValues,
		@retSalTcTermsComBCondBDescription,
		@retSalTcTermsComBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComBID,
		Property_Name = @retSalTcTermsComBCondBPropertyName,
		Operator = @retSalTcTermsComBCondBOperator,
		[Values] = @retSalTcTermsComBCondBValues,
		[Description] = @retSalTcTermsComBCondBDescription,
		[Status] = @retSalTcTermsComBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComBCondBGuid
END

---- Commencement C,  Has change valid to date, additional clause Homecoming - Same role
DECLARE @retSalTcTermsComCGuid UNIQUEIDENTIFIER = '4C77E3D4-9BE4-4D14-AE37-843A60FF377A',
	@retSalTcTermsComCName NVARCHAR(MAX) = @prefix + ' Commencement, C',
	@retSalTcTermsComCDescription NVARCHAR(MAX) = '',
	@retSalTcTermsComCText NVARCHAR(MAX) = 'This is a limited tenure contract. The contract is effective from <Change Valid From> and will cease on <Change Valid To>, unless otherwise terminated in accordance with this contract.<br>
<br>
At the end of this contract you will return to your current position.',
	@retSalTcTermsComCHeadline NVARCHAR(MAX) = 'Commencement date',
	@retSalTcTermsComCSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsComCGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsComCName, 
		@retSalTcTermsComCDescription,
		@retSalTcTermsComCText, 
		@retSalTcTermsComCHeadline,
		@retSalTcTermsComCSortOrder,
		@retSalTcTermsComCGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsComCName, 
		[Description] = @retSalTcTermsComCDescription, 
		[Text] = @retSalTcTermsComCText,
		[Headline] = @retSalTcTermsComCHeadline,
		SortOrder = @retSalTcTermsComCSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComCGuid
END
DECLARE @retSalTcTermsComCID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComCGuid)

-- Create condition A for Commence C, has change valid to
DECLARE @retSalTcTermsComCCondAGuid UNIQUEIDENTIFIER = 'AC2356B1-C0C7-4EAD-BA28-6E0B3E7A366A',
	@retSalTcTermsComCCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsComCCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsComCCondAValues NVARCHAR(MAX) = '',
	@retSalTcTermsComCCondADescription NVARCHAR(MAX) = 'Has change valid to',
	@retSalTcTermsComCCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComCCondAGuid)
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
		@retSalTcTermsComCCondAGuid,
		@retSalTcTermsComCID,
		@retSalTcTermsComCCondAPropertyName,
		@retSalTcTermsComCCondAOperator,
		@retSalTcTermsComCCondAValues,
		@retSalTcTermsComCCondADescription,
		@retSalTcTermsComCCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComCID,
		Property_Name = @retSalTcTermsComCCondAPropertyName,
		Operator = @retSalTcTermsComCCondAOperator,
		[Values] = @retSalTcTermsComCCondAValues,
		[Description] = @retSalTcTermsComCCondADescription,
		[Status] = @retSalTcTermsComCCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComCCondAGuid
END

-- Create condition B for Commence C, has additional clause "Homecoming - Same Role"
DECLARE @retSalTcTermsComCCondBGuid UNIQUEIDENTIFIER = 'C915DC82-AB45-4AFF-A5DA-4901CAC34148',
	@retSalTcTermsComCCondBPropertyName NVARCHAR(MAX) = 'extendedcase_AdditionalClause',
	@retSalTcTermsComCCondBOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsComCCondBValues NVARCHAR(MAX) = 'Homecoming - Same Role',
	@retSalTcTermsComCCondBDescription NVARCHAR(MAX) = 'Has additional clause Homecoming - Same Role',
	@retSalTcTermsComCCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComCCondBGuid)
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
		@retSalTcTermsComCCondBGuid,
		@retSalTcTermsComCID,
		@retSalTcTermsComCCondBPropertyName,
		@retSalTcTermsComCCondBOperator,
		@retSalTcTermsComCCondBValues,
		@retSalTcTermsComCCondBDescription,
		@retSalTcTermsComCCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComCID,
		Property_Name = @retSalTcTermsComCCondBPropertyName,
		Operator = @retSalTcTermsComCCondBOperator,
		[Values] = @retSalTcTermsComCCondBValues,
		[Description] = @retSalTcTermsComCCondBDescription,
		[Status] = @retSalTcTermsComCCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComCCondBGuid
END

---- Commencement D, Has change valid to date, additional clause "Homecoming - Similar role"
DECLARE @retSalTcTermsComDGuid UNIQUEIDENTIFIER = 'E51466CA-EFEE-4331-B990-4CD7D4A4BF56',
	@retSalTcTermsComDName NVARCHAR(MAX) = @prefix + ' Commencement, D',
	@retSalTcTermsComDDescription NVARCHAR(MAX) = '',
	@retSalTcTermsComDText NVARCHAR(MAX) = 'This is a limited tenure contract. The contract is effective from <Change Valid From> and will cease on <Change Valid To>, unless otherwise terminated in accordance with this contract.<br>
<br>
Prior to the end of this temporary contract we encourage you to review the job competency profiles for current vacancies available within IKEA and to apply for any positions which interest you and for which you meet the job requirements.<br>
<br>
Should you not secure a new permanent or temporary role prior to the end of this contract IKEA will appoint you to a position within your skills and competence at the same (or higher) Position Class.',
	@retSalTcTermsComDHeadline NVARCHAR(MAX) = 'Commencement date',
	@retSalTcTermsComDSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsComDGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsComDName, 
		@retSalTcTermsComDDescription,
		@retSalTcTermsComDText, 
		@retSalTcTermsComDHeadline,
		@retSalTcTermsComDSortOrder,
		@retSalTcTermsComDGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsComDName, 
		[Description] = @retSalTcTermsComDDescription, 
		[Text] = @retSalTcTermsComDText,
		[Headline] = @retSalTcTermsComDHeadline,
		SortOrder = @retSalTcTermsComDSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComDGuid
END
DECLARE @retSalTcTermsComDID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComDGuid)

-- Create condition A for Commence D, has change valid to
DECLARE @retSalTcTermsComDCondAGuid UNIQUEIDENTIFIER = '096B0DB9-5A72-47B3-ADB1-EB3A0EBAE4C8',
	@retSalTcTermsComDCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsComDCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsComDCondAValues NVARCHAR(MAX) = '',
	@retSalTcTermsComDCondADescription NVARCHAR(MAX) = 'Has change valid to',
	@retSalTcTermsComDCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComDCondAGuid)
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
		@retSalTcTermsComDCondAGuid,
		@retSalTcTermsComDID,
		@retSalTcTermsComDCondAPropertyName,
		@retSalTcTermsComDCondAOperator,
		@retSalTcTermsComDCondAValues,
		@retSalTcTermsComDCondADescription,
		@retSalTcTermsComDCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComDID,
		Property_Name = @retSalTcTermsComDCondAPropertyName,
		Operator = @retSalTcTermsComDCondAOperator,
		[Values] = @retSalTcTermsComDCondAValues,
		[Description] = @retSalTcTermsComDCondADescription,
		[Status] = @retSalTcTermsComDCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComDCondAGuid
END

-- Create condition B for Commence D, has additional clause "Homecoming - Similar role"
DECLARE @retSalTcTermsComDCondBGuid UNIQUEIDENTIFIER = 'CEBD02D8-D902-44F4-A934-6F5D7A87857C',
	@retSalTcTermsComDCondBPropertyName NVARCHAR(MAX) = 'extendedcase_AdditionalClause',
	@retSalTcTermsComDCondBOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsComDCondBValues NVARCHAR(MAX) = 'Homecoming - Similar role',
	@retSalTcTermsComDCondBDescription NVARCHAR(MAX) = 'Has additional clause Homecoming - Similar role',
	@retSalTcTermsComDCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComDCondBGuid)
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
		@retSalTcTermsComDCondBGuid,
		@retSalTcTermsComDID,
		@retSalTcTermsComDCondBPropertyName,
		@retSalTcTermsComDCondBOperator,
		@retSalTcTermsComDCondBValues,
		@retSalTcTermsComDCondBDescription,
		@retSalTcTermsComDCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComDID,
		Property_Name = @retSalTcTermsComDCondBPropertyName,
		Operator = @retSalTcTermsComDCondBOperator,
		[Values] = @retSalTcTermsComDCondBValues,
		[Description] = @retSalTcTermsComDCondBDescription,
		[Status] = @retSalTcTermsComDCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComDCondBGuid
END

---- Commencement E, Has change valid to date, additional clause "Parental Leave Cover - Same Role"
DECLARE @retSalTcTermsComEGuid UNIQUEIDENTIFIER = '063C0D80-1224-4741-BA49-AB35A46906C5',
	@retSalTcTermsComEName NVARCHAR(MAX) = @prefix + ' Commencement, E',
	@retSalTcTermsComEDescription NVARCHAR(MAX) = '',
	@retSalTcTermsComEText NVARCHAR(MAX) = 'This is a fixed term contract to replace a co-worker on a period of parental leave.<br>
<br>
This contract is effective from <Change Valid From> and will cease on <Change Valid To>.<br>
<br>
In the event that the co-worker on Parental Leave advises IKEA they intend to return to work prior to the end of their approved period of parental leave, IKEA may terminate this contract with 4 weeks’ notice at which time you will return to the position held prior to commencement of this fixed term contract, at the applicable rate of pay.',
	@retSalTcTermsComEHeadline NVARCHAR(MAX) = 'Commencement date',
	@retSalTcTermsComESortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsComEGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsComEName, 
		@retSalTcTermsComEDescription,
		@retSalTcTermsComEText, 
		@retSalTcTermsComEHeadline,
		@retSalTcTermsComESortOrder,
		@retSalTcTermsComEGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsComEName, 
		[Description] = @retSalTcTermsComEDescription, 
		[Text] = @retSalTcTermsComEText,
		[Headline] = @retSalTcTermsComEHeadline,
		SortOrder = @retSalTcTermsComESortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComEGuid
END
DECLARE @retSalTcTermsComEID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComEGuid)

-- Create condition A for Commence E, has change valid to
DECLARE @retSalTcTermsComECondAGuid UNIQUEIDENTIFIER = '34FB0441-F5C5-4F0E-AD75-43B3DC0DD0FB',
	@retSalTcTermsComECondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsComECondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsComECondAValues NVARCHAR(MAX) = '',
	@retSalTcTermsComECondADescription NVARCHAR(MAX) = 'Has change valid to',
	@retSalTcTermsComECondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComECondAGuid)
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
		@retSalTcTermsComECondAGuid,
		@retSalTcTermsComEID,
		@retSalTcTermsComECondAPropertyName,
		@retSalTcTermsComECondAOperator,
		@retSalTcTermsComECondAValues,
		@retSalTcTermsComECondADescription,
		@retSalTcTermsComECondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComEID,
		Property_Name = @retSalTcTermsComECondAPropertyName,
		Operator = @retSalTcTermsComECondAOperator,
		[Values] = @retSalTcTermsComECondAValues,
		[Description] = @retSalTcTermsComECondADescription,
		[Status] = @retSalTcTermsComECondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComECondAGuid
END

-- Create condition B for Commence E, has additional clause "Parental Leave Cover - Same Role"
DECLARE @retSalTcTermsComECondBGuid UNIQUEIDENTIFIER = '65E63293-A4D9-47DA-8AFC-E391A38A9E85',
	@retSalTcTermsComECondBPropertyName NVARCHAR(MAX) = 'extendedcase_AdditionalClause',
	@retSalTcTermsComECondBOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsComECondBValues NVARCHAR(MAX) = 'Parental Leave Cover - Same Role',
	@retSalTcTermsComECondBDescription NVARCHAR(MAX) = 'Has additional clause Parental Leave Cover - Same Role',
	@retSalTcTermsComECondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComECondBGuid)
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
		@retSalTcTermsComECondBGuid,
		@retSalTcTermsComEID,
		@retSalTcTermsComECondBPropertyName,
		@retSalTcTermsComECondBOperator,
		@retSalTcTermsComECondBValues,
		@retSalTcTermsComECondBDescription,
		@retSalTcTermsComECondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComEID,
		Property_Name = @retSalTcTermsComECondBPropertyName,
		Operator = @retSalTcTermsComECondBOperator,
		[Values] = @retSalTcTermsComECondBValues,
		[Description] = @retSalTcTermsComECondBDescription,
		[Status] = @retSalTcTermsComECondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComECondBGuid
END


---- Commencement F, Has change valid to date, additional clause "Parental Leave Cover - Similar Role"
DECLARE @retSalTcTermsComFGuid UNIQUEIDENTIFIER = '57564E8B-E857-4D26-8874-5C1C8365E277',
	@retSalTcTermsComFName NVARCHAR(MAX) = @prefix + ' Commencement, F',
	@retSalTcTermsComFDescription NVARCHAR(MAX) = '',
	@retSalTcTermsComFText NVARCHAR(MAX) = 'This is a fixed term contract to replace a co-worker on a period of parental leave.<br>
<br>
This contract is effective from <Change Valid From> and will cease on <Change Valid To>.<br>
<br>
In the event that the co-worker on Parental Leave advises IKEA they intend to return to work prior to the end of their approved period of parental leave, IKEA may terminate this contract with 4 weeks’ notice at which time you will return to a similar role prior to commencement of this fixed term contract, at the applicable rate of pay.',
	@retSalTcTermsComFHeadline NVARCHAR(MAX) = 'Commencement date',
	@retSalTcTermsComFSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsComFGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsComFName, 
		@retSalTcTermsComFDescription,
		@retSalTcTermsComFText, 
		@retSalTcTermsComFHeadline,
		@retSalTcTermsComFSortOrder,
		@retSalTcTermsComFGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsComFName, 
		[Description] = @retSalTcTermsComFDescription, 
		[Text] = @retSalTcTermsComFText,
		[Headline] = @retSalTcTermsComFHeadline,
		SortOrder = @retSalTcTermsComFSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComFGuid
END
DECLARE @retSalTcTermsComFID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsComFGuid)

-- Create condition A for Commence F, has change valid to
DECLARE @retSalTcTermsComFCondAGuid UNIQUEIDENTIFIER = '1C4E4407-5ADC-4DBA-8F97-2DFF669E1364',
	@retSalTcTermsComFCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsComFCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsComFCondAValues NVARCHAR(MAX) = '',
	@retSalTcTermsComFCondADescription NVARCHAR(MAX) = 'Has change valid to',
	@retSalTcTermsComFCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComFCondAGuid)
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
		@retSalTcTermsComFCondAGuid,
		@retSalTcTermsComFID,
		@retSalTcTermsComFCondAPropertyName,
		@retSalTcTermsComFCondAOperator,
		@retSalTcTermsComFCondAValues,
		@retSalTcTermsComFCondADescription,
		@retSalTcTermsComFCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComFID,
		Property_Name = @retSalTcTermsComFCondAPropertyName,
		Operator = @retSalTcTermsComFCondAOperator,
		[Values] = @retSalTcTermsComFCondAValues,
		[Description] = @retSalTcTermsComFCondADescription,
		[Status] = @retSalTcTermsComFCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComFCondAGuid
END

-- Create condition B for Commence F, has additional clause "Parental Leave Cover - Similar Role"
DECLARE @retSalTcTermsComFCondBGuid UNIQUEIDENTIFIER = 'BA23B46D-F6A6-4C38-A841-0C567C27AB51',
	@retSalTcTermsComFCondBPropertyName NVARCHAR(MAX) = 'extendedcase_AdditionalClause',
	@retSalTcTermsComFCondBOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsComFCondBValues NVARCHAR(MAX) = 'Parental Leave Cover - Similar Role',
	@retSalTcTermsComFCondBDescription NVARCHAR(MAX) = 'Has additional clause Parental Leave Cover - Similar Role',
	@retSalTcTermsComFCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsComFCondBGuid)
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
		@retSalTcTermsComFCondBGuid,
		@retSalTcTermsComFID,
		@retSalTcTermsComFCondBPropertyName,
		@retSalTcTermsComFCondBOperator,
		@retSalTcTermsComFCondBValues,
		@retSalTcTermsComFCondBDescription,
		@retSalTcTermsComFCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsComFID,
		Property_Name = @retSalTcTermsComFCondBPropertyName,
		Operator = @retSalTcTermsComFCondBOperator,
		[Values] = @retSalTcTermsComFCondBValues,
		[Description] = @retSalTcTermsComFCondBDescription,
		[Status] = @retSalTcTermsComFCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsComFCondBGuid
END




-- #################################### Remuneration
DECLARE @retSalTcTermsRemunGuid UNIQUEIDENTIFIER = 'EA40B6B0-59E8-484B-9680-D48E8727A84E',
	@retSalTcTermsRemunName NVARCHAR(MAX) = @prefix + ' Remuneration',
	@retSalTcTermsRemunDescription NVARCHAR(MAX) = '',
	@retSalTcTermsRemunText NVARCHAR(MAX) = 'Your total remuneration per annum including superannuation is $<Basic Pay Amount>.<br>
	<br>
Your salary will be paid directly into your nominated bank account on a fortnightly basis, two weeks in arrears. ',
	@retSalTcTermsRemunHeadline NVARCHAR(MAX) = 'Remuneration',
	@retSalTcTermsRemunSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsRemunGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsRemunName, 
		@retSalTcTermsRemunDescription,
		@retSalTcTermsRemunText, 
		@retSalTcTermsRemunHeadline,
		@retSalTcTermsRemunSortOrder,
		@retSalTcTermsRemunGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsRemunName, 
		[Description] = @retSalTcTermsRemunDescription, 
		[Text] = @retSalTcTermsRemunText,
		[Headline] = @retSalTcTermsRemunHeadline,
		SortOrder = @retSalTcTermsRemunSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsRemunGuid
END

-- #################################### Superannuation
DECLARE @retSalTcTermsSuperGuid UNIQUEIDENTIFIER = '7CAC3B64-8051-4D84-A9E2-3713C981A795',
	@retSalTcTermsSuperName NVARCHAR(MAX) = @prefix + ' Superannuation',
	@retSalTcTermsSuperDescription NVARCHAR(MAX) = '',
	@retSalTcTermsSuperText NVARCHAR(MAX) = 'IKEA will make superannuation contributions, on your behalf, to the IKEA default Superannuation Fund, at the rate payable under the Superannuation Guarantee Legislation (SGL). This rate is currently 9.5% of your wage/salary.<br>
	<br>
IKEA’s current employer superannuation fund is the Retail Employees Superannuation Trust (REST), which is the fund into which the superannuation contributions will be made unless an alternate fund is nominated by you in writing, in accordance with the Superannuation Guarantee Legislation (SGL).<br>
<br>
It is your responsibility to nominate a Super Fund for your contributions to be made to, and to ensure that you complete the necessary paperwork for enrolment into your nominated fund.  IKEA will supply you with a REST Member Guide, including an application form.<br>',
	@retSalTcTermsSuperHeadline NVARCHAR(MAX) = 'Superannuation',
	@retSalTcTermsSuperSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsSuperGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsSuperName, 
		@retSalTcTermsSuperDescription,
		@retSalTcTermsSuperText, 
		@retSalTcTermsSuperHeadline,
		@retSalTcTermsSuperSortOrder,
		@retSalTcTermsSuperGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsSuperName, 
		[Description] = @retSalTcTermsSuperDescription, 
		[Text] = @retSalTcTermsSuperText,
		[Headline] = @retSalTcTermsSuperHeadline,
		SortOrder = @retSalTcTermsSuperSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsSuperGuid
END


-- #################################### 13a-b Hours of Work
--TODO: Fix Work A
---- Hours of Work A
DECLARE @retSalTcTermsHWAGuid UNIQUEIDENTIFIER = '45296772-BE49-4A2C-95BD-5521A6017F23',
	@retSalTcTermsHWAName NVARCHAR(MAX) = @prefix + ' Hours of Work, A',
	@retSalTcTermsHWADescription NVARCHAR(MAX) = '',
	@retSalTcTermsHWAText NVARCHAR(MAX) = 
	'Your contracted hours will be <Contracted Hours> hours, plus reasonable additional hours, per fortnight, worked on a rotating 14-day roster.  This roster will include some late night and weekend work.<br>
<br>
Your level of salary takes into account these additional hours, which may be required from time to time to fulfill the responsibilities of your role.<br>
<br>
If you are a part-time co-worker, the days and times for your part-time arrangement are to be discussed and agreed with your manager in writing.  If an additional day/s are worked upon mutual agreement with your direct manager these will be paid at the normal rate of pay.',
	@retSalTcTermsHWAHeadline NVARCHAR(MAX) = 'Hours of Work',
	@retSalTcTermsHWASortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsHWAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsHWAName, 
		@retSalTcTermsHWADescription,
		@retSalTcTermsHWAText, 
		@retSalTcTermsHWAHeadline,
		@retSalTcTermsHWASortOrder,
		@retSalTcTermsHWAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsHWAName, 
		[Description] = @retSalTcTermsHWADescription, 
		[Text] = @retSalTcTermsHWAText,
		[Headline] = @retSalTcTermsHWAHeadline,
		SortOrder = @retSalTcTermsHWASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsHWAGuid
END
DECLARE @retSalTcTermsHWAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsHWAGuid)

-- TODO: Fix condition A (not a list of departments)
-- Create condition for Hours of Work A
DECLARE @retSalTcTermsHWACondGuid UNIQUEIDENTIFIER = 'C5C4C06B-AAE0-4D56-8C5D-AA2D556B07FC',
	@retSalTcTermsHWACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalTcTermsHWACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsHWACondValues NVARCHAR(MAX) = '76',
	@retSalTcTermsHWACondDescription NVARCHAR(MAX) = 'Is full time',
	@retSalTcTermsHWACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsHWACondGuid)
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
		@retSalTcTermsHWACondGuid,
		@retSalTcTermsHWAID,
		@retSalTcTermsHWACondPropertyName,
		@retSalTcTermsHWACondOperator,
		@retSalTcTermsHWACondValues,
		@retSalTcTermsHWACondDescription,
		@retSalTcTermsHWACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsHWAID,
		Property_Name = @retSalTcTermsHWACondPropertyName,
		Operator = @retSalTcTermsHWACondOperator,
		[Values] = @retSalTcTermsHWACondValues,
		[Description] = @retSalTcTermsHWACondDescription,
		[Status] = @retSalTcTermsHWACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsHWACondGuid
END

-- TODO: Fix Work B
---- Hours of Work B
DECLARE @retSalTcTermsHWBGuid UNIQUEIDENTIFIER = '1596C6A3-57CC-49FA-AF47-7E7999294D6A',
	@retSalTcTermsHWBName NVARCHAR(MAX) = @prefix + ' Hours of Work, B',
	@retSalTcTermsHWBDescription NVARCHAR(MAX) = '',
	@retSalTcTermsHWBText NVARCHAR(MAX) = 
	'Your contracted hours of work will be <Contracted Hours> hours between Monday to Friday per fortnight.  This may include some late night and weekend work from time to time during peak periods.<br>
	<br>
As a salaried co-worker you are paid for the job and therefore your level of Remuneration takes into account these additional hours, which may be require to fulfill the responsibilities of your role.<br>
<br>
If you are a part-time co-worker, the days and times for your part-time arrangement are to be discussed and agreed with your manager in writing.  If an additional day/s are worked upon mutual agreement with your direct manager these will be paid at the normal rate of pay.',
	@retSalTcTermsHWBHeadline NVARCHAR(MAX) = 'Hours of Work',
	@retSalTcTermsHWBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsHWBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsHWBName, 
		@retSalTcTermsHWBDescription,
		@retSalTcTermsHWBText, 
		@retSalTcTermsHWBHeadline,
		@retSalTcTermsHWBSortOrder,
		@retSalTcTermsHWBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsHWBName, 
		[Description] = @retSalTcTermsHWBDescription, 
		[Text] = @retSalTcTermsHWBText,
		[Headline] = @retSalTcTermsHWBHeadline,
		SortOrder = @retSalTcTermsHWBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsHWBGuid
END
DECLARE @retSalTcTermsHWBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsHWBGuid)

-- Create condition for hours of work B
DECLARE @retSalTcTermsHWBCondGuid UNIQUEIDENTIFIER = 'BD5939D5-FFA8-418A-95B9-7A5699C4796A',
	@retSalTcTermsHWBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalTcTermsHWBCondOperator NVARCHAR(MAX) = 'LessThan',
	@retSalTcTermsHWBCondValues NVARCHAR(MAX) = '76',
	@retSalTcTermsHWBCondDescription NVARCHAR(MAX) = 'Is part time',
	@retSalTcTermsHWBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsHWBCondGuid)
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
		@retSalTcTermsHWBCondGuid,
		@retSalTcTermsHWBID,
		@retSalTcTermsHWBCondPropertyName,
		@retSalTcTermsHWBCondOperator,
		@retSalTcTermsHWBCondValues,
		@retSalTcTermsHWBCondDescription,
		@retSalTcTermsHWBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsHWBID,
		Property_Name = @retSalTcTermsHWBCondPropertyName,
		Operator = @retSalTcTermsHWBCondOperator,
		[Values] = @retSalTcTermsHWBCondValues,
		[Description] = @retSalTcTermsHWBCondDescription,
		[Status] = @retSalTcTermsHWBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsHWBCondGuid
END


-- #################################### Probationary Period

---- Probationary Period
DECLARE @retSalTcTermsProbTimeGuid UNIQUEIDENTIFIER = 'BD705255-A708-44A1-AF78-C520E05F8320',
	@retSalTcTermsProbTimeName NVARCHAR(MAX) = @prefix + ' Probationary Period',
	@retSalTcTermsProbTimeDescription NVARCHAR(MAX) = '',
	@retSalTcTermsProbTimeText NVARCHAR(MAX) = 'IKEA offers this employment to you on a probationary basis for a period of six (6) months, during which time your performance standards will be subject to regular review and assessment.  In the six (6)-month period, if either you or IKEA wishes to terminate the employment relationship, then either party can effect that termination with one week’s notice in writing.',
	@retSalTcTermsProbTimeHeadline NVARCHAR(MAX) = 'Probationary Period',
	@retSalTcTermsProbTimeSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsProbTimeGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsProbTimeName, 
		@retSalTcTermsProbTimeDescription,
		@retSalTcTermsProbTimeText, 
		@retSalTcTermsProbTimeHeadline,
		@retSalTcTermsProbTimeSortOrder,
		@retSalTcTermsProbTimeGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsProbTimeName, 
		[Description] = @retSalTcTermsProbTimeDescription, 
		[Text] = @retSalTcTermsProbTimeText,
		[Headline] = @retSalTcTermsProbTimeHeadline,
		SortOrder = @retSalTcTermsProbTimeSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsProbTimeGuid
END
DECLARE @retSalTcTermsProbID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsProbTimeGuid)

-- Create condition for probation
DECLARE @retSalTcTermsProbCondGuid UNIQUEIDENTIFIER = '2AC37295-EC02-4108-B999-0B648F8EFF02',
	@retSalTcTermsProbCondPropertyName NVARCHAR(MAX) = 'extendedcase_ProbationPeriod',
	@retSalTcTermsProbCondOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsProbCondValues NVARCHAR(MAX) = '',
	@retSalTcTermsProbCondDescription NVARCHAR(MAX) = 'Has probation period',
	@retSalTcTermsProbCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsProbCondGuid)
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
		@retSalTcTermsProbCondGuid,
		@retSalTcTermsProbID,
		@retSalTcTermsProbCondPropertyName,
		@retSalTcTermsProbCondOperator,
		@retSalTcTermsProbCondValues,
		@retSalTcTermsProbCondDescription,
		@retSalTcTermsProbCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsProbID,
		Property_Name = @retSalTcTermsProbCondPropertyName,
		Operator = @retSalTcTermsProbCondOperator,
		[Values] = @retSalTcTermsProbCondValues,
		[Description] = @retSalTcTermsProbCondDescription,
		[Status] = @retSalTcTermsProbCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsProbCondGuid
END

-- #################################### Performance Management
DECLARE @retSalTcTermsPerfAGuid UNIQUEIDENTIFIER = 'F71E4568-AD9F-40C4-B884-6965BAC37F83',
	@retSalTcTermsPerfAName NVARCHAR(MAX) = @prefix + ' Performance',
	@retSalTcTermsPerfADescription NVARCHAR(MAX) = '',
	@retSalTcTermsPerfAText NVARCHAR(MAX) = 'A Co-worker Performance Review will be conducted at least once a year, usually between September and November.  However, your first review will be conducted during your 6-month probationary period.  This review will be based on your initial Co-worker discussion and your position’s Performance Criteria and Job Profile.  Areas of performance and non-performance will be discussed and addressed in accordance with company guidelines. Whilst the company conducts annual performance reviews, it also maintains an ongoing performance management program with its co-workers.',
	@retSalTcTermsPerfAHeadline NVARCHAR(MAX) = 'Performance Management',
	@retSalTcTermsPerfASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsPerfAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsPerfAName, 
		@retSalTcTermsPerfADescription,
		@retSalTcTermsPerfAText, 
		@retSalTcTermsPerfAHeadline,
		@retSalTcTermsPerfASortOrder,
		@retSalTcTermsPerfAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsPerfAName, 
		[Description] = @retSalTcTermsPerfADescription, 
		[Text] = @retSalTcTermsPerfAText,
		[Headline] = @retSalTcTermsPerfAHeadline,
		SortoRder = @retSalTcTermsPerfASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPerfAGuid
END

DECLARE @retSalTcTermsPerfAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPerfAGuid)

-- Create condition for performance
DECLARE @retSalTcTermsPerfACondGuid UNIQUEIDENTIFIER = 'EF362BD4-C765-4938-8D94-02997F3C03BE',
	@retSalTcTermsPerfACondPropertyName NVARCHAR(MAX) = 'extendedcase_ProbationPeriod',
	@retSalTcTermsPerfACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsPerfACondValues NVARCHAR(MAX) = 'Yes',
	@retSalTcTermsPerfACondDescription NVARCHAR(MAX) = 'Has probation period',
	@retSalTcTermsPerfACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsPerfACondGuid)
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
		@retSalTcTermsPerfACondGuid,
		@retSalTcTermsPerfAID,
		@retSalTcTermsPerfACondPropertyName,
		@retSalTcTermsPerfACondOperator,
		@retSalTcTermsPerfACondValues,
		@retSalTcTermsPerfACondDescription,
		@retSalTcTermsPerfACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsPerfAID,
		Property_Name = @retSalTcTermsPerfACondPropertyName,
		Operator = @retSalTcTermsPerfACondOperator,
		[Values] = @retSalTcTermsPerfACondValues,
		[Description] = @retSalTcTermsPerfACondDescription,
		[Status] = @retSalTcTermsPerfACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsPerfACondGuid
END

-- #################################### Remuneration Review
DECLARE @retSalTcTermsRenRevGuid UNIQUEIDENTIFIER = '1B196357-4FB6-4B56-9B72-68064F6D289B',
	@retSalTcTermsRenRevName NVARCHAR(MAX) = @prefix + ' Remuneration review',
	@retSalTcTermsRenRevDescription NVARCHAR(MAX) = '',
	@retSalTcTermsRenRevText NVARCHAR(MAX) = 'In line with IKEA’s Remuneration Policy, your Total Remuneration package will be reviewed annually following your performance review.<br>
	<br>
The earliest your Total Remuneration package will be reviewed will be in January <Next Salary Review Year>.',
	@retSalTcTermsRenRevHeadline NVARCHAR(MAX) = 'Remuneration review',
	@retSalTcTermsRenRevSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsRenRevGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsRenRevName, 
		@retSalTcTermsRenRevDescription,
		@retSalTcTermsRenRevText, 
		@retSalTcTermsRenRevHeadline,
		@retSalTcTermsRenRevSortOrder,
		@retSalTcTermsRenRevGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsRenRevName, 
		[Description] = @retSalTcTermsRenRevDescription, 
		[Text] = @retSalTcTermsRenRevText,
		[Headline] = @retSalTcTermsRenRevHeadline,
		SortOrder = @retSalTcTermsRenRevSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsRenRevGuid
END



-- #################################### Confidential Information
DECLARE @retSalTcTermsConfGuid UNIQUEIDENTIFIER = 'D5D2902D-98B1-4891-9401-B3EB78DCCE1E',
	@retSalTcTermsConfName NVARCHAR(MAX) = @prefix + ' Confidential Information',
	@retSalTcTermsConfDescription NVARCHAR(MAX) = '',
	@retSalTcTermsConfText NVARCHAR(MAX) = 'In the course of your employment, you may be exposed to “Confidential Information” concerning IKEA. Confidential Information means any information obtained by you in the course of your employment, including:
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
	@retSalTcTermsConfHeadline NVARCHAR(MAX) = 'Confidential Information',
	@retSalTcTermsConfSortOrder INT = @termsCounter 
 SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsConfGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsConfName, 
		@retSalTcTermsConfDescription,
		@retSalTcTermsConfText, 
		@retSalTcTermsConfHeadline,
		@retSalTcTermsConfSortOrder,
		@retSalTcTermsConfGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsConfName, 
		[Description] = @retSalTcTermsConfDescription, 
		[Text] = @retSalTcTermsConfText,
		[Headline] = @retSalTcTermsConfHeadline,
		SortOrder = @retSalTcTermsConfSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsConfGuid
END

-- #################################### Leave Entitlements 

---- Leave Entitlements  A
DECLARE @retSalTcTermsLeaveAGuid UNIQUEIDENTIFIER = '4A91E33B-4E5E-457F-97DC-67E70D94305A',
	@retSalTcTermsLeaveAName NVARCHAR(MAX) = @prefix + ' Leave, full time',
	@retSalTcTermsLeaveADescription NVARCHAR(MAX) = '',
	@retSalTcTermsLeaveAText NVARCHAR(MAX) = 'All Full Time Co-workers shall be entitled to up to 4 weeks’ paid Annual Leave per anniversary year in accordance with the provisions of the applicable legislation, accruing and credited on a fortnightly basis.<br>
	<br>
Annual Leave is to be taken within a period not exceeding 12 months from the date it becomes due. As a result of the peak business demands placed upon IKEA during certain periods of the year, no Annual Leave is available to be taken by co-workers during the weeks posted by IKEA at the beginning of each calendar year (known as “block out” periods), unless IKEA approves such an Annual Leave request in writing. Such Annual Leave “block out” period may be different from department to department and subject to change from year to year.<br>
<br>
All Full Time Co-workers shall accrue, on a fortnightly basis, an amount of paid Personal Leave which can be taken as either Sick Leave or as Carer’s Leave up to a maximum of 10 days per anniversary year.  <br>
<br>
Any unused Personal Leave will accumulate from year to year. Co-workers are not entitled to be paid for their accumulated Personal Leave on termination. <br>
<br>
IKEA is committed to ensuring that Sick Leave is only utilised in cases of genuine need. The misuse of Sick Leave will lead to performance management of the co-worker.<br>
<br>
If a co-worker is required to access their Sick Leave, all reasonable efforts must be made by the co-worker to contact their immediate manager or the person in charge of the location at the time, at least 1 hour prior to their commencement time. At this point of contact, the co-worker will also be required to advise management of the nature of their absence and the estimated duration of the absence.<br>
<br>
In order to qualify for paid Sick Leave, IKEA requires the production of a medical certificate from a medical practitioner dated at the time of absence or a statutory declaration signed by a Justice of the Peace, specifying the nature of the absence for:<br>
<br>
a. any absences in excess of one day;<br>
<br>
b. absences on a single day if they occur on either side of a Non-Working Day or Public Holiday;<br>
c. where IKEA management believes that the co-worker has had an excessive amount of single day absences or a pattern of absence has been identified; or<br>
<br>
d. where IKEA management has previously performance managed a co-worker in relation to an excessive amount of sick leave and has requested in writing that all future claims for sick leave be supported by a medical certificate.',
	@retSalTcTermsLeaveAHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@retSalTcTermsLeaveASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsLeaveAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsLeaveAName, 
		@retSalTcTermsLeaveADescription,
		@retSalTcTermsLeaveAText, 
		@retSalTcTermsLeaveAHeadline,
		@retSalTcTermsLeaveASortOrder,
		@retSalTcTermsLeaveAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsLeaveAName, 
		[Description] = @retSalTcTermsLeaveADescription, 
		[Text] = @retSalTcTermsLeaveAText,
		[Headline] = @retSalTcTermsLeaveAHeadline,
		SortOrder = @retSalTcTermsLeaveASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsLeaveAGuid
END

DECLARE @retSalTcTermsLeaveAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsLeaveAGuid)


-- Create condition for Leave Entitlements A
DECLARE @retSalTcTermsLeaveACondGuid UNIQUEIDENTIFIER = '4236FE3E-B3BE-4532-B578-46BD51F5C716',
	@retSalTcTermsLeaveACondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalTcTermsLeaveACondOperator NVARCHAR(MAX) = 'Equal',
	@retSalTcTermsLeaveACondValues NVARCHAR(MAX) = '76',
	@retSalTcTermsLeaveACondDescription NVARCHAR(MAX) = 'Is full time',
	@retSalTcTermsLeaveACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsLeaveACondGuid)
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
		@retSalTcTermsLeaveACondGuid,
		@retSalTcTermsLeaveAID,
		@retSalTcTermsLeaveACondPropertyName,
		@retSalTcTermsLeaveACondOperator,
		@retSalTcTermsLeaveACondValues,
		@retSalTcTermsLeaveACondDescription,
		@retSalTcTermsLeaveACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsLeaveAID,
		Property_Name = @retSalTcTermsLeaveACondPropertyName,
		Operator = @retSalTcTermsLeaveACondOperator,
		[Values] = @retSalTcTermsLeaveACondValues,
		[Description] = @retSalTcTermsLeaveACondDescription,
		[Status] = @retSalTcTermsLeaveACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsLeaveACondGuid
END

---- Leave Entitlements  B
DECLARE @retSalTcTermsLeaveBGuid UNIQUEIDENTIFIER = '8E0507DC-CCA3-49C9-A6C2-CE95D2793B5D',
	@retSalTcTermsLeaveBName NVARCHAR(MAX) = @prefix + ' Leave, part time',
	@retSalTcTermsLeaveBDescription NVARCHAR(MAX) = '',
	@retSalTcTermsLeaveBText NVARCHAR(MAX) = 'All Part Time Co-workers will be entitled to a pro rata amount of Annual Leave based on your paid hours worked in accordance with the provisions of the applicable legislation, accruing and credited on a fortnightly basis.<br>
<br>
Annual Leave is to be taken within a period not exceeding 12 months from the date it becomes due. As a result of the peak business demands placed upon IKEA during certain periods of the year, no Annual Leave is available to be taken by co-workers during the weeks posted by IKEA at the beginning of each calendar year (known as “block out” periods), unless IKEA approves such an Annual Leave request in writing. Such Annual Leave “block out” period may be different from department to department and subject to change from year to year.<br>
<br>
All Part Time Co-workers shall accrue, on a fortnightly basis, an amount of paid Personal Leave which can be taken as either Sick Leave or as Carer’s Leave.  Part-time Co-workers receive a pro-rata entitlement to Personal Leave based on your paid hours Worked. <br>
<br>
Any unused Personal Leave will accumulate from year to year. Co-workers are not entitled to be paid for their accumulated Personal Leave on termination. <br>
<br>
IKEA is committed to ensuring that Sick Leave is only utilised in cases of genuine need. The misuse of Sick Leave will lead to performance management of the co-worker.<br>
<br>
If a co-worker is required to access their Sick Leave, all reasonable efforts must be made by the co-worker to contact their immediate manager or the person in charge of the location at the time, at least 1 hour prior to their commencement time. At this point of contact, the co-worker will also be required to advise management of the nature of their absence and the estimated duration of the absence.<br>
<br>
In order to qualify for paid Sick Leave, IKEA requires the production of a medical certificate from a medical practitioner dated at the time of absence or a statutory declaration signed by a Justice of the Peace, specifying the nature of the absence for:<br>
<br>
a. any absences in excess of one day;<br>
<br>
b. absences on a single day if they occur on either side of a Non-Working Day or Public Holiday;<br>
<br>
c. where IKEA management believes that the co-worker has had an excessive amount of single day absences or a pattern of absence has been identified; or<br>
<br>
d. where IKEA management has previously performance managed a co-worker in relation to an excessive amount of sick leave and has requested in writing that all future claims for sick leave be supported by a medical certificate.',
	@retSalTcTermsLeaveBHeadline NVARCHAR(MAX) = 'Leave Entitlements',
	@retSalTcTermsLeaveBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsLeaveBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsLeaveBName, 
		@retSalTcTermsLeaveBDescription,
		@retSalTcTermsLeaveBText, 
		@retSalTcTermsLeaveBHeadline,
		@retSalTcTermsLeaveBSortOrder,
		@retSalTcTermsLeaveBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsLeaveBName, 
		[Description] = @retSalTcTermsLeaveBDescription, 
		[Text] = @retSalTcTermsLeaveBText,
		[Headline] = @retSalTcTermsLeaveBHeadline,
		SortOrder = @retSalTcTermsLeaveBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsLeaveBGuid
END
DECLARE @retSalTcTermsLeaveBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsLeaveBGuid)

-- Create condition for leave entitlements B
DECLARE @retSalTcTermsLeaveBCondGuid UNIQUEIDENTIFIER = '1366C3F0-233D-48D2-8542-D23ECE551EC2',
	@retSalTcTermsLeaveBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retSalTcTermsLeaveBCondOperator NVARCHAR(MAX) = 'LessThan',
	@retSalTcTermsLeaveBCondValues NVARCHAR(MAX) = '76',
	@retSalTcTermsLeaveBCondDescription NVARCHAR(MAX) = 'Is part time',
	@retSalTcTermsLeaveBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsLeaveBCondGuid)
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
		@retSalTcTermsLeaveBCondGuid,
		@retSalTcTermsLeaveBID,
		@retSalTcTermsLeaveBCondPropertyName,
		@retSalTcTermsLeaveBCondOperator,
		@retSalTcTermsLeaveBCondValues,
		@retSalTcTermsLeaveBCondDescription,
		@retSalTcTermsLeaveBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsLeaveBID,
		Property_Name = @retSalTcTermsLeaveBCondPropertyName,
		Operator = @retSalTcTermsLeaveBCondOperator,
		[Values] = @retSalTcTermsLeaveBCondValues,
		[Description] = @retSalTcTermsLeaveBCondDescription,
		[Status] = @retSalTcTermsLeaveBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsLeaveBCondGuid
END

-- #################################### Issues Resolution
DECLARE @retSalTcTermsIssuesGuid UNIQUEIDENTIFIER = '0E499425-1BCE-4B07-A656-D88D52349D0B',
	@retSalTcTermsIssuesName NVARCHAR(MAX) = @prefix + ' Issues Resolution',
	@retSalTcTermsIssuesDescription NVARCHAR(MAX) = '',
	@retSalTcTermsIssuesText NVARCHAR(MAX) = 'If any issues arise during your employment with IKEA, the matter should initially be discussed with your immediate manager, in accordance with IKEA’s Issue Resolution Procedure.  If the problem remains unresolved, you may refer it to more senior levels of management for further discussion in accordance with the Issue Resolution Procedure.',
	@retSalTcTermsIssuesHeadline NVARCHAR(MAX) = 'Issues Resolution',
	@retSalTcTermsIssuesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsIssuesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsIssuesName, 
		@retSalTcTermsIssuesDescription,
		@retSalTcTermsIssuesText, 
		@retSalTcTermsIssuesHeadline,
		@retSalTcTermsIssuesSortOrder,
		@retSalTcTermsIssuesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsIssuesName, 
		[Description] = @retSalTcTermsIssuesDescription, 
		[Text] = @retSalTcTermsIssuesText,
		[Headline] = @retSalTcTermsIssuesHeadline,
		SortOrder = @retSalTcTermsIssuesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsIssuesGuid
END

-- #################################### Equal Employment Opportunity 
DECLARE @retSalTcTermsEqualGuid UNIQUEIDENTIFIER = '8C2C7434-17BD-4E0A-BF21-459212D978E3',
	@retSalTcTermsEqualName NVARCHAR(MAX) = @prefix + ' Equal Employment',
	@retSalTcTermsEqualDescription NVARCHAR(MAX) = '',
	@retSalTcTermsEqualText NVARCHAR(MAX) = 'IKEA''s policy is to provide all co-workers with equal opportunity.  This policy precludes discrimination and harassment based on, but not limited to, race, colour, religion, gender, age, marital status and disability.  You are required to familiarise yourself with this policy and comply with it at all times.',
	@retSalTcTermsEqualHeadline NVARCHAR(MAX) = 'Equal Employment Opportunity ',
	@retSalTcTermsEqualSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsEqualGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsEqualName, 
		@retSalTcTermsEqualDescription,
		@retSalTcTermsEqualText, 
		@retSalTcTermsEqualHeadline,
		@retSalTcTermsEqualSortOrder,
		@retSalTcTermsEqualGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsEqualName, 
		[Description] = @retSalTcTermsEqualDescription, 
		[Text] = @retSalTcTermsEqualText,
		[Headline] = @retSalTcTermsEqualHeadline,
		SortOrder = @retSalTcTermsEqualSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsEqualGuid
END

-- #################################### Appearance & Conduct
DECLARE @retSalTcTermsAppearGuid UNIQUEIDENTIFIER = '03609F36-C253-44F4-9D83-2A49E19132AA',
	@retSalTcTermsAppearName NVARCHAR(MAX) = @prefix + ' Appearance & Conduct',
	@retSalTcTermsAppearDescription NVARCHAR(MAX) = '',
	@retSalTcTermsAppearText NVARCHAR(MAX) = 'IKEA has established guidelines necessary for the professional appearance that the company expects all co-workers to present, and as such co-workers are to wear smart casual attire within these guidelines.<br>
	<br>
Co-workers are expected to project a favorable and professional image for IKEA, and are to be courteous, efficient and reliable in their dealings with colleagues, existing and potential customers and suppliers to IKEA.',
	@retSalTcTermsAppearHeadline NVARCHAR(MAX) = 'Appearance & Conduct',
	@retSalTcTermsAppearSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsAppearGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsAppearName, 
		@retSalTcTermsAppearDescription,
		@retSalTcTermsAppearText, 
		@retSalTcTermsAppearHeadline,
		@retSalTcTermsAppearSortOrder,
		@retSalTcTermsAppearGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsAppearName, 
		[Description] = @retSalTcTermsAppearDescription, 
		[Text] = @retSalTcTermsAppearText,
		[Headline] = @retSalTcTermsAppearHeadline,
		SortOrder = @retSalTcTermsAppearSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsAppearGuid
END

-- #################################### Induction & Ongoing Learning & Development
DECLARE @retSalTcTermsInductionGuid UNIQUEIDENTIFIER = 'DDE77511-772A-42E4-90AA-68A1D6803C72',
	@retSalTcTermsInductionName NVARCHAR(MAX) = @prefix + ' Induction',
	@retSalTcTermsInductionDescription NVARCHAR(MAX) = '',
	@retSalTcTermsInductionText NVARCHAR(MAX) = 'IKEA is committed to your induction and ongoing development and as such, has a requirement that you undertake and are committed to training as offered by the company.  Whilst the majority of training is conducted on the job, you may be required from time to time to attend external training programs at different locations as organised by the company.<br>
	<br>
IKEA encourages its co-workers to take responsibility for their own learning and development..<br>
<br><br>
IKEA also encourages its co-workers to take responsibility for their own learning and development.',
	@retSalTcTermsInductionHeadline NVARCHAR(MAX) = 'Induction & Ongoing Learning & Development',
	@retSalTcTermsInductionSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsInductionGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsInductionName, 
		@retSalTcTermsInductionDescription,
		@retSalTcTermsInductionText, 
		@retSalTcTermsInductionHeadline,
		@retSalTcTermsInductionSortOrder,
		@retSalTcTermsInductionGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsInductionName, 
		[Description] = @retSalTcTermsInductionDescription, 
		[Text] = @retSalTcTermsInductionText,
		[Headline] = @retSalTcTermsInductionHeadline,
		SortOrder = @retSalTcTermsInductionSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsInductionGuid
END

-- #################################### Occupational Health & Safety
DECLARE @retSalTcTermsSafetyGuid UNIQUEIDENTIFIER = '89F0B307-A266-4EE5-B947-EEAA665E6216',
	@retSalTcTermsSafetyName NVARCHAR(MAX) = @prefix + ' Safety',
	@retSalTcTermsSafetyDescription NVARCHAR(MAX) = '',
	@retSalTcTermsSafetyText NVARCHAR(MAX) = 'IKEA understands the requirement of ensuring a safe and healthy working environment for all co-workers in its offices, warehouses and stores, and a safe and healthy shopping environment for customers.  In fulfilling this aim, we undertake regular consultation with co-workers on health and safety issues and concerns.',
	@retSalTcTermsSafetyHeadline NVARCHAR(MAX) = 'Occupational Health & Safety',
	@retSalTcTermsSafetySortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsSafetyGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsSafetyName, 
		@retSalTcTermsSafetyDescription,
		@retSalTcTermsSafetyText, 
		@retSalTcTermsSafetyHeadline,
		@retSalTcTermsSafetySortOrder,
		@retSalTcTermsSafetyGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsSafetyName, 
		[Description] = @retSalTcTermsSafetyDescription, 
		[Text] = @retSalTcTermsSafetyText,
		[Headline] = @retSalTcTermsSafetyHeadline,
		SortOrder = @retSalTcTermsSafetySortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsSafetyGuid
END
-- #################################### Termination
-- A. Termination has no end date
DECLARE @retSalTcTermsTerminationAGuid UNIQUEIDENTIFIER = '9D46DC4C-C0FA-4418-A44F-ACE2F96BA621',
	@retSalTcTermsTerminationAName NVARCHAR(MAX) = @prefix + ' Termination, No change valid to date',
	@retSalTcTermsTerminationADescription NVARCHAR(MAX) = '',
	@retSalTcTermsTerminationAText NVARCHAR(MAX) = 'IKEA may terminate your employment by giving four (4) weeks’ notice, or payment in lieu at your ordinary rate of pay.  If you are over 45 years of age and have at least two years’ continuous employment with IKEA, you will be entitled to an additional week’s notice.<br>
<br>
If you wish to resign, you must provide IKEA with four (4) weeks’ notice.  If you fail to give the appropriate notice to IKEA, IKEA shall have the right to withhold monies due to you up to a maximum of your ordinary rate of pay for the period of notice not served.<br>
<br>
IKEA may at its election not require you to attend the workplace during the notice period.<br>
<br>
Notices of resignation or termination must be supplied in writing, and must comply with the above named notice periods unless a new period is agreed to in writing between you and IKEA.<br>
<br>
A failure on your part to resign in writing will not affect the validity of your resignation.<br>
<br>
IKEA retains the right to terminate your employment without notice in the case of summary dismissal.<br>
<br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee. IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.<br>
<br>
Termination payments will be made by way of Electronic Funds Transfer within 4 days of the end of the termination pay period.',
	@retSalTcTermsTerminationAHeadline NVARCHAR(MAX) = 'Termination',
	@retSalTcTermsTerminationASortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsTerminationAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsTerminationAName, 
		@retSalTcTermsTerminationADescription,
		@retSalTcTermsTerminationAText, 
		@retSalTcTermsTerminationAHeadline,
		@retSalTcTermsTerminationASortOrder,
		@retSalTcTermsTerminationAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsTerminationAName, 
		[Description] = @retSalTcTermsTerminationADescription, 
		[Text] = @retSalTcTermsTerminationAText,
		[Headline] = @retSalTcTermsTerminationAHeadline,
		SortOrder = @retSalTcTermsTerminationASortOrder 
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsTerminationAGuid
END

DECLARE @retSalTcTermsTerminationAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsTerminationAGuid)
 
-- Create condition for for termination
DECLARE @retSalTcTermsTerminationACondGuid UNIQUEIDENTIFIER = 'F3447D0D-2C2C-4E24-B46A-C583D205F2F8',
	@retSalTcTermsTerminationACondPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsTerminationACondOperator NVARCHAR(MAX) = 'IsEmpty',
	@retSalTcTermsTerminationACondValues NVARCHAR(MAX) = '',
	@retSalTcTermsTerminationACondDescription NVARCHAR(MAX) = 'No change valid to date',
	@retSalTcTermsTerminationACondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsTerminationACondGuid)
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
		@retSalTcTermsTerminationACondGuid,
		@retSalTcTermsTerminationAID,
		@retSalTcTermsTerminationACondPropertyName,
		@retSalTcTermsTerminationACondOperator,
		@retSalTcTermsTerminationACondValues,
		@retSalTcTermsTerminationACondDescription,
		@retSalTcTermsTerminationACondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsTerminationAID,
		Property_Name = @retSalTcTermsTerminationACondPropertyName,
		Operator = @retSalTcTermsTerminationACondOperator,
		[Values] = @retSalTcTermsTerminationACondValues,
		[Description] = @retSalTcTermsTerminationACondDescription,
		[Status] = @retSalTcTermsTerminationACondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsTerminationACondGuid
END

-- B. Termination has end date
DECLARE @retSalTcTermsTerminationBGuid UNIQUEIDENTIFIER = 'D035680B-ADDF-4AF0-9999-3171B92D0852',
	@retSalTcTermsTerminationBName NVARCHAR(MAX) = @prefix + ' Termination, Has contract end date',
	@retSalTcTermsTerminationBDescription NVARCHAR(MAX) = '',
	@retSalTcTermsTerminationBText NVARCHAR(MAX) = 'Your employment will terminate on the date specified in clause 2 above.<br>
<br>
Despite clause 2, IKEA may terminate your employment by giving 4 weeks’ notice, or payment in lieu at your ordinary rate of pay.  If you are over 45 years of age and have at least two years’ continuous employment with IKEA, you will be entitled to an additional week’s notice.<br>
<br>
If you wish to resign, you must provide IKEA with four (4) weeks’ notice.  If you fail to give the appropriate notice to IKEA, IKEA shall have the right to withhold monies due to you up to a maximum of your ordinary rate of pay for the period of notice not served.<br>
<br>
Notices of resignation or termination must be supplied in writing, and must comply with the abovenamed notice periods unless a new period is agreed to in writing between you and IKEA.  A failure on your part to resign in writing will not affect the validity of your resignation.  <br>
<br>
IKEA retains the right to terminate your employment without notice in the case of summary dismissal.<br>
<br>
Upon termination of your employment, all material, equipment, uniforms, information, company records, data etc issued to you or created by you in your employment is to be returned to IKEA or its nominee. IKEA reserves the right to withhold an appropriate sum of money from a co-worker’s termination payment until such time as any outstanding company property as detailed above is returned.<br>
<br>
Termination payments will be made by way of Electronic Funds Transfer within 4 days of the end of the termination pay period.',
	@retSalTcTermsTerminationBHeadline NVARCHAR(MAX) = 'Termination',
	@retSalTcTermsTerminationBSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsTerminationBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsTerminationBName, 
		@retSalTcTermsTerminationBDescription,
		@retSalTcTermsTerminationBText, 
		@retSalTcTermsTerminationBHeadline,
		@retSalTcTermsTerminationBSortOrder,
		@retSalTcTermsTerminationBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsTerminationBName, 
		[Description] = @retSalTcTermsTerminationBDescription, 
		[Text] = @retSalTcTermsTerminationBText,
		[Headline] = @retSalTcTermsTerminationBHeadline,
		SortOrder = @retSalTcTermsTerminationBSortOrder 
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsTerminationBGuid
END

DECLARE @retSalTcTermsTerminationBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsTerminationBGuid)
 
-- Create condition for for termination
DECLARE @retSalTcTermsTerminationBCondGuid UNIQUEIDENTIFIER = 'E28048E7-4B84-4008-836C-F1E6E1FD2136',
	@retSalTcTermsTerminationBCondPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retSalTcTermsTerminationBCondOperator NVARCHAR(MAX) = 'HasValue',
	@retSalTcTermsTerminationBCondValues NVARCHAR(MAX) = '',
	@retSalTcTermsTerminationBCondDescription NVARCHAR(MAX) = 'Has change valid to date',
	@retSalTcTermsTerminationBCondStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retSalTcTermsTerminationBCondGuid)
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
		@retSalTcTermsTerminationBCondGuid,
		@retSalTcTermsTerminationBID,
		@retSalTcTermsTerminationBCondPropertyName,
		@retSalTcTermsTerminationBCondOperator,
		@retSalTcTermsTerminationBCondValues,
		@retSalTcTermsTerminationBCondDescription,
		@retSalTcTermsTerminationBCondStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retSalTcTermsTerminationBID,
		Property_Name = @retSalTcTermsTerminationBCondPropertyName,
		Operator = @retSalTcTermsTerminationBCondOperator,
		[Values] = @retSalTcTermsTerminationBCondValues,
		[Description] = @retSalTcTermsTerminationBCondDescription,
		[Status] = @retSalTcTermsTerminationBCondStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retSalTcTermsTerminationBCondGuid
END

-- #################################### Company Policies & Procedures 
DECLARE @retSalTcTermsPoliciesGuid UNIQUEIDENTIFIER = 'F1AFE56F-D2C1-4705-B38C-BECDB46717B0',
	@retSalTcTermsPoliciesName NVARCHAR(MAX) = @prefix + ' Policies',
	@retSalTcTermsPoliciesDescription NVARCHAR(MAX) = '',
	@retSalTcTermsPoliciesText NVARCHAR(MAX) = 'You shall be required to comply with all Company Policies and Procedures as advised to you and as outlined in IKEA’s Policy Guidelines and IKEA Store Introduction Program. These Policies and Procedures may be subject to change/amendment from time to time, and form part of your contract of employment.',
	@retSalTcTermsPoliciesHeadline NVARCHAR(MAX) = 'Company Policies & Procedures',
	@retSalTcTermsPoliciesSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsPoliciesGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsPoliciesName, 
		@retSalTcTermsPoliciesDescription,
		@retSalTcTermsPoliciesText, 
		@retSalTcTermsPoliciesHeadline,
		@retSalTcTermsPoliciesSortOrder,
		@retSalTcTermsPoliciesGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsPoliciesName, 
		[Description] = @retSalTcTermsPoliciesDescription, 
		[Text] = @retSalTcTermsPoliciesText,
		[Headline] = @retSalTcTermsPoliciesHeadline,
		SortOrder = @retSalTcTermsPoliciesSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPoliciesGuid
END

-- #################################### Other Terms and Conditions
DECLARE @retSalTcTermsOtherTermsGuid UNIQUEIDENTIFIER = 'A27DC217-758B-4B2F-B628-C7A6FCFB9DE6',
	@retSalTcTermsOtherTermsName NVARCHAR(MAX) = @prefix + ' Other T&C',
	@retSalTcTermsOtherTermsDescription NVARCHAR(MAX) = '',
	@retSalTcTermsOtherTermsText NVARCHAR(MAX) = 'The terms and conditions contained within the IKEA Everyday Work Ways Handbook, ico-worker.com/au, IKEA Intranet website and the IKEA Group Code of Conduct may also apply to your employment. These documents may be amended from time to time.',
	@retSalTcTermsOtherTermsHeadline NVARCHAR(MAX) = 'Other Terms and Conditions',
	@retSalTcTermsOtherTermsSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsOtherTermsGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsOtherTermsName, 
		@retSalTcTermsOtherTermsDescription,
		@retSalTcTermsOtherTermsText, 
		@retSalTcTermsOtherTermsHeadline,
		@retSalTcTermsOtherTermsSortOrder,
		@retSalTcTermsOtherTermsGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsOtherTermsName, 
		[Description] = @retSalTcTermsOtherTermsDescription, 
		[Text] = @retSalTcTermsOtherTermsText,
		[Headline] = @retSalTcTermsOtherTermsHeadline,
		SortOrder = @retSalTcTermsOtherTermsSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsOtherTermsGuid
END

-- #################################### Police Checks
DECLARE @retSalTcTermsPoliceGuid UNIQUEIDENTIFIER = 'FF449CCC-36BE-4B8A-97F1-A8D9E7B495BF',
	@retSalTcTermsPoliceName NVARCHAR(MAX) = @prefix + ' Police',
	@retSalTcTermsPoliceDescription NVARCHAR(MAX) = '',
	@retSalTcTermsPoliceText NVARCHAR(MAX) = 'Some positions at IKEA require evidence of good character (for example - positions that deal with children or cash).  Obtaining details of your criminal history via a police check/s is an integral part of the assessment of your suitability for such positions.<br>
<br>
You may be required to provide a police check/s at the time you are given this offer of employment.  Alternatively, you may be required to provide a police check/s during your employment (for instance, when it is suspected that you have incurred a criminal record since the commencement of your employment, or where you begin working in a position requiring evidence of good character).  By signing this offer of employment, you consent to complete, sign and lodge the relevant police check application documentation (which will be provided to you by IKEA), and to direct that the corresponding police check record/s be forwarded directly to IKEA (where permitted) or (otherwise) to provide IKEA with the original police check record/s immediately on receipt.<br>
<br>
If you are required to provide the police check/s at the time you are given this offer of employment, you acknowledge that the offer of employment will be subject to the check being satisfactory to IKEA.<br>
<br>
If you are required to provide the police check/s at any other time during your employment and the check is not satisfactory to IKEA, it may give grounds for summary dismissal.<br>
<br>
Please note that the existence of a criminal history does not mean that the check will automatically be unsatisfactory, or that you will be assessed automatically as being unsuitable.  Each case will be assessed on its merits and will depend upon the inherent requirements of the position.',
	@retSalTcTermsPoliceHeadline NVARCHAR(MAX) = 'Police Checks',
	@retSalTcTermsPoliceSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsPoliceGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsPoliceName, 
		@retSalTcTermsPoliceDescription,
		@retSalTcTermsPoliceText, 
		@retSalTcTermsPoliceHeadline,
		@retSalTcTermsPoliceSortOrder,
		@retSalTcTermsPoliceGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsPoliceName, 
		[Description] = @retSalTcTermsPoliceDescription, 
		[Text] = @retSalTcTermsPoliceText,
		[Headline] = @retSalTcTermsPoliceHeadline,
		SortOrder = @retSalTcTermsPoliceSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsPoliceGuid
END


-- #################################### Communications with Media
DECLARE @retSalTcTermsMediaGuid UNIQUEIDENTIFIER = '1F791297-DDEA-443B-BCA4-73FADC2079EF',
	@retSalTcTermsMediaName NVARCHAR(MAX) = @prefix + ' Media',
	@retSalTcTermsMediaDescription NVARCHAR(MAX) = '',
	@retSalTcTermsMediaText NVARCHAR(MAX) = 'You shall not provide information or speak on behalf of IKEA or otherwise to the media on any matters concerning IKEA’s business or activities.  You must refer all requests from the media for information and/or interviews to the Retail Manager.',
	@retSalTcTermsMediaHeadline NVARCHAR(MAX) = 'Communications with Media',
	@retSalTcTermsMediaSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsMediaGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsMediaName, 
		@retSalTcTermsMediaDescription,
		@retSalTcTermsMediaText, 
		@retSalTcTermsMediaHeadline,
		@retSalTcTermsMediaSortOrder,
		@retSalTcTermsMediaGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsMediaName, 
		[Description] = @retSalTcTermsMediaDescription, 
		[Text] = @retSalTcTermsMediaText,
		[Headline] = @retSalTcTermsMediaHeadline,
		SortOrder = @retSalTcTermsMediaSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsMediaGuid
END

-- #################################### Obligation to report unlawful activities
DECLARE @retSalTcTermsUnlawGuid UNIQUEIDENTIFIER = '396D9C6E-9736-4488-8565-6DED348C6350',
	@retSalTcTermsUnlawName NVARCHAR(MAX) = @prefix + ' Unlawful',
	@retSalTcTermsUnlawDescription NVARCHAR(MAX) = '',
	@retSalTcTermsUnlawText NVARCHAR(MAX) = 'If you become aware of or suspect any unlawful act or omission by any IKEA employee, you must advise IKEA immediately.',
	@retSalTcTermsUnlawHeadline NVARCHAR(MAX) = 'Obligation to report unlawful activities',
	@retSalTcTermsUnlawSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsUnlawGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsUnlawName, 
		@retSalTcTermsUnlawDescription,
		@retSalTcTermsUnlawText, 
		@retSalTcTermsUnlawHeadline,
		@retSalTcTermsUnlawSortOrder,
		@retSalTcTermsUnlawGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsUnlawName, 
		[Description] = @retSalTcTermsUnlawDescription, 
		[Text] = @retSalTcTermsUnlawText,
		[Headline] = @retSalTcTermsUnlawHeadline,
		SortOrder = @retSalTcTermsUnlawSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsUnlawGuid
END
-- #################################### Variation
DECLARE @retSalTcTermsVarGuid UNIQUEIDENTIFIER = 'F6DAD77B-F1AA-4DB3-B7BE-3DF045BA39EA',
	@retSalTcTermsVarName NVARCHAR(MAX) = @prefix + ' Variation',
	@retSalTcTermsVarDescription NVARCHAR(MAX) = '',
	@retSalTcTermsVarText NVARCHAR(MAX) = 'This Agreement may only be varied by a written agreement signed by IKEA and you.',
	@retSalTcTermsVarHeadline NVARCHAR(MAX) = 'Variation',
	@retSalTcTermsVarSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsVarGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsVarName, 
		@retSalTcTermsVarDescription,
		@retSalTcTermsVarText, 
		@retSalTcTermsVarHeadline,
		@retSalTcTermsVarSortOrder,
		@retSalTcTermsVarGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsVarName, 
		[Description] = @retSalTcTermsVarDescription, 
		[Text] = @retSalTcTermsVarText,
		[Headline] = @retSalTcTermsVarHeadline,
		SortOrder = @retSalTcTermsVarSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsVarGuid
END


-- #################################### Intellectual Property
DECLARE @retSalTcTermsIntelPropGuid UNIQUEIDENTIFIER = '1A6945FA-F51F-4623-822C-3E61DF5491F7',
	@retSalTcTermsIntelPropName NVARCHAR(MAX) = @prefix + ' Int. Property',
	@retSalTcTermsIntelPropDescription NVARCHAR(MAX) = '',
	@retSalTcTermsIntelPropText NVARCHAR(MAX) = 'IKEA owns all copyright in any works and all inventions, discoveries, novel designs, improvements or modifications, computer program material and trademarks which you write or develop in the course of your employment (in or out of working hours) (“Intellectual Property”).<br>
<br>
You assign to IKEA any interest you have in the Intellectual Property, and you must disclose any Intellectual Property to IKEA.<br>
<br>
During and after your employment, you must do anything IKEA reasonably requires (at IKEA''s cost) to:
<ul>
<li>obtain statutory protection (including by patent, design registration, trade mark registration or copyright) for the Intellectual Property for IKEA in any country; or</li>
<li>Perfect or evidence IKEA’s ownership of the Intellectual Property.</li>
</ul>',

	@retSalTcTermsIntelPropHeadline NVARCHAR(MAX) = 'Intellectual Property',
	@retSalTcTermsIntelPropSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsIntelPropGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsIntelPropName, 
		@retSalTcTermsIntelPropDescription,
		@retSalTcTermsIntelPropText, 
		@retSalTcTermsIntelPropHeadline,
		@retSalTcTermsIntelPropSortOrder,
		@retSalTcTermsIntelPropGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsIntelPropName, 
		[Description] = @retSalTcTermsIntelPropDescription, 
		[Text] = @retSalTcTermsIntelPropText,
		[Headline] = @retSalTcTermsIntelPropHeadline,
		SortOrder = @retSalTcTermsIntelPropSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsIntelPropGuid
END

-- #################################### Suspension
DECLARE @retSalTcTermsSuspGuid UNIQUEIDENTIFIER = 'AF8CCD89-989F-4537-83A1-7D56E205CCEE',
	@retSalTcTermsSuspName NVARCHAR(MAX) = @prefix + ' Suspension',
	@retSalTcTermsSuspDescription NVARCHAR(MAX) = '',
	@retSalTcTermsSuspText NVARCHAR(MAX) = 'If we have reason to believe that you may have engaged in a serious breach of your employment obligations, IKEA may at its discretion suspend you from your duties, either with or without pay, while an investigation is conducted.',
	@retSalTcTermsSuspHeadline NVARCHAR(MAX) = 'Suspension',
	@retSalTcTermsSuspSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcTermsSuspGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcTermsID, 
		@retSalTcTermsSuspName, 
		@retSalTcTermsSuspDescription,
		@retSalTcTermsSuspText, 
		@retSalTcTermsSuspHeadline,
		@retSalTcTermsSuspSortOrder,
		@retSalTcTermsSuspGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcTermsID,
		[Name] = @retSalTcTermsSuspName, 
		[Description] = @retSalTcTermsSuspDescription, 
		[Text] = @retSalTcTermsSuspText,
		[Headline] = @retSalTcTermsSuspHeadline,
		SortOrder = @retSalTcTermsSuspSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcTermsSuspGuid
END

-- Add terms paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @retSalTcTermsID, @counter
SET @counter = @counter + 1


-- #################################### End Text
---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalTcEndTextParagraphGuid UNIQUEIDENTIFIER = '0BFC2FE3-0D4A-4E33-9293-5D93A1BA6316',
	@retSalTcEndTextParagraphName NVARCHAR(MAX) = @prefix + ' End Text',
	@retSalTcEndTextParagraphType INT = @ParagraphTypeText,
	@retSalTcEndTextParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalTcEndTextParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalTcEndTextParagraphName, @retSalTcEndTextParagraphDescription, @retSalTcEndTextParagraphType, @retSalTcEndTextParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalTcEndTextParagraphName, [Description] = @retSalTcEndTextParagraphDescription, ParagraphType = @retSalTcEndTextParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalTcEndTextParagraphGuid
END
DECLARE @retSalTcEndTextParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalTcEndTextParagraphGuid)

-- Create a text field
DECLARE @retSalTcEndTextGuid UNIQUEIDENTIFIER = 'AF161F37-D002-40B0-80BD-E1CBA7C4A5B1',
	@retSalTcEndTextName NVARCHAR(MAX) = @prefix + '  End Text',
	@retSalTcEndTextDescription NVARCHAR(MAX) = '',
	@retSalTcEndTextText NVARCHAR(MAX) = 'As an indication of your understanding and acceptance of these conditions, please sign this letter of offer, and return to the undersigned within seven (7) days.  Please retain the second copy for your records.<br>
<br>
If you have any questions pertaining to this offer of employment or any of the information contained herein, please do not hesitate to contact me before signing this letter.<br>
<br>
We look forward to you joining the IKEA team, and look forward to a mutually rewarding association.',
	@retSalTcEndTextHeadline NVARCHAR(MAX) = '',
	@retSalTcEndTextSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcEndTextGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcEndTextParagraphID, 
		@retSalTcEndTextName, 
		@retSalTcEndTextDescription,
		@retSalTcEndTextText, 
		@retSalTcEndTextHeadline,
		@retSalTcEndTextSortOrder,
		@retSalTcEndTextGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcEndTextParagraphID,
		[Name] = @retSalTcEndTextName, 
		[Description] = @retSalTcEndTextDescription, 
		[Text] = @retSalTcEndTextText,
		[Headline] = @retSalTcEndTextHeadline,
		SortOrder = @retSalTcEndTextSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcEndTextGuid
END

-- Add end text paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @retSalTcEndTextParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Contractor Signature
---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalTcConSignParagraphGuid UNIQUEIDENTIFIER = '0A42E6C4-AE79-4368-8F56-E3C4F185099E',
	@retSalTcConSignParagraphName NVARCHAR(MAX) = @prefix + ' Con. Sign.',
	@retSalTcConSignParagraphType INT = @ParagraphTypeText,
	@retSalTcConSignParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalTcConSignParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalTcConSignParagraphName, @retSalTcConSignParagraphDescription, @retSalTcConSignParagraphType, @retSalTcConSignParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalTcConSignParagraphName, [Description] = @retSalTcConSignParagraphDescription, ParagraphType = @retSalTcConSignParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalTcConSignParagraphGuid
END
DECLARE @retSalTcConSignParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalTcConSignParagraphGuid)

-- Create a text field
DECLARE @retSalTcConSignGuid UNIQUEIDENTIFIER = '2AC56CC7-5967-4FD2-81E5-77A5E1E7DE8B',
	@retSalTcConSignName NVARCHAR(MAX) = @prefix + ' Con. Sign.',
	@retSalTcConSignDescription NVARCHAR(MAX) = '',
	@retSalTcConSignText NVARCHAR(MAX) = 'Yours sincerely<br>
	<Reports To Line Manager><br>
	<Position Title (Local Job Name) of Reports To Line Manager><br>
	<strong>IKEA Pty Limited</strong>',
	@retSalTcConSignHeadline NVARCHAR(MAX) = '',
	@retSalTcConSignSortOrder INT = @termsCounter
SET @termsCounter = @termsCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcConSignGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcConSignParagraphID, 
		@retSalTcConSignName, 
		@retSalTcConSignDescription,
		@retSalTcConSignText, 
		@retSalTcConSignHeadline,
		@retSalTcConSignSortOrder,
		@retSalTcConSignGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcConSignParagraphID,
		[Name] = @retSalTcConSignName, 
		[Description] = @retSalTcConSignDescription, 
		[Text] = @retSalTcConSignText,
		[Headline] = @retSalTcConSignHeadline,
		SortOrder = @retSalTcConSignSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcConSignGuid
END

-- Add contractor signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @retSalTcConSignParagraphID, @counter
SET @counter = @counter + 1

-- #################################### Acceptance
---- Create or update paragraph
-- Paragraph guid
DECLARE @retSalTcAcceptParagraphGuid UNIQUEIDENTIFIER = 'C8C5DD49-597A-499A-BC27-80A106676D3D',
	@retSalTcAcceptParagraphName NVARCHAR(MAX) = @prefix + ' Acceptance',
	@retSalTcAcceptParagraphType INT = @ParagraphTypeText,
	@retSalTcAcceptParagraphDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retSalTcAcceptParagraphGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retSalTcAcceptParagraphName, @retSalTcAcceptParagraphDescription, @retSalTcAcceptParagraphType, @retSalTcAcceptParagraphGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retSalTcAcceptParagraphName, [Description] = @retSalTcAcceptParagraphDescription, ParagraphType = @retSalTcAcceptParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retSalTcAcceptParagraphGuid
END
DECLARE @retSalTcAcceptParagraphID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retSalTcAcceptParagraphGuid)

-- Create a text field
DECLARE @retSalTcAcceptGuid UNIQUEIDENTIFIER = '742FEB07-C415-46EC-B171-7D897F263F40',
	@retSalTcAcceptName NVARCHAR(MAX) = @prefix + ' Acceptance',
	@retSalTcAcceptDescription NVARCHAR(MAX) = '',
	@retSalTcAcceptText NVARCHAR(MAX) = '<table style="border: 1px solid black">
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
	@retSalTcAcceptHeadline NVARCHAR(MAX) = '',
	@retSalTcAcceptSortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retSalTcAcceptGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retSalTcAcceptParagraphID, 
		@retSalTcAcceptName, 
		@retSalTcAcceptDescription,
		@retSalTcAcceptText, 
		@retSalTcAcceptHeadline,
		@retSalTcAcceptSortOrder,
		@retSalTcAcceptGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retSalTcAcceptParagraphID,
		[Name] = @retSalTcAcceptName, 
		[Description] = @retSalTcAcceptDescription, 
		[Text] = @retSalTcAcceptText,
		[Headline] = @retSalTcAcceptHeadline,
		SortOrder = @retSalTcAcceptSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retSalTcAcceptGuid
END

-- Add acceptance paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retSalTcID, @retSalTcAcceptParagraphID, @counter
SET @counter = @counter + 1




-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @retSalTcGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



COMMIT

