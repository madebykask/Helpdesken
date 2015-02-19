IF COL_LENGTH('dbo.tblDocumentCategory','ShowOnExternalPage') IS NULL
BEGIN
	ALTER TABLE [dbo].tblDocumentCategory ADD ShowOnExternalPage bit not null default (0)
END
GO