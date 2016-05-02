-- update DB from 5.3.22 to 5.3.23 version

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 54, 3, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 54)


IF COL_LENGTH('tblCaseInvoiceArticle','CreditedForArticle_Id') IS NULL
begin
    alter table tblCaseInvoiceArticle 
	add [CreditedForArticle_Id] int null
end
GO

IF COL_LENGTH('tblCaseSolution','PersonsEmail') IS NULL
begin
    alter table tblCaseSolution 
	add [PersonsEmail] nvarchar(100) null
end
GO

Insert into tblCaseSolutionFieldSettings
Select cs.Id, 55, 1, GETDATE(), GETDATE() from tblCaseSolution cs
Where cs.Id not in (Select CaseSolution_Id from tblCaseSolutionFieldSettings csf 
					where cs.Id = csf.CaseSolution_Id and FieldName_Id = 55)


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.23'
