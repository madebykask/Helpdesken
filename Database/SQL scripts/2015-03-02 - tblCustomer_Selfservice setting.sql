IF COL_LENGTH('dbo.tblCustomer','ShowFAQOnExternalStartPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowFAQOnExternalStartPage int not
END
GO