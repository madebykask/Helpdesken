
BEGIN TRAN

	DECLARE @today DATE = GETDATE()
	DECLARE @now DATETIME = GETDATE()
	DECLARE @cluCode NVARCHAR(5) = 'E901A'--'1190D'--'4B310'
	DECLARE @computerUserCategoryID INT = 1
	DECLARE @customerID INT = 39

	DECLARE @businessUnits TABLE
	(
		[BU_TK] [int] NOT NULL,
		[CLT_CLASS] [varchar](2) NOT NULL,
		[CLUT_TYPE] [varchar](3) NOT NULL,
		[CLU_CODE] [varchar](5) NOT NULL,
		[NAME] [varchar](90) NOT NULL,
		[NAME_SHORT] [varchar](55) NULL,
		[Created] [datetime] NULL,
		[Exported] [datetime] NULL
	)


	INSERT INTO @businessUnits(BU_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, BU.[NAME], BU.NAME_SHORT, Created, Exported)
	SELECT BU.BU_TK, BU.CLT_CLASS, BU.CLU_CODE, BU.CLUT_TYPE, BU.[NAME], BU.NAME_SHORT, BU.Created, BU.Exported FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
	WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO > @today)
	AND BU.VALID_FROM <= @today
	AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @today)
	AND Exported IS NULL


	INSERT INTO tblComputerUsers(UserId, FirstName, ComputerUsersCategoryID)
	SELECT BU.CLUT_TYPE + ' ' + BU.CLU_CODE, BU.[NAME] + + ' (' + BU.CLUT_TYPE + ' ' + BU.CLU_CODE + ')', @computerUserCategoryID FROM @businessUnits BU


	UPDATE BU SET Exported = @now FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
	JOIN @businessUnits BU2 ON BU.BU_TK = BU2.BU_TK
COMMIT


BEGIN TRAN
	DECLARE @customerID INT = 39
	UPDATE CU SET OU_Id = 29494, Department_Id = 45464 FROM tblComputerUsers CU WHERE RegTime = '2017-10-20 15:17:26.440'
COMMIT