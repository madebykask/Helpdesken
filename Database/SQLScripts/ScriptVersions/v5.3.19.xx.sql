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
	add InvoiceDate datetime null
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

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblInvoiceArticle_tblProductArea_tblInvoiceArticle]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblInvoiceArticle_tblProductArea]'))
begin
	ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblInvoiceArticle] FOREIGN KEY([InvoiceArticle_Id])
	REFERENCES [dbo].[tblInvoiceArticle] ([Id])
end
GO

ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblInvoiceArticle]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblInvoiceArticle_tblProductArea_tblProductArea]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblInvoiceArticle_tblProductArea]'))
begin
	ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblProductArea] FOREIGN KEY([ProductArea_Id])
	REFERENCES [dbo].[tblProductArea] ([Id])
end
GO

ALTER TABLE [dbo].[tblInvoiceArticle_tblProductArea] CHECK CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblProductArea]
GO

if not exists(select * from sysobjects WHERE Name = N'tblCaseInvoiceSettings')
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
	
	ALTER TABLE [dbo].[tblCaseInvoiceSettings]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseInvoiceSettings_tblCustomer] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[tblCustomer] ([Id])
	

	ALTER TABLE [dbo].[tblCaseInvoiceSettings] CHECK CONSTRAINT [FK_tblCaseInvoiceSettings_tblCustomer]

end
go	

 IF COL_LENGTH('tblCaseInvoiceSettings','Currency') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add Currency nvarchar(50) null
 END

 IF COL_LENGTH('tblCaseInvoiceSettings','OrderNoPrefix') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add OrderNoPrefix nvarchar(50) null
 END

 IF COL_LENGTH('tblCaseInvoiceSettings','Issuer') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add Issuer nvarchar(50) null
 END

 IF COL_LENGTH('tblCaseInvoiceSettings','OurReference') IS NULL
 BEGIN
	alter table tblCaseInvoiceSettings
	add OurReference nvarchar(50) null
 END

if exists (select id from tblcase where CaseType_Id not in (select id from tblCaseType))
begin
	Begin Try
 	    Begin Tran        
		Set Identity_Insert [tblCaseType] On  

		insert into tblCaseType (Id, Customer_Id, CaseType, isDefault, RequireApproving, ShowOnExternalPage, Parent_CaseType_Id, RelatedField,
																				ITILProcess, isEMailDefault, AutomaticApproveTime, Form_Id, [User_Id], [Status], Selectable, CreatedDate, ChangedDate)
		select distinct c.CaseType_Id, c.customer_Id, 'CaseType_' + Cast(CaseType_Id as nvarchar) as ctName,
														0,0,0,Null, '',0,0,0,Null, Null, 0,1, GETDATE(), GETDATE()  
		from tblCase c
		Where CaseType_Id not in (Select id from tblCaseType)

		Set Identity_Insert [tblCaseType] Off
	    Commit Tran
	End Try
	Begin Catch
				RollBack Tran
	End Catch
end

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCase_tblCaseType]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCase]'))
ALTER TABLE [dbo].[tblCase]  WITH CHECK ADD  CONSTRAINT [FK_tblCase_tblCaseType] FOREIGN KEY([CaseType_Id])
REFERENCES [dbo].[tblCaseType] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCase_tblCaseType]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCase]'))
ALTER TABLE [dbo].[tblCase] CHECK CONSTRAINT [FK_tblCase_tblCaseType]
GO

-- IsAbout in Administration
insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_ReportedBy', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_ReportedBy')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_Name', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_Name')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_EMail', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_EMail')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_Phone', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_Phone')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_CellPhone', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_CellPhone')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Region_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Region_Id')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Department_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Department_Id')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_OU_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_OU_Id')


insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_CostCentre', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_CostCentre')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Place', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Place')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_UserCode', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_UserCode')



-- IsAbout
if not exists(select * from sysobjects WHERE Name = N'tblCaseIsAbout')
BEGIN
CREATE TABLE [dbo].[tblCaseIsAbout](            
            [Case_Id] [int] NOT NULL,
            [ReportedBy] [nvarchar](40) NULL,
            [Person_Name] [nvarchar](50) NULL,
            [Person_Email] [nvarchar](100) NULL,
            [Person_Phone] [nvarchar](40) NULL,
            [Person_CellPhone] [nvarchar](30) NULL,
            [Region_Id] [int] NULL,
            [Department_Id] [int] NULL,
            [OU_Id] [int] NULL,
            [CostCentre] [nvarchar](50) NULL,
            [Place] [nvarchar](100) NULL,
            [UserCode] [nvarchar](20) NULL,
CONSTRAINT [PK_tblCaseIsAbout] PRIMARY KEY CLUSTERED 
(
            [Case_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblCase]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseIsAbout_tblCase] FOREIGN KEY([Case_Id])
REFERENCES [dbo].[tblCase] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblCase]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout] CHECK CONSTRAINT [FK_tblCaseIsAbout_tblCase]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblDepartment]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseIsAbout_tblDepartment] FOREIGN KEY([Department_Id])
REFERENCES [dbo].[tblDepartment] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblDepartment]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout] CHECK CONSTRAINT [FK_tblCaseIsAbout_tblDepartment]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblOU]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseIsAbout_tblOU] FOREIGN KEY([OU_Id])
REFERENCES [dbo].[tblOU] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblOU]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout] CHECK CONSTRAINT [FK_tblCaseIsAbout_tblOU]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblRegion]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseIsAbout_tblRegion] FOREIGN KEY([Region_Id])
REFERENCES [dbo].[tblRegion] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseIsAbout_tblRegion]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseIsAbout]'))
ALTER TABLE [dbo].[tblCaseIsAbout] CHECK CONSTRAINT [FK_tblCaseIsAbout_tblRegion]
GO


IF COL_LENGTH('tblCustomer','UseInternalLogNoteOnExternalPage') IS NULL
 BEGIN
	alter table tblCustomer
	add UseInternalLogNoteOnExternalPage int not null default(0)
 END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.19'

