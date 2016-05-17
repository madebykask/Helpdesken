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

IF COL_LENGTH('tblCaseInvoiceOrder','OrderState') IS NULL
begin
    alter table tblCaseInvoiceOrder
	add [OrderState] int not null Default(1)		
end
Go

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

					 
-- OrderState = 1:Saved Order - 2:Sent Order - 9: Deleted Order)
-- Set Invoiced orders state to 2
Update tblCaseInvoiceOrder set OrderState = 2
Where InvoicedByUserId is not null
Go

if not exists(select * from sysobjects WHERE Name = N'tblReportFavorites')
begin
	CREATE TABLE [dbo].[tblReportFavorites] (
		[Id]				INT				IDENTITY (1, 1) NOT NULL,
		[Customer_Id]		INT				NOT NULL,
		[Type]				INT				NOT NULL,
		[Name]				NVARCHAR (300)	NOT NULL,
		[Filters]			NVARCHAR (MAX),
		[UpdateDate]		DATETIME		NOT NULL
		CONSTRAINT [PK_tblReportFavoritesId] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_tblReportFavorites_tblCustomers] FOREIGN KEY ([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id]),
	);
end
Go

IF COL_LENGTH('tblCaseInvoiceArticle','Amount') IS NOT NULL
begin
    alter table tblCaseInvoiceArticle 
	alter column Amount decimal(18, 3) null
end
GO

IF COL_LENGTH('tblCaseInvoiceArticle','Ppu') IS NOT NULL
begin
    alter table tblCaseInvoiceArticle 
	alter column Ppu decimal(18, 3) null
end
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.23'
