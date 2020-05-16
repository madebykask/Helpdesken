--update DB from 5.3.46 to 5.3.47 version

if not exists (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblInventory_tblComputerType]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[tblInventory]'))
END

RAISERROR('Insert My department cases feature toggle to tblFeatureToggle', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM tblFeatureToggle WHERE [StrongName] = 'DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES')
BEGIN
	INSERT INTO tblFeatureToggle([StrongName], [Active], [Description]) VALUES('DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES', 
	0,'Disable the the user interface for administration of initiators access to department cases. Toggle disables the admin interface, not the functionality.')

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.47'
GO

