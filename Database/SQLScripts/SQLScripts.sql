-- update DB from 5.3.13 to 5.3.14 version

if (SELECT COLUMNPROPERTY(OBJECT_ID('tblCaseSolution', 'U'), 'RegistrationSource', 'AllowsNull')) = 0
BEGIN
	ALTER TABLE tblCaseSolution ALTER COLUMN RegistrationSource int NULL
END

-- Parent-child cases
if not exists(select * from sysobjects WHERE Name = N'tblParentChildCaseRelations')
BEGIN
	CREATE TABLE [dbo].[tblParentChildCaseRelations](
		[Ancestor_Id] [int] NOT NULL,
		[Descendant_Id] [int] NOT NULL,
		 CONSTRAINT [PK_tblParentChildCases] PRIMARY KEY CLUSTERED 
		(
			[Ancestor_Id] ASC,
			[Descendant_Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	ALTER TABLE [dbo].[tblParentChildCases]  WITH CHECK ADD  CONSTRAINT [FK_tblParentChildCases_tblCase] FOREIGN KEY([Ancestor_Id])
	REFERENCES [dbo].[tblCase] ([Id])
		ON UPDATE CASCADE
		ON DELETE CASCADE;

	ALTER TABLE [dbo].[tblParentChildCases] CHECK CONSTRAINT [FK_tblParentChildCases_tblCase];	

	ALTER TABLE [dbo].[tblParentChildCases]  WITH CHECK ADD  CONSTRAINT [FK_tblParentChildCases_tblCase1] FOREIGN KEY([Descendant_Id])
	REFERENCES [dbo].[tblCase] ([Id]);

	ALTER TABLE [dbo].[tblParentChildCases] CHECK CONSTRAINT [FK_tblParentChildCases_tblCase];
END
GO

IF COL_LENGTH('dbo.tblUsers','CreateSubCasePermission') IS NULL
BEGIN 	 
	ALTER TABLE [dbo].[tblUsers] ADD [CreateSubCasePermission] INT NOT NULL DEFAULT(0);	
END
GO

update tblUsers set  CreateSubCasePermission = 1 where  CreateCasePermission = 1; 
Go

IF COL_LENGTH('dbo.tblsettings','PreventToSaveCaseWithInactiveValue') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblsettings]
	ADD [PreventToSaveCaseWithInactiveValue] int Not Null default(0)
End
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.14'


