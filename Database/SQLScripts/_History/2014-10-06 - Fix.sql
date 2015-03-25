IF COL_LENGTH('dbo.tblCustomer','CommunicateWithNotifier') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomer]
	ADD [CommunicateWithNotifier] INT NULL
	
	ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_CommunicateWithNotifier]  DEFAULT (1) FOR [CommunicateWithNotifier]
END
GO