--update DB from 5.3.52 to 5.3.53 version

RAISERROR ('Add Column ExtendedCaseForm_Id to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.ExtendedCaseTranslations','ExtendedCaseForm_Id') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[ExtendedCaseTranslations]
	ADD [ExtendedCaseForm_Id] INT NULL

	ALTER TABLE [dbo].[ExtendedCaseTranslations] WITH NOCHECK ADD CONSTRAINT [FK_ExtendedCaseTranslations_ExtendedCaseForms]
	FOREIGN KEY([ExtendedCaseForm_Id]) REFERENCES [dbo].[ExtendedCaseForms] ([Id])

END
GO

RAISERROR ('Add Control.Filuppladdning Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.Filuppladdning')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.Filuppladdning'
           ,'Filuppladdning')
		   END
GO
RAISERROR ('Add Control.Filuppladdning English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.Filuppladdning')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.Filuppladdning'
           ,'File upload')
		   END
GO


RAISERROR ('Add Message.DraFilerHit Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Message.DraFilerHit')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Message.DraFilerHit'
           ,'Dra filer hit')
		   END
GO
RAISERROR ('Add Message.DraFilerHit English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Message.DraFilerHit')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Message.DraFilerHit'
           ,'Drop files here')
		   END
GO

RAISERROR ('Add Tab.Fliknamn Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Tab.Fliknamn')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Tab.Fliknamn'
           ,'Fliknamn')
		   END
GO
RAISERROR ('Add Tab.Fliknamn English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Tab.Fliknamn')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Tab.Fliknamn'
           ,'Tab Name')
		   END
GO

RAISERROR ('ALTER PROCEDURE [dbo].[EC_Get_Initiator_By_Name]', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[EC_Get_Initiator_By_Name]', 'P') IS NOT NULL)
DROP PROCEDURE  [dbo].[EC_Get_Initiator_By_Name]
GO

CREATE PROCEDURE [dbo].[EC_Get_Initiator_By_Name] 
(
	@name nvarchar(512),
	@customerGuid uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @customerId INT
	SELECT @customerId = Id FROM tblCustomer WHERE CustomerGuid = @customerGuid

	DECLARE @Id INT,
	 @UserId nvarchar(50),
	 @RegionId int,
	 @DepartmentId INT,
	 @DivisionId INT, 
	 @OU_Id int,
	 @CostCenter nvarchar(50),
	 @OU nvarchar(100),
	 @RegionName nvarchar(200),
	 @DepartmentName nvarchar(200)	 

	select top 1 
	     @Id = ISNULL(Id, 0),
	   	@DepartmentId = ISNULL(Department_Id, 0),
		--@OU_Id = ISNULL(OU_Id, 0) -- OU is it ok to use it instead?		
		@OU_Id = OU_Id
	from tblComputerUsers cu 
	where cu.Customer_Id = @customerId
	AND cu.UserId = @name

	IF (@Id > 0)
	BEGIN 
	   -- Department and Region:
	   IF (@DepartmentId > 0)
	   BEGIN 
		  -- get department info
		  SELECT @RegionId = ISNULL(d.Region_Id,0), @DepartmentName = d.Department FROM dbo.tblDepartment d WHERE d.Id = @DepartmentId AND Customer_Id = @customerId

		  -- get region info
		  IF @RegionId > 0
		  BEGIN
			 SELECT @RegionName = Region from tblRegion where Id = @RegionId
		  END
	   END

	   -- OU
	   IF (@OU_Id > 0)
	   BEGIN 
	    --SELECT @OU = OU FROM tblOU WHERE Id = @OU_Id --Commented out, replaced by below:

		-- get ou info
		SELECT @DepartmentId = ISNULL(o.Department_Id,0), @OU = o.OU FROM tblOU o WHERE o.Id = @OU_Id
	   END	
	   

	END

	select top 1 
	    Isnull(cu.UserId, '') as UserId ,
	    ISNULL(cu.UserCode, '') AS UserCode,
	    Isnull(cu.FirstName, '') as FirstName,
	    Isnull(cu.SurName, '') as LastName,
	    Isnull(cu.Email, '') as Email,
	    Isnull(cu.Phone, '') as Phone,
	    Isnull(cu.Cellphone, '') as Mobile,
	    Isnull(cu.Location, '') as Place,	    
	    Department_Id AS DepartmentId,
	    ISNULL(@DepartmentName, '') AS Department,
	    @RegionId AS RegionId,
	    ISNULL(@RegionName, '') AS Region,
	    @OU_Id AS OU_Id,
	    ISNULL(@OU, '') AS OU,
	    Isnull(cu.CostCentre, '') as CostCentre 
	    --Division_Id? 
	from tblComputerUsers cu 
	where Id = @Id	
END
GO




  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.53'
GO