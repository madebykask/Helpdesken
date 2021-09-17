--update DB from 5.3.52 to 5.3.53 version

RAISERROR ('Add Section.sektion Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Section.sektion')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Section.sektion'
           ,'Sektion')
		   END
GO

RAISERROR ('Add Section.sektion English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Section.sektion')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Section.sektion'
           ,'Section')
		   END
GO


RAISERROR ('Add Section.sektion English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Section.sektion')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Section.sektion'
           ,'Section')
		   END
GO

RAISERROR ('Add Control.textfalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.textfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.textfalt'
           ,'Textfält')
		   END
GO

RAISERROR ('Add Control.textfalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.textfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.textfalt'
           ,'Text Field')
		   END
GO

RAISERROR ('Add Control.textarea Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.textarea')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.textarea'
           ,'Textarea')
		   END
GO

RAISERROR ('Add Control.textarea English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.textarea')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.textarea'
           ,'Text Area')
		   END
GO

RAISERROR ('Add Control.datumfalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.datumfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.datumfalt'
           ,'Datumfält')
		   END
GO

RAISERROR ('Add Control.datumfalt English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.datumfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.datumfalt'
           ,'Date Field')
		   END
GO

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.53'
GO


