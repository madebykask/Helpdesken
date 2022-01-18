--update DB from 5.3.53 to 5.3.54 version

RAISERROR ('Add Column WarrantyEndDate to tblComputer', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputer','WarrantyEndDate') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblComputer]
		ADD [WarrantyEndDate] datetime Null
	End

Go

if not exists(select * from tblReport WHERE Id=18)
	begin
		INSERT INTO tblReport(Id) VALUES(18)
	end
GO
  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.54'
GO