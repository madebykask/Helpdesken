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



if exists (select id from tblcase where CaseType_Id not in (select id from tblCaseType))
begin
	Begin Try
 	    Begin Tran        
		Set Identity_Insert [tblCaseType] On  

		insert into tblCaseType (Id, Customer_Id, CaseType, isDefault, RequireApproving, ShowOnExternalPage, Parent_CaseType_Id, RelatedField,
																				ITILProcess, isEMailDefault, AutomaticApproveTime, Form_Id, [User_Id], [Status], Selectable, CreatedDate, ChangedDate)
		select distinct c.CaseType_Id, c.customer_Id, 'CaseType_' + Cast(CaseType_Id as nvarchar) as ctName,
														0,0,0,Null, '',0,0,0,Null, Null, 1,1, GETDATE(), GETDATE()  
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


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.19'

