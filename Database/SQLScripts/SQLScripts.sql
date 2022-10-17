--update DB from 5.3.56 to 5.3.57 version
RAISERROR ('Add Column SiteURL to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SiteURL') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblSettings
		ADD SiteURL nvarchar(100) Null
	End
Go

RAISERROR ('Add Column SelfServiceURL to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SelfServiceURL') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblSettings
		ADD SelfServiceURL nvarchar(100) Null
	End

Go

RAISERROR ('Add Column GDPRType to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','GDPRType') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD GDPRType int not Null default 0
	End

Go

RAISERROR ('Add Column CaseTypes to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','CaseTypes') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD CaseTypes nvarchar(256) Null
	End

Go

RAISERROR ('Add Column GDPRType to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','GDPRType') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD GDPRType int not Null default 0
	End

Go

RAISERROR ('Add Column CaseTypes to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','CaseTypes') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD CaseTypes nvarchar(256) Null
	End

Go

RAISERROR ('Add Foreign Key Customer_Id to tblRegistrationSourceCustomer', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblRegistrationSourceCustomer_tblCustomer]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblRegistrationSourceCustomer] WITH CHECK ADD CONSTRAINT [FK_tblRegistrationSourceCustomer_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblMail2TicketCase', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMail2TicketCase_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblMail2TicketCase] WITH CHECK ADD CONSTRAINT [FK_tblMail2TicketCase_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblLocalAdmin', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblLocalAdmin_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblLocalAdmin] WITH CHECK ADD CONSTRAINT [FK_tblLocalAdmin_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblFormFieldValueHistory', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblFormFieldValueHistory_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblFormFieldValueHistory] WITH CHECK ADD CONSTRAINT [FK_tblFormFieldValueHistory_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Delete from tblCaseStatistics where there no longer is a connected case', 10, 1) WITH NOWAIT
DELETE cs
FROM [dbo].[tblCaseStatistics] AS cs
LEFT JOIN tblCase as c
	on cs.Case_Id = c.Id
WHERE c.Id IS NULL
GO


RAISERROR ('Add Foreign Key Case_Id to tblCaseStatistics', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseStatistics_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblCaseStatistics] WITH CHECK ADD CONSTRAINT [FK_tblCaseStatistics_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Customer_Id to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseFilterFavorite_tblCustomer]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblCaseFilterFavorite] WITH CHECK ADD CONSTRAINT [FK_tblCaseFilterFavorite_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END
GO

RAISERROR ('Add Foreign Key Customer_Id to tblBR_Rules', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblBR_Rules_tblCustomer]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblBR_Rules] WITH CHECK ADD CONSTRAINT [FK_tblBR_Rules_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END
GO

RAISERROR ('Delete from tblMergedCases Child where there no longer is a connected case', 10, 1) WITH NOWAIT
DELETE mcc
FROM tblMergedCases AS mcc
LEFT JOIN tblcase AS c
	ON mcc.MergedChild_Id = c.Id
WHERE c.Id IS NULL
GO

RAISERROR ('Delete from tblMergedCases Parent where there no longer is a connected case', 10, 1) WITH NOWAIT
DELETE mcp
FROM tblMergedCases AS mcp
LEFT JOIN tblcase AS c
	ON mcp.MergedParent_Id = c.Id
WHERE c.Id IS NULL
GO

RAISERROR ('Add Foreign Key Case_Id to tblMergedCases MergedParent_Id', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Parent]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblMergedCases] WITH CHECK ADD CONSTRAINT [FK_tblMergedCases_tblCase_Parent] FOREIGN KEY([MergedParent_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblMergedCases MergedChild_Id', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Child]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblMergedCases] WITH CHECK ADD CONSTRAINT [FK_tblMergedCases_tblCase_Child] FOREIGN KEY([MergedChild_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.57'
GO