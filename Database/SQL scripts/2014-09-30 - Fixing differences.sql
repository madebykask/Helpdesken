IF COL_LENGTH('dbo.tblText','ChangedByUser_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblText]
	ADD [ChangedByUser_Id] INT NULL
END
GO

IF COL_LENGTH('dbo.tblTextTranslation','ChangedByUser_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblTextTranslation]
	ADD [ChangedByUser_Id] INT NULL
END
GO

IF COL_LENGTH('dbo.tblTextTranslation','ChangedDate') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblTextTranslation]
	ADD [ChangedDate] DATETIME NULL
	
	ALTER TABLE [dbo].[tblTextTranslation] ADD  CONSTRAINT [DF_tblTextTranslation_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
	
END
GO

IF COL_LENGTH('dbo.tblTextTranslation','CreatedDate') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblTextTranslation]
	ADD [CreatedDate] DATETIME NULL
	
	ALTER TABLE [dbo].[tblTextTranslation] ADD  CONSTRAINT [DF_tblTextTranslation_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
	
END
GO