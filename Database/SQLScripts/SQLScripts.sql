--update DB from 5.3.40 to 5.3.41 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowOnMobile' and sysobjects.name = N'tblCaseSolution')
BEGIN
    ALTER TABLE tblCaseSolution
    ADD ShowOnMobile int NOT NULL DEFAULT(0)        
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'InventoryViewPermission' and sysobjects.name = N'tblUsers')
BEGIN
    ALTER TABLE tblUsers
    ADD InventoryViewPermission int NOT NULL DEFAULT(0)        
END
GO

--Update Users with InventoryViewPermission = 1 where InventoryPermission = 1
Update tblUsers Set InventoryViewPermission = 1 
Where InventoryPermission = 1

RAISERROR('Creating index IX_tblCaseHistory_Case_Id', 10, 1) WITH NOWAIT
if not exists (SELECT name FROM sysindexes WHERE name = 'IX_tblCaseHistory_Case_Id')	
    CREATE NONCLUSTERED INDEX [IX_tblCaseHistory_Case_Id] ON [dbo].[tblCaseHistory]
    (
	   [Case_Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    GO
GO

-- Create feature toogle
RAISERROR('Adding Feature toggle table tblFeatureToggle', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblFeatureToggle' AND type='U')
BEGIN
	CREATE TABLE [dbo].[tblFeatureToggle](
		[StrongName] [nvarchar](100) NOT NULL,
		[Active] [bit] NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[ChangeDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_tblFeatureToggle] PRIMARY KEY CLUSTERED 
	(
		[StrongName] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[tblFeatureToggle] ADD  CONSTRAINT [DF_tblFeatureToggle_Active]  DEFAULT ((0)) FOR [Active]
	ALTER TABLE [dbo].[tblFeatureToggle] ADD  CONSTRAINT [DF_tblFeatureToggle_Description]  DEFAULT ('') FOR [Description]
	ALTER TABLE [dbo].[tblFeatureToggle] ADD  CONSTRAINT [DF_tblFeatureToggle_ChangeDate]  DEFAULT (getutcdate()) FOR [ChangeDate]
END
GO
RAISERROR('Adding Feature toggle trigger trFeatureToggleChange', 10, 1) WITH NOWAIT
IF EXISTS (SELECT * FROM sysobjects WHERE name='trFeatureToggleChange' AND type='TR')
BEGIN	
	DROP TRIGGER [dbo].[trFeatureToggleChange] 
END
GO
CREATE TRIGGER [dbo].[trFeatureToggleChange] 
	ON  [dbo].[tblFeatureToggle] 
	AFTER UPDATE
AS 
BEGIN
	DECLARE @strongName NVARCHAR(MAX)
	UPDATE FT SET ChangeDate = GETUTCDATE() FROM inserted U
	JOIN tblFeatureToggle FT ON U.StrongName = FT.StrongName
END

RAISERROR('Adding feature toggle REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT * FROM tblFeatureToggle WHERE StrongName = 'REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH')
BEGIN
	INSERT INTO [tblFeatureToggle](StrongName, Active, [Description]) 
	VALUES (
		'REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH',
		0,
		'Should the report generator use the previous implementation of the search method'
	)
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.41'
--ROLLBACK --TMP






