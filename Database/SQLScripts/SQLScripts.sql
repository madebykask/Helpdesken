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

 -- New field in tblProductArea
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnExternalPage' and sysobjects.name = N'tblProductArea')
	ALTER TABLE tblProductArea ADD ShowOnExternalPage int Default(1) NOT NULL
GO

-- DocumentPermission for Systemadministrators
UPDATE tblUsers Set DocumentPermission = 1 WHERE UserGroup_Id = 4;
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.21'
