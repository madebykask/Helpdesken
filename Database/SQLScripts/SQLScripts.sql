-- update DB from 5.3.9.XX to 5.3.10.xx version

IF COL_LENGTH('dbo.tblSettings','ComputerUserSearchRestriction') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblSettings]
	ADD ComputerUserSearchRestriction int default 0 not null
END
GO

-- 20150617
if not exists(select * from sysobjects WHERE Name = N'tblCustomerAvailableUser')
BEGIN
	CREATE TABLE [dbo].[tblCustomerAvailableUser](
		[Customer_Id] [int] NOT NULL,
		[User_Id] [int] NOT NULL	
	 CONSTRAINT [PK_tblCustomerAvailableUser] PRIMARY KEY CLUSTERED 
	(
		[Customer_Id] ASC,
		[User_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
        
	ALTER TABLE [dbo].[tblCustomerAvailableUser]  WITH CHECK ADD  CONSTRAINT [FK_tblCustomerAvailableUser_tblCustomer] FOREIGN KEY([Customer_Id])
	REFERENCES [dbo].[tblCustomer] ([Id])
		ON UPDATE CASCADE
		ON DELETE CASCADE
	ALTER TABLE [dbo].[tblCustomerAvailableUser]  WITH CHECK ADD  CONSTRAINT [FK_tblCustomerAvailableUser_tblUsers] FOREIGN KEY([User_Id])
	REFERENCES [dbo].[tblUsers] ([Id])
		ON UPDATE CASCADE
		ON DELETE CASCADE
	
	insert into tblCustomerAvailableUser(customer_id, user_id) 
	select customer_id, user_id 
	from tblCustomerUser
END

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.10'
