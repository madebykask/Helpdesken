 IF COL_LENGTH('tblcaseinvoiceorder','InvoiceDate') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add InvoiceDate datetime2(7) null
 END

  IF COL_LENGTH('tblcaseinvoiceorder','InvoicedByUserId') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add InvoicedByUserId int null
 END


  IF COL_LENGTH('tblcaseinvoiceorder','ReportedBy') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add ReportedBy nvarchar(100) null
 END
 
 IF COL_LENGTH('tblcaseinvoiceorder','Persons_Name') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Persons_Name nvarchar(100) null
 END
 
  IF COL_LENGTH('tblcaseinvoiceorder','Persons_Email') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Persons_Email nvarchar(100) null
 END

  IF COL_LENGTH('tblcaseinvoiceorder','Persons_Phone') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Persons_Phone nvarchar(100) null
 END

  IF COL_LENGTH('tblcaseinvoiceorder','Persons_Cellphone') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Persons_Cellphone nvarchar(100) null
 END

   IF COL_LENGTH('tblcaseinvoiceorder','Region_Id') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Region_Id int null
 END

    IF COL_LENGTH('tblcaseinvoiceorder','Department_Id') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Department_Id int null
 END

     IF COL_LENGTH('tblcaseinvoiceorder','OU_Id') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add OU_Id int null
 END

      IF COL_LENGTH('tblcaseinvoiceorder','Place') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add Place nvarchar(100) null
 END

       IF COL_LENGTH('tblcaseinvoiceorder','UserCode') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add UserCode nvarchar(100) null
 END

        IF COL_LENGTH('tblcaseinvoiceorder','CostCentre') IS NULL
 BEGIN
	alter table tblcaseinvoiceorder
	add CostCentre nvarchar(100) null
 END


         IF COL_LENGTH('tblcaseinvoiceorder','DeliveryPeriod') IS NOT NULL
 BEGIN
	 ALTER TABLE tblcaseinvoiceorder
DROP COLUMN DeliveryPeriod
 END


          IF COL_LENGTH('tblcaseinvoiceorder','Reference') IS NOT NULL
 BEGIN
	 ALTER TABLE tblcaseinvoiceorder
	DROP COLUMN Reference
 END


IF object_id('tblCaseInvoiceSettings', 'U') is not null
begin
	CREATE TABLE [dbo].[tblCaseInvoiceSettings](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CustomerId] [int] NOT NULL,
		[ExportPath] [nvarchar](200) NULL,
		[Currency] [nvarchar](50) NULL,
		[OrderNoPrefix] [nvarchar](50) NULL,
		[Issuer] [nvarchar](50) NULL,
		[OurReference] [nvarchar](50) NULL,
	 CONSTRAINT [PK_tblCaseInvoiceSettings] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]
end
go

ALTER TABLE [dbo].[tblCaseInvoiceSettings]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseInvoiceSettings_tblCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tblCustomer] ([Id])
GO

ALTER TABLE [dbo].[tblCaseInvoiceSettings] CHECK CONSTRAINT [FK_tblCaseInvoiceSettings_tblCustomer]
GO




           IF COL_LENGTH('tblCaseInvoiceSettings','OurReference') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add OurReference nvarchar(50) null
 end

            IF COL_LENGTH('tblInvoiceArticle','OurReference') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add OurReference nvarchar(50) null
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


 
IF object_id('tblInvoiceArticle_tblProductArea', 'U') is not null
begin
	CREATE TABLE [dbo].[tblInvoiceArticle_tblProductArea](
		[InvoiceArticle_Id] [int] NOT NULL,
		[ProductArea_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblInvoiceArticle_tblProductArea] PRIMARY KEY CLUSTERED 
	(
		[InvoiceArticle_Id] ASC,
		[ProductArea_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO

ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblInvoiceArticle] FOREIGN KEY([InvoiceArticle_Id])
REFERENCES [dbo].[tblInvoiceArticle] ([Id])
GO

ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblInvoiceArticle]
GO

ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblProductArea] FOREIGN KEY([ProductArea_Id])
REFERENCES [dbo].[tblProductArea] ([Id])
GO

ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblProductArea]
GO

IF COL_LENGTH('tblInvoiceArticle','ProductAreaId') IS NOT NULL
begin
	alter table tblinvoicearticle drop column productareaid
end

IF COL_LENGTH('tblInvoiceArticle','Ppu') IS NOT NULL
begin
	alter table tblinvoicearticle 
	alter column Ppu decimal(18,3) null
end