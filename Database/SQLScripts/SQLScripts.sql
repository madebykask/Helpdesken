--update DB from 5.3.52 to 5.3.53 version

RAISERROR ('Add Column ExtendedCaseForm_Id to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.ExtendedCaseTranslations','ExtendedCaseForm_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[ExtendedCaseTranslations]
	ADD [ExtendedCaseForm_Id] INT NULL

	ALTER TABLE [dbo].[ExtendedCaseTranslations] WITH NOCHECK ADD CONSTRAINT [FK_ExtendedCaseTranslations_ExtendedCaseForms]
	FOREIGN KEY([ExtendedCaseForm_Id]) REFERENCES [dbo].[ExtendedCaseForms] ([Id])

END
GO

RAISERROR ('Add Control.Filuppladdning Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.Filuppladdning')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.Filuppladdning'
           ,'Filuppladdning')
		   END
GO
RAISERROR ('Add Control.Filuppladdning English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.Filuppladdning')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.Filuppladdning'
           ,'File upload')
		   END
GO


RAISERROR ('Add Message.DraFilerHit Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Message.DraFilerHit')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Message.DraFilerHit'
           ,'Dra filer hit')
		   END
GO
RAISERROR ('Add Message.DraFilerHit English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Message.DraFilerHit')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Message.DraFilerHit'
           ,'Drop files here')
		   END
GO


  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.53'
GO