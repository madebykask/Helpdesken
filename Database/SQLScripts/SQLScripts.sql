-- update DB from 5.3.17 to 5.3.18 version

ALTER TABLE tblEMailLog ALTER COLUMN ResponseMessage nvarchar(1000)
GO



insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_ReportedBy', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_ReportedBy')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_User_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_User_Id')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_Name', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_Name')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_EMail', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_EMail')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_Phone', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_Phone')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Persons_CellPhone', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Persons_CellPhone')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Customer_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Customer_Id')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Region_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Region_Id')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Department_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Department_Id')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_OU_Id', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_OU_Id')


insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_CostCentre', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_CostCentre')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_Place', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_Place')

insert into tblCasefieldsettings 
(Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, EMailIdentifier, CreatedDate, ChangedDate, Locked)
Select Id, 'IsAbout_UserCode', 0, 0,0,0, '',NULL, 0, NULL, Getdate(), GetDate(), 0 from tblCustomer c
where not exists (select * from tblCasefieldsettings where  Customer_Id = c.Id and CaseField = 'IsAbout_UserCode')



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.18'

