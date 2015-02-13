-- fix for missing column for DB backup that stored in GIT
IF COL_LENGTH('dbo.tblWatchDateCalendarValue','WatchDateValueName') IS NULL
BEGIN
	alter table tblWatchDateCalendarValue add WatchDateValueName [nvarchar](50) NULL;
end

-- fix for wrong data
update tblCustomer set CommunicateWithNotifier = 1 where CommunicateWithNotifier is null;