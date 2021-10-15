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

RAISERROR ('Add Message.DragFilerHit Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Message.DragFilerHit')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Message.DragFilerHit'
           ,'Drag filer hit')
		   END
GO
RAISERROR ('Add Message.DragFilerHit English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Message.DragFilerHit')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Message.DragFilerHit'
           ,'Drop files here')
		   END
GO


  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.53'
GO