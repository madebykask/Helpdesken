--update DB from 5.3.58.2 to 5.4.0.0 version


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.4.0.0'
GO

ALTER TABLE [dbo].[tblStateSecondary]
ADD AutocloseDays INT DEFAULT 0;
GO

ALTER TRIGGER [dbo].[TR_CreateCaseNumber] ON [dbo].[tblCase] 
FOR INSERT
AS
	DECLARE @ID AS int
	
	Set @Id = (Select Id FROM Inserted)

	Declare @newCaseNumber Decimal(18,0) 

	BEGIN TRANSACTION

	SELECT @newCaseNumber  = MAX(CaseNumber) + 1 FROM tblCase

	UPDATE tblCase Set CaseNumber = @newCaseNumber WHERE Id=@ID AND CaseNumber=0

	COMMIT TRANSACTION
