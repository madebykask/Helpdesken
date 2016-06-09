-- update DB from 5.3.23 to 5.3.24 version

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'GUID' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD GUID uniqueidentifier NOT NULL Default(newid())
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDateFrom' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD FinishingDateFrom datetime NULL                                                                            
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDateUntil' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD FinishingDateUntil datetime NULL                                                                             
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Departments' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD Departments nvarchar(200) NULL                                                                              
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseTypes' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD CaseTypes nvarchar(200) NULL                                                                                   
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProductAreas' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD ProductAreas nvarchar(200) NULL                                                                             
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroups' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD WorkingGroups nvarchar(200) NULL                                                                         
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Selection' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD Selection int NOT NULL Default(5)
                             end
GO

-- Nytt fält i tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Filter' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD Filter int NOT NULL Default(0)
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ScheduleTime' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD ScheduleTime numeric(8,2) NOT NULL Default(0.0)
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Departments' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD Departments nvarchar(200) NULL                                                                
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseTypes' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD CaseTypes nvarchar(200) NULL                                                                     
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProductAreas' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD ProductAreas nvarchar(200) NULL                                                               
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroups' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD WorkingGroups nvarchar(200) NULL                                                           
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Selection' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD Selection int NOT NULL Default(5)
                             end
GO

-- Nytt fält i tblQuestionnaire
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Filter' and sysobjects.name = N'tblQuestionnaire')
                             begin
                                                          ALTER TABLE tblQuestionnaire ADD Filter int NOT NULL Default(0)
                             end
GO



-- New field in tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryPermission' and sysobjects.name = N'tblUsers')
	ALTER TABLE tblUsers ADD InventoryPermission int NOT NULL Default(0)
GO

UPDATE tblusers 
SET InventoryPermission = 1 
WHERE UserGroup_Id = 4


-- New field in tblCaseFieldSettings
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Mail2TicketIdentifier' and sysobjects.name = N'tblCaseFieldSettings')
	ALTER TABLE tblCaseFieldSettings ADD Mail2TicketIdentifier nvarchar(50) NULL
GO

if not exists(select * from sysobjects WHERE Name = N'tblMail2Ticket') 
BEGIN  
	CREATE TABLE [dbo].[tblMail2Ticket](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Case_Id] [int] NULL,
		[Log_id] [int] NULL,
		[EMailAddress] [nvarchar](100) NOT NULL,
		[Type] [nvarchar](10) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_tblMail2Ticket] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	ALTER TABLE [dbo].[tblMail2Ticket] ADD  CONSTRAINT [DF_tblMail2Ticket_EMailAddress]  DEFAULT ('') FOR [EMailAddress]


	ALTER TABLE [dbo].[tblMail2Ticket] ADD  CONSTRAINT [DF_tblMail2Ticket_Type]  DEFAULT ('') FOR [Type]


	ALTER TABLE [dbo].[tblMail2Ticket] ADD  CONSTRAINT [DF_tblMail2Ticket_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]


	ALTER TABLE [dbo].[tblMail2Ticket]  WITH CHECK ADD  CONSTRAINT [FK_tblMail2Ticket_tblCase] FOREIGN KEY([Case_Id])
	REFERENCES [dbo].[tblCase] ([Id])


	ALTER TABLE [dbo].[tblMail2Ticket] CHECK CONSTRAINT [FK_tblMail2Ticket_tblCase]


	ALTER TABLE [dbo].[tblMail2Ticket]  WITH CHECK ADD  CONSTRAINT [FK_tblMail2Ticket_tblLog] FOREIGN KEY([Log_id])
	REFERENCES [dbo].[tblLog] ([Id])


	ALTER TABLE [dbo].[tblMail2Ticket] CHECK CONSTRAINT [FK_tblMail2Ticket_tblLog]

End

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblFormUrl]') AND type in (N'U'))
BEGIN
            SET ANSI_NULLS ON

            SET QUOTED_IDENTIFIER ON

            CREATE TABLE [dbo].[tblFormUrl](
            [Id] [int] NOT NULL,
            [ExternalSite] [bit] NULL,
            [Scheme] [nvarchar](10) NULL,
            [Host] [nvarchar](1000) NULL,
CONSTRAINT [PK_FormUrl] PRIMARY KEY CLUSTERED 
(
            [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

            ALTER TABLE [dbo].[tblFormUrl] ADD  CONSTRAINT [DF_FormUrl_External]  DEFAULT ((0)) FOR [ExternalSite]

END

IF COL_LENGTH('tblForm','FormUrl_Id') IS NULL
BEGIN
   ALTER TABLE tblForm ADD FormUrl_Id int NULL
END

--New fields in tblFormField
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Label' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ALTER COLUMN Label nvarchar(200) NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Show' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD Show int NOT NULL Default(1)
GO


IF COL_LENGTH('tblCaseInvoiceArticle','TextForArticle_Id') IS NULL
BEGIN
    ALTER TABLE tblCaseInvoiceArticle ADD TextForArticle_Id int NULL
END

IF COL_LENGTH('tblGlobalSettings','DhPlusDBVersion') IS NULL
BEGIN
      ALTER TABLE tblGlobalSettings ADD DhPlusDBVersion NVARCHAR(20) NULL
END

IF COL_LENGTH('tblGlobalSettings','SiteUrlV4') IS NULL
BEGIN
       ALTER TABLE tblGlobalSettings ADD SiteUrlV4 NVARCHAR(200) NULL
END

IF COL_LENGTH('tblGlobalSettings','SiteUrlV5') IS NULL
BEGIN
       ALTER TABLE tblGlobalSettings ADD SiteUrlV5 NVARCHAR(200) NULL
END


GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblFormSettings]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[tblFormSettings](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Customer_Id] [int] NULL,
		[TextType_Id] [int] NULL,
		[LogTranslations] [bit] NULL,
	 CONSTRAINT [PK_tblFormSettings] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblFormSettings] ADD  CONSTRAINT [DF_tblFormSettings_TextType_Id]  DEFAULT ((200)) FOR [TextType_Id]

	ALTER TABLE [dbo].[tblFormSettings] ADD  CONSTRAINT [DF_tblFormSettings_LogTranslations]  DEFAULT ((0)) FOR [LogTranslations]
END

GO

IF COL_LENGTH('tblFormSettings','CountryName') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD AreaName NVARCHAR(40) NULL
END

IF COL_LENGTH('tblFormSettings','EmployeeContractXmlPath') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD EmployeeContractXmlPath NVARCHAR(1000) NULL
END

IF COL_LENGTH('tblFormSettings','DateFormat') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD [DateFormat] NVARCHAR(20) NULL
END

IF COL_LENGTH('tblFormSettings','ApiBaseUrl') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD [ApiBaseUrl] NVARCHAR(2000) NULL
END

IF COL_LENGTH('tblFormSettings','ApiUserName') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD [ApiUserName] NVARCHAR(2000) NULL
END

IF COL_LENGTH('tblFormSettings','ApiPassword') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD [ApiPassword] NVARCHAR(2000) NULL
END

IF COL_LENGTH('tblFormSettings','CountryCode') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD [CountryCode] NVARCHAR(10) NULL
END

IF COL_LENGTH('tblFormSettings','CountryPrefix') IS NULL
BEGIN
      ALTER TABLE tblFormSettings ADD [CountryPrefix] NVARCHAR(3) NULL
END


IF COL_LENGTH('tblOU','SearchKey') IS NULL
BEGIN
       ALTER TABLE tblOU ADD SearchKey NVARCHAR(200) NULL
END

IF COL_LENGTH('tblForm','FormXmlName') IS NULL
BEGIN
       ALTER TABLE tblForm ADD FormXmlName NVARCHAR(100) NULL
END

IF COL_LENGTH('tblForm','DataSource') IS NULL
BEGIN
      ALTER TABLE tblForm ADD DataSource int default(1) NULL
END

IF COL_LENGTH('tblFormField','PreloadOrder') IS NULL
BEGIN
      ALTER TABLE tblFormField ADD PreloadOrder int default(999999) NULL
END

IF COL_LENGTH('tblFormFieldValue','FormFieldText') IS NULL
BEGIN
      ALTER TABLE tblFormFieldValue ADD FormFieldText NVARCHAR(2000) NULL
END

IF COL_LENGTH('tblFormFieldValue','InitialFormFieldText') IS NULL
BEGIN
      ALTER TABLE tblFormFieldValue ADD InitialFormFieldText NVARCHAR(2000) NULL
END

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblFormFieldValueHistory]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[tblFormFieldValueHistory](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Case_Id] [int] NOT NULL,
		[FormField_Id] [int] NOT NULL,
		[CaseHistory_Id] [int] NOT NULL,
		[FormFieldValue] [nvarchar](2000) NOT NULL,
		[InitialFormFieldValue] [nvarchar](2000) NULL,
	 CONSTRAINT [PK_tblFormFieldValueHistory_1] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

IF COL_LENGTH('tblFormFieldValueHistory','FormFieldText') IS NULL
BEGIN
      ALTER TABLE tblFormFieldValueHistory ADD FormFieldText NVARCHAR(2000) NULL
END

IF COL_LENGTH('tblFormFieldValueHistory','InitialFormFieldText') IS NULL
BEGIN
      ALTER TABLE tblFormFieldValueHistory ADD InitialFormFieldText NVARCHAR(2000) NULL
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.24'
