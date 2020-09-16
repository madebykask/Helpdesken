--update DB from 5.3.48 to 5.3.49 version

RAISERROR('Increase NovelClient column lenght in tblComputer', 10, 1) WITH NOWAIT
ALTER TABLE dbo.tblComputer
  ALTER COLUMN NovellClient NVARCHAR(100) NOT NULL;

RAISERROR ('Add ServerFileName to tblServer', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ServerFileName' and sysobjects.name = N'tblServer')
BEGIN
    ALTER TABLE tblServer
    ADD ServerFileName nvarchar(100) 
END

RAISERROR ('Add ServerDocument to tblServer', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ServerDocument' and sysobjects.name = N'tblServer')
BEGIN
    ALTER TABLE tblServer
    ADD ServerDocument image   
END

--Begin tran
DECLARE @LoopVar int

SET @LoopVar = (SELECT MIN(Id) FROM dbo.tblCustomer)
WHILE @LoopVar is not null
BEGIN
  -- Do Stuff with current value of @LoopVar
  If exists (select ServerField from dbo.tblServerFieldSettings where Customer_Id = @LoopVar and ServerField = 'ServerName' )    
  begin
	  If not exists (select ServerField from dbo.tblServerFieldSettings where Customer_Id = @LoopVar and ServerField = 'ServerFileName' )
		  begin
		  Insert into dbo.tblServerFieldSettings (Customer_Id, ServerField, Show, [Label], Label_ENG, [Required], ShowInList, CreatedDate, ChangedDate )
		  values (@LoopVar, 'ServerFileName', 0, 'Dokument', 'Document' ,0, 0, GETDATE(), GETDATE())
		  end
  end
  --Ok, done, now get the next value
  SET @LoopVar = (SELECT MIN(Id) FROM dbo.tblCustomer
    WHERE @LoopVar < Id)
END
--ROLLBACK 

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.49'
GO

