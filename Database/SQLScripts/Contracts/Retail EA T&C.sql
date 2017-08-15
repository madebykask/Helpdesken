--########################################
--########## RET EA T&C ##################
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

DECLARE @prefix NVARCHAR(MAX) = 'DC EA HIR'

-- #################################### Contract Clusters – DC EA (Hiring) ####################################

-- Get the form
DECLARE @retTcGuid UNIQUEIDENTIFIER = '0A574914-AA5C-4883-9AFA-A2B9C9115A10'
DECLARE @retTcID INT, @counter INT = 0
SELECT @retTcID = ID FROM tblCaseDocument CD WHERE CD.CaseDocumentGUID = @retTcGuid

-- Clear the forms paragraph references
DELETE CDCDP FROM tblCaseDocument_CaseDocumentParagraph CDCDP
JOIN tblCaseDocument CD ON CDCDP.CaseDocument_Id = CD.ID
WHERE CD.CaseDocumentGUID = @retTcGuid

-- #################################### Logo
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @logoID, @counter
SET @counter = @counter + 1

-- #################################### Draft
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @draftID, @counter
SET @counter = @counter + 1

-- #################################### Footer
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @footerID, @counter
SET @counter = @counter + 1

-- #################################### Address and company info
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @addressInfoID, @counter
SET @counter = @counter + 1


-- #################################### Employment greeting

---- Create or update paragraph
-- Paragraph guid
DECLARE @retTcEmployGreetingGuid UNIQUEIDENTIFIER = '00a2a2ac-622a-443c-937c-d3ac7d18ff8a',
	@cdpName NVARCHAR(MAX) = @prefix + ' Greeting',
	@cdpParagraphType INT = @ParagraphTypeText,
	@cdpDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retTcEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@cdpName, @cdpDescription, @cdpParagraphType, @retTcEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @cdpName, [Description] = @cdpDescription, ParagraphType = @cdpParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retTcEmployGreetingGuid
END
DECLARE @retTcEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retTcEmployGreetingGuid)

---- Create or update text A, Full Time
DECLARE @retTcEmployGreetingTextAGuid UNIQUEIDENTIFIER = 'd2e6a1c4-995a-47e1-b57b-4665f4a91c5d',
	@retTcEmployGreetingTextAName NVARCHAR(MAX) = @prefix + ' Greeting, no end date',
	@retTcEmployGreetingTextADescription NVARCHAR(MAX) = '',
	@retTcEmployGreetingTextAText NVARCHAR(MAX) = 'IKEA Pty Ltd (IKEA) is pleased to present you with a contract of employment under the terms and conditions of the IKEA Enterprise Agreement 2017.',
	@retTcEmployGreetingTextAHeadline NVARCHAR(MAX) = '',
	@retTcEmployGreetingTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcEmployGreetingTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcEmployGreetingID, 
		@retTcEmployGreetingTextAName, 
		@retTcEmployGreetingTextADescription,
		@retTcEmployGreetingTextAText, 
		@retTcEmployGreetingTextAHeadline,
		@retTcEmployGreetingTextASortOrder,
		@retTcEmployGreetingTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcEmployGreetingID,
		[Name] = @retTcEmployGreetingTextAName, 
		[Description] = @retTcEmployGreetingTextADescription, 
		[Text] = @retTcEmployGreetingTextAText,
		[Headline] = @retTcEmployGreetingTextAHeadline,
		SortOrder = @retTcEmployGreetingTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcEmployGreetingTextAGuid
END
DECLARE @retTcEmployGreetingTextAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcEmployGreetingTextAGuid)

-- Create condition for Text A, Full time
DECLARE @retTcEmployGreetingTextACondAGuid UNIQUEIDENTIFIER = '1cce66c3-03eb-47b4-8928-e53489ec6c3e',
	@retTcEmployGreetingTextACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retTcEmployGreetingTextACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retTcEmployGreetingTextACondAValues NVARCHAR(MAX) = '',
	@retTcEmployGreetingTextACondADescription NVARCHAR(MAX) = 'CHange valid to has no end date',
	@retTcEmployGreetingTextACondAStatus INT = 1
IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcEmployGreetingTextACondAGuid)
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
		@retTcEmployGreetingTextACondAGuid,
		@retTcEmployGreetingTextAID,
		@retTcEmployGreetingTextACondAPropertyName,
		@retTcEmployGreetingTextACondAOperator,
		@retTcEmployGreetingTextACondAValues,
		@retTcEmployGreetingTextACondADescription,
		@retTcEmployGreetingTextACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcEmployGreetingTextAID,
		Property_Name = @retTcEmployGreetingTextACondAPropertyName,
		Operator = @retTcEmployGreetingTextACondAOperator,
		[Values] = @retTcEmployGreetingTextACondAValues,
		[Description] = @retTcEmployGreetingTextACondADescription,
		[Status] = @retTcEmployGreetingTextACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcEmployGreetingTextACondAGuid
END

---- Create or update text B, Part Time
DECLARE @retTcEmployGreetingTextBGuid UNIQUEIDENTIFIER = '0df6a940-6107-48aa-8ac0-369b40620bdf',
	@retTcEmployGreetingTextBName NVARCHAR(MAX) = @prefix + ' Greeting, has end date',
	@retTcEmployGreetingTextBDescription NVARCHAR(MAX) = '',
	@retTcEmployGreetingTextBText NVARCHAR(MAX) = 'IKEA Pty Ltd (IKEA) is pleased to present you with a temporary contract of employment under the terms and conditions of the IKEA Enterprise Agreement 2017.',
	@retTcEmployGreetingTextBHeadline NVARCHAR(MAX) = '',
	@retTcEmployGreetingTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcEmployGreetingTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcEmployGreetingID, 
		@retTcEmployGreetingTextBName, 
		@retTcEmployGreetingTextBDescription,
		@retTcEmployGreetingTextBText, 
		@retTcEmployGreetingTextBHeadline,
		@retTcEmployGreetingTextBSortOrder,
		@retTcEmployGreetingTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcEmployGreetingID,
		[Name] = @retTcEmployGreetingTextBName, 
		[Description] = @retTcEmployGreetingTextBDescription, 
		[Text] = @retTcEmployGreetingTextBText,
		[Headline] = @retTcEmployGreetingTextBHeadline,
		SortOrder = @retTcEmployGreetingTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcEmployGreetingTextBGuid
END
DECLARE @retTcEmployGreetingTextBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcEmployGreetingTextBGuid)

-- Create condition for Text B, Part time
DECLARE @retTcEmployGreetingTextBCondAGuid UNIQUEIDENTIFIER = '518c7cbc-1a47-47cd-bf27-816f8b31c45e',
	@retTcEmployGreetingTextBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retTcEmployGreetingTextBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retTcEmployGreetingTextBCondAValues NVARCHAR(MAX) = '',
	@retTcEmployGreetingTextBCondADescription NVARCHAR(MAX) = 'Has change valid to end date',
	@retTcEmployGreetingTextBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcEmployGreetingTextBCondAGuid)
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
		@retTcEmployGreetingTextBCondAGuid,
		@retTcEmployGreetingTextBID,
		@retTcEmployGreetingTextBCondAPropertyName,
		@retTcEmployGreetingTextBCondAOperator,
		@retTcEmployGreetingTextBCondAValues,
		@retTcEmployGreetingTextBCondADescription,
		@retTcEmployGreetingTextBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcEmployGreetingTextBID,
		Property_Name = @retTcEmployGreetingTextBCondAPropertyName,
		Operator = @retTcEmployGreetingTextBCondAOperator,
		[Values] = @retTcEmployGreetingTextBCondAValues,
		[Description] = @retTcEmployGreetingTextBCondADescription,
		[Status] = @retTcEmployGreetingTextBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcEmployGreetingTextBCondAGuid
END

