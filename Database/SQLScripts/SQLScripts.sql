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
    alter column POP3Password nvarchar (40) null
END

RAISERROR ('Extend RegUserId lenght to 200 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'RegUserId' and sysobjects.name = N'tblCase')
BEGIN
    ALTER TABLE tblCase	
    alter column RegUserId nvarchar (200) null
END

RAISERROR ('Extend Body of MailTemplate_Language lenght to 4000 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Body' and sysobjects.name = N'tblMailTemplate_tblLanguage')
BEGIN
    ALTER TABLE tblMailTemplate_tblLanguage	
    alter column Body nvarchar (4000) null
END

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.50'
GO

