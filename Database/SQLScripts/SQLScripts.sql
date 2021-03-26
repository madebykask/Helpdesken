--update DB from 5.3.50 to 5.3.51 version
RAISERROR ('Add Column AvailableTabsSelfsevice to tblCaseSolution', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseSolution','AvailableTabsSelfsevice') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [AvailableTabsSelfsevice] nvarchar(100) Not Null default('')
End
Go

RAISERROR ('Add Column ActiveTabSelfservice to tblCaseSolution', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseSolution','ActiveTabSelfservice') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [ActiveTabSelfservice] nvarchar(100) Not Null default('')
End
Go

RAISERROR ('Adding Unique Constraint to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF OBJECT_ID('dbo.[UQ_LanguageId_Property]') IS NULL 
Begin
ALTER TABLE ExtendedCaseTranslations   
ADD CONSTRAINT UQ_LanguageId_Property UNIQUE (LanguageId, Property); 
end

RAISERROR ('Extend RegUserId lenght to 200 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'RegUserId' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory	
    alter column RegUserId nvarchar (200) null
END

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.51'
GO
