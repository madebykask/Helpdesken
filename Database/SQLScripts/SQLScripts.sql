--update DB from 5.3.50 to 5.3.51 version
RAISERROR ('Add Column AvailableTabsSelfsevice to tblCaseSolution', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseSolution','AvailableTabsSelfsevice') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [AvailableTabsSelfsevice] nvarchar(100) Not Null default('both')
End
Go

RAISERROR ('Add Column ActiveTabSelfservice to tblCaseSolution', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseSolution','ActiveTabSelfservice') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [ActiveTabSelfservice] nvarchar(100) Not Null default('case-tab')
End
Go

RAISERROR ('Adding Unique Constraint to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF OBJECT_ID('dbo.[UQ_LanguageId_Property]') IS NULL 
Begin
ALTER TABLE ExtendedCaseTranslations   
ADD CONSTRAINT UQ_LanguageId_Property UNIQUE (LanguageId, Property); 
end

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.51'
GO
