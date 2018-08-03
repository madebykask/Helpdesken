--update DB from 5.3.37 to 5.3.38 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsEmpty' and sysobjects.name = N'tblComputerUsersCategory')
BEGIN
    ALTER TABLE tblComputerUsersCategory
    ADD IsEmpty bit NOT NULL DEFAULT(0)        
END
GO

RAISERROR ('Adding UserSearchCategory_Id case field setting to tblCaseFieldSettings', 10, 1) WITH NOWAIT
;WITH cus as 
(select fs1.Customer_Id as CustomerId
 from tblCaseFieldSettings fs1
 where NOT EXISTS
	  (
		  select 1 from tblCaseFieldSettings fs2 
		  where fs2.Customer_Id = fs1.Customer_Id 
		  AND fs2.CaseField = 'UserSearchCategory_Id'
	  )
       AND fs1.Customer_Id IS NOT NULL
GROUP BY fs1.Customer_Id) 
INSERT INTO tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, Locked)
select cus.CustomerId, 'UserSearchCategory_Id', 0, 0, 0, 0, '', null, 0, 0 
from cus
GO

RAISERROR ('Adding IsAbout_UserSearchCategory_Id case field setting to tblCaseFieldSettings', 10, 1) WITH NOWAIT
GO
;WITH cus as 
(select fs1.Customer_Id as CustomerId
 from tblCaseFieldSettings fs1
 where NOT EXISTS
	  (
		  select 1 from tblCaseFieldSettings fs2 
		  where fs2.Customer_Id = fs1.Customer_Id 
		  AND fs2.CaseField = 'IsAbout_UserSearchCategory_Id'
	  )
       AND fs1.Customer_Id IS NOT NULL
GROUP BY fs1.Customer_Id) 
INSERT INTO tblCaseFieldSettings (Customer_Id, CaseField, Show, [Required], ShowExternal, FieldSize, RelatedField, DefaultValue, ListEdit, Locked)
select cus.CustomerId, 'IsAbout_UserSearchCategory_Id', 0, 0, 0, 0, '', null, 0, 0 
from cus
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

RAISERROR ('Add column Hide on table tblCaseFieldSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'Hide' and sysobjects.name = N'tblCaseFieldSettings')
BEGIN 
  ALTER TABLE [dbo].[tblCaseFieldSettings]
  ADD Hide bit NULL  
  
  ALTER TABLE [dbo].[tblCaseFieldSettings] 
  ADD CONSTRAINT [DF_tblCaseFieldSettings_Hide]  DEFAULT(0) FOR [Hide]
  
  EXEC('UPDATE [dbo].[tblCaseFieldSettings] SET Hide = 0')

  ALTER TABLE [dbo].[tblCaseFieldSettings]
  ALTER COLUMN Hide bit NOT NULL 

END
GO 

-- remove active default constraint
IF EXISTS (select 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('tblCaseFieldSettings') and name = 'DF_tblCaseFieldSettings_Active') 
BEGIN
    ALTER TABLE tblCaseFieldSettings
    DROP CONSTRAINT DF_tblCaseFieldSettings_Active
END
GO

-- remove  active column
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
           where syscolumns.name = N'Active' and sysobjects.name = N'tblCaseFieldSettings')
BEGIN
   ALTER TABLE tblCaseFieldSettings
   DROP COLUMN Active;
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
    -- DROP DEFAULT CONSTRAINT FIRSTr
    DECLARE @ObjectName NVARCHAR(100)
    SELECT @ObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS
    WHERE [object_id] = OBJECT_ID('[dbo].[tblCaseSections]') AND [name] = 'ShowUserSearchCategory';
    EXEC('ALTER TABLE [dbo].[tblCaseSections] DROP CONSTRAINT ' + @ObjectName)

    ALTER TABLE tblCaseSections
    DROP COLUMN ShowUserSearchCategory
END
GO

RAISERROR ('Adding column UniqueMessageId to tblMail2Ticket', 10, 1) WITH NOWAIT
IF not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'UniqueMessageId' and sysobjects.name = N'tblMail2Ticket')
BEGIN
    ALTER TABLE dbo.tblMail2Ticket
    ADD UniqueMessageId nvarchar(100) null
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.38'
--ROLLBACK --TMP

  

