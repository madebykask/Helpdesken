-- update DB from 5.3.11 to 5.3.12 version

-- Nytt fält i tblCaseSolution
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RegistrationSource' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD RegistrationSource int Default(0) NOT NULL
GO


if not exists(select * from sysobjects WHERE Name = N'tblCaseStatistics') 
BEGIN  
	CREATE TABLE [dbo].[tblCaseStatistics](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Case_Id] [int] NOT NULL,
		[WasSolvedInTime] [int] NULL,     
	 CONSTRAINT [PK_tblCaseStatistics] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];
	ALTER TABLE [dbo].[tblCaseStatistics]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseStatistics_tblCase] FOREIGN KEY([CaseId])
	REFERENCES [dbo].[tblCase] ([Id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[tblCaseStatistics] CHECK CONSTRAINT [FK_tblCaseStatistics_tblCase];
END

	
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.12'
