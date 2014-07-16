
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

EXEC [dbo].[dhSetTranslation] @Text = N'ReportedBy', @Translation = N'用户身份', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_Name', @Translation = N'通知人', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_EMail', @Translation = N'电子邮件', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_Phone', @Translation = N'电话', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Customer_Id', @Translation = N'顾客', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Region_Id', @Translation = N'区域', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Department_Id', @Translation = N'部门', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Place', @Translation = N'地方', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CaseType_Id', @Translation = N'案例类型', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Caption', @Translation = N'标题', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Description', @Translation = N'描述', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Miscellaneous', @Translation = N'其他', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ContactBeforeAction', @Translation = N'电话联系', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Available', @Translation = N'可用', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Filename', @Translation = N'附加档案', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'InventoryNumber', @Translation = N'PC编号', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'User_Id', @Translation = N'注册人', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ProductArea_Id', @Translation = N'系统', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Supplier_Id', @Translation = N'提供者', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Performer_User_Id', @Translation = N'管理员', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Priority_Id', @Translation = N'优先', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'StateSecondary_Id', @Translation = N'子状态', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'PlanDate', @Translation = N'计划日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'WatchDate', @Translation = N'监护日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Persons_CellPhone', @Translation = N'手机', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ComputerType_Id', @Translation = N'计算机类型', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'InvoiceNumber', @Translation = N'发票号码', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'InventoryLocation', @Translation = N'位置', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CaseResponsibleUser_Id', @Translation = N'负责', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'WorkingGroup_Id', @Translation = N'工作组', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'OU_Id', @Translation = N'单位', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Cost', @Translation = N'成本', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'tblLog.Text_Internal', @Translation = N'内部日志标注', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'tblLog.Text_External', @Translation = N'外部日志标注', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'tblLog.Charge', @Translation = N'借方', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'UserCode', @Translation = N'指令人代码', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CaseNumber', @Translation = N'案', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'RegTime', @Translation = N'注册日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ChangeTime', @Translation = N'更改日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Category_Id', @Translation = N'类别', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'SMS', @Translation = N'短信', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Status_Id', @Translation = N'状态', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'FinishingDate', @Translation = N'完成日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'System_Id', @Translation = N'系统', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Urgency_Id', @Translation = N'紧急', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Impact_Id', @Translation = N'影响', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'ReferenceNumber', @Translation = N'参考号', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'AgreedDate', @Translation = N'约定日期', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'Verified', @Translation = N'验证', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'VerifiedDescription', @Translation = N'验证描述', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'FinishingDescription', @Translation = N'完成说明', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId
EXEC [dbo].[dhSetTranslation] @Text = N'CausingPart', @Translation = N'原因部分', @LanguageId = @Id, @IsCaseTranslation = 1, @CustomerId = @CustomerId

EXEC [dbo].[dhSetTranslation] @Text = N'Ärende', @Translation = N'案', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Ärendehantering', @Translation = N'案件管理', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Ärendeinformation', @Translation = N'案件信息', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Ärendelogg', @Translation = N'案件日志', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Åtgärder', @Translation = N'操作', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Avbryt', @Translation = N'取消', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Borttagning', @Translation = N'删除', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Historik', @Translation = N'历史', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Hög prioritet', @Translation = N'高优先级', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Informera anmälaren om åtgärden', @Translation = N'通知用户', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Kommunikation', @Translation = N'沟通', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Lägg till', @Translation = N'添加', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Skicka ej mail till anmälaren', @Translation = N'不发送邮件给用户', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Skicka intern loggpost till', @Translation = N'发送内部日志给', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Spara', @Translation = N'保存', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Spara och stäng', @Translation = N'保存并关闭', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Standardtext', @Translation = N'标准文本', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Standardtexter', @Translation = N'标准文本', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Ta bort', @Translation = N'删除', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Datum', @Translation = N'日期', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Registrerad av', @Translation = N'注册人', @LanguageId = @Id
EXEC [dbo].[dhSetTranslation] @Text = N'Filer', @Translation = N'文档', @LanguageId = @Id
