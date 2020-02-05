--update DB from 5.3.44 to 5.3.46 version

RAISERROR ('Add ShowOnExtPageDepartmentCases to tblComputerUsers table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ShowOnExtPageDepartmentCases' and sysobjects.name = N'tblComputerUsers')
BEGIN
    ALTER TABLE tblComputerUsers
    ADD ShowOnExtPageDepartmentCases bit not null default 0 
END

RAISERROR('Creating report ''Report - Report Generator - Extended Case'' to customers', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM tblReport WHERE ID = 30)
BEGIN
	INSERT INTO tblReport(Id, ReportGUID) VALUES(30, 'D95FF512-78E9-48F9-B515-12F2FB00E786')
END

RAISERROR('Adding Report - Report Generator - Extended Case to customers', 10, 1) WITH NOWAIT
INSERT INTO tblReport_tblCustomer(Customer_Id, Report_Id, Show)
SELECT C.Id, 30, 0 FROM tblCustomer C
LEFT JOIN tblReport_tblCustomer RC ON RC.Customer_Id = C.Id AND RC.Report_Id = 30
WHERE RC.Report_Id IS NULL

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.46'
GO

