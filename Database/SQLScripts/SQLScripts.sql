--update DB from 5.3.39 to 5.3.40 version

RAISERROR ('Creating idx_casestatistics_case index in tblCaseStatistics', 10, 1) WITH NOWAIT
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'idx_casestatistics_case' AND object_id = OBJECT_ID('tblCaseStatistics'))
BEGIN
	CREATE NONCLUSTERED INDEX idx_casestatistics_case
	ON [dbo].[tblCaseStatistics] ([Case_Id])
	INCLUDE ([Id],[WasSolvedInTime])
END
GO

RAISERROR ('Adding ShowCaseActionsPanelOnTop column in tblCustomer', 10, 1) WITH NOWAIT
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
			where syscolumns.name = N'ShowCaseActionsPanelOnTop' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD ShowCaseActionsPanelOnTop bit not null DEFAULT(1)
END
GO

RAISERROR ('Adding ShowCaseActionsPanelAtBottom column in tblCustomer', 10, 1) WITH NOWAIT
GO
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
			where syscolumns.name = N'ShowCaseActionsPanelAtBottom' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD ShowCaseActionsPanelAtBottom bit not null DEFAULT(0)
END
GO 

RAISERROR ('Create table tblWorkstationTabSettings', 10, 1) WITH NOWAIT
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblWorkstationTabSettings' AND type='U')
BEGIN
	CREATE TABLE [dbo].[tblWorkstationTabSettings] (
		[Id]                    INT              IDENTITY (1, 1) NOT NULL,
		[Customer_Id]           INT              NULL,
		[TabField]              NVARCHAR (50)     CONSTRAINT [DF_tblWorkstationTabSettings_TabField] DEFAULT ('') NOT NULL,
		[Show]                  BIT              CONSTRAINT [DF_tblWorkstationTabSettings_Show] DEFAULT ((1)) NOT NULL,
		[CreatedDate]           DATETIME         CONSTRAINT [DF_tblWorkstationTabSettings_CreatedDate] DEFAULT (getdate()) NOT NULL,
		[ChangedDate]           DATETIME         CONSTRAINT [DF_tblWorkstationTabSettings_ChangedDate] DEFAULT (getdate()) NOT NULL,
		CONSTRAINT [PK_tblWorkstationTabSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_tblWorkstationTabSettings_tblCustomer] FOREIGN KEY ([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
	);

	ALTER TABLE [dbo].[tblWorkstationTabSettings] NOCHECK CONSTRAINT [FK_tblWorkstationTabSettings_tblCustomer];
END
GO

RAISERROR ('Create table tblWorkstationTabSetting_tblLang', 10, 1) WITH NOWAIT
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblWorkstationTabSetting_tblLang' AND type='U')
BEGIN
	CREATE TABLE [dbo].[tblWorkstationTabSetting_tblLang] (
		[WorkstationTabSetting_Id] INT            NOT NULL,
		[Language_Id]          INT            NOT NULL,
		[Label]                NVARCHAR (50)  NULL,
		[FieldHelp]            NVARCHAR (200) NULL,
		CONSTRAINT [PK_tblWorkstationTabSetting_tblLang] PRIMARY KEY CLUSTERED ([WorkstationTabSetting_Id] ASC, [Language_Id] ASC),
		CONSTRAINT [FK_tblWorkstationTabSetting_tblLang_tblWorkstationTabSettings] FOREIGN KEY ([WorkstationTabSetting_Id]) REFERENCES [dbo].[tblWorkstationTabSettings] ([Id]),
		CONSTRAINT [FK_tblWorkstationTabSetting_tblLang_tblLanguage] FOREIGN KEY ([Language_Id]) REFERENCES [dbo].[tblLanguage] ([Id])
	);

	ALTER TABLE [dbo].[tblWorkstationTabSetting_tblLang] NOCHECK CONSTRAINT [FK_tblWorkstationTabSetting_tblLang_tblWorkstationTabSettings];
	ALTER TABLE [dbo].[tblWorkstationTabSetting_tblLang] NOCHECK CONSTRAINT [FK_tblWorkstationTabSetting_tblLang_tblLanguage];

END
GO

RAISERROR ('Creating table tblInventoryTypeStandardSettings', 10, 1) WITH NOWAIT
GO
IF NOT EXISTS(select * from sysobjects WHERE Name = N'tblInventoryTypeStandardSettings')
BEGIN 

    CREATE TABLE [dbo].[tblInventoryTypeStandardSettings](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [Customer_Id] [int] NOT NULL,
	    [ShowPrinters] [bit] NOT NULL,
	    [ShowWorkstations] [bit] NOT NULL,
	    [ShowServers] [bit] NOT NULL,
	CONSTRAINT [PK_tblInventoryTypeStandardSettings] PRIMARY KEY CLUSTERED 
    (
	    [Id] ASC
    ) ON [PRIMARY]
    ) ON [PRIMARY]        

    ALTER TABLE [dbo].[tblInventoryTypeStandardSettings]  WITH NOCHECK ADD CONSTRAINT [FK_tblInventoryTypeStandardSettings_tblCustomer] FOREIGN KEY([Customer_Id])
    REFERENCES [dbo].[tblCustomer] ([Id])
    
    ALTER TABLE [dbo].[tblInventoryTypeStandardSettings] NOCHECK CONSTRAINT [FK_tblInventoryTypeStandardSettings_tblCustomer]    
END
GO

RAISERROR ('Populating tblInventoryTypeStandardSettings with default values', 10, 1) WITH NOWAIT
GO
DECLARE @count int
DECLARE @curRow int
DECLARE @Customers as Table(RowNumber int, CustomerId int)

INSERT INTO @Customers (RowNumber, CustomerId)
select (ROW_NUMBER() OVER (Order by cus.Id)) as RowNumber, cus.Id from tblCustomer cus 
    LEFT JOIN tblInventoryTypeStandardSettings ts ON cus.Id = ts.Customer_Id
WHERE ts.Id IS NULL
ORDER BY cus.Id

select @count = COUNT(*) from @Customers
SET @curRow = 1

while(@curRow <= @count)
BEGIN
    DECLARE @customerId int = 0	     
    select @customerId = CustomerId from @Customers where RowNumber = @curRow
    IF NOT EXISTS (select 1 from tblInventoryTypeStandardSettings where Customer_Id = @customerId)
    BEGIN	   
	   INSERT INTO [dbo].[tblInventoryTypeStandardSettings] (Customer_Id, ShowPrinters, ShowWorkstations, ShowServers)
	   VALUES (@customerId, 1, 1, 1)
    END
    SET @curRow += 1
End;
GO

--select * from [dbo].[tblInventoryTypeStandardSettings] 

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.40'
--ROLLBACK --TMP






