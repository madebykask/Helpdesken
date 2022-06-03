--update DB from 5.3.54 to 5.3.55 version

RAISERROR ('Add Column WarrantyEndDate to tblComputer', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblComputer','WarrantyEndDate') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblComputer]
		ADD [WarrantyEndDate] datetime Null
	End

Go

if not exists(select * from tblReport WHERE Id=18)
	begin
		INSERT INTO tblReport(Id) VALUES(18)
	end
GO

RAISERROR ('Create table tblCaseSolution_tblLanguage', 10, 1) WITH NOWAIT
IF(OBJECT_ID('tblCaseSolution_tblLanguage', 'U') IS NULL)
	Begin

		CREATE TABLE [dbo].[tblCaseSolutionCategory_tblLanguage](
			[Category_Id] [int] NOT NULL,
			[Language_Id] [int] NOT NULL,
			[CaseSolutionCategory] [nvarchar](100) NOT NULL,
		 CONSTRAINT [PK_tblCaseSolutionCategory_tblLanguage] PRIMARY KEY CLUSTERED 
		(
			[Category_Id] ASC,
			[Language_Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblCaseSolutionCategory_tblLanguage] ADD  CONSTRAINT [DF_tblCaseSolutionCategory_tblLanguage_CaseSolutionCategory]  DEFAULT ('') FOR [CaseSolutionCategory]


		ALTER TABLE [dbo].[tblCaseSolutionCategory_tblLanguage]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolutionCategory_tblLanguage_tblCaseSolutionCategory] FOREIGN KEY([Category_Id])
		REFERENCES [dbo].[tblCaseSolutionCategory] ([Id])


		ALTER TABLE [dbo].[tblCaseSolutionCategory_tblLanguage] CHECK CONSTRAINT [FK_tblCaseSolutionCategory_tblLanguage_tblCaseSolutionCategory]


		ALTER TABLE [dbo].[tblCaseSolutionCategory_tblLanguage] WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolutionCategory_tblLanguage_tblLanguage] FOREIGN KEY([Language_Id])
		REFERENCES [dbo].[tblLanguage] ([Id])

		ALTER TABLE [dbo].[tblCaseSolutionCategory_tblLanguage] CHECK CONSTRAINT [FK_tblCaseSolutionCategory_tblLanguage_tblLanguage]

	END
GO

RAISERROR ('Add Column InitiatorFilter to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseFilterFavorite','InitiatorFilter') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblCaseFilterFavorite
		ADD [InitiatorFilter] nvarchar(200) Null
	End

RAISERROR ('Add Column Category to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseFilterFavorite','Category') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblCaseFilterFavorite
		ADD Category nvarchar(200) Null
	End

Go

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.55'
GO