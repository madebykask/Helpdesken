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

-- Lock Message1
If not exists (select * from tbltext where id = 10014 )
insert into tbltext (id, TextString, TextType) VALUES (10014, 'OBS! Detta ärende är öppnat av', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10014 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10014, 2, 'Please note! This case is opened by') 

-- Lock Message2
If not exists (select * from tbltext where id = 10015 )
insert into tbltext (id, TextString, TextType) VALUES (10015, 'OBS! Du har redan öppnat detta ärende i en annan session', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10015 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10015, 2, 'Please note! You have already opened this case in another session') 

-- Attach
If not exists (select * from tbltext where id = 10016 )
insert into tbltext (id, TextString, TextType) VALUES (10016, 'Bifoga', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10016 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10016, 2, 'Attach')

-- File delete confirmation
If not exists (select * from tbltext where id = 10017)
insert into tbltext (id, TextString, TextType) VALUES (10017, 'Är du säker på att du vill ta bort bifogad fil', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10017 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10017, 2, 'Are you sure you want to delete attached file')

-- yes
If not exists (select * from tbltext where id = 10018)
insert into tbltext (id, TextString, TextType) VALUES (10018, 'Ja', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10018 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10018, 2, 'Yes')

-- No
If not exists (select * from tbltext where id = 10019)
insert into tbltext (id, TextString, TextType) VALUES (10019, 'Nej', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10019 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10019, 2, 'No')

-- Case/Ärende
If not exists (select * from tbltext where id = 10020)
insert into tbltext (id, TextString, TextType) VALUES (10020, 'Ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10020 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10020, 2, 'Case')

-- Actions/Händelser
If not exists (select * from tbltext where id = 10021)
insert into tbltext (id, TextString, TextType) VALUES (10021, 'Händelser', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10021 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10021, 2, 'Actions')

-- Workflow/Åtgärd
If not exists (select * from tbltext where id = 10022)
insert into tbltext (id, TextString, TextType) VALUES (10022, 'Åtgärd', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10022 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10022, 2, 'Workflow')

-- Files/Filer
If not exists (select * from tbltext where id = 10023)
insert into tbltext (id, TextString, TextType) VALUES (10023, 'Bifogad fil', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10023 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10023, 2, 'Attached file')

-- Other actions/Övriga händelser
If not exists (select * from tbltext where id = 10024)
insert into tbltext (id, TextString, TextType) VALUES (10024, 'Övriga händelser', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10024 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10024, 2, 'Other actions')

If not exists (select * from tbltext where id = 10025)
insert into tbltext (id, TextString, TextType) VALUES (10025, 'Stäng', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10025 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10025, 2, 'Close')

If not exists (select * from tbltext where id = 10026)
insert into tbltext (id, TextString, TextType) VALUES (10026, 'Filtrering startar när minst två tecken har angetts', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10026 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10026, 2, 'Filtering will start after input of two characters')

If not exists (select * from tbltext where id = 10027)
insert into tbltext (id, TextString, TextType) VALUES (10027, 'Tillbaka', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10027 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10027, 2, 'Back')

If not exists (select * from tbltext where id = 10028)
insert into tbltext (id, TextString, TextType) VALUES (10028, 'Skriv för att filtrera', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10028 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10028, 2, 'Type to search')

If not exists (select * from tbltext where id = 10029)
insert into tbltext (id, TextString, TextType) VALUES (10029, 'Inget resultat', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10029 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10029, 2, 'No result')

If not exists (select * from tbltext where id = 10030)
insert into tbltext (id, TextString, TextType) VALUES (10030, 'Fältet är obligatoriskt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10030 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10030, 2, 'Field is required')

If not exists (select * from tbltext where id = 10031)
insert into tbltext (id, TextString, TextType) VALUES (10031, 'Fyll i obligatoriska fält.', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10031 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10031, 2, 'Please fill in required fields.')

If not exists (select * from tbltext where id = 10032)
insert into tbltext (id, TextString, TextType) VALUES (10032, 'Användar ID', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10032 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10032, 2, 'User ID')


If not exists (select * from tbltext where id = 10033)
insert into tbltext (id, TextString, TextType) VALUES (10033, 'Skapa ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10033 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10033, 2, 'Create case')

If not exists (select * from tbltext where id = 10034)
insert into tbltext (id, TextString, TextType) VALUES (10034, 'Välj språk', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10034 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10034, 2, 'Select language')

If not exists (select * from tbltext where id = 10035)
insert into tbltext (id, TextString, TextType) VALUES (10035, 'Klar', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10035 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10035, 2, 'Clear')


If not exists (select * from tbltext where id = 10036)
insert into tbltext (id, TextString, TextType) VALUES (10036, 'Mina ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10036 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10036, 2, 'My cases')

If not exists (select * from tbltext where id = 10037)
insert into tbltext (id, TextString, TextType) VALUES (10037, 'Pågående ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10037 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10037, 2, 'In progress')

If not exists (select * from tbltext where id = 10038)
insert into tbltext (id, TextString, TextType) VALUES (10038, 'Nya idag', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10038 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10038, 2, 'New today')

If not exists (select * from tbltext where id = 10039)
insert into tbltext (id, TextString, TextType) VALUES (10039, 'Avslutade idag', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10039 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10039, 2, 'Closed today')

If not exists (select * from tbltext where id = 10040)
insert into tbltext (id, TextString, TextType) VALUES (10040, 'Välkommen', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10040 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10040, 2, 'Welcome')

If not exists (select * from tbltext where id = 10041)
insert into tbltext (id, TextString, TextType) VALUES (10041, 'Med den här applikationen kan du', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10041 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10041, 2, 'With this application you can')

If not exists (select * from tbltext where id = 10042)
insert into tbltext (id, TextString, TextType) VALUES (10042, 'Skapa ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10042 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10042, 2, 'Create cases')

If not exists (select * from tbltext where id = 10043)
insert into tbltext (id, TextString, TextType) VALUES (10043, 'Tilldela ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10043 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10043, 2, 'Assign cases')

If not exists (select * from tbltext where id = 10044)
insert into tbltext (id, TextString, TextType) VALUES (10044, 'Redigera ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10044 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10044, 2, 'Edit cases')

If not exists (select * from tbltext where id = 10045)
insert into tbltext (id, TextString, TextType) VALUES (10045, 'Hoppas du gillar den!', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10045 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10045, 2, 'Hope you enjoy!')

If not exists (select * from tbltext where id = 10046)
insert into tbltext (id, TextString, TextType) VALUES (10046, 'DH Helpdesk utvecklingsteam​', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 10046 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(10046, 2, 'DH Helpdesk development team')





