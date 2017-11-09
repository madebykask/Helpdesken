
--update DB from 5.3.34 to 5.3.35 version



IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceTime' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceTime] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceOvertime' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceOvertime] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceMaterial' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceMaterial] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoicePrice' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoicePrice] bit NOT NULL Default(1)
end

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.35'