--update DB from 5.3.37 to 5.3.38 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsEmpty' and sysobjects.name = N'tblComputerUsersCategory')
BEGIN
    ALTER TABLE tblComputerUsersCategory
    ADD IsEmpty bit NOT NULL DEFAULT(0)    

    -- todo: check if new category should be added for all customers
    --INSERT INTO tblComputerUsersCategory(Name, ComputerUsersCategoryGuid, IsReadOnly, CustomerID, IsEmpty)
    --VALUES('NULL', newid(), 0, <customer_id>, 1)
END
GO

RAISERROR ('Adding column DefaultUserSearchCategory on table tblCaseSections', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'DefaultUserSearchCategory' and sysobjects.name = N'tblCaseSections')
BEGIN
    ALTER TABLE tblCaseSections
    ADD DefaultUserSearchCategory int NULL     
END
GO

RAISERROR ('Adding column ShowUserSearchCategory on table tblCaseSections', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowUserSearchCategory' and sysobjects.name = N'tblCaseSections')
BEGIN
    ALTER TABLE tblCaseSections
    ADD ShowUserSearchCategory bit NOT NULL DEFAULT(0)
END
GO

RAISERROR ('Adding column UserSearchCategory_Id to tblCaseSolution', 10, 1) WITH NOWAIT
IF not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UserSearchCategory_Id' and sysobjects.name = N'tblCaseSolution')
BEGIN
   
  ALTER TABLE tblCaseSolution
  ADD UserSearchCategory_Id int NULL
  
  -- add case solution setting 
  INSERT INTO tblCaseSolutionFieldSettings (CaseSolution_Id, FieldName_Id, Mode, CreatedDate, ChangedDate)
  SELECT cs2.CaseSolution_Id, 69, cs2.Mode, GETDATE(), GETDATE()		
  FROM tblCaseSolutionFieldSettings as cs2
  LEFT JOIN tblCaseSolutionFieldSettings as cs1 on cs2.CaseSolution_Id = cs1.CaseSolution_Id AND cs1.FieldName_Id = 69
  WHERE cs2.FieldName_Id = 17 
  AND   cs1.FieldName_Id is NULL

END
GO

RAISERROR ('Adding column IsAbout_UserSearchCategory_Id to tblCaseSolution', 10, 1) WITH NOWAIT
IF not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'IsAbout_UserSearchCategory_Id' and sysobjects.name = N'tblCaseSolution')   
BEGIN

  ALTER TABLE tblCaseSolution
  ADD IsAbout_UserSearchCategory_Id int NULL
  
  -- add case solution setting   
  INSERT INTO tblCaseSolutionFieldSettings (CaseSolution_Id, FieldName_Id, Mode, CreatedDate, ChangedDate)
  SELECT cs2.CaseSolution_Id, 70, cs2.Mode, GETDATE(), GETDATE()		
  FROM tblCaseSolutionFieldSettings as cs2
  LEFT JOIN tblCaseSolutionFieldSettings as cs1 on cs2.CaseSolution_Id = cs1.CaseSolution_Id AND cs1.FieldName_Id = 70
  WHERE cs2.FieldName_Id = 17 
  AND   cs1.FieldName_Id is NULL

END
GO


RAISERROR ('Updating tblCaseSolution.Caption column to nvarchar(100)', 10, 1) WITH NOWAIT
IF exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Caption' and sysobjects.name = N'tblCaseSolution')   
BEGIN

  ALTER TABLE tblCaseSolution
  ALTER COLUMN Caption nvarchar(100) NOT NULL

END
GO




-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.38'
--ROLLBACK --TMP

