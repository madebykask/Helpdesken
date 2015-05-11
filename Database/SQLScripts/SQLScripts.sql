-- update DB from 5.3.5.XX to 5.3.6.xx version


IF COL_LENGTH('dbo.tblcasehistory','CaseFile') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcasehistory]
	ADD [CaseFile] nvarchar(50) NULL 
END
GO 

IF COL_LENGTH('dbo.tblcasehistory','LogFile') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcasehistory]
	ADD [LogFile] nvarchar(50) NULL 
END
GO 

IF COL_LENGTH('dbo.tblcasehistory','CaseLog') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblcasehistory]
	ADD [CaseLog] nvarchar(Max) NULL 
END
GO 

Delete from tbluserworkinggroup
where UserRole = 0 


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.6'
