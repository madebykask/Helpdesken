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

RAISERROR ('Create PROCEDURE [dbo].[EC_Get_Initiator_By_Name]', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[EC_Get_Initiator_By_Name]', 'P') IS NULL)
EXEC('
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

		-- get ou info
		SELECT @DepartmentId = ISNULL(o.Department_Id,0), @OU = o.OU FROM tblOU o WHERE o.Id = @OU_Id
	   END	
	   

	END

	select top 1 
	    Isnull(cu.UserId, '''') as UserId ,
	    ISNULL(cu.UserCode, '''') AS UserCode,
	    Isnull(cu.FirstName, '''') as FirstName,
	    Isnull(cu.SurName, '''') as LastName,
	    Isnull(cu.Email, '''') as Email,
	    Isnull(cu.Phone, '''') as Phone,
	    Isnull(cu.Cellphone, '''') as Mobile,
	    Isnull(cu.Location, '''') as Place,	    
	    Department_Id AS DepartmentId,
	    ISNULL(@DepartmentName, '''') AS Department,
	    @RegionId AS RegionId,
	    ISNULL(@RegionName, '''') AS Region,
	    @OU_Id AS OU_Id,
	    ISNULL(@OU, '''') AS OU,
	    Isnull(cu.CostCentre, '''') as CostCentre 
	    --Division_Id? 
	from tblComputerUsers cu 
	where Id = @Id	
END')
GO

--Text_Internal in tblLog
RAISERROR ('Drop default Constraint for Text_Internal', 10, 1) WITH NOWAIT
IF OBJECT_ID('[DF_tblLog_Text_Internal]', 'D') IS NOT NULL
	begin
		ALTER TABLE [dbo].[tblLog] DROP CONSTRAINT [DF_tblLog_Text_Internal]
	end
go

RAISERROR ('Drop IsFulltextIndexed for Text_Internal', 10, 1) WITH NOWAIT
Declare @exists int
	SELECT @exists =  COLUMNPROPERTY(OBJECT_ID('[dbo].[tblLog]'), 'Text_Internal', 'IsFulltextIndexed')
	if(@exists = 1)
		begin
		--Drop
			EXEC sp_fulltext_column 
			@tabname =  'tblLog' , 
			@colname =  'Text_Internal' , 
			@action =  'drop' 
		end
go

RAISERROR ('Extend Text_Internal in tblLog lenght to nvarchar(MAX)', 10, 1) WITH NOWAIT
ALTER TABLE [dbo].[tblLog] 
alter column Text_Internal nvarchar(MAX) not null
Go

RAISERROR ('Add IsFulltextIndexed for Text_Internal', 10, 1) WITH NOWAIT
EXEC sp_fulltext_column 
@tabname =  'tblLog' , 
@colname =  'Text_Internal' ,  
@action =  'add' 	
Go

RAISERROR ('Add default Constraint for Text_Internal', 10, 1) WITH NOWAIT
ALTER TABLE [dbo].[tblLog] ADD  CONSTRAINT [DF_tblLog_Text_Internal]  DEFAULT ('') FOR [Text_Internal]
GO

--Text_External in tblLog
RAISERROR ('Drop default Constraint for Text_External (DF_tblLog_Text_External)', 10, 1) WITH NOWAIT
IF OBJECT_ID('[DF_tblLog_Text_External]', 'D') IS NOT NULL
	begin
		ALTER TABLE [dbo].[tblLog] DROP CONSTRAINT [DF_tblLog_Text_External]
	end
go

RAISERROR ('Drop default Constraint for Text_External (DF_tblLog_Text)', 10, 1) WITH NOWAIT
IF OBJECT_ID('[DF_tblLog_Text]', 'D') IS NOT NULL
	begin
		ALTER TABLE [dbo].[tblLog] DROP CONSTRAINT [DF_tblLog_Text]
	end
go

RAISERROR ('Drop IsFulltextIndexed for Text_External', 10, 1) WITH NOWAIT
Declare @exists int
	SELECT @exists =  COLUMNPROPERTY(OBJECT_ID('[dbo].[tblLog]'), 'Text_External', 'IsFulltextIndexed')
	if(@exists = 1)
		begin
		--Drop
			EXEC sp_fulltext_column 
			@tabname =  'tblLog' , 
			@colname =  'Text_External' , 
			@action =  'drop' 
		end
go

RAISERROR ('Extend Text_External in tblLog lenght to nvarchar(MAX)', 10, 1) WITH NOWAIT
ALTER TABLE [dbo].[tblLog] 
alter column Text_External nvarchar(MAX) not null
Go

RAISERROR ('Add IsFulltextIndexed for Text_External', 10, 1) WITH NOWAIT
EXEC sp_fulltext_column 
@tabname =  'tblLog' , 
@colname =  'Text_External' ,  
@action =  'add' 	
Go

RAISERROR ('Add default Constraint for Text_External', 10, 1) WITH NOWAIT
ALTER TABLE [dbo].[tblLog] ADD  CONSTRAINT [DF_tblLog_Text_External]  DEFAULT ('') FOR [Text_External]
GO

RAISERROR ('Add Control.Radioknapp Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.Radioknapp')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.Radioknapp'
           ,'Radioknapp')
		   END
GO
RAISERROR ('Add Control.Radioknapp English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.Radioknapp')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.Radioknapp'
           ,'Radio Button')
		   END
GO

RAISERROR ('Add Control.Kryssruta Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.Kryssruta')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.Kryssruta'
           ,'Kryssruta')
		   END
GO
RAISERROR ('Add Control.Kryssruta English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.Kryssruta')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.Kryssruta'
           ,'Checkbox')
		   END
GO

RAISERROR ('Add Control.Rullgardinsmeny Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.Rullgardinsmeny')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.Rullgardinsmeny'
           ,'Rullgardinsmeny')
		   END
GO
RAISERROR ('Add Control.Rullgardinsmeny English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.Rullgardinsmeny')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.Rullgardinsmeny'
           ,'Dropdown Menu')
		   END
GO

RAISERROR ('Add DataSource.Value.Val Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'DataSource.Value.Val')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'DataSource.Value.Val'
           ,'Val')
		   END
GO
RAISERROR ('Add DataSource.Value.Val English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'DataSource.Value.Val')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'DataSource.Value.Val'
           ,'Option')
		   END
GO

RAISERROR ('Create Stored procedure [dbo].[EC_Get_DepartmentsByCustomer]', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[EC_Get_DepartmentsByCustomer]', 'P') IS NULL)
EXEC('
CREATE PROCEDURE [dbo].[EC_Get_DepartmentsByCustomer]
(@CustomerGuid uniqueIdentifier)
AS
BEGIN
	SET NOCOUNT ON;

	
	Declare @Customer_Id int

	select @Customer_Id = Id from tblCustomer where CustomerGUID = @CustomerGuid

	if (@Customer_Id is not null)
	begin

			SELECT null as [Value], '''' as [Text] UNION ALL
			select Id As Value , Department As [TEXT]  from tblDepartment where Customer_id =  @Customer_Id and [Status] = 1  order by [TEXT] ASC
	end
	else
	begin
		SELECT null as [Value], '''' as [Text]
	end
END')
GO


RAISERROR ('Create Stored procedure [dbo].[EC_Get_OusByDepartmentDs]', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[EC_Get_OusByDepartmentDs]', 'P') IS NULL)
EXEC('
CREATE PROCEDURE [dbo].[EC_Get_OusByDepartmentDs](
    @CustomerGuid uniqueIdentifier, 
    @Department_Id int = 0
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Customer_Id int
	SELECT @Customer_Id = Id FROM tblCustomer WHERE CustomerGUID = @CustomerGuid
     
	IF (@Customer_Id is null or @Department_Id = 0)
	BEGIN
		SELECT '''' as Id, '''' as OU, '''' as Code
	END
	ELSE
	BEGIN
		SELECT '''' as Id, '''' as OU, '''' as Code
		UNION ALL
        SELECT Cast(Id as nvarchar(10)), OU, Code
        FROM tblOU
        WHERE Department_Id = @Department_Id AND 
	          Parent_OU_Id IS NULL AND 
	          [Status] = 1
        ORDER BY OU ASC;	
	END
END')
GO

RAISERROR ('Create Stored procedure [dbo].[EC_Get_RegionsByCustomer]', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[EC_Get_RegionsByCustomer]', 'P') IS NULL)
EXEC('
CREATE PROCEDURE [dbo].[EC_Get_RegionsByCustomer]
(@CustomerGuid uniqueIdentifier)
AS
BEGIN
	SET NOCOUNT ON;

	
	Declare @Customer_Id int

	select @Customer_Id = Id from tblCustomer where CustomerGUID = @CustomerGuid

	if (@Customer_Id is not null)
	begin

			SELECT null as [Value], '''' as [Text] UNION ALL
			select Id As Value , Region As [TEXT]  from tblRegion where Customer_id =  @Customer_Id and [Status] = 1  order by [TEXT] ASC
	end
	else
	begin
		SELECT null as [Value], '''' as [Text]
	end
END')
GO

RAISERROR ('Add Control.Infofalt Swedish to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 1 AND Property = 'Control.Infofalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (1
           ,'Control.Infofalt'
           ,'Infofält')
		   END
GO
RAISERROR ('Add Control.Infofalt English to ExtendedCaseTranslations', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM ExtendedCaseTranslations WHERE LanguageId = 2 AND Property = 'Control.Infofalt')
BEGIN	 
INSERT INTO [dbo].[ExtendedCaseTranslations]
           ([LanguageId]
           ,[Property]
           ,[Text])
     VALUES
           (2
           ,'Control.Infofalt'
           ,'Info Field')
		   END
GO


RAISERROR ('Add getInitiatorByName to ExtendedCaseCustomDataSources if not exists', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT 1 FROM [dbo].[ExtendedCaseCustomDataSources] WHERE DataSourceId = 'getInitiatorByName')
BEGIN

	INSERT INTO [dbo].[ExtendedCaseCustomDataSources]
			   ([DataSourceId]
			   ,[Description]
			   ,[MetaData]
			   ,[CreatedOn]
			   ,[CreatedBy]
			   ,[UpdatedOn]
			   ,[UpdatedBy])
		 VALUES
			   ('getInitiatorByName' --<DataSourceId, nvarchar(100),>
			   ,'get initator by name' -- <Description, nvarchar(500),>
			   , '{Type: "db-sp", ProcedureName: "EC_Get_Initiator_By_Name"}'--<MetaData, nvarchar(max),>
			   , CURRENT_TIMESTAMP --<CreatedOn, datetime,>
			   , 'DHS' --<CreatedBy, nvarchar(50),>
			   , NULL --<UpdatedOn, datetime,>
			   , NULL --<UpdatedBy, nvarchar(50),>
			   )
END

RAISERROR ('Add OusByDepartmentDs to ExtendedCaseCustomDataSources if not exists', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT 1 FROM [dbo].[ExtendedCaseCustomDataSources] WHERE DataSourceId = 'OusByDepartmentDs')
BEGIN

	INSERT INTO [dbo].[ExtendedCaseCustomDataSources]
			   ([DataSourceId]
			   ,[Description]
			   ,[MetaData]
			   ,[CreatedOn]
			   ,[CreatedBy]
			   ,[UpdatedOn]
			   ,[UpdatedBy])
		 VALUES
			   ('OusByDepartmentDs' --<DataSourceId, nvarchar(100),>
			   ,'db-sp' -- <Description, nvarchar(500),>
			   , '{ Type: "db-sp", ProcedureName: "EC_Get_OusByDepartmentDs" }'--<MetaData, nvarchar(max),>
			   , CURRENT_TIMESTAMP --<CreatedOn, datetime,>
			   , 'DHS' --<CreatedBy, nvarchar(50),>
			   , NULL --<UpdatedOn, datetime,>
			   , NULL --<UpdatedBy, nvarchar(50),>
			   )
END


RAISERROR ('Add RegionsByCustomer to ExtendedCaseOptionDataSources if not exists', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT 1 FROM [dbo].[ExtendedCaseOptionDataSources] WHERE DataSourceId = 'RegionsByCustomer')
BEGIN

INSERT INTO [dbo].[ExtendedCaseOptionDataSources]
           ([DataSourceId]
           ,[Description]
           ,[MetaData]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           ( 'RegionsByCustomer' --<DataSourceId, nvarchar(100),>
           , NULL --<Description, nvarchar(500),>
           , '{Type:"db-sp", ProcedureName:"EC_Get_RegionsByCustomer"}' --<MetaData, nvarchar(max),>
           , CURRENT_TIMESTAMP --<CreatedOn, datetime,>
           , 'DHS' --<CreatedBy, nvarchar(50),>
           , NULL --<UpdatedOn, datetime,>
           , NULL --<UpdatedBy, nvarchar(50),>
		   )
END


RAISERROR ('Add DepartmentsByCustomer to ExtendedCaseOptionDataSources if not exists', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT 1 FROM [dbo].[ExtendedCaseOptionDataSources] WHERE DataSourceId = 'DepartmentsByCustomer')
BEGIN

INSERT INTO [dbo].[ExtendedCaseOptionDataSources]
           ([DataSourceId]
           ,[Description]
           ,[MetaData]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           ( 'DepartmentsByCustomer' --<DataSourceId, nvarchar(100),>
           , NULL --<Description, nvarchar(500),>
           , '{Type:"db-sp", ProcedureName:"EC_Get_DepartmentsByCustomer"}' --<MetaData, nvarchar(max),>
           , CURRENT_TIMESTAMP --<CreatedOn, datetime,>
           , 'DHS' --<CreatedBy, nvarchar(50),>
           , NULL --<UpdatedOn, datetime,>
           , NULL --<UpdatedBy, nvarchar(50),>
		   )
END



  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.53'
