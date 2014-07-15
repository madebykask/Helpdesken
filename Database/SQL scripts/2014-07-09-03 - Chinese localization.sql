
IF OBJECT_ID('dhSetTranslation', 'P') IS NOT NULL
BEGIN 
	DROP PROC [dbo].[dhSetTranslation]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[dhSetTranslation]
	@Text NVARCHAR(2000),
	@Translation NVARCHAR(1000),
	@LanguageId INT,
	@IsCaseTranslation BIT = 0,
	@CustomerId INT = NULL
AS

	IF @IsCaseTranslation = 0
	BEGIN 
		DECLARE @TextId INT
		SET @TextId = (SELECT [Id] FROM [dbo].[tblText] WHERE [TextString] = @Text)
	
		IF @TextId IS NOT NULL
		BEGIN
			DECLARE @TranslationId INT
			SET @TranslationId = (SELECT [Id] FROM [dbo].[tblTextTranslation] WHERE [Text_Id] = @TextId AND [Language_Id] = @LanguageId)
			IF @TranslationId IS NULL
			BEGIN
				INSERT INTO [dbo].[tblTextTranslation] ([Text_Id], [Language_Id], [TextTranslation]) VALUES (@TextId, @LanguageId, @Translation)
				SET @TranslationId = SCOPE_IDENTITY(); 
			END
			ELSE
				UPDATE [dbo].[tblTextTranslation] SET [TextTranslation] = @Translation 
				WHERE [Text_Id] = @TextId AND [Language_Id] = @LanguageId

			SELECT * FROM [dbo].[tblTextTranslation] WHERE [Id] = @TranslationId
		END

		RETURN
	END

	IF @CustomerId IS NULL
	BEGIN
		RETURN
	END

	DECLARE @CaseSettingstId INT
	SET @CaseSettingstId = (SELECT [Id] FROM [dbo].[tblCaseFieldSettings] 
						WHERE [CaseField] = @Text AND [Customer_Id] = @CustomerId)

	IF @CaseSettingstId IS NOT NULL
	BEGIN
		DECLARE @CaseTranslationId INT
		SET @CaseTranslationId = 
			(SELECT [CaseFieldSettings_Id] FROM [dbo].[tblCaseFieldSettings_tblLang] 
			WHERE [CaseFieldSettings_Id] = @CaseSettingstId AND [Language_Id] = @LanguageId)
		IF @CaseTranslationId IS NULL
		BEGIN
			INSERT INTO [dbo].[tblCaseFieldSettings_tblLang] ([CaseFieldSettings_Id], [Language_Id], [Label])
			VALUES (@CaseSettingstId, @LanguageId, @Translation)
		END
		ELSE
			UPDATE [dbo].[tblCaseFieldSettings_tblLang] SET [Label] = @Translation
			WHERE [CaseFieldSettings_Id] = @CaseSettingstId AND [Language_Id] = @LanguageId
		
		SELECT * FROM [dbo].[tblCaseFieldSettings_tblLang] WHERE [CaseFieldSettings_Id] = @CaseSettingstId AND [Language_Id] = @LanguageId
	END
GO


DECLARE @LanguageId NVARCHAR(10)
DECLARE @LanguageName NVARCHAR(50)
DECLARE @IsActive INT
DECLARE @CustomerId INT

SET @LanguageId = 'CN'
SET @LanguageName = 'Chinese'
SET @IsActive = 1
SET @CustomerId = 9

DECLARE @Id INT
SET @Id = (SELECT [Id] FROM [dbo].[tblLanguage] WHERE [LanguageID] = @LanguageID)
IF @Id IS NULL
BEGIN
	INSERT INTO [dbo].[tblLanguage] ([Id], [LanguageID], [LanguageName], [Active]) 
	VALUES ((SELECT MAX([Id]) + 1 FROM [dbo].[tblLanguage]), @LanguageId, @LanguageName, @IsActive)
	SET @Id = SCOPE_IDENTITY(); 
END

EXEC [dbo].[dhSetTranslation] @Text = N'ReportedBy', @Translation = N'用户名', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_Name', @Translation = N'姓名', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_EMail', @Translation = N'邮箱', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_Phone', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Customer_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Region_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Department_Id', @Translation = N'宜家部门', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Place', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CaseType_Id', @Translation = N'案件类型', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Caption', @Translation = N'品名', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Description', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Miscellaneous', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ContactBeforeAction', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Available', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Filename', @Translation = N'附件', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'InventoryNumber', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'User_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ProductArea_Id', @Translation = N'产品区域', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Supplier_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Performer_User_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Priority_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'StateSecondary_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'PlanDate', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'WatchDate', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_CellPhone', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ComputerType_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'InvoiceNumber', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'InventoryLocation', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CaseResponsibleUser_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'WorkingGroup_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'OU_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Cost', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'tblLog.Text_Internal', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'tblLog.Text_External', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'tblLog.Charge', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'UserCode', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CaseNumber', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'RegTime', @Translation = N'注册日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ChangeTime', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Category_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'SMS', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Status_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'FinishingDate', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'System_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Urgency_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Impact_Id', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ReferenceNumber', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'AgreedDate', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Verified', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'VerifiedDescription', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'SolutionRate', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'FinishingDescription', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CausingPart', @Translation = N'', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId

EXEC [dbo].[dhSetTranslation] @Text = N'Spara', @Translation = N'保存', @LanguageId = @Id
