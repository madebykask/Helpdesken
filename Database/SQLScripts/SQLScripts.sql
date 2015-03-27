-- 2013-03-23 Aleksei Matveev
-- Change 'DeliveryPeriod' column type to NVARCHAR(200)
IF COL_LENGTH('dbo.tblCaseInvoiceOrder','DeliveryPeriod') IS NOT NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseInvoiceOrder]
	ALTER COLUMN [DeliveryPeriod] NVARCHAR(200) NULL
END
GO 

IF COL_LENGTH('dbo.tblCaseSettings','ColStyle') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSettings]
	ADD [ColStyle] nvarchar(50) NULL 
END
GO 
-- 2013-03-24 Aleksei Matveev
-- Add 'ShowSolutionTime' into 'tblUsers'
IF COL_LENGTH('dbo.tblUsers','ShowSolutionTime') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblUsers]
	ADD [ShowSolutionTime] INT NOT NULL DEFAULT(0)
END
GO 

-- 2013-03-24 Aleksei Matveev
-- Add 'IsDefault' into 'tblUsers'
IF COL_LENGTH('dbo.tblUserWorkingGroup','IsDefault') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblUserWorkingGroup]
	ADD [IsDefault] INT NOT NULL DEFAULT(0)
END
GO 

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Persons_Phone')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#9]' where casefield = 'Persons_Phone'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Persons_EMail')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#8]' where casefield = 'Persons_EMail'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Description')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#5]' where casefield = 'Description'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Caption')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#4]' where casefield = 'Caption'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Persons_Name')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#3]' where casefield = 'Persons_Name'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'ReportedBy')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#27]' where casefield = 'ReportedBy'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Category_Id')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#26]' where casefield = 'Category_Id'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'CaseType_Id')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#25]' where casefield = 'CaseType_Id'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Place')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#24]' where casefield = 'Place'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Miscellaneous')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#23]' where casefield = 'Miscellaneous'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'WatchDate')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#21]' where casefield = 'WatchDate'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Customer_Id')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#2]' where casefield = 'Customer_Id'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Available')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#19]' where casefield = 'Available'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Persons_CellPhone')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#18]' where casefield = 'Persons_CellPhone'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'InventoryNumber')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#17]' where casefield = 'InventoryNumber'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'RegTime')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#16]' where casefield = 'RegTime'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'WorkingGroup_Id')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#15]' where casefield = 'WorkingGroup_Id'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'Priority_Id')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#12]' where casefield = 'Priority_Id'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'tblLog.Text_Internal')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#11]' where casefield = 'tblLog.Text_Internal'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'tblLog.Text_External')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#10]' where casefield = 'tblLog.Text_External'
end

if exists(select * from [dbo].[tblCaseFieldSettings] where casefield = 'CaseNumber')
begin
	update tblcasefieldsettings set EMailIdentifier = '[#1]' where casefield = 'CaseNumber'
end

-------------------------------------------------------------------
-- Support for storing grid settings for user and customer
if not exists(select * from sysobjects WHERE Name = N'UserGridSettings')
BEGIN
	CREATE TABLE [dbo].[UserGridSettings](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CustomerId] [int] not null,
		[UserId] [int] NOT NULL,
		[GridId] [nchar](20) NOT NULL,
		[Parameter] [nchar](20) NOT NULL,
		[Value] [nchar](20) NOT NULL,
		CONSTRAINT [PK_UserGridSettings] PRIMARY KEY CLUSTERED 
		(
			[CustomerId] ASC,
			[UserId] ASC,
			[GridId] ASC
		)
	) ON [PRIMARY]


	ALTER TABLE [dbo].[UserGridSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_UserGridSettings_tblCustomer] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[tblCustomer] ([Id])


	ALTER TABLE [dbo].[UserGridSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_UserGridSettings_tblUsers] FOREIGN KEY([UserId])
	REFERENCES [dbo].[tblUsers] ([Id])

END
go