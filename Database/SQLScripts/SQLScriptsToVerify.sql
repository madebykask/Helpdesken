--TODO:
--Add ExtendedCase TABLES
use [dhHelpdeskNG_ExtendedCase]


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
	ALTER TABLE [dbo].[ExtendedCaseForms] ADD [Status] [int] NOT NULL Default(0)
GO



if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'DefaultTab' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE [dbo].tblCaseSolution ADD DefaultTab nvarchar(100) NOT NULL DEFAULT('case-tab')
	end
GO


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ModuleExtendedCase' and sysobjects.name = N'tblSettings')
	begin
		ALTER TABLE [dbo].[tblSettings] ADD [ModuleExtendedCase] int not null Default(0)
	end
GO

/* StateSecondary columns, Make sure they exist in db, these are old ones.... */
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'AlternativeStateSecondaryName' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE [dbo].[tblStateSecondary] ADD [AlternativeStateSecondaryName] [nvarchar](50) NULL
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'StateSecondaryId' and sysobjects.name = N'tblStateSecondary')
	begin
		ALTER TABLE [dbo].[tblStateSecondary] ADD [StateSecondaryId] int not null Default(0)
	end
GO

/* StateSecondary columns, Make sure they exist in db, these are old ones.... */



if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroupId' and sysobjects.name = N'tblWorkingGroup')
	begin
		ALTER TABLE [dbo].[tblWorkingGroup] ADD [WorkingGroupId] int not null Default(0)
	end
GO


--FOR CURRENT RECORD - TEST

----tblComputerUsers - make sure it does not allow null and add newid()
--if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ComputerUserGUID' and sysobjects.name = N'tblComputerUsers')
--begin
--		EXECUTE  sp_executesql  "update tblComputerUsers set ComputerUserGUID = newid() where ComputerUserGUID is null"

--		if not exists(select *
--					  from sys.all_columns c
--					  join sys.tables t on t.object_id = c.object_id
--					  join sys.schemas s on s.schema_id = t.schema_id
--					  join sys.default_constraints d on c.default_object_id = d.object_id
--					  where t.name = 'tblComputerUsers'
--					  and c.name = 'ComputerUserGUID'
--					  and s.name = 'dbo'
--					  and d.name = 'DF_ComputerUserGUID')
--		begin
--			Alter table tblComputerUsers
--			Add constraint DF_ComputerUserGUID default (newid()) For ComputerUserGUID		
--		end		

--		Alter table tblComputerUsers
--		ALTER COLUMN [ComputerUserGUID] uniqueIdentifier NOT NULL
--end
--else
--begin
--	Alter table tblComputerUsers
--	Add ComputerUserGUID uniqueIdentifier NOT NULL CONSTRAINT DF_ComputerUserGUID default (newid())
--end
--GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ExtendedCasePath' and sysobjects.name = N'tblGlobalSettings')
	begin
		ALTER TABLE [dbo].tblGlobalSettings ADD ExtendedCasePath nvarchar(500) NULL 
	end
GO