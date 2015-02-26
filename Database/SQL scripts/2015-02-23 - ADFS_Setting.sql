if not exists(select * from sysobjects WHERE Name = N'tblADFSSetting')
begin
	CREATE TABLE [dbo].[tblADFSSetting](
	[ApplicationId] [nvarchar](50) NULL,
	[AttrDomain] [nvarchar](50) NULL,
	[AttrUserId] [nvarchar](50) NULL,
	[AttrEmployeeNumber] [nvarchar](50) NULL,
	[AttrFirstName] [nvarchar](50) NULL,
	[AttrSurName] [nvarchar](50) NULL,
	[AttrEmail] [nvarchar](50) NULL,
	[SaveSSOLog] [bit] NOT NULL
	) ON [PRIMARY]	

	ALTER TABLE [dbo].[tblADFSSetting] ADD  CONSTRAINT [DF_tblADFSSetting_SaveSSOLog]  DEFAULT ((1)) FOR [SaveSSOLog]
	
end