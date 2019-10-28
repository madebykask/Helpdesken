DECLARE @MobileType INT
SET @MobileType = 500


  -- Add new text type:
IF NOT EXISTS(SELECT 1 FROM tblTextType WHERE TextType = 'Mobile')
BEGIN 
    -- 
    INSERT INTO tblTextType (Id, TextType, [Status])
    VALUES(500, 'Mobile', 1)
END

------------------------- fix for Ikea. moving all mobile translations to start from 30000
-- fix for ids 10000 - 10061
begin transaction
If exists (select top 1 * from [dbo].[tblText] where [TextType] = @MobileType and [Id] < 10062)
BEGIN
	DECLARE @MyCursor CURSOR;
	DECLARE @Id int = 0
	DECLARE @NewId int = 0

	ALTER TABLE [dbo].[tblTextTranslation] NOCHECK CONSTRAINT [FK_tblTextTranslation_tblText]

	SET @MyCursor = CURSOR FOR
	SELECT DISTINCT [Id] FROM [dbo].[tblText]
	WHERE [TextType] = @MobileType and [Id] < 10062

	OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @Id

	WHILE @@FETCH_STATUS = 0
    BEGIN
		SET @NewId = @Id + 20000;
		RAISERROR ('Increasing text translation Id = %d to %d.', 10, 1, @Id, @NewId) WITH NOWAIT
				
		INSERT INTO [dbo].[tblText] (Id, TextString, TextType, CreatedDate, ChangedDate, ChangedByUser_Id, TextGUID)
			SELECT @NewId, [TextString], [TextType], GETUTCDATE(), GETUTCDATE(), [ChangedByUser_Id], [TextGUID]
			FROM [dbo].[tblText] WHERE [Id] = @Id
		DELETE [dbo].[tblText] WHERE [Id] = @Id

		UPDATE [dbo].[tblTextTranslation] SET [Text_Id] = @NewId WHERE [Id] = @Id

		FETCH NEXT FROM @MyCursor 
	    INTO @Id 
	END

	CLOSE @MyCursor;
    DEALLOCATE @MyCursor;

	ALTER TABLE [dbo].[tblTextTranslation] CHECK CONSTRAINT [FK_tblTextTranslation_tblText]
END
rollback transaction

-- Home
If not exists (select * from tbltext where id = 30000)
insert into tbltext (id, TextString, TextType) VALUES (30000, 'Hem', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30000 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30000, 2, 'Home')

-- Add
If not exists (select * from tbltext where id = 30001)
insert into tbltext (id, TextString, TextType) VALUES (30001, 'Lägg till', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30001 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30001, 2, 'Add')

-- Overview
If not exists (select * from tbltext where id = 30002)
insert into tbltext (id, TextString, TextType) VALUES (30002, 'Översikt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30002 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30002, 2, 'Overview')


-- Case Overview
If not exists (select * from tbltext where id = 30003)
insert into tbltext (id, TextString, TextType) VALUES (30003, 'Ärendeöversikt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30003 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30003, 2, 'Case Overview')

-- Lanuage
If not exists (select * from tbltext where id = 30004)
insert into tbltext (id, TextString, TextType) VALUES (30004, 'Språk', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30004 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30004, 2, 'Language')

--Log out
If not exists (select * from tbltext where id = 30005)
insert into tbltext (id, TextString, TextType) VALUES (30005, 'Logga ut', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30005 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30005, 2, 'Log out')

-- Search
If not exists (select * from tbltext where id = 30006)
insert into tbltext (id, TextString, TextType) VALUES (30006, 'Sök', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30006 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30006, 2, 'Search')

-- Cancel 
If not exists (select * from tbltext where id = 30007)
insert into tbltext (id, TextString, TextType) VALUES (30007, 'Avbryt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30007 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30007, 2, 'Cancel')

-- Done
If not exists (select * from tbltext where id = 30008)
insert into tbltext (id, TextString, TextType) VALUES (30008, 'Klar', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30008 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30008, 2, 'Done')

-- Select
If not exists (select * from tbltext where id = 30009)
insert into tbltext (id, TextString, TextType) VALUES (30009, 'Välj', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30009 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30009, 2, 'Select')

-- All Cases
If not exists (select * from tbltext where id = 30012 )
insert into tbltext (id, TextString, TextType) VALUES (30012, 'Alla ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30012 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30012, 2, 'All cases')

-- My Cases
If not exists (select * from tbltext where id = 30013 )
insert into tbltext (id, TextString, TextType) VALUES (30013, 'Mina ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30013 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30013, 2, 'My cases')

-- Lock Message1
If not exists (select * from tbltext where id = 30014 )
insert into tbltext (id, TextString, TextType) VALUES (30014, 'OBS! Detta ärende är öppnat av', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30014 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30014, 2, 'Please note! This case is opened by') 

-- Lock Message2
If not exists (select * from tbltext where id = 30015 )
insert into tbltext (id, TextString, TextType) VALUES (30015, 'OBS! Du har redan öppnat detta ärende i en annan session', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30015 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30015, 2, 'Please note! You have already opened this case in another session') 

-- Attach
If not exists (select * from tbltext where id = 30016 )
insert into tbltext (id, TextString, TextType) VALUES (30016, 'Bifoga', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30016 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30016, 2, 'Attach')

-- File delete confirmation
If not exists (select * from tbltext where id = 30017)
insert into tbltext (id, TextString, TextType) VALUES (30017, 'Är du säker på att du vill ta bort bifogad fil', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30017 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30017, 2, 'Are you sure you want to delete attached file')

-- yes
If not exists (select * from tbltext where id = 30018)
insert into tbltext (id, TextString, TextType) VALUES (30018, 'Ja', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30018 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30018, 2, 'Yes')

-- No
If not exists (select * from tbltext where id = 30019)
insert into tbltext (id, TextString, TextType) VALUES (30019, 'Nej', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30019 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30019, 2, 'No')

-- Case/Ärende
If not exists (select * from tbltext where id = 30020)
insert into tbltext (id, TextString, TextType) VALUES (30020, 'Ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30020 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30020, 2, 'Case')

-- Actions/Händelser
If not exists (select * from tbltext where id = 30021)
insert into tbltext (id, TextString, TextType) VALUES (30021, 'Händelser', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30021 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30021, 2, 'Actions')

-- Workflow/Åtgärd
If not exists (select * from tbltext where id = 30022)
insert into tbltext (id, TextString, TextType) VALUES (30022, 'Åtgärd', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30022 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30022, 2, 'Workflow')

-- Files/Filer
If not exists (select * from tbltext where id = 30023)
insert into tbltext (id, TextString, TextType) VALUES (30023, 'Bifogad fil', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30023 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30023, 2, 'Attached file')

-- Other actions/Övriga händelser
If not exists (select * from tbltext where id = 30024)
insert into tbltext (id, TextString, TextType) VALUES (30024, 'Övriga händelser', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30024 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30024, 2, 'Other actions')

If not exists (select * from tbltext where id = 30025)
insert into tbltext (id, TextString, TextType) VALUES (30025, 'Stäng', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30025 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30025, 2, 'Close')

If not exists (select * from tbltext where id = 30026)
insert into tbltext (id, TextString, TextType) VALUES (30026, 'Filtrering startar när minst två tecken har angetts', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30026 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30026, 2, 'Filtering will start after input of two characters')

If not exists (select * from tbltext where id = 30027)
insert into tbltext (id, TextString, TextType) VALUES (30027, 'Tillbaka', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30027 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30027, 2, 'Back')

If not exists (select * from tbltext where id = 30028)
insert into tbltext (id, TextString, TextType) VALUES (30028, 'Skriv för att filtrera', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30028 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30028, 2, 'Type to search')

If not exists (select * from tbltext where id = 30029)
insert into tbltext (id, TextString, TextType) VALUES (30029, 'Inget resultat', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30029 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30029, 2, 'No result')

If not exists (select * from tbltext where id = 30030)
insert into tbltext (id, TextString, TextType) VALUES (30030, 'Fältet är obligatoriskt', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30030 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30030, 2, 'Field is required')

If not exists (select * from tbltext where id = 30031)
insert into tbltext (id, TextString, TextType) VALUES (30031, 'Fyll i obligatoriska fält.', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30031 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30031, 2, 'Please fill in required fields.')

If not exists (select * from tbltext where id = 30032)
insert into tbltext (id, TextString, TextType) VALUES (30032, 'Användar ID', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30032 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30032, 2, 'User ID')


If not exists (select * from tbltext where id = 30033)
insert into tbltext (id, TextString, TextType) VALUES (30033, 'Skapa ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30033 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30033, 2, 'Create case')

If not exists (select * from tbltext where id = 30034)
insert into tbltext (id, TextString, TextType) VALUES (30034, 'Välj språk', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30034 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30034, 2, 'Select language')

If not exists (select * from tbltext where id = 30035)
insert into tbltext (id, TextString, TextType) VALUES (30035, 'Klar', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30035 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30035, 2, 'Clear')


If not exists (select * from tbltext where id = 30036)
insert into tbltext (id, TextString, TextType) VALUES (30036, 'Mina ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30036 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30036, 2, 'My cases')

If not exists (select * from tbltext where id = 30037)
insert into tbltext (id, TextString, TextType) VALUES (30037, 'Pågående ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30037 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30037, 2, 'In progress')

If not exists (select * from tbltext where id = 30038)
insert into tbltext (id, TextString, TextType) VALUES (30038, 'Nya idag', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30038 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30038, 2, 'New today')

If not exists (select * from tbltext where id = 30039)
insert into tbltext (id, TextString, TextType) VALUES (30039, 'Avslutade idag', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30039 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30039, 2, 'Closed today')

If not exists (select * from tbltext where id = 30040)
insert into tbltext (id, TextString, TextType) VALUES (30040, 'Välkommen', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30040 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30040, 2, 'Welcome')

If not exists (select * from tbltext where id = 30041)
insert into tbltext (id, TextString, TextType) VALUES (30041, 'Med den här applikationen kan du', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30041 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30041, 2, 'With this application you can')

If not exists (select * from tbltext where id = 30042)
insert into tbltext (id, TextString, TextType) VALUES (30042, 'Skapa ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30042 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30042, 2, 'Create cases')

If not exists (select * from tbltext where id = 30043)
insert into tbltext (id, TextString, TextType) VALUES (30043, 'Tilldela ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30043 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30043, 2, 'Assign cases')

If not exists (select * from tbltext where id = 30044)
insert into tbltext (id, TextString, TextType) VALUES (30044, 'Redigera ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30044 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30044, 2, 'Edit cases')

If not exists (select * from tbltext where id = 30045)
insert into tbltext (id, TextString, TextType) VALUES (30045, 'Hoppas du gillar den!', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30045 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30045, 2, 'Hope you enjoy!')

If not exists (select * from tbltext where id = 30046)
insert into tbltext (id, TextString, TextType) VALUES (30046, 'DH Helpdesk utvecklingsteam​', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30046 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30046, 2, 'DH Helpdesk development team')

If not exists (select * from tbltext where id = 30047)
insert into tbltext (id, TextString, TextType) VALUES (30047, 'Pågående ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30047 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30047, 2, 'In progress')

If not exists (select * from tbltext where id = 30048)
insert into tbltext (id, TextString, TextType) VALUES (30048, 'Nya idag', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30048 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30048, 2, 'New today')

If not exists (select * from tbltext where id = 30049)
insert into tbltext (id, TextString, TextType) VALUES (30049, 'Avslutade idag', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30049 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30049, 2, 'Closed today')

If not exists (select * from tbltext where id = 30050)
insert into tbltext (id, TextString, TextType) VALUES (30050, 'Avsluta ärende', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30050 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30050, 2, 'Close case')

If not exists (select * from tbltext where id = 30051)
insert into tbltext (id, TextString, TextType) VALUES (30051, 'Max antal tecken är {0}', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30051 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30051, 2, 'The maximum length is {0} character(s)')

If not exists (select * from tbltext where id = 30052)
insert into tbltext (id, TextString, TextType) VALUES (30052, 'Till', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30052 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30052, 2, 'To')

If not exists (select * from tbltext where id = 30053)
insert into tbltext (id, TextString, TextType) VALUES (30053, 'Kopia', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30053 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30053, 2, 'Cc')

  If not exists (select * from tbltext where id = 30054)
insert into tbltext (id, TextString, TextType) VALUES (30054, 'Anmälare', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30054 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30054, 2, 'Initiator')

If not exists (select * from tbltext where id = 30055)
insert into tbltext (id, TextString, TextType) VALUES (30055, 'Handläggare', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30055 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30055, 2, 'Administrator')

If not exists (select * from tbltext where id = 30056)
insert into tbltext (id, TextString, TextType) VALUES (30056, 'Driftgrupp', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30056 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30056, 2, 'WorkingGroup')

If not exists (select * from tbltext where id = 30057)
insert into tbltext (id, TextString, TextType) VALUES (30057, 'E-postgrupp', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30057 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30057, 2, 'EmailGroup')

If not exists (select * from tbltext where id = 30058)
insert into tbltext (id, TextString, TextType) VALUES (30058, 'Användare', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30058 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30058, 2, 'Users')

If not exists (select * from tbltext where id = 30059)
insert into tbltext (id, TextString, TextType) VALUES (30059, 'Ingen tillgänglig mailadress', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30059 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30059, 2, 'No email address available')
--
If not exists (select * from tbltext where id = 30060)
insert into tbltext (id, TextString, TextType) VALUES (30060, 'Kommunicera i ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30060 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30060, 2, 'Communicate in cases')
--
If not exists (select * from tbltext where id = 30061)
insert into tbltext (id, TextString, TextType) VALUES (30061, 'Avsluta ärenden', @MobileType)

If not exists (select * from tblTextTranslation where text_id = 30061 and Language_Id = 2)
insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30061, 2, 'Close cases')

GO
