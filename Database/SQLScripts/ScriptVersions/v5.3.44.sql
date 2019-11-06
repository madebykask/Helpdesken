--update DB from 5.3.43 to 5.3.44 version

RAISERROR ('Add column Operation to tblFileViewLog table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Operation' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ADD Operation int null    
END
GO

RAISERROR ('Adding toggle for usage of file access logging. DISABLE_LOG_VIEW_CASE_FILE', 10, 1) WITH NOWAIT
IF NOT EXISTS(SELECT 1 FROM tblFeatureToggle FT WHERE FT.StrongName = 'DISABLE_LOG_VIEW_CASE_FILE')
BEGIN
	INSERT INTO tblFeatureToggle(Active, ChangeDate, [Description], StrongName)
	SELECT 1, GETDATE(), 'Toogle for activating case file access logging', 'DISABLE_LOG_VIEW_CASE_FILE'
END
GO

RAISERROR ('Add column UserName to tblFileViewLog table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'UserName' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ADD UserName nvarchar(200) null    
END
GO


RAISERROR ('Change column User_Id to tblFileViewLog table', 10, 1) WITH NOWAIT
if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'User_Id' and sysobjects.name = N'tblFileViewLog')
BEGIN
    ALTER TABLE tblFileViewLog
    ALTER COLUMN [User_Id] int null    
END
GO

RAISERROR ('Change size of tblEmailLog.MessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblEmailLog
ALTER COLUMN MessageId NVARCHAR(300) NULL

GO

RAISERROR ('Change size of tblMail2Ticket.UniqueMessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblMail2Ticket
ALTER COLUMN UniqueMessageId NVARCHAR(300) NULL

GO

RAISERROR ('Change size of tblAccountEMailLog.MessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblAccountEMailLog
ALTER COLUMN MessageId NVARCHAR(300) NULL

GO

RAISERROR ('Change size of tblChangeEMailLog.MessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblChangeEMailLog
ALTER COLUMN MessageId NVARCHAR(300) NULL

GO

RAISERROR ('Change size of tblOrderEMailLog.MessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblOrderEMailLog
ALTER COLUMN MessageId NVARCHAR(300) NULL

GO

RAISERROR ('Change size of tblProblemEMailLog.MessageId to 300', 10, 1) WITH NOWAIT
ALTER TABLE tblProblemEMailLog
ALTER COLUMN MessageId NVARCHAR(300) NULL

GO

RAISERROR ('Change tblProblemLog.CreatedDate to default UTC date', 10, 1) WITH NOWAIT
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblProblemLog_CreatedDate]') AND type = 'D')
BEGIN 
	ALTER TABLE [dbo].[tblProblemLog] DROP  CONSTRAINT [DF_tblProblemLog_CreatedDate] 
END	
ALTER TABLE [dbo].[tblProblemLog] ADD  CONSTRAINT [DF_tblProblemLog_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]

GO

RAISERROR ('Change tblProblemLog.ChangeDate to default UTC date', 10, 1) WITH NOWAIT
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblProblemLog_ChangedDate]') AND type = 'D')
BEGIN 
	ALTER TABLE [dbo].[tblProblemLog] DROP  CONSTRAINT [DF_tblProblemLog_ChangedDate]  
END
ALTER TABLE [dbo].[tblProblemLog] ADD  CONSTRAINT [DF_tblProblemLog_ChangedDate]  DEFAULT (getutcdate()) FOR [ChangedDate]
GO

RAISERROR ('Add tblComputerStatus table', 10, 1) WITH NOWAIT
IF NOT EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where sysobjects.name = N'tblComputerStatus')
BEGIN 
	CREATE TABLE [dbo].[tblComputerStatus](
		[Id] [int] NOT NULL,
		[ComputerStatus] [nvarchar](50) NOT NULL,
		[Type] [int] NOT NULL,
		[Customer_Id] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ChangedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_tblComputerStatus] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	

	ALTER TABLE [dbo].[tblComputerStatus] ADD  CONSTRAINT [DF_tblComputerStatus_Type]  DEFAULT ((1)) FOR [Type]
	ALTER TABLE [dbo].[tblComputerStatus] ADD  CONSTRAINT [DF_tblComputerStatus_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
	ALTER TABLE [dbo].[tblComputerStatus] ADD  CONSTRAINT [DF_tblComputerStatus_ChangedDate]  DEFAULT (getutcdate()) FOR [ChangedDate]
	ALTER TABLE [dbo].[tblComputerStatus]  WITH CHECK ADD  CONSTRAINT [FK_tblComputerStatus_tblCustomer] FOREIGN KEY([Customer_Id])
	REFERENCES [dbo].[tblCustomer] ([Id])
	ALTER TABLE [dbo].[tblComputerStatus] CHECK CONSTRAINT [FK_tblComputerStatus_tblCustomer]	
END
GO

RAISERROR ('Add Foreign key for column ComputerContractStatus_Id in tblComputer table', 10, 1) WITH NOWAIT
IF OBJECT_ID('dbo.[FK_tblComputer_tblComputerStatus_Contract]', 'F') IS NULL
BEGIN
	ALTER TABLE [dbo].[tblComputer]  WITH NOCHECK ADD  CONSTRAINT [FK_tblComputer_tblComputerStatus_Contract] FOREIGN KEY([ComputerContractStatus_Id])
	REFERENCES [dbo].[tblComputerStatus] ([Id])
	ALTER TABLE [dbo].[tblComputer] NOCHECK CONSTRAINT [FK_tblComputer_tblComputerStatus_Contract]
END
GO

RAISERROR ('Add missing data for tblComputerStatus table', 10, 1) WITH NOWAIT
IF EXISTS (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where sysobjects.name = N'tblComputerStatus')
BEGIN
	DECLARE @MyCursor CURSOR;
	DECLARE @CustomerId int;
	DECLARE @Id int;

    SET @MyCursor = CURSOR FOR
	SELECT DISTINCT c.Id FROM tblCustomer AS c
		LEFT JOIN tblComputerStatus AS cs ON c.Id = cs.Customer_Id
		WHERE cs.Customer_Id IS NULL
		ORDER BY c.Id

	OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @CustomerId

    WHILE @@FETCH_STATUS = 0
    BEGIN
		
	  RAISERROR ('Adding data to tblCustomerStatus for customer %d', 10, 1, @CustomerId) WITH NOWAIT
   	  SELECT @Id = MAX([Id]) FROM tblComputerStatus
	  IF(@Id IS NULL)
		  INSERT [dbo].[tblComputerStatus] ([Id], [ComputerStatus], [Type], [Customer_Id]) VALUES (1, N'Aktiv', 1, @CustomerId),
			(2, N'Ej kopplad till användare', 1, @CustomerId),
			(3, N'Stulen', 1, @CustomerId),
			(11, N'Leasing', 2, @CustomerId),
			(12, N'Köpt', 2, @CustomerId)
	  ELSE
	  BEGIN
		  INSERT [dbo].[tblComputerStatus] ([Id], [ComputerStatus], [Type], [Customer_Id]) VALUES (@Id+1, N'Aktiv', 1, @CustomerId),
			(@Id+2, N'Ej kopplad till användare', 1, @CustomerId),
			(@Id+3, N'Stulen', 1, @CustomerId),
			(@Id+4, N'Leasing', 2, @CustomerId),
			(@Id+5, N'Köpt', 2, @CustomerId),
			(@Id+6, N'Hyrd', 2, @CustomerId)
		  UPDATE [dbo].[tblComputer] SET [Status]=@id+1 WHERE [Customer_Id] = @CustomerId AND [Status] = 1
		  UPDATE [dbo].[tblComputer] SET [Status]=@id+2 WHERE [Customer_Id] = @CustomerId AND [Status] = 2
		  UPDATE [dbo].[tblComputer] SET [Status]=@id+3 WHERE [Customer_Id] = @CustomerId AND [Status] = 3
		  UPDATE [dbo].[tblComputer] SET [ComputerContractStatus_Id]=@id+4 WHERE [Customer_Id] = @CustomerId AND [ComputerContractStatus_Id] = 11
		  UPDATE [dbo].[tblComputer] SET [ComputerContractStatus_Id]=@id+5 WHERE [Customer_Id] = @CustomerId AND [ComputerContractStatus_Id] = 12
	  END

      FETCH NEXT FROM @MyCursor 
      INTO @CustomerId 
    END; 

    CLOSE @MyCursor;
    DEALLOCATE @MyCursor;
END


RAISERROR ('Add Region_Id to tblComputer table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'Region_Id' and sysobjects.name = N'tblComputer')
BEGIN
    ALTER TABLE tblComputer
    ADD Region_Id int null 
	ALTER TABLE [dbo].[tblComputer]  WITH NOCHECK ADD  CONSTRAINT [FK_tblComputer_tblRegion] FOREIGN KEY([Region_Id])
	REFERENCES [dbo].[tblComputer] ([Id])
	ALTER TABLE [dbo].[tblComputer] NOCHECK CONSTRAINT [FK_tblComputer_tblRegion]
END


RAISERROR ('Add ParentLogType to tblLogFile table', 10, 1) WITH NOWAIT
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'ParentLogType' and sysobjects.name = N'tblLogFile')
BEGIN
    ALTER TABLE tblLogFile
    ADD ParentLogType int null 

	EXEC('UPDATE lfc
		SET lfc.[ParentLogType] = lfp.LogType
	FROM [dbo].[tblLogFile] as lfc
	INNER JOIN [dbo].[tblLogFile] as lfp ON lfc.ParentLog_Id = lfp.Log_Id
	WHERE lfc.ParentLog_Id IS NOT NULL and lfc.[FileName] = lfp.[FileName]')
END
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.44'
GO

