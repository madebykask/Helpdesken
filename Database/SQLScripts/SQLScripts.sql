-- update DB from 5.3.10.XX to 5.3.11.xx version

if exists(select * from sysobjects WHERE Name = N'UserGridSettings') 
	AND not exists(Select A.name IndexName,A.index_id,c.name ColumnName     
		From sys.indexes A  
		Inner Join sys.index_columns B On A.object_id = B.object_id And A.index_id = B.index_id 
		Inner Join sys.columns C On c.object_id = B.object_id  And C.column_id  = B.column_id 
		Where A.Object_ID = OBJECT_ID('UserGridSettings') and a.name = 'PK_UserGridSettings' and c.name = 'id')
BEGIN
	ALTER TABLE [dbo].[UserGridSettings] DROP CONSTRAINT [PK_UserGridSettings];

	CREATE NONCLUSTERED INDEX [IDX_UserSettings([CustomerId,UserId,GridId,FieldId)] ON [dbo].[UserGridSettings] (
		[CustomerId] ASC,
		[UserId] ASC,
		[GridId] ASC,
		[FieldId] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

	ALTER TABLE [dbo].[UserGridSettings] ADD  CONSTRAINT [PK_UserGridSettings] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC	
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
END
GO

-- Task according to Dan Frieman <dan.frieman@dhsolutions.se> recomendations
if not exists(select * from sysobjects WHERE Name = N'tblUserGridSettings')
BEGIN
	exec sp_rename 'UserGridSettings', 'tblUserGridSettings';	
END
GO

-- Task according to Dan Frieman <dan.frieman@dhsolutions.se> recomendations
IF COL_LENGTH('dbo.tblUserGridSettings','UserId') IS NOT NULL
BEGIN
	EXEC sp_rename '[tblUserGridSettings].[UserId]', 'User_Id', 'COLUMN';
	EXEC sp_rename '[tblUserGridSettings].[CustomerId]', 'Customer_Id', 'COLUMN';		
	EXEC sp_rename '[tblUserGridSettings].[GridId]', 'Grid_Id', 'COLUMN';		
	EXEC sp_rename  '[tblUserGridSettings].[FieldId]', 'Field_Id', 'COLUMN';
END
GO
	
if not exists(select * from sysobjects WHERE Name = N'tblGrid')
BEGIN
	CREATE TABLE [dbo].[tblGrid](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[GridName] [nchar](32) NOT NULL,
	 CONSTRAINT [PK_tblGrid] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	insert into tblGrid values('case_overview');	

	update tblUserGridSettings set Grid_Id = '1';
		
	DROP INDEX [IDX_UserSettings([CustomerId,UserId,GridId,FieldId)] ON [dbo].[tblUserGridSettings];

	alter table tblUserGridSettings alter column Grid_Id int not null

	CREATE NONCLUSTERED INDEX [IDX_UserSettings([CustomerId,UserId,GridId,FieldId)] ON [dbo].[tblUserGridSettings]
	(
		[Customer_Id] ASC,
		[User_Id] ASC,
		[Grid_Id] ASC,
		[Field_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	ALTER TABLE [dbo].[tblUserGridSettings] ADD  CONSTRAINT [FK_UserGridSettings_tblGrid] FOREIGN KEY([Grid_Id])
	REFERENCES [dbo].[tblGrid] ([Id]) 
		ON DELETE CASCADE
		ON UPDATE CASCADE
END
GO
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.11'
