DECLARE @EnglishId INT
SET @EnglishId = (SELECT [Id] FROM [dbo].[tblLanguage] WHERE [LanguageID] = 'EN')

EXEC [dbo].[dhSetTranslation] @Text = N'Anmälare', @Translation = N'Initiator', @LanguageId = @EnglishId


