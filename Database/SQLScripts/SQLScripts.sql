--update DB from 5.3.39 to 5.3.40 version


-- Create index on tblCaseStatistics to tblCase
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'idx_casestatistics_case' AND object_id = OBJECT_ID('tblCaseStatistics'))
BEGIN
	CREATE NONCLUSTERED INDEX idx_casestatistics_case
	ON [dbo].[tblCaseStatistics] ([Case_Id])
	INCLUDE ([Id],[WasSolvedInTime])
END

-- add tblCustomer.ShowCaseActionsPanelOnTop
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
			where syscolumns.name = N'ShowCaseActionsPanelOnTop' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD ShowCaseActionsPanelOnTop bit not null DEFAULT(1)
END

--add  tblCustomer.ShowCaseActionsPanelAtBottom
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
			where syscolumns.name = N'ShowCaseActionsPanelAtBottom' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD ShowCaseActionsPanelAtBottom bit not null DEFAULT(0)
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.40'
--ROLLBACK --TMP


