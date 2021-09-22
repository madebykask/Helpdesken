--update DB from 5.3.52 to 5.3.53 version

RAISERROR ('Add Column Customer_Id to ExtendedCaseForms', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.ExtendedCaseForms','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[ExtendedCaseTranslations]
	ADD [ExtendedCaseForm_Id] INT NULL

	ALTER TABLE [dbo].[ExtendedCaseTranslations] WITH NOCHECK ADD CONSTRAINT [FK_ExtendedCaseTranslations_ExtendedCaseForms]
	FOREIGN KEY([ExtendedCaseForm_Id]) REFERENCES [dbo].[ExtendedCaseForms] ([Id])

END
GO

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.53'
GO