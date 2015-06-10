-- update DB from 5.3.8.XX to 5.3.9.xx version

ALTER TABLE tblSettings ALTER COLUMN LDAPFilter nvarchar(100) NOT NULL


-- * * * * * * *  BEGIN  * * * * * * 
-- * we can delete this when deploying on acceptance/customer *
-- Temporary solution for "Source" field
IF COL_LENGTH('dbo.tblCase','source_id') IS NULL
BEGIN
	ALTER TABLE tblCase ADD source_id int not Null default 1;
END
go

-- we can delete this when deploying on acceptance/customer
IF COL_LENGTH('dbo.tblCase','source_id') IS not NULL
BEGIN
	DELETE from tblCaseFieldSettings where casefield = 'source'
	DECLARE @STR VARCHAR(100)
	SET @STR = (
		SELECT NAME
		FROM SYSOBJECTS SO
		JOIN SYSCONSTRAINTS SC ON SO.ID = SC.CONSTID
		WHERE OBJECT_NAME(SO.PARENT_OBJ) = 'tblCase'
			AND SO.XTYPE = 'D' AND SC.COLID = (SELECT COLID FROM SYSCOLUMNS
		WHERE ID = OBJECT_ID('tblCase')	AND NAME = 'source_id')
	)
	SET @STR = 'ALTER TABLE tblCase DROP CONSTRAINT ' + @STR EXEC (@STR)
	ALTER TABLE tblCase DROP column source_id
END
go
-- * * * * * * *  END * * *  * * * * * 

if not exists(select * from sysobjects WHERE Name = N'tblRegistrationSourceCustomer')
	begin
		CREATE TABLE [dbo].[tblRegistrationSourceCustomer](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[SystemCode] INT NOT NULL,
			[SourceName] [nvarchar](50) NOT NULL,
			[Customer_Id] INT NOT NULL,
			[IsDefault] INT NOT NULL,
			[IsActive] INT NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getdate()),
			[ChangedDate] [datetime] NOT NULL DEFAULT (getdate())
		) ON [PRIMARY]

		
		ALTER TABLE tblRegistrationSourceCustomer ADD
 			CONSTRAINT [PK_tblRegistrationSourceCustomer] PRIMARY KEY CLUSTERED 
			(
				[Id]
			) 

	end
GO

IF COL_LENGTH('dbo.tblCase','RegistrationSourceCustomer_Id') IS NULL
BEGIN
	ALTER TABLE tblCase ADD [RegistrationSourceCustomer_Id] int default NULL;
	ALTER TABLE [dbo].[tblCase]  WITH CHECK ADD  CONSTRAINT [FK_tblCase_tblRegistrationSourceCustomer] FOREIGN KEY([RegistrationSourceCustomer_Id])
	REFERENCES [dbo].[tblRegistrationSourceCustomer] ([Id])
		ON UPDATE CASCADE
		ON DELETE CASCADE	
END
go

IF COL_LENGTH('dbo.tblCaseFieldSettings','Locked') IS NULL
BEGIN
	ALTER TABLE tblCaseFieldSettings ADD Locked int not null
	CONSTRAINT DF_tblCaseFieldSettings_Locked DEFAULT 0
	WITH VALUES
END

GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.9'
