CREATE TABLE [dbo].[ExtendedCaseTranslations]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LanguageId] INT NULL, 
    [Property] NVARCHAR(200) NOT NULL, 
    [Text] NVARCHAR(MAX) NOT NULL
)
