IF COL_LENGTH('dbo.tblCustomer','ShowDocumentsOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowDocumentsOnExternalPage int not null default (0)
END
GO