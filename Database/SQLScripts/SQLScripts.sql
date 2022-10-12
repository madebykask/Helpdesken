--update DB from 5.3.56 to 5.3.57 version
RAISERROR ('Add Column SiteURL to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SiteURL') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblSettings
		ADD SiteURL nvarchar(100) Null
	End
Go

RAISERROR ('Add Column SelfServiceURL to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SelfServiceURL') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblSettings
		ADD SelfServiceURL nvarchar(100) Null
	End

Go

RAISERROR ('Add Column GDPRType to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','GDPRType') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD GDPRType int not Null default 0
	End

Go

RAISERROR ('Add Column CaseTypes to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','CaseTypes') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD CaseTypes nvarchar(256) Null
	End

Go
  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.57'
GO