IF COL_LENGTH('dbo.tblCustomerUser','ShowOnStartPage') IS NOT NULL
BEGIN	
	DECLARE @ConstraintName VARCHAR(256)
	SET @ConstraintName = (
		 SELECT [obj].[name]
		 FROM [sys].[columns] col 
			LEFT OUTER JOIN [sys].[objects] obj ON obj.[object_id] = col.[default_object_id] 
			AND obj.[type] = 'D' 
		 WHERE col.[object_id] = OBJECT_ID('tblCustomerUser') 
			AND obj.[name] IS NOT NULL
			AND col.[name] = 'ShowOnStartPage'
	) 

	IF(@ConstraintName IS NOT NULL)
	BEGIN
		EXEC ('ALTER TABLE [dbo].[tblCustomerUser] DROP CONSTRAINT [' + @ConstraintName + ']')
	END
		
	ALTER TABLE [dbo].[tblCustomerUser] 
	ADD CONSTRAINT [DF_tblCustomerUser_ShowOnStartPage] DEFAULT ((1)) FOR [ShowOnStartPage]
END
GO 