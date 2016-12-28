-- update DB from 5.3.29 to 5.3.30 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPhone' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserPhone] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserEMail' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserEMail] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
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

Declare @OrderFieldValue nvarchar(50)
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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPersonalIdentityNumber' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserPersonalIdentityNumber] [nvarchar](200) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserPersonalIdentityNumber'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Personnummer'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserInitials' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserInitials] [nvarchar](10) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserInitials'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Initialer'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserExtension' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserExtension] [nvarchar](20) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserExtension'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Anknytning'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserTitle' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserTitle] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserTitle'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Titel'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserLocation' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserLocation] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserLocation'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Placering'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserRoomNumber' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserRoomNumber] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserRoomNumber'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Rum'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPostalAddress' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [UserPostalAddress] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserPostalAddress'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Besöksadress'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EmploymentType_Id' and sysobjects.name = N'tblOrder')	
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [EmploymentType_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblDepartment3] FOREIGN KEY([UserDepartment_Id2])
	REFERENCES [dbo].[tblDepartment] ([Id])
	
	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblDepartment3]
END 
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'EmploymentType'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Anställningstyp'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserDepartment_Id' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [UserDepartment_Id] [int] NULL 

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblDepartment1] FOREIGN KEY([UserDepartment_Id])
	REFERENCES [dbo].[tblDepartment] ([Id])
	
	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblDepartment1]
END 

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserDepartment_Id'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Avdelning'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserDepartment_Id2' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [UserDepartment_Id2] [int] NULL

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblDepartment3] FOREIGN KEY([UserDepartment_Id2])
	REFERENCES [dbo].[tblDepartment] ([Id])
	
	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblDepartment3]
END 

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserDepartment_Id2'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Flytt till förvaltning'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserOU_Id' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [UserOU_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblOU1] FOREIGN KEY([UserOU_Id])
	REFERENCES [dbo].[tblOU] ([Id])

	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblOU1]
END 

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'UserOU_Id'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Enhet'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InfoUser' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [InfoUser] [nvarchar](200) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'InfoUser'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Övrigt'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Responsibility' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [Responsibility] [nvarchar](20) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'Responsibility'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Ansvar'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Activity' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [Activity] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'Activity'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Verksamhet'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Manager' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [Manager] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'Manager'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Chef'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ReferenceNumber' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [ReferenceNumber] [nvarchar](20) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'ReferenceNumber'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Referensnummer'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountStartDate' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [AccountStartDate] [datetime] NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'AccountStartDate'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Startdatum'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountEndDate' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [AccountEndDate] [datetime] NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'AccountEndDate'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Slutdatum'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EMailType' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [EMailType] int NOT NULL CONSTRAINT [DF_tblOrder_EMailType] DEFAULT ((0))
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'EMailType'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'E-posttyp'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HomeDirectory' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [HomeDirectory] [bit] NOT NULL CONSTRAINT [DF_tblOrder_HomeDirectory]  DEFAULT ((0))
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'HomeDirectory'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Hemkatalog'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Profile' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [Profile] [bit] NOT NULL CONSTRAINT [DF_tblOrder_Profile]  DEFAULT ((0))
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'Profile'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Profil'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [InventoryNumber] [nvarchar](20) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'InventoryNumber'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Inventarienummer'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountInfo' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [AccountInfo] [nvarchar](500) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'AccountInfo'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Övrigt'

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

if not exists(select * from sysobjects WHERE Name = N'tblOrderFieldTypes')
BEGIN
	CREATE TABLE [dbo].[tblOrderFieldTypes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[OrderFieldType] [nvarchar](50) NOT NULL,
		[OrderType_Id] [int] NULL,
		[OrderField] [int] NOT NULL DEFAULT (1),
		[CreatedDate] [datetime] NOT NULL DEFAULT (getdate()),
		[ChangedDate] [datetime] NOT NULL DEFAULT (getdate()),
	 CONSTRAINT [PK_tblOrderFielTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblOrderFieldTypes]  WITH NOCHECK ADD CONSTRAINT [FK_tblOrderFieldTypes_tblOrderType] FOREIGN KEY([OrderType_Id])
	REFERENCES [dbo].[tblOrderType] ([Id])

	ALTER TABLE [dbo].[tblOrderFieldTypes] CHECK CONSTRAINT [FK_tblOrderFieldTypes_tblOrderType]

END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType_Id' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [OrderFieldType_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblOrderFieldTypes] FOREIGN KEY([OrderFieldType_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblOrderFieldTypes]
END

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'OrderFieldType'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Vallista'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType2' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [OrderFieldType2] [nvarchar](500) NULL
END

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'OrderFieldType2'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Vallista 2'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType3_Id' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [OrderFieldType3_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblOrderFieldTypes3] FOREIGN KEY([OrderFieldType3_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblOrderFieldTypes3]
END

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'OrderFieldType3'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Vallista 3'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType4_Id' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [OrderFieldType4_Id] [int] NULL

	
	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblOrderFieldTypes4] FOREIGN KEY([OrderFieldType4_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblOrderFieldTypes4]
END

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'OrderFieldType4'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Vallista 4'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType5_Id' and sysobjects.name = N'tblOrder')
BEGIN
	ALTER TABLE [dbo].[tblOrder] ADD [OrderFieldType5_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblOrderFieldTypes5] FOREIGN KEY([OrderFieldType5_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblOrderFieldTypes5]
END

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'OrderFieldType5'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Vallista 5'

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


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.30'

