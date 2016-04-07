-- update DB from 5.3.21 to 5.3.22 version


IF COL_LENGTH('tblCaseSolution','Status') IS NULL
begin
    alter table tblCaseSolution 
	add [Status] int not null default(1)
end
GO

if exists(select * from sys.default_constraints where name like 'DF_tblCausingPart_CreatedDate')
Begin
  alter table tblCausingPart
  drop constraint DF_tblCausingPart_CreatedDate
end


ALTER TABLE tblCaseSolution ALTER COLUMN Text_External nvarchar(max) not null
GO

ALTER TABLE tblCaseSolution ALTER COLUMN Text_Internal nvarchar(max) not null
GO

ALTER TABLE tblLog ALTER COLUMN Text_External nvarchar(max) not null
GO

ALTER TABLE tblLog ALTER COLUMN Text_Internal nvarchar(max) not null
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.22'
