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

	ALTER TABLE [dbo].[tblOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrder_tblEmploymentType] FOREIGN KEY([EmploymentType_Id])
	REFERENCES [dbo].[tblEmploymentType] ([Id])
	
	ALTER TABLE [dbo].[tblOrder] CHECK CONSTRAINT [FK_tblOrder_tblEmploymentType]
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
	ALTER TABLE [dbo].[tblOrder] ADD [EMailType] int NULL
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


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InvoiceRow_Id' and sysobjects.name = N'tblLog')
	ALTER TABLE [dbo].[tblLog] ADD [InvoiceRow_Id] INT NULL,
    FOREIGN KEY(InvoiceRow_Id) REFERENCES tblInvoiceRow(Id);
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactId' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [ContactId] [nvarchar](200) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'ContactId'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Id'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactName' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [ContactName] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'ContactName'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Namn'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactPhone' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [ContactPhone] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'ContactPhone'
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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactEMail' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [ContactEMail] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'ContactEMail'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'E-post'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryName' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [DeliveryName] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'DeliveryName'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Namn'

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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryPhone' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [DeliveryPhone] [nvarchar](50) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'DeliveryPhone'
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

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InfoProduct' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ADD [InfoProduct] [nvarchar](500) NULL
GO

Declare @OrderFieldValue nvarchar(50)
Set @OrderFieldValue = 'InfoProduct'
Declare @OrderFieldLabel nvarchar(50)
Set @OrderFieldLabel = 'Övrigt program'

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

update tblLog 
set InvoiceRow_Id = (select Id from tblInvoiceRow ir where ir.Case_Id = l.Case_Id)
from tblLog l

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InvoiceRow_Id' and sysobjects.name = N'tblCaseInvoiceRow')
	ALTER TABLE [dbo].[tblCaseInvoiceRow] ADD [InvoiceRow_Id] INT NULL,
    FOREIGN KEY(InvoiceRow_Id) REFERENCES tblInvoiceRow(Id);
GO

update tblCaseInvoiceRow
set InvoiceRow_Id = (select Id from tblInvoiceRow ir where ir.Case_Id = cir.Case_Id)
from tblCaseInvoiceRow cir
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPhone' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserPhone] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserEMail' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserEMail] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPersonalIdentityNumber' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserPersonalIdentityNumber] [nvarchar](200) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserInitials' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserInitials] [nvarchar](10) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserExtension' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserExtension] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserTitle' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserTitle] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserLocation' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserLocation] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserRoomNumber' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserRoomNumber] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserPostalAddress' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserPostalAddress] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EmploymentType_Id' and sysobjects.name = N'tblOrderHistory')	
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [EmploymentType_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblEmploymentType] FOREIGN KEY([EmploymentType_Id])
	REFERENCES [dbo].[tblEmploymentType] ([Id])
	
	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblEmploymentType]
END 
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserDepartment_Id' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserDepartment_Id] [int] NULL 

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblDepartment1] FOREIGN KEY([UserDepartment_Id])
	REFERENCES [dbo].[tblDepartment] ([Id])
	
	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblDepartment1]
END 

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserDepartment_Id2' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserDepartment_Id2] [int] NULL

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblDepartment3] FOREIGN KEY([UserDepartment_Id2])
	REFERENCES [dbo].[tblDepartment] ([Id])
	
	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblDepartment3]
END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserOU_Id' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [UserOU_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOU1] FOREIGN KEY([UserOU_Id])
	REFERENCES [dbo].[tblOU] ([Id])

	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblOU1]
END 

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InfoUser' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [InfoUser] [nvarchar](200) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Responsibility' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [Responsibility] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Activity' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [Activity] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Manager' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [Manager] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ReferenceNumber' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [ReferenceNumber] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountStartDate' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [AccountStartDate] [datetime] NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountEndDate' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [AccountEndDate] [datetime] NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EMailType' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [EMailType] int NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HomeDirectory' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [HomeDirectory] [bit] NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Profile' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [Profile] [bit] NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryNumber' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [InventoryNumber] [nvarchar](20) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountInfo' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [AccountInfo] [nvarchar](500) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType_Id' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [OrderFieldType_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes] FOREIGN KEY([OrderFieldType_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes]
END

--if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType2' and sysobjects.name = N'tblOrderHistory')
--BEGIN
--	ALTER TABLE [dbo].[tblOrderHistory] ADD [OrderFieldType2] [nvarchar](500) NULL
--END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType3_Id' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [OrderFieldType3_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes3] FOREIGN KEY([OrderFieldType3_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes3]
END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType4_Id' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [OrderFieldType4_Id] [int] NULL

	
	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes4] FOREIGN KEY([OrderFieldType4_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes4]
END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderFieldType5_Id' and sysobjects.name = N'tblOrderHistory')
BEGIN
	ALTER TABLE [dbo].[tblOrderHistory] ADD [OrderFieldType5_Id] [int] NULL

	ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes5] FOREIGN KEY([OrderFieldType5_Id])
		REFERENCES [dbo].[tblOrderFieldTypes] ([Id])

	ALTER TABLE [dbo].[tblOrderHistory] CHECK CONSTRAINT [FK_tblOrderHistory_tblOrderFieldTypes5]
END

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactId' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [ContactId] [nvarchar](200) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactName' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [ContactName] [nvarchar](50) NULL
GO


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactPhone' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [ContactPhone] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactEMail' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [ContactEMail] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryName' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [DeliveryName] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryPhone' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [DeliveryPhone] [nvarchar](50) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InfoProduct' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ADD [InfoProduct] [nvarchar](500) NULL
GO


if exists(select * from sysobjects WHERE Name = N'tblCaseExtraFollowers')
begin
  ALTER TABLE [dbo].[tblCaseExtraFollowers] ALTER COLUMN [CreatedByUser_Id] int NULL 
end
go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Deleted' and sysobjects.name = N'tblOrderFieldTypes')
	ALTER TABLE [dbo].[tblOrderFieldTypes] ADD [Deleted] BIT NOT NULL CONSTRAINT [DF_tblOrderFieldTypes_Deleted]  DEFAULT ((0))
GO

--Populate GUID
-- tblWorkingGroup
--New field in tblWorkingGroup
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroupGUID' and sysobjects.name = N'tblWorkingGroup')
	begin
		ALTER TABLE tblWorkingGroup ADD WorkingGroupGUID uniqueidentifier Default (newid()) NULL
	end
GO
--Update tblWorkinGroup with WorkingGroupGUID
UPDATE tblWorkingGroup
SET WorkingGroupGUID = newid()
WHERE WorkingGroupGUID IS NULL

-- tblCaseType
--New field in tblCaseType
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseTypeGUID' and sysobjects.name = N'tblCaseType')
	begin
		ALTER TABLE tblCaseType ADD CaseTypeGUID uniqueidentifier Default (newid()) NULL
	end
GO
--Update tblCaseType with CaseTypeGUID
UPDATE tblCasetype
SET CaseTypeGUID = newid()
WHERE CaseTypeGUID IS NULL

-- tblDepartment
--New field in tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DepartmentGUID' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD DepartmentGUID uniqueidentifier Default (newid()) NULL
	end
GO
--Update tblDepartment with DepartmentGUID
UPDATE tblDepartment
SET DepartmentGUID = newid()
WHERE DepartmentGUID IS NULL

-- tblFinishingCause
--New field in tblFinishingCause
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingCauseGUID' and sysobjects.name = N'tblFinishingCause')
	begin
		ALTER TABLE tblFinishingCause ADD FinishingCauseGUID uniqueidentifier Default (newid()) NULL
	end
GO
--Update tblFinishingCause with FinishingCauseGUID
UPDATE tblFinishingCause
SET FinishingCauseGUID = newid()
WHERE FinishingCauseGUID IS NULL

-- tblFinishingCauseCategory
--New field in tblFinishingCauseCategory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingCauseCategoryGUID' and sysobjects.name = N'tblFinishingCauseCategory')
	begin
		ALTER TABLE tblFinishingCauseCategory ADD FinishingCauseCategoryGUID uniqueidentifier Default (newid()) NULL
	end
GO
--Update tblFinishingCauseCategory with FinishingCauseCategoryGUID
UPDATE tblFinishingCauseCategory
SET FinishingCauseCategoryGUID = newid()
WHERE FinishingCauseCategoryGUID IS NULL

-- tblAccountActivityGroup
--New field in tblAccountActivityGroup
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountActivityGroupGUID' and sysobjects.name = N'tblAccountActivityGroup')
	begin
		ALTER TABLE tblAccountActivityGroup ADD AccountActivityGroupGUID uniqueidentifier Default (newid()) NULL
	end
GO

-- tblCaseFieldSettings
--New field in tblCaseFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseFieldSettingsGUID' and sysobjects.name = N'tblCaseFieldSettings')
	begin
		ALTER TABLE tblCaseFieldSettings ADD CaseFieldSettingsGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

UPDATE tblCaseFieldSettings
SET CaseFieldSettingsGUID = newid()
WHERE CaseFieldSettingsGUID IS NULL

-- tblCategory
--New field in tblCategory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CategoryGUID' and sysobjects.name = N'tblCategory')
	begin
		ALTER TABLE tblCategory ADD CategoryGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

UPDATE tblCategory
SET CategoryGUID = newid()
WHERE CategoryGUID IS NULL

--tblCausingpart
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CausingPartGUID' and sysobjects.name = N'tblCausingPart')
	begin
		ALTER TABLE tblCausingPart ADD CausingPartGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblCountry
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CountryGUID' and sysobjects.name = N'tblCountry')
	begin
		ALTER TABLE tblCountry ADD CountryGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblDocument
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DocumentGUID' and sysobjects.name = N'tblDocument')
	begin
		ALTER TABLE tblDocument ADD DocumentGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblDomain
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DomainGUID' and sysobjects.name = N'tblDomain')
	begin
		ALTER TABLE tblDomain ADD DomainGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblHoliday
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HolidayGUID' and sysobjects.name = N'tblHoliday')
	begin
		ALTER TABLE tblHoliday ADD HolidayGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblHolidayHeader
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HolidayHeaderGUID' and sysobjects.name = N'tblHolidayHeader')
	begin
		ALTER TABLE tblHolidayHeader ADD HolidayHeaderGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblImpact
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ImpactGUID' and sysobjects.name = N'tblImpact')
	begin
		ALTER TABLE tblImpact ADD ImpactGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblInfoText
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InfoTextGUID' and sysobjects.name = N'tblInfoText')
	begin
		ALTER TABLE tblInfoText ADD InfoTextGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblLanguage
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LanguageGUID' and sysobjects.name = N'tblLanguage')
	begin
		ALTER TABLE tblLanguage ADD LanguageGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblLinkGroup
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LinkGroupGUID' and sysobjects.name = N'tblLinkGroup')
	begin
		ALTER TABLE tblLinkGroup ADD LinkGroupGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblOperatingSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OperatingSystemGUID' and sysobjects.name = N'tblOperatingSystem')
	begin
		ALTER TABLE tblOperatingSystem ADD OperatingSystemGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblOrderState
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderStateGUID' and sysobjects.name = N'tblOrderState')
	begin
		ALTER TABLE tblOrderState ADD OrderStateGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblOrderType
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderTypeGUID' and sysobjects.name = N'tblOrderType')
	begin
		ALTER TABLE tblOrderType ADD OrderTypeGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblRegistrationSourceCustomer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RegistrationSourceCustomerGUID' and sysobjects.name = N'tblRegistrationSourceCustomer')
	begin
		ALTER TABLE tblRegistrationSourceCustomer ADD RegistrationSourceCustomerGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblReport
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ReportGUID' and sysobjects.name = N'tblReport')
	begin
		ALTER TABLE tblReport ADD ReportGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SettingsGUID' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD SettingsGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblSupplier
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SupplierGUID' and sysobjects.name = N'tblSupplier')
	begin
		ALTER TABLE tblSupplier ADD SupplierGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SystemGUID' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD SystemGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblText
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'TextGUID' and sysobjects.name = N'tblText')
	begin
		ALTER TABLE tblText ADD TextGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblTextTranslation
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'TextTranslationGUID' and sysobjects.name = N'tblTextTranslation')
	begin
		ALTER TABLE tblTextTranslation ADD TextTranslationGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblUrgency
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UrgencyGUID' and sysobjects.name = N'tblUrgency')
	begin
		ALTER TABLE tblUrgency ADD UrgencyGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO

--tblWatchDateCalendar
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WatchDateCalendarGUID' and sysobjects.name = N'tblWatchDateCalendar')
	begin
		ALTER TABLE tblWatchDateCalendar ADD WatchDateCalendarGUID uniqueidentifier Default (newid()) NOT NULL
	end
GO


--tblOU
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OUGUID' and sysobjects.name = N'tblOU')
begin
		EXECUTE  sp_executesql  "update tblOU set OUGUID = newid() where OUGUID is null" 

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblOU'
					  and c.name = 'OUGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_OUGUID')
		begin
			Alter table tblOU
			Add constraint DF_OUGUID default (newid()) For OUGUID		
		end		
end
else
begin
	Alter table tblOU
	--Add OUGUID uniqueIdentifier NOT NULL default (newid())
	Add OUGUID uniqueIdentifier NOT NULL CONSTRAINT DF_OUGUID default (newid())
end
GO


--tblPriority
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PriorityGUID' and sysobjects.name = N'tblPriority')
begin
		EXECUTE  sp_executesql  "update tblPriority set PriorityGUID = newid() where PriorityGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblPriority'
					  and c.name = 'PriorityGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_PriorityGUID')
		begin
			Alter table tblPriority
			Add constraint DF_PriorityGUID default (newid()) For PriorityGUID		
		end		
end
else
begin
	Alter table tblPriority
	--Add PriorityGUID uniqueIdentifier NOT NULL default (newid())
	Add PriorityGUID uniqueIdentifier NOT NULL CONSTRAINT DF_PriorityGUID default (newid())
end
GO

 --tblProductArea
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProductAreaGUID' and sysobjects.name = N'tblProductArea')
begin
		EXECUTE  sp_executesql  "update tblProductArea set ProductAreaGUID = newid() where ProductAreaGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblProductArea'
					  and c.name = 'ProductAreaGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_ProductAreaGUID')
		begin
			Alter table tblProductArea
			Add constraint DF_ProductAreaGUID default (newid()) For ProductAreaGUID		
		end		
end
else
begin
	Alter table tblProductArea
	--Add ProductAreaGUID uniqueIdentifier NOT NULL default (newid())
	Add ProductAreaGUID uniqueIdentifier NOT NULL CONSTRAINT DF_ProductAreaGUID default (newid())
end
GO

--tblLink
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LinkGUID' and sysobjects.name = N'tblLink')
begin
		EXECUTE  sp_executesql  "update tblLink set LinkGUID = newid() where LinkGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblLink'
					  and c.name = 'LinkGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_LinkGUID')
		begin
			Alter table tblLink
			Add constraint DF_LinkGUID default (newid()) For LinkGUID		
		end		
end
else
begin
	Alter table tblLink
	--Add LinkGUID uniqueIdentifier NOT NULL default (newid())
	Add LinkGUID uniqueIdentifier NOT NULL CONSTRAINT DF_LinkGUID default (newid())
end
GO

  --tblRegion
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RegionGUID' and sysobjects.name = N'tblRegion')
begin
		EXECUTE  sp_executesql  "update tblRegion set RegionGUID = newid() where RegionGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblRegion'
					  and c.name = 'RegionGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_RegionGUID')
		begin
			Alter table tblRegion
			Add constraint DF_RegionGUID default (newid()) For RegionGUID		
		end		
end
else
begin
	Alter table tblRegion
	--Add RegionGUID uniqueIdentifier NOT NULL default (newid())
	Add RegionGUID uniqueIdentifier NOT NULL CONSTRAINT DF_RegionGUID default (newid())
end
GO

   --tblStateSecondary
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondaryGUID' and sysobjects.name = N'tblStateSecondary')
begin
		EXECUTE  sp_executesql  "update tblStateSecondary set StateSecondaryGUID = newid() where StateSecondaryGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblStateSecondary'
					  and c.name = 'StateSecondaryGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_StateSecondaryGUID')
		begin
			Alter table tblStateSecondary
			Add constraint DF_StateSecondaryGUID default (newid()) For StateSecondaryGUID		
		end		
end
else
begin
	Alter table tblStateSecondary
	--Add StateSecondaryGUID uniqueIdentifier NOT NULL default (newid())
	Add StateSecondaryGUID uniqueIdentifier NOT NULL CONSTRAINT DF_StateSecondaryGUID default (newid())
end
GO

--tblStatus
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StatusGUID' and sysobjects.name = N'tblStatus')
begin
		EXECUTE  sp_executesql  "update tblStatus set StatusGUID = newid() where StatusGUID is null"

		if not exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblStatus'
					  and c.name = 'StatusGUID'
					  and s.name = 'dbo'
					  and d.name = 'DF_StatusGUID')
		begin
			Alter table tblStatus
			Add constraint DF_StatusGUID default (newid()) For StatusGUID		
		end		
end
else
begin
	Alter table tblStatus
	--Add StatusGUID uniqueIdentifier NOT NULL default (newid())
	Add StatusGUID uniqueIdentifier NOT NULL CONSTRAINT DF_StatusGUID default (newid())
end
GO

--tblComputerType
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ComputerTypeGUID' and sysobjects.name = N'tblComputerType')
begin
		EXECUTE  sp_executesql  "update tblComputerType set ComputerTypeGUID = newid() where ComputerTypeGUID is null" 

		if exists(select *
					  from sys.all_columns c
					  join sys.tables t on t.object_id = c.object_id
					  join sys.schemas s on s.schema_id = t.schema_id
					  join sys.default_constraints d on c.default_object_id = d.object_id
					  where t.name = 'tblComputerType'
					  and c.name = 'ComputerTypeGUID'
					  and s.name = 'dbo')
		begin
			declare @tmpObject nvarchar(100)
		end	
		else
		begin
			Alter table tblComputerType
			Add constraint DF_ComputerTypeGUID default (newid()) For ComputerTypeGUID		
		end	
end
else
begin
	Alter table tblComputerType
	--Add ComputerTypeGUID uniqueIdentifier NOT NULL default (newid())
	Add ComputerTypeGUID uniqueIdentifier NOT NULL CONSTRAINT DF_ComputerTypeGUID default (newid())
end
GO


 --New EMailIdentifier for ProductArea
UPDATE tblCaseFieldSettings SET EMailIdentifier = '[#28]'
WHERE CaseField = 'ProductArea_Id'

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InvoiceFileFolder' and sysobjects.name = N'tblGlobalSettings')
	begin
		ALTER TABLE tblGlobalSettings ADD InvoiceFileFolder NVARCHAR(500) NOT NULL Default ('')
	end
GO

-- New field in tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SettingForNoMail' and sysobjects.name = N'tblUsers')
	ALTER TABLE tblUsers ADD SettingForNoMail int Default(1) NOT NULL
GO

--New length in tblSettings
ALTER TABLE tblSettings
ALTER COLUMN LDAPBase nvarchar(200) NOT NULL



--tblComputerType
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryType_Id' and sysobjects.name = N'tblComputerType')
	begin
		ALTER TABLE tblComputerType ADD InventoryType_Id int NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'User_Id' and sysobjects.name = N'tblReportFavorites')
BEGIN
	ALTER TABLE [dbo].[tblReportFavorites] ADD [User_Id] INT NULL

	ALTER TABLE [dbo].[tblReportFavorites]  WITH NOCHECK ADD  CONSTRAINT [FK_tblReportFavorites_tblUsers] FOREIGN KEY([User_Id])
		REFERENCES [dbo].[tblUsers] ([Id])

	ALTER TABLE [dbo].[tblReportFavorites] CHECK CONSTRAINT [FK_tblReportFavorites_tblUsers]
END
GO

--tblPriority
ALTER TABLE tblPriority
ALTER COLUMN InformUserText nvarchar(520)

UPDATE [dbo].[tblOrderFieldSettings] SET Show = 0, ShowInList = 0, [Required] = 0, ShowExternal = 0
WHERE [OrderField] IN ('UserInitials', 'UserLocation',
 'UserPostalAddress', 'UserDepartment_Id', 'UserOU_Id',
  'Responsibility', 'ReferenceNumber', 'InventoryNumber',
  'AccountInfo', 'ContactId', 'ContactName',
  'ContactPhone', 'ContactEMail')
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'BatchEmail' and sysobjects.name = N'tblSettings')
BEGIN
	ALTER TABLE [dbo].[tblSettings] ADD [BatchEmail] BIT NOT NULL DEFAULT(0)
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Body' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [Body] NVARCHAR(MAX) NULL
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Subject' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [Subject] NVARCHAR(MAX) NULL
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Cc' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [Cc] NVARCHAR(1000) NULL 
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Bcc' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [Bcc] NVARCHAR(1000) NULL 
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HighPriority' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [HighPriority] BIT NOT NULL DEFAULT(0)
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Files' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [Files] NVARCHAR(MAX) NULL
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'From' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [From] NVARCHAR(1000) NULL
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SendStatus' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [SendStatus] INT NOT NULL DEFAULT(0)
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LastAttempt' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [LastAttempt] DATETIME NULL
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Attempts' and sysobjects.name = N'tblEmailLog')
BEGIN
	ALTER TABLE [dbo].[tblEmailLog] ADD [Attempts] INT NULL
END
GO

if not exists(select * from sysobjects WHERE Name = N'tblEmailLogAttempts')
BEGIN
	CREATE TABLE [dbo].[tblEmailLogAttempts](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[EmailLog_Id] INT NOT NULL,
		[Date] DATETIME NOT NULL,
		[Message] NVARCHAR(MAX) NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_tblEmailLogAttempts_tblEmailLog] FOREIGN KEY ([EmailLog_Id]) REFERENCES [dbo].[tblEmailLog] ([Id])
	)
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionGeneral' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD [CaptionGeneral] [nvarchar](30) NULL
GO

UPDATE [dbo].[tblOrderType] SET [CaptionGeneral] = N'Allmänt'
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionOrder' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD [CaptionOrder] [nvarchar](30) NULL
GO

UPDATE [dbo].[tblOrderType] SET [CaptionOrder] = N'Beställning'
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionOrderInfo' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD [CaptionOrderInfo] [nvarchar](30) NULL
GO

UPDATE [dbo].[tblOrderType] SET [CaptionOrderInfo] = N'Beställningsinformation'
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionDeliveryInfo' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD [CaptionDeliveryInfo] [nvarchar](30) NULL
GO

UPDATE [dbo].[tblOrderType] SET [CaptionDeliveryInfo] = N'Leveransinformation'
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionProgram' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD [CaptionProgram] [nvarchar](30) NULL
GO

UPDATE [dbo].[tblOrderType] SET [CaptionProgram] = N'Program'
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionOther' and sysobjects.name = N'tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD [CaptionOther] [nvarchar](30) NULL
GO

UPDATE [dbo].[tblOrderType] SET [CaptionOther] = N'Övrigt'
GO

if not exists(select * from tblDate where DateKey = '20170101')
begin
 exec [dbo].[sp_PopulateTblDate] '2017-01-01', '2017-12-31'
end
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.30'
