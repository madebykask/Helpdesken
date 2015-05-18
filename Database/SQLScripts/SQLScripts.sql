-- update DB from 5.3.6.XX to 5.3.7.xx version

-- fix for old field names, we have discovered this bug on production 
-- 2015-04-28
UPDATE tblCaseSettings SET tblCaseName = Replace(tblCaseName, 'tblUsers.FirstName', 'Performer_User_Id')


IF COL_LENGTH('dbo.tblcasehistory','ClosingReason') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcasehistory]
	ADD [ClosingReason] nvarchar(50) NULL 
END
GO 


IF COL_LENGTH('dbo.tblUsers','TimeZoneId') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblUsers]
	ADD TimeZoneId nvarchar(64) default NULL
END
GO 

Delete from tbluserworkinggroup
where UserRole = 0 


ALTER TABLE tblProblem ALTER COLUMN ChangedByUser_Id int NULL
GO

ALTER TABLE tblChange ALTER COLUMN ChangedByUser_Id int NULL 
GO

ALTER TABLE tblDocumentCategory ALTER COLUMN CreatedByUser_Id int NULL 
GO

ALTER TABLE tblCase ALTER COLUMN User_Id int NULL 
GO

ALTER TABLE tblCaseHistory ALTER COLUMN User_Id int NULL 
GO

ALTER TABLE tblCase ALTER COLUMN ApprovedBy_User_Id int NULL 
GO

ALTER TABLE tblCaseHistory ALTER COLUMN ApprovedBy_User_Id int NULL 
GO

ALTER TABLE tblCase ALTER COLUMN Performer_User_Id int NULL 
GO

ALTER TABLE tblCaseHistory ALTER COLUMN Performer_User_Id int NULL 
GO

-- 2015-05-18
update tblUsers set timezoneid = 'W. Europe Standard Time' where timezoneid is null;
go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.7'
