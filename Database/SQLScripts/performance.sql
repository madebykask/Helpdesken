
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







