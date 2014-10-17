IF COL_LENGTH('dbo.tblLicenseFile','File') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblLicenseFile]
	ADD [File] IMAGE NULL
END
GO