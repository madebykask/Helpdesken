-- update DB from 5.3.25 to 5.3.26 version


if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LatestSLACountDate' and sysobjects.name = N'tblCase')
	ALTER TABLE tblCase ADD LatestSLACountDate DateTime NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'LatestSLACountDate' and sysobjects.name = N'tblCaseHistory')
	ALTER TABLE tblCaseHistory ADD LatestSLACountDate DateTime NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowOnCaseOverview' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD ShowOnCaseOverview int NOT NULL Default(1)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ConnectedButton' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD ConnectedButton int NULL 
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'ShowInsideCase' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD ShowInsideCase int NOT NULL Default(1)
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'SetCurrentUserAsPerformer' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD SetCurrentUserAsPerformer int NULL
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'OverWritePopUp' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD OverWritePopUp int NOT NULL Default(0)
Go



if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedDate' and sysobjects.name = N'tblInvoiceArticle_tblProductArea')
	ALTER TABLE tblInvoiceArticle_tblProductArea ADD CreatedDate DateTime NOT NULL Default getdate()
Go

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreatedBy_UserId' and sysobjects.name = N'tblInvoiceArticle_tblProductArea')
	begin
		ALTER TABLE tblInvoiceArticle_tblProductArea ADD CreatedBy_UserId int NULL 

		ALTER TABLE [dbo].tblInvoiceArticle_tblProductArea ADD 
			CONSTRAINT [FK_tblInvoiceArticle_tblProductArea_tblUsers] FOREIGN KEY 
			(
				[CreatedBy_UserId]
			) REFERENCES [dbo].tblUsers (
				[Id]
			)	
	end
GO

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CalcSolvedInTimeByFinishingDate' and sysobjects.name = N'tblSettings')
begin
	DECLARE @sql NVARCHAR(MAX)
	WHILE 1=1
	BEGIN
		SELECT TOP 1 @sql = N'alter table tblSettings drop constraint ['+dc.NAME+N']'
		from sys.default_constraints dc
		JOIN sys.columns c
			ON c.default_object_id = dc.object_id
		WHERE 
			dc.parent_object_id = OBJECT_ID('tblSettings')
		AND c.name = N'CalcSolvedInTimeByFinishingDate'
		IF @@ROWCOUNT = 0 BREAK
		EXEC (@sql)
	END
	
	ALTER TABLE tblSettings drop column CalcSolvedInTimeByFinishingDate
end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CalcSolvedInTimeByLatestSLADate' and sysobjects.name = N'tblSettings')
	ALTER TABLE tblSettings ADD CalcSolvedInTimeByLatestSLADate int NOT NULL Default(0)
GO



if COL_LENGTH('tblCaseFilterFavorite','RegionFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column RegionFilter nvarchar(200)  null 
END
Go


if COL_LENGTH('tblCaseFilterFavorite','DepartmentFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column DepartmentFilter nvarchar(200)  null 
END
Go


if COL_LENGTH('tblCaseFilterFavorite','RegisteredByFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column RegisteredByFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','CaseTypeFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column CaseTypeFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','ProductAreaFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column ProductAreaFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','WorkingGroupFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column WorkingGroupFilter nvarchar(200)  null 
END


if COL_LENGTH('tblCaseFilterFavorite','ResponsibleFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column ResponsibleFilter nvarchar(200)  null 
END

if COL_LENGTH('tblCaseFilterFavorite','AdministratorFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column AdministratorFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','PriorityFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column PriorityFilter nvarchar(200)  null 
END

if COL_LENGTH('tblCaseFilterFavorite','StatusFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column StatusFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','SubStatusFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column SubStatusFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','RemainingTimeFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column RemainingTimeFilter nvarchar(200)  null 
END
Go

if COL_LENGTH('tblCaseFilterFavorite','ClosingReasonFilter') != 400
BEGIN	
	alter table tblCaseFilterFavorite alter column ClosingReasonFilter nvarchar(200)  null 
END
Go


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.26'

