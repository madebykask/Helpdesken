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

DECLARE @prefix NVARCHAR(MAX) = 'RET EA TC'

-- #################################### Contract Clusters – Retail EA (T&C) ####################################

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
	@retTcEmployGreetingName NVARCHAR(MAX) = @prefix + ' Greeting',
	@retTcEmployGreetingParagraphType INT = @ParagraphTypeText,
	@retTcEmployGreetingDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retTcEmployGreetingGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retTcEmployGreetingName, @retTcEmployGreetingDescription, @retTcEmployGreetingParagraphType, @retTcEmployGreetingGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retTcEmployGreetingName, [Description] = @retTcEmployGreetingDescription, ParagraphType = @retTcEmployGreetingParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retTcEmployGreetingGuid
END
DECLARE @retTcEmployGreetingID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retTcEmployGreetingGuid)

---- Create or update text A, No change valid to end date
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

-- Create condition for Text A,  Has change valid to end date
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

-- Add greeting paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @retTcEmployGreetingID, @counter
SET @counter = @counter + 1


-- ################################# Begin two column table 


---- Create or update paragraph
-- Paragraph guid
DECLARE @retTcTableGuid UNIQUEIDENTIFIER = 'F8CD8086-073A-43B6-BA9A-96437B967A37',
	@retTcTableName NVARCHAR(MAX) = @prefix + ' Table',
	@retTcTableParagraphType INT = @ParagraphTypeTableTwoColumns,
	@retTcTableDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retTcTableGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retTcTableName, @retTcTableDescription, @retTcTableParagraphType, @retTcTableGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retTcTableName, [Description] = @retTcTableDescription, ParagraphType = @retTcTableParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retTcTableGuid
END
DECLARE @retTcTableID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retTcTableGuid)

DECLARE @tableCounter INT = 0

-- #################################### Classification
DECLARE @retTcTableClassGuid UNIQUEIDENTIFIER = 'C93E1BAA-BE5A-4043-8772-E7BC7EDE14F4',
	@retTcTableClassName NVARCHAR(MAX) = @prefix + ' Classification',
	@retTcTableClassDescription NVARCHAR(MAX) = '',
	@retTcTableClassText NVARCHAR(MAX) = '<Position Title (Local Job Name)>',
	@retTcTableClassHeadline NVARCHAR(MAX) = 'Classification:',
	@retTcTableClassSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableClassGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableClassName, 
		@retTcTableClassDescription,
		@retTcTableClassText, 
		@retTcTableClassHeadline,
		@retTcTableClassSortOrder,
		@retTcTableClassGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableClassName, 
		[Description] = @retTcTableClassDescription, 
		[Text] = @retTcTableClassText,
		[Headline] = @retTcTableClassHeadline,
		SortOrder = @retTcTableClassSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableClassGuid
END


-- #################################### Location
DECLARE @retTcTableLocGuid UNIQUEIDENTIFIER = '546E1BAA-BE5A-4043-8772-E7BC7EDE14F4',
	@retTcTableLocName NVARCHAR(MAX) = @prefix + ' Location',
	@retTcTableLocDescription NVARCHAR(MAX) = '',
	@retTcTableLocText NVARCHAR(MAX) = '<Business Unit>',
	@retTcTableLocHeadline NVARCHAR(MAX) = 'Location:',
	@retTcTableLocSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableLocGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableLocName, 
		@retTcTableLocDescription,
		@retTcTableLocText, 
		@retTcTableLocHeadline,
		@retTcTableLocSortOrder,
		@retTcTableLocGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableLocName, 
		[Description] = @retTcTableLocDescription, 
		[Text] = @retTcTableLocText,
		[Headline] = @retTcTableLocHeadline,
		SortOrder = @retTcTableLocSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableLocGuid
END

-- #################################### Dates

-- A. Has no change valid to date (Effective date)
DECLARE @retTcTableDateAGuid UNIQUEIDENTIFIER = '24B38DCC-2CF7-4A0E-B983-1BC9CF4F6626',
	@retTcTableDateAName NVARCHAR(MAX) = @prefix + ' Date, no valid to',
	@retTcTableDateADescription NVARCHAR(MAX) = '',
	@retTcTableDateAText NVARCHAR(MAX) = '<Change Valid From>',
	@retTcTableDateAHeadline NVARCHAR(MAX) = 'Effective date:',
	@retTcTableDateASortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableDateAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableDateAName, 
		@retTcTableDateADescription,
		@retTcTableDateAText, 
		@retTcTableDateAHeadline,
		@retTcTableDateASortOrder,
		@retTcTableDateAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableDateAName, 
		[Description] = @retTcTableDateADescription, 
		[Text] = @retTcTableDateAText,
		[Headline] = @retTcTableDateAHeadline,
		SortOrder = @retTcTableDateASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableDateAGuid
END
DECLARE @retTcTableDateAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableDateAGuid)

-- A. Condition (has no change valid to date)
DECLARE @retTcTableDateACondAGuid UNIQUEIDENTIFIER = '6341C827-5BE0-4572-8D33-76E270062E7A',
	@retTcTableDateACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retTcTableDateACondAOperator NVARCHAR(MAX) = 'IsEmpty',
	@retTcTableDateACondAValues NVARCHAR(MAX) = '',
	@retTcTableDateACondADescription NVARCHAR(MAX) = 'Has no end date',
	@retTcTableDateACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableDateACondAGuid)
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
		@retTcTableDateACondAGuid,
		@retTcTableDateAID,
		@retTcTableDateACondAPropertyName,
		@retTcTableDateACondAOperator,
		@retTcTableDateACondAValues,
		@retTcTableDateACondADescription,
		@retTcTableDateACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableDateAID,
		Property_Name = @retTcTableDateACondAPropertyName,
		Operator = @retTcTableDateACondAOperator,
		[Values] = @retTcTableDateACondAValues,
		[Description] = @retTcTableDateACondADescription,
		[Status] = @retTcTableDateACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableDateACondAGuid
END

-- B. Has change valid to end date (Temporary Contract Period)
DECLARE @retTcTableDateBGuid UNIQUEIDENTIFIER = '39B9B1BD-CB42-4023-B18B-7817DAF60A3C',
	@retTcTableDateBName NVARCHAR(MAX) = @prefix + ' Date, has valid to',
	@retTcTableDateBDescription NVARCHAR(MAX) = '',
	@retTcTableDateBText NVARCHAR(MAX) = 'from <Change Valid From> to <Change Valid To>',
	@retTcTableDateBHeadline NVARCHAR(MAX) = 'Temporary Contract Period:',
	@retTcTableDateBSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableDateBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableDateBName, 
		@retTcTableDateBDescription,
		@retTcTableDateBText, 
		@retTcTableDateBHeadline,
		@retTcTableDateBSortOrder,
		@retTcTableDateBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableDateBName, 
		[Description] = @retTcTableDateBDescription, 
		[Text] = @retTcTableDateBText,
		[Headline] = @retTcTableDateBHeadline,
		SortOrder = @retTcTableDateBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableDateBGuid
END
DECLARE @retTcTableDateBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableDateBGuid)

-- B. Condition (has change valid to date)
DECLARE @retTcTableDateBCondAGuid UNIQUEIDENTIFIER = '3BA51F9D-D854-4CEE-A8D6-C4088A6CD7F3',
	@retTcTableDateBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ChangeValidTo',
	@retTcTableDateBCondAOperator NVARCHAR(MAX) = 'HasValue',
	@retTcTableDateBCondAValues NVARCHAR(MAX) = '',
	@retTcTableDateBCondADescription NVARCHAR(MAX) = 'Has end date',
	@retTcTableDateBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableDateBCondAGuid)
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
		@retTcTableDateBCondAGuid,
		@retTcTableDateBID,
		@retTcTableDateBCondAPropertyName,
		@retTcTableDateBCondAOperator,
		@retTcTableDateBCondAValues,
		@retTcTableDateBCondADescription,
		@retTcTableDateBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableDateBID,
		Property_Name = @retTcTableDateBCondAPropertyName,
		Operator = @retTcTableDateBCondAOperator,
		[Values] = @retTcTableDateBCondAValues,
		[Description] = @retTcTableDateBCondADescription,
		[Status] = @retTcTableDateBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableDateBCondAGuid
END


-- #################################### Employment type

-- A. Employment, full time
DECLARE @retTcTableEmployAGuid UNIQUEIDENTIFIER = '482D32A0-FB45-45F8-B244-6919FF446242',
	@retTcTableEmployAName NVARCHAR(MAX) = @prefix + ' Employment, full time',
	@retTcTableEmployADescription NVARCHAR(MAX) = '',
	@retTcTableEmployAText NVARCHAR(MAX) = 'Full Time',
	@retTcTableEmployAHeadline NVARCHAR(MAX) = 'Employment type:',
	@retTcTableEmployASortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableEmployAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableEmployAName, 
		@retTcTableEmployADescription,
		@retTcTableEmployAText, 
		@retTcTableEmployAHeadline,
		@retTcTableEmployASortOrder,
		@retTcTableEmployAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableEmployAName, 
		[Description] = @retTcTableEmployADescription, 
		[Text] = @retTcTableEmployAText,
		[Headline] = @retTcTableEmployAHeadline,
		SortOrder = @retTcTableEmployASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableEmployAGuid
END
DECLARE @retTcTableEmployAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableEmployAGuid)

-- A. Employment, full time condition
DECLARE @retTcTableEmployACondAGuid UNIQUEIDENTIFIER = 'E5486A38-C412-4435-8236-7FF13ABF860F',
	@retTcTableEmployACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retTcTableEmployACondAOperator NVARCHAR(MAX) = 'Equal',
	@retTcTableEmployACondAValues NVARCHAR(MAX) = '76',
	@retTcTableEmployACondADescription NVARCHAR(MAX) = 'Has full time',
	@retTcTableEmployACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableEmployACondAGuid)
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
		@retTcTableEmployACondAGuid,
		@retTcTableEmployAID,
		@retTcTableEmployACondAPropertyName,
		@retTcTableEmployACondAOperator,
		@retTcTableEmployACondAValues,
		@retTcTableEmployACondADescription,
		@retTcTableEmployACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableEmployAID,
		Property_Name = @retTcTableEmployACondAPropertyName,
		Operator = @retTcTableEmployACondAOperator,
		[Values] = @retTcTableEmployACondAValues,
		[Description] = @retTcTableEmployACondADescription,
		[Status] = @retTcTableEmployACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableEmployACondAGuid
END

-- B. Employment, part time
DECLARE @retTcTableEmployBGuid UNIQUEIDENTIFIER = 'AC848C86-0AC0-4404-99E0-E75F4B87C177',
	@retTcTableEmployBName NVARCHAR(MAX) = @prefix + ' Employment, part time',
	@retTcTableEmployBDescription NVARCHAR(MAX) = '',
	@retTcTableEmployBText NVARCHAR(MAX) = 'Part Time',
	@retTcTableEmployBHeadline NVARCHAR(MAX) = 'Employment type:',
	@retTcTableEmployBSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableEmployBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableEmployBName, 
		@retTcTableEmployBDescription,
		@retTcTableEmployBText, 
		@retTcTableEmployBHeadline,
		@retTcTableEmployBSortOrder,
		@retTcTableEmployBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableEmployBName, 
		[Description] = @retTcTableEmployBDescription, 
		[Text] = @retTcTableEmployBText,
		[Headline] = @retTcTableEmployBHeadline,
		SortOrder = @retTcTableEmployBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableEmployBGuid
END
DECLARE @retTcTableEmployBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableEmployBGuid)

-- B. Employment, part time, condition A, less than 76
DECLARE @retTcTableEmployBCondAGuid UNIQUEIDENTIFIER = '31BE8F45-4D03-4653-B3AA-5269DA0090E3',
	@retTcTableEmployBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retTcTableEmployBCondAOperator NVARCHAR(MAX) = 'LessThan',
	@retTcTableEmployBCondAValues NVARCHAR(MAX) = '76',
	@retTcTableEmployBCondADescription NVARCHAR(MAX) = 'Has less than 76 hours',
	@retTcTableEmployBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableEmployBCondAGuid)
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
		@retTcTableEmployBCondAGuid,
		@retTcTableEmployBID,
		@retTcTableEmployBCondAPropertyName,
		@retTcTableEmployBCondAOperator,
		@retTcTableEmployBCondAValues,
		@retTcTableEmployBCondADescription,
		@retTcTableEmployBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableEmployBID,
		Property_Name = @retTcTableEmployBCondAPropertyName,
		Operator = @retTcTableEmployBCondAOperator,
		[Values] = @retTcTableEmployBCondAValues,
		[Description] = @retTcTableEmployBCondADescription,
		[Status] = @retTcTableEmployBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableEmployBCondAGuid
END

-- B. Employment, part time, condition B, more than 0
DECLARE @retTcTableEmployBCondBGuid UNIQUEIDENTIFIER = 'C223813B-8A32-41C6-AFFA-3E4AF0678785',
	@retTcTableEmployBCondBPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retTcTableEmployBCondBOperator NVARCHAR(MAX) = 'LargerThan',
	@retTcTableEmployBCondBValues NVARCHAR(MAX) = '0',
	@retTcTableEmployBCondBDescription NVARCHAR(MAX) = 'Has more than 0 hours',
	@retTcTableEmployBCondBStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableEmployBCondBGuid)
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
		@retTcTableEmployBCondBGuid,
		@retTcTableEmployBID,
		@retTcTableEmployBCondBPropertyName,
		@retTcTableEmployBCondBOperator,
		@retTcTableEmployBCondBValues,
		@retTcTableEmployBCondBDescription,
		@retTcTableEmployBCondBStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableEmployBID,
		Property_Name = @retTcTableEmployBCondBPropertyName,
		Operator = @retTcTableEmployBCondBOperator,
		[Values] = @retTcTableEmployBCondBValues,
		[Description] = @retTcTableEmployBCondBDescription,
		[Status] = @retTcTableEmployBCondBStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableEmployBCondBGuid
END


-- C. Employment, casual
DECLARE @retTcTableEmployCGuid UNIQUEIDENTIFIER = '01393556-C5F4-430A-900B-A475CE3338F1',
	@retTcTableEmployCName NVARCHAR(MAX) = @prefix + ' Employment, casual',
	@retTcTableEmployCDescription NVARCHAR(MAX) = '',
	@retTcTableEmployCText NVARCHAR(MAX) = 'Casual',
	@retTcTableEmployCHeadline NVARCHAR(MAX) = 'Employment type:',
	@retTcTableEmployCSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableEmployCGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableEmployCName, 
		@retTcTableEmployCDescription,
		@retTcTableEmployCText, 
		@retTcTableEmployCHeadline,
		@retTcTableEmployCSortOrder,
		@retTcTableEmployCGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableEmployCName, 
		[Description] = @retTcTableEmployCDescription, 
		[Text] = @retTcTableEmployCText,
		[Headline] = @retTcTableEmployCHeadline,
		SortOrder = @retTcTableEmployCSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableEmployCGuid
END
DECLARE @retTcTableEmployCID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableEmployCGuid)

-- C. Employment, casual condition
DECLARE @retTcTableEmployCCondAGuid UNIQUEIDENTIFIER = '12D3A112-BE62-44EA-9ED7-BD0106CBA119',
	@retTcTableEmployCCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retTcTableEmployCCondAOperator NVARCHAR(MAX) = 'Equal',
	@retTcTableEmployCCondAValues NVARCHAR(MAX) = '0',
	@retTcTableEmployCCondADescription NVARCHAR(MAX) = 'Has no contracted hours',
	@retTcTableEmployCCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableEmployCCondAGuid)
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
		@retTcTableEmployCCondAGuid,
		@retTcTableEmployCID,
		@retTcTableEmployCCondAPropertyName,
		@retTcTableEmployCCondAOperator,
		@retTcTableEmployCCondAValues,
		@retTcTableEmployCCondADescription,
		@retTcTableEmployCCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableEmployCID,
		Property_Name = @retTcTableEmployCCondAPropertyName,
		Operator = @retTcTableEmployCCondAOperator,
		[Values] = @retTcTableEmployCCondAValues,
		[Description] = @retTcTableEmployCCondADescription,
		[Status] = @retTcTableEmployCCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableEmployCCondAGuid
END

-- #################################### Contracted hours per fortnight

-- A. Has contracted hours
DECLARE @retTcTableHoursAGuid UNIQUEIDENTIFIER = 'FD1CACB1-23F4-4B89-8A82-63C5ACCA4533',
	@retTcTableHoursAName NVARCHAR(MAX) = @prefix + ' Contracted hours, has',
	@retTcTableHoursADescription NVARCHAR(MAX) = '',
	@retTcTableHoursAText NVARCHAR(MAX) = '<Contracted Hours>',
	@retTcTableHoursAHeadline NVARCHAR(MAX) = 'Contracted Hours per Fortnight:',
	@retTcTableHoursASortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableHoursAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableHoursAName, 
		@retTcTableHoursADescription,
		@retTcTableHoursAText, 
		@retTcTableHoursAHeadline,
		@retTcTableHoursASortOrder,
		@retTcTableHoursAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableHoursAName, 
		[Description] = @retTcTableHoursADescription, 
		[Text] = @retTcTableHoursAText,
		[Headline] = @retTcTableHoursAHeadline,
		SortOrder = @retTcTableHoursASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableHoursAGuid
END
DECLARE @retTcTableHoursAID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableHoursAGuid)

-- Has contracted hours
DECLARE @retTcTableHoursACondAGuid UNIQUEIDENTIFIER = '13D89B06-F644-4C2A-B3A8-E50215221C1D',
	@retTcTableHoursACondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retTcTableHoursACondAOperator NVARCHAR(MAX) = 'LargerThan',
	@retTcTableHoursACondAValues NVARCHAR(MAX) = '0',
	@retTcTableHoursACondADescription NVARCHAR(MAX) = 'Has contracted hours',
	@retTcTableHoursACondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableHoursACondAGuid)
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
		@retTcTableHoursACondAGuid,
		@retTcTableHoursAID,
		@retTcTableHoursACondAPropertyName,
		@retTcTableHoursACondAOperator,
		@retTcTableHoursACondAValues,
		@retTcTableHoursACondADescription,
		@retTcTableHoursACondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableHoursAID,
		Property_Name = @retTcTableHoursACondAPropertyName,
		Operator = @retTcTableHoursACondAOperator,
		[Values] = @retTcTableHoursACondAValues,
		[Description] = @retTcTableHoursACondADescription,
		[Status] = @retTcTableHoursACondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableHoursACondAGuid
END

-- B. Has no contracted hours
DECLARE @retTcTableHoursBGuid UNIQUEIDENTIFIER = '2F95E58A-9E38-427F-B6D0-E519708E4D18',
	@retTcTableHoursBName NVARCHAR(MAX) = @prefix + ' Contracted hours, has no',
	@retTcTableHoursBDescription NVARCHAR(MAX) = '',
	@retTcTableHoursBText NVARCHAR(MAX) = '',
	@retTcTableHoursBHeadline NVARCHAR(MAX) = '',
	@retTcTableHoursBSortOrder INT = @tableCounter 
 SET @tableCounter = @tableCounter + 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcTableHoursBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcTableID, 
		@retTcTableHoursBName, 
		@retTcTableHoursBDescription,
		@retTcTableHoursBText, 
		@retTcTableHoursBHeadline,
		@retTcTableHoursBSortOrder,
		@retTcTableHoursBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcTableID,
		[Name] = @retTcTableHoursBName, 
		[Description] = @retTcTableHoursBDescription, 
		[Text] = @retTcTableHoursBText,
		[Headline] = @retTcTableHoursBHeadline,
		SortOrder = @retTcTableHoursBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcTableHoursBGuid
END
DECLARE @retTcTableHoursBID INT = (SELECT ID FROM tblCaseDocumentText CDT WHERE CDT.CaseDocumentTextGUID = @retTcTableHoursBGuid)

-- Has contracted hours
DECLARE @retTcTableHoursBCondAGuid UNIQUEIDENTIFIER = 'B5CDDE85-C71D-4619-9DA4-EBB76F4A4382',
	@retTcTableHoursBCondAPropertyName NVARCHAR(MAX) = 'extendedcase_ContractedHours',
	@retTcTableHoursBCondAOperator NVARCHAR(MAX) = 'Equal',
	@retTcTableHoursBCondAValues NVARCHAR(MAX) = '0',
	@retTcTableHoursBCondADescription NVARCHAR(MAX) = 'Has no contracted hours',
	@retTcTableHoursBCondAStatus INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentTextCondition CDC WHERE CDC.CaseDocumentTextConditionGUID = @retTcTableHoursBCondAGuid)
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
		@retTcTableHoursBCondAGuid,
		@retTcTableHoursBID,
		@retTcTableHoursBCondAPropertyName,
		@retTcTableHoursBCondAOperator,
		@retTcTableHoursBCondAValues,
		@retTcTableHoursBCondADescription,
		@retTcTableHoursBCondAStatus,
		@now, 
		@userID,
		@now,
		@userID
	)
END
ELSE
BEGIN
	UPDATE CDTC SET CaseDocumentText_Id = @retTcTableHoursBID,
		Property_Name = @retTcTableHoursBCondAPropertyName,
		Operator = @retTcTableHoursBCondAOperator,
		[Values] = @retTcTableHoursBCondAValues,
		[Description] = @retTcTableHoursBCondADescription,
		[Status] = @retTcTableHoursBCondAStatus,
		CreatedDate = @now,
		CreatedByUser_Id = @userID,
		ChangedDate = @now,
		ChangedByUser_Id = @userID
	FROM tblCaseDocumentTextCondition CDTC
	WHERE CDTC.CaseDocumentTextConditionGUID = @retTcTableHoursBCondAGuid
END





-- Add table paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @retTcTableID, @counter
SET @counter = @counter + 1

-- #################################### Signature

---- Create or update paragraph
-- Paragraph guid
DECLARE @retTcSignatureGuid UNIQUEIDENTIFIER = 'C73950D0-CDCF-49D8-A6F3-BFE32D62FED5',
	@retTcSignatureName NVARCHAR(MAX) = @prefix + ' Signature',
	@retTcSignatureParagraphType INT = @ParagraphTypeText,
	@retTcSignatureDescription NVARCHAR(MAX) = ''

IF NOT EXISTS (SELECT * FROM tblCaseDocumentParagraph CDP WHERE  CDP.CaseDocumentParagraphGUID = @retTcSignatureGuid)
BEGIN
	INSERT INTO tblCaseDocumentParagraph([Name], [Description], ParagraphType, CaseDocumentParagraphGUID)
	VALUES (@retTcSignatureName, @retTcSignatureDescription, @retTcSignatureParagraphType, @retTcSignatureGuid)
END
ELSE
BEGIN
	UPDATE CDP SET [Name] = @retTcSignatureName, [Description] = @retTcSignatureDescription, ParagraphType = @retTcSignatureParagraphType
	FROM tblCaseDocumentParagraph CDP 
	WHERE CDP.CaseDocumentParagraphGUID = @retTcSignatureGuid
END
DECLARE @retTcSignatureID INT = (SELECT ID FROM tblCaseDocumentParagraph WHERE CaseDocumentParagraphGUID = @retTcSignatureGuid)

---- Create or update text A 
DECLARE @retTcSignatureTextAGuid UNIQUEIDENTIFIER = '982E9BAC-0134-4749-9D56-770F2458CC67',
	@retTcSignatureTextAName NVARCHAR(MAX) = @prefix + ' Signature A',
	@retTcSignatureTextADescription NVARCHAR(MAX) = '',
	@retTcSignatureTextAText NVARCHAR(MAX) = 'Your terms and conditions of employment will be as per the IKEA Enterprise Agreement 2017, the IKEA Group Code of Conduct and IKEA policies and procedures, as amended from time to time.  You can access this information via ‘ico-worker.com’ <i>(the IKEA co-worker website)</i> or ‘IKEA Inside’ <i>(the IKEA intranet)</i>.',
	@retTcSignatureTextAHeadline NVARCHAR(MAX) = '',
	@retTcSignatureTextASortOrder INT = 0

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcSignatureTextAGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcSignatureID, 
		@retTcSignatureTextAName, 
		@retTcSignatureTextADescription,
		@retTcSignatureTextAText, 
		@retTcSignatureTextAHeadline,
		@retTcSignatureTextASortOrder,
		@retTcSignatureTextAGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcSignatureID,
		[Name] = @retTcSignatureTextAName, 
		[Description] = @retTcSignatureTextADescription, 
		[Text] = @retTcSignatureTextAText,
		[Headline] = @retTcSignatureTextAHeadline,
		SortOrder = @retTcSignatureTextASortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcSignatureTextAGuid
END

---- Create or update text B
DECLARE @retTcSignatureTextBGuid UNIQUEIDENTIFIER = '09F228BB-8FEE-43CA-B4E9-12F928613307',
	@retTcSignatureTextBName NVARCHAR(MAX) = @prefix + ' Signature B',
	@retTcSignatureTextBDescription NVARCHAR(MAX) = '',
	@retTcSignatureTextBText NVARCHAR(MAX) = '<p><Reports To Line Manager><br />		<Position Title (Local Job Name) of Reports To Line Manager><br />		IKEA <Business Unit></p>		<hr style="height:2px;border:none;color:#000;background-color:#000;" />		',
	@retTcSignatureTextBHeadline NVARCHAR(MAX) = '',
	@retTcSignatureTextBSortOrder INT = 1

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcSignatureTextBGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcSignatureID, 
		@retTcSignatureTextBName, 
		@retTcSignatureTextBDescription,
		@retTcSignatureTextBText, 
		@retTcSignatureTextBHeadline,
		@retTcSignatureTextBSortOrder,
		@retTcSignatureTextBGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcSignatureID,
		[Name] = @retTcSignatureTextBName, 
		[Description] = @retTcSignatureTextBDescription, 
		[Text] = @retTcSignatureTextBText,
		[Headline] = @retTcSignatureTextBHeadline,
		SortOrder = @retTcSignatureTextBSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcSignatureTextBGuid
END


---- Create or update text C
DECLARE @retTcSignatureTextCGuid UNIQUEIDENTIFIER = '29D41A59-DB4D-4B79-B553-8374EF9ED78C',
	@retTcSignatureTextCName NVARCHAR(MAX) = @prefix + ' Signature C',
	@retTcSignatureTextCDescription NVARCHAR(MAX) = '',
	@retTcSignatureTextCText NVARCHAR(MAX) = '<p style="text-align:center;"><strong>Acknowledgement</strong></p>		<p>I accept the terms and conditions of employment as detailed above.</p>		<p>Signed:  _____________________________        Date:  _______________</p>		<p>Co-worker Name: <Co-worker First Name> <Co-worker Last Name></p>		',
	@retTcSignatureTextCHeadline NVARCHAR(MAX) = '',
	@retTcSignatureTextCSortOrder INT = 2

IF NOT EXISTS (SELECT * FROM tblCaseDocumentText CDT WHERE  CDT.CaseDocumentTextGUID = @retTcSignatureTextCGuid)
BEGIN
	INSERT INTO tblCaseDocumentText(CaseDocumentParagraph_Id, [Name], [Description], [Text],[Headline], SortOrder, CaseDocumentTextGUID)
	VALUES (@retTcSignatureID, 
		@retTcSignatureTextCName, 
		@retTcSignatureTextCDescription,
		@retTcSignatureTextCText, 
		@retTcSignatureTextCHeadline,
		@retTcSignatureTextCSortOrder,
		@retTcSignatureTextCGuid)
END
ELSE
BEGIN
	UPDATE CDT SET 
		CaseDocumentParagraph_Id = @retTcSignatureID,
		[Name] = @retTcSignatureTextCName, 
		[Description] = @retTcSignatureTextCDescription, 
		[Text] = @retTcSignatureTextCText,
		[Headline] = @retTcSignatureTextCHeadline,
		SortOrder = @retTcSignatureTextCSortOrder
	FROM tblCaseDocumentText CDT 
	WHERE CDT.CaseDocumentTextGUID = @retTcSignatureTextCGuid
END


-- Add signature paragraph to case document
INSERT INTO tblCaseDocument_CaseDocumentParagraph(CaseDocument_Id, CaseDocumentParagraph_Id, SortOrder)
SELECT @retTcID, @retTcSignatureID, @counter
SET @counter = @counter + 1



-- Preview result
SELECT CDT.ID, CDCDP.Id, CDCDP.SortOrder, CDP.ID ParagraphID, CDP.Name ParagraphName, CDP.Description ParagraphDescription, CDT.Name ConditionalTextName, CDT.Headline, CDT.Text Content, CDTC.Operator, CDTC.Property_Name, CDTC.[Values], CDTC.Status, CDTC.Description, CDT.SortOrder, CDTC.Status  FROM tblCaseDocument CD
LEFT JOIN tblCaseDocument_CaseDocumentParagraph CDCDP ON CDCDP.CaseDocument_Id = CD.Id
LEFT JOIN tblCaseDocumentParagraph CDP ON CDCDP.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentText CDT ON CDT.CaseDocumentParagraph_Id = CDP.Id
LEFT JOIN tblCaseDocumentTextCondition CDTC ON CDTC.CaseDocumentText_Id = CDT.Id
WHERE CD.CaseDocumentGUID = @retTcGuid
ORDER BY CDCDP.SortOrder, CDT.SortOrder



COMMIT