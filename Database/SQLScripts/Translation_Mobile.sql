DECLARE @MobileType INT
SET @MobileType = 500

  -- Add new text type:
IF NOT EXISTS(SELECT 1 FROM tblTextType WHERE TextType = 'Mobile')
BEGIN 
    -- 
    INSERT INTO tblTextType (Id, TextType, [Status])
    VALUES(500, 'Mobile', 1)
END

-- Home
If not exists (select * from tbltext where id = 10000)
insert into tbltext (id, TextString, TextType) VALUES (10000, 'Hem', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10000 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10000, 2, 'Home')

-- Add
If not exists (select * from tbltext where id = 10001)
insert into tbltext (id, TextString, TextType) VALUES (10001, 'Lägg till', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10001 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10001, 2, 'Add')

-- Overview
If not exists (select * from tbltext where id = 10002)
insert into tbltext (id, TextString, TextType) VALUES (10002, 'Översikt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10002 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10002, 2, 'Overview')


-- Case Overview
If not exists (select * from tbltext where id = 10003)
insert into tbltext (id, TextString, TextType) VALUES (10003, 'Ärendeöversikt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10003 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10003, 2, 'Case Overview')

-- Lanuage
If not exists (select * from tbltext where id = 10004)
insert into tbltext (id, TextString, TextType) VALUES (10004, 'Språk', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10004 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10004, 2, 'Language')

--Log out
If not exists (select * from tbltext where id = 10005)
insert into tbltext (id, TextString, TextType) VALUES (10005, 'Logga ut', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10005 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10005, 2, 'Log out')

-- Search
If not exists (select * from tbltext where id = 10006)
insert into tbltext (id, TextString, TextType) VALUES (10006, 'Sök', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10006 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10006, 2, 'Search')

-- Cancel 
If not exists (select * from tbltext where id = 10007)
insert into tbltext (id, TextString, TextType) VALUES (10007, 'Avbryt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10007 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10007, 2, 'Cancel')

-- Done
If not exists (select * from tbltext where id = 10008)
insert into tbltext (id, TextString, TextType) VALUES (10008, 'Klar', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10008 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10008, 2, 'Done')

-- Select
If not exists (select * from tbltext where id = 10009)
insert into tbltext (id, TextString, TextType) VALUES (10009, 'Välj', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10009 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10009, 2, 'Select')

-- All Cases
If not exists (select * from tbltext where id = 10012 )
insert into tbltext (id, TextString, TextType) VALUES (10012, 'Alla ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10012 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10012, 2, 'All cases')


-- My Cases
If not exists (select * from tbltext where id = 10013 )
insert into tbltext (id, TextString, TextType) VALUES (10013, 'Mina ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10013 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10013, 2, 'My cases')