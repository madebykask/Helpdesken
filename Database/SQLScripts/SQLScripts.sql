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


-- New table tblMail2Ticket

/****** Object:  Table [dbo].[tblMail2Ticket]    Script Date: 2016-05-31 13:31:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

GO

ALTER TABLE [dbo].[tblMail2Ticket] ADD  CONSTRAINT [DF_tblMail2Ticket_EMailAddress]  DEFAULT ('') FOR [EMailAddress]
GO

ALTER TABLE [dbo].[tblMail2Ticket] ADD  CONSTRAINT [DF_tblMail2Ticket_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[tblMail2Ticket] ADD  CONSTRAINT [DF_tblMail2Ticket_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[tblMail2Ticket]  WITH CHECK ADD  CONSTRAINT [FK_tblMail2Ticket_tblCase] FOREIGN KEY([Case_Id])
REFERENCES [dbo].[tblCase] ([Id])
GO

ALTER TABLE [dbo].[tblMail2Ticket] CHECK CONSTRAINT [FK_tblMail2Ticket_tblCase]
GO

ALTER TABLE [dbo].[tblMail2Ticket]  WITH CHECK ADD  CONSTRAINT [FK_tblMail2Ticket_tblLog] FOREIGN KEY([Log_id])
REFERENCES [dbo].[tblLog] ([Id])
GO

ALTER TABLE [dbo].[tblMail2Ticket] CHECK CONSTRAINT [FK_tblMail2Ticket_tblLog]
GO

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

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.24'
