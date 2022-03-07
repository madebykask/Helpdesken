--update DB from 5.3.54 to 5.3.55 version


RAISERROR ('Add Column Category to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseFilterFavorite','Category') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblCaseFilterFavorite
		ADD Category nvarchar(200) Null
	End

Go

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.55'
GO