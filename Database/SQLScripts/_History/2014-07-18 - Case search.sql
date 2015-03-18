IF COL_LENGTH('dbo.tblCustomerUser','CaseRegistrationDateFilterShow') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseRegistrationDateFilterShow] BIT NOT NULL DEFAULT(0)
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseWatchDateFilterShow') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseWatchDateFilterShow] BIT NOT NULL DEFAULT(0)
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingDateFilterShow') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseClosingDateFilterShow] BIT NOT NULL DEFAULT(0)
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseRegistrationDateStartFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseRegistrationDateStartFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseRegistrationDateEndFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseRegistrationDateEndFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseWatchDateStartFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseWatchDateStartFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseWatchDateEndFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseWatchDateEndFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingDateStartFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseClosingDateStartFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingDateEndFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseClosingDateEndFilter] DATETIME NULL 
END
GO

IF COL_LENGTH('dbo.tblCustomerUser','CaseClosingReasonFilter') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblCustomerUser]
	ADD [CaseClosingReasonFilter] NVARCHAR(50) NULL 
END
GO


DECLARE @customerId INT
DECLARE @customer_cursor CURSOR 
SET @customer_cursor = CURSOR FAST_FORWARD FOR SELECT [Id] FROM [dbo].[tblCustomer]
OPEN @customer_cursor
FETCH NEXT FROM @customer_cursor INTO @customerId
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @CaseFieldId INT
	SET @CaseFieldId = (SELECT [Id] FROM [dbo].[tblCaseFieldSettings] WHERE [CaseField] = 'ClosingReason' AND [Customer_Id] = @customerId)
	IF @CaseFieldId IS NULL
	BEGIN
		INSERT INTO [dbo].[tblCaseFieldSettings] ([Customer_Id], [CaseField])
		VALUES (@customerId, 'ClosingReason')
		SET @CaseFieldId = SCOPE_IDENTITY();

		INSERT INTO [dbo].[tblCaseFieldSettings_tblLang] ([CaseFieldSettings_Id], [Language_Id], [Label])
		VALUES (@CaseFieldId, 1, 'Closing Reason')

		INSERT INTO [dbo].[tblCaseFieldSettings_tblLang] ([CaseFieldSettings_Id], [Language_Id], [Label])
		VALUES (@CaseFieldId, 2, 'Closing Reason')
	END

	FETCH NEXT FROM @customer_cursor INTO @customerId
END

CLOSE @customer_cursor
DEALLOCATE @customer_cursor

