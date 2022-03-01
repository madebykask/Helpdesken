--update DB from 5.3.53 to 5.3.54 version

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

		CREATE TABLE [dbo].[tblCaseSolution_tblLanguage](
			[CaseSolution_Id] [int] NOT NULL,
			[Language_Id] [int] NOT NULL,
			[CaseSolutionName] [nvarchar](50) NOT NULL,
			[ShortDescription] [nvarchar](100) NOT NULL,
			[Information] [nvarchar](MAX) NULL,
		 CONSTRAINT [PK_tblCaseSolution_tblLanguage] PRIMARY KEY CLUSTERED 
		(
			[CaseSolution_Id] ASC,
			[Language_Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage] ADD  CONSTRAINT [DF_tblCaseSolution_tblLanguage_CaseSolutionName]  DEFAULT ('') FOR [CaseSolutionName]


		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage] ADD  CONSTRAINT [DF_tblCaseSolution_tblLanguage_ShortDescription]  DEFAULT ('') FOR [ShortDescription]


		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage] ADD  CONSTRAINT [DF_tblCaseSolution_tblLanguage_Information]  DEFAULT ('') FOR [Information]


		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolution_tblLanguage_tblCaseSolution] FOREIGN KEY([CaseSolution_Id])
		REFERENCES [dbo].[tblCaseSolution] ([Id])


		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage] CHECK CONSTRAINT [FK_tblCaseSolution_tblLanguage_tblCaseSolution]


		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage] WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolution_tblLanguage_tblLanguage] FOREIGN KEY([Language_Id])
		REFERENCES [dbo].[tblLanguage] ([Id])

		ALTER TABLE [dbo].[tblCaseSolution_tblLanguage] CHECK CONSTRAINT [FK_tblCaseSolution_tblLanguage_tblLanguage]

	END
GO

RAISERROR ('Create table tblCaseSolutionCategory_tblLanguage', 10, 1) WITH NOWAIT
IF(OBJECT_ID('tblCaseSolutionCategory_tblLanguage', 'U') IS NULL)
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
  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.54'
GO