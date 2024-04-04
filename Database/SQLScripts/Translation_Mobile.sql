DECLARE @MobileType INT
SET @MobileType = 500


  -- Add new text type:
IF NOT EXISTS(SELECT 1 FROM tblTextType WHERE TextType = 'Mobile')
BEGIN 
    -- 
    INSERT INTO tblTextType (Id, TextType, [Status])
    VALUES(500, 'Mobile', 1)
END

DECLARE @mobileEndRange INT = 39999
IF EXISTS (SELECT * FROM tblText T WHERE T.Id BETWEEN 30000 AND @mobileEndRange AND T.TextType <> @MobileType)
BEGIN
    RAISERROR ('Error, unknown translations exists within mobile text range', 10, 1) WITH NOWAIT
    RETURN
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

		UPDATE [dbo].[tblTextTranslation] SET [Text_Id] = @NewId WHERE [Text_Id] = @Id

		FETCH NEXT FROM @MyCursor 
	    INTO @Id 
	END

	CLOSE @MyCursor;
    DEALLOCATE @MyCursor;

	ALTER TABLE [dbo].[tblTextTranslation] CHECK CONSTRAINT [FK_tblTextTranslation_tblText]
END
commit transaction


-- Home
If not exists (select * from tbltext where id = 30000)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30000, 'Hem', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30000 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30000, 2, 'Home')
End

-- Add
If not exists (select * from tbltext where id = 30001)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30001, 'Lägg till', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30001 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30001, 2, 'Add')
end

-- Overview
If not exists (select * from tbltext where id = 30002)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30002, 'Översikt', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30002 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30002, 2, 'Overview')
end

-- Case Overview
If not exists (select * from tbltext where id = 30003)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30003, 'Ärendeöversikt', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30003 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30003, 2, 'Case Overview')
end


-- Lanuage
If not exists (select * from tbltext where id = 30004)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30004, 'Språk', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30004 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30004, 2, 'Language')
end

--Log out
If not exists (select * from tbltext where id = 30005)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30005, 'Logga ut', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30005 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30005, 2, 'Log out')
end

-- Search
If not exists (select * from tbltext where id = 30006)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30006, 'Sök', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30006 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30006, 2, 'Search')
end

-- Cancel 
If not exists (select * from tbltext where id = 30007)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30007, 'Avbryt', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30007 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30007, 2, 'Cancel')
end

-- Done
If not exists (select * from tbltext where id = 30008)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30008, 'Klar', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30008 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30008, 2, 'Done')
end

-- Select
If not exists (select * from tbltext where id = 30009)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30009, 'Välj', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30009 and Language_Id = 2)
begin	
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30009, 2, 'Select')
end

-- All Cases
If not exists (select * from tbltext where id = 30012 )
begin
	insert into tbltext (id, TextString, TextType) VALUES (30012, 'Alla ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30012 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30012, 2, 'All cases')
end

-- My Cases
If not exists (select * from tbltext where id = 30013 )
begin
	insert into tbltext (id, TextString, TextType) VALUES (30013, 'Mina ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30013 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30013, 2, 'My cases')
end

-- Lock Message1
If not exists (select * from tbltext where id = 30014 )
begin
	insert into tbltext (id, TextString, TextType) VALUES (30014, 'OBS! Detta ärende är öppnat av', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30014 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30014, 2, 'Please note! This case is opened by') 
end


-- Lock Message2
If not exists (select * from tbltext where id = 30015 )
begin
	insert into tbltext (id, TextString, TextType) VALUES (30015, 'OBS! Du har redan öppnat detta ärende i en annan session', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30015 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30015, 2, 'Please note! You have already opened this case in another session') 
end


-- Attach
If not exists (select * from tbltext where id = 30016 )
begin
	insert into tbltext (id, TextString, TextType) VALUES (30016, 'Bifoga', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30016 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30016, 2, 'Attach')
end


-- File delete confirmation
If not exists (select * from tbltext where id = 30017)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30017, 'Är du säker på att du vill ta bort bifogad fil', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30017 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30017, 2, 'Are you sure you want to delete attached file')
end


-- yes
If not exists (select * from tbltext where id = 30018)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30018, 'Ja', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30018 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30018, 2, 'Yes')
end

-- No
If not exists (select * from tbltext where id = 30019)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30019, 'Nej', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30019 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30019, 2, 'No')
end


-- Case/Ärende
If not exists (select * from tbltext where id = 30020)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30020, 'Ärende', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30020 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30020, 2, 'Case')
end


-- Actions/Händelser
If not exists (select * from tbltext where id = 30021)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30021, 'Händelser', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30021 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30021, 2, 'Actions')
end


-- Workflow/Åtgärd
If not exists (select * from tbltext where id = 30022)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30022, 'Åtgärd', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30022 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30022, 2, 'Workflow')
end


-- Files/Filer
If not exists (select * from tbltext where id = 30023)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30023, 'Bifogad fil', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30023 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30023, 2, 'Attached file')
end


-- Other actions/Övriga händelser
If not exists (select * from tbltext where id = 30024)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30024, 'Övriga händelser', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30024 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30024, 2, 'Other actions')
end

If not exists (select * from tbltext where id = 30025)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30025, 'Stäng', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30025 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30025, 2, 'Close')
end


If not exists (select * from tbltext where id = 30026)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30026, 'Filtrering startar när minst två tecken har angetts', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30026 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30026, 2, 'Filtering will start after input of two characters')
end


If not exists (select * from tbltext where id = 30027)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30027, 'Tillbaka', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30027 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30027, 2, 'Back')
end


If not exists (select * from tbltext where id = 30028)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30028, 'Skriv för att filtrera', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30028 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30028, 2, 'Type to search')
end


If not exists (select * from tbltext where id = 30029)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30029, 'Inget resultat', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30029 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30029, 2, 'No result')
end


If not exists (select * from tbltext where id = 30030)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30030, 'Fältet är obligatoriskt', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30030 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30030, 2, 'Field is required')
end


If not exists (select * from tbltext where id = 30031)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30031, 'Fyll i obligatoriska fält.', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30031 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30031, 2, 'Please fill in required fields.')
end


If not exists (select * from tbltext where id = 30032)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30032, 'Användar ID', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30032 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30032, 2, 'User ID')
end


If not exists (select * from tbltext where id = 30033)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30033, 'Skapa ärende', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30033 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30033, 2, 'Create case')
end


If not exists (select * from tbltext where id = 30034)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30034, 'Välj språk', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30034 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30034, 2, 'Select language')
end


If not exists (select * from tbltext where id = 30035)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30035, 'Klar', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30035 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30035, 2, 'Clear')
end


If not exists (select * from tbltext where id = 30036)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30036, 'Mina ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30036 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30036, 2, 'My cases')
end


If not exists (select * from tbltext where id = 30037)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30037, 'Pågående ärende', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30037 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30037, 2, 'In progress')
end


If not exists (select * from tbltext where id = 30038)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30038, 'Nya idag', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30038 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30038, 2, 'New today')	
end


If not exists (select * from tbltext where id = 30039)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30039, 'Avslutade idag', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30039 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30039, 2, 'Closed today')
end


If not exists (select * from tbltext where id = 30040)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30040, 'Välkommen', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30040 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30040, 2, 'Welcome')
end


If not exists (select * from tbltext where id = 30041)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30041, 'Med den här applikationen kan du', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30041 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30041, 2, 'With this application you can')
end


If not exists (select * from tbltext where id = 30042)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30042, 'Skapa ärenden', @MobileType)
end
if not exists (select * from tblTextTranslation where text_id = 30042 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30042, 2, 'Create cases')
end


If not exists (select * from tbltext where id = 30043)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30043, 'Tilldela ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30043 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30043, 2, 'Assign cases')
end


If not exists (select * from tbltext where id = 30044)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30044, 'Redigera ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30044 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30044, 2, 'Edit cases')
end


If not exists (select * from tbltext where id = 30045)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30045, 'Hoppas du gillar den!', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30045 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30045, 2, 'Hope you enjoy!')
end


If not exists (select * from tbltext where id = 30046)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30046, 'DH Helpdesk utvecklingsteam​', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30046 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30046, 2, 'DH Helpdesk development team')
end


If not exists (select * from tbltext where id = 30047)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30047, 'Pågående ärende', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30047 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30047, 2, 'In progress')
end


If not exists (select * from tbltext where id = 30048)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30048, 'Nya idag', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30048 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30048, 2, 'New today')
end


If not exists (select * from tbltext where id = 30049)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30049, 'Avslutade idag', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30049 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30049, 2, 'Closed today')
end


If not exists (select * from tbltext where id = 30050)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30050, 'Avsluta ärende', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30050 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30050, 2, 'Close case')
end


If not exists (select * from tbltext where id = 30051)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30051, 'Max antal tecken är {0}', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30051 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30051, 2, 'The maximum length is {0} character(s)')
end


If not exists (select * from tbltext where id = 30052)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30052, 'Till', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30052 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30052, 2, 'To')
end


If not exists (select * from tbltext where id = 30053)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30053, 'Kopia', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30053 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30053, 2, 'Cc')
end


If not exists (select * from tbltext where id = 30054)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30054, 'Anmälare', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30054 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30054, 2, 'Initiator')
end


If not exists (select * from tbltext where id = 30055)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30055, 'Handläggare', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30055 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30055, 2, 'Administrator')
end


If not exists (select * from tbltext where id = 30056)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30056, 'Driftgrupp', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30056 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30056, 2, 'WorkingGroup')
end


If not exists (select * from tbltext where id = 30057)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30057, 'E-postgrupp', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30057 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30057, 2, 'EmailGroup')
end


If not exists (select * from tbltext where id = 30058)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30058, 'Användare', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30058 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30058, 2, 'Users')
end


If not exists (select * from tbltext where id = 30059)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30059, 'Ingen tillgänglig mailadress', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30059 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30059, 2, 'No email address available')
end

--
If not exists (select * from tbltext where id = 30060)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30060, 'Kommunicera i ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30060 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30060, 2, 'Communicate in cases')
end

--
If not exists (select * from tbltext where id = 30061)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30061, 'Avsluta ärenden', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30061 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30061, 2, 'Close cases')
end
--
If not exists (select * from tbltext where id = 30062)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30062, 'har inte en giltig filändelse', @MobileType)
end
If not exists (select * from tblTextTranslation where text_id = 30062 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30062, 2, 'does not have a valid file extension')
end
--New sprint 76
If not exists (select * from tbltext where id = 30063)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30063, 'Informera handläggare', @MobileType)
end

If not exists (select * from tblTextTranslation where text_id = 30063 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30063, 2, 'Inform administrator')
end

If not exists (select * from tbltext where id = 30064)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30064, 'Informera', @MobileType)
end

If not exists (select * from tblTextTranslation where text_id = 30064 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30064, 2, 'Inform')
end

If not exists (select * from tbltext where id = 30065)
begin
	insert into tbltext (id, TextString, TextType) VALUES (30065, 'Ärendet uppfyller inte villkoren i business rules.', @MobileType)
end

If not exists (select * from tblTextTranslation where text_id = 30065 and Language_Id = 2)
begin
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(30065, 2, 'The case does not fulfill the conditions in the business rules.')
end


