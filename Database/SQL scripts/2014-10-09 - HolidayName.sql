IF COL_LENGTH('dbo.tblHoliday','HolidayName') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblHoliday]
	ADD [HolidayName] NVARCHAR(50) DEFAULT('') NOT NULL
END
GO