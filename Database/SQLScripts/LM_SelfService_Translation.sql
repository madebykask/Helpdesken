

DECLARE @textType INT
SET @textType = 300 --SelfService/LineManager

If not exists (select * from tbltext where id = 1409 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1409, 'Start', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1409 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1409, 1, 'Start')
--GO

If not exists (select * from tbltext where id = 1410 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1410, 'Initiate a case', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1410 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1410, 1, 'Skapa ett ärende')
--GO

If not exists (select * from tbltext where id = 1411 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1411, 'Ongoing cases', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1411 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1411, 1, 'Pågående ärenden')
--GO

If not exists (select * from tbltext where id = 1412 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1412, 'Documents', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1412 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1412, 1, 'Dokument')
--GO

If not exists (select * from tbltext where id = 1413 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1413, 'Need Help?', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1413 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1413, 1, 'Hjälp')
--GO

If not exists (select * from tbltext where id = 1414 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1414, 'Bulletin board', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1414 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1414, 1, 'Anslagstavla')
--GO
If not exists (select * from tbltext where id = 1417 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1417, 'Search', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1417 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1417, 1, 'Sök')
--GO

If not exists (select * from tbltext where id = 1418 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1418, 'Search Phrase', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1418 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1418, 1, 'Sökfras')
--GO

If not exists (select * from tbltext where id = 1419 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1419, 'Select language', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1419 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1419, 1, 'Välj språk')
--GO

If not exists (select * from tbltext where id = 1420 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1420, 'About', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1420 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1420, 1, 'Om')
--GO
If not exists (select * from tbltext where id = 1422 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1422, 'User', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1422 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1422, 1, 'Användare')
--GO

If not exists (select * from tbltext where id = 1423 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1423, 'Help', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1423 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1423, 1, 'Hjälp')
--GO

If not exists (select * from tbltext where id = 1427 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1427, 'Communicate', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1427 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1427, 1, 'Kommunicera')
--GO

If not exists (select * from tbltext where id = 1428 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1428, 'New Case', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1428 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1428, 1, 'Nytt ärende')
--GO

If not exists (select * from tbltext where id = 1429 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1429, 'Save and Send', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1429 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1429, 1, 'Spara och skicka')
--GO

If not exists (select * from tbltext where id = 1430 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1430, 'User Information', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1430 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1430, 1, 'Användarinformation')
--GO

If not exists (select * from tbltext where id = 1431 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1431, 'Computer Information', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1431 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1431, 1, 'Datorinformation')
--GO

If not exists (select * from tbltext where id = 1432 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1432, 'Case Information', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1432 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1432, 1, 'Ärendeinformation')
--GO

If not exists (select * from tbltext where id = 1433 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1433, 'Send SMS when case is closed', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1433 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1433, 1, 'Skicka SMS när ärendet avslutas')
--GO

If not exists (select * from tbltext where id = 1434 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1434, 'Item Cost', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1434 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1434, 1, 'Artikel kostnad')
--GO

If not exists (select * from tbltext where id = 1435 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1435, 'Additional Cost', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1435 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1435, 1, 'Tilläggskostnad')
--GO

If not exists (select * from tbltext where id = 1436 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1436, 'Currency', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1436 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1436, 1, 'Valuta')
--GO

If not exists (select * from tbltext where id = 1437 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1437, 'File name', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1437 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1437, 1, 'Filnamn')
--GO

If not exists (select * from tbltext where id = 1438 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1438, 'Add', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1438 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1438, 1, 'Lägg till')
--GO

If not exists (select * from tbltext where id = 1439 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1439, 'Close', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1439 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1439, 1, 'Stäng')
--GO

If not exists (select * from tbltext where id = 1440 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1440, 'Other', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1440 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1440, 1, 'Annat')
--GO

If not exists (select * from tbltext where id = 1441 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1441, 'Case Log', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1441 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1441, 1, 'Ärendelogg')
--GO

If not exists (select * from tbltext where id = 1442 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1442, 'Date', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1442 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1442, 1, 'Datum')
--GO

If not exists (select * from tbltext where id = 1443 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1443, 'No', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1443 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1443, 1, 'Nej')
--GO

If not exists (select * from tbltext where id = 1444 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1444, 'Yes', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1444 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1444, 1, 'Ja')
--GO

If not exists (select * from tbltext where id = 1445 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1445, 'Text Note', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1445 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1445, 1, 'Notering')
--GO

If not exists (select * from tbltext where id = 1446 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1446, 'Attachment', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1446 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1446, 1, 'Bifogad fil')
--GO

If not exists (select * from tbltext where id = 1447 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1447, 'Closed Case', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1447 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1447, 1, 'Avslutat ärende')
--GO

If not exists (select * from tbltext where id = 1448 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1448, 'Case', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1448 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1448, 1, 'Ärende')
--GO

If not exists (select * from tbltext where id = 1449 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1449, 'IKEA Inside', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1449 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1449, 1, 'IKEA Inside')
--GO

If not exists (select * from tbltext where id = 1450 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1450, 'Finished Service Requests', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1450 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1450, 1, 'Avslutade Service Requests')
--GO

If not exists (select * from tbltext where id = 1451 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1451, 'Ongoing Service Requests', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1451 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1451, 1, 'Pågående Service Requests')
--GO

If not exists (select * from tbltext where id = 1453 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1453, 'Co-worker Employee Number', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1453 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1453, 1, 'Co-worker anställningsnummer')
--GO

If not exists (select * from tbltext where id = 1454 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1454, 'Co-worker First name', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1454 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1454, 1, 'Förnamn')
--GO

If not exists (select * from tbltext where id = 1455 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1455, 'Co-worker Last name', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1455 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1455, 1, 'Efternamn')
--GO

If not exists (select * from tbltext where id = 1456 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1456, 'E-mail', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1456 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1456, 1, 'E-post')
--GO

If not exists (select * from tbltext where id = 1457 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1457, 'Co-Workers', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1457 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1457, 1, 'Medarbetare')
--GO

If not exists (select * from tbltext where id = 1458 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1458, 'Name', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1458 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1458, 1, 'Namn')
--GO

If not exists (select * from tbltext where id = 1459 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1459, 'Description', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1459 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1459, 1, 'Beskrivning')
--GO

If not exists (select * from tbltext where id = 1461 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1461, 'HR Documents', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1461 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1461, 1, 'HR Dokument')
--GO

If not exists (select * from tbltext where id = 1462 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1462, 'Error Code', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1462 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1462, 1, 'Felkod')
--GO

If not exists (select * from tbltext where id = 1463 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1463, 'Me', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1463 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1463, 1, 'Jag')
--GO

If not exists (select * from tbltext where id = 1464 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1464, 'Go Back', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1464 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1464, 1, 'Gå tillbaka')
--GO

If not exists (select * from tbltext where id = 1465 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1465, 'Case not found!', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1465 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1465, 1, 'Ärendet hittades inte!')
--GO

If not exists (select * from tbltext where id = 1466 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1466, 'Case not found among your cases!', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1466 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1466, 1, 'Ärendet hittades inte bland dina ärenden!')
--GO
 
If not exists (select * from tbltext where id = 1467 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1467, 'Customer is not valid!', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1467 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1467, 1, 'Kund är inte giltig!')
--GO

If not exists (select * from tbltext where id = 1468 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1468, 'Process is not valid!', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1468 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1468, 1, 'Process är inte giltig!')
--GO

If not exists (select * from tbltext where id = 1469 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1469, 'You don''t have access to cases, please login again.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1469 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1469, 1, 'Du har inte tillgång till ärenden, vänligen logga in.')
--GO

If not exists (select * from tbltext where id = 1470 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1470, 'Not able to load user cases', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1470 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1470, 1, 'Kunde inte ladda  ärenden för användaren.')
--GO

If not exists (select * from tbltext where id = 1471 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1471, 'You don''t have access to the portal.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1471 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1471, 1, 'Du har inte tillgång till portalen.')
--GO

If not exists (select * from tbltext where id = 1472 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1472, 'Customer not found!', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1472 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1472, 1, 'Kunden hittades inte!')
--GO

If not exists (select * from tbltext where id = 1473 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1473, 'Customer Id is empty!', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1473 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1473, 1, 'Kund Id är tomt!')
--GO

If not exists (select * from tbltext where id = 1474 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1474, 'You don''t have access to the portal. (User Id is not specified)', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1474 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1474, 1, 'Du har inte tillgång till portalen. (Användare är inte angiven)')
--GO

If not exists (select * from tbltext where id = 1475 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1475, 'You don''t have access to the portal. (User is not manager for country)', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1475 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1475, 1, 'Du har inte tillgång till portalen. (Användaren är inte ansvarig för landet)')
--GO

If not exists (select * from tbltext where id = 1476 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1476, 'You don''t have access to the portal. (Employee Number is not specified)', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1476 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1476, 1, 'Du har inte tillgång till portalen. (Anställningsnummer är inte angivet)')
--GO

If not exists (select * from tbltext where id = 1477 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1477, 'You don''t have access to the portal. (User is not manager)', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1477 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1477, 1, 'Du har inte tillgång till portalen. (Användare är inte ansvarig)')
--GO

If not exists (select * from tbltext where id = 1478 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1478, 'If you want to add information, please enter it in the fields below:', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1478 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1478, 1, 'Om du vill komplettera med information, gör det i fälten nedan:')
--GO

If not exists (select * from tbltext where id = 1479 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1479, 'Should the initiator be contacted before action?', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1479 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1479, 1, 'Ska anmälaren kontaktas innan åtgärd?')
--GO


If not exists (select * from tbltext where id = 1480 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1480, 'Initiator is available during the following times', @textType)
else
	update tbltext set textstring = 'Initiator is available during the following times' where id = 1480
--GO
If not exists (select * from tblTextTranslation where text_id = 1480 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1480, 1, 'Anmälaren nås säkrast på följande tider')
--GO

If not exists (select * from tbltext where id = 1481 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1481, 'Add comment', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1481 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1481, 1, 'lägg till kommentar')
--GO

If not exists (select * from tbltext where id = 1482 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1482, 'Send', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1482 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1482, 1, 'Skicka')
--GO

If not exists (select * from tbltext where id = 1578 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1578, 'Select for: Hiring a new co-worker, Re-hiring a co-worker returning to IKEA, IKEA Co-workers moving between countries, IKEA Co-workers changing Business Unit.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1578 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1578, 1, 'Select for: Hiring a new co-worker, Re-hiring a co-worker returning to IKEA, IKEA Co-workers moving between countries, IKEA Co-workers changing Business Unit.')
--GO

If not exists (select * from tbltext where id = 1579 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1579, 'Select for: Changes to Function, Team, Job, Cost Centre or Basic Pay/Salary changes.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1579 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1579, 1, 'Select for: Changes to Function, Team, Job, Cost Centre or Basic Pay/Salary changes.')

If not exists (select * from tbltext where id = 1693 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1693, 'Add from clipboard', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1693 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1693, 1, 'Lägg till från Urklipp')

If not exists (select * from tbltext where id = 1694 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1694, 'Copy image data into clipboard and press Ctrl+V', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1694 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1694, 1, 'Kopiera bilddata till Urklipp och tryck Ctrl + V')

If not exists (select * from tbltext where id = 1695 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1695, 'Preview', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1695 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1695, 1, 'Förhandsgranska')

If not exists (select * from tbltext where id = 1696 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1696, 'Save', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1696 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1696, 1, 'Spara')

If not exists (select * from tbltext where id = 1697 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1697, 'Cancel', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1697 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1697, 1, 'Avbryt')


If not exists (select * from tbltext where id = 1704 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1704, 'Categories', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1704 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1704, 1, 'Kategorier')


If not exists (select * from tbltext where id = 1705 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1705, 'Answer', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1705 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1705, 1, 'Svar')


If not exists (select * from tbltext where id = 1706 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1706, 'Internal response', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1706 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1706, 1, 'Internt svar')


If not exists (select * from tbltext where id = 1709 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1709, 'One or more mandatory fields were not filled in, please check the case.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1709 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1709, 1, 'Ett eller flera obligatoriska fält! Var vänlig kontrollera i ärendet.')

	
If not exists (select * from tbltext where id = 1710 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1710, 'The case could not be saved because the lowest possible level on the field', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1710 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1710, 1, 'Ärendet kunde inte sparas då den lägsta möjliga nivån på fält')

	
If not exists (select * from tbltext where id = 1711 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1711, 'has not been selected. Please check the case.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1711 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1711, 1, 'inte har valts. Vänligen kontrollera ärendet.')
	

	
If not exists (select * from tbltext where id = 1751 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1751, 'Select E-mail', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1751 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1751, 1, 'Välj E-post')
	

If not exists (select * from tbltext where id = 1752 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1752, 'Done', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1752 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1752, 1, 'Klar')
	

If not exists (select * from tbltext where id = 1753 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1753, 'Email address is not valid.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1753 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1753, 1, 'E-postadress är inte giltig.')
	

If not exists (select * from tbltext where id = 1754 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1754, 'Same email already added.', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1754 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1754, 1, 'Samma e-post redan lagts.')
	

If not exists (select * from tbltext where id = 1755 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1755, 'User', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1755 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1755, 1, 'Anmälare')
	

If not exists (select * from tbltext where id = 1756 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1756, 'selected', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1756 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1756, 1, 'vald')
	

If not exists (select * from tbltext where id = 1757 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1757, 'Add followers', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1757 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1757, 1, 'Välj följare')
	
If not exists (select * from tbltext where id = 1761 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1761, 'must be specified', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1761 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1761, 1, 'måste anges')
	

If not exists (select * from tbltext where id = 1775 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1775, '1. Write filename (leave blank for automatic naming)', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1775 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1775, 1, '1. Skriv ett filnamn (kan lämnas blankt)')
--GO

If not exists (select * from tbltext where id = 1776 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1776, '2. Press Ctrl+V for inserting picture', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1776 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1776, 1, '2. Tryck Ctrl+V för att klippa in bilden')
--GO

If not exists (select * from tbltext where id = 1777 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1777, '3. Click on Save for adding to case', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1777 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1777, 1, '3. Spara för att lägga till på ärendet')
--GO

If not exists (select * from tbltext where id = 1779 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1779, 'Comment is required!', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1779 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1779, 1, 'Måste ange kommentar!')
--GO
	
If not exists (select * from tbltext where id = 1780 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1780, 'Re-open', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1780 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1780, 1, 'Aktivera')
--GO

If not exists (select * from tbltext where id = 1793 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1793, 'Orders', @textType)
--GO

If not exists (select * from tblTextTranslation where text_id = 1793 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1793, 1, 'Beställningar')
If not exists (select * from tbltext where id = 1794 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1794, 'Records in search result', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1794 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1794, 1, 'Poster i sökresultat')
--GO

If not exists (select * from tbltext where id = 1795 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1795, 'Cases', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1795 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1795, 1, 'Ärenden')
--GO

If not exists (select * from tbltext where id = 1796 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1796, 'All Cases', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1796 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1796, 1, 'Alla ärenden')
--GO

If not exists (select * from tbltext where id = 1806 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1806, 'Followers', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1806 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1806, 1, 'Följare')
--GO

If not exists (select * from tbltext where id = 1881 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1881, 'Attachments', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1881 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1881, 1, 'Bilagor')
--GO

If not exists (select * from tbltext where id = 1986 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (1986, 'Calendar', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1986 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1986, 1, 'Aktuellt')
--GO

If not exists (select * from tbltext where id = 1987 AND TextType = @textType)
    insert into tbltext (id, TextString, TextType) VALUES (1987, 'Operational log', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 1987 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1987, 1, 'Driftlogg')
--GO

If exists (select * from tblTextTranslation where text_id = 2006)
	delete from tblTextTranslation where text_id = 2006

If not exists (select * from tbltext where id = 2006)
	delete from tbltext where id = 2006

If not exists (select * from tbltext where id = 2006 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2006, 'Case is not valid', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2006 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2006, 1, 'Utökat ärende är inte giltigt')
--GO

If not exists (select * from tbltext where id = 2007 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2007, 'Initiator', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2007 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2007, 1, 'Anmälare')
--GO

If not exists (select * from tbltext where id = 2008 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2008, 'Regarding', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2008 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2008, 1, 'Angående')
--GO

If not exists (select * from tbltext where id = 2009 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2009, 'Computer information', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2009 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2009, 1, 'Datorinformation')
--GO

If not exists (select * from tbltext where id = 2010 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2010, 'Case information', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2010 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2010, 1, 'Ärendeinformation')
--GO

If not exists (select * from tbltext where id = 2011 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2011, 'Case management', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2011 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2011, 1, 'Ärendehantering')
--GO

If not exists (select * from tbltext where id = 2012 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2012, 'Communication', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2012 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2012, 1, 'Kommunikation')
--GO

If not exists (select * from tbltext where id = 2013 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2013, 'Status', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2013 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2013, 1, 'Status')
--GO

If not exists (select * from tbltext where id = 2014 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2014, 'Invoice', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2014 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2014, 1, 'Fakturor')
--GO

If not exists (select * from tbltext where id = 2015 AND TextType = @textType)
	insert into tbltext (id, TextString, TextType) VALUES (2015, 'Invoicing', @textType)
--GO
If not exists (select * from tblTextTranslation where text_id = 2015 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2015, 1, 'Fakturering')
--GO


GO
-- NOTE: Please check Translations.sql to avoid tblText.Id collision
