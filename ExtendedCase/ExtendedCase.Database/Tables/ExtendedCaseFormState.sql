CREATE TABLE [dbo].[ExtendedCaseFormState](
	[Id] [int]  NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ExtendedCaseDataId] [int] NOT NULL,
	[TabId] [nvarchar](50) NOT NULL,
	[SectionId] [nvarchar](50) NOT NULL,
	[SectionIndex] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
  CONSTRAINT [FK_ExtendedCaseFormState_ExtendedCaseData] FOREIGN KEY([ExtendedCaseDataId]) REFERENCES [dbo].[ExtendedCaseData] ([Id]))
