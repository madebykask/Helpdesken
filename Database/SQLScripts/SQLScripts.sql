
--update DB from 5.3.33 to 5.3.34 version

--UPDATE tblCustomerUser field length
ALTER TABLE tblCustomerUser
ALTER COLUMN CaseDepartmentFilter nvarchar(100)

-- New field in tblCaseSolutionConditionProperties
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'TableFieldStatus' and sysobjects.name = N'tblCaseSolutionConditionProperties')
   ALTER TABLE tblCaseSolutionConditionProperties ADD TableFieldStatus nvarchar(100) NULL
GO	


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.34'