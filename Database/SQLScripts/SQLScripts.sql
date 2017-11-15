--update DB from 5.3.34 to 5.3.35 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'SplitToCaseSolutionType' and sysobjects.name = N'tblCaseSolution')
begin

	ALTER TABLE [tblCaseSolution] ADD [SplitToCaseSolutionType] int NOT NULL DEFAULT(0)

end
   

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblCaseSolution_SplitToCaseSolution' AND xtype='U')
BEGIN

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCaseSolution_SplitToCaseSolution](
		[CaseSolution_Id] [int] NOT NULL,
		[SplitToCaseSolution_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblCaseSolutionAncestor_tblCaseSolutionDescendant] PRIMARY KEY CLUSTERED 
	(
		[CaseSolution_Id] ASC,
		[SplitToCaseSolution_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'ExtendedCaseForm_Id' and sysobjects.name = N'tblCase_ExtendedCaseData')
begin

	ALTER TABLE [tblCase_ExtendedCaseData] ADD [ExtendedCaseForm_Id] int 
end


IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceTime' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceTime] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceOvertime' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceOvertime] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceMaterial' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceMaterial] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoicePrice' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoicePrice] bit NOT NULL Default(1)
end


if  exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'SplitToCaseSolutionType' and sysobjects.name = N'tblCaseSolution')
begin
	EXEC sp_rename 'tblCaseSolution.SplitToCaseSolutionType', 'CaseRelationType', 'COLUMN'
end

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.35'