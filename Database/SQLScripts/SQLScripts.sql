--update DB from 5.3.44 to 5.3.46 version



RAISERROR ('Add Status to tblCustomer table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Status' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD [Status] Int not null default 1 
END


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.46'
GO

