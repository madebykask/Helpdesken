--update DB from 5.3.58.2 to 5.4.0.0 version

RAISERROR ('Add Column AutocloseDays to tblStateSecondary', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblStateSecondary','AutocloseDays') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblStateSecondary]
		ADD AutocloseDays INT DEFAULT 0
	End
Go

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

Go

RAISERROR ('Alter tblGDPRDataPrivacyFavorite.ProductAreas from 256 nvarchar to 1000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','ProductAreas') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblGDPRDataPrivacyFavorite]
		ALTER COLUMN ProductAreas nvarchar(1000) NULL
	End
Go

RAISERROR ('Alter tblGDPRDataPrivacyFavorite.CaseTypes from 256 nvarchar to 1000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','CaseTypes') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblGDPRDataPrivacyFavorite]
		ALTER COLUMN CaseTypes nvarchar(1000) NULL
	End
Go

RAISERROR ('Alter tblFAQ.Answer from 2000 nvarchar to 4000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('tblFAQ','Answer') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblFAQ]
		ALTER COLUMN Answer nvarchar(4000) Not NULL
	End
Go

RAISERROR ('Alter tblFAQ.Answer_Internal from 1000 nvarchar to 4000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('tblFAQ','Answer_Internal') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblFAQ]
		ALTER COLUMN Answer_Internal nvarchar(4000) Not NULL
	End
Go

UPDATE [dbo].[tblSettings]
SET BlockedEmailRecipients = 
    CASE 
        WHEN BlockedEmailRecipients IS NULL THEN ';noreply'
        ELSE CONCAT(BlockedEmailRecipients, ';noreply')
    END
WHERE CHARINDEX(';noreply', BlockedEmailRecipients) = 0 OR BlockedEmailRecipients IS NULL;
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.4.0.0'
GO


