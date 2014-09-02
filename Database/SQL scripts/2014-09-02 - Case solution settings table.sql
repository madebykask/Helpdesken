USE [DH_Support]
GO

/****** Object:  Table [dbo].[tblCaseSolutionFieldSettings]    Script Date: 9/2/2014 10:22:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCaseSolutionFieldSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaseSolution_Id] [int] NOT NULL,
	[FieldName_Id] [int] NOT NULL,
	[Readonly] [int] NOT NULL,
	[Show] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblCaseSolutionFieldSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_Readonly]  DEFAULT ((0)) FOR [Readonly]
GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_Show]  DEFAULT ((0)) FOR [Show]
GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] ADD  CONSTRAINT [DF_tblCaseSolutionFieldSettings_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseSolutionFieldSettings_tblCaseSolution] FOREIGN KEY([CaseSolution_Id])
REFERENCES [dbo].[tblCaseSolution] ([Id])
GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings] CHECK CONSTRAINT [FK_tblCaseSolutionFieldSettings_tblCaseSolution]
GO

ALTER TABLE [dbo].[tblCaseSolutionFieldSettings]
ADD CONSTRAINT [DF_tblCaseSolutionFieldSettings_UNIQUE] UNIQUE (CaseSolution_Id, FieldName_Id)
GO