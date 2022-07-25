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
  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.56'
GO