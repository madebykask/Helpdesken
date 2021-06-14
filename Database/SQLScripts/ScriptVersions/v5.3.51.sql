--update DB from 5.3.50 to 5.3.51 version
RAISERROR ('Add Column AvailableTabsSelfsevice to tblCaseSolution', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseSolution','AvailableTabsSelfsevice') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [AvailableTabsSelfsevice] nvarchar(100) Not Null default('')
End
Go

RAISERROR ('Add Column ActiveTabSelfservice to tblCaseSolution', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseSolution','ActiveTabSelfservice') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCaseSolution]
	ADD [ActiveTabSelfservice] nvarchar(100) Not Null default('')
End
Go

RAISERROR ('Adding Unique Constraint to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF OBJECT_ID('dbo.[UQ_LanguageId_Property]') IS NULL 
Begin
ALTER TABLE ExtendedCaseTranslations   
ADD CONSTRAINT UQ_LanguageId_Property UNIQUE (LanguageId, Property); 
end

RAISERROR ('Extend RegUserId lenght to 200 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'RegUserId' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory	
    alter column RegUserId nvarchar (200) null
END

--New table for User rights on ContractCategory
RAISERROR ('Create table tblUsers_tblContractCategory', 10, 1) WITH NOWAIT
if not exists(select * from sysobjects WHERE Name = N'tblUsers_tblContractCategory')
Begin	
	CREATE TABLE [tblUsers_tblContractCategory](
		[User_Id] [int] NOT NULL,
		[ContractCategory_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblUsers_tblContractCategory] PRIMARY KEY CLUSTERED 
	(
		[User_Id] ASC,
		[ContractCategory_Id] ASC
		)
		WITH (
			PAD_INDEX = OFF, 
			STATISTICS_NORECOMPUTE = OFF, 
			IGNORE_DUP_KEY = OFF, 
			ALLOW_ROW_LOCKS = ON, 
			ALLOW_PAGE_LOCKS = ON, 
			FILLFACTOR = 90) ON [PRIMARY]
	)
	ON [PRIMARY]


	ALTER TABLE [tblUsers_tblContractCategory]  ADD CONSTRAINT [FK_tblUsers_tblContractCategory_tblContractCategory] FOREIGN KEY([ContractCategory_Id])
	REFERENCES [tblContractCategory] ([Id])

	ALTER TABLE [tblUsers_tblContractCategory] CHECK CONSTRAINT [FK_tblUsers_tblContractCategory_tblContractCategory]

	ALTER TABLE [tblUsers_tblContractCategory]  ADD  CONSTRAINT [FK_tblUsers_tblContractCategory_tblUsers] FOREIGN KEY([User_Id])
	REFERENCES [tblUsers] ([Id])

	ALTER TABLE [tblUsers_tblContractCategory] CHECK CONSTRAINT [FK_tblUsers_tblContractCategory_tblUsers]

end
GO

RAISERROR ('Extend URL1 lenght to 2000 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'URL1' and sysobjects.name = N'tblFAQ')
BEGIN
    ALTER TABLE tblFAQ	
    alter column URL1 nvarchar (2000) null
END
GO

RAISERROR ('Extend URL2 lenght to 2000 char', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'URL2' and sysobjects.name = N'tblFAQ')
BEGIN
    ALTER TABLE tblFAQ	
    alter column URL2 nvarchar (2000) null
END
GO

RAISERROR ('Update Showinlist, readonly and required in tblComputerFieldSettings for documents', 10, 1) WITH NOWAIT
	UPDATE [tblComputerFieldSettings]
    SET ShowInList = 0, 
    ReadOnly = 0, 
    Required = 0 
	WHERE ComputerField = 'Dokument' OR ComputerField = 'Document'
GO

RAISERROR ('Add Column Price to tblComputerType', 10, 1) WITH NOWAIT
	IF COL_LENGTH('dbo.tblComputerType','Price') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblComputerType]
		ADD [Price] INT NOT NULL default(0)
	End
Go

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.51'
GO
