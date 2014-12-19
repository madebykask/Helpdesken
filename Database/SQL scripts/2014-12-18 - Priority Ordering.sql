IF COL_LENGTH('dbo.tblPriority','OrderNum') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblPriority]	ADD [OrderNum] INT NULL
END
GO