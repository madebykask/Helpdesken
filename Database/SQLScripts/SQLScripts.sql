-- update DB from 5.3.19 to 5.3.20 version

 IF COL_LENGTH('dbo.tblCaseInvoiceOrder','DeliveryPeriod') IS NOT NULL
 BEGIN
	 ALTER TABLE tblcaseinvoiceorder
	DROP COLUMN DeliveryPeriod
 END
-- New fields in tblCaseHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsAbout_Persons_Name' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD IsAbout_Persons_Name nvarchar(50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsAbout_ReportedBy' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD IsAbout_ReportedBy nvarchar(40) NULL
GO

 IF COL_LENGTH('dbo.tblCaseInvoiceOrder','Reference') IS NOT NULL
 BEGIN
	 ALTER TABLE tblcaseinvoiceorder
	DROP COLUMN Reference
 END

IF COL_LENGTH('dbo.tblCaseInvoiceOrder','CreditForOrder_Id') IS NULL
BEGIN
	ALTER TABLE tblCaseInvoiceOrder ADD CreditForOrder_Id int null 
END
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsAbout_Persons_Phone' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD IsAbout_Persons_Phone nvarchar(40) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsAbout_Department_Id' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD IsAbout_Department_Id int NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsAbout_UserCode' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD IsAbout_UserCode nvarchar(20) NULL
GO

ALTER TABLE tblCase ALTER COLUMN Persons_Phone nvarchar(50)
ALTER TABLE tblCase ALTER COLUMN Persons_CellPhone nvarchar(50)

ALTER TABLE tblCaseHistory ALTER COLUMN Persons_Phone nvarchar(50)
ALTER TABLE tblCaseHistory ALTER COLUMN Persons_CellPhone nvarchar(50)



 IF COL_LENGTH('tblCaseInvoiceArticle','IsInvoiced') IS NOT NULL
 BEGIN
    IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_tblCaseInvoiceArticle_IsInvoiced]') AND type = 'D')
	BEGIN
		ALTER TABLE [dbo].[tblCaseInvoiceArticle] Drop CONSTRAINT [DF_tblCaseInvoiceArticle_IsInvoiced]
	END
	ALTER TABLE tblCaseInvoiceArticle
	DROP COLUMN IsInvoiced
 END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.20'

