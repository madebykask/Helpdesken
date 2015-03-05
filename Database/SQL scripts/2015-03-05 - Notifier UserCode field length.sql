IF COL_LENGTH('dbo.tblComputerUsers','UserCode') IS NOT NULL
BEGIN
	ALTER TABLE [dbo].[tblComputerUsers] 
	ALTER COLUMN [UserCode] NVARCHAR(50) NOT NULL
END
GO 