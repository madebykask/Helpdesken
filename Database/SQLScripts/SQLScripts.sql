-- update DB from 5.3.21 to 5.3.22 version


IF COL_LENGTH('tblCaseSolution','Status') IS NULL
begin
    alter table tblCaseSolution 
	add [Status] int not null default(1)
end
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.22'
