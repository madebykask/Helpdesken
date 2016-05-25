-- update DB from 5.3.23 to 5.3.24 version

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDateFrom' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD FinishingDateFrom datetime NULL                                                                            
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDateUntil' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD FinishingDateUntil datetime NULL                                                                             
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Departments' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD Departments nvarchar(200) NULL                                                                              
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CaseTypes' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD CaseTypes nvarchar(200) NULL                                                                                   
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ProductAreas' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD ProductAreas nvarchar(200) NULL                                                                             
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroups' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD WorkingGroups nvarchar(200) NULL                                                                         
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Selection' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD Selection int NOT NULL Default(5)
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ScheduleTime' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD ScheduleTime numeric(8,2) NOT NULL Default(0.0)
                             end
GO

-- New field in tblQuestionnaireCircular
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Filter' and sysobjects.name = N'tblQuestionnaireCircular')
                             begin
                                                          ALTER TABLE tblQuestionnaireCircular ADD Filter int NOT NULL Default(0)
                             end
GO


-- New field in tblUsers
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'InventoryPermission' and sysobjects.name = N'tblUsers')
	ALTER TABLE tblUsers ADD InventoryPermission int NOT NULL Default(0)
GO

UPDATE tblusers 
SET InventoryPermission = 1 
WHERE UserGroup_Id = 4

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.24'
