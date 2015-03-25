ALTER TABLE tblCaseSettings ALTER COLUMN CustomerId int NULL
GO

-- CaseSettings for UserGroup = 4

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Casenumber' and UserGroup=4)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Casenumber', 1, 40, 4, 1, 0, getDate(), getDate())


if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'CaseType_Id' and UserGroup=4)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'CaseType_Id', 1, 50, 4, 2, 0, getDate(), getDate())


if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Caption' and UserGroup=4)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Caption', 1, 200, 4, 3, 0, getDate(), getDate())


if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Performer_User_Id' and UserGroup=4)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Performer_User_Id', 1, 100, 4, 4, 0, getDate(), getDate())


if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Priority_Id' and UserGroup=4)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Priority_Id', 1, 100, 4, 5, 0, getDate(), getDate())


-- CaseSettings for UserGroup = 1
if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Casenumber' and UserGroup=1)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Casenumber', 1, 40, 1, 1, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'CaseType_Id' and UserGroup=1)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'CaseType_Id', 1, 50, 1, 2, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Caption' and UserGroup=1)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Caption', 1, 200, 1, 3, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Performer_User_Id' and UserGroup=1)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Performer_User_Id', 1, 100, 1, 4, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Priority_Id' and UserGroup=1)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Priority_Id', 1, 100, 1, 5, 0, getDate(), getDate())


-- CaseSettings for UserGroup = 2

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Casenumber' and UserGroup=2)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Casenumber', 1, 40, 2, 1, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'CaseType_Id' and UserGroup=2)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'CaseType_Id', 1, 50, 2, 2, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Caption' and UserGroup=2)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Caption', 1, 200, 2, 3, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Performer_User_Id' and UserGroup=2)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Performer_User_Id', 1, 100, 2, 4, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Priority_Id' and UserGroup=2)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Priority_Id', 1, 100, 2, 5, 0, getDate(), getDate())


-- CaseSettings for UserGroup = 3

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Casenumber' and UserGroup=3)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Casenumber', 1, 40, 3, 1, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'CaseType_Id' and UserGroup=3)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'CaseType_Id', 1, 50, 3, 2, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Caption' and UserGroup=3)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Caption', 1, 200, 3, 3, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Performer_User_Id' and UserGroup=3)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Performer_User_Id', 2, 100, 3, 4, 0, getDate(), getDate())

if not exists (select * from tblCaseSettings where customerid is null and tblCaseName = 'Priority_Id' and UserGroup=3)
	insert into tblCaseSettings (CustomerId, [User_Id], tblCaseName, Line, MinWidth, UserGroup, ColOrder, ShowInMobileList, RegTime,
		ChangeTime) VALUES (null, null, 'Priority_Id', 1, 100, 3, 5, 0, getDate(), getDate())