--TODO:
--Add ExtendedCase TABLES
use [dhHelpdeskNG_ExtendedCase]

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'[ActiveTab]' and sysobjects.name = N'tblCase')
	begin
		--ALTER TABLE [dbo].[tblCase] ADD [ActiveTab] int NOT NULL DEFAULT(0)
		ALTER TABLE [dbo].[tblCase] ADD [ActiveTab] nvarchar(100)  NULL DEFAULT('case-tab')
	end
GO


if not exists(select * from sysobjects WHERE Name = N'tblCase_ExtendedCaseData')
begin
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCase_ExtendedCaseData](
		[Case_Id] [int] NOT NULL,
		[ExtendedCaseData_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblCase_ExtendedCaseData] PRIMARY KEY CLUSTERED 
	(
		[Case_Id] ASC,
		[ExtendedCaseData_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[tblCase_ExtendedCaseData]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCase_ExtendedCaseData_ExtendedCaseData] FOREIGN KEY([ExtendedCaseData_Id])
	REFERENCES [dbo].[ExtendedCaseData] ([Id])

	ALTER TABLE [dbo].[tblCase_ExtendedCaseData] CHECK CONSTRAINT [FK_tblCase_ExtendedCaseData_ExtendedCaseData]

	ALTER TABLE [dbo].[tblCase_ExtendedCaseData]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCase_ExtendedCaseData_tblCase] FOREIGN KEY([Case_Id])
	REFERENCES [dbo].[tblCase] ([Id])

	ALTER TABLE [dbo].[tblCase_ExtendedCaseData] CHECK CONSTRAINT [FK_tblCase_ExtendedCaseData_tblCase]
end


if not exists(select * from sysobjects WHERE Name = N'tblCaseSolution_ExtendedCaseForms')
begin

	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCaseSolution_ExtendedCaseForms](
		[CaseSolution_Id] [int] NOT NULL,
		[ExtendedCaseForms_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblCaseSolution_ExtendedCaseForms] PRIMARY KEY CLUSTERED 
	(
		[CaseSolution_Id] ASC,
		[ExtendedCaseForms_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_ExtendedCaseForms] FOREIGN KEY([ExtendedCaseForms_Id])
	REFERENCES [dbo].[ExtendedCaseForms] ([Id])

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms] CHECK CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_ExtendedCaseForms]

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms]  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_tblCaseSolution] FOREIGN KEY([CaseSolution_Id])
	REFERENCES [dbo].[tblCaseSolution] ([Id])

	ALTER TABLE [dbo].[tblCaseSolution_ExtendedCaseForms] CHECK CONSTRAINT [FK_tblCaseSolution_ExtendedCaseForms_tblCaseSolution]

end


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Name' and sysobjects.name = N'ExtendedCaseForms')
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Name] nvarchar(100) null
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Guid' and sysobjects.name = N'ExtendedCaseForms')
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Guid] [uniqueidentifier] NULL
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Status' and sysobjects.name = N'ExtendedCaseForms')
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Status] [int] NULL Default(0)
GO

update [ExtendedCaseForms] set [Status] = 0 where [Status] is null
