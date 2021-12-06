--update DB from 5.3.51 to 5.3.52 version

--New table for Extended Case Form Templates
RAISERROR ('Create table ExtendedCaseFormTemplates', 10, 1) WITH NOWAIT
IF(OBJECT_ID('ExtendedCaseFormTemplates', 'U') IS NULL)
Begin	
	CREATE TABLE [dbo].[ExtendedCaseFormTemplates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Customer_Id]  [int] NOT NULL FOREIGN KEY REFERENCES tblUsers(Id),
	[MetaData] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[Guid] [uniqueidentifier] NULL,
	[Status] [int] NOT NULL,
	[Version] [int] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[ExtendedCaseFormTemplates] ADD  DEFAULT ((1)) FOR [Status]
ALTER TABLE [dbo].[ExtendedCaseFormTemplates] ADD  DEFAULT ((0)) FOR [Version]
END
GO

IF COL_LENGTH('tblSettings', 'BlockedEmailRecipients') IS NULL
	BEGIN
		ALTER TABLE tblSettings ADD BlockedEmailRecipients nvarchar(4000) NULL
	END
GO

RAISERROR ('Add Column Copy to tblComputerFieldSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputerFieldSettings','Copy') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblComputerFieldSettings]
	ADD [Copy] int Not Null default(1)
End
Go

RAISERROR ('Updating Copy in tblComputerFieldSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputerFieldSettings','Copy') IS NOT NULL
BEGIN
	UPDATE [tblComputerFieldSettings]
    SET [Copy] = 0
	WHERE ComputerField = 'Status' 
	OR ComputerField = 'Stolen'
	OR ComputerField = 'ReplacedWithComputerName'
	OR ComputerField = 'Sendback'
	OR ComputerField = 'ScrapDate'
	OR ComputerField = 'CreatedDate'
	OR ComputerField = 'ChangedDate'
	OR ComputerField = 'SyncChangedDate'
	OR ComputerField = 'ScanDate'
	OR ComputerField = 'LDAPPath'
END
GO

RAISERROR ('Add Column Customer_Id to tblOperatingSystem', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblOperatingSystem','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblOperatingSystem]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblOperatingSystem] WITH NOCHECK ADD CONSTRAINT [FK_tblOperatingSystem_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
End
Go

RAISERROR ('Add Column CreatedByEditor to ExtendedCaseForms', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.ExtendedCaseForms','CreatedByEditor') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[ExtendedCaseForms]
	ADD [CreatedByEditor] BIT NULL 

End
Go

RAISERROR ('Add Column Customer_Id to tblProcessor', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblProcessor','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblProcessor]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblProcessor] WITH NOCHECK ADD CONSTRAINT [FK_tblProcessor_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblNIC', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblNIC','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblNIC]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblNIC] WITH NOCHECK ADD CONSTRAINT [FK_tblNIC_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblComputerModel', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputerModel','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblComputerModel]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblComputerModel] WITH NOCHECK ADD CONSTRAINT [FK_tblComputerModel_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to tblRAM', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblRAM','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblRAM]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[tblRAM] WITH NOCHECK ADD CONSTRAINT [FK_tblRAM_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('Add Column Customer_Id to ExtendedCaseForms', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.ExtendedCaseForms','Customer_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[ExtendedCaseForms]
	ADD [Customer_Id] INT NULL

	ALTER TABLE [dbo].[ExtendedCaseForms] WITH NOCHECK ADD CONSTRAINT [FK_ExtendedCaseForms_tblCustomer]
	FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])

End
Go

RAISERROR ('ADD Building and floor settings', 10, 1) WITH NOWAIT
begin transaction

DECLARE @tempTable TABLE ([Id] int
						,[Customer_Id] int
						,[ComputerField] nvarchar(50)
						,[Show] int
						,[ShowExternal] int
						,[Label] nvarchar(50)
						,[Label_ENG] nvarchar(50)
						,[Required] int
						,[FieldHelp] nvarchar(200)
						,[ShowInList] int
						,[ShowInListExternal] int
						,[XMLElement] nvarchar(100)
						,[ReadOnly] int
						,[SortOrder] int
						,[ComputerFieldGroup_Id] int 
						,[CopyField] int
						,[CreatedDate] datetime
						,[ChangedDate] datetime
						,[Copy] int);    

INSERT @tempTable ([Id]
				,[Customer_Id]
				,[ComputerField]
				,[Show]
				,[ShowExternal]
				,[Label]
				,[Label_ENG]
				,[Required]
				,[FieldHelp]
				,[ShowInList]
				,[ShowInListExternal]
				,[XMLElement]
				,[ReadOnly]
				,[SortOrder]
				,[ComputerFieldGroup_Id]
				,[CopyField]
				,[CreatedDate]
				,[ChangedDate]
				,[Copy]) 
SELECT DISTINCT [Id]
				,[Customer_Id]
				,[ComputerField]
				,[Show]
				,[ShowExternal]
				,[Label]
				,[Label_ENG]
				,[Required]
				,[FieldHelp]
				,[ShowInList]
				,[ShowInListExternal]
				,[XMLElement]
				,[ReadOnly]
				,[SortOrder]
				,[ComputerFieldGroup_Id]
				,[CopyField]
				,[CreatedDate]
				,[ChangedDate]
				,[Copy]
FROM [dbo].tblComputerFieldSettings 
WHERE ComputerField = 'Room_Id' 

DECLARE @currentId INT
WHILE(1 = 1)
    BEGIN
		SET @currentId = NULL
		SELECT TOP 1 @currentId = [id] FROM @tempTable
		IF @currentId IS NULL
			BREAK
		IF NOT EXISTS(select * from [dbo].tblComputerFieldSettings 
				where [ComputerField] = 'Building' and [Customer_Id] IN (SELECT [Customer_Id] from [dbo].tblComputerFieldSettings where [Id] = @currentId)) 
			INSERT INTO [dbo].tblComputerFieldSettings ([Customer_Id]
					,[ComputerField]
					,[Show]
					,[ShowExternal]
					,[Label]
					,[Label_ENG]
					,[Required]
					,[FieldHelp]
					,[ShowInList]
					,[ShowInListExternal]
					,[XMLElement]
					,[ReadOnly]
					,[SortOrder]
					,[ComputerFieldGroup_Id]
					,[CopyField]
					,[CreatedDate]
					,[ChangedDate]
					,[Copy])
				SELECT [Customer_Id]
					,N'Building'
					,[Show]
					,[ShowExternal]
					,N'Building'
					,N'Building'
					,[Required]
					,N''
					,0
					,0
					,NULL
					,[ReadOnly]
					,[SortOrder]
					,NULL
					,[CopyField]
					,CURRENT_TIMESTAMP
					,CURRENT_TIMESTAMP
					,[Copy] 
				FROM @tempTable 
				WHERE [Id] = @currentId

		IF NOT EXISTS(select * from [dbo].tblComputerFieldSettings 
				where [ComputerField] = 'Floor' and [Customer_Id] IN (SELECT [Customer_Id] from [dbo].tblComputerFieldSettings where [Id] = @currentId)) 
			INSERT INTO [dbo].tblComputerFieldSettings ([Customer_Id]
					,[ComputerField]
					,[Show]
					,[ShowExternal]
					,[Label]
					,[Label_ENG]
					,[Required]
					,[FieldHelp]
					,[ShowInList]
					,[ShowInListExternal]
					,[XMLElement]
					,[ReadOnly]
					,[SortOrder]
					,[ComputerFieldGroup_Id]
					,[CopyField]
					,[CreatedDate]
					,[ChangedDate]
					,[Copy])
				SELECT [Customer_Id]
					,N'Floor'
					,[Show]
					,[ShowExternal]
					,N'Floor'
					,N'Floor'
					,[Required]
					,N''
					,0
					,0
					,NULL
					,[ReadOnly]
					,[SortOrder]
					,NULL
					,[CopyField]
					,CURRENT_TIMESTAMP
					,CURRENT_TIMESTAMP
					,[Copy] 
				FROM @tempTable 
				WHERE [Id] = @currentId

        DELETE TOP(1) FROM @tempTable
    END

--SELECT *FROM [dbo].tblComputerFieldSettings WHERE ComputerField = 'Room_Id'
--SELECT *FROM [dbo].tblComputerFieldSettings WHERE ComputerField = 'Building'
--SELECT *FROM [dbo].tblComputerFieldSettings WHERE ComputerField = 'Floor'

commit transaction

RAISERROR ('Add Section.sektion Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Section.sektion')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Section.sektion'
           ,'Sektion')
		   END
GO

RAISERROR ('Add Section.sektion English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Section.sektion')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Section.sektion'
           ,'Section')
		   END
GO


RAISERROR ('Add Section.sektion English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Section.sektion')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Section.sektion'
           ,'Section')
		   END
GO

RAISERROR ('Add Control.textfalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.textfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.textfalt'
           ,'Textfält')
		   END
GO

RAISERROR ('Add Control.textfalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.textfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.textfalt'
           ,'Text Field')
		   END
GO

RAISERROR ('Add Control.textarea Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.textarea')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.textarea'
           ,'Textarea')
		   END
GO

RAISERROR ('Add Control.textarea English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.textarea')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.textarea'
           ,'Text Area')
		   END
GO

RAISERROR ('Add Control.datumfalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.datumfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.datumfalt'
           ,'Datumfält')
		   END
GO

RAISERROR ('Add Control.datumfalt English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.datumfalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.datumfalt'
           ,'Date Field')
		   END
GO

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.52'
GO


