CREATE TABLE [dbo].[ExtendedCaseData](
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY, 
	[ExtendedCaseGuid] [uniqueidentifier]  NOT NULL DEFAULT NEWID(),
	[ExtendedCaseFormId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,	
	CONSTRAINT [FK_ExtendedCaseData_ExtendedCaseForms] FOREIGN KEY([ExtendedCaseFormId]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]))