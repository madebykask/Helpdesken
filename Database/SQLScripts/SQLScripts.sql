--update DB from 5.3.49 to 5.3.50 version


RAISERROR ('Extend Pop3Password lenght to 40 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'POP3Password' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings	
    alter column POP3Password nvarchar (40) not null
END


RAISERROR ('Extend Pop3Password lenght to 40 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'POP3Password' and sysobjects.name = N'tblWorkingGroup')
BEGIN
    ALTER TABLE tblWorkingGroup	
    alter column POP3Password nvarchar (40) not null
END

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.50'
GO

