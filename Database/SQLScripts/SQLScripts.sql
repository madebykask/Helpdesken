-- update DB from 5.3.29 to 5.3.30 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPhone' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserPhone] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserEMail' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserEMail] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(20)
Set @OrderFieldValue = 'UserPhone'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Telefon'

INSERT INTO [dbo].[tblOrderFieldSettings]
           ([OrderType_Id],[Customer_Id],[OrderField],[Show],[ShowInList],[ShowExternal]
           ,[Label],[Required],[DefaultValue],[EMailIdentifier],[CreatedDate],[ChangedDate])
	 SELECT distinct ofs.[OrderType_Id], ofs.[Customer_Id], @OrderFieldValue, 0, 0, 0,
		   @OrderFieldLabel, 0, '', NULL, GETDATE(), GETDATE()
		   from [dbo].[tblOrderFieldSettings] as ofs
		   where Not Exists (Select iofs.Id from [dbo].[tblOrderFieldSettings] as iofs
				where iofs.OrderField = @OrderFieldValue and iofs.Customer_Id = ofs.Customer_Id 
				and ((iofs.OrderType_Id is not null and ofs.OrderType_Id is not null and iofs.OrderType_Id = ofs.OrderType_Id) or (iofs.OrderType_Id is null and ofs.OrderType_Id is null))) 
GO

Declare @OrderFieldValue nvarchar(20)
Set @OrderFieldValue = 'UserEMail'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'E-Post'

INSERT INTO [dbo].[tblOrderFieldSettings]
           ([OrderType_Id],[Customer_Id],[OrderField],[Show],[ShowInList],[ShowExternal]
           ,[Label],[Required],[DefaultValue],[EMailIdentifier],[CreatedDate],[ChangedDate])
	 SELECT distinct ofs.[OrderType_Id], ofs.[Customer_Id], @OrderFieldValue, 0, 0, 0,
		   @OrderFieldLabel, 0, '', NULL, GETDATE(), GETDATE()
		   from [dbo].[tblOrderFieldSettings] as ofs
		   where Not Exists (Select iofs.Id from [dbo].[tblOrderFieldSettings] as iofs
				where iofs.OrderField = @OrderFieldValue and iofs.Customer_Id = ofs.Customer_Id 
				and ((iofs.OrderType_Id is not null and ofs.OrderType_Id is not null and iofs.OrderType_Id = ofs.OrderType_Id) or (iofs.OrderType_Id is null and ofs.OrderType_Id is null))) 
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FieldHelp' and sysobjects.name = N'tblOrderFieldSettings')
	ALTER TABLE [dbo].[tblOrderFieldSettings] ADD [FieldHelp] [nvarchar](200) NULL
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LastSyncedDate' and sysobjects.name = N'tblInvoiceArticle')
	ALTER TABLE [dbo].tblInvoiceArticle ADD [LastSyncedDate] [datetime] NOT NULL DEFAULT(GETDATE())
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'tblInvoiceArticle')
	ALTER TABLE [dbo].tblInvoiceArticle ADD [Status] [int] NOT NULL DEFAULT(1)
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.30'

