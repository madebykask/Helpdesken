-- update DB from 5.3.8.XX to 5.3.9.xx version

ALTER TABLE tblSettings ALTER COLUMN LDAPFilter nvarchar(100) NOT NULL

-- Temporary solution for "Source" field
IF COL_LENGTH('dbo.tblCase','source_id') IS NULL
BEGIN
	ALTER TABLE tblCase ADD source_id int not Null default 1;
END
go

set ansi_nulls off;
insert into tblCaseFieldSettings(customer_id, casefield, show, required) 
select t1.* from (
	select distinct customer_id, 'source' as source, '0' as show, '0' as required
	from tblCaseFieldSettings
) as t1
where t1.Customer_Id not in (
	select customer_id 
	from tblCaseFieldSettings
	where CaseField = 'source'
)
order by customer_id
set ansi_nulls on;

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.9'
