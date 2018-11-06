-- if the procedure does not exist create a placeholder
if not exists (select * from sys.objects where name = N'CBD_Get_TaxReg' and type = N'P') 
begin
    exec('create procedure CBD_Get_TaxReg as RAISERROR (''CBD_Get_TaxReg not defined'', 16, 1);');
end
go

ALTER PROCEDURE [dbo].[CBD_Get_TaxReg] 
	@cluCode NVARCHAR(MAX) 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @now DATE = GETDATE()

     ;WITH LatestBusinessUnits (CLU_CODE, NAME, VALID_TO, VALID_FROM, DELETE_DATE) as (
		SELECT TOP 1 CLU_CODE, NAME, VALID_TO, VALID_FROM, DELETE_DATE
		FROM CBD_IN_CEM_BUSINESS_UNIT_T 
		WHERE (CLUT_TYPE + ' ' + CLU_CODE) = @cluCode	
		ORDER BY BU_TK DESC
	)
     SELECT X.BUTRGH_TK, X.NAME, X.TAX_REG_NO, X.TRGT_TYPE, X.SRC_NAME_INS, '(' + X.TRGT_TYPE + ') ' + X.TAX_REG_NO  as FormattedTax 
	FROM 
	(
		SELECT BU.[NAME], T.*, ROW_NUMBER() OVER (PARTITION BY T.CLU_CODE ORDER BY CASE WHEN T.TRGT_TYPE = 'DV' THEN 1 ELSE 0 END DESC, T.TRGT_TYPE) R 
		FROM LatestBusinessUnits BU
		  INNER JOIN CBD_IN_CEM_E_BU_TAX_REG_T T ON T.CLU_CODE = BU.CLU_CODE
		WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO > @now)
		AND (T.VALID_TO IS NULL OR T.VALID_TO > @now)
		AND BU.VALID_FROM <= @now
		AND T.VALID_FROM <= @now
		AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @now)
		AND (T.DELETE_DATE IS NULL OR T.DELETE_DATE > @now)
	) X
	WHERE (X.CLUT_TYPE + ' ' + X.CLU_CODE) = @cluCode	
	ORDER BY X.R
END
GO


-- if the procedure does not exist create a placeholder
if not exists (select * from sys.objects where name = N'CBD_ComputerUser_Sync' and type = N'P') 
begin
    exec('create procedure CBD_ComputerUser_Sync as RAISERROR (''CBD_ComputerUser_Sync not defined'', 16, 1);');
end
go

ALTER PROCEDURE [dbo].[CBD_ComputerUser_Sync] 
(
	@customerID INT,
	@ouID INT,
	@computerUserCategoryGUID UNIQUEIDENTIFIER -- make sure category has associated ExtendedCaseFormId and Solution
)
AS
BEGIN

      DECLARE @now DATETIME = GETUTCDATE(),
	        @today DATE = GETUTCDATE(),
	        @departmentId NVARCHAR(MAX) = 'IKEA Vendor' -- do not change - Departments are created with special script
  

	DECLARE @computerUserCategoryID INT
	SELECT @computerUserCategoryID = ID FROM tblComputerUsersCategory C WHERE C.ComputerUsersCategoryGuid = @computerUserCategoryGUID

	RAISERROR('Declaring temporary tables ', 10, 1) WITH NOWAIT

	DECLARE @activeBU TABLE
	(
		PK INT NOT NULL PRIMARY KEY, -- CBD_IN_CEM_BUSINESS_UNIT_T.BU_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_TO
		[Name] NVARCHAR(MAX),
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLU_CODE		-- '0012A'
		Exported DATETIME NULL,
		Unique (CLT_CLASS, CLUT_TYPE, CLU_CODE)
	)

	DECLARE @activeAddr TABLE
	(
		PK INT NOT NULL PRIMARY KEY,	-- CBD_IN_CEM_BU_ADDRESS_T.BUAH_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BU_ADDRESS_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_ADDRESS_T.VALID_TO
		Country VARCHAR(10),		-- CBD_IN_CEM_BU_ADDRESS_T.GA_CODE_CTY		-- 'DE'
		SeqNo INT,				-- CBD_IN_CEM_BU_ADDRESS_T.SEQ_NO			-- 1,2,3
		AdtType VARCHAR(5),			-- CBD_IN_CEM_BU_ADDRESS_T.ADT_TYPE			-- 'MAIN'
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_ADDRESS_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_ADDRESS_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_ADDRESS_T.CLU_CODE			-- '0012A'
		Exported DATETIME NULL,      	-- CBD_IN_CEM_BU_ADDRESS_T.Exported				
		Unique (CLT_CLASS, CLUT_TYPE, CLU_CODE)
	)

	DECLARE @activeEmail TABLE
	(
		PK INT NOT NULL PRIMARY KEY,	-- CBD_IN_CEM_BU_MEDIA_T.BUAH_TK
		Email NVARCHAR(115),		-- CBD_IN_CEM_BU_MEDIA_T.ADDRESS
		ValidFrom DATETIME NOT NULL,-- CBD_IN_CEM_BU_MEDIA_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.VALID_TO			
		SeqNo INT,					-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO				-- 1,2,3
		SeqNoAddr INT,				-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO_ADDR			
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_MEDIA_T.CLT_CLASS			-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_MEDIA_T.CLUT_TYPE			-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_MEDIA_T.CLU_CODE			-- '0012A'
		MedtType VARCHAR(10),		-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO_ADDR		-- 'PHONE' | 'EMAIL'
		Exported DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.Exported
	     Unique (CLT_CLASS, CLUT_TYPE, CLU_CODE)
	)

	--NEW: 
	DECLARE @activePhone TABLE
	(
		PK INT NOT NULL PRIMARY KEY,	-- CBD_IN_CEM_BU_MEDIA_T.BUAH_TK
		Phone NVARCHAR(115),		-- CBD_IN_CEM_BU_MEDIA_T.ADDRESS
		ValidFrom DATETIME NOT NULL,  -- CBD_IN_CEM_BU_MEDIA_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.VALID_TO			
		SeqNo INT,				-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO			-- 1,2,3
		SeqNoAddr INT,				-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO_ADDR			
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_MEDIA_T.CLT_CLASS			-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_MEDIA_T.CLUT_TYPE			-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_MEDIA_T.CLU_CODE			-- '0012A'
		MedtType VARCHAR(10),		-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO_ADDR		-- 'PHONE'!
		Exported DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.Exported
		Unique (CLT_CLASS, CLUT_TYPE, CLU_CODE)
	)
	
	DECLARE @activeBank TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.BUTRGH_TK	
	
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLU_CODE		-- '0012A'
		Exported DATETIME NULL,		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.Exported
		AccountNo VARCHAR(50)		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.ACCOUNT_NO			
	)


	DECLARE @inactiveBU TABLE
	(
		PK INT NOT NULL PRIMARY KEY,	-- CBD_IN_CEM_BUSINESS_UNIT_T.BUAH_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_TO	
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BUSINESS_UNIT_T.DELETED_DATE
	)

	DECLARE @inactiveBU_ADDR TABLE
	(
		PK INT NOT NULL PRIMARY KEY,	-- CBD_IN_CEM_BU_ADDRESS_T.BUAH_TK
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_ADDRESS_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_ADDRESS_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_ADDRESS_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BU_ADDRESS_T.DELETED_DATE
	)

	DECLARE @inactiveBU_M TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_MEDIA_T.BUAH_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BU_MEDIA_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.VALID_TO	
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_MEDIA_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_MEDIA_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_MEDIA_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.DELETED_DATE
		MEDT_TYPE varchar(10)	     -- CBD_IN_CEM_BU_MEDIA_T.MEDT_TYPE:     Email|Tel
	)
	
	DECLARE @inactiveBU_BANK TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.BUAH_TK
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.DELETED_DATE
	)	

	DECLARE @insertedCU TABLE
	(
		ID INT,
		UserId NVARCHAR(50), 
		FirstName NVARCHAR(50), 
		Phone NVARCHAR(50), 
		Email NVARCHAR(100), 
		[Status] INT,
		ComputerUsersCategoryID INT, 
		Customer_Id INT, 
		Department_Id INT, 
		OU_Id INT
	)

	DECLARE @updatedCU_BU TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		ComputerUserCategoryID INT,
		[Status] INT,
		Department_Id INT,
		Before_UserId NVARCHAR(50),
		Before_Status INT,
		Before_FirstName NVARCHAR(50),
		Before_ComputerUserCategoryID INT,
		Before_Department_Id INT
	)

	DECLARE @updatedCU_M TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Email NVARCHAR(100),
		Before_Email NVARCHAR(100)
	)

	DECLARE @updatedCU_Tel TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Phone NVARCHAR(50),
		Before_Phone NVARCHAR(50)
	)

	DECLARE @updatedCU_ABANK TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		AccountNo NVARCHAR(50),
		Before_AccountNo NVARCHAR(50)
	)

	DECLARE @updatedCU_ADDR TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Department_Id INT,
		Before_Department_Id INT
	)

	DECLARE @deactivatedBU TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		[Status] NVARCHAR(100),
		[Before_Status] NVARCHAR(100)
	)

	DECLARE @deactivatedAddr TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Department_Id INT,
		Before_Department_Id INT
	)

	DECLARE @deactivatedEmail TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Email NVARCHAR(100),
		Before_Email NVARCHAR(100)
	)
		
	DECLARE @deactivatedPhone TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Phone NVARCHAR(100),
		Before_Phone NVARCHAR(100)
	)  

	DECLARE @deactivatedBank TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		AccountNo NVARCHAR(50),
		Before_AccountNo NVARCHAR(50)
	)

	DECLARE @expiredBU TABLE
	(
		BU_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		[NAME] VARCHAR(90),
		VALID_FROM DATETIME,
		VALID_TO DATETIME,
		DELETE_DATE DATETIME
	)

	DECLARE @expiredBU_M TABLE
	(
		BUAH_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		[ADDRESS] VARCHAR(90),
		SEQ_NO INT,
		MEDT_TYPE VARCHAR(10),
		VALID_FROM DATETIME,
		VALID_TO DATETIME,
		DELETE_DATE DATETIME
	)

	--DECLARE @expiredBU_BANK TABLE
	--(
	--	BUBNKAH_TK INT,
	--	CLT_CLASS VARCHAR(2),
	--	CLUT_TYPE VARCHAR(3), 
	--	CLU_CODE VARCHAR(5),
	--	ACCOUNT_NO VARCHAR(35),
	--	DELETE_DATE DATETIME
	--)

	DECLARE @exportedBU TABLE
	(
		BU_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		[NAME] VARCHAR(90),
		VALID_FROM DATETIME,
		VALID_TO DATETIME,
		DELETE_DATE DATETIME
	)

	DECLARE @exportedBU_M TABLE
	(
		BUAH_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		[ADDRESS] VARCHAR(115),
		SEQ_NO INT,
		MEDT_TYPE VARCHAR(10),
		VALID_FROM DATETIME,
		VALID_TO DATETIME,
		DELETE_DATE DATETIME
	)

	DECLARE @exportedBU_ADDR TABLE
	(
		BUAH_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		GA_CODE_CTY VARCHAR(10),
		VALID_FROM DATETIME,
		VALID_TO DATETIME,
		DELETE_DATE DATETIME
	)

	DECLARE @exportedBU_BANK TABLE
	(
		BUBNKAH_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		ACCOUNT_NO VARCHAR(35),
		DELETE_DATE DATETIME
	)
		   
	-- Active business units
	RAISERROR('Retrieveing active business units', 10, 1) WITH NOWAIT
	INSERT INTO @activeBU(PK, ValidFrom, ValidTo, [Name], CLT_CLASS, CLUT_TYPE, CLU_CODE, Exported)
	SELECT BU.BU_TK, BU.VALID_FROM, BU.VALID_TO, BU.[NAME], BU.CLT_CLASS, BU.CLUT_TYPE, BU.CLU_CODE, BU.Exported 
	FROM 
	(
	    -- select only last among duplicate records: R = 1
	    SELECT *, ROW_NUMBER() OVER (PARTITION BY CLUT_TYPE, CLU_CODE ORDER BY BU_TK DESC) R
	    FROM CBD_IN_CEM_BUSINESS_UNIT_T
	    WHERE VALID_FROM <= @today 
	    AND (VALID_TO IS NULL OR VALID_TO >= @today)      -- valid date range
	    AND (DELETE_DATE IS NULL OR DELETE_DATE > @today) -- delete day 	
	) BU	  
	WHERE BU.R = 1

	--test
	RAISERROR('Retrieveing active business unit addresses', 10, 1) WITH NOWAIT
	INSERT INTO @activeAddr(PK, ValidFrom, ValidTo, Country, SeqNo, AdtType, CLT_CLASS, CLUT_TYPE, CLU_CODE, Exported)
	SELECT ADDR.BUAH_TK, ADDR.VALID_FROM, ADDR.VALID_TO, ADDR.GA_CODE_CTY, ADDR.SEQ_NO, ADDR.ADT_TYPE, ADDR.CLT_CLASS, ADDR.CLUT_TYPE, ADDR.CLU_CODE, ADDR.Exported 
	FROM 
	(
		-- Select latest record from range 
		SELECT *, ROW_NUMBER() OVER (PARTITION BY  CLT_CLASS, CLUT_TYPE, CLU_CODE  ORDER BY CASE WHEN ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, BUAH_TK DESC) R
		FROM CBD_IN_CEM_BU_ADDRESS_T 
		WHERE (VALID_TO IS NULL OR VALID_TO >= @today)     -- valid date range
		AND VALID_FROM <= @today                                  
		AND (DELETE_DATE IS NULL OR DELETE_DATE > @today)  -- delete date
	) ADDR
	WHERE ADDR.R = 1 	
     								
	-- Active business unit emails
	RAISERROR('Retrieveing active business unit address Email', 10, 1) WITH NOWAIT
	INSERT INTO @activeEmail(PK, Email, ValidFrom, ValidTo, SeqNo, SeqNoAddr, CLT_CLASS, CLUT_TYPE, CLU_CODE, MedtType, Exported)
	SELECT E.BUAH_TK, E.[ADDRESS], E.VALID_FROM, E.VALID_TO, E.SEQ_NO, E.SEQ_NO_ADDR, E.CLT_CLASS, E.CLUT_TYPE, E.CLU_CODE, E.MEDT_TYPE, E.Exported 
	FROM 
	(
		SELECT M.BUAH_TK, M.[ADDRESS], M.VALID_FROM, M.VALID_TO, M.SEQ_NO, M.SEQ_NO_ADDR, M.CLT_CLASS, M.CLUT_TYPE, M.CLU_CODE, M.MEDT_TYPE, M.Exported, M.DELETE_DATE,
		ROW_NUMBER() OVER (PARTITION BY  M.CLT_CLASS, M.CLUT_TYPE, M.CLU_CODE , M.BUAH_TK ORDER BY CASE WHEN M.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, M.SEQ_NO) R 
		FROM CBD_IN_CEM_BU_MEDIA_T M
		INNER JOIN @activeAddr ADDR ON M.BUAH_TK = ADDR.PK 
		WHERE M.MEDT_TYPE = 'EMAIL'
		AND (M.VALID_TO IS NULL OR M.VALID_TO >= @today)
		AND M.VALID_FROM <= @today
	     AND (M.DELETE_DATE IS NULL OR M.DELETE_DATE > @today)	
	) E
     WHERE E.R = 1	

	-- Active business unit phones
	RAISERROR('Retrieveing active business unit address Phone', 10, 1) WITH NOWAIT
	INSERT INTO @activePhone(PK, Phone, ValidFrom, ValidTo, SeqNo, SeqNoAddr, CLT_CLASS, CLUT_TYPE, CLU_CODE, MedtType, Exported)
     SELECT E.BUAH_TK, E.[ADDRESS], E.VALID_FROM, E.VALID_TO, E.SEQ_NO, E.SEQ_NO_ADDR, E.CLT_CLASS, E.CLUT_TYPE, E.CLU_CODE, E.MEDT_TYPE, E.Exported 
	FROM 
	(
		SELECT M.BUAH_TK, M.[ADDRESS], M.VALID_FROM, M.VALID_TO, M.SEQ_NO, M.SEQ_NO_ADDR, M.CLT_CLASS, M.CLUT_TYPE, M.CLU_CODE, M.MEDT_TYPE, M.Exported, M.DELETE_DATE,
		ROW_NUMBER() OVER (PARTITION BY  M.CLT_CLASS, M.CLUT_TYPE, M.CLU_CODE, M.BUAH_TK ORDER BY CASE WHEN M.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, M.SEQ_NO) R 
		FROM CBD_IN_CEM_BU_MEDIA_T M 
		INNER JOIN @activeAddr ADDR ON M.BUAH_TK = ADDR.PK -- filters out by latest address record
		WHERE M.MEDT_TYPE = 'TEL'
		AND (M.VALID_TO IS NULL OR M.VALID_TO >= @today)
		AND M.VALID_FROM <= @today
	     AND (M.DELETE_DATE IS NULL OR M.DELETE_DATE > @today)	
	) E
	WHERE E.R = 1	

	-- Active bank account
	RAISERROR('Retrieveing active bank account', 10, 1) WITH NOWAIT
	INSERT INTO @activeBank(PK, CLT_CLASS, CLUT_TYPE, CLU_CODE, AccountNo, Exported)
	SELECT X.BUBNKAH_TK, X.CLT_CLASS, X.CLUT_TYPE, X.CLU_CODE, X.ACCOUNT_NO, X.Exported 
	FROM 
	(
		SELECT BU.NAME, B.*, ROW_NUMBER() OVER (PARTITION BY B.CLU_CODE ORDER BY B.BUBNKAH_TK DESC) R 
		FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
		  INNER JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T B ON B.CLU_CODE = BU.CLU_CODE
		WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO >= @today)
		AND BU.VALID_FROM <= @today
		AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @today)
		AND (B.DELETE_DATE IS NULL OR B.DELETE_DATE > @today)
	) X
	WHERE X.R = 1

	-- Get all business unit records that is inactive and not yet marked as expired (having no address equals inactive too)
	RAISERROR('Retrieveing all business unit records that is inactive and not yet marked as expired (having no address equals inactive too', 10, 1) WITH NOWAIT
	INSERT INTO @inactiveBU(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted, ValidFrom, ValidTo)
	SELECT BU.BU_TK, BU.CLU_CODE, BU.CLUT_TYPE, BU.CLT_CLASS, BU.DELETE_DATE, BU.VALID_FROM, BU.VALID_TO 
	FROM 
	(
	    -- select only last BU among duplicate records: R = 1
	    SELECT *, ROW_NUMBER() OVER (PARTITION BY CLUT_TYPE, CLU_CODE ORDER BY BU_TK DESC) R
	    FROM CBD_IN_CEM_BUSINESS_UNIT_T 
	    WHERE (VALID_TO < @today OR DELETE_DATE <= @today)
	    AND Expired IS NULL
	) BU	  
	WHERE BU.R = 1		

	-- TODO: Check if we shall select only the latest record?!
	-- Get all address records that is inactive and not yet marked as expired
	RAISERROR('Retrieveing all address records that is inactive and not yet marked as expired', 10, 1) WITH NOWAIT
	INSERT INTO @inactiveBU_ADDR(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted)
	SELECT ADDR.BUAH_TK, ADDR.CLU_CODE, ADDR.CLUT_TYPE, ADDR.CLT_CLASS, ADDR.DELETE_DATE 
	FROM CBD_IN_CEM_BU_ADDRESS_T ADDR 
	WHERE (ADDR.VALID_TO < @today OR ADDR.DELETE_DATE <= @today)
	AND ADDR.Expired IS NULL

	--TODO: Check if we shall select only latest record?!
	-- Get all email records that is inactive and not yet marked as expired
	RAISERROR('Retrieveing all email records that is inactive and not yet marked as expired', 10, 1) WITH NOWAIT
	INSERT INTO @inactiveBU_M(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted, ValidFrom, ValidTo, MEDT_TYPE)
	SELECT M.BUAH_TK, M.CLU_CODE, M.CLUT_TYPE, M.CLT_CLASS, M.DELETE_DATE, M.VALID_FROM, M.VALID_TO, M.MEDT_TYPE
	FROM CBD_IN_CEM_BU_MEDIA_T M
	WHERE (M.VALID_TO < @today
	OR M.DELETE_DATE <= @today)
	AND M.Expired IS NULL

	-- Get all bank records that is inactive and not yet marked as expired
	--RAISERROR('Retrieveing all bank records that is inactive and not yet marked as expired', 10, 1) WITH NOWAIT
	--INSERT INTO @inactiveBU_BANK(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted)
	--SELECT BANK.BUBNKAH_TK, BANK.CLU_CODE, BANK.CLUT_TYPE, BANK.CLT_CLASS, BANK.DELETE_DATE 
	--FROM CBD_IN_CEM_BU_BANK_ACCOUNT_T BANK
	--WHERE BANK.DELETE_DATE <= @today
	--AND BANK.Expired IS NULL

	---------------------------------------------------------------------------
	-- 1. Update BU data to ComputerUser
	RAISERROR('Updating business unit in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU 
	SET FirstName = SUBSTRING(ISNULL(BU.[NAME] + ' (' + BU.CLUT_TYPE + ' ' + BU.CLU_CODE + ')', ''), 1, 50), -- FIX: substring name to 50! To be removed later after column resize to 100.
	    [Status] = 1, 
	    ComputerUsersCategoryID = @computerUserCategoryID
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.ComputerUsersCategoryID, deleted.UserId, deleted.FirstName, deleted.ComputerUsersCategoryID
	INTO @updatedCU_BU(Id, UserId, FirstName, ComputerUserCategoryID, Before_UserId, Before_FirstName, Before_ComputerUserCategoryID)
	FROM @activeBU BU
	INNER JOIN tblComputerUsers CU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	WHERE BU.Exported IS NULL
	and CU.Customer_Id = @customerID
	
	-- 2. All customers that should be inserted 
	RAISERROR('Inserting all new business units in tblComputerUser', 10, 1) WITH NOWAIT
	INSERT INTO tblComputerUsers(UserId, FirstName, Phone, Email, [Status], ComputerUsersCategoryID, Customer_Id, Department_Id, OU_Id, UserCode)
	OUTPUT inserted.UserId, inserted.FirstName, inserted.Phone, inserted.Email, inserted.[Status], inserted.ComputerUsersCategoryID, inserted.Customer_Id, inserted.Department_Id, inserted.OU_Id 
	INTO @insertedCU(UserId, FirstName, Phone, Email, [Status], ComputerUsersCategoryID, Customer_Id, Department_Id, OU_Id)
	SELECT ABU.CLUT_TYPE + ' ' + ABU.CLU_CODE, 
		  SUBSTRING(ISNULL(ABU.[NAME] + ' (' + ABU.CLUT_TYPE + ' ' + ABU.CLU_CODE + ')', ''), 1, 50), -- FIX: substring name to 50! To be removed later after column resize to 100.
		  SUBSTRING(ISNULL(AP.Phone, ''), 0, 50),
		  SUBSTRING(ISNULL(AE.Email, ''), 0, 100),
		  1, 
		  @computerUserCategoryID, 
		  @customerID, 
		  D.Id, 
		  @ouID ,
		  ISNULL(ABANK.AccountNo, '') -- save in UserCode
	FROM @activeBU ABU
	LEFT JOIN tblComputerUsers CU ON (ABU.CLUT_TYPE + ' ' + ABU.CLU_CODE) = CU.UserId
	LEFT JOIN @activeAddr ADDR ON ADDR.CLT_CLASS = ABU.CLT_CLASS AND ADDR.CLU_CODE = ABU.CLU_CODE AND ADDR.CLUT_TYPE = ABU.CLUT_TYPE
	LEFT JOIN tblRegion R ON ADDR.Country = R.Region AND R.Customer_Id = @customerID
	LEFT JOIN tblDepartment D ON D.Region_Id = R.Id AND D.DepartmentId = @departmentId	
	LEFT JOIN @activePhone AP ON AP.CLT_CLASS = ABU.CLT_CLASS AND AP.CLU_CODE = ABU.CLU_CODE AND AP.CLUT_TYPE = ABU.CLUT_TYPE	
	LEFT JOIN @activeEmail AE ON AE.CLT_CLASS = ABU.CLT_CLASS AND AE.CLU_CODE = ABU.CLU_CODE AND AE.CLUT_TYPE = ABU.CLUT_TYPE
	LEFT JOIN  @activeBank ABANK ON ABANK.CLT_CLASS = ABU.CLT_CLASS AND ABANK.CLU_CODE = ABU.CLU_CODE AND ABANK.CLUT_TYPE = ABU.CLUT_TYPE
	WHERE CU.Id IS NULL 
	AND ABU.Exported IS NULL	
	---------------------------------------------------------------------------

	-- Set CBD BU data as exported
	RAISERROR('Marking inserted business units as exported', 10, 1) WITH NOWAIT
	UPDATE CBD_BU 
	SET Exported = @now 
	OUTPUT inserted.BU_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.[NAME], inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU(BU_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, [NAME], VALID_FROM, VALID_TO)
	FROM @activeBU BU
	INNER JOIN CBD_IN_CEM_BUSINESS_UNIT_T CBD_BU ON BU.PK = CBD_BU.BU_TK
	INNER JOIN tblComputerUsers CU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId	
	WHERE BU.Exported IS NULL
	AND CU.Customer_Id = @customerID
	-- extra check for affected records
	-- AND ( 
	--	  EXISTS(select 1 from @insertedCU ins where ins.UserId = (BU.CLUT_TYPE + ' ' + BU.CLU_CODE)) OR 
	--	  EXISTS (select 1 from @updatedCU_BU upd where upd.UserId = (BU.CLUT_TYPE + ' ' + BU.CLU_CODE))
	--    )

	-- Update address data to ComputerUser
	RAISERROR('Updating address in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU 
	SET Department_Id = D.Id
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Department_Id, deleted.Department_Id
	INTO @updatedCU_ADDR(Id, UserId, FirstName, Department_Id, Before_Department_Id)
	FROM @activeAddr ADDR 
	JOIN tblRegion R ON R.Region = ADDR.Country AND R.Customer_Id = @customerID
	JOIN tblDepartment D ON D.Region_Id = R.Id AND D.DepartmentId = @departmentId
	JOIN tblComputerUsers CU ON (ADDR.CLUT_TYPE + ' ' + ADDR.CLU_CODE) = CU.UserId
	WHERE ADDR.Exported IS NULL
	AND CU.Customer_Id = @customerID
	
	-- Set CBD address as exported
	RAISERROR('Set CBD address as exported', 10, 1) WITH NOWAIT
	UPDATE CBD_ADDR SET Exported = @now 
	OUTPUT inserted.BUAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.GA_CODE_CTY, inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU_ADDR(BUAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, GA_CODE_CTY, VALID_FROM, VALID_TO)
	FROM @activeAddr AA
	JOIN CBD_IN_CEM_BU_ADDRESS_T CBD_ADDR ON AA.PK = CBD_ADDR.BUAH_TK
	JOIN tblComputerUsers CU ON (AA.CLUT_TYPE + ' ' + AA.CLU_CODE) = CU.UserId
	WHERE AA.Exported IS NULL
	AND CU.Customer_Id = @customerID
		
	RAISERROR('Update email in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU SET Email = SUBSTRING(ISNULL(AE.Email, ''),0,100)
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Email, deleted.Email 
	INTO @updatedCU_M(ID, UserId, FirstName, Email, Before_Email)
	FROM @activeEmail AE
	JOIN tblComputerUsers CU ON (AE.CLUT_TYPE + ' ' + AE.CLU_CODE) = CU.UserId
	WHERE AE.Exported IS NULL
	AND CU.Customer_Id = @customerID
		
	RAISERROR('Set email as exported in CBD table', 10, 1) WITH NOWAIT
	UPDATE CBD_M SET Exported = @now 
	OUTPUT inserted.BUAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.[ADDRESS], inserted.SEQ_NO, inserted.MEDT_TYPE, inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU_M(BUAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, [ADDRESS], SEQ_NO, MEDT_TYPE, VALID_FROM, VALID_TO)
	FROM @activeEmail AE
	JOIN CBD_IN_CEM_BU_MEDIA_T CBD_M ON AE.PK = CBD_M.BUAH_TK AND AE.SeqNo = CBD_M.SEQ_NO AND CBD_M.MEDT_TYPE = 'EMAIL'
	JOIN tblComputerUsers CU ON (AE.CLUT_TYPE + ' ' + AE.CLU_CODE) = CU.UserId
	WHERE AE.Exported IS NULL
	AND CU.Customer_Id = @customerID

	RAISERROR('Update phone in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU 
	SET Phone = SUBSTRING(ISNULL(AP.Phone, ''), 0, 50)
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Phone, deleted.Phone 
	INTO @updatedCU_Tel(ID, UserId, FirstName, Phone, Before_Phone)
	FROM @activePhone AP
	JOIN tblComputerUsers CU ON (AP.CLUT_TYPE + ' ' + AP.CLU_CODE) = CU.UserId
	WHERE AP.Exported IS NULL
	AND CU.Customer_Id = @customerID
		
	RAISERROR('Set phone as exported in CBD table', 10, 1) WITH NOWAIT
	UPDATE CBD_M 
	SET Exported = @now 
	OUTPUT inserted.BUAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.[ADDRESS], inserted.SEQ_NO, inserted.MEDT_TYPE, inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU_M(BUAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, [ADDRESS], SEQ_NO, MEDT_TYPE, VALID_FROM, VALID_TO)
	FROM @activePhone AP
	JOIN CBD_IN_CEM_BU_MEDIA_T CBD_M ON AP.PK = CBD_M.BUAH_TK AND AP.SeqNo = CBD_M.SEQ_NO AND CBD_M.MEDT_TYPE = 'TEL'
	JOIN tblComputerUsers CU ON (AP.CLUT_TYPE + ' ' + AP.CLU_CODE) = CU.UserId
	WHERE AP.Exported IS NULL
	AND CU.Customer_Id = @customerID

	RAISERROR('Update bank details to computer user', 10, 1) WITH NOWAIT
	UPDATE CU SET UserCode = ISNULL(BANK.AccountNo, '')
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.UserCode, deleted.UserCode 
	INTO @updatedCU_ABANK(ID, UserId, FirstName, AccountNo, Before_AccountNo)
	FROM @activeBank BANK
	JOIN tblComputerUsers CU ON (BANK.CLUT_TYPE + ' ' + BANK.CLU_CODE) = CU.UserId
	WHERE BANK.Exported IS NULL
	AND CU.Customer_Id = @customerID
	    
	RAISERROR('Deactivating account numbers', 10, 1) WITH NOWAIT
	UPDATE CU SET UserCode = ''
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.UserCode, deleted.UserCode 
	INTO @deactivatedBank(Id, UserId, FirstName, AccountNo, Before_AccountNo)
	FROM tblComputerUsers CU
	JOIN @inactiveBU_BANK IBANK ON (IBANK.CLUT_TYPE + ' ' + IBANK.CLU_CODE) = CU.UserId
	LEFT JOIN @activeBank ABANK ON (ABANK.CLT_CLASS = IBANK.CLT_CLASS AND ABANK.CLU_CODE = IBANK.CLU_CODE AND ABANK.CLUT_TYPE = IBANK.CLUT_TYPE)
	WHERE ABANK.PK IS NULL
	AND CU.Customer_Id = @customerID
	AND CU.[Status] <> 0
	
    RAISERROR('Set CBD bank account as exported', 10, 1) WITH NOWAIT
    UPDATE BANK SET Exported = @now 
    OUTPUT inserted.BUBNKAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.ACCOUNT_NO
    INTO @exportedBU_BANK(BUBNKAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, ACCOUNT_NO)
    FROM @activeBank ABANK
    JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T BANK ON ABANK.PK = BANK.BUBNKAH_TK
    JOIN tblComputerUsers CU ON (BANK.CLUT_TYPE + ' ' + BANK.CLU_CODE) = CU.UserId
    WHERE ABANK.Exported IS NULL
    AND CU.Customer_Id = @customerID

	-- Deactivate computer user
	RAISERROR('Deactivating users in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU SET [Status] = 0 
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.[Status], deleted.[Status] 
	INTO @deactivatedBU(Id, UserId, FirstName, [Status], Before_Status)
	FROM tblComputerUsers CU
	INNER JOIN  @inactiveBU BU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	LEFT JOIN @activeBU ABU ON (ABU.CLT_CLASS = BU.CLT_CLASS AND ABU.CLU_CODE = BU.CLU_CODE AND ABU.CLUT_TYPE = BU.CLUT_TYPE)
	WHERE ABU.PK IS NULL
	AND CU.Customer_Id = @customerID
	AND CU.[Status] <> 0

	-- Deactivate computer user (if no address)
	RAISERROR('Deactivating addresses in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU SET Department_Id = NULL
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Department_Id, deleted.Department_Id 
	INTO @deactivatedAddr(Id, UserId, FirstName, Department_Id, Before_Department_Id)
	FROM tblComputerUsers CU
	JOIN  @inactiveBU_ADDR ADDR ON (ADDR.CLUT_TYPE + ' ' + ADDR.CLU_CODE) = CU.UserId
	LEFT JOIN @activeAddr A ON (A.CLT_CLASS = ADDR.CLT_CLASS AND A.CLU_CODE = ADDR.CLU_CODE AND A.CLUT_TYPE = ADDR.CLUT_TYPE)
	WHERE A.PK IS NULL
	AND CU.Customer_Id = @customerID
	AND CU.[Status] <> 0

	-- Deactivate email
	RAISERROR('Deactivating emails in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU SET Email = ''
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Email, deleted.Email 
	INTO @deactivatedEmail(Id, UserId, FirstName, Email, Before_Email)
	FROM tblComputerUsers CU
	JOIN @inactiveBU_M M ON (M.CLUT_TYPE + ' ' + M.CLU_CODE) = CU.UserId AND M.MEDT_TYPE = 'EMAIL'
	LEFT JOIN @activeEmail AE ON (AE.CLT_CLASS = M.CLT_CLASS AND AE.CLU_CODE = M.CLU_CODE AND AE.CLUT_TYPE = M.CLUT_TYPE) 
	WHERE AE.PK IS NULL
	AND CU.Customer_Id = @customerID
	AND CU.[Status] <> 0
	
	-- Deactivate phone
	RAISERROR('Deactivating phone in tblComputerUser', 10, 1) WITH NOWAIT
	UPDATE CU SET Phone = ''
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Email, deleted.Email 
	INTO @deactivatedPhone(Id, UserId, FirstName, Phone, Before_Phone)
	FROM tblComputerUsers CU
	JOIN @inactiveBU_M M ON (M.CLUT_TYPE + ' ' + M.CLU_CODE) = CU.UserId AND M.MEDT_TYPE = 'TEL'
	LEFT JOIN @activePhone AP ON (AP.CLT_CLASS = M.CLT_CLASS AND AP.CLU_CODE = M.CLU_CODE AND AP.CLUT_TYPE = M.CLUT_TYPE) 
	WHERE AP.PK IS NULL
	AND CU.Customer_Id = @customerID	
	AND CU.[Status] <> 0

	-- Set business unit expired
	RAISERROR('Setting business unit expired', 10, 1) WITH NOWAIT
	UPDATE BU SET Expired = @now
	FROM @inactiveBU IBU
	JOIN CBD_IN_CEM_BUSINESS_UNIT_T BU ON BU.BU_TK = IBU.PK

	-- Set email|phone expired
	RAISERROR('Setting email and phone expired', 10, 1) WITH NOWAIT
	UPDATE M SET Expired = @now
	FROM @inactiveBU_M IM
	JOIN CBD_IN_CEM_BU_MEDIA_T M ON M.BUAH_TK = IM.PK

	-- Set bank account expired
	--RAISERROR('Setting bank account expired', 10, 1) WITH NOWAIT
	--UPDATE BANK SET Expired = @now
	--FROM @inactiveBU_BANK IBANK
	--JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T BANK ON BANK.BUBNKAH_TK = IBANK.PK

	-- Set address expired
	RAISERROR('Setting address expired', 10, 1) WITH NOWAIT
	UPDATE ADDR 
	SET Expired = @now
	FROM @inactiveBU_ADDR IADDR
	JOIN CBD_IN_CEM_BU_ADDRESS_T ADDR ON ADDR.BUAH_TK = IADDR.PK

	---- Deactivate computer user
	--UPDATE CU SET [Status] = 0 
	--OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.[Status], deleted.[Status] 
	--INTO @deactivatedBU(Id, UserId, FirstName, [Status], Before_Status)
	--FROM tblComputerUsers CU
	--JOIN  CBD_IN_CEM_BUSINESS_UNIT_T BU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	--LEFT JOIN @activeBU ABU ON (ABU.CLT_CLASS = BU.CLT_CLASS AND ABU.CLU_CODE = BU.CLU_CODE AND ABU.CLUT_TYPE = BU.CLUT_TYPE)
	--WHERE ABU.PK IS NULL AND BU.Expired IS NULL
	--AND CU.[Status] <> 0

	---- Deactivate computer user
	--UPDATE CU SET [Status] = 0 
	--OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.[Status], deleted.[Status] 
	--INTO @deactivatedBU(Id, UserId, FirstName, [Status], Before_Status)
	--FROM tblComputerUsers CU
	--JOIN  CBD_IN_CEM_BUSINESS_UNIT_T BU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	--LEFT JOIN @activeBU ABU ON (ABU.CLT_CLASS = BU.CLT_CLASS AND ABU.CLU_CODE = BU.CLU_CODE AND ABU.CLUT_TYPE = BU.CLUT_TYPE)
	--WHERE ABU.PK IS NULL AND BU.Expired IS NULL
	--AND CU.[Status] <> 0

-- UNCOMMENT FOR DEBUG ONLY
/*
	SELECT 'Active BU' [Active BU], * FROM @activeBU ORDER BY CLU_CODE
	SELECT 'Inserted CU' [Inserted tblComputerUser], * FROM @insertedCU ORDER BY ID

	SELECT 'Updated BU' [Updated BU tblComputerUser], * FROM @updatedCU_BU ORDER BY ID
	SELECT 'Marked BU exported' [Marked BU exported], * FROM @exportedBU ORDER BY CLU_CODE

	SELECT 'Updated Email' [Updated BU Email tblComputerUser], * FROM @updatedCU_M ORDER BY ID
	SELECT 'Updated Phone' [Updated BU Phone tblComputerUser], * FROM @updatedCU_Tel ORDER BY ID
	SELECT 'Marked BU_M exported' [Marked BU_M exported], * FROM @exportedBU_M ORDER BY CLU_CODE

	--SELECT 'Updated bank account' [Updated bank account], * FROM @updatedCU_ABANK ORDER BY ID
	--SELECT 'Marked BU_BANK exported' [Marked BU_BANK exported], * FROM @exportedBU_BANK ORDER BY CLU_CODE
	
	SELECT 'Updated address (country)' [Updated address (country)], * FROM @updatedCU_ADDR ORDER BY ID
	SELECT 'Marked BU_ADDR exported' [Marked BU_ADDR exported], * FROM @exportedBU_ADDR ORDER BY CLU_CODE

	SELECT 'Deactivated BU' [Deactivated BU], * FROM @deactivatedBU ORDER BY ID
	SELECT 'Deactivated emails' [Deactivated emails], * FROM @deactivatedEmail ORDER BY ID
	SELECT 'Deactivated phone' [Deactivated phones], * FROM @deactivatedPhone ORDER BY ID
	--SELECT 'Deactivated bank account' [Deactivated bank account], * FROM @deactivatedBank ORDER BY ID
	SELECT 'Deactivated address' [Deactivated address], * FROM @deactivatedAddr ORDER BY ID

	SELECT 'Expired BU (user)' [Inactive BU], * FROM @inactiveBU ORDER BY CLU_CODE
	SELECT 'Expired BU_M (email|phone)' [Inactive BU_M], * FROM @inactiveBU_M ORDER BY CLU_CODE
	--SELECT 'Expired BU_BANK' [Inactive BU_BANK], * FROM @inactiveBU_BANK ORDER BY CLU_CODE
	SELECT 'Expired BU_ADDR (addr)' [Inactive BU_ADDR], * FROM @inactiveBU_ADDR ORDER BY CLU_CODE
*/
END
GO

-- UPDATE Exported to null to resync Bank accountNo and Phone values
UPDATE [dbo].[CBD_IN_CEM_BU_BANK_ACCOUNT_T]
SET [Exported] = null
 
UPDATE [dbo].[CBD_IN_CEM_BU_MEDIA_T]
SET [Exported] = null
WHERE [MEDT_TYPE] = 'TEL'
GO
