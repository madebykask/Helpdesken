--update DB from 5.3.38 to 5.3.39 version

-- New column Workinggroup_Id in tblCaseType
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WorkingGroup_Id' and sysobjects.name = N'tblCaseType')
ALTER TABLE tblCaseType
    ADD WorkingGroup_Id INTEGER,
    FOREIGN KEY(WorkingGroup_Id) REFERENCES tblWorkingGroup(Id)
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'FK_tblCaseType_tblWorkingGroup') AND type = 'F')
ALTER TABLE [dbo].tblCaseType  WITH NOCHECK ADD  CONSTRAINT [FK_tblCaseType_tblWorkingGroup] FOREIGN KEY([WorkingGroup_Id])
REFERENCES [dbo].[tblWorkingGroup] ([Id])
GO

ALTER TABLE [dbo].tblCaseType CHECK CONSTRAINT [FK_tblCaseType_tblWorkingGroup]
GO


--tblSettings
ALTER TABLE tblSettings
ALTER COLUMN POP3UserName NVARCHAR(50) not null

--tblCaseHistory
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsAbout_Persons_EMail' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory
    ADD IsAbout_Persons_EMail NVARCHAR(100)       
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsAbout_Persons_CellPhone' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory
    ADD IsAbout_Persons_CellPhone NVARCHAR(50)       
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsAbout_Region_Id' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory
    ADD IsAbout_Region_Id INT       
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsAbout_OU_Id' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory
    ADD IsAbout_OU_Id INT       
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsAbout_CostCentre' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory
    ADD IsAbout_CostCentre NVARCHAR(50)      
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'IsAbout_Place' and sysobjects.name = N'tblCaseHistory')
BEGIN
    ALTER TABLE tblCaseHistory
    ADD IsAbout_Place NVARCHAR(100) NULL    
END
GO

-- changing tblContractFile.ContentType size to 100
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
            where syscolumns.name = N'ContentType' and sysobjects.name = N'tblContractFile')
BEGIN
    ALTER TABLE tblContractFile
    ALTER COLUMN ContentType nvarchar(100) NOT NULL
END

-- Update Null or empty user pw
Update tblUsers
set [Password] = 'DH&HD2018'
Where ([Password] is null or [Password] = '')

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.39'
--ROLLBACK --TMP


