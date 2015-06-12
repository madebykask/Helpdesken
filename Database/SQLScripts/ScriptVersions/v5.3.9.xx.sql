-- update DB from 5.3.8.XX to 5.3.9.xx version

ALTER TABLE tblSettings ALTER COLUMN LDAPFilter nvarchar(100) NOT NULL


if not exists(select * from sysobjects WHERE Name = N'tblRegistrationSourceCustomer')
	begin
		CREATE TABLE [dbo].[tblRegistrationSourceCustomer](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[SystemCode] INT NULL,
			[SourceName] [nvarchar](50) NOT NULL,
			[Customer_Id] INT NOT NULL,
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
