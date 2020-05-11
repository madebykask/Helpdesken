--update DB from 5.3.45 to 5.3.46 version

RAISERROR ('Add ShowOnExtPageDepartmentCases to tblComputerUsers table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ShowOnExtPageDepartmentCases' and sysobjects.name = N'tblComputerUsers')
BEGIN
    ALTER TABLE tblComputerUsers
    ADD ShowOnExtPageDepartmentCases bit not null default 0 
END

RAISERROR('Creating report ''Report - Report Generator - Extended Case'' to customers', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM tblReport WHERE ID = 30)
BEGIN
	INSERT INTO tblReport(Id, ReportGUID) VALUES(30, 'D95FF512-78E9-48F9-B515-12F2FB00E786')
END

RAISERROR('Adding Report - Report Generator - Extended Case to customers', 10, 1) WITH NOWAIT
INSERT INTO tblReport_tblCustomer(Customer_Id, Report_Id, Show)
SELECT C.Id, 30, 0 FROM tblCustomer C
LEFT JOIN tblReport_tblCustomer RC ON RC.Customer_Id = C.Id AND RC.Report_Id = 30
WHERE RC.Report_Id IS NULL


RAISERROR ('Add UseMobileRouting to tblGlobalSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UseMobileRouting' and sysobjects.name = N'tblGlobalSettings')
BEGIN
    ALTER TABLE tblGlobalSettings
    ADD UseMobileRouting bit not null default 0 
END

RAISERROR ('Add FileUploadExtensionWhitelist to tblGlobalSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'FileUploadExtensionWhitelist' and sysobjects.name = N'tblGlobalSettings')
BEGIN
    ALTER TABLE tblGlobalSettings
    ADD FileUploadExtensionWhitelist nvarchar(3072)  
END

RAISERROR ('Remove default constraint on UseMobileRouting', 10, 1) WITH NOWAIT
IF EXISTS (SELECT dc.name FROM sys.default_constraints dc 
	JOIN sys.columns c ON c.default_object_id = dc.object_id
	WHERE c.[name] LIKE 'UseMobileRouting'
	AND dc.parent_object_id = OBJECT_ID('dbo.tblSettings'))
BEGIN
	DECLARE @sql NVARCHAR(MAX)

	SELECT TOP 1 @sql = N'alter table dbo.tblSettings drop constraint ['+dc.NAME+N']'
	from sys.default_constraints dc
	JOIN sys.columns c
		ON c.default_object_id = dc.object_id
	WHERE 
		dc.parent_object_id = OBJECT_ID('dbo.tblSettings')
	AND c.name LIKE 'UseMobileRouting'

	EXECUTE(@sql)
END 

RAISERROR ('Remove UseMobileRouting on tblSettings', 10, 1) WITH NOWAIT
if  exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UseMobileRouting' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    DROP COLUMN UseMobileRouting 
END



RAISERROR ('Add clustered index to tblDepartment', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes WHERE [type]=1 AND object_id = OBJECT_ID('dbo.tblDepartment'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblDepartment_PK_Clust] ON [dbo].[tblDepartment]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END


RAISERROR ('Add clustered index to tblUsers', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblUsers'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblUsers_PK_Clust] ON [dbo].[tblUsers]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblProjectSchedule', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblProjectSchedule'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblProjectSchedule_PK_Clust] ON [dbo].[tblProjectSchedule]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblCaseSettings', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblCaseSettings'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblCaseSettings_PK_Clust] ON [dbo].[tblCaseSettings]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblUsergroups', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblUsergroups'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblUsergroups_PK_Clust] ON [dbo].[tblUsergroups]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblCase', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblCase'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblCase_PK_Clust] ON [dbo].[tblCase]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblCustomer', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblCustomer'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblCustomer_PK_Clust] ON [dbo].[tblCustomer]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblProjectCollaborator', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblProjectCollaborator'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblProjectCollaborator_PK_Clust] ON [dbo].[tblProjectCollaborator]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblProjectLog', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblProjectLog'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblProjectLog_PK_Clust] ON [dbo].[tblProjectLog]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblProject', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblProject'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblProject_PK_Clust] ON [dbo].[tblProject]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblSettings', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblSettings'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblSettings_PK_Clust] ON [dbo].[tblSettings]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblLog', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblLog'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblLog_PK_Clust] ON [dbo].[tblLog]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblLogProgram', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes  WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblLogProgram'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblLogProgram_PK_Clust] ON [dbo].[tblLogProgram]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add clustered index to tblCaseHistory', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes WHERE [type]=1  AND object_id = OBJECT_ID('dbo.tblCaseHistory'))
BEGIN
    CREATE UNIQUE CLUSTERED INDEX [IDX_tblCaseHistory_PK_Clust] ON [dbo].[tblCaseHistory]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

RAISERROR ('Add tblCase index to tblCaseIsAbout', 10, 1) WITH NOWAIT
if not exists (select * from sys.indexes WHERE name='IDX_tblCaseIsAbout_Case' AND object_id = OBJECT_ID('dbo.tblCaseIsAbout'))
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [IDX_tblCaseIsAbout_Case] ON [dbo].[tblCaseIsAbout]
	(
		[Case_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END


DECLARE @MobileType INT
SET @MobileType = 500
  BEGIN TRAN
	If exists (select top 1 * from [dbo].[tblText] where [TextType] <> @MobileType and [Id] > 30000)
		BEGIN

		DECLARE @MyCursor CURSOR;
		DECLARE @newId INT
		DECLARE @moveId INT

		SET @MyCursor = CURSOR FOR
		SELECT DISTINCT [Id] FROM [dbo].[tblText]
		WHERE [TextType] <> @MobileType and [Id] > 30000

		OPEN @MyCursor 
	    FETCH NEXT FROM @MyCursor 
	    INTO @moveId

		WHILE @@FETCH_STATUS = 0
		    BEGIN

			SELECT TOP 1 @newId = T.Id+1 FROM tblText T
			WHERE T.Id < 30000
			ORDER BY T.Id DESC 

			RAISERROR ('Updating text translation Id = %d to %d.', 10, 1, @moveId, @NewId) WITH NOWAIT
			INSERT INTO tblText(Id, ChangedByUser_Id, ChangedDate, CreatedDate, TextGUID, TextString, TextType)
			SELECT @newId,ChangedByUser_Id, ChangedDate, CreatedDate, TextGUID, TextString, TextType FROM tblText T 
			WHERE T.Id = @moveId 

			UPDATE TT SET Text_Id = @newId FROM tblTextTranslation TT WHERE Text_Id = @moveId

			DELETE FROM tblText WHERE ID = @moveId

			FETCH NEXT FROM @MyCursor 
		    INTO @moveId 
		END

		CLOSE @MyCursor;
	    DEALLOCATE @MyCursor;
	end
COMMIT 

RAISERROR ('Add EwsApplicationId to tblSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'EwsApplicationId' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    ADD EwsApplicationId NVARCHAR(200)
END

RAISERROR ('Add EwsClientSecret to tblSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'EwsClientSecret' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    ADD EwsClientSecret NVARCHAR(200)
END

RAISERROR ('Add EwsTenantId to tblSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'EwsTenantId' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    ADD EwsTenantId NVARCHAR(200)
END

RAISERROR ('Add UseEws to tblSettings', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UseEws' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    ADD UseEws bit not null default 0
END

RAISERROR ('Add ShowExternal to tblInventoryTypeProperty', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ShowExternal' and sysobjects.name = N'tblInventoryTypeProperty')
BEGIN
    ALTER TABLE tblInventoryTypeProperty
    ADD ShowExternal int not null default 0
END

RAISERROR ('Add ShowInListExternal to tblInventoryTypeProperty', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ShowInListExternal' and sysobjects.name = N'tblInventoryTypeProperty')
BEGIN
    ALTER TABLE tblInventoryTypeProperty
    ADD ShowInListExternal int not null default 0
END

RAISERROR ('Add InventoryGUID to tblInventory', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'InventoryGUID' and sysobjects.name = N'tblInventory')
BEGIN
    ALTER TABLE tblInventory
    ADD InventoryGUID uniqueidentifier  not null default newid()
END

RAISERROR ('Add ComputerType_Id to tblInventory', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ComputerType_Id' and sysobjects.name = N'tblInventory')
BEGIN
    ALTER TABLE tblInventory
	ADD ComputerType_Id int  null
END

RAISERROR ('Add ComputerType_Id FOREIGN KEY to tblInventory', 10, 1) WITH NOWAIT
if not exists (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblInventory_tblComputerType]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[tblInventory]'))
BEGIN
    ALTER TABLE tblInventory	
    WITH CHECK ADD  CONSTRAINT [FK_tblInventory_tblComputerType] FOREIGN KEY([ComputerType_Id])
REFERENCES [dbo].[tblComputerType] ([Id])
END

RAISERROR('Insert My department cases feature toggle to tblFeatureToggle', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM tblFeatureToggle WHERE [StrongName] = 'DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES')
BEGIN
	INSERT INTO tblFeatureToggle([StrongName], [Active], [Description]) VALUES('DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES', 
	0,'Disable the the user interface for administration of initiators access to department cases. Toggle disables the admin interface, not the functionality.')
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.46'
GO

