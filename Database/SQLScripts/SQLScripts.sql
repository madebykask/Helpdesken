-- 2013-03-23 Aleksei Matveev
-- Change 'DeliveryPeriod' column type to NVARCHAR(200)
IF COL_LENGTH('dbo.tblCaseInvoiceOrder','DeliveryPeriod') IS NOT NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseInvoiceOrder]
	ALTER COLUMN [DeliveryPeriod] NVARCHAR(200) NULL
END
GO 