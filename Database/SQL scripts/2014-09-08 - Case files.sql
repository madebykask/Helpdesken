IF COL_LENGTH('dbo.tblCaseFile','UserId') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCaseFile]
	ADD [UserId] INT NULL

	ALTER TABLE [dbo].[tblCaseFile] WITH CHECK ADD CONSTRAINT [FK_tblCaseFile_tblUser] FOREIGN KEY([UserId])
	REFERENCES [dbo].[tblUsers] ([Id])
END

GO