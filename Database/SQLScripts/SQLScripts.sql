-- update DB from 5.3.4.XX to 5.3.5.xx version
IF COL_LENGTH('dbo.UserGridSettings','FieldId') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[UserGridSettings]
	ADD [FieldId] int default(NULL)

	/****** Object:  Index [IDX_UserSettings(FieldId)]    Script Date: 31.03.2015 14:15:20 ******/
  CREATE NONCLUSTERED INDEX [IDX_UserSettings(FieldId)] ON [dbo].[UserGridSettings]
  (
    [FieldId] ASC
  )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO 

-- Add new values to tblTextType - Nina
If not exists (select * from tblTextType where Id = 0)
	insert into tblTextType (Id, TextType, Status) VALUES (0, 'Dh Helpdesk', 1)
GO

If not exists (select * from tblTextType where Id = 1)
	insert into tblTextType (Id, TextType, Status) VALUES (1, 'Grunddata', 1)
GO

IF COL_LENGTH('dbo.tblGlobalSettings','HelpdeskDBVersion') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblGlobalSettings]
	ADD [HelpdeskDBVersion] nvarchar(20) NULL 
END
GO 

---------------------------------------------------------------
--  Add filter by "Initiator" in case overview table filter  --
IF COL_LENGTH('dbo.tblCustomerUser','CaseInitiatorFilterShow') IS NULL
BEGIN	 
	ALTER TABLE [dbo].[tblCustomerUser]
	add [CaseInitiatorFilterShow] BIT NOT NULL DEFAULT(0)
END
GO 


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.5'
