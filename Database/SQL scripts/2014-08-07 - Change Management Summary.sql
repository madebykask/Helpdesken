

IF NOT EXISTS (SELECT * FROM [dbo].[tblModule] WHERE [Name] = 'Change Management')
BEGIN
	INSERT INTO [dbo].[tblModule] ([Name], [Description])
	VALUES ('Change Management', 'Change Management')
	
	DELETE FROM [dbo].[tblUsers_tblModule]
END