-- update DB from 5.3.19 to 5.3.20 version

 IF COL_LENGTH('dbo.tblCaseInvoiceOrder','DeliveryPeriod') IS NOT NULL
 BEGIN
	 ALTER TABLE tblcaseinvoiceorder
	DROP COLUMN DeliveryPeriod
 END

 IF COL_LENGTH('dbo.tblCaseInvoiceOrder','Reference') IS NOT NULL
 BEGIN
	 ALTER TABLE tblcaseinvoiceorder
	DROP COLUMN Reference
 END

IF COL_LENGTH('dbo.tblCaseInvoiceOrder','CreditForOrder_Id') IS NULL
BEGIN
	ALTER TABLE tblCaseInvoiceOrder ADD CreditForOrder_Id int null 
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.20'

