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

RAISERROR ('Add column Active on table tblCaseFieldSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Active' and sysobjects.name = N'tblCaseFieldSettings')
BEGIN 
  ALTER TABLE [dbo].[tblCaseFieldSettings]
  ADD Active bit NULL  
  
  ALTER TABLE [dbo].[tblCaseFieldSettings] 
  ADD CONSTRAINT [DF_tblCaseFieldSettings_Active]  DEFAULT(0) FOR [Active]
  
  EXEC('UPDATE [dbo].[tblCaseFieldSettings] SET Active = 0')

  ALTER TABLE [dbo].[tblCaseFieldSettings]
  ALTER COLUMN Active bit NOT NULL 

END
GO 

RAISERROR ('Remove column DefaultUserSearchCategory on table tblCaseSections', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
           where syscolumns.name = N'DefaultUserSearchCategory' and sysobjects.name = N'tblCaseSections')
BEGIN
    ALTER TABLE tblCaseSections
    DROP COLUMN DefaultUserSearchCategory
END
GO

RAISERROR ('Remove column ShowUserSearchCategory on table tblCaseSections', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
           where syscolumns.name = N'ShowUserSearchCategory' and sysobjects.name = N'tblCaseSections')
BEGIN
    -- DROP DEFAULT CONSTRAINT FIRST
    DECLARE @ObjectName NVARCHAR(100)
    SELECT @ObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS
    WHERE [object_id] = OBJECT_ID('[dbo].[tblCaseSections]') AND [name] = 'ShowUserSearchCategory';
    EXEC('ALTER TABLE [dbo].[tblCaseSections] DROP CONSTRAINT ' + @ObjectName)

    ALTER TABLE tblCaseSections
    DROP COLUMN ShowUserSearchCategory
END
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.38'
--ROLLBACK --TMP

  