
IF COL_LENGTH('dbo.tblCaseSolution','OrderNum') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [OrderNum] INT NULL
END
GO
