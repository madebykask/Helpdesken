
-- Not required if 1.0.4 update was made
IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultLanguageId' and sysobjects.name = N'ExtendedCaseForms')
begin
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [DefaultLanguageId] [int] NULL 
end
GO