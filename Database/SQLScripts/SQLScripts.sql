-- update DB from 5.3.24 to 5.3.25 version
--New fields in tblFormField
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Label' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD Label nvarchar(200) NULL
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Show' and sysobjects.name = N'tblFormField')
	ALTER TABLE tblFormField ADD Show int NOT NULL Default(0)
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 53, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 53)
GO


IF COL_LENGTH('tblCaseSolution','IsAbout_ReportedBy') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_ReportedBy] nvarchar(40) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 65, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 65)
GO

 IF COL_LENGTH('tblCaseSolution','IsAbout_PersonsName') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_PersonsName] nvarchar(50) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 61, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 61)
GO

IF COL_LENGTH('tblCaseSolution','IsAbout_PersonsEmail') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_PersonsEmail] nvarchar(100) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 60, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 60)
GO

 IF COL_LENGTH('tblCaseSolution','IsAbout_PersonsPhone') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_PersonsPhone] nvarchar(50) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 62, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 62)
GO

IF COL_LENGTH('tblCaseSolution','IsAbout_PersonsCellPhone') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_PersonsCellPhone] nvarchar(30) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 59, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 59)
GO
 
 IF COL_LENGTH('tblCaseSolution','IsAbout_Region_Id') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_Region_Id] int null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 64, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 64)
GO

IF COL_LENGTH('tblCaseSolution','IsAbout_Department_Id') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_Department_Id] int null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 57, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 57)
GO

IF COL_LENGTH('tblCaseSolution','IsAbout_OU_Id') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_OU_Id] int null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 58, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 58)
GO



 IF COL_LENGTH('tblCaseSolution','IsAbout_Place') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_Place] nvarchar(100) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 63, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 63)
GO
 

IF COL_LENGTH('tblCaseSolution','IsAbout_CostCentre') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_CostCentre] nvarchar(50) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 56, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 56)
GO

IF COL_LENGTH('tblCaseSolution','IsAbout_UserCode') IS NULL
begin
    alter table tblCaseSolution 
	add [IsAbout_UserCode] nvarchar(20) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 66, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 66)
GO
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.25'

