-- update DB from 5.3.26 to 5.3.27 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ActionLeadTime' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD ActionLeadTime Int not NULL Default(0)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ActionExternalTime' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD ActionExternalTime Int not NULL Default(0)
Go

-- tblSettings 
ALTER TABLE tblSettings ALTER COLUMN LDAPFilter nvarchar(150)
GO


-- New field in tblInventoryTypeProperty
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Unique' and sysobjects.name = N'tblInventoryTypeProperty')
      begin
             ALTER TABLE tblInventoryTypeProperty ADD [Unique] int NOT NULL Default(0)                                              
      end
GO

if not exists(select * from sysobjects WHERE Name = N'tblBR_Rules')
begin
CREATE TABLE [dbo].[tblBR_Rules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Customer_Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Event_Id] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ContinueOnSuccess] [bit] NOT NULL,
	[ContinueOnError] [bit] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[CreatedByUser_Id] [int] NOT NULL,
	[ChangedTime] [datetime] NOT NULL,
	[ChangedByUser_Id] [int] NOT NULL,
 CONSTRAINT [PK_tblBR_Rules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[tblBR_Rules]  WITH CHECK ADD  CONSTRAINT [FK_tblBR_Rules_tblUsers] FOREIGN KEY([CreatedByUser_Id])
REFERENCES [dbo].[tblUsers] ([Id])


ALTER TABLE [dbo].[tblBR_Rules] CHECK CONSTRAINT [FK_tblBR_Rules_tblUsers]


ALTER TABLE [dbo].[tblBR_Rules]  WITH CHECK ADD  CONSTRAINT [FK_tblBR_Rules_tblUsers1] FOREIGN KEY([ChangedByUser_Id])
REFERENCES [dbo].[tblUsers] ([Id])


ALTER TABLE [dbo].[tblBR_Rules] CHECK CONSTRAINT [FK_tblBR_Rules_tblUsers1]

end
Go

if not exists(select * from sysobjects WHERE Name = N'tblBR_RuleConditions')
begin
	CREATE TABLE [dbo].[tblBR_RuleConditions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Rule_Id] [int] NOT NULL,
		[Field_Id] [nvarchar](50) NOT NULL,
		[FromValue] [nvarchar](4000) NOT NULL,
		[ToValue] [nvarchar](4000) NOT NULL,
		[Sequence] [int] NOT NULL,
	 CONSTRAINT [PK_tblBR_RuleConditions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	

	ALTER TABLE [dbo].[tblBR_RuleConditions]  WITH CHECK ADD  CONSTRAINT [FK_tblBR_RuleConditions_tblBR_Rules] FOREIGN KEY([Rule_Id])
	REFERENCES [dbo].[tblBR_Rules] ([Id])
	ON DELETE CASCADE
	

	ALTER TABLE [dbo].[tblBR_RuleConditions] CHECK CONSTRAINT [FK_tblBR_RuleConditions_tblBR_Rules]
	

end
go

if not exists(select * from sysobjects WHERE Name = N'tblBR_RuleActions')
begin
	CREATE TABLE [dbo].[tblBR_RuleActions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Rule_Id] [int] NOT NULL,
		[ActionType_Id] [int] NOT NULL,
		[Sequence] [int] NOT NULL,
	 CONSTRAINT [PK_tblBR_RuleActions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	

	ALTER TABLE [dbo].[tblBR_RuleActions]  WITH CHECK ADD  CONSTRAINT [FK_tblBR_RuleActions_tblBR_Rules] FOREIGN KEY([Rule_Id])
	REFERENCES [dbo].[tblBR_Rules] ([Id])
	ON DELETE CASCADE
	

	ALTER TABLE [dbo].[tblBR_RuleActions] CHECK CONSTRAINT [FK_tblBR_RuleActions_tblBR_Rules]	
end
go

if not exists(select * from sysobjects WHERE Name = N'tblBR_ActionParams')
begin
	CREATE TABLE [dbo].[tblBR_ActionParams](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[RuleAction_Id] [int] NOT NULL,
		[ParamType_Id] [int] NOT NULL,
		[ParamValue] [nvarchar](4000) NOT NULL,
	 CONSTRAINT [PK_tblBR_ActionParams] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	

	ALTER TABLE [dbo].[tblBR_ActionParams]  WITH CHECK ADD  CONSTRAINT [FK_tblBR_ActionParams_tblBR_ActionParams] FOREIGN KEY([RuleAction_Id])
	REFERENCES [dbo].[tblBR_RuleActions] ([Id])
	ON DELETE CASCADE
	

	ALTER TABLE [dbo].[tblBR_ActionParams] CHECK CONSTRAINT [FK_tblBR_ActionParams_tblBR_ActionParams]
end
go

-- Add Created By to Invoice Order
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedByUser_Id' and 
				sysobjects.name = N'tblCaseInvoiceOrder')
begin
   ALTER TABLE tblCaseInvoiceOrder ADD [CreatedByUser_Id] int NULL
end  
go

if (exists(select * from tblCaseInvoiceOrder where CreatedByUser_Id is null))
begin 
   Update tblCaseInvoiceOrder set CreatedByUser_Id = (Select top 1 id from tblUsers)
   Where CreatedByUser_Id is null

   ALTER TABLE tblCaseInvoiceOrder Alter Column [CreatedByUser_Id] int Not NULL
end
go

-- Add Created Time to Invoice Order
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedTime' and 
				sysobjects.name = N'tblCaseInvoiceOrder')
begin
   ALTER TABLE tblCaseInvoiceOrder ADD [CreatedTime] datetime NULL
end  
go

if (exists(select * from tblCaseInvoiceOrder where [CreatedTime] is null))
begin 
   Update tblCaseInvoiceOrder set [CreatedTime] = GETDATE()
   Where [CreatedTime] is null

   ALTER TABLE tblCaseInvoiceOrder Alter Column [CreatedTime] datetime Not NULL
end
go


-- Add Changed By to Invoice Order
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedByUser_Id' and 
				sysobjects.name = N'tblCaseInvoiceOrder')
begin
   ALTER TABLE tblCaseInvoiceOrder ADD [ChangedByUser_Id] int NULL
end  
go

if (exists(select * from tblCaseInvoiceOrder where ChangedByUser_Id is null))
begin 
   Update tblCaseInvoiceOrder set ChangedByUser_Id = (Select top 1 id from tblUsers)
   Where ChangedByUser_Id is null

   ALTER TABLE tblCaseInvoiceOrder Alter Column [ChangedByUser_Id] int Not NULL
end
go

-- Add Changed Time to Invoice Order
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ChangedTime' and 
				sysobjects.name = N'tblCaseInvoiceOrder')
begin
   ALTER TABLE tblCaseInvoiceOrder ADD [ChangedTime] datetime NULL
end  
go

if (exists(select * from tblCaseInvoiceOrder where [ChangedTime] is null))
begin 
   Update tblCaseInvoiceOrder set [ChangedTime] = GETDATE()
   Where [ChangedTime] is null

   ALTER TABLE tblCaseInvoiceOrder Alter Column [ChangedTime] datetime Not NULL
end
go


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.27'

