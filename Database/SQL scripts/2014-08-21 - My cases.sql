DECLARE @moduleId INT
DECLARE @Position INT
SET @Position = 204

IF NOT EXISTS (SELECT * FROM [dbo].[tblModule] WHERE [Name] = 'My cases')
BEGIN
	INSERT INTO [dbo].[tblModule] ([Name], [Description])
	VALUES ('My cases', 'My cases')	
	
	SET @moduleId = SCOPE_IDENTITY(); 
END

IF @moduleId IS NOT NULL
BEGIN
	DECLARE @userId INT
	DECLARE @user_cursor CURSOR 
	SET @user_cursor = CURSOR FAST_FORWARD FOR SELECT [Id] FROM [dbo].[tblUsers]
	OPEN @user_cursor
	FETCH NEXT FROM @user_cursor INTO @userId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @Id INT
		SET @Id = (SELECT [Id] FROM [dbo].[tblUsers_tblModule] WHERE [User_Id] = @userId AND [Module_Id] = @moduleId)
		
		IF @Id IS NULL
		BEGIN	
			INSERT INTO [dbo].[tblUsers_tblModule] ([User_Id], [Module_Id], [Position], [isVisible], [NumberOfRows])
			VALUES (@userId, @moduleId, @Position, 1, 3)
		END

		FETCH NEXT FROM @user_cursor INTO @userId
	END

	CLOSE @user_cursor
	DEALLOCATE @user_cursor
END