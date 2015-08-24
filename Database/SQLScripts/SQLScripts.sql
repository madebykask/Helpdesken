-- update DB from 5.3.11 to 5.3.13 version


if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Modal' and sysobjects.name = N'tblform')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblForm') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblForm') and name = 'Modal')  

		set @sql = N'alter table tblform drop constraint ' + @default 
		exec sp_executesql @sql  
	end
go

if COL_LENGTH('dbo.tblform', 'ViewMode') IS NULL
begin
	alter table tblform
	alter column Modal int 

	EXEC sp_rename '[tblForm].[Modal]', 'ViewMode', 'COLUMN';

	ALTER TABLE tblForm ADD CONSTRAINT DF_tblForm_ViewMode DEFAULT 0 FOR ViewMode;
end
go


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.13'
