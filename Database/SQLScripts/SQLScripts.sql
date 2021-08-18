--update DB from 5.3.51 to 5.3.52 version

--New table for Extended Case Form Templates
RAISERROR ('Create table ExtendedCaseFormTemplates', 10, 1) WITH NOWAIT
IF(OBJECT_ID('ExtendedCaseFormTemplates', 'U') IS NULL)
Begin	
	CREATE TABLE [dbo].[ExtendedCaseFormTemplates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Customer_Id]  [int] NOT NULL FOREIGN KEY REFERENCES tblUsers(Id),
	[MetaData] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[Guid] [uniqueidentifier] NULL,
	[Status] [int] NOT NULL,
	[Version] [int] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[ExtendedCaseFormTemplates] ADD  DEFAULT ((1)) FOR [Status]
ALTER TABLE [dbo].[ExtendedCaseFormTemplates] ADD  DEFAULT ((0)) FOR [Version]
END
GO

IF COL_LENGTH('tblSettings', 'BlockedEmailRecipients') IS NULL
	BEGIN
		ALTER TABLE tblSettings ADD BlockedEmailRecipients nvarchar(4000) NULL
	END
GO

RAISERROR ('Add Column Copy to tblComputerFieldSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputerFieldSettings','Copy') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblComputerFieldSettings]
	ADD [Copy] int Not Null default(1)
End
Go

RAISERROR ('Updating Copy in tblComputerFieldSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputerFieldSettings','Copy') IS NOT NULL
BEGIN
	UPDATE [tblComputerFieldSettings]
    SET [Copy] = 0
	WHERE ComputerField = 'Status' 
	OR ComputerField = 'Stolen'
	OR ComputerField = 'ReplacedWithComputerName'
	OR ComputerField = 'Sendback'
	OR ComputerField = 'ScrapDate'
	OR ComputerField = 'CreatedDate'
	OR ComputerField = 'ChangedDate'
	OR ComputerField = 'SyncChangedDate'
	OR ComputerField = 'ScanDate'
	OR ComputerField = 'LDAPPath'
	OR ComputerField = 'MACAddress'
	OR ComputerField = 'Theftmark'
END
GO

RAISERROR ('Add Column Customer_Id to tblOperatingSystem', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblOperatingSystem','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblOperatingSystem]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblOperatingSystem] WITH NOCHECK ADD CONSTRAINT [FK_tblOperatingSystem_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblProcessor', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblProcessor','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblProcessor]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblProcessor] WITH NOCHECK ADD CONSTRAINT [FK_tblProcessor_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblNIC', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblNIC','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblNIC]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblNIC] WITH NOCHECK ADD CONSTRAINT [FK_tblNIC_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblComputerModel', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputerModel','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblComputerModel]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblComputerModel] WITH NOCHECK ADD CONSTRAINT [FK_tblComputerModel_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblRAM', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblRAM','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblRAM]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblRAM] WITH NOCHECK ADD CONSTRAINT [FK_tblRAM_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.52'
GO


