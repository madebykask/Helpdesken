-- update DB from 5.3.15 to 5.3.16 version

-- Change length on field tblCaseSolution
--Text_External
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Text_External' and sysobjects.name = N'tblCaseSolution')
            begin
                         declare @default sysname, @sql nvarchar(max)  
                         select @default = name  from sys.default_constraints  
                         where parent_object_id = object_id('tblCaseSolution') 
                                     AND type = 'D' 
                                     AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCaseSolution') and name = 'Text_External') 

                         set @sql = N'alter table tblCaseSolution drop constraint ' + @default 
                         exec sp_executesql @sql  
            end
go

ALTER TABLE tblCaseSolution ALTER COLUMN Text_External  nvarchar(max) 

ALTER TABLE tblCaseSolution ADD CONSTRAINT DF_Text_External DEFAULT '' FOR Text_External

ALTER TABLE tblCaseSolution ALTER COLUMN Text_External  nvarchar(max) not null


--Text_Internal
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Text_Internal' and sysobjects.name = N'tblCaseSolution')
            begin
                         declare @default sysname, @sql nvarchar(max)  
                         select @default = name  from sys.default_constraints  
                         where parent_object_id = object_id('tblCaseSolution') 
                                     AND type = 'D' 
                                     AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCaseSolution') and name = 'Text_Internal') 

                         set @sql = N'alter table tblCaseSolution drop constraint ' + @default 
                         exec sp_executesql @sql  
            end
go

ALTER TABLE tblCaseSolution ALTER COLUMN Text_Internal  nvarchar(max) 

ALTER TABLE tblCaseSolution ADD CONSTRAINT DF_Text_Internal DEFAULT '' FOR Text_Internal

ALTER TABLE tblCaseSolution ALTER COLUMN Text_Internal  nvarchar(max) not null


IF COL_LENGTH('dbo.tblEMailLog','SendTime') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblEMailLog] ADD [SendTime] DateTime	null
END
GO

IF COL_LENGTH('dbo.tblEMailLog','ResponseMessage') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblEMailLog] ADD [ResponseMessage] Nvarchar(400) null 
END
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.16'


