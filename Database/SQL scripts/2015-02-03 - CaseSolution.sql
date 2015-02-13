IF COL_LENGTH('dbo.tblCaseSolution','PersonsName') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD PersonsName NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','PersonsPhone') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD PersonsPhone NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','PersonsCellPhone') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD PersonsCellPhone NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Region_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Region_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblRegion] FOREIGN KEY 
			(
				[Region_Id]
			) REFERENCES [dbo].[tblRegion] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','OU_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD OU_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblOU] FOREIGN KEY 
			(
				[OU_Id]
			) REFERENCES [dbo].[tblOU] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Place') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Place NVARCHAR(100) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','UserCode') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD UserCode NVARCHAR(20) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','System_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD System_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblSystem] FOREIGN KEY 
			(
				[System_Id]
			) REFERENCES [dbo].[tblSystem] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Urgency_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Urgency_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblUrgency] FOREIGN KEY 
			(
				Urgency_Id
			) REFERENCES [dbo].[tblUrgency] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Impact_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Impact_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblImpact] FOREIGN KEY 
			(
				Impact_Id
			) REFERENCES [dbo].[tblImpact] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InvoiceNumber') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InvoiceNumber NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','ReferenceNumber') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD ReferenceNumber NVARCHAR(200) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Status_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Status_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblStatus] FOREIGN KEY 
			(
				Status_Id
			) REFERENCES [dbo].[tblStatus] (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','StateSecondary_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD StateSecondary_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblStateSecondary] FOREIGN KEY 
			(
				StateSecondary_Id
			) REFERENCES [dbo].tblStateSecondary (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Verified') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD Verified int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','VerifiedDescription') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD VerifiedDescription NVARCHAR(200) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','SolutionRate') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD SolutionRate NVARCHAR(10) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InventoryNumber') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InventoryNumber NVARCHAR(20) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InventoryType') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InventoryType NVARCHAR(50) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','InventoryLocation') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD InventoryLocation NVARCHAR(100) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Supplier_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Supplier_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblSupplier] FOREIGN KEY 
			(
				Supplier_Id
			) REFERENCES [dbo].tblSupplier (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','SMS') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD SMS int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Available') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Available NVARCHAR(100) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Cost') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD Cost int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','OtherCost') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD OtherCost int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Currency') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Currency NVARCHAR(10) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','ContactBeforeAction') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD ContactBeforeAction int not null default 0 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Problem_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Problem_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblProblem] FOREIGN KEY 
			(
				Problem_Id
			) REFERENCES [dbo].tblProblem (
				[Id]
			)	
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','Change_Id') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD Change_Id int NULL 

	ALTER TABLE [dbo].tblCaseSolution ADD 
			CONSTRAINT [FK_tblCaseSolution_tblChange] FOREIGN KEY 
			(
				Change_Id
			) REFERENCES [dbo].tblChange (
				[Id]
			)	
END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'WatchDate' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD WatchDate datetime NULL			
	end
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'FinishingDate' and sysobjects.name = N'tblCaseSolution')
	begin
		ALTER TABLE tblCaseSolution ADD FinishingDate datetime NULL			
	end
GO

IF COL_LENGTH('dbo.tblCaseSolution','FinishingDescription') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution	ADD FinishingDescription NVARCHAR(200) NULL 
END
GO

IF COL_LENGTH('dbo.tblCaseSolution','UpdateNotifierInformation') IS NULL
BEGIN
	ALTER TABLE [dbo].tblCaseSolution ADD UpdateNotifierInformation int null 
END
GO
