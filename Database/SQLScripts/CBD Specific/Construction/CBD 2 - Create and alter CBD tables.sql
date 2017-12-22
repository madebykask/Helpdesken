
/****** Object:  StoredProcedure [dbo].[CBD_ComputerUser_Sync]    Script Date: 2017-12-06 16:22:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Johan Weinitz
-- =============================================
CREATE PROCEDURE [dbo].[CBD_ComputerUser_Sync] 
(
	@customerID INT,
	@ouID INT,
	@computerUserCategoryGUID UNIQUEIDENTIFIER
)
AS
BEGIN
DECLARE @now DATETIME = GETUTCDATE(),
		@today DATE = GETUTCDATE(),
		@departmentId NVARCHAR(MAX) = 'IKEA Vendor'


	DECLARE @computerUserCategoryID INT
	SELECT @computerUserCategoryID = ID FROM tblComputerUsersCategory C WHERE C.ComputerUsersCategoryGuid = @computerUserCategoryGUID

	DECLARE @activeBU TABLE
	(
		PK INT,						-- CBD_IN_CEM_BUSINESS_UNIT_T.BU_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_TO
		[Name] NVARCHAR(MAX),
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLU_CODE		-- '0012A'
		Exported DATETIME NULL
	)

	DECLARE @activeAddr TABLE
	(
		PK INT,						-- CBD_IN_CEM_BU_ADDRESS_T.BUAH_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BU_ADDRESS_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_ADDRESS_T.VALID_TO
		Country VARCHAR(10),		-- CBD_IN_CEM_BU_ADDRESS_T.GA_CODE_CTY		-- 'DE'
		SeqNo INT,					-- CBD_IN_CEM_BU_ADDRESS_T.SEQ_NO			-- 1,2,3
		AdtType VARCHAR(5),			-- CBD_IN_CEM_BU_ADDRESS_T.ADT_TYPE			-- 'MAIN'
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_ADDRESS_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_ADDRESS_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_ADDRESS_T.CLU_CODE			-- '0012A'
		Exported DATETIME NULL		-- CBD_IN_CEM_BU_ADDRESS_T.Exported				
	)

	DECLARE @activeEmail TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_MEDIA_T.BUAH_TK
		Email NVARCHAR(115),		-- CBD_IN_CEM_BU_MEDIA_T.ADDRESS
		ValidFrom DATETIME NOT NULL,-- CBD_IN_CEM_BU_MEDIA_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.VALID_TO			
		SeqNo INT,					-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO				-- 1,2,3
		SeqNoAddr INT,				-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO_ADDR			
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_MEDIA_T.CLT_CLASS			-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_MEDIA_T.CLUT_TYPE			-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_MEDIA_T.CLU_CODE			-- '0012A'
		MedtType VARCHAR(10),		-- CBD_IN_CEM_BU_MEDIA_T.SEQ_NO_ADDR		-- 'PHONE' | 'EMAIL'
		Exported DATETIME NULL		-- CBD_IN_CEM_BU_MEDIA_T.Exported
	)

	DECLARE @activeBank TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.BUTRGH_TK	

		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLU_CODE		-- '0012A'
		Exported DATETIME NULL,		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.Exported
		AccountNo VARCHAR(35)		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.ACCOUNT_NO			
	)

	DECLARE @inactiveBU TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BUSINESS_UNIT_T.BUAH_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BUSINESS_UNIT_T.VALID_TO	
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BUSINESS_UNIT_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BUSINESS_UNIT_T.DELETED_DATE
	)


	DECLARE @inactiveBU_M TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_MEDIA_T.BUAH_TK
		ValidFrom DATETIME,			-- CBD_IN_CEM_BU_MEDIA_T.VALID_FROM
		ValidTo DATETIME NULL,		-- CBD_IN_CEM_BU_MEDIA_T.VALID_TO	
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_MEDIA_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_MEDIA_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_MEDIA_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BU_MEDIA_T.DELETED_DATE
	)


	DECLARE @inactiveBU_BANK TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.BUAH_TK
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.CLU_CODE		-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BU_BANK_ACCOUNT_T.DELETED_DATE
	)

	DECLARE @inactiveBU_ADDR TABLE
	(
		PK INT NOT NULL,			-- CBD_IN_CEM_BU_ADDRESS_T.BUAH_TK
		CLT_CLASS VARCHAR(2),		-- CBD_IN_CEM_BU_ADDRESS_T.CLT_CLASS		-- 'BU'
		CLUT_TYPE VARCHAR(3),		-- CBD_IN_CEM_BU_ADDRESS_T.CLUT_TYPE		-- 'ISV' 
		CLU_CODE VARCHAR(5),		-- CBD_IN_CEM_BU_ADDRESS_T.CLU_CODE			-- '0012A'
		Deleted DATETIME NULL		-- CBD_IN_CEM_BU_ADDRESS_T.DELETED_DATE
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

	DECLARE @updatedCU_ABANK TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		AccountNo NVARCHAR(100),
		Before_AccountNo NVARCHAR(100)
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


	DECLARE @deactivatedBank TABLE
	(
		ID INT,
		UserId NVARCHAR(50),
		FirstName NVARCHAR(50),
		Phone NVARCHAR(100),
		Before_Phone NVARCHAR(100)
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

	DECLARE @expiredBU_BANK TABLE
	(
		BUBNKAH_TK INT,
		CLT_CLASS VARCHAR(2),
		CLUT_TYPE VARCHAR(3), 
		CLU_CODE VARCHAR(5),
		ACCOUNT_NO VARCHAR(35),
		DELETE_DATE DATETIME
	)

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
		[ADDRESS] VARCHAR(90),
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
		ACCOUNT_NO VARCHAR(30),
		DELETE_DATE DATETIME
	)

	-- Active business unit addresses
	INSERT INTO @activeAddr(PK, ValidFrom, ValidTo, Country, SeqNo, AdtType, CLT_CLASS, CLUT_TYPE, CLU_CODE, Exported)
	SELECT ADDR.BUAH_TK, ADDR.VALID_FROM, ADDR.VALID_TO, ADDR.GA_CODE_CTY, ADDR.SEQ_NO, ADDR.ADT_TYPE, ADDR.CLT_CLASS, ADDR.CLUT_TYPE, ADDR.CLU_CODE, ADDR.Exported FROM (
		SELECT ADDR.*, ROW_NUMBER() OVER (PARTITION BY  ADDR.CLT_CLASS, ADDR.CLUT_TYPE, ADDR.CLU_CODE  ORDER BY CASE WHEN ADDR.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, ADDR.SEQ_NO) R FROM CBD_IN_CEM_BU_ADDRESS_T ADDR
		WHERE (ADDR.VALID_TO IS NULL OR ADDR.VALID_TO >= @today)
		AND ADDR.VALID_FROM <= @today
		AND (ADDR.DELETE_DATE IS NULL OR ADDR.DELETE_DATE > @today)
	) ADDR
	WHERE ADDR.R = 1

	-- Active business units
	INSERT INTO @activeBU(PK, ValidFrom, ValidTo, [Name], CLT_CLASS, CLUT_TYPE, CLU_CODE, Exported)
	SELECT BU.BU_TK, BU.VALID_FROM, BU.VALID_TO, BU.[NAME], BU.CLT_CLASS, BU.CLUT_TYPE, BU.CLU_CODE, BU.Exported FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
	--JOIN @activeAddr ADDR ON ADDR.CLT_CLASS = BU.CLT_CLASS AND ADDR.CLUT_TYPE = BU.CLUT_TYPE AND ADDR.CLU_CODE = BU.CLU_CODE
	WHERE BU.VALID_FROM <= @today 
	AND (BU.VALID_TO IS NULL OR BU.VALID_TO >= @today)
	AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @today)


	-- Active business unit address emails
	INSERT INTO @activeEmail(PK, Email, ValidFrom, ValidTo, SeqNo, SeqNoAddr, CLT_CLASS, CLUT_TYPE, CLU_CODE, MedtType, Exported)
	SELECT E.BUAH_TK, E.[ADDRESS], E.VALID_FROM, E.VALID_TO, E.SEQ_NO, E.SEQ_NO_ADDR, E.CLT_CLASS, E.CLUT_TYPE, E.CLU_CODE, E.MEDT_TYPE, E.Exported FROM (
		SELECT M.BUAH_TK, M.[ADDRESS], M.VALID_FROM, M.VALID_TO, M.SEQ_NO, M.SEQ_NO_ADDR, M.CLT_CLASS, M.CLUT_TYPE, M.CLU_CODE, M.MEDT_TYPE, M.Exported, ROW_NUMBER() OVER (PARTITION BY  M.CLT_CLASS, M.CLUT_TYPE, M.CLU_CODE , M.BUAH_TK ORDER BY CASE WHEN M.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, M.SEQ_NO) R FROM CBD_IN_CEM_BU_MEDIA_T M 
		JOIN @activeAddr ADDR ON M.BUAH_TK = ADDR.PK
		WHERE (M.VALID_TO IS NULL OR M.VALID_TO >= @today)
		AND M.VALID_FROM <= @today
		AND (M.DELETE_DATE IS NULL OR M.DELETE_DATE > @today)
		AND M.MEDT_TYPE = 'EMAIL'
	) E
	WHERE E.R = 1

	-- Active bank account
	INSERT INTO @activeBank(PK, CLT_CLASS, CLUT_TYPE, CLU_CODE, AccountNo, Exported)
	SELECT X.BUBNKAH_TK, X.CLT_CLASS, X.CLUT_TYPE, X.CLU_CODE, X.ACCOUNT_NO, X.Exported FROM (
		SELECT BU.NAME, B.*, ROW_NUMBER() OVER (PARTITION BY B.CLU_CODE ORDER BY B.BUBNKAH_TK DESC) R FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
		JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T B ON B.CLU_CODE = BU.CLU_CODE
		WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO >= @today)
		AND BU.VALID_FROM <= @today
		AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @today)
		AND (B.DELETE_DATE IS NULL OR B.DELETE_DATE > @today)
	) X
	WHERE X.R = 1

	-- Get all business unit records that is inactive and not yet marked as expired (having no address equals inactive too)
	INSERT INTO @inactiveBU(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted, ValidFrom, ValidTo)
	SELECT BU.BU_TK, BU.CLU_CODE, BU.CLUT_TYPE, BU.CLT_CLASS, BU.DELETE_DATE, BU.VALID_FROM, BU.VALID_TO FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
	WHERE (BU.VALID_TO < @today
	OR BU.DELETE_DATE <= @today)
	AND BU.Expired IS NULL


	-- Get all email records that is inactive and not yet marked as expired
	INSERT INTO @inactiveBU_M(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted, ValidFrom, ValidTo)
	SELECT M.BUAH_TK, M.CLU_CODE, M.CLUT_TYPE, M.CLT_CLASS, M.DELETE_DATE, M.VALID_FROM, M.VALID_TO FROM CBD_IN_CEM_BU_MEDIA_T M
	WHERE (M.VALID_TO < @today
	OR M.DELETE_DATE <= @today)
	AND M.Expired IS NULL

	-- Get all bank records that is inactive and not yet marked as expired
	INSERT INTO @inactiveBU_BANK(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted)
	SELECT BANK.BUBNKAH_TK, BANK.CLU_CODE, BANK.CLUT_TYPE, BANK.CLT_CLASS, BANK.DELETE_DATE FROM CBD_IN_CEM_BU_BANK_ACCOUNT_T BANK
	WHERE BANK.DELETE_DATE <= @today
	AND BANK.Expired IS NULL

		-- Get all bank records that is inactive and not yet marked as expired
	INSERT INTO @inactiveBU_ADDR(PK, CLU_CODE, CLUT_TYPE, CLT_CLASS, Deleted)
	SELECT ADDR.BUAH_TK, ADDR.CLU_CODE, ADDR.CLUT_TYPE, ADDR.CLT_CLASS, ADDR.DELETE_DATE FROM CBD_IN_CEM_BU_ADDRESS_T ADDR
	WHERE (ADDR.VALID_TO < @today
	OR ADDR.DELETE_DATE <= @today)
	AND ADDR.Expired IS NULL

	
	-- Update BU data to ComputerUser
	UPDATE CU SET FirstName = BU.[NAME] + ' (' + BU.CLUT_TYPE + ' ' + BU.CLU_CODE + ')', [Status] = 1, ComputerUsersCategoryID = @computerUserCategoryID
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.ComputerUsersCategoryID, deleted.UserId, deleted.FirstName, deleted.ComputerUsersCategoryID
	INTO @updatedCU_BU(Id, UserId, FirstName, ComputerUserCategoryID, Before_UserId, Before_FirstName, Before_ComputerUserCategoryID)
	FROM @activeBU BU
	JOIN tblComputerUsers CU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	WHERE BU.Exported IS NULL

	
	-- All customers that should be inserted 
	INSERT INTO tblComputerUsers(UserId, FirstName, Phone, Email, [Status], ComputerUsersCategoryID, Customer_Id, Department_Id, OU_Id)
	OUTPUT inserted.UserId, inserted.FirstName, inserted.Phone, inserted.Email, inserted.[Status], inserted.ComputerUsersCategoryID, inserted.Customer_Id, inserted.Department_Id, inserted.OU_Id 
	INTO @insertedCU(UserId, FirstName, Phone, Email, [Status], ComputerUsersCategoryID, Customer_Id, Department_Id, OU_Id)
	SELECT ABU.CLUT_TYPE + ' ' + ABU.CLU_CODE, ABU.[NAME] + ' (' + ABU.CLUT_TYPE + ' ' + ABU.CLU_CODE + ')', ISNULL(ABANK.AccountNo, ''), ISNULL(AE.Email, ''), 1, @computerUserCategoryID, @customerID, D.Id, @ouID  
	FROM @activeBU ABU
	LEFT JOIN @activeAddr ADDR ON ADDR.CLT_CLASS = ABU.CLT_CLASS AND ADDR.CLU_CODE = ABU.CLU_CODE AND ADDR.CLUT_TYPE = ABU.CLUT_TYPE
	LEFT JOIN tblRegion R ON ADDR.Country = R.Region
	LEFT JOIN tblDepartment D ON D.Region_Id = R.Id AND D.DepartmentId = @departmentId
	LEFT JOIN tblComputerUsers CU ON (ABU.CLUT_TYPE + ' ' + ABU.CLU_CODE) = CU.UserId
	LEFT JOIN  @activeEmail AE ON AE.CLT_CLASS = ABU.CLT_CLASS AND AE.CLU_CODE = ABU.CLU_CODE AND AE.CLUT_TYPE = ABU.CLUT_TYPE
	LEFT JOIN  @activeBank ABANK ON ABANK.CLT_CLASS = ABANK.CLT_CLASS AND ABANK.CLU_CODE = ABU.CLU_CODE AND ABANK.CLUT_TYPE = ABU.CLUT_TYPE
	WHERE CU.Id IS NULL 
	AND ABU.Exported IS NULL

	-- Set CBD BU data as exported
	UPDATE CBD_BU SET Exported = @now 
	OUTPUT inserted.BU_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.[NAME], inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU(BU_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, [NAME], VALID_FROM, VALID_TO)
	FROM @activeBU BU
	JOIN CBD_IN_CEM_BUSINESS_UNIT_T CBD_BU ON BU.PK = CBD_BU.BU_TK
	JOIN tblComputerUsers CU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	WHERE BU.Exported IS NULL


	-- Update address data to ComputerUser
	UPDATE CU SET Department_Id = D.Id
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Department_Id, deleted.Department_Id
	INTO @updatedCU_ADDR(Id, UserId, FirstName, Department_Id, Before_Department_Id)
	FROM @activeAddr ADDR 
	JOIN tblRegion R ON R.Region = ADDR.Country
	JOIN tblDepartment D ON D.Region_Id = R.Id AND D.DepartmentId = @departmentId
	JOIN tblComputerUsers CU ON (ADDR.CLUT_TYPE + ' ' + ADDR.CLU_CODE) = CU.UserId
	WHERE ADDR.Exported IS NULL


	-- Set CBD address as exported
	UPDATE CBD_ADDR SET Exported = @now 
	OUTPUT inserted.BUAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.GA_CODE_CTY, inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU_ADDR(BUAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, GA_CODE_CTY, VALID_FROM, VALID_TO)
	FROM @activeAddr AA
	JOIN CBD_IN_CEM_BU_ADDRESS_T CBD_ADDR ON AA.PK = CBD_ADDR.BUAH_TK
	JOIN tblComputerUsers CU ON (AA.CLUT_TYPE + ' ' + AA.CLU_CODE) = CU.UserId
	WHERE AA.Exported IS NULL


	-- Update email data to computeruser
	UPDATE CU SET Email = ISNULL(AE.Email, '')
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Email, deleted.Email 
	INTO @updatedCU_M(ID, UserId, FirstName, Email, Before_Email)
	FROM @activeEmail AE
	JOIN tblComputerUsers CU ON (AE.CLUT_TYPE + ' ' + AE.CLU_CODE) = CU.UserId
	WHERE AE.Exported IS NULL

	-- Set CBD email as exported
	UPDATE CBD_M SET Exported = @now 
	OUTPUT inserted.BUAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.[ADDRESS], inserted.SEQ_NO, inserted.MEDT_TYPE, inserted.VALID_FROM, inserted.VALID_TO 
	INTO @exportedBU_M(BUAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, [ADDRESS], SEQ_NO, MEDT_TYPE, VALID_FROM, VALID_TO)
	FROM @activeEmail AE
	JOIN CBD_IN_CEM_BU_MEDIA_T CBD_M ON AE.PK = CBD_M.BUAH_TK AND AE.SeqNo = CBD_M.SEQ_NO AND CBD_M.MEDT_TYPE = 'EMAIL'
	JOIN tblComputerUsers CU ON (AE.CLUT_TYPE + ' ' + AE.CLU_CODE) = CU.UserId
	WHERE AE.Exported IS NULL

	-- Update bank data to computeruser (use phone field)
	UPDATE CU SET Phone = ISNULL(BANK.AccountNo, '')
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Phone, deleted.Phone 
	INTO @updatedCU_ABANK(ID, UserId, FirstName, AccountNo, Before_AccountNo)
	FROM @activeBank BANK
	JOIN tblComputerUsers CU ON (BANK.CLUT_TYPE + ' ' + BANK.CLU_CODE) = CU.UserId
	WHERE BANK.Exported IS NULL

	-- Set CBD bank account as exported
	UPDATE BANK SET Exported = @now 
	OUTPUT inserted.BUBNKAH_TK, inserted.CLT_CLASS, inserted.CLU_CODE, inserted.CLUT_TYPE, inserted.DELETE_DATE, inserted.ACCOUNT_NO
	INTO @exportedBU_BANK(BUBNKAH_TK, CLT_CLASS, CLU_CODE, CLUT_TYPE, DELETE_DATE, ACCOUNT_NO)
	FROM @activeBank ABANK
	JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T BANK ON ABANK.PK = BANK.BUBNKAH_TK
	JOIN tblComputerUsers CU ON (BANK.CLUT_TYPE + ' ' + BANK.CLU_CODE) = CU.UserId
	WHERE ABANK.Exported IS NULL

	-- Deactivate computer user
	UPDATE CU SET [Status] = 0 
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.[Status], deleted.[Status] 
	INTO @deactivatedBU(Id, UserId, FirstName, [Status], Before_Status)
	FROM tblComputerUsers CU
	JOIN  @inactiveBU BU ON (BU.CLUT_TYPE + ' ' + BU.CLU_CODE) = CU.UserId
	LEFT JOIN @activeBU ABU ON (ABU.CLT_CLASS = BU.CLT_CLASS AND ABU.CLU_CODE = BU.CLU_CODE AND ABU.CLUT_TYPE = BU.CLUT_TYPE)
	WHERE ABU.PK IS NULL
	AND CU.[Status] <> 0

	-- Deactivate computer user (if no address)
	UPDATE CU SET Department_Id = NULL
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Department_Id, deleted.Department_Id 
	INTO @deactivatedAddr(Id, UserId, FirstName, Department_Id, Before_Department_Id)
	FROM tblComputerUsers CU
	JOIN  @inactiveBU_ADDR ADDR ON (ADDR.CLUT_TYPE + ' ' + ADDR.CLU_CODE) = CU.UserId
	LEFT JOIN @activeAddr A ON (A.CLT_CLASS = ADDR.CLT_CLASS AND A.CLU_CODE = ADDR.CLU_CODE AND A.CLUT_TYPE = ADDR.CLUT_TYPE)
	WHERE A.PK IS NULL
	AND CU.[Status] <> 0

	-- Deactivate email
	UPDATE CU SET Email = ''
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Email, deleted.Email 
	INTO @deactivatedEmail(Id, UserId, FirstName, Email, Before_Email)
	FROM tblComputerUsers CU
	JOIN @inactiveBU_M M ON (M.CLUT_TYPE + ' ' + M.CLU_CODE) = CU.UserId
	LEFT JOIN @activeEmail AE ON (AE.CLT_CLASS = M.CLT_CLASS AND AE.CLU_CODE = M.CLU_CODE AND AE.CLUT_TYPE = M.CLUT_TYPE)
	WHERE AE.PK IS NULL

	-- Deactivate account number
	UPDATE CU SET Phone = ''
	OUTPUT inserted.Id, inserted.UserId, inserted.FirstName, inserted.Phone, deleted.Phone 
	INTO @deactivatedBank(Id, UserId, FirstName, Phone, Before_Phone)
	FROM tblComputerUsers CU
	JOIN @inactiveBU_BANK IBANK ON (IBANK.CLUT_TYPE + ' ' + IBANK.CLU_CODE) = CU.UserId
	LEFT JOIN @activeBank ABANK ON (ABANK.CLT_CLASS = IBANK.CLT_CLASS AND ABANK.CLU_CODE = IBANK.CLU_CODE AND ABANK.CLUT_TYPE = IBANK.CLUT_TYPE)
	WHERE ABANK.PK IS NULL
	AND CU.[Status] <> 0

	-- Set business unit expired
	UPDATE BU SET Expired = @now
	FROM @inactiveBU IBU
	JOIN CBD_IN_CEM_BUSINESS_UNIT_T BU ON BU.BU_TK = IBU.PK

	-- Set email expired
	UPDATE M SET Expired = @now
	FROM @inactiveBU_BANK IM
	JOIN CBD_IN_CEM_BU_MEDIA_T M ON M.BUAH_TK = IM.PK

	-- Set bank account expired
	UPDATE BANK SET Expired = @now
	FROM @inactiveBU_BANK IBANK
	JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T BANK ON BANK.BUBNKAH_TK = IBANK.PK

	-- Set bank account expired
	UPDATE ADDR SET Expired = @now
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



	SELECT 'Active BU' [Active BU], * FROM @activeBU ORDER BY CLU_CODE
	SELECT 'Inserted CU' [Inserted tblComputerUser], * FROM @insertedCU ORDER BY ID

	SELECT 'Updated BU' [Updated BU tblComputerUser], * FROM @updatedCU_BU ORDER BY ID
	SELECT 'Marked BU exported' [Marked BU exported], * FROM @exportedBU ORDER BY CLU_CODE

	SELECT 'Updated Email' [Updated BU Email tblComputerUser], * FROM @updatedCU_M ORDER BY ID
	SELECT 'Marked BU_M exported' [Marked BU_M exported], * FROM @exportedBU_M ORDER BY CLU_CODE

	SELECT 'Updated bank account' [Updated bank account], * FROM @updatedCU_ABANK ORDER BY ID
	SELECT 'Marked BU_BANK exported' [Marked BU_BANK exported], * FROM @exportedBU_BANK ORDER BY CLU_CODE
	
	SELECT 'Updated address (country)' [Updated address (country)], * FROM @updatedCU_ADDR ORDER BY ID
	SELECT 'Marked BU_ADDR exported' [Marked BU_ADDR exported], * FROM @exportedBU_ADDR ORDER BY CLU_CODE

	SELECT 'Deactivated BU' [Deactivated BU], * FROM @deactivatedBU ORDER BY ID
	SELECT 'Deactivated emails' [Deactivated emails], * FROM @deactivatedEmail ORDER BY ID
	SELECT 'Deactivated bank account' [Deactivated bank account], * FROM @deactivatedBank ORDER BY ID
	SELECT 'Deactivated address' [Deactivated address], * FROM @deactivatedAddr ORDER BY ID

	SELECT 'Expired BU' [Inactive BU], * FROM @inactiveBU ORDER BY CLU_CODE
	SELECT 'Expired BU_M' [Inactive BU_M], * FROM @inactiveBU_M ORDER BY CLU_CODE
	SELECT 'Expired BU_BANK' [Inactive BU_BANK], * FROM @inactiveBU_BANK ORDER BY CLU_CODE
	SELECT 'Expired BU_ADDR' [Inactive BU_ADDR], * FROM @inactiveBU_ADDR ORDER BY CLU_CODE


END

GO

-- =============================================
-- Author:		Johan Weinitz
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CBD_Get_Address] 
	-- Add the parameters for the stored procedure here
	@cluCode NVARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @now DATE = GETDATE()

	SELECT X.BUAH_TK, X.STREET, X.ZIP_CODE, X.CITY, X.GA_CODE_CTY  FROM (
        SELECT BU.[NAME], ADDR.*, ROW_NUMBER() OVER (PARTITION BY ADDR.CLU_CODE ORDER BY CASE WHEN ADDR.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, ADDR.SEQ_NO) R FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
        JOIN CBD_IN_CEM_BU_ADDRESS_T ADDR ON ADDR.CLU_CODE = BU.CLU_CODE
        WHERE (ADDR.VALID_TO IS NULL OR ADDR.VALID_TO > @now)
        AND (BU.VALID_TO IS NULL OR BU.VALID_TO > @now)
        AND ADDR.VALID_FROM <= @now
        AND BU.VALID_FROM <= @now
        AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @now)
        AND (ADDR.DELETE_DATE IS NULL OR ADDR.DELETE_DATE > @now)
    ) X
    WHERE X.R = 1
    AND (X.CLUT_TYPE + ' ' + X.CLU_CODE) = @cluCode
END

GO


-- =============================================
-- Author:		Johan Weinitz
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CBD_Get_Bank] 
	-- Add the parameters for the stored procedure here
	@cluCode NVARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @now DATE = GETDATE()

	SELECT X.BUBNKAH_TK, X.NAME, X.BANK_NAME, X.IBAN, X.SWIFT_CODE, X.ACCOUNT_NO FROM (
		SELECT BU.NAME, B.*, ROW_NUMBER() OVER (PARTITION BY B.CLU_CODE ORDER BY B.BUBNKAH_TK DESC) R FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
		JOIN CBD_IN_CEM_BU_BANK_ACCOUNT_T B ON B.CLU_CODE = BU.CLU_CODE
		WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO > @now)
		AND BU.VALID_FROM <= @now
		AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @now)
		AND (B.DELETE_DATE IS NULL OR B.DELETE_DATE > @now)
	) X
	WHERE X.R = 1
	AND (X.CLUT_TYPE + ' ' + X.CLU_CODE) = @cluCode
END
GO

-- =============================================
-- Author:		Johan Weinitz
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CBD_Get_Emails] 
	-- Add the parameters for the stored procedure here
	@cluCode NVARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @now DATE = GETDATE()

	SELECT  X.BUAH_TK, X.[NAME], X.[ADDRESS], X.ADT_TYPE FROM (
		SELECT BU.[NAME], M.*, ROW_NUMBER() OVER (PARTITION BY M.CLU_CODE, M.BUAH_TK ORDER BY CASE WHEN M.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, M.SEQ_NO) R FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
		JOIN CBD_IN_CEM_BU_ADDRESS_T ADDR ON ADDR.CLU_CODE = BU.CLU_CODE
		JOIN CBD_IN_CEM_BU_MEDIA_T M ON M.CLU_CODE = BU.CLU_CODE AND M.BUAH_TK = ADDR.BUAH_TK
		WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO > @now)
		AND (ADDR.VALID_TO IS NULL OR ADDR.VALID_TO > @now)
		AND (M.VALID_TO IS NULL OR M.VALID_TO > @now)
		AND BU.VALID_FROM <= @now
		AND ADDR.VALID_FROM <= @now
		AND M.VALID_FROM <= @now
		AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @now)
		AND (ADDR.DELETE_DATE IS NULL OR ADDR.DELETE_DATE > @now)
		AND (M.DELETE_DATE IS NULL OR M.DELETE_DATE > @now)
		AND M.MEDT_TYPE = 'EMAIL'
	) X
	WHERE (X.CLUT_TYPE + ' ' + X.CLU_CODE) = @cluCode
	ORDER BY X.R
END
GO


-- =============================================
-- Author:		Johan Weinitz
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CBD_Get_Phone] 
	-- Add the parameters for the stored procedure here
	@cluCode NVARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @now DATE = GETDATE()

	SELECT X.[NAME], X.BUAH_TK, X.[ADDRESS] FROM (
		SELECT BU.[NAME], M.*, ROW_NUMBER() OVER (PARTITION BY M.CLU_CODE ORDER BY CASE WHEN M.ADT_TYPE = 'MAIN' THEN 1 ELSE 0 END DESC, M.SEQ_NO) R FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
		JOIN CBD_IN_CEM_BU_ADDRESS_T ADDR ON ADDR.CLU_CODE = BU.CLU_CODE
		JOIN CBD_IN_CEM_BU_MEDIA_T M ON M.CLU_CODE = BU.CLU_CODE AND M.BUAH_TK = ADDR.BUAH_TK
		WHERE (BU.VALID_TO IS NULL OR BU.VALID_TO > @now)
		AND (ADDR.VALID_TO IS NULL OR ADDR.VALID_TO > @now)
		AND (M.VALID_TO IS NULL OR M.VALID_TO > @now)
		AND BU.VALID_FROM <= @now
		AND ADDR.VALID_FROM <= @now
		AND M.VALID_FROM <= @now
		AND (BU.DELETE_DATE IS NULL OR BU.DELETE_DATE > @now)
		AND (ADDR.DELETE_DATE IS NULL OR ADDR.DELETE_DATE > @now)
		AND (M.DELETE_DATE IS NULL OR M.DELETE_DATE > @now)
		AND M.MEDT_TYPE = 'TEL'
	) X
	WHERE X.R = 1
	AND (X.CLUT_TYPE + ' ' + X.CLU_CODE) = @cluCode
END

GO


-- =============================================
-- Author:		Johan Weinitz
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CBD_Get_TaxReg] 
	-- Add the parameters for the stored procedure here
	@cluCode NVARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @now DATE = GETDATE()

	SELECT X.BUTRGH_TK, X.NAME, X.TAX_REG_NO, X.TRGT_TYPE, X.SRC_NAME_INS FROM (
		SELECT BU.[NAME], T.*, ROW_NUMBER() OVER (PARTITION BY T.CLU_CODE ORDER BY CASE WHEN T.TRGT_TYPE = 'DV' THEN 1 ELSE 0 END DESC, T.BUTRGH_TK) R FROM CBD_IN_CEM_BUSINESS_UNIT_T BU
		JOIN CBD_IN_CEM_E_BU_TAX_REG_T T ON T.CLU_CODE = BU.CLU_CODE
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


-- =============================================
-- Author:		Johan Weinitz
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CBD_Get_TaxRegType] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	SELECT 'DV' TaxRegType
	UNION ALL
	SELECT 'GST' TaxRegType
	UNION ALL
	SELECT 'EV' TaxRegType
	UNION ALL
	SELECT 'CT' TaxRegType

END


GO
ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH CHECK CHECK CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_tblCase];

ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH CHECK CHECK CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_tblCaseSections];

ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH CHECK CHECK CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_ExtendedCaseData];

ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH CHECK CHECK CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSolution];

ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH CHECK CHECK CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSections];

ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH CHECK CHECK CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_ExtendedCaseForms];

ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_tblComputerUsersCategory];

ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_ExtendedCaseForms];

ALTER TABLE [dbo].[tblComputerUsersCategory] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUsersCategory_tblComputerUsersCategory];

ALTER TABLE [dbo].[tblComputerUsersCategory] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUsersCategory_ExtendedCaseForms1];

ALTER TABLE [dbo].[tblComputerUsersCategory] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUsersCategory_tblCustomer];

ALTER TABLE [dbo].[tblComputerUsersCategory] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUsersCategory_ExtendedCaseForms];

ALTER TABLE [dbo].[tblComputerUsers] WITH CHECK CHECK CONSTRAINT [FK_tblComputerUsers_tblComputerUsersCategory];

