--update DB from 5.3.39 to 5.3.40 version


-- Create index on tblCaseStatistics to tblCase
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'idx_casestatistics_case' AND object_id = OBJECT_ID('tblCaseStatistics'))
BEGIN
	CREATE NONCLUSTERED INDEX idx_casestatistics_case
	ON [dbo].[tblCaseStatistics] ([Case_Id])
	INCLUDE ([Id],[WasSolvedInTime])
END

-- add tblCustomer.ShowCaseActionsPanelOnTop
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
			where syscolumns.name = N'ShowCaseActionsPanelOnTop' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD ShowCaseActionsPanelOnTop bit not null DEFAULT(1)
END

--add  tblCustomer.ShowCaseActionsPanelAtBottom
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
			where syscolumns.name = N'ShowCaseActionsPanelAtBottom' and sysobjects.name = N'tblCustomer')
BEGIN
    ALTER TABLE tblCustomer
    ADD ShowCaseActionsPanelAtBottom bit not null DEFAULT(0)
END

RAISERROR ('Create table tblWorkstationTabSettings', 10, 1) WITH NOWAIT
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

RAISERROR ('Create table tblWorkstationTabSetting_tblLang', 10, 1) WITH NOWAIT
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

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.40'
--ROLLBACK --TMP


