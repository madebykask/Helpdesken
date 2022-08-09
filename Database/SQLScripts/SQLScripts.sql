--update DB from 5.3.55 to 5.3.56 version


RAISERROR ('Create table tblMergedCases', 10, 1) WITH NOWAIT
IF(OBJECT_ID('tblMergedCases', 'U') IS NULL)
Begin
CREATE TABLE [dbo].[tblMergedCases](
	[MergedParent_Id] [int] NOT NULL,
	[MergedChild_Id] [int] NOT NULL
 CONSTRAINT [PK_tblMergedCases] PRIMARY KEY CLUSTERED 
(
	[MergedParent_Id] ASC,
	[MergedChild_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

RAISERROR ('Add Column Merged to tblFinishingCause', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblFinishingCause','Merged') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblFinishingCause]
		ADD Merged bit not null default 0
	End
Go

-- To add mandatory 'Merged case' in Finishing causes for all Customers
insert into [dbo].[tblFinishingCause] (
    [Customer_Id]
      ,[FinishingCause]
      ,[FinishingCauseGUID]
      ,[Merged]

)

select distinct Id, 'Sammanfogat ärende' , newid(), 1
from tblCustomer
where id not in 
(
    select distinct customer_id from tblFinishingCause where merged=1

)

Go

RAISERROR ('Add Column MergeCasePermission to tblUsers', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblUsers','MergeCasePermission') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblUsers]
		ADD MergeCasePermission int null default 0
	End
Go

-- Decided that all users should have this set on
UPDATE [dbo].[tblUsers]
SET MergeCasePermission = 1;
GO

--New mailtemplate for Merged Cases
If not exists (select * from tblMailTemplate where MailID = 18 and Customer_Id is null)
	insert into tblMailTemplate (MailID, IsStandard, SendMethod) Values(18, 1, 0)
GO

Declare @mailID int
Set @mailID = (Select Id from tblMailTemplate where MailID = 18 and Customer_Id is null)

If not exists (select * from tblMailTemplate_tblLanguage where MailTemplate_Id = @mailID and Language_Id = 1)
	Insert into tblMailTemplate_tblLanguage (MailTemplate_Id, Language_Id, MailTemplateName, Subject, Body) Values(@mailID, 1, 'Sammanfogat ärende', 'Sammanfogat ärende', 'Sammanfogat ärende')


If not exists (select * from tblMailTemplate_tblLanguage where MailTemplate_Id = @mailID and Language_Id = 2)
	Insert into tblMailTemplate_tblLanguage (MailTemplate_Id, Language_Id, MailTemplateName, Subject, Body) Values(@mailID, 2, 'Merged Case', 'Merged Case', 'Merged Case')
GO

--Insert the new template for all customers..

insert into tblMailTemplate (MailID, Customer_Id, isStandard, SendMethod)
select 18, Id, 1, 0 from tblCustomer where Id not in(select Customer_Id from tblMailTemplate where MailID = 18 and Customer_Id is not null)

GO

--Insert mailtemplates for all customers

insert into tblMailTemplate_tblLanguage ([MailTemplate_Id]
      ,[Language_Id]
      ,[MailTemplateName]
      ,[Subject]
      ,[Body])



SELECT        dbo.tblMailTemplate.Id, dbo.tblMailTemplate_tblLanguage.Language_Id, dbo.tblMailTemplate_tblLanguage.MailTemplateName, dbo.tblMailTemplate_tblLanguage.Subject, dbo.tblMailTemplate_tblLanguage.Body
FROM            dbo.tblMailTemplate INNER JOIN
                         dbo.tblMailTemplate AS tblMailTemplate_1 ON dbo.tblMailTemplate.MailID = tblMailTemplate_1.MailID INNER JOIN
                         dbo.tblMailTemplate_tblLanguage ON tblMailTemplate_1.Id = dbo.tblMailTemplate_tblLanguage.MailTemplate_Id
WHERE        (dbo.tblMailTemplate.MailID = 18) AND (dbo.tblMailTemplate.Customer_Id IS NOT NULL) AND (tblMailTemplate_1.Customer_Id IS NULL)

AND  dbo.tblMailTemplate.Id not in (select MailTemplate_Id from tblmailtemplate_tbllanguage)

GO

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.56'
GO