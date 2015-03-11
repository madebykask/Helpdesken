-- Majid 2015-03-03
IF COL_LENGTH('dbo.tblCustomer','ShowCoWorkersOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowCoWorkersOnExternalPage int not null default (0)
END
GO


-- Majid 2015-03-03
IF COL_LENGTH('dbo.tblCustomer','ShowHelpOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCustomer ADD ShowHelpOnExternalPage int not null default (1)
END
GO