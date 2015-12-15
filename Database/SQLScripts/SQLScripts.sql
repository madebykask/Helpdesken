-- update DB from 5.3.18 to 5.3.19 version
-- Nytt fält i tblAccountActivity
if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'CreateCase_StateSecondary_Id' and sysobjects.name = N'tblAccountActivity')
begin
ALTER TABLE tblAccountActivity ADD CreateCase_StateSecondary_Id int NULL 

ALTER TABLE [dbo].[tblAccountActivity] ADD 
    CONSTRAINT [FK_tblAccountActivity_tblStateSecondary] FOREIGN KEY 
    (
                                [CreateCase_StateSecondary_Id]
    ) REFERENCES [dbo].[tblStateSecondary] (
                                [Id]
    )                           
end
GO



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.19'

