-- update DB from 5.3.30 to 5.3.31 version

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblUsers_UserId_Status')
	CREATE INDEX IX_tblUsers_UserId_Status ON tblUsers (UserId, Status)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCase_Customer_Id_User_Id')
	CREATE INDEX IX_tblCase_Customer_Id_User_Id ON tblCase (Customer_Id, User_Id)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblUsers_UserId_Password_Status')
	CREATE INDEX IX_tblUsers_UserId_Password_Status ON tblUsers (UserId, Password, Status)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblProductArea_Customer_Id_Parent_ProductArea_Id')
	CREATE INDEX IX_tblProductArea_Customer_Id_Parent_ProductArea_Id ON tblProductArea (Customer_Id, Parent_ProductArea_Id)
GO

if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCustomerUser_UserId')
	CREATE INDEX IX_tblCustomerUser_UserId ON tblCustomerUser (User_Id)
GO

UPDATE [dbo].[tblOrderType] SET [CaptionUserInfo] = N'Användare' WHERE [CaptionUserInfo] is NULL
UPDATE [dbo].[tblOrderType] SET [CaptionOrdererInfo] = N'Beställare' WHERE [CaptionOrdererInfo] is NULL
UPDATE [dbo].[tblOrderType] SET [CaptionReceiverInfo] = N'Kontakt' WHERE [CaptionReceiverInfo] is NULL
GO

--New fields in tblCaseFieldSettings
INSERT INTO tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'Project', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'Project')

INSERT INTO tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'Problem', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'Problem')

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrdererID' and sysobjects.name = N'tblOrder')
	ALTER TABLE [dbo].[tblOrder] ALTER COLUMN [OrdererID] [nvarchar](600) NULL
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrdererID' and sysobjects.name = N'tblOrderHistory')
	ALTER TABLE [dbo].[tblOrderHistory] ALTER COLUMN [OrdererID] [nvarchar](600) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MultiValue' and sysobjects.name = N'tblOrderFieldSettings')
	ALTER TABLE [dbo].[tblOrderFieldSettings] ADD [MultiValue] [bit] NOT NULL CONSTRAINT [DF_tblOrderFieldSettings_MultiValue]  DEFAULT ((0))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesInitiator' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesInitiator] [bit] NOT NULL DEFAULT ((0))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesRegistrator' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesRegistrator] [bit] NOT NULL DEFAULT ((1))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesFollower' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesFollower] [bit] NOT NULL DEFAULT ((0))
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MyCasesRegarding' and sysobjects.name = N'tblCustomer')
	ALTER TABLE [dbo].[tblCustomer] ADD [MyCasesRegarding] [bit] NOT NULL DEFAULT ((0))
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.31'
