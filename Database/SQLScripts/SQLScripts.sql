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


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.39'
--ROLLBACK --TMP

  

