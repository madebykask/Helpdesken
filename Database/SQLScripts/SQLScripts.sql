--update DB from 5.3.39 to 5.3.40 version



-- Create index on tblCaseStatistics to tblCase
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'idx_casestatistics_case' AND object_id = OBJECT_ID('tblCaseStatistics'))
BEGIN
	CREATE NONCLUSTERED INDEX idx_casestatistics_case
	ON [dbo].[tblCaseStatistics] ([Case_Id])
	INCLUDE ([Id],[WasSolvedInTime])
END
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.40'
--ROLLBACK --TMP


