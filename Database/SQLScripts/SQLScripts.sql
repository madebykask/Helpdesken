-- update DB from 5.3.11 to 5.3.12 version

-- Nytt fält i tblCaseSolution
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'RegistrationSource' and sysobjects.name = N'tblCaseSolution')
	ALTER TABLE tblCaseSolution ADD RegistrationSource int Default(0) NOT NULL
GO


if not exists(select * from sysobjects WHERE Name = N'tblCaseStatistics') 
BEGIN  
	CREATE TABLE [dbo].[tblCaseStatistics](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Case_Id] [int] NOT NULL,
		[WasSolvedInTime] [int] NULL,     
	 CONSTRAINT [PK_tblCaseStatistics] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];
	ALTER TABLE [dbo].[tblCaseStatistics]  WITH CHECK ADD  CONSTRAINT [FK_tblCaseStatistics_tblCase] FOREIGN KEY([CaseId])
	REFERENCES [dbo].[tblCase] ([Id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[tblCaseStatistics] CHECK CONSTRAINT [FK_tblCaseStatistics_tblCase];
END

-- http://helpdesk5.dhsolutions.se/Cases/Edit/53106
-- 200 bytes for 100 nvarchar (https://msdn.microsoft.com/en-us/library/ms188732.aspx)
if COL_LENGTH('tblCase','Caption') != 200
BEGIN	
	alter table tblCase alter column Caption nvarchar(100);
END
if COL_LENGTH('tblCaseHistory','Caption') != 200
BEGIN
	alter table tblCase alter column Caption nvarchar(100);
END


-- Script for update old templates with new fields at case
insert into tblcasesolutionfieldsettings  
select casesolution_id, 51, 1, getdate(), getdate() 
from tblcasesolutionfieldsettings 
where casesolution_id 
	not in (select casesolution_id from tblcasesolutionfieldsettings
			where fieldname_id = 51)
group by casesolution_id
Go



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.12'
