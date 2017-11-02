-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProductArea_Id' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD ProductArea_Id int NULL 

		ALTER TABLE [dbo].[tblForm] ADD 
			CONSTRAINT [FK_tblForm_tblProductArea] FOREIGN KEY 
			(
				[ProductArea_Id]
			) REFERENCES [dbo].[tblProductArea] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblQuestionnaireCircularPart
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SendDate' and sysobjects.name = N'tblQuestionnaireCircularPart')
	ALTER TABLE tblQuestionnaireCircularPart ADD SendDate datetime NULL
GO

-- Nytt fält i tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountType' and sysobjects.name = N'tblUsers')
	ALTER TABLE tblUsers ADD AccountType int NOT NULL Default(1)
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseType_Id' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD CaseType_Id int NULL 

		ALTER TABLE [dbo].[tblForm] ADD 
			CONSTRAINT [FK_tblForm_tblCaseType] FOREIGN KEY 
			(
				[CaseType_Id]
			) REFERENCES [dbo].[tblCaseType] (
				[Id]
			)	
	end
GO

-- Ny tabell
if not exists(select * from sysobjects WHERE Name = N'tblFormFieldValue')
	begin
		CREATE TABLE [tblFormFieldValue](
			[Case_Id] [int] NOT NULL,
			[FormField_Id] [int] NOT NULL,
			[FormFieldValue] [nvarchar](2000) NOT NULL) ON [PRIMARY]


		ALTER TABLE [tblFormFieldValue] ADD
 			CONSTRAINT [PK_tblFormFieldValue] PRIMARY KEY CLUSTERED 
			(
				[Case_Id] ASC,
				[FormField_Id] ASC
			) 
		ALTER TABLE [tblFormFieldValue] ADD  
			CONSTRAINT [FK_tblFormFieldValue_tblCase] 
				FOREIGN KEY([Case_Id]) REFERENCES [tblCase] ([Id])


		ALTER TABLE [tblFormFieldValue] ADD  
			CONSTRAINT [FK_tblFormFieldValue_tblFormField] 
				FOREIGN KEY([FormField_Id]) REFERENCES [tblFormField] ([Id])
	end
GO

If not exists (select * from tblText where Id = 65)
	insert into tblText (Id, Textstring) VALUES (65, 'Svarstid')
GO

If not exists (select * from tblTextTranslation where Text_Id = 65 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(65, 2, 'Response time')
GO

If not exists (select * from tblText where Id = 88)
	insert into tblText (Id, Textstring) VALUES (88, 'Tilldelning handläggare')
GO

If not exists (select * from tblTextTranslation where Text_Id = 88 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(88, 2, 'Assignment Administrator')
GO

If not exists (select * from tblText where Id = 104)
	insert into tblText (Id, Textstring) VALUES (104, 'Medeltid')
GO

If not exists (select * from tblTextTranslation where Text_Id = 104 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(104, 2, 'Average Time')
GO

-- Referens från ärende till produktområde
if not exists(select * from sysobjects WHERE Name = N'FK_tblCase_tblProductArea')
	ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblProductArea] FOREIGN KEY 
			(
				[ProductArea_Id]
			) REFERENCES [dbo].[tblProductArea] (
				[Id]
			)
GO

-- Nytt fält i tblCase
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LeadTime' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE tblCase ADD LeadTime int NOT NULL Default(0) 

		
	end
GO

-- Nytt fält i tblCaseHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LeadTime' and sysobjects.name = N'tblCaseHistory')
	begin
		ALTER TABLE tblCaseHistory ADD LeadTime int NOT NULL Default(0) 
	end
GO


-- Nytt fält i tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WatchDateMail' and sysobjects.name = N'tblUsers')
	begin
		ALTER TABLE tblUsers ADD WatchDateMail int NOT NULL Default(1) 
	end
GO

UPDATE tblText Set TextString = 'Skicka mail när bevakningsdatum inträffar' WHERE Id=1047;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Send mail when Watch Date occurs' WHERE Text_Id=1047 AND Language_Id=2;
GO

-- Nytt fält i tblGlobalSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LoginOption' and sysobjects.name = N'tblGlobalSettings')
	begin
		ALTER TABLE tblGlobalSettings ADD LoginOption int NOT NULL Default(0) 
	end
GO

-- Nytt fält i tblMailTemplate
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MailTemplateGUID' and sysobjects.name = N'tblMailTemplate')
	begin
		ALTER TABLE tblMailTemplate ADD MailTemplateGUID uniqueidentifier NOT NULL Default(newid()) 
	end
GO


-- Ny tabell
if not exists(select * from sysobjects WHERE Name = N'tblMailTemplate_tblLanguage')
	begin
		CREATE TABLE [dbo].[tblMailTemplate_tblLanguage](
			[MailTemplate_Id] [int] NOT NULL,
			[Language_Id] [int] NOT NULL,
			[MailTemplateName] [nvarchar](50) NULL,
			[Subject] [nvarchar](200) NOT NULL,
			[Body] [nvarchar](2000) NOT NULL,
			[MailFooter] [nvarchar](500) NULL) 

		ALTER TABLE [tblMailTemplate_tblLanguage] ADD
 			CONSTRAINT [PK_tblMailTemplate_tblLanguage] PRIMARY KEY CLUSTERED 
			(
				[MailTemplate_Id] ASC,
				[Language_Id] ASC
			) ON [PRIMARY]

		ALTER TABLE [dbo].[tblMailTemplate_tblLanguage] 
			ADD  CONSTRAINT [FK_tblMailTemplate_tblLanguage_tblLanguage] FOREIGN KEY([Language_Id])
				REFERENCES [dbo].[tblLanguage] ([Id])

		ALTER TABLE [dbo].[tblMailTemplate_tblLanguage] 
			ADD  CONSTRAINT [FK_tblMailTemplate_tblLanguage_tblMailTemplate] FOREIGN KEY([MailTemplate_Id])
				REFERENCES [dbo].[tblMailTemplate] ([Id])

	end
GO

-- Läs över värden
if not exists(select *  from tblMailTemplate_tblLanguage)
	begin
		exec ('INSERT INTO tblMailTemplate_tblLanguage (MailTemplate_Id, Language_Id, MailTemplateName, Subject, Body) SELECT Id, Language_Id, MailTemplateName, Subject, Body FROM tblMailTemplate WHERE Language_Id=1')
	end 

GO

if not exists(select *  from tblMailTemplate_tblLanguage WHERE Language_Id=2)
	begin
		exec('INSERT INTO tblMailTemplate_tblLanguage (MailTemplate_Id, Language_Id, MailTemplateName, Subject, Body)
		SELECT     tblMailTemplate_1.Id, 2, tblmailtemplate.MailtemplateName, dbo.tblMailTemplate.Subject, dbo.tblMailTemplate.Body
		FROM         dbo.tblMailTemplate INNER JOIN
                      dbo.tblMailTemplate AS tblMailTemplate_1 ON dbo.tblMailTemplate.MailID = tblMailTemplate_1.MailID AND tblMailTemplate_1.Language_Id = 1 and tblMailTemplate_1.Customer_id= tblMailTemplate.customer_id
		WHERE     (dbo.tblMailTemplate.Language_Id = 2)')
	end 
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MailTemplateName' and sysobjects.name = N'tblMailTemplate')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblMailTemplate') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblMailTemplate') and name = 'MailTemplateName') 

		set @sql = N'alter table tblMailTemplate drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblMailTemplate 
		drop column MailTemplateName
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Subject' and sysobjects.name = N'tblMailTemplate')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblMailTemplate') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblMailTemplate') and name = 'Subject') 

		set @sql = N'alter table tblMailTemplate drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblMailTemplate 
		drop column Subject
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Body' and sysobjects.name = N'tblMailTemplate')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblMailTemplate') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblMailTemplate') and name = 'Body') 

		set @sql = N'alter table tblMailTemplate drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblMailTemplate 
		drop column Body
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MailFooter' and sysobjects.name = N'tblMailTemplate')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblMailTemplate') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblMailTemplate') and name = 'MailFooter') 

		set @sql = N'alter table tblMailTemplate drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblMailTemplate 
		drop column MailFooter
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Language_Id' and sysobjects.name = N'tblMailTemplate')
	begin
		ALTER TABLE tblMailTemplate DROP CONSTRAINT FK_tblMailTemplate_tblLanguage
	end
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Language_Id' and sysobjects.name = N'tblMailTemplate')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblMailTemplate') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblMailTemplate') and name = 'Language_Id') 

		set @sql = N'alter table tblMailTemplate drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblMailTemplate 
		drop column Language_Id
	end
go

UPDATE tblText Set TextString = 'som används går inte att ta bort' WHERE Id=426;
GO

UPDATE tblTextTranslation Set TextTranslation = 'used can not be removed' WHERE Text_Id=426 AND Language_Id=2;
GO

-- Ändra storlek på fält
ALTER TABLE tblCase ALTER COLUMN UserCode nvarchar(20)
GO

ALTER TABLE tblCaseHistory ALTER COLUMN UserCode nvarchar(20) 
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OpenCase_StateSecondary_Id' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD OpenCase_StateSecondary_Id int NULL
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangeCouncil' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD ChangeCouncil int NOT NULL Default (0)
	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblChangeCouncil')
	begin
		CREATE TABLE [dbo].[tblChangeCouncil](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[ChangeCouncilGUID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
			[Change_Id] [int] NOT NULL,
			[ChangeCouncilName] [nvarchar](100) NOT NULL,
			[ChangeCouncilEmail] [nvarchar](50) NULL,
			[ChangeCouncilStatus] [int] NOT NULL DEFAULT ((0)),
			[ChangeCouncilNote] [ntext] NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getutcdate()),
			[ChangedDate] [datetime] NOT NULL DEFAULT (getutcdate()))


		ALTER TABLE [tblChangeCouncil] ADD
 			CONSTRAINT [PK_tblChangeCouncil] PRIMARY KEY CLUSTERED ([Id] ASC)


		ALTER TABLE [dbo].[tblChangeCouncil]  WITH CHECK ADD  
			CONSTRAINT [FK_tblChangeCouncil_tblChange] FOREIGN KEY([Change_Id])
				REFERENCES [dbo].[tblChange] ([Id])


	end 
GO

if not exists(select * from sysobjects WHERE Name = N'tblChangeContact')
	begin
		CREATE TABLE [dbo].[tblChangeContact](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Change_Id] [int] NOT NULL,
			[ContactName] [nvarchar](50) NULL,
			[ContactPhone] [nvarchar](50) NULL,
			[ContactEMail] [nvarchar](50) NULL,
			[ContactCompany] [nvarchar](50) NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getutcdate()),
			[ChangedDate] [datetime] NOT NULL DEFAULT (getutcdate())) ON [PRIMARY]

		ALTER TABLE [tblChangeContact] ADD
 			CONSTRAINT [PK_tblChangeContact] PRIMARY KEY CLUSTERED ([Id] ASC)

		ALTER TABLE [dbo].[tblChangeContact] 
			ADD CONSTRAINT [FK_tblChangeContact_tblChange] FOREIGN KEY([Change_Id])
			REFERENCES [dbo].[tblChange] ([Id])

	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblChecklists')
	begin
		CREATE TABLE [dbo].[tblChecklists](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[ChecklistName] [nvarchar](50) NOT NULL,
			[Customer_Id] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getutcdate()),
			[ChangedDate] [datetime] NOT NULL DEFAULT (getutcdate()))

		ALTER TABLE [tblChecklists] ADD
 			CONSTRAINT [PK_tblChecklists] PRIMARY KEY CLUSTERED ([Id] ASC)


		ALTER TABLE [dbo].[tblChecklists] 
			ADD  CONSTRAINT [FK_tblChecklists_tblCustomer] FOREIGN KEY([Customer_Id])
				REFERENCES [dbo].[tblCustomer] ([Id])

	end
GO

-- Nytt fält i tblCheckListService
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Checklists_Id' and sysobjects.name = N'tblCheckListService')
	begin
		ALTER TABLE tblCheckListService ADD Checklists_Id int NULL 

		ALTER TABLE [dbo].[tblCheckListService] ADD 
			CONSTRAINT [FK_tblCheckListService_tblChecklists] FOREIGN KEY 
			(
				[Checklists_Id]
			) REFERENCES [dbo].[tblChecklists] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblCheckList
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Checklists_Id' and sysobjects.name = N'tblCheckList')
	begin
		ALTER TABLE tblCheckList ADD Checklists_Id int NULL 

		ALTER TABLE [dbo].[tblCheckList] ADD 
			CONSTRAINT [FK_tblCheckList_tblChecklists] FOREIGN KEY 
			(
				[Checklists_Id]
			) REFERENCES [dbo].[tblChecklists] (
				[Id]
			)	
	end
GO

UPDATE tblText Set TextString = 'Checklistor' WHERE Id=496;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Checklists' WHERE Text_Id=496 AND Language_Id=2;
GO

if not exists(select * from sysobjects WHERE Name = N'tblChecklistSchedule')
	begin
		CREATE TABLE [dbo].[tblChecklistSchedule](
			[Checklists_Id] [int] NOT NULL,
			[ScheduleType] [int] NOT NULL DEFAULT (1),
			[ScheduleTime] [numeric](9, 3) NOT NULL DEFAULT (0),
			[ScheduleDay] [nvarchar](50) NULL DEFAULT (0),
			[Recipients] [nvarchar](500) NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT getutcdate(),
			[ChangedDate] [datetime] NOT NULL  DEFAULT getutcdate())


		ALTER TABLE [tblChecklistSchedule] ADD CONSTRAINT [PK_tblChecklistSchedule] PRIMARY KEY CLUSTERED ([Checklists_Id])

		ALTER TABLE [dbo].[tblChecklistSchedule]  ADD  CONSTRAINT [FK_tblChecklistSchedule_tblChecklists] FOREIGN KEY([Checklists_Id])
			REFERENCES [dbo].[tblChecklists] ([Id])




	end
GO

-- Nytt fält i tblCheckListRow
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RowStatus' and sysobjects.name = N'tblCheckListRow')
	begin
		ALTER TABLE tblCheckListRow ADD RowStatus int NOT NULL Default(1) 

			
	end
GO

UPDATE tblText Set TextString = 'Godkänd' WHERE Id=531;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Approved' WHERE Text_Id=531 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Godkänd med anm' WHERE Id=723;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Approved with note' WHERE Text_Id=723 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Ej godkänd' WHERE Id=721;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Not Approved' WHERE Text_Id=721 AND Language_Id=2;
GO

-- Ändra storlek på fält
ALTER TABLE tblChecklistAction ALTER COLUMN ChecklistAction nvarchar(500) not null
GO

-- Nytt fält i tblOrderType
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionReceiverInfo' and sysobjects.name = N'tblOrderType')
	begin
		ALTER TABLE tblOrderType ADD CaptionReceiverInfo nvarchar(30) NULL

			
	end
GO


-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContractNumber' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD ContractNumber nvarchar(50) NULL

			
	end
GO

-- Nytt fält i tblComputerFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SortOrder' and sysobjects.name = N'tblComputerFieldSettings')
	begin
		ALTER TABLE tblComputerFieldSettings ADD SortOrder int NOT NULL Default(0)

			
	end
GO

-- Nytt fält i tblComputerFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ComputerFieldGroup_Id' and sysobjects.name = N'tblComputerFieldSettings')
	begin
		ALTER TABLE tblComputerFieldSettings ADD ComputerFieldGroup_Id int NULL

			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension1' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD AccountingDimension1 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension2' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD AccountingDimension2 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension3' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD AccountingDimension3 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension4' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD AccountingDimension4 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension5' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD AccountingDimension5 nvarchar(20) NULL

			
	end
GO

UPDATE tblText Set TextString = 'Kontodimension' WHERE Id=669;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Accounting Dimension' WHERE Text_Id=669 AND Language_Id=2;
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactName' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD ContactName nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactPhone' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD ContactPhone nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactEMail' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD ContactEmail nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LocationAddress' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD LocationAddress nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LocationPostalCode' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD LocationPostalCode nvarchar(10) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LocationPostalAddress' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD LocationPostalAddress nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LocationRoom' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD LocationRoom nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblOrder 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension1' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD AccountingDimension1 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension2' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD AccountingDimension2 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension3' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD AccountingDimension3 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension4' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD AccountingDimension4 nvarchar(20) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountingDimension5' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD AccountingDimension5 nvarchar(20) NULL

			
	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblOrderProperty')
	begin
		CREATE TABLE [dbo].[tblOrderProperty](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[OrderProperty] [nvarchar](50) NOT NULL,
			[OrderType_Id] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
			[ChangedDate] [datetime] NOT NULL,
 		CONSTRAINT [PK_tblOrderProperty] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [dbo].[tblOrderProperty]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderProperty_tblOrderType] FOREIGN KEY([OrderType_Id])
		REFERENCES [dbo].[tblOrderType] ([Id])

		ALTER TABLE [dbo].[tblOrderProperty] CHECK CONSTRAINT [FK_tblOrderProperty_tblOrderType]

		ALTER TABLE [dbo].[tblOrderProperty] ADD  CONSTRAINT [DF_tblOrderProperty_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]

		ALTER TABLE [dbo].[tblOrderProperty] ADD  CONSTRAINT [DF_tblOrderProperty_ChangedDate]  DEFAULT (getutcdate()) FOR [ChangedDate]
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderProperty_Id' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD OrderProperty_Id int NULL 

		ALTER TABLE [dbo].[tblOrder] ADD 
			CONSTRAINT [FK_tblOrder_tblOrderProperty] FOREIGN KEY 
			(
				[OrderProperty_Id]
			) REFERENCES [dbo].[tblOrderProperty] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryDepartment_Id' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryDepartment_Id int NULL 

		ALTER TABLE [dbo].[tblOrder] ADD 
			CONSTRAINT [FK_tblOrder_tblDepartment] FOREIGN KEY 
			(
				[DeliveryDepartment_Id]
			) REFERENCES [dbo].[tblDepartment] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryOU' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryOU nvarchar(50) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryAddress' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryAddress nvarchar(50) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryPostalCode' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryPostalCode nvarchar(10) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryPostalAddress' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryPostalAddress nvarchar(50) NULL

			
	end
GO

-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryLocation' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryLocation nvarchar(50) NULL

			
	end
GO

-- Nytt fält i tblOrderHistory
if not exists(select * from sysobjects WHERE Name = N'tblOrderHistory')
	begin
		CREATE TABLE [dbo].[tblOrderHistory](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Order_Id] [int] NOT NULL,
			[OrderHistoryGUID] [uniqueidentifier] NOT NULL,
			[Domain_Id] [int] NULL,
			[OrderDate] [datetime] NULL,
			[UserId] [nvarchar](20) NULL,
			[UserFirstName] [nvarchar](20) NULL,
			[UserLastName] [nvarchar](50) NULL,
			[OrdererID] [nvarchar](40) NULL,
			[Orderer] [nvarchar](50) NULL,
			[OrdererAddress] [nvarchar](50) NULL,
			[OrdererInvoiceAddress] [nvarchar](50) NULL,
			[OrdererLocation] [nvarchar](50) NULL,
			[OrdererEMail] [nvarchar](100) NULL,
			[OrdererPhone] [nvarchar](50) NULL,
			[OrdererCode] [nvarchar](10) NULL,
			[OrdererReferenceNumber] [nvarchar](20) NULL,
			[AccountingDimension1] [nvarchar](20) NULL,
			[AccountingDimension2] [nvarchar](20) NULL,
			[AccountingDimension3] [nvarchar](20) NULL,
			[AccountingDimension4] [nvarchar](20) NULL,
			[AccountingDimension5] [nvarchar](20) NULL,
			[Department_Id] [int] NULL,
			[OU_Id] [int] NULL,
			[OrderProperty_Id] [int] NULL,
			[OrderRow] [nvarchar](100) NULL,
			[OrderRow2] [nvarchar](100) NULL,
			[OrderRow3] [nvarchar](100) NULL,
			[OrderRow4] [nvarchar](100) NULL,
			[OrderRow5] [nvarchar](100) NULL,
			[OrderRow6] [nvarchar](100) NULL,
			[OrderRow7] [nvarchar](100) NULL,
			[OrderRow8] [nvarchar](100) NULL,
			[Configuration] [nvarchar](400) NULL,
			[OrderInfo] [nvarchar](800) NULL,
			[OrderInfo2] [int] NOT NULL,
			[ReceiverId] [nvarchar](40) NULL,
			[ReceiverName] [nvarchar](50) NULL,
			[ReceiverEMail] [nvarchar](100) NULL,
			[ReceiverPhone] [nvarchar](50) NULL,
			[ReceiverLocation] [nvarchar](50) NULL,
			[MarkOfGoods] [nvarchar](100) NULL,
			[SupplierOrderNumber] [nvarchar](20) NULL,
			[SupplierOrderDate] [datetime] NULL,
			[SupplierOrderInfo] [nvarchar](200) NULL,
			[User_Id] [int] NULL,
			[Deliverydate] [datetime] NULL,
			[InstallDate] [datetime] NULL,
			[OrderState_Id] [int] NULL,
			[OrderType_Id] [int] NULL,
			[DeliveryDepartment_Id] [int] NULL,
			[DeliveryOU] [nvarchar](50) NULL,
			[DeliveryAddress] [nvarchar](50) NULL,
			[DeliveryPostalCode] [nvarchar](10) NULL,
			[DeliveryPostalAddress] [nvarchar](50) NULL,
			[DeliveryLocation] [nvarchar](50) NULL,
			[DeliveryInfo] [nvarchar](200) NULL,
			[DeliveryInfo2] [nvarchar](50) NULL,
			[DeliveryInfo3] [nvarchar](50) NULL,
			[Filename] [nvarchar](100) NULL,
			[CaseNumber] [numeric](18, 0) NULL,
			[Info] [nvarchar](200) NULL,
			[Deleted] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
			[CreatedByUser_Id] [int] NOT NULL,
 		CONSTRAINT [PK_tblOrderHistory] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [dbo].[tblOrderHistory]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblDepartment] FOREIGN KEY([DeliveryDepartment_Id])
		REFERENCES [dbo].[tblDepartment] ([Id])

		ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblDomain] FOREIGN KEY([Domain_Id])
		REFERENCES [dbo].[tblDomain] ([Id])


		ALTER TABLE [dbo].[tblOrderHistory]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrder] FOREIGN KEY([Order_Id])
		REFERENCES [dbo].[tblOrder] ([Id])


		ALTER TABLE [dbo].[tblOrderHistory]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderProperty] FOREIGN KEY([OrderProperty_Id])
		REFERENCES [dbo].[tblOrderProperty] ([Id])


		ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderState] FOREIGN KEY([OrderState_Id])
		REFERENCES [dbo].[tblOrderState] ([Id])


		ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOrderType] FOREIGN KEY([OrderType_Id])
		REFERENCES [dbo].[tblOrderType] ([Id])

		ALTER TABLE [dbo].[tblOrderHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblOrderHistory_tblOU] FOREIGN KEY([OU_Id])
		REFERENCES [dbo].[tblOU] ([Id])


		ALTER TABLE [dbo].[tblOrderHistory] ADD  CONSTRAINT [DF_tblOrderHistory_OrderHistoryGUID]  DEFAULT (newid()) FOR [OrderHistoryGUID]

		ALTER TABLE [dbo].[tblOrderHistory] ADD  CONSTRAINT [DF_tblOrderHistory_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
	end
GO

-- Nytt fält i tblOrderEMailLog
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderHistory_Id' and sysobjects.name = N'tblOrderEMailLog')
	begin
		ALTER TABLE tblOrderEMailLog ADD OrderHistory_Id int NULL 

		ALTER TABLE [dbo].[tblOrderEMailLog] ADD 
			CONSTRAINT [FK_tblOrderEMailLog_tblOrderHistory] FOREIGN KEY 
			(
				[OrderHistory_Id]
			) REFERENCES [dbo].[tblOrderHistory] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblOrderLog
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OrderHistory_Id' and sysobjects.name = N'tblOrderLog')
	begin
		ALTER TABLE tblOrderLog ADD OrderHistory_Id int NULL 

		ALTER TABLE [dbo].[tblOrderLog] ADD 
			CONSTRAINT [FK_tblOrderLog_tblOrderHistory] FOREIGN KEY 
			(
				[OrderHistory_Id]
			) REFERENCES [dbo].[tblOrderHistory] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Priority_Id' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD Priority_Id int NULL 

		ALTER TABLE [dbo].[tblForm] ADD 
			CONSTRAINT [FK_tblForm_tblPriority] FOREIGN KEY 
			(
				[Priority_Id]
			) REFERENCES [dbo].[tblPriority] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Category_Id' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD Category_Id int NULL 

		ALTER TABLE [dbo].[tblForm] ADD 
			CONSTRAINT [FK_tblForm_tblCategory] FOREIGN KEY 
			(
				[Category_Id]
			) REFERENCES [dbo].[tblCategory] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondary_Id' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD StateSecondary_Id int NULL 

		ALTER TABLE [dbo].[tblForm] ADD 
			CONSTRAINT [FK_tblForm_tblStateSecondary] FOREIGN KEY 
			(
				[StateSecondary_Id]
			) REFERENCES [dbo].[tblStateSecondary] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroup_Id' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD WorkingGroup_Id int NULL 

		ALTER TABLE [dbo].[tblForm] ADD 
			CONSTRAINT [FK_tblForm_tblWorkingGroup] FOREIGN KEY 
			(
				[WorkingGroup_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Location2' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD Location2 nvarchar(50) NULL			
	end
GO

-- Ändra storlek på fält
ALTER TABLE tblOrderType ALTER COLUMN OrderType nvarchar(50) not null
GO

-- Ändra storlek på fält
ALTER TABLE tblDomain ALTER COLUMN LDAPBase nvarchar(500) not null
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MainMenuPath' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD MainMenuPath nvarchar(100) NULL 

		
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryCreate' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD InventoryCreate int NOT NULL Default(1)			
	end
GO

-- Ändra storlek på fält
ALTER TABLE tblServer ALTER COLUMN Manufacturer nvarchar(100) not null
GO

ALTER TABLE tblServerSoftware ALTER COLUMN Manufacturer nvarchar(100) not null
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OU_Id' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD OU_Id int NULL 

		ALTER TABLE [dbo].[tblComputer] ADD 
			CONSTRAINT [FK_tblForm_tblOU] FOREIGN KEY 
			(
				[OU_Id]
			) REFERENCES [dbo].[tblOU] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblComputerFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CopyField' and sysobjects.name = N'tblComputerFieldSettings')
	begin
		ALTER TABLE tblComputerFieldSettings ADD CopyField int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreateComputerFromOrder' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD CreateComputerFromOrder int NOT NULL Default(0)			
	end
GO


-- Nytt fält i tblOrder
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeliveryOU_Id' and sysobjects.name = N'tblOrder')
	begin
		ALTER TABLE tblOrder ADD DeliveryOU_Id int NULL 

		ALTER TABLE [dbo].[tblOrder] ADD 
			CONSTRAINT [FK_tblOrder_tblOU_2] FOREIGN KEY 
			(
				[DeliveryOU_Id]
			) REFERENCES [dbo].[tblOU] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultAdministratorExternal' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD DefaultAdministratorExternal int NULL			
	end
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'isDefaultAdministratorExternal' and sysobjects.name = N'tblUsers')
	begin
		DECLARE @sql NVARCHAR(MAX)

    		SELECT TOP 1 @sql = N'alter table tblUsers drop constraint ['+dc.NAME+N']'

    		FROM sys.default_constraints dc
    			INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    		WHERE dc.parent_object_id = OBJECT_ID('tblUsers')
    			AND c.name = N'isDefaultAdministratorExternal'
    
		EXEC (@sql)

		ALTER TABLE tblUsers DROP COLUMN isDefaultAdministratorExternal	
	end
GO

UPDATE tblText Set TextString = 'Eventuella ändringar måste sparas innan du flyttar ärendet. Vill du flytta ärendet nu eller avbryta' WHERE Id=973;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Any changes must be saved before moving case. Want to move the case now or cancel' WHERE Text_Id=973 AND Language_Id=2;
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowStatusPanel' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD ShowStatusPanel int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultAdministrator' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD DefaultAdministrator int NULL			
	end
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'isDefaultAdministrator' and sysobjects.name = N'tblUsers')
	begin
		DECLARE @sql NVARCHAR(MAX)

    		SELECT TOP 1 @sql = N'alter table tblUsers drop constraint ['+dc.NAME+N']'

    		FROM sys.default_constraints dc
    			INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    		WHERE dc.parent_object_id = OBJECT_ID('tblUsers')
    			AND c.name = N'isDefaultAdministrator'
    
		EXEC (@sql)

		ALTER TABLE tblUsers DROP COLUMN isDefaultAdministrator		
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseCaseTypeFilter' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD CaseCaseTypeFilter nvarchar(50) NULL Default(0)			
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseStatusFilter' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD CaseStatusFilter nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseResponsibleFilter' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD CaseResponsibleFilter nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CasePriorityFilter' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD CasePriorityFilter nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblCustomer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CloseCaseEMailList' and sysobjects.name = N'tblCustomer')
	begin
		ALTER TABLE tblCustomer ADD CloseCaseEMailList nvarchar(400) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactEMail' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD ContactEMail nvarchar(50) NULL			
	end
GO


-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactName' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD ContactName nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactPhone' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD ContactPhone nvarchar(50) NULL			
	end
GO

UPDATE tblText Set TextString = 'Skicka e-post till följande e-post adresser när ett ärende avslutas' WHERE Id=1010;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Send e-mail to the following e-mail addresses when closing a case' WHERE Text_Id=1010 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Endast unika e-postadresser' WHERE Id=260;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Only unique e-mail addresses' WHERE Text_Id=260 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Påminnelse' WHERE Id=241;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Reminder' WHERE Text_Id=241 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Är du säker på att du vill skicka en påminnelse' WHERE Id=377;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Äre you sure you want to send a reminder' WHERE Text_Id=377 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Visa även skrotade datorer' WHERE Id=381;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Show also scrapped computers' WHERE Text_Id=381 AND Language_Id=2;
GO

-- Ny tabell tblHolidayHeader
if not exists(select * from sysobjects WHERE Name = N'tblHolidayHeader')
	begin
		CREATE TABLE [dbo].[tblHolidayHeader](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[HolidayHeaderName] [nvarchar](50) NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getdate())
		) ON [PRIMARY]

		
		ALTER TABLE [tblHolidayHeader] ADD
 			CONSTRAINT [PK_tblHolidayHeader] PRIMARY KEY CLUSTERED 
			(
				[Id]
			) 

	end
GO

-- Lägg till data i tblHolidayHeader
if not exists(select * from tblHolidayHeader)
	begin
		INSERT INTO tblHolidayHeader(HolidayHeaderName) VALUES('Default')
	end
GO

-- Nytt fält i tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HolidayHeader_Id' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD HolidayHeader_Id int NULL		

		ALTER TABLE [dbo].[tblDepartment] ADD 
			CONSTRAINT [FK_tblDepartment_tblHolidayHeader] FOREIGN KEY 
			(
				[HolidayHeader_Id]
			) REFERENCES [dbo].[tblHolidayHeader] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblHoliday
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HolidayHeader_Id' and sysobjects.name = N'tblHoliday')
	begin
		ALTER TABLE tblHoliday ADD HolidayHeader_Id int DEFAULT(1) NOT NULL	

		ALTER TABLE [dbo].[tblHoliday] ADD 
			CONSTRAINT [FK_tblHoliday_tblHolidayHeader] FOREIGN KEY 
			(
				[HolidayHeader_Id]
			) REFERENCES [dbo].[tblHolidayHeader] (
				[Id]
			)		
	end
GO

UPDATE tblDepartment SET HolidayHeader_Id = 1 WHERE HolidayHeader_Id  IS NULL
GO

UPDATE tblText Set TextString = 'Kalendrar' WHERE Id=1093;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Calendars' WHERE Text_Id=1093 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Kalender' WHERE Id=936;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Calendar' WHERE Text_Id=936 AND Language_Id=2;
GO

-- Nytt fält i tblProductArea
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Priority_Id' and sysobjects.name = N'tblProductArea')
	begin
		ALTER TABLE tblProductArea ADD Priority_Id int NULL	

		ALTER TABLE [dbo].[tblProductArea] ADD 
			CONSTRAINT [FK_tblProductArea_tblPriority] FOREIGN KEY 
			(
				[Priority_Id]
			) REFERENCES [dbo].[tblPriority] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreateCasePermission' and sysobjects.name = N'tblUsers')
	begin
		ALTER TABLE tblUsers ADD CreateCasePermission int Default(1) NOT NULL			
	end
GO

-- Nytt fält i tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DeleteAttachedFilePermission' and sysobjects.name = N'tblUsers')
	begin
		ALTER TABLE tblUsers ADD DeleteAttachedFilePermission int Default(1) NOT NULL			
	end
GO

UPDATE tblText Set TextString = 'Användaren får skapa ärenden' WHERE Id=199;
GO

UPDATE tblTextTranslation Set TextTranslation = 'User has permission to create cases' WHERE Text_Id=199 AND Language_Id=2;
GO


UPDATE tblText Set TextString = 'Användaren får ta bort bifogade filer' WHERE Id=499;
GO

UPDATE tblTextTranslation Set TextTranslation = 'User has permission to delete attached files' WHERE Text_Id=499 AND Language_Id=2;
GO

-- Ny tabell tblWatchDateCalendar
if not exists(select * from sysobjects WHERE Name = N'tblWatchDateCalendar')
	begin
		CREATE TABLE [dbo].[tblWatchDateCalendar](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[WatchDateCalendarName] [nvarchar](50) NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getdate())
		) ON [PRIMARY]

		
		ALTER TABLE [tblWatchDateCalendar] ADD
 			CONSTRAINT [PK_tblWatchDateCalendar] PRIMARY KEY CLUSTERED 
			(
				[Id]
			) 

	end
GO

-- Ny tabell tblWatchDateCalendar
if not exists(select * from sysobjects WHERE Name = N'tblWatchDateCalendarValue')
	begin
		CREATE TABLE [dbo].[tblWatchDateCalendarValue](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[WatchDateCalendar_Id] [int] NOT NULL,
			[WatchDate] [datetime] NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getdate()))
		ON [PRIMARY]

		ALTER TABLE [tblWatchDateCalendarValue] ADD
 			CONSTRAINT [PK_tblWatchDateCalendarValue] PRIMARY KEY CLUSTERED 
			(
				[Id]
			) 

		ALTER TABLE [dbo].[tblWatchDateCalendarValue] ADD 
			CONSTRAINT [FK_tblWatchDateCalendarValue_tblWatchDateCalendar] FOREIGN KEY 
			(
				[WatchDateCalendar_Id]
			) REFERENCES [dbo].[tblWatchDateCalendar] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WatchDateCalendar_Id' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD WatchDateCalendar_Id int NULL		

		ALTER TABLE [dbo].[tblDepartment] ADD 
			CONSTRAINT [FK_tblDepartment_tblWatchDateCalendar] FOREIGN KEY 
			(
				[WatchDateCalendar_Id]
			) REFERENCES [dbo].[tblWatchDateCalendar] (
				[Id]
			)		
	end
GO


UPDATE tblText Set TextString = 'Är du säker på att du vill kopiera aktuell post' WHERE Id=965;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Are you sure you want to copy selected record' WHERE Text_Id=965 AND Language_Id=2;
GO

-- Nytt fält i tblText
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'TextType' and sysobjects.name = N'tblText')
	begin
		ALTER TABLE tblText ADD TextType int Default(0) NOT NULL			
	end
GO

If not exists (select * from tblText where Id = 1212)
	insert into tblText (Id, Textstring) VALUES (1212, 'Översättningar')
GO

If not exists (select * from tblTextTranslation where Text_Id = 1212 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1212, 2, 'Translations')
GO


-- Nytt fält i tblStatus
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroup_Id' and sysobjects.name = N'tblStatus')
	begin
		ALTER TABLE tblStatus ADD WorkingGroup_Id int NULL		

		ALTER TABLE [dbo].[tblStatus] ADD 
			CONSTRAINT [FK_tblStatus_tblWorkingGroup] FOREIGN KEY 
			(
				[WorkingGroup_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblStatus
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondary_Id' and sysobjects.name = N'tblStatus')
	begin
		ALTER TABLE tblStatus ADD StateSecondary_Id int NULL		

		ALTER TABLE [dbo].[tblStatus] ADD 
			CONSTRAINT [FK_tblStatus_tblStateSecondary] FOREIGN KEY 
			(
				[StateSecondary_Id]
			) REFERENCES [dbo].[tblStateSecondary] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblStateSecondary
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroup_Id' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD WorkingGroup_Id int NULL		

		ALTER TABLE [dbo].[tblStateSecondary] ADD 
			CONSTRAINT [FK_tblStateSecondary_tblWorkingGroup] FOREIGN KEY 
			(
				[WorkingGroup_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblWorkingGroup
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondary_Id' and sysobjects.name = N'tblWorkingGroup')
	begin
		ALTER TABLE tblWorkingGroup ADD StateSecondary_Id int NULL		

		ALTER TABLE [dbo].[tblWorkingGroup] ADD 
			CONSTRAINT [FK_tblWorkingGroup_tblStateSecondary] FOREIGN KEY 
			(
				[StateSecondary_Id]
			) REFERENCES [dbo].[tblStateSecondary] (
				[Id]
			)		
	end
GO

If not exists (select * from tblText where Id = 105)
	insert into tblText (Id, Textstring) VALUES (105, 'Regler')
GO

If not exists (select * from tblTextTranslation where Text_Id = 105 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(105, 2, 'Rules')
GO


If not exists (select * from tblText where Id = 106)
	insert into tblText (Id, Textstring) VALUES (106, 'Nedanstående värden ändras när aktuell driftgrupp väljs')
GO

If not exists (select * from tblTextTranslation where Text_Id = 106 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(106, 2, 'The following values ​​change when the current operating group is selected')
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondaryPermission' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD StateSecondaryPermission int NOT NULL Default(1)			
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PriorityPermission' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD PriorityPermission int NOT NULL Default(1)			
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaptionPermission' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD CaptionPermission int NOT NULL Default(1)			
	end
GO



If not exists (select * from tblText where Id = 107)
	insert into tblText (Id, Textstring) VALUES (107, 'Användaren får ändra')
GO

If not exists (select * from tblTextTranslation where Text_Id = 107 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(107, 2, 'User has permission to change')
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ContactBeforeActionPermission' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD ContactBeforeActionPermission int NOT NULL Default(1)			
	end
GO


If not exists (select * from tblText where Id = 108)
	insert into tblText (Id, Textstring) VALUES (108, 'Nedanstående värden ändras när aktuell status väljs')
GO

If not exists (select * from tblTextTranslation where Text_Id = 108 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(108, 2, 'The following values ​​change when the current status is selected')
GO

If not exists (select * from tblText where Id = 109)
	insert into tblText (Id, Textstring) VALUES (109, 'Nedanstående värden ändras när aktuell understatus väljs')
GO

If not exists (select * from tblTextTranslation where Text_Id = 109 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(109, 2, 'The following values ​​change when the current sub status is selected')
GO

-- Nytt fält i tblContractCategory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Form_Id' and sysobjects.name = N'tblContractCategory')
	begin
		ALTER TABLE tblContractCategory ADD Form_Id int NULL		

		ALTER TABLE [dbo].[tblContractCategory] ADD 
			CONSTRAINT [FK_tblContractCategory_tblForm] FOREIGN KEY 
			(
				[Form_Id]
			) REFERENCES [dbo].[tblForm] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FormUpdate' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD FormUpdate int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblCustomerUser
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserInfoPermission' and sysobjects.name = N'tblCustomerUser')
	begin
		ALTER TABLE tblCustomerUser ADD UserInfoPermission int NOT NULL Default(1)			
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SetUserToAdministrator' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD SetUserToAdministrator int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblCaseSolution
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Department_Id' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD Department_Id int NULL		

		ALTER TABLE [dbo].[tblCaseSolution] ADD 
			CONSTRAINT [FK_tblCaseSolution_tblDepartment] FOREIGN KEY 
			(
				[Department_Id]
			) REFERENCES [dbo].[tblDepartment] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangeAnalysisLogNotification' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD ChangeAnalysisLogNotification int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblProblem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnStartPage' and sysobjects.name = N'tblProblem')
	begin
		ALTER TABLE tblProblem ADD ShowOnStartPage int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblFAQ
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnStartPage' and sysobjects.name = N'tblFAQ')
	begin
		ALTER TABLE tblFAQ ADD ShowOnStartPage int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Version' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD Version nvarchar(10) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Domain_Id' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD Domain_Id int NULL		

		ALTER TABLE [dbo].[tblSystem] ADD 
			CONSTRAINT [FK_tblSystem_tblDomain] FOREIGN KEY 
			(
				[Domain_Id]
			) REFERENCES [dbo].[tblDomain] (
				[Id]
			)		
	end
GO

If not exists (select * from tblText where Id = 135)
	insert into tblText (Id, Textstring) VALUES (135, 'Avslagen')
GO

If not exists (select * from tblTextTranslation where Text_Id = 135 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(135, 2, 'Rejected')
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OS_Id' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD OS_Id int NULL		

		ALTER TABLE [dbo].[tblSystem] ADD 
			CONSTRAINT [FK_tblSystem_tblOperatingSystem] FOREIGN KEY 
			(
				[OS_Id]
			) REFERENCES [dbo].[tblOperatingSystem] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblInvoiceRow
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedByUser_Id' and sysobjects.name = N'tblInvoiceRow')
	begin
		ALTER TABLE tblInvoiceRow ADD CreatedByUser_Id int NULL		

		ALTER TABLE [dbo].[tblInvoiceRow] ADD 
			CONSTRAINT [FK_tblInvoiceRow_tblUsers] FOREIGN KEY 
			(
				[CreatedByUser_Id]
			) REFERENCES [dbo].[tblUsers] (
				[Id]
			)		
	end
GO


-- Nytt fält i tblInvoiceRow
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedByUser_Id' and sysobjects.name = N'tblInvoiceRow')
	begin
		ALTER TABLE tblInvoiceRow ADD ChangedByUser_Id int NULL		

		ALTER TABLE [dbo].[tblInvoiceRow] ADD 
			CONSTRAINT [FK_tblInvoiceRow_tblUsers2] FOREIGN KEY 
			(
				[ChangedByUser_Id]
			) REFERENCES [dbo].[tblUsers] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DocumentPath' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD DocumentPath nvarchar(100) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'License' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD License nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SystemOwnerUserId' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD SystemOwnerUserId nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SystemResponsibleUserId' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD SystemResponsibleUserId nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ViceSystemResponsibleUserId' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD ViceSystemResponsibleUserId nvarchar(50) NULL			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Price' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD Price int NOT NULL Default(0)			
	end
GO

UPDATE tblText Set TextString = 'Användaren får läsa' WHERE Id=1018;
GO

UPDATE tblTextTranslation Set TextTranslation = 'User has permission to read' WHERE Text_Id=1018 AND Language_Id=2;
GO

-- Nytt fält i tblChecklists 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroup_Id' and sysobjects.name = N'tblChecklists')
	begin
		ALTER TABLE tblChecklists ADD WorkingGroup_Id int NULL		

		ALTER TABLE [dbo].[tblChecklists] ADD 
			CONSTRAINT [FK_tblChecklists_tblWorkingGroup] FOREIGN KEY 
			(
				[WorkingGroup_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblChange
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PlannedReadyDate' and sysobjects.name = N'tblChange')
	begin
		ALTER TABLE tblChange ADD PlannedReadyDate datetime NULL			
	end
GO

-- Nytt fält i tblChangeHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PlannedReadyDate' and sysobjects.name = N'tblChangeHistory')
	begin
		ALTER TABLE tblChangeHistory ADD PlannedReadyDate datetime NULL			
	end
GO

If not exists (select * from tblText where Id = 113)
	insert into tblText (Id, Textstring) VALUES (113, 'Tyska')
GO

If not exists (select * from tblTextTranslation where Text_Id = 113 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(113, 2, 'German')
GO

If not exists (select * from tblText where Id = 157)
	insert into tblText (Id, Textstring) VALUES (157, 'Markera de kunder du vill visa på startsidan')
GO

If not exists (select * from tblTextTranslation where Text_Id = 157 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(157, 2, 'Select customers you want to show on start page')
GO

-- Ändra storlek på fält
ALTER TABLE tblAccount ALTER COLUMN ContactId nvarchar(200) null
GO

ALTER TABLE tblAccount ALTER COLUMN UserId nvarchar(200) null
GO

ALTER TABLE tblAccount ALTER COLUMN UserPersonalIdentityNumber nvarchar(200) null
GO


-- Nytt fält i tblAccountFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MultiValue' and sysobjects.name = N'tblAccountFieldSettings')
	begin
		ALTER TABLE tblAccountFieldSettings ADD MultiValue int NOT NULL Default(0)			
	end
GO

If not exists (select * from tblText where Id = 190)
	insert into tblText (Id, Textstring) VALUES (190, 'Flerval')
GO

If not exists (select * from tblTextTranslation where Text_Id = 190 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(190, 2, 'Multiple choice')
GO

if not exists(select * from sysobjects WHERE Name = N'tblSite')
	begin
		CREATE TABLE [dbo].[tblSite](
			[Id] [int] NOT NULL,
			[Site] [nvarchar](100) NOT NULL,
			[DSN] [nvarchar](100) NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getdate()))

		ALTER TABLE [tblSite] ADD
 			CONSTRAINT [PK_tblSite] PRIMARY KEY CLUSTERED ([Id] ASC)
	end
GO

ALTER TABLE tblSystem ALTER COLUMN SystemName nvarchar(100) not null
GO

ALTER TABLE tblSystem ALTER COLUMN Version nvarchar(50) null
GO

ALTER TABLE tblAccountActivity ALTER COLUMN AccountActivityDescription nvarchar(1500) null
GO

If not exists (select * from tblText where Id = 195)
	insert into tblText (Id, Textstring) VALUES (195, 'Allmän information')
GO

If not exists (select * from tblTextTranslation where Text_Id = 195 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(195, 2, 'General information')
GO

-- Nytt fält i tblCaseSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowInMobileList' and sysobjects.name = N'tblCaseSettings')
	begin
		ALTER TABLE tblCaseSettings ADD ShowInMobileList int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblProductArea
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'isEMailDefault' and sysobjects.name = N'tblProductArea')
	begin
		ALTER TABLE tblProductArea ADD isEMailDefault int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblWorkingGroup 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreateCase_ProductArea_Id' and sysobjects.name = N'tblWorkingGroup')
	begin
		ALTER TABLE tblWorkingGroup ADD CreateCase_ProductArea_Id int NULL		

		ALTER TABLE [dbo].[tblWorkingGroup] ADD 
			CONSTRAINT [FK_tblWorkingGroup_tblProductArea] FOREIGN KEY 
			(
				[CreateCase_ProductArea_Id]
			) REFERENCES [dbo].[tblProductArea] (
				[Id]
			)		
	end
GO

If not exists (select * from tblText where Id = 196)
	insert into tblText (Id, Textstring) VALUES (196, 'Portugisiska')
GO

If not exists (select * from tblTextTranslation where Text_Id = 196 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(196, 2, 'Portuguese')
GO

if not exists(select * from sysobjects WHERE Name = N'tblComputerUserFS_tblLanguage')
	begin
		CREATE TABLE [dbo].[tblComputerUserFS_tblLanguage](
			[ComputerUserFieldSettings_Id] [int] NOT NULL,
			[Language_Id] [int] NOT NULL,
			[Label] [nvarchar](50) NOT NULL,
			[FieldHelp] [nvarchar](200) NULL,
 		CONSTRAINT [PK_tblComputerUserFS_tblLanguage] PRIMARY KEY CLUSTERED 
		(
			[ComputerUserFieldSettings_Id] ASC,
			[Language_Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblComputerUserFS_tblLanguage]  WITH CHECK ADD  CONSTRAINT [FK_tblComputerUserFS_tblLanguage_tblComputerUserFieldSettings] FOREIGN KEY([ComputerUserFieldSettings_Id])
		REFERENCES [dbo].[tblComputerUserFieldSettings] ([Id])

		ALTER TABLE [dbo].[tblComputerUserFS_tblLanguage]  WITH CHECK ADD  CONSTRAINT [FK_tblComputerUserFS_tblLanguage_tblLanguage] FOREIGN KEY([Language_Id])
		REFERENCES [dbo].[tblLanguage] ([Id])

	end
GO


if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
           where syscolumns.name = N'FieldHelp' and sysobjects.name = N'tblComputerUserFieldSettings')
   ALTER TABLE tblComputerUserFieldSettings ADD FieldHelp nvarchar(50) not NULL Default('')
GO

if not exists(select * from tblComputerUserFS_tblLanguage WHERE Language_Id=1)
begin
	if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Label' and sysobjects.name = N'tblComputerUserFieldSettings')
	begin
		if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
                   where syscolumns.name = N'FieldHelp' and sysobjects.name = N'tblComputerUserFieldSettings')
		begin
			exec sp_executesql N'INSERT INTO tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label, FieldHelp) 
			                  SELECT Id, 1,Label,FieldHelp from tblComputerUserFieldSettings' 
		end
	end
end

if not exists(select * from tblComputerUserFS_tblLanguage WHERE Language_Id=2)
begin
	if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Label' and sysobjects.name = N'tblComputerUserFieldSettings')
	begin
		if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
                   where syscolumns.name = N'FieldHelp' and sysobjects.name = N'tblComputerUserFieldSettings')
		begin			
			exec sp_executesql  N'INSERT INTO tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label, FieldHelp) 
			                  SELECT Id, 2, Label, FieldHelp from tblComputerUserFieldSettings' 
		end
	end
end

If not exists (select * from tblText where Id = 245)
	insert into tblText (Id, Textstring) VALUES (245, 'Kalender helgdagar')
GO

If not exists (select * from tblTextTranslation where Text_Id = 245 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(245, 2, 'Holiday calendar')
GO


If not exists (select * from tblText where Id = 337)
	insert into tblText (Id, Textstring) VALUES (337, 'Kalender bevakningsdatum')
GO

If not exists (select * from tblTextTranslation where Text_Id = 337 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(337, 2, 'Watch Date calendar')
GO

UPDATE tblText Set TextString = 'Ärenden med' WHERE Id=25;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Cases with' WHERE Text_Id=25 AND Language_Id=2;
GO

if not exists(select * from sysobjects WHERE Name = N'tblLink_tblUsers')
	begin
		CREATE TABLE [dbo].[tblLink_tblUsers](
			[Link_Id] [int] NOT NULL,
			[User_Id] [int] NOT NULL,
		 CONSTRAINT [PK_tblLink_tblUsers] PRIMARY KEY CLUSTERED 
		(
			[Link_Id] ASC,
			[User_Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblLink_tblUsers]  WITH CHECK ADD  CONSTRAINT [FK_tblLink_tblUsers_tblLink] FOREIGN KEY([Link_Id])
		REFERENCES [dbo].[tblLink] ([Id])

		ALTER TABLE [dbo].[tblLink_tblUsers]  WITH CHECK ADD  CONSTRAINT [FK_tblLink_tblUsers_tblUsers] FOREIGN KEY([User_Id])
		REFERENCES [dbo].[tblUsers] ([Id])
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SelectEmailAddressSettings' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD SelectEmailAddressSettings int NOT NULL Default(31)			
	end
GO

If not exists (select * from tblText where Id = 498)
	insert into tblText (Id, Textstring) VALUES (498, 'Begränsa rättighet att skapa ärenden till')
GO

If not exists (select * from tblTextTranslation where Text_Id = 498 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(498, 2, 'Restrict access to create case to')
GO

if not exists(select * from sysobjects WHERE Name = N'tblProductArea_tblWorkingGroup')
	begin
		CREATE TABLE [dbo].[tblProductArea_tblWorkingGroup](
			[ProductArea_Id] [int] NOT NULL,
			[WorkingGroup_Id] [int] NOT NULL,
		 CONSTRAINT [PK_tblProductArea_tblWorkingGroup] PRIMARY KEY CLUSTERED 
		(
			[ProductArea_Id] ASC,
			[WorkingGroup_Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblProductArea_tblWorkingGroup]  WITH CHECK ADD  CONSTRAINT [FK_tblProductArea_tblWorkingGroup_tblProductArea] FOREIGN KEY([ProductArea_Id])
		REFERENCES [dbo].[tblProductArea] ([Id])

		ALTER TABLE [dbo].[tblProductArea_tblWorkingGroup]  WITH CHECK ADD  CONSTRAINT [FK_tblProductArea_tblWorkingGroup_tblWorkingGroup] FOREIGN KEY([WorkingGroup_Id])
		REFERENCES [dbo].[tblWorkingGroup] ([Id])
	end
GO



if not exists(select * from sysobjects WHERE Name = N'tblInventoryTypeGroup')
	begin
		CREATE TABLE [dbo].[tblInventoryTypeGroup](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[InventoryTypeGroup] [nvarchar](50) NOT NULL,
			[SortOrder] [int] NOT NULL DEFAULT ((0)),
			[InventoryType_Id] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL DEFAULT (getdate()),
			[ChangedDate] [datetime] NOT NULL DEFAULT (getdate()))

		ALTER TABLE [tblInventoryTypeGroup] ADD CONSTRAINT [PK_tblInventoryTypeGroup] PRIMARY KEY CLUSTERED ([Id])
 
		ALTER TABLE [dbo].[tblInventoryTypeGroup] 
			ADD  CONSTRAINT [FK_tbltblInventoryTypeGroup_tblInventoryType] FOREIGN KEY([InventoryType_Id])
				REFERENCES [dbo].[tblInventoryType] ([Id])
	end

-- Nytt fält i tblInventoryTypeProperty
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryTypeGroup_Id' and sysobjects.name = N'tblInventoryTypeProperty')
	begin
		ALTER TABLE tblInventoryTypeProperty ADD InventoryTypeGroup_Id int NULL 

		ALTER TABLE [dbo].[tblInventoryTypeProperty] ADD 
			CONSTRAINT [FK_tblInventoryTypeProperty_tblInventoryTypeGroup] FOREIGN KEY 
			(
				[InventoryTypeGroup_Id]
			) REFERENCES [dbo].[tblInventoryTypeGroup] (
				[Id]
			)	
	end
GO

-- Referens från settings till customer
if not exists(select * from sysobjects WHERE Name = N'FK_tblSettings_tblCustomer')
	ALTER TABLE [dbo].[tblSettings] ADD 
		CONSTRAINT [FK_tblSettings_tblCustomer] FOREIGN KEY 
			(
				[Customer_Id]
			) REFERENCES [dbo].[tblCustomer] (
				[Id]
			)
GO


If not exists (select * from tblText where Id = 628)
	insert into tblText (Id, Textstring) VALUES (628, 'Timeout Session')
GO

If not exists (select * from tblTextTranslation where Text_Id = 628 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(628, 2, 'Timeout Session')
GO

-- Nytt fält i tblForm
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ExternalPage' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD ExternalPage int NOT NULL Default(0)			
	end
GO

UPDATE tblText Set TextString = 'Flytta ärende till annan site' WHERE Id=788;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Move Case to another site' WHERE Text_Id=788 AND Language_Id=2;
GO

-- Nytt fält i tblContract
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'NoticeTime' and sysobjects.name = N'tblContract')
	begin
		ALTER TABLE tblContract ADD NoticeTime int NOT NULL Default(1)			
	end
GO

-- Nytt fält i tblContractHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'NoticeTime' and sysobjects.name = N'tblContractHistory')
	begin
		ALTER TABLE tblContractHistory ADD NoticeTime int NOT NULL Default(1)			
	end
GO

UPDATE tblMailTemplate_tblLanguage SET Body = Body + '<br><br>[#99]' WHERE Body NOT Like '%#99]%'
GO

UPDATE tblText Set TextString = 'före' WHERE Id=722;
GO

UPDATE tblTextTranslation Set TextTranslation = 'before' WHERE Text_Id=722 AND Language_Id=2;
GO

-- Nytt fält i tblProductArea
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreateCase_Form_Id' and sysobjects.name = N'tblProductArea')
	begin
		ALTER TABLE tblProductArea ADD CreateCase_Form_Id int NULL 

		ALTER TABLE [dbo].[tblProductArea] ADD 
			CONSTRAINT [FK_tblProductArea_tblForm] FOREIGN KEY 
			(
				[CreateCase_Form_Id]
			) REFERENCES [dbo].[tblForm] (
				[Id]
			)	
	end
GO

-- Nytt fält i tblLink
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'NewWindowHeight' and sysobjects.name = N'tblLink')
	begin
		ALTER TABLE tblLink ADD NewWindowHeight int NOT NULL Default(500)			
	end
GO

-- Nytt fält i tblLink
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'NewWindowWidth' and sysobjects.name = N'tblLink')
	begin
		ALTER TABLE tblLink ADD NewWindowWidth int NOT NULL Default(700)			
	end
GO

-- Nytt fält i tblComputer
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ComputerContractStatus_Id' and sysobjects.name = N'tblComputer')
	begin
		ALTER TABLE tblComputer ADD ComputerContractStatus_Id int NULL			
	end
GO


ALTER TABLE tblCaseFieldSettings ALTER COLUMN Customer_Id int NULL
GO

-- Nytt fält i tblLog
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Price' and sysobjects.name = N'tblLog')
	begin
		ALTER TABLE tblLog ADD Price int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblStateSecondary
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondaryId' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD StateSecondaryId int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OverwriteCaseEMailAddress' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD OverwriteCaseEMailAddress int NOT NULL Default(1)			
	end
GO

UPDATE tblText Set TextString = 'Hög prioritet' WHERE Id=583;
GO

UPDATE tblTextTranslation Set TextTranslation = 'High priority' WHERE Text_Id=583 AND Language_Id=2;
GO

-- Referens från case till supplier
if not exists(select * from sysobjects WHERE Name = N'FK_tblCase_tblSupplier')
	ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblSupplier] FOREIGN KEY 
			(
				[Supplier_Id]
			) REFERENCES [dbo].[tblSupplier] (
				[Id]
			)
GO

If not exists (select * from tblText where Id = 675)
	insert into tblText (Id, Textstring) VALUES (675, 'Holländska')
GO

If not exists (select * from tblTextTranslation where Text_Id = 675 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(675, 2, 'Dutch')
GO

UPDATE tblText Set TextString = 'Franska' WHERE Id=812;
GO

UPDATE tblTextTranslation Set TextTranslation = 'French' WHERE Text_Id=812 AND Language_Id=2;
GO

-- Referens från case till working group
if not exists(select * from sysobjects WHERE Name = N'FK_tblCase_tblWorkingGroup')
	ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblWorkingGroup] FOREIGN KEY 
			(
				[WorkingGroup_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)
GO

-- Referens från case till understatus
if not exists(select * from sysobjects WHERE Name = N'FK_tblCase_tblStateSecondary')
	ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblStateSecondary] FOREIGN KEY 
			(
				[StateSecondary_Id]
			) REFERENCES [dbo].[tblStateSecondary] (
				[Id]
			)
GO

-- Referens från case till kategori
if not exists(select * from sysobjects WHERE Name = N'FK_tblCase_tblCategory')
	ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblCategory] FOREIGN KEY 
			(
				[Category_Id]
			) REFERENCES [dbo].[tblCategory] (
				[Id]
			)
GO

-- Ändra storlek på fält
ALTER TABLE tblServer ALTER COLUMN SerialNumber nvarchar(60) not null
GO

-- Nytt fält i tblAccountActivity 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'tblAccountActivity')
	begin
		ALTER TABLE tblAccountActivity ADD Status int NOT NULL Default(1)			
	end
GO

UPDATE tblText Set TextString = 'Ärendehantering' WHERE Id=719;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Case Management' WHERE Text_Id=719 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Administration' WHERE Id=720;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Administration' WHERE Text_Id=720 AND Language_Id=2;
GO

if not exists(select * from sysobjects WHERE Name = N'DF_tblCase_RegTime')
	ALTER TABLE tblCase ADD CONSTRAINT DF_tblCase_RegTime DEFAULT getutcdate() FOR RegTime
GO

if not exists(select * from sysobjects WHERE Name = N'DF_tblCase_ChangeTime')
	ALTER TABLE tblCase ADD CONSTRAINT DF_tblCase_ChangeTime DEFAULT getutcdate() FOR ChangeTime
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ModuleDataSecurity' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD ModuleDataSecurity int NOT NULL Default(0)			
	end
GO



-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PhysicalFilePath' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD PhysicalFilePath nvarchar(100) NULL			
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'VirtualFilePath' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD VirtualFilePath nvarchar(100) NULL			
	end
GO

-- Nytt fält i tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DataSecurityPermission' and sysobjects.name = N'tblUsers')
	begin
		ALTER TABLE tblUsers ADD DataSecurityPermission int NOT NULL DEFAULT(0)			
	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblFileViewLog')
	begin
		CREATE TABLE [dbo].[tblFileViewLog](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Case_Id] [int] NOT NULL,
			[User_Id] [int] NOT NULL,
			[FileName] [nvarchar](200) NOT NULL,
			[FilePath] [nvarchar](200) NOT NULL,
			[FileSource] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
 		CONSTRAINT [PK_tblFileViewLog] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblFileViewLog] ADD  CONSTRAINT [DF_tblFileViewLog_FileSource]  DEFAULT ((1)) FOR [FileSource]

		ALTER TABLE [dbo].[tblFileViewLog] ADD  CONSTRAINT [DF_Table_1_ViewDate]  DEFAULT (getutcdate()) FOR [CreatedDate]

		ALTER TABLE [dbo].[tblFileViewLog]  WITH CHECK ADD  CONSTRAINT [FK_tblFileViewLog_tblCase] FOREIGN KEY([Case_Id])
		REFERENCES [dbo].[tblCase] ([Id])

		ALTER TABLE [dbo].[tblFileViewLog] CHECK CONSTRAINT [FK_tblFileViewLog_tblCase]

		ALTER TABLE [dbo].[tblFileViewLog]  WITH CHECK ADD  CONSTRAINT [FK_tblFileViewLog_tblUsers] FOREIGN KEY([User_Id])
		REFERENCES [dbo].[tblUsers] ([Id])

		ALTER TABLE [dbo].[tblFileViewLog] CHECK CONSTRAINT [FK_tblFileViewLog_tblUsers]
	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblCaseCleanUp')
	begin
		CREATE TABLE [dbo].[tblCaseCleanUp](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Customer_Id] [int] NOT NULL,
			[ProductArea_Id] [nvarchar](200) NULL,
			[RegTimeFrom] [datetime] NULL,
			[RegTimeUntil] [datetime] NULL,
			[FinishingCause_Id] [nvarchar](100) NULL,
			[Exclude_Status_Id] [nvarchar](100) NULL,
			[RunDate] [datetime] NULL,
			[CreatedByUser_Id] [int] NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
 		CONSTRAINT [PK_tblCaseCleanUp] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblCaseCleanUp] ADD  CONSTRAINT [DF_tblCaseCleanUp_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]

		ALTER TABLE [dbo].[tblCaseCleanUp]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseCleanUp_tblCustomer] FOREIGN KEY([Customer_Id])
		REFERENCES [dbo].[tblCustomer] ([Id])

		ALTER TABLE [dbo].[tblCaseCleanUp] CHECK CONSTRAINT [FK_tblCaseCleanUp_tblCustomer]

		ALTER TABLE [dbo].[tblCaseCleanUp]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseCleanUp_tblUsers] FOREIGN KEY([CreatedByUser_Id])
		REFERENCES [dbo].[tblUsers] ([Id])

		ALTER TABLE [dbo].[tblCaseCleanUp] CHECK CONSTRAINT [FK_tblCaseCleanUp_tblUsers]

	end

GO

-- Nytt fält i tblCase
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseCleanUp_Id' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE tblCase ADD CaseCleanUp_Id int NULL 

		ALTER TABLE [dbo].[tblCase] ADD 
			CONSTRAINT [FK_tblCase_tblCaseCleanUp] FOREIGN KEY 
			(
				[CaseCleanUp_Id]
			) REFERENCES [dbo].[tblCaseCleanUp] (
				[Id]
			)	
	end
GO

UPDATE tblText Set TextString = 'Användaren har rättighet till datasäkerhetsinformation' WHERE Id=126;
GO

UPDATE tblTextTranslation Set TextTranslation = 'User has permission to Data Security info' WHERE Text_Id=126 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Datasäkerhetsinformation' WHERE Id=780;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Data Security info' WHERE Text_Id=780 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Rensa ärenden' WHERE Id=974;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Clear cases' WHERE Text_Id=974 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Nedladdade filer' WHERE Id=725;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Downloaded files' WHERE Text_Id=725 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Exkludera ärenden' WHERE Id=729;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Exclude cases' WHERE Text_Id=729 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Visa ärenden' WHERE Id=293;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Show cases' WHERE Text_Id=293 AND Language_Id=2;
GO

if not exists(select * from sysobjects WHERE Name = N'tblAccessLog')
	begin
		CREATE TABLE [dbo].[tblAccessLog](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Customer_Id] [int] NOT NULL,
			[UserId] [nvarchar](50) NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
 		CONSTRAINT [PK_tblAccessLog] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblAccessLog] ADD  CONSTRAINT [DF_tblAccessLog_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]

		ALTER TABLE [dbo].[tblAccessLog]  WITH CHECK ADD  CONSTRAINT [FK_tblAccessLog_tblCustomer] FOREIGN KEY([Customer_Id])
		REFERENCES [dbo].[tblCustomer] ([Id])

		ALTER TABLE [dbo].[tblAccessLog] CHECK CONSTRAINT [FK_tblAccessLog_tblCustomer]
	end
GO

UPDATE tblText Set TextString = 'Ärendet har ändrats. Klicka på spara annars förloras dina ändringarna.' WHERE Id=1025;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Case has changed. Click on save or your changes will be lost.' WHERE Text_Id=1025 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1025 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'utan' WHERE Id=370;
GO

UPDATE tblTextTranslation Set TextTranslation = 'without' WHERE Text_Id=370 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=370 AND Language_Id > 2
GO

-- Referens från tblComputerUsers till tblOU
if not exists(select * from sysobjects WHERE Name = N'FK_tblComputerUsers_tblOU')
	ALTER TABLE [dbo].[tblComputerUsers] ADD 
		CONSTRAINT [FK_tblComputerUsers_tblOU] FOREIGN KEY 
			(
				[OU_Id]
			) REFERENCES [dbo].[tblOU] (
				[Id]
			)
GO

-- Nytt fält i tblLink
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SortOrder' and sysobjects.name = N'tblLink')
	begin
		ALTER TABLE tblLink ADD SortOrder nvarchar(2) NULL		
	end
GO

UPDATE tblText Set TextString = 'Grupperat per' WHERE Id=1021;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Grouped by' WHERE Text_Id=1021 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1021 AND Language_Id > 2
GO

-- Nytt fält i tblPriority
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'BackGroundColor' and sysobjects.name = N'tblPriority')
	begin
		ALTER TABLE tblPriority ADD BackGroundColor nvarchar(7) NOT NULL DEFAULT('ffffff');			
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AllowedEMailRecipients' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD AllowedEMailRecipients nvarchar(200) NULL		
	end
GO

-- Nytt fält i tblAccount
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AccountType5' and sysobjects.name = N'tblAccount')
	begin
		ALTER TABLE tblAccount ADD AccountType5 int NULL		
	end
GO	

declare @cmd1 nvarchar(1000)
declare @defName nvarchar(100)

set @cmd1 = 'ALTER TABLE dbo.tblDepartment drop constraint '

select @defName = d.name
 from sys.tables t
  join    sys.default_constraints d
   on d.parent_object_id = t.object_id
  join    sys.columns c
   on c.object_id = t.object_id
    and c.column_id = d.parent_column_id
 where t.name = 'tblDepartment'
  and t.schema_id = schema_id('dbo')
  and c.name = 'AccountancyAmount'

set @cmd1 = @cmd1 + ' ' + @defName
execute (@cmd1)


ALTER TABLE tblDepartment ALTER COLUMN AccountancyAmount int not null 
set @cmd1 = 'ALTER TABLE tblDepartment ADD CONSTRAINT ' + @defName + ' DEFAULT (0) FOR AccountancyAmount'
execute (@cmd1)

Go

if not exists(select * from sysobjects WHERE Name = N'tblLinkGroup')
	begin
		CREATE TABLE [dbo].[tblLinkGroup](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[LinkGroup] [nvarchar](50) NOT NULL,
			[Customer_Id] [int] NOT NULL)

		ALTER TABLE [dbo].[tblLinkGroup] ADD CONSTRAINT [PK_tblLinkGroup] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)


		ALTER TABLE [dbo].[tblLinkGroup] ADD CONSTRAINT [FK_tblLinkGroup_tblCustomer] FOREIGN KEY([Customer_Id])
		REFERENCES [dbo].[tblCustomer] ([Id])

	end
GO



-- Nytt fält
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LinkGroup_Id' and sysobjects.name = N'tblLink')
	begin
		ALTER TABLE tblLink ADD LinkGroup_Id int NULL

		ALTER TABLE [dbo].[tblLink] ADD 
			CONSTRAINT [FK_tblLink_tblLinkGroup] FOREIGN KEY 
				(
					[LinkGroup_Id]
				) REFERENCES [dbo].[tblLinkGroup] (
					[Id]
				)
	end
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocId' and sysobjects.name = N'tblCase')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCase') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocId')  

		set @sql = N'alter table tblCase drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCase 
		drop column MOSS_DocId
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocId' and sysobjects.name = N'tblCaseHistory')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCaseHistory') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCaseHistory') and name = 'MOSS_DocId')  

		set @sql = N'alter table tblCaseHistory drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCaseHistory
		drop column MOSS_DocId
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocURL' and sysobjects.name = N'tblCase')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCase') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocURL')  

		set @sql = N'alter table tblCase drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCase 
		drop column MOSS_DocURL
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocURL' and sysobjects.name = N'tblCaseHistory')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCaseHistory') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocURL')  

		set @sql = N'alter table tblCaseHistory drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCaseHistory
		drop column MOSS_DocURL
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocVersion' and sysobjects.name = N'tblCase')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCase') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocVersion')  

		set @sql = N'alter table tblCase drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCase 
		drop column MOSS_DocVersion
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocVersion' and sysobjects.name = N'tblCaseHistory')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCaseHistory') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocVersion')  

		set @sql = N'alter table tblCaseHistory drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCaseHistory
		drop column MOSS_DocVersion
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocURLText' and sysobjects.name = N'tblCase')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCase') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocURLText')  

		set @sql = N'alter table tblCase drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCase 
		drop column MOSS_DocURLText
	end
go

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MOSS_DocURLText' and sysobjects.name = N'tblCaseHistory')
	begin
		declare @default sysname, @sql nvarchar(max)  
		select @default = name  from sys.default_constraints  
		where parent_object_id = object_id('tblCaseHistory') 
			AND type = 'D' 
			AND parent_column_id = (select column_id from sys.columns where object_id = object_id('tblCase') and name = 'MOSS_DocURLText')  

		set @sql = N'alter table tblCaseHistory drop constraint ' + @default 
		exec sp_executesql @sql  
		alter table tblCaseHistory
		drop column MOSS_DocURLText
	end
go

UPDATE tblText Set TextString = 'Välj kund' WHERE Id=958;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Select Customer' WHERE Text_Id=958 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=958 AND Language_Id > 2
GO

-- Ändra storlek på fält
ALTER TABLE tblCase ALTER COLUMN Persons_phone nvarchar(40) not null
GO

ALTER TABLE tblCaseHistory ALTER COLUMN Persons_phone nvarchar(40) not null
GO

-- Ändra storlek på fält
ALTER TABLE tblLink ALTER COLUMN URLAddress nvarchar(150) not null
GO

-- Ändra storlek på fält
ALTER TABLE tblForm ALTER COLUMN FormPath nvarchar(150) null
GO

UPDATE tblText Set TextString = 'När du bifogar en fil måste du även ange en loggpost' WHERE Id=330;
GO

UPDATE tblTextTranslation Set TextTranslation = 'When you attach a file, you must also add a log note' WHERE Text_Id=330 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=330 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Må' WHERE Id=560;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Mo' WHERE Text_Id=560 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=560 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Ti' WHERE Id=682;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Tu' WHERE Text_Id=682 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=682 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'On' WHERE Id=683;
GO

UPDATE tblTextTranslation Set TextTranslation = 'We' WHERE Text_Id=683 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=683 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'To' WHERE Id=343;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Th' WHERE Text_Id=343 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=343 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Fr' WHERE Id=1027;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Fr' WHERE Text_Id=1027 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1027 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Lö' WHERE Id=282;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Sa' WHERE Text_Id=282 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=282 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Sö' WHERE Id=24;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Su' WHERE Text_Id=24 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=24 AND Language_Id > 2
GO

-- Referens från tblCaseType till tblCaseType
if not exists(select * from sysobjects WHERE Name = N'FK_tblCaseType_tblCaseType')
	ALTER TABLE [dbo].[tblCaseType] ADD 
		CONSTRAINT [FK_tblCaseType_tblCaseType] FOREIGN KEY 
			(
				[Parent_CaseType_Id]
			) REFERENCES [dbo].[tblCaseType] (
				[Id]
			)
GO

UPDATE tblText Set TextString = 'Polska' WHERE Id=1143;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Polish' WHERE Text_Id=1143 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1143 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Fakturanummer' WHERE Id=665;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Invoice Number' WHERE Text_Id=665 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=665 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Integration' WHERE Id=716;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Integration' WHERE Text_Id=716 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=716 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Version' WHERE Id=714;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Version' WHERE Text_Id=714 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=714 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Special' WHERE Id=715;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Special' WHERE Text_Id=715 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=715 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Normal' WHERE Id=718;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Normal' WHERE Text_Id=718 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=718 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Överenskommet datum' WHERE Id=867;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Agreed Date' WHERE Text_Id=867 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=867 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Bakgrundsfärg' WHERE Id=1050;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Background color' WHERE Text_Id=1050 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1050 AND Language_Id > 2
GO

ALTER TABLE tblProductArea ALTER COLUMN ProductArea nvarchar(80) not null
GO

UPDATE tblText Set TextString = 'Verifierad' WHERE Id=796;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Verified' WHERE Text_Id=796 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=796 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Verifierad beskrivning' WHERE Id=1043;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Verified description' WHERE Text_Id=1043 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1043 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Synkronisera användare som ingår i organisationsenheten' WHERE Id=1055;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Synchronize users included in the OU' WHERE Text_Id=1055 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1055 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Förälderenhet' WHERE Id=1109;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Parent Unit' WHERE Text_Id=1109 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1109 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Felaktigt användarnamn eller lösenord' WHERE Id=1;
GO

UPDATE tblText Set TextString = 'Italienska' WHERE Id=1117;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Italian' WHERE Text_Id=1117 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=1117 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Avslutsstatus' WHERE Id=259;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Finishing status' WHERE Text_Id=259 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=259 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Kontoaktivitet' WHERE Id=217;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Account Activity' WHERE Text_Id=217 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=217 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'senaste posterna på startsidan' WHERE Id=733;
GO

UPDATE tblTextTranslation Set TextTranslation = 'recent records on start page' WHERE Text_Id=733 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=733 AND Language_Id > 2
GO


UPDATE tblText Set TextString = 'Fortsätt räkna tid om aktuell understatus är vald' WHERE Id=552;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Count time during this substatus' WHERE Text_Id=552 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=552 AND Language_Id > 2
GO

-- Nytt fält
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EMailIdentifier' and sysobjects.name = N'tblCaseFieldSettings')
	begin
		ALTER TABLE tblCaseFieldSettings ADD EMailIdentifier nvarchar(10) NULL
	end
GO


-- Nytt fält i tblCase 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProjectSchedule_Id' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE tblCase ADD ProjectSchedule_Id int NULL		

		ALTER TABLE [dbo].[tblCase] ADD 
			CONSTRAINT [FK_tblCase_tblProjectSchedule] FOREIGN KEY 
			(
				[ProjectSchedule_Id]
			) REFERENCES [dbo].[tblProjectSchedule] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblCaseHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProjectSchedule_Id' and sysobjects.name = N'tblCaseHistory')
	begin
		ALTER TABLE tblCaseHistory ADD ProjectSchedule_Id int NULL		

		ALTER TABLE [dbo].[tblCaseHistory] ADD 
			CONSTRAINT [FK_tblCaseHistory_tblProjectSchedule] FOREIGN KEY 
			(
				[ProjectSchedule_Id]
			) REFERENCES [dbo].[tblProjectSchedule] (
				[Id]
			)		
	end
GO

UPDATE tblText Set TextString = 'Ändringshantering' WHERE Id=1151;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Change Management' WHERE Text_Id=1151 AND Language_Id=2;
GO

UPDATE tblText Set TextString = 'Kopplade ärenden' WHERE Id=357;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Connected Cases' WHERE Text_Id=357 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=357 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Avsluta kopplade ärenden' WHERE Id=152;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Finish connected cases' WHERE Text_Id=152 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=152 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Om' WHERE Id=357;
GO

UPDATE tblTextTranslation Set TextTranslation = 'About' WHERE Text_Id=357 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=357 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Profil' WHERE Id=358;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Profile' WHERE Text_Id=358 AND Language_Id=2;
GO

DELETE FROM tblTextTranslation WHERE Text_Id=358 AND Language_Id > 2
GO

UPDATE tblText Set TextString = 'Ursprung' WHERE Id=239;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Origin' WHERE Text_Id=239 AND Language_Id>1;
GO

-- Ändra storlek på fält
ALTER TABLE tblContractFile ALTER COLUMN FileName nvarchar(250) not null
GO

-- Referens från tblProductArea till tblProductArea 
if not exists(select * from sysobjects WHERE Name = N'FK_tblProductArea_tblProductArea')
	ALTER TABLE [dbo].[tblProductArea ] ADD 
		CONSTRAINT [FK_tblProductArea_tblProductArea] FOREIGN KEY 
			(
				[Parent_ProductArea_Id]
			) REFERENCES [dbo].[tblProductArea] (
				[Id]
			)
GO

-- Referens från tblOU till tblOU
if not exists(select * from sysobjects WHERE Name = N'FK_tblOU_tblOU')
	ALTER TABLE [dbo].[tblOU] ADD 
		CONSTRAINT [FK_tblOU_tblOU] FOREIGN KEY 
			(
				[Parent_OU_Id]
			) REFERENCES [dbo].[tblOU] (
				[Id]
			)
GO

-- Referens från tblFinishingCause till tblFinishingCause
if not exists(select * from sysobjects WHERE Name = N'FK_tblFinishingCause_tblFinishingCause')
	ALTER TABLE [dbo].[tblFinishingCause] ADD 
		CONSTRAINT [FK_tblFinishingCause_tblFinishingCause] FOREIGN KEY 
			(
				[Parent_FinishingCause_Id]
			) REFERENCES [dbo].[tblFinishingCause] (
				[Id]
			)
GO

if not exists(select * from sysobjects WHERE Name = N'tblModule')
	begin
		CREATE TABLE [dbo].[tblModule](
			[Id] [int] NOT NULL,
			[Name] [nvarchar](50) NOT NULL,
			[Description] [nvarchar](500) NULL)
		
		ALTER TABLE [dbo].[tblModule] ADD
 			CONSTRAINT [PK_tblModule] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			) ON [PRIMARY]
	end
GO


if not exists(select * from sysobjects WHERE Name = N'tblUsers_tblModule')
	begin
		CREATE TABLE [dbo].[tblUsers_tblModule](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[User_Id] [int] NOT NULL,
			[Module_Id] [int] NOT NULL,
			[Position] [int] NOT NULL CONSTRAINT [DF_tblUser_tblModule_Position]  DEFAULT ((0)),
			[isVisible] [bit] NOT NULL CONSTRAINT [DF_tblUser_tblModule_isVisible]  DEFAULT ((1)),
			[NumberOfRows] [int] NULL CONSTRAINT [DF_tblUser_tblModule_NumberOfRows]  DEFAULT ((3)))

		ALTER TABLE [dbo].[tblUsers_tblModule] ADD
 			CONSTRAINT [PK_tblUsers_tblModule] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			) ON [PRIMARY]

		ALTER TABLE [dbo].[tblUsers_tblModule] ADD CONSTRAINT [FK_tblUser_tblModule_tblModule] FOREIGN KEY([Module_Id])
		REFERENCES [dbo].[tblModule] ([Id])


		ALTER TABLE [dbo].[tblUsers_tblModule] ADD CONSTRAINT [FK_tblUser_tblModule_tblUsers] FOREIGN KEY([User_Id])
		REFERENCES [dbo].[tblUsers] ([Id])
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnStartPage' and sysobjects.name = N'tblDocument')
	begin
		ALTER TABLE tblDocument ADD ShowOnstartPage bit NOT NULL Default(0)
	end
GO

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'EmailFolder' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD EmailFolder nvarchar(50) NULL			
	end
GO

-- Referens från tblOrderType till tblOrderType
if not exists(select * from sysobjects WHERE Name = N'FK_tblOrderType_tblOrderType')
	ALTER TABLE [dbo].[tblOrderType] ADD 
		CONSTRAINT [FK_tblOrderType_tblOrderType] FOREIGN KEY 
			(
				[Parent_OrderType_Id]
			) REFERENCES [dbo].[tblOrderType] (
				[Id]
			)
GO

UPDATE tblText Set TextString = 'Påminnelse anmälare' WHERE Id=321;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Reminder notifier' WHERE Text_Id=321 AND Language_Id>1;
GO

-- Nytt fält i tblStateSecondary
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ReminderDays' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD ReminderDays int NULL Default(0)			
	end
GO




UPDATE tblText Set TextString = 'Skicka påminnelse efter' WHERE Id=320;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Send reminder after' WHERE Text_Id=320 AND Language_Id>1;
GO

if not exists(select * from sysobjects WHERE Name = N'tblCausingPart')
	begin
		CREATE TABLE [dbo].[tblCausingPart](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Parent_CausingPart_Id] [int] NULL,
			[Name] [nvarchar](100) NOT NULL,
			[Description] [nvarchar](300) NULL,
			[Status] [int] NOT NULL default(1),
			[CreatedDate] [datetime] NOT NULL default(getutcdate()),
			[ChangedDate] [datetime] NOT NULL default(getutcdate()),
			[CustomerId] [int] NOT NULL)

		ALTER TABLE [tblCausingPart] ADD
 			CONSTRAINT [PK_tblCausingPart] PRIMARY KEY CLUSTERED ([Id] ASC)

		ALTER TABLE [dbo].[tblCausingPart]  ADD  
			CONSTRAINT [FK_tblCausingPart_tblCustomer] FOREIGN KEY([CustomerId])
			REFERENCES [dbo].[tblCustomer] ([Id])
	end
GO

-- Nytt fält i tblCase
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CausingPartId' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE tblCase ADD CausingPartId int NULL

		ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblCausingPart] FOREIGN KEY 
			(
				[CausingPartId]
			) REFERENCES [dbo].[tblCausingPart] (
				[Id]
			)
	end
GO

-- Nytt fält i tblCaseHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CausingPartId' and sysobjects.name = N'tblCaseHistory')
	begin
		ALTER TABLE tblCaseHistory ADD CausingPartId int NULL

		ALTER TABLE [dbo].[tblCaseHistory] ADD 
		CONSTRAINT [FK_tblCaseHistory_tblCausingPart] FOREIGN KEY 
			(
				[CausingPartId]
			) REFERENCES [dbo].[tblCausingPart] (
				[Id]
			)
	end
GO

-- Nytt fält i tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HeadOfDepartmentTitle' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD HeadOfDepartmentTitle nvarchar(50) NULL
	end
GO

-- Nytt fält i tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HeadOfDepartmentCity' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD HeadOfDepartmentCity nvarchar(20) NULL
	end
GO

-- Nytt fält i tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HeadOfDepartmentSignature' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD HeadOfDepartmentSignature nvarchar(100) NULL
	end
GO

-- Nytt fält i tblCase
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultOwnerWG_Id' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE tblCase ADD DefaultOwnerWG_Id INT Null


		ALTER TABLE [dbo].[tblCase] ADD 
		CONSTRAINT [FK_tblCase_tblWG] FOREIGN KEY 
			(
				[DefaultOwnerWG_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)
	end
GO

-- Nytt fält i tblCaseHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultOwnerWG_Id' and sysobjects.name = N'tblCaseHistory')
	begin
		ALTER TABLE tblCaseHistory ADD DefaultOwnerWG_Id INT Null


		ALTER TABLE [dbo].[tblCaseHistory] ADD 
		CONSTRAINT [FK_tblCaseHistory_tblWG] FOREIGN KEY 
			(
				[DefaultOwnerWG_Id]
			) REFERENCES [dbo].[tblWorkingGroup] (
				[Id]
			)
	end
GO

-- Nytt fält i tblWorkingGroup 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SendExternalEmailToWGUsers' and sysobjects.name = N'tblWorkingGroup')
	begin
		ALTER TABLE tblWorkingGroup ADD SendExternalEmailToWGUsers INT Not Null Default (0)
	end
GO

-- Nytt fält i tblStateSecondary
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'MailTemplate_Id' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD MailTemplate_Id INT Null


		ALTER TABLE [dbo].[tblStateSecondary] ADD 
		CONSTRAINT [FK_tblStateSecondary_tblMailTemplate] FOREIGN KEY 
			(
				[MailTemplate_Id]
			) REFERENCES [dbo].[tblMailTemplate] (
				[Id]
			)
	end
GO

if not exists(select * from sysobjects WHERE Name = N'tblTextType')
	begin
		CREATE TABLE [dbo].[tblTextType](
			[Id] [int] NOT NULL,
			[TextType] [nvarchar](50) NOT NULL,
			[Status] [int] NOT NULL DEFAULT(1))

		ALTER TABLE [tblTextType] ADD
 			CONSTRAINT [PK_tblTextType] PRIMARY KEY CLUSTERED ([Id] ASC)
	end
GO

-- Nytt fält i tblStateSecondary 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'isDefault' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD isDefault int Not Null Default (0)
	end
GO

-- Nytt fält i tblStateSecondary 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AlternativeStateSecondaryName' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD AlternativeStateSecondaryName nvarchar(50) Null
	end
GO

UPDATE tblText Set TextString = 'Alternativt namn' WHERE Id=415;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Alternative name' WHERE Text_Id=415 AND Language_Id>1;
GO

-- Nytt fält i tblLog
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OverTime' and sysobjects.name = N'tblLog')
	begin
		ALTER TABLE tblLog ADD OverTime int Not Null Default(0)
	end
GO

-- Nytt fält i tblDepartment
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OverTimeAmount' and sysobjects.name = N'tblDepartment')
	begin
		ALTER TABLE tblDepartment ADD OverTimeAmount int Not Null Default(0)
	end
GO

UPDATE tblText Set TextString = 'Övertid' WHERE Id=323;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Over Time' WHERE Text_Id=323 AND Language_Id>1;
GO

UPDATE tblText Set TextString = 'Övertidsbelopp' WHERE Id=325;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Over Time Amount' WHERE Text_Id=325 AND Language_Id>1;
GO

-- Nytt fält i tblGlobalSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ExternalSite' and sysobjects.name = N'tblGlobalSettings')
	begin
		ALTER TABLE tblGlobalSettings ADD ExternalSite nvarchar(100) Null
	end
GO

-- Nytt fält i tblFormFieldValue
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InitialFormFieldValue' and sysobjects.name = N'tblFormFieldValue')
	begin
		ALTER TABLE tblFormFieldValue ADD InitialFormFieldValue nvarchar(2000) Null
	end
GO

UPDATE tblText Set TextString = 'Formulär' WHERE Id=455;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Form' WHERE Text_Id=455 AND Language_Id>1;
GO

-- Nytt fält i tblCaseSolution 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'TemplatePath' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD TemplatePath nvarchar(250) Null
	end
GO

-- Nytt fält i tblCaseSolution 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowInSelfService' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD ShowInSelfService bit Not Null Default(0)
	end
GO

-- Nytt fält i tblFormField
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FormFieldIdentifier' and sysobjects.name = N'tblFormField')
	begin
		ALTER TABLE tblFormField ADD FormFieldIdentifier nvarchar(50) Null
	end
GO

-- Nytt fält i tblSite
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SiteName' and sysobjects.name = N'tblSite')
	begin
		ALTER TABLE tblSite ADD SiteName nvarchar(50) Null
	end
GO

-- Nytt fält i tblFormField
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseField' and sysobjects.name = N'tblFormField')
	begin
		ALTER TABLE tblFormField ADD CaseField nvarchar(50) Null
	end
GO

-- Nytt fält i tblStateSecondary
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingCause_Id' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE tblStateSecondary ADD FinishingCause_Id INT Null


		ALTER TABLE [dbo].[tblStateSecondary] ADD 
		CONSTRAINT [FK_tblStateSecondary_tblFinishingCause] FOREIGN KEY 
			(
				[FinishingCause_Id]
			) REFERENCES [dbo].[tblFinishingCause] (
				[Id]
			)
	end
GO

-- Nytt fält i tblProductArea
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DocumentPath' and sysobjects.name = N'tblProductArea')
	begin
		ALTER TABLE tblProductArea ADD DocumentPath nvarchar(100) Null
	end
GO

-- Nytt fält i tblLinkGroup
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SortOrder' and sysobjects.name = N'tblLinkGroup')
	begin
		ALTER TABLE tblLinkGroup ADD SortOrder int Default(0) NOT NULL		
	end
GO

UPDATE tblText Set TextString = 'Ryska' WHERE Id=549;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Russian' WHERE Text_Id=549 AND Language_Id>1;
GO

UPDATE tblText Set TextString = 'Spanska' WHERE Id=556;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Spanish' WHERE Text_Id=556 AND Language_Id>1;
GO

UPDATE tblText Set TextString = 'Flamländska' WHERE Id=527;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Flemish' WHERE Text_Id=527 AND Language_Id>1;
GO

-- Nytt fält i tblFormFieldValue
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedDate' and sysobjects.name = N'tblFormFieldValue')
	begin
		ALTER TABLE tblFormFieldValue ADD CreatedDate datetime not null default(getutcdate())
	end
GO

-- Nytt fält i tblFormFieldValue
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedDate' and sysobjects.name = N'tblFormFieldValue')
	begin
		ALTER TABLE tblFormFieldValue ADD ChangedDate datetime not null default(getutcdate())
	end
GO

-- Nytt fält i tblCategory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'isEMailDefault' and sysobjects.name = N'tblCategory')
	begin
		ALTER TABLE tblCategory ADD isEMailDefault int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblComputerFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowExternal' and sysobjects.name = N'tblComputerFieldSettings')
	begin
		ALTER TABLE tblComputerFieldSettings ADD ShowExternal int NOT NULL Default(0)			
	end
GO

-- Nytt fält i tblComputerFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowInListExternal' and sysobjects.name = N'tblComputerFieldSettings')
	begin
		ALTER TABLE tblComputerFieldSettings ADD ShowInListExternal int NOT NULL Default(0)			
	end
GO

UPDATE tblText Set TextString = 'h' WHERE Id=411;
GO

UPDATE tblTextTranslation Set TextTranslation = 'h' WHERE Text_Id=411 AND Language_Id>1;
GO

-- Nytt fält i tblText
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedByUser_Id' and sysobjects.name = N'tblText')
	begin
		ALTER TABLE tblText ADD ChangedByUser_Id int NULL		

		ALTER TABLE [dbo].[tblText] ADD 
			CONSTRAINT [FK_tblText_tblUsers] FOREIGN KEY 
			(
				[ChangedByUser_Id]
			) REFERENCES [dbo].[tblUsers] (
				[Id]
			)		
	end
GO

-- Nytt fält i tblTextTranslation
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedDate' and sysobjects.name = N'tblTextTranslation')
	begin
		ALTER TABLE tblTextTranslation ADD CreatedDate datetime NOT NULL DEFAULT getdate()			
	end
GO

-- Nytt fält i tblTextTranslation
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedDate' and sysobjects.name = N'tblTextTranslation')
	begin
		ALTER TABLE tblTextTranslation ADD ChangedDate datetime NOT NULL DEFAULT getdate()			
	end
GO

-- Nytt fält i tblTextTranslation
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedByUser_Id' and sysobjects.name = N'tblTextTranslation')
	begin
		ALTER TABLE tblTextTranslation ADD ChangedByUser_Id int NULL		

		ALTER TABLE [dbo].[tblTextTranslation] ADD 
			CONSTRAINT [FK_tblTextTranslation_tblUsers] FOREIGN KEY 
			(
				[ChangedByUser_Id]
			) REFERENCES [dbo].[tblUsers] (
				[Id]
			)		
	end
GO

-- Ändra storlek på fält
ALTER TABLE tblComputerUserFieldSettings ALTER COLUMN Customer_Id int NULL
GO


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CommunicateWithNotifier' and sysobjects.name = N'tblCustomer')
	begin
		ALTER TABLE tblCustomer ADD CommunicateWithNotifier int NOT NULL Default(1)
	end
GO


-- Ändra storlek på fält
ALTER TABLE tblCase ALTER COLUMN Caption nvarchar(100) NOT NULL
GO

-- Ändra storlek på fält
ALTER TABLE tblCaseHistory ALTER COLUMN Caption nvarchar(100) NOT NULL
GO

-- Lägg till en "default" (Customer_Id IS NULL) kund i tblCaseFieldSettings
declare @CaseFieldSettings_Id int

if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'ReportedBy')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'ReportedBy', 1, 1, 1, 0, '', null, 0, '[#27]')


set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'ReportedBy')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Användar ID')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'User ID')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_Name')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Persons_Name', 1, 0, 1, 0, '', null, 0, '[#3]')


set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_Name')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Anmälare')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Notifier')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_EMail')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Persons_EMail', 1, 1, 1, 0, '', null, 0, '[#8]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_EMail')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'E-post')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'E-mail')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_Phone')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Persons_Phone', 1, 1, 1, 0, '', null, 0, '[#9]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_Phone')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Telefon')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Phone')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_CellPhone')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Persons_CellPhone', 0, 0, 0, 0, '', null, 0, '[#18]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Persons_CellPhone')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Mobil')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Cell Phone')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'UserCode')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'UserCode', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'UserCode')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Ansvarskod')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Orderer Code')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Customer_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Customer_Id', 1, 1, 1, 0, '', null, 0, '[#2]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Customer_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Kund')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Customer')


if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Region_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Region_Id', 0, 0, 0, 0, '', null, 0, '[#2]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Region_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Område')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Region')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Department_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Department_Id', 1, 1, 1, 0, '', null, 0, '[#2]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Department_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Avdelning')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Department')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'OU_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'OU_Id', 1, 1, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'OU_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Enhet')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Unit')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Place')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Place', 1, 1, 1, 0, '', null, 0, '[#24]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Place')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Placering')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Place')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'InventoryNumber')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'InventoryNumber', 1, 0, 1, 0, '', null, 0, '[#24]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'InventoryNumber')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'PC Nummer')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'PC Number')


if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'ComputerType_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'ComputerType_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'ComputerType_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Datortyp')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Computer Type')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'InventoryLocation')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'InventoryLocation', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'InventoryLocation')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Placering')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Place')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'CaseNumber')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'CaseNumber', 0, 0, 0, 0, '', null, 0, '[#1]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'CaseNumber')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Ärende')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Case')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'RegTime')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'RegTime', 1, 0, 0, 0, '', null, 0, '[#16]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'RegTime')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Registreringsdatum')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Registration Date')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'ChangeTime')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'ChangeTime', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'ChangeTime')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Ändringsdatum')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Change Date')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'User_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'User_Id', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'User_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Registrerad av')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Registrated By')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'CaseType_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'CaseType_Id', 1, 1, 1, 0, '', null, 0, '[#25]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'CaseType_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Ärendetyp')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Case Type')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'ProductArea_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'ProductArea_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'ProductArea_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Produktområde')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Product Area')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'System_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'System_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'System_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'System')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'System')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Urgency_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Urgency_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Urgency_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Brådskandegrad')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Urgent degree')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Impact_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Impact_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Impact_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Påverkan')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Impact')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Category_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Category_Id', 0, 0, 0, 0, '', null, 0, '[#26]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Category_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Kategori')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Category')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Supplier_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Supplier_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Supplier_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Leverantör')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Supplier')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'InvoiceNumber')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'InvoiceNumber', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'InvoiceNumber')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Fakturanummer')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Invoice number')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'ReferenceNumber')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'ReferenceNumber', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'ReferenceNumber')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Referensnummer')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Reference Number')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Caption')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Caption', 1, 1, 1, 0, '', null, 0, '[#4]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Caption')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Rubrik')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Caption')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Description')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Description', 1, 0, 1, 0, '', null, 0, '[#5]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Description')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Beskrivning')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Description')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Miscellaneous')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Miscellaneous', 0, 0, 0, 0, '', null, 0, '[#23]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Miscellaneous')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Övrigt')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Other')





if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'ContactBeforeAction')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'ContactBeforeAction', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'ContactBeforeAction')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Telefonkontakt')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Phone Contact')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'SMS')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'SMS', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'SMS')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'SMS')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'SMS')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'AgreedDate')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'AgreedDate', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'AgreedDate')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Överrenskommet datum')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Agreed Date')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Available')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Available', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Available')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Anträffbar')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Available')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Cost')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Cost', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Cost')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Kostnad')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Cost')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Filename')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Filename', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Filename')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Bifogad fil')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Attached File')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'WorkingGroup_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'WorkingGroup_Id', 0, 0, 0, 0, '', null, 0, '[#15]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'WorkingGroup_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Driftgrupp')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Working Group')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'CaseResponsibleUser_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'CaseResponsibleUser_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'CaseResponsibleUser_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Ansvarig')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Responsible')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Performer_User_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Performer_User_Id', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Performer_User_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Handläggare')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Administrator')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Priority_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Priority_Id', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Priority_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Prioritet')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Priority')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Status_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Status_Id', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Status_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Status')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'State')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'StateSecondary_Id')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'StateSecondary_Id', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'StateSecondary_Id')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Understatus')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Sub State')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'PlanDate')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'PlanDate', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'PlanDate')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Planerad åtgärdsdatum')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Planned Action Date')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'WatchDate')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'WatchDate', 1, 0, 1, 0, '', null, 0, '[#21]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'WatchDate')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Bevakningsdatum')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Watch Date')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'Verified')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'Verified', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'Verified')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Verifierad')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Verified')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'VerifiedDescription')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'VerifiedDescription', 0, 1, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'VerifiedDescription')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Verifierad beskrivning')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Verified Description')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'SolutionRate')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'SolutionRate', 0, 1, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'SolutionRate')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Lösningsgrad')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Solution Rate')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Text_Internal')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'tblLog.Text_Internal', 1, 0, 0, 0, '', null, 0, '[#11]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Text_Internal')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Intern notering')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Internal Log Note')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Text_External')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'tblLog.Text_External', 1, 0, 0, 0, '', null, 0, '[#10]')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Text_External')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Extern notering')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'External Log Note')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Charge')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'tblLog.Charge', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Charge')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Debitering')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Debiting')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Filename')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'tblLog.Filename', 1, 0, 1, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'tblLog.Filename')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Bifogad fil')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Attached File')




if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'FinishingDescription')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'FinishingDescription', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'FinishingDescription')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Avslutsbeskrivning')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Finishing Description')



if not exists (select * from tblCaseFieldSettings where customer_id is null and CaseField = 'FinishingDate')
	insert into tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit,
		EmailIdentifier) VALUES (null, 'FinishingDate', 0, 0, 0, 0, '', null, 0, '')

set @CaseFieldSettings_Id = (select id from tblCaseFieldSettings where customer_id is null and CaseField = 'FinishingDate')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=1)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 1, 'Avslutsdatum')

if not exists (select * from tblCaseFieldSettings_tblLang where CaseFieldSettings_Id=@CaseFieldSettings_Id and language_id=2)
	insert into tblCaseFieldSettings_tblLang(CaseFieldSettings_Id, Language_Id, Label)
		values(@CaseFieldSettings_Id, 2, 'Finishing Date')


-- Lägg till en "default" (Customer_Id IS NULL) kund i tblComputerUserFieldSettings
declare @ComputerUserFieldSettings_Id int

if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'UserId')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'UserId', 1, 1, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'UserId')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Användar ID')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'User Id')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Domain_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Domain_Id', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Domain_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Domän')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Domain')





if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'LogonName')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'LogonName', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'LogonName')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Inloggningsnamn')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Logon Name')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'FirstName')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'FirstName', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'FirstName')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Förnamn')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'First Name')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Initials')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Initials', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Initials')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Initialer')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Initials')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'SurName')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'SurName', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'SurName')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Efternamn')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Last Name')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'DisplayName')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'DisplayName', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'DisplayName')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Namn')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Display Name')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Location')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Location', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Location')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Placering')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Location')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Phone')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Phone', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Phone')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Telefon')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Phone')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Cellphone')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Cellphone', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Cellphone')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Mobil')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Cell phone')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Email')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Email', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Email')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'E-post')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'E-mail')





if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'UserCode')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'UserCode', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'UserCode')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Kod')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Number')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'PostalAddress')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'PostalAddress', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'PostalAddress')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Adress')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Address')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'PostalCode')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'PostalCode', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'PostalCode')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Postnummer')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Zip Code')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'City')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'City', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'City')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Ort')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'City')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Title')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Title', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Title')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Titel')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Title')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Region_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Region_Id', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Region_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Område')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Region')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Department_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Department_Id', 1, 1, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Department_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Avdelning')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Department')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'OU')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'OU', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'OU')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Enhet')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Unit')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'OU_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'OU_Id', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'OU_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Organisationsenhet')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Organizational Unit')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Division_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Division_Id', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Division_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Division')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Division')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'ManagerComputerUser_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'ManagerComputerUser_Id', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'ManagerComputerUser_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Chef')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Manager')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'ComputerUserGroup_Id')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'ComputerUserGroup_Id', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'ComputerUserGroup_Id')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Grupp')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Group')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Info')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'Info', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'Info')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Övrigt')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Misc')



if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'OrderPermission')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'OrderPermission', 0, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'OrderPermission')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Beställare')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Orderer')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'RegTime')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'RegTime', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'RegTime')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Skapad datum')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Created Date')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'ChangeTime')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'ChangeTime', 1, 0, 0, 1, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'ChangeTime')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Senast ändrad datum')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Changed Date')




if not exists (select * from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'SyncChangedDate')
	insert into tblComputerUserFieldSettings (Customer_Id, ComputerUserField, Show, [Required], MinLength, ShowInList, LDAPAttribute)
	 VALUES (null, 'SyncChangedDate', 1, 0, 0, 0, '')


set @ComputerUserFieldSettings_Id = (select id from tblComputerUserFieldSettings where customer_id is null and ComputerUserField = 'SyncChangedDate')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=1)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 1, 'Synkroniserad datum')

if not exists (select * from tblComputerUserFS_tblLanguage where ComputerUserFieldSettings_Id=@ComputerUserFieldSettings_Id and language_id=2)
	insert into tblComputerUserFS_tblLanguage(ComputerUserFieldSettings_Id, Language_Id, Label)
		values(@ComputerUserFieldSettings_Id, 2, 'Synchronize Date')


If not exists (select * from tblText where Id = 1213)
	insert into tblText (Id, Textstring) VALUES (1213, 'Informera anmälaren som standard')
GO

If not exists (select * from tblTextTranslation where Text_Id = 1213 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1213, 2, 'Inform notifier as standard')
GO



-- Nytt fält i tblHoliday
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'HolidayName' and sysobjects.name = N'tblHoliday')
   ALTER TABLE tblHoliday ADD HolidayName nvarchar(50) null

GO

-- Nytt fält i tblWatchDateCalendarValue
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WatchDateValueName' and sysobjects.name = N'tblWatchDateCalendarValue')
ALTER TABLE tblWatchDateCalendarValue ADD WatchDateValueName nvarchar(50) null
GO

If not exists (select * from tblText where Id = 1214)
	insert into tblText (Id, Textstring) VALUES (1214, 'Ändrad av')
GO

If not exists (select * from tblTextTranslation where Text_Id = 1214 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1214, 2, 'Changed by')
GO

If not exists (select * from tblText where Id = 1215)
	insert into tblText (Id, Textstring) VALUES (1215, 'Login')
GO

If not exists (select * from tblTextTranslation where Text_Id = 1215 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation)
	SELECT 1215, Id, 'Login' from tblLanguage WHERE Id > 1	
GO

-- Nytt fält i tblForm 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'tblForm')
	begin
		ALTER TABLE tblForm ADD Status int NOT Null Default(1)
	end
GO

-- Nytt fält i tblUsers 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowSolutionTime' and sysobjects.name = N'tblUsers')
	begin
		ALTER TABLE tblUsers ADD ShowSolutionTime int NOT Null Default(1)
	end
GO


UPDATE tblText Set TextString = 'Sessionen har löpt ut. Du kommer att omdirigeras till inloggningssidan.' WHERE Id=545;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Session expired. You will be redirected to login page.' WHERE Text_Id=545 AND Language_Id>1;
GO


UPDATE tblText Set TextString = 'På grund av inaktivitet kommer din session att löpa ut om' WHERE Id=437;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Due to inactivity, your session will expire in' WHERE Text_Id=437 AND Language_Id>1;
GO

UPDATE tblText Set TextString = 'minuter' WHERE Id=407;
GO

UPDATE tblTextTranslation Set TextTranslation = 'minutes' WHERE Text_Id=407 AND Language_Id>1;
GO

UPDATE tblText Set TextString = 'Om du vill fortsätta arbeta bör du spara eller navigera i systemet för att förnya din session.' WHERE Id=403;
GO

UPDATE tblTextTranslation Set TextTranslation = 'If you want to continue working please save or navigate within the system to renew your session.' WHERE Text_Id=403 AND Language_Id>1;
GO

UPDATE tblText Set TextString = 'Visa lösningstid på ärendeöversikten' WHERE Id=554;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Show solution time on Case Overview' WHERE Text_Id=554 AND Language_Id>1;
GO


-- Nytt fält i tblCase 
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RegUserName' and sysobjects.name = N'tblCase')
	begin
		ALTER TABLE tblCase ADD RegUserName nvarchar(200) Null
	end
GO

UPDATE tblText Set TextString = 'Standard vid ärenderegistrering via e-post' WHERE Id=854;
GO

UPDATE tblTextTranslation Set TextTranslation = 'Default when cases are registered by e-mail' WHERE Text_Id=854 AND Language_Id>1;
GO


-- Nytt fält i tblInventoryTypeProperty
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'XMLTag' and sysobjects.name = N'tblInventoryTypeProperty')
	begin
		ALTER TABLE tblInventoryTypeProperty ADD XMLTag nvarchar(200) Null
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'PublicInformation' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD PublicInformation int NOT Null Default(0)
	end
GO


-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD Status int NOT Null Default(1)
	end
GO

-- Nytt fält i tblSystem
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DocumentURL' and sysobjects.name = N'tblSystem')
	begin
		ALTER TABLE tblSystem ADD DocumentURL nvarchar(100) Null
	end
GO
if not exists(select * from tblReport WHERE Id=23)
	begin
		INSERT INTO tblReport(Id) VALUES(23)
	end
GO

-- Nytt fält i tblLink
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseSolution_Id' and sysobjects.name = N'tblLink')
	begin
		ALTER TABLE tblLInk ADD CaseSolution_Id int NULL 

		ALTER TABLE [dbo].[tblLink] ADD 
			CONSTRAINT [FK_tblLink_tblCaseSolution] FOREIGN KEY 
			(
				[CaseSolution_Id]
			) REFERENCES [dbo].[tblCaseSolution] (
				[Id]
			)	
	end
GO 

-- Nytt fält i tblSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DateStyle' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE tblSettings ADD DateStyle int NOT Null Default(121)
	end
GO

-- Nytt fält i tblCustomer
--if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'tblCustomer')
--	begin
--		--ALTER TABLE tblCustomer ADD Status int NOT Null Default(1)
--	end
--GO

If not exists (select * from tblText where Id = 1305)
	insert into tblText (Id, Textstring) VALUES (1305, 'Steg')
GO

If not exists (select * from tblTextTranslation where Text_Id = 1305 and Language_Id = 2)
	INSERT INTO tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1305, 2, 'Step')
GO

update tblmailtemplate_tbllanguage set body = replace(body, '[#99]', '[#98]')
where body like '%![#99]%' ESCAPE '!'
and body not like '%![#98]%' ESCAPE '!'
and mailtemplate_id in (select id from tblmailtemplate where (mailid = 1 or mailid=3 or mailid=4 or mailid=5) )

GO


/*
CREATE tblDocumentDataText if it doesn't exists
*/
IF NOT EXISTS(SELECT * FROM sysobjects WHERE Name = N'tblDocumentDataText')
BEGIN
	CREATE TABLE [dbo].[tblDocumentDataText](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[Disabled] [bit] NULL,
		[FormGuid] [uniqueidentifier] NULL,
		[Customer_Id] [int] NULL,
		[Text] [nvarchar](max) NULL,
		[Description] [nvarchar](50) NULL,
		[TextType] [nvarchar](max) NULL,
		[CreatedDate] [datetime] NULL,
		[Name1] [nvarchar](max) NULL,
		[Operator1] [nvarchar](max) NULL,
		[Value1] [nvarchar](max) NULL,
		[Condition] [nvarchar](50) NULL,
		[Name2] [nvarchar](max) NULL,
		[Operator2] [nvarchar](max) NULL,
		[Value2] [nvarchar](max) NULL,
	 CONSTRAINT [PK_TEMP_tblDocumentDataConditions_Ex2] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblDocumentDataText] ADD  CONSTRAINT [DF_tblDocumentDataText_Disabled]  DEFAULT ((0)) FOR [Disabled]
	ALTER TABLE [dbo].[tblDocumentDataText] ADD  CONSTRAINT [DF_tblDocumentDataText_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

END

/*
CREATE tblDepartmentData if it doesn't exists
*/
GO
IF NOT EXISTS(SELECT * FROM sysobjects WHERE Name = N'tblDepartmentData')
BEGIN

	CREATE TABLE [dbo].[tblDepartmentData](
		[Search_Key] [nvarchar](100) NOT NULL,
		[HeaderName] [nvarchar](100) NULL,
		[FooterName] [nvarchar](100) NULL,
		[EmployerName] [nvarchar](100) NULL,
		[PolishEmployer] [nvarchar](100) NULL,
		[Tel] [nvarchar](100) NULL,
		[Fax] [nvarchar](100) NULL,
		[NIP] [nvarchar](100) NULL,
		[Regon] [nvarchar](100) NULL,
		[KRSNo] [nvarchar](100) NULL,
		[KapitalNo] [nvarchar](100) NULL,
		[INGBankNo] [nvarchar](100) NULL,
	 CONSTRAINT [PK_tblDepartmentData] PRIMARY KEY CLUSTERED 
	(
		[Search_Key] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
END

/*
CREATE tblFormFieldValueHistory if it doesn't exists
*/
GO
IF NOT EXISTS(SELECT * FROM sysobjects WHERE Name = N'tblFormFieldValueHistory')
BEGIN

	CREATE TABLE [dbo].[tblFormFieldValueHistory](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Case_Id] [int] NOT NULL,
		[FormField_Id] [int] NOT NULL,
		[CaseHistory_Id] [int] NOT NULL,
		[FormFieldValue] [nvarchar](2000) NOT NULL,
	 CONSTRAINT [PK_tblFormFieldValueHistory_1] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ECT_Get_Rules]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ECT_Get_Rules]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ECT_Get_Events]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ECT_Get_Events]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblRules_tblEvent]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblRules]'))
ALTER TABLE [dbo].[tblRules] DROP CONSTRAINT [FK_tblRules_tblEvent]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblRules]') AND type in (N'U'))
DROP TABLE [dbo].[tblRules]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblEvent]') AND type in (N'U'))
DROP TABLE [dbo].[tblEvent]
 
/*
DROP Tables that isn't needed
*/
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CaseID]') AND type in (N'U'))
DROP TABLE [dbo].[CaseID]

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Translation]') AND type in (N'U'))
DROP TABLE [dbo].[Translation]

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TEMP_WorkingGroup_Changed]') AND type in (N'U'))
DROP TABLE [dbo].[TEMP_WorkingGroup_Changed]

/*
DROP Columns that isn't needed
*/
GO
IF EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'ECT_Initiator_MailTemplate_Id' and sysobjects.name = N'tblStateSecondary')
BEGIN
	ALTER TABLE tblStateSecondary  DROP COLUMN ECT_Initiator_MailTemplate_Id
END

/*
ADD missing columns tblDepartment
*/
GO
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'StrAddr' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD StrAddr nvarchar(300) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'CloseDay' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD CloseDay nvarchar(300) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'TelNbr' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD TelNbr nvarchar(75) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'Unit' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD Unit nvarchar(100) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'HrManager' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD HrManager nvarchar(150) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'StoreManager' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD StoreManager nvarchar(150) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'Extra' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD Extra nvarchar(1000) null
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'UpdateDate' and sysobjects.name = N'tblDepartment')
	ALTER TABLE tblDepartment ADD UpdateDate smalldatetime null      

/*
ADD missing columns tblFormField
*/
GO
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'HCMData' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD HCMData bit NOT NULL default ((0))
GO

IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'ParentGVFields' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD ParentGVFields bit NOT NULL default ((0))

/*
ADD missing columns tblLinkGroup
*/
GO
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
where syscolumns.name = N'LinkGuid' and sysobjects.name = N'tblLinkGroup')
	ALTER TABLE tblLinkGroup ADD LinkGuid uniqueidentifier null
GO

/*
DROP columns
*/
IF EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'AuthenticateLDAPServer' and sysobjects.name = N'tblSettings')
BEGIN
	ALTER TABLE tblSettings  DROP COLUMN AuthenticateLDAPServer
END

GO

IF OBJECT_ID(N'DF__tblSettin__Authe__0F431ABE', 'D') IS NOT NULL 
    ALTER TABLE dbo.tblSettings DROP CONSTRAINT DF__tblSettin__Authe__0F431ABE
    
GO

IF EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'AuthenticateLDAPServerType' and sysobjects.name = N'tblSettings')
BEGIN
	ALTER TABLE tblSettings  DROP COLUMN AuthenticateLDAPServerType
END

GO

IF OBJECT_ID(N'DF_tblUsers____AllocateCaseMail', 'D') IS NOT NULL 
    ALTER TABLE dbo.tblUsers DROP CONSTRAINT DF_tblUsers____AllocateCaseMail
    
GO

IF EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'___AllocateCaseMail' and sysobjects.name = N'tblUsers')
BEGIN
	ALTER TABLE tblUsers  DROP COLUMN ___AllocateCaseMail
END

GO

IF OBJECT_ID(N'DF_tblStateSecondary_OnHold', 'D') IS NOT NULL 
    ALTER TABLE dbo.tblStateSecondary DROP CONSTRAINT DF_tblStateSecondary_OnHold
    
GO

IF EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'OnHold ' and sysobjects.name = N'tblStateSecondary')
BEGIN
	ALTER TABLE tblStateSecondary  DROP COLUMN OnHold 
END

GO

/*
ADD column to tblFormField
*/
IF NOT EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'SortOrder' and sysobjects.name = N'tblFormField')
BEGIN
	ALTER TABLE tblFormField  ADD SortOrder int not null default ((0))
END

GO

/*
ADD column to tblRegion
*/
IF NOT EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'Code' and sysobjects.name = N'tblRegion')
BEGIN
	ALTER TABLE tblRegion  ADD Code nvarchar(20) null
END

GO

/*
ADD column to tblDepartment
*/
IF NOT EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'Code' and sysobjects.name = N'tblDepartment')
BEGIN
	ALTER TABLE tblDepartment  ADD Code nvarchar(20) null
END

GO

/*
ADD column to tblOU
*/
IF NOT EXISTS (SELECT * FROM syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
WHERE syscolumns.name = N'Code' and sysobjects.name = N'tblOU')
BEGIN
	ALTER TABLE tblOU  ADD Code nvarchar(20) null
END



/*
CREATE tblSSoLog if it doesn't exists
*/
GO
IF NOT EXISTS(SELECT * FROM sysobjects WHERE Name = N'tblSSOLog')
BEGIN

	CREATE TABLE [dbo].[tblSSOLog](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ApplicationId] [nvarchar](100) NULL,
		[NetworkId] [nvarchar](100) NULL,
		[ClaimData] [nvarchar](4000) NULL,
		[CreatedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_tblSSOLog] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblSSOLog] ADD  CONSTRAINT [DF_tblSSOLog_CreateDate]  DEFAULT (getdate()) FOR [CreatedDate]
	
END

/*
CREATE tblActionSetting if it doesn't exists
*/
GO
IF NOT EXISTS(SELECT * FROM sysobjects WHERE Name = N'tblActionSetting')
BEGIN

	CREATE TABLE [dbo].[tblActionSetting](
		[Customer_Id] [int] NOT NULL,
		[ObjectId] [int] NOT NULL,
		[ObjectValue] [nvarchar](800) NOT NULL,
		[ObjectClass] [nvarchar](100) NULL,
		[Visibled] [bit] NOT NULL,
	 CONSTRAINT [PK_tblActionSetting] PRIMARY KEY CLUSTERED 
	(
		[Customer_Id] ASC,
		[ObjectId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblActionSetting]  WITH CHECK ADD  CONSTRAINT [FK_tblActionSetting_tblCustomer] FOREIGN KEY([Customer_Id])
	REFERENCES [dbo].[tblCustomer] ([Id])

	ALTER TABLE [dbo].[tblActionSetting] CHECK CONSTRAINT [FK_tblActionSetting_tblCustomer]
	ALTER TABLE [dbo].[tblActionSetting] ADD  CONSTRAINT [DF_tblMenuSetting_Enable]  DEFAULT ((1)) FOR [Visibled]

END
 
/*
DROP Column tblEvent, tblRules
DROP sp ECT_Get_Events, ECT_Get_Rules
*/
GO