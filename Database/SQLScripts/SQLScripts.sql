-- update DB from 5.3.7.XX to 5.3.8.xx version

-- 2015-05-18
update tblUsers set timezoneid = 'W. Europe Standard Time' where timezoneid is null;
go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.8'
