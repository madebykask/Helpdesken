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
        WHEN BlockedEmailRecipients IS NULL THEN 'noreply'
        ELSE CONCAT(BlockedEmailRecipients, ';noreply')
    END
WHERE CHARINDEX('noreply', BlockedEmailRecipients) = 0 OR BlockedEmailRecipients IS NULL;
Go


IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_tblFormFieldValueHistory_Case_Id'
        AND object_id = OBJECT_ID('tblFormFieldValueHistory')
)
BEGIN
    PRINT 'Index exists.'
END
ELSE
BEGIN
    CREATE INDEX IX_tblFormFieldValueHistory_Case_Id ON tblFormFieldValueHistory(Case_Id);
END
GO

RAISERROR ('Add Column ErrorMailTo to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','ErrorMailTo') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD ErrorMailTo nvarchar(200) null
	End
Go

-- #15156 - Ändra från ntext till nvarchar(max)

IF EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[tblCase]'))
	DROP FULLTEXT INDEX ON [dbo].[tblCase]
GO

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCase' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		alter table tblcase alter column [Description] nvarchar(MAX) not null
	End
Go

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCaseSolution' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		alter table tblCaseSolution alter column [Description] nvarchar(MAX) null
	End
Go

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCaseHistory' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		alter table tblCaseHistory alter column [Description] nvarchar(MAX) not null
	End
Go

IF EXISTS (SELECT * from sys.fulltext_catalogs AS c where c.[name] = 'SearchCasesFTS')
BEGIN
	CREATE FULLTEXT INDEX ON dbo.tblCase
  (   
	Place Language 1033,   
	Persons_Name Language 1033,   
	Persons_EMail Language 1033,
	Caption Language 1033,	  
	Persons_Phone Language 1033,
	[Description] Language 1033,
	Miscellaneous Language 1033,
	ReportedBy Language 1033,
	InventoryNumber Language 1033,
	Available Language 1033,
	Persons_CellPhone Language 1033,
	InventoryType Language 1033,
	InventoryLocation Language 1033,
	InvoiceNumber Language 1033,
	UserCode Language 1033,
	ReferenceNumber Language 1033,
	VerifiedDescription Language 1033,
	RegUserName Language 1033,
	CostCentre Language 1033
  )  
  KEY INDEX PK_tblCase  
  ON SearchCasesFTS
          WITH STOPLIST = SYSTEM, CHANGE_TRACKING AUTO;  
END
GO


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.4.0.0'
GO


