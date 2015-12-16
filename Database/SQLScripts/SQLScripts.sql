-- update DB from 5.3.18 to 5.3.19 version
-- Nytt fält i tblAccountActivity
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreateCase_StateSecondary_Id' and sysobjects.name = N'tblAccountActivity')
begin
ALTER TABLE tblAccountActivity ADD CreateCase_StateSecondary_Id int NULL 

ALTER TABLE [dbo].[tblAccountActivity] ADD 
    CONSTRAINT [FK_tblAccountActivity_tblStateSecondary] FOREIGN KEY 
    (
                                [CreateCase_StateSecondary_Id]
    ) REFERENCES [dbo].[tblStateSecondary] (
                                [Id]
    )                           
end
GO

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


if not exists(select * from sysobjects WHERE Name = N'tblInvoiceArticle_tblProductArea')
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


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.19'

