--update DB from 5.3.55 to 5.3.56 version


RAISERROR ('Add Column RelationType to tblParentChildCaseRelations', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblParentChildCaseRelations','RelationType') IS NULL
	BEGIN	 
		ALTER TABLE [Helpdesk-Dev].[dbo].[tblParentChildCaseRelations]
		ADD RelationType bit not null default 0
	End
Go

RAISERROR ('Add Column Merged to tblFinishingCause', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblFinishingCause','Merged') IS NULL
	BEGIN	 
		ALTER TABLE [Helpdesk-Dev].[dbo].[tblFinishingCause]
		ADD Merged bit not null default 0
	End
Go

-- To add mandatory 'Merged case' in Finishing causes for all Customers
insert into [Helpdesk-Dev].[dbo].[tblFinishingCause] (
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
  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.56'
GO