--update DB from 5.3.37 to 5.3.38 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsEmpty' and sysobjects.name = N'tblComputerUsersCategory')
BEGIN
    ALTER TABLE tblComputerUsersCategory
    ADD IsEmpty bit NOT NULL DEFAULT(0)    

    -- create empty category for customer
    --INSERT INTO tblComputerUsersCategory(Name, ComputerUsersCategoryGuid, IsReadOnly, CustomerID, IsEmpty)
    --VALUES('NULL', newid(), 0, 1, 1)
END
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.38'
--ROLLBACK --TMP


