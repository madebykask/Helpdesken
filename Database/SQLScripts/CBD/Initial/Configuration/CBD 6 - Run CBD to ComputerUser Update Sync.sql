
DECLARE @customerID INT = 4,
	@ouID INT = NULL,
	@computerUserCategoryGUID UNIQUEIDENTIFIER = 'AD565179-E89D-41C0-8176-2A5B23FFD402'

DECLARE	@return_value int

EXEC	@return_value = [dbo].[CBD_ComputerUser_Sync]
		@customerID,
		@ouID,
		@computerUserCategoryGUID

SELECT	'Return Value' = @return_value

GO
