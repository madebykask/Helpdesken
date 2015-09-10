-- update DB from 5.3.13 to 5.3.14 version


if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Modal' and sysobjects.name = N'tblform')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblForm') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblForm') and name = 'Modal')  

		set @sql = N'alter table tblform drop constraint ' + @default 
		exec sp_executesql @sql  
	end
go

if COL_LENGTH('dbo.tblform', 'ViewMode') IS NULL
begin
	alter table tblform
	alter column Modal int 

	EXEC sp_rename '[tblForm].[Modal]', 'ViewMode', 'COLUMN';

	ALTER TABLE tblForm ADD CONSTRAINT DF_tblForm_ViewMode DEFAULT 0 FOR ViewMode;
end
go



IF COL_LENGTH('dbo.tblcase','CostCentre') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcase]
	ADD CostCentre nvarchar(50) null
END
GO

IF COL_LENGTH('dbo.tblcaseHistory','CostCentre') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcaseHistory]
	ADD CostCentre nvarchar(50) null
END
GO

IF COL_LENGTH('dbo.tblcomputerusers','CostCentre') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcomputerusers]
	ADD CostCentre nvarchar(50) null
END
GO


DECLARE @CustomerId int

DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT DISTINCT Id 
FROM tblCustomer

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @CustomerId
WHILE @@FETCH_STATUS = 0
BEGIN 
    --Do something with Id here

	if not exists (select * from tblCaseFieldSettings where CaseField = 'CostCentre' and Customer_Id = @CustomerId)
	begin
		insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, Locked)
		values (@CustomerId, 'CostCentre', 0, 0,0,0, '', null, 0, 0)
	end
    FETCH NEXT FROM MY_CURSOR INTO @CustomerId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

-- Script for update old templates with new fields at case
insert into tblcasesolutionfieldsettings  
select casesolution_id, 53, 1, getdate(), getdate() 
from tblcasesolutionfieldsettings 
where casesolution_id 
	not in (select casesolution_id from tblcasesolutionfieldsettings
			where fieldname_id = 53)
group by casesolution_id
Go


DECLARE @CustomerId int

DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT DISTINCT Id 
FROM tblCustomer

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @CustomerId
WHILE @@FETCH_STATUS = 0
BEGIN 
    --Do something with Id here

	if not exists (select * from tblcomputeruserfieldsettings where ComputerUserField = 'CostCentre' and Customer_Id = @CustomerId)
	begin
		insert into tblcomputeruserfieldsettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
		values (@CustomerId, 'CostCentre', 0, 0, 0, 0, '')
	end
    FETCH NEXT FROM MY_CURSOR INTO @CustomerId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

-- http://helpdesk5.dhsolutions.se/Cases/Edit/53386
if COL_LENGTH('tblDepartment','SearchKey') != 400
BEGIN	
	alter table tblDepartment alter column SearchKey nvarchar(200);
END

-- http://helpdesk5.dhsolutions.se/Cases/Edit/53386
if COL_LENGTH('tblDepartment','Department') != 400
BEGIN	
	alter table tblDepartment alter column Department nvarchar(200) not null
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.13'
