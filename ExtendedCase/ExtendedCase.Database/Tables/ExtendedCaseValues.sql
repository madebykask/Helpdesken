CREATE TABLE [dbo].[ExtendedCaseValues](
	[Id] [int]  NOT NULL PRIMARY KEY IDENTITY,
	[ExtendedCaseDataId] [int] NOT NULL,
	[FieldId] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[SecondaryValue] [nvarchar](max) NULL,
    [Properties] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_ExtendedCaseValues_ExtendedCaseData] FOREIGN KEY([ExtendedCaseDataId]) REFERENCES [dbo].[ExtendedCaseData] ([Id])
)

