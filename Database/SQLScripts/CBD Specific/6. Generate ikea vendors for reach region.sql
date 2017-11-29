BEGIN TRAN

DECLARE @customerID INT = 39, 
	@now DATETIME = GETDATE()

INSERT INTO tblRegion(Customer_Id, Region, Status, IsDefault, CreatedDate, ChangedDate, RegionGUID, LanguageId)
SELECT @customerID, X.Region, 1, 0, @now, @now, NEWID(), 0 FROM (
	SELECT GA_CODE_CTY Region FROM CBD_IN_CEM_BU_ADDRESS_T ADDR
	GROUP BY GA_CODE_CTY
) X
LEFT JOIN tblRegion R ON X.Region = R.Region AND R.Customer_Id = @customerID
WHERE R.Id IS NULL 


INSERT INTO tblDepartment(Customer_id, DepartmentGUID, Department, DepartmentId, Region_Id, [Status])
SELECT @customerID, NEWID(), 'IKEA Vendor', 'IKEA Vendor', R.Id, 1 FROM tblRegion R
JOIN (
	SELECT GA_CODE_CTY Region FROM CBD_IN_CEM_BU_ADDRESS_T ADDR
	GROUP BY GA_CODE_CTY
) X ON X.Region = R.Region AND R.Customer_Id = @customerID
LEFT JOIN tblDepartment D ON D.Region_Id = R.Id AND D.DepartmentId = 'IKEA Vendor'
WHERE D.Id IS NULL

SELECT * FROM tblRegion R
JOIN tblDepartment D ON D.Region_Id = R.ID
WHERE D.DepartmentId = 'IKEA Vendor'
--SELECT * FROM tblCustomer C
--SELECT * FROM tblDepartment D


COMMIT