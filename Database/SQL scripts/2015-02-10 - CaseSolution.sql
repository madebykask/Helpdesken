

IF COL_LENGTH('dbo.tblCaseSolution','FormGUID') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD FormGUID uniqueidentifier NULL 
END