-- update DB from 5.3.20 to 5.3.21 version

 IF COL_LENGTH('tblCaseInvoiceOrder','Project_Id') IS NULL
 BEGIN
	alter table tblCaseInvoiceOrder
	add Project_Id int null
 end
 GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblCaseInvoiceOrder_tblProject]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblCaseInvoiceOrder]'))
begin
	ALTER TABLE [dbo].[tblCaseInvoiceOrder]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseInvoiceOrder_tblProject] FOREIGN KEY([Project_Id])
	REFERENCES [dbo].[tblProject] ([Id])
end
ALTER TABLE tblCase ALTER COLUMN Persons_Phone nvarchar(50)
ALTER TABLE tblCase ALTER COLUMN Persons_CellPhone nvarchar(50)

ALTER TABLE tblCaseHistory ALTER COLUMN Persons_Phone nvarchar(50)
ALTER TABLE tblCaseHistory ALTER COLUMN Persons_CellPhone nvarchar(50)

GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.21'
