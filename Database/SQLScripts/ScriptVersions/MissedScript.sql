
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

