

--tblCase
--tblCaseHistory
--tblCaseSolution

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCase' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		Print ('HEJ')
		-- TODO: ÄNDRA KOLUMN TILL NVARCHAR MAX
	End
Go
