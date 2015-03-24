-- 2013-03-23 Aleksei Matveev
-- Change 'DeliveryPeriod' column type to NVARCHAR(200)
IF COL_LENGTH('dbo.tblCaseInvoiceOrder','DeliveryPeriod') IS NOT NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseInvoiceOrder]
	ALTER COLUMN [DeliveryPeriod] NVARCHAR(200) NULL
END
GO 

-- 2013-03-24 Aleksei Matveev
-- Add 'ShowSolutionTime' into 'tblUsers'
IF COL_LENGTH('dbo.tblUsers','ShowSolutionTime') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblUsers]
	ADD [ShowSolutionTime] INT NOT NULL DEFAULT(0)
END
GO 

-- 2013-03-24 Aleksei Matveev
-- Add 'IsDefault' into 'tblUsers'
IF COL_LENGTH('dbo.tblUserWorkingGroup','IsDefault') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblUserWorkingGroup]
	ADD [IsDefault] INT NOT NULL DEFAULT(0)
END
GO 