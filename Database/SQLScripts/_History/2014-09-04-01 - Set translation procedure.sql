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
	
		IF @TextId IS NULL
		BEGIN
			INSERT INTO [dbo].[tblText] ([TextString]) VALUES (@Text)
			SET @TextId = SCOPE_IDENTITY(); 
		END
		
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