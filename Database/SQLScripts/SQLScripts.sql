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

 IF COL_LENGTH('tblCaseInvoiceSettings','DocTemplate') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add DocTemplate nvarchar(50) null
 end

 -- New field in tblProductArea
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnExternalPage' and sysobjects.name = N'tblProductArea')
	ALTER TABLE tblProductArea ADD ShowOnExternalPage int Default(1) NOT NULL
GO

-- New field in tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DocumentPermission' and sysobjects.name = N'tblUsers')
	ALTER TABLE tblUsers ADD DocumentPermission int Default(0) NOT NULL
GO


IF COL_LENGTH('tblInvoiceArticle','Number') IS NOT NULL
begin
    alter table tblinvoicearticle 
	alter column Number Nvarchar(15) not null
end
 
IF COL_LENGTH('tblInvoiceArticle','ProductAreaId') IS NOT NULL
begin
	alter table tblInvoiceArticle drop column ProductAreaId
end

IF COL_LENGTH('tblInvoiceArticle','Ppu') IS NOT NULL
begin
	alter table tblinvoicearticle 
	alter column Ppu decimal(18,3) null
end


IF COL_LENGTH('tblInvoiceArticle','NameEng') IS NULL
 BEGIN
	alter table tblInvoiceArticle
	add NameEng Nvarchar(100) not null 
 end


 IF COL_LENGTH('tblInvoiceArticle','Description') IS NULL
 BEGIN
	alter table tblInvoiceArticle
	add [Description] nvarchar(200) not null 
 end

 

  IF COL_LENGTH('tblInvoiceArticle','TextDemand') IS NULL
 BEGIN
	alter table tblInvoiceArticle
	add TextDemand bit null 
 end

 IF COL_LENGTH('tblInvoiceArticle','Blocked') IS NULL
 BEGIN
	alter table tblInvoiceArticle
	add Blocked bit null 
 end

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.20'

