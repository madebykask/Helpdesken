-- update DB from 5.3.7.XX to 5.3.8.xx version

-- 2015-05-18
update tblUsers set timezoneid = 'W. Europe Standard Time' where timezoneid is null;
go

if not exists (select * from tblTextType where id = 300)
begin
  insert into tblTextType values(300, 'Line Manager', 1)
end
GO

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.8'
