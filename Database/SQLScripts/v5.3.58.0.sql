--update DB from 5.3.57 to 5.3.58.0 version

--Altered these column to reflect the sizes in tblCase
--tblCaseIsAbout
RAISERROR ('Alter tblCaseIsAbout.ReportedBy from 40 nvarchar to 200 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseIsAbout','ReportedBy') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseIsAbout]
		ALTER COLUMN ReportedBy nvarchar(200) NULL
	End
Go

RAISERROR ('Alter tblCaseIsAbout.Person_Phone from 40 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseIsAbout','Person_Phone') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseIsAbout]
		ALTER COLUMN Person_Phone nvarchar(50) NULL
	End
Go

RAISERROR ('Alter tblCaseIsAbout.Person_CellPhone from 30 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseIsAbout','Person_CellPhone') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseIsAbout]
		ALTER COLUMN Person_CellPhone nvarchar(50) NULL
	End
Go

--tblCaseHistory 

RAISERROR ('Alter tblCaseHistory.IsAbout_ReportedBy from 40 nvarchar to 200 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseHistory','IsAbout_ReportedBy') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseHistory]
		ALTER COLUMN IsAbout_ReportedBy nvarchar(200) NULL
	End
Go

RAISERROR ('Alter tblCaseHistory.IsAbout_Persons_Phone from 40 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseHistory','IsAbout_Persons_Phone') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseHistory]
		ALTER COLUMN IsAbout_Persons_Phone nvarchar(50) NULL
	End
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.58.0'
GO