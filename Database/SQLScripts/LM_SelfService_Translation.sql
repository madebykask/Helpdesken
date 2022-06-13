
If not exists (select * from tbltext where id = 1409 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1409, 'Start', 300)
GO

If not exists (select * from tblTextTranslation where text_id = 1409 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1409, 1, 'Start')
GO

If not exists (select * from tbltext where id = 1410 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1410, 'Initiate a case', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1410 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1410, 1, 'Skapa ett ärende')
Go

If not exists (select * from tbltext where id = 1411 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1411, 'Ongoing cases', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1411 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1411, 1, 'Pågående ärenden')
Go

If not exists (select * from tbltext where id = 1412 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1412, 'Documents', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1412 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1412, 1, 'Dokument')
Go

If not exists (select * from tbltext where id = 1413 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1413, 'Need Help?', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1413 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1413, 1, 'Hjälp')
Go

If not exists (select * from tbltext where id = 1414 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1414, 'Bulletin board', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1414 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1414, 1, 'Anslagstavla')
Go
If not exists (select * from tbltext where id = 1417 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1417, 'Search', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1417 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1417, 1, 'Sök')
Go

If not exists (select * from tbltext where id = 1418 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1418, 'Search Phrase', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1418 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1418, 1, 'Sökfras')
Go

If not exists (select * from tbltext where id = 1419 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1419, 'Select language', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1419 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1419, 1, 'Välj språk')
Go

If not exists (select * from tbltext where id = 1420 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1420, 'About', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1420 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1420, 1, 'Om')
Go
If not exists (select * from tbltext where id = 1422 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1422, 'User', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1422 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1422, 1, 'Användare')
Go

If not exists (select * from tbltext where id = 1423 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1423, 'Help', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1423 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1423, 1, 'Hjälp')
Go

If not exists (select * from tbltext where id = 1427 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1427, 'Communicate', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1427 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1427, 1, 'Kommunicera')
Go

If not exists (select * from tbltext where id = 1428 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1428, 'New Case', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1428 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1428, 1, 'Nytt ärende')
Go

If not exists (select * from tbltext where id = 1429 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1429, 'Save and Send', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1429 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1429, 1, 'Spara och skicka')
Go

If not exists (select * from tbltext where id = 1430 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1430, 'User Information', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1430 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1430, 1, 'Användarinformation')
Go

If not exists (select * from tbltext where id = 1431 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1431, 'Computer Information', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1431 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1431, 1, 'Datorinformation')
Go

If not exists (select * from tbltext where id = 1432 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1432, 'Case Information', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1432 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1432, 1, 'Ärendeinformation')
Go

If not exists (select * from tbltext where id = 1433 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1433, 'Send SMS when case is closed', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1433 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1433, 1, 'Skicka SMS när ärendet avslutas')
Go

If not exists (select * from tbltext where id = 1434 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1434, 'Item Cost', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1434 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1434, 1, 'Artikel kostnad')
Go

If not exists (select * from tbltext where id = 1435 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1435, 'Additional Cost', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1435 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1435, 1, 'Tilläggskostnad')
Go

If not exists (select * from tbltext where id = 1436 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1436, 'Currency', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1436 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1436, 1, 'Valuta')
Go

If not exists (select * from tbltext where id = 1437 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1437, 'File name', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1437 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1437, 1, 'Filnamn')
Go

If not exists (select * from tbltext where id = 1438 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1438, 'Add', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1438 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1438, 1, 'Lägg till')
Go

If not exists (select * from tbltext where id = 1439 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1439, 'Close', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1439 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1439, 1, 'Stäng')
Go

If not exists (select * from tbltext where id = 1440 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1440, 'Other', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1440 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1440, 1, 'Annat')
Go

If not exists (select * from tbltext where id = 1441 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1441, 'Case Log', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1441 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1441, 1, 'Ärendelogg')
Go

If not exists (select * from tbltext where id = 1442 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1442, 'Date', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1442 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1442, 1, 'Datum')
Go

If not exists (select * from tbltext where id = 1443 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1443, 'No', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1443 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1443, 1, 'Nej')
Go

If not exists (select * from tbltext where id = 1444 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1444, 'Yes', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1444 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1444, 1, 'Ja')
Go

If not exists (select * from tbltext where id = 1445 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1445, 'Text Note', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1445 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1445, 1, 'Notering')
Go

If not exists (select * from tbltext where id = 1446 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1446, 'Attachment', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1446 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1446, 1, 'Bifogad fil')
Go

If not exists (select * from tbltext where id = 1447 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1447, 'Closed Case', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1447 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1447, 1, 'Avslutat ärende')
Go

If not exists (select * from tbltext where id = 1448 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1448, 'Case', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1448 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1448, 1, 'Ärende')
Go

If not exists (select * from tbltext where id = 1449 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1449, 'IKEA Inside', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1449 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1449, 1, 'IKEA Inside')
Go

If not exists (select * from tbltext where id = 1450 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1450, 'Finished Service Requests', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1450 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1450, 1, 'Avslutade Service Requests')
Go

If not exists (select * from tbltext where id = 1451 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1451, 'Ongoing Service Requests', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1451 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1451, 1, 'Pågående Service Requests')
Go

If not exists (select * from tbltext where id = 1453 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1453, 'Co-worker Employee Number', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1453 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1453, 1, 'Co-worker anställningsnummer')
Go

If not exists (select * from tbltext where id = 1454 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1454, 'Co-worker First name', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1454 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1454, 1, 'Förnamn')
Go

If not exists (select * from tbltext where id = 1455 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1455, 'Co-worker Last name', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1455 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1455, 1, 'Efternamn')
Go

If not exists (select * from tbltext where id = 1456 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1456, 'E-mail', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1456 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1456, 1, 'E-post')
Go

If not exists (select * from tbltext where id = 1457 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1457, 'Co-Workers', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1457 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1457, 1, 'Medarbetare')
Go

If not exists (select * from tbltext where id = 1458 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1458, 'Name', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1458 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1458, 1, 'Namn')
Go

If not exists (select * from tbltext where id = 1459 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1459, 'Description', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1459 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1459, 1, 'Beskrivning')
Go

If not exists (select * from tbltext where id = 1461 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1461, 'HR Documents', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1461 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1461, 1, 'HR Dokument')
Go

If not exists (select * from tbltext where id = 1462 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1462, 'Error Code', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1462 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1462, 1, 'Felkod')
Go

If not exists (select * from tbltext where id = 1463 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1463, 'Me', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1463 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1463, 1, 'Jag')
Go

If not exists (select * from tbltext where id = 1464 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1464, 'Go Back', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1464 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1464, 1, 'Gå tillbaka')
Go

If not exists (select * from tbltext where id = 1465 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1465, 'Case not found!', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1465 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1465, 1, 'Ärendet hittades inte!')
Go

If not exists (select * from tbltext where id = 1466 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1466, 'Case not found among your cases!', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1466 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1466, 1, 'Ärendet hittades inte bland dina ärenden!')
Go
 
If not exists (select * from tbltext where id = 1467 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1467, 'Customer is not valid!', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1467 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1467, 1, 'Kund är inte giltig!')
Go

If not exists (select * from tbltext where id = 1468 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1468, 'Process is not valid!', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1468 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1468, 1, 'Process är inte giltig!')
Go

If not exists (select * from tbltext where id = 1469 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1469, 'You don''t have access to cases, please login again.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1469 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1469, 1, 'Du har inte tillgång till ärenden, vänligen logga in.')
Go

If not exists (select * from tbltext where id = 1470 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1470, 'Not able to load user cases', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1470 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1470, 1, 'Kunde inte ladda  ärenden för användaren.')
Go

If not exists (select * from tbltext where id = 1471 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1471, 'You don''t have access to the portal.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1471 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1471, 1, 'Du har inte tillgång till portalen.')
Go

If not exists (select * from tbltext where id = 1472 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1472, 'Customer not found!', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1472 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1472, 1, 'Kunden hittades inte!')
Go

If not exists (select * from tbltext where id = 1473 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1473, 'Customer Id is empty!', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1473 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1473, 1, 'Kund Id är tomt!')
Go

If not exists (select * from tbltext where id = 1474 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1474, 'You don''t have access to the portal. (User Id is not specified)', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1474 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1474, 1, 'Du har inte tillgång till portalen. (Användare är inte angiven)')
Go

If not exists (select * from tbltext where id = 1475 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1475, 'You don''t have access to the portal. (User is not manager for country)', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1475 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1475, 1, 'Du har inte tillgång till portalen. (Användaren är inte ansvarig för landet)')
Go

If not exists (select * from tbltext where id = 1476 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1476, 'You don''t have access to the portal. (Employee Number is not specified)', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1476 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1476, 1, 'Du har inte tillgång till portalen. (Anställningsnummer är inte angivet)')
Go

If not exists (select * from tbltext where id = 1477 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1477, 'You don''t have access to the portal. (User is not manager)', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1477 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1477, 1, 'Du har inte tillgång till portalen. (Användare är inte ansvarig)')
Go

If not exists (select * from tbltext where id = 1478 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1478, 'If you want to add information, please enter it in the fields below:', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1478 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1478, 1, 'Om du vill komplettera med information, gör det i fälten nedan:')
Go

If not exists (select * from tbltext where id = 1479 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1479, 'Should the initiator be contacted before action?', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1479 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1479, 1, 'Ska anmälaren kontaktas innan åtgärd?')
Go


If not exists (select * from tbltext where id = 1480 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1480, 'Initiator is available during the following times', 300)
else
	update tbltext set textstring = 'Initiator is available during the following times' where id = 1480
Go
If not exists (select * from tblTextTranslation where text_id = 1480 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1480, 1, 'Anmälaren nås säkrast på följande tider')
Go

If not exists (select * from tbltext where id = 1481 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1481, 'Add comment', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1481 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1481, 1, 'lägg till kommentar')
Go

If not exists (select * from tbltext where id = 1482 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1482, 'Send', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1482 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1482, 1, 'Skicka')
Go

If not exists (select * from tbltext where id = 1578 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1578, 'Select for: Hiring a new co-worker, Re-hiring a co-worker returning to IKEA, IKEA Co-workers moving between countries, IKEA Co-workers changing Business Unit.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1578 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1578, 1, 'Select for: Hiring a new co-worker, Re-hiring a co-worker returning to IKEA, IKEA Co-workers moving between countries, IKEA Co-workers changing Business Unit.')
Go

If not exists (select * from tbltext where id = 1579 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1579, 'Select for: Changes to Function, Team, Job, Cost Centre or Basic Pay/Salary changes.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1579 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1579, 1, 'Select for: Changes to Function, Team, Job, Cost Centre or Basic Pay/Salary changes.')

If not exists (select * from tbltext where id = 1693 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1693, 'Add from clipboard', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1693 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1693, 1, 'Lägg till från Urklipp')

If not exists (select * from tbltext where id = 1694 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1694, 'Copy image data into clipboard and press Ctrl+V', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1694 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1694, 1, 'Kopiera bilddata till Urklipp och tryck Ctrl + V')

If not exists (select * from tbltext where id = 1695 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1695, 'Preview', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1695 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1695, 1, 'Förhandsgranska')

If not exists (select * from tbltext where id = 1696 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1696, 'Save', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1696 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1696, 1, 'Spara')

If not exists (select * from tbltext where id = 1697 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1697, 'Cancel', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1697 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1697, 1, 'Avbryt')


If not exists (select * from tbltext where id = 1704 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1704, 'Categories', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1704 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1704, 1, 'Kategorier')


If not exists (select * from tbltext where id = 1705 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1705, 'Answer', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1705 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1705, 1, 'Svar')


If not exists (select * from tbltext where id = 1706 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1706, 'Internal response', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1706 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1706, 1, 'Internt svar')


If not exists (select * from tbltext where id = 1709 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1709, 'One or more mandatory fields were not filled in, please check the case.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1709 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1709, 1, 'Ett eller flera obligatoriska fält! Var vänlig kontrollera i ärendet.')

	
If not exists (select * from tbltext where id = 1710 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1710, 'The case could not be saved because the lowest possible level on the field', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1710 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1710, 1, 'Ärendet kunde inte sparas då den lägsta möjliga nivån på fält')

	
If not exists (select * from tbltext where id = 1711 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1711, 'has not been selected. Please check the case.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1711 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1711, 1, 'inte har valts. Vänligen kontrollera ärendet.')
	

	
If not exists (select * from tbltext where id = 1751 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1751, 'Select E-mail', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1751 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1751, 1, 'Välj E-post')
	

If not exists (select * from tbltext where id = 1752 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1752, 'Done', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1752 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1752, 1, 'Klar')
	

If not exists (select * from tbltext where id = 1753 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1753, 'Email address is not valid.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1753 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1753, 1, 'E-postadress är inte giltig.')
	

If not exists (select * from tbltext where id = 1754 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1754, 'Same email already added.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1754 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1754, 1, 'Samma e-post redan lagts.')
	

If not exists (select * from tbltext where id = 1755 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1755, 'User', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1755 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1755, 1, 'Anmälare')
	

If not exists (select * from tbltext where id = 1756 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1756, 'selected', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1756 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1756, 1, 'vald')
	

If not exists (select * from tbltext where id = 1757 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1757, 'Add followers', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1757 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1757, 1, 'Välj följare')
	
If not exists (select * from tbltext where id = 1761 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1761, 'must be specified', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1761 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1761, 1, 'måste anges')
	

If not exists (select * from tbltext where id = 1775 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1775, '1. Write filename (leave blank for automatic naming)', 300)
Go

If not exists (select * from tblTextTranslation where text_id = 1775 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1775, 1, '1. Skriv ett filnamn (kan lämnas blankt)')
Go

If not exists (select * from tbltext where id = 1776 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1776, '2. Press Ctrl+V for inserting picture', 300)
Go

If not exists (select * from tblTextTranslation where text_id = 1776 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1776, 1, '2. Tryck Ctrl+V för att klippa in bilden')
Go

If not exists (select * from tbltext where id = 1777 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1777, '3. Click on Save for adding to case', 300)
Go

If not exists (select * from tblTextTranslation where text_id = 1777 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1777, 1, '3. Spara för att lägga till på ärendet')
Go

If not exists (select * from tbltext where id = 1779 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1779, 'Comment is required!', 300)
Go

If not exists (select * from tblTextTranslation where text_id = 1779 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1779, 1, 'Måste ange kommentar!')
Go
	
If not exists (select * from tbltext where id = 1780 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1780, 'Re-open', 300)
Go

If not exists (select * from tblTextTranslation where text_id = 1780 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1780, 1, 'Aktivera')
Go

If not exists (select * from tbltext where id = 1793 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1793, 'Orders', 300)
Go

If not exists (select * from tblTextTranslation where text_id = 1793 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1793, 1, 'Beställningar')
If not exists (select * from tbltext where id = 1794 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1794, 'Records in search result', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1794 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1794, 1, 'Poster i sökresultat')
Go

If not exists (select * from tbltext where id = 1795 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1795, 'Cases', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1795 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1795, 1, 'Ärenden')
Go

If not exists (select * from tbltext where id = 1796 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1796, 'All Cases', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1796 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1796, 1, 'Alla ärenden')
Go

If not exists (select * from tbltext where id = 1806 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1806, 'Followers', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1806 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1806, 1, 'Följare')
Go

If not exists (select * from tbltext where id = 1881 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1881, 'Attachments', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1881 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1881, 1, 'Bilagor')
Go

If not exists (select * from tbltext where id = 1986 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (1986, 'Calendar', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1986 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1986, 1, 'Aktuellt')
Go

If not exists (select * from tbltext where id = 1987 AND TextType = 300)
    insert into tbltext (id, TextString, TextType) VALUES (1987, 'Operational log', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 1987 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1987, 1, 'Driftlogg')
Go

If exists (select * from tblTextTranslation where text_id = 2006)
	delete from tblTextTranslation where text_id = 2006

If exists (select * from tbltext where id = 2006)
	delete from tbltext where id = 2006

If not exists (select * from tbltext where id = 2006 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2006, 'Case is not valid', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2006 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2006, 1, 'Utökat ärende är inte giltigt')
Go

If not exists (select * from tbltext where id = 2113 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2113, 'Initiator', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2113 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2113, 1, 'Anmälare')
Go
--Corrected id:s 20220613 - Katta Ask
If not exists (select * from tbltext where id = 2114 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2114, 'Regarding', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2114 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2114, 1, 'Angående')
Go

If not exists (select * from tbltext where id = 2115 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2115, 'Computer information', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2115 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2115, 1, 'Datorinformation')
Go

If not exists (select * from tbltext where id = 2116 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2116, 'Case information', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2116 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2116, 1, 'Ärendeinformation')
Go

If not exists (select * from tbltext where id = 2117 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2117, 'Case management', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2117 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2117, 1, 'Ärendehantering')
Go

If not exists (select * from tbltext where id = 2118 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2118, 'Communication', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2118 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2118, 1, 'Kommunikation')
Go

If not exists (select * from tbltext where id = 2119 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2119, 'Status', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2119 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2119, 1, 'Status')
Go

If not exists (select * from tbltext where id = 2120 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2120, 'Invoice', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2120 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2120, 1, 'Fakturor')
Go

If not exists (select * from tbltext where id = 2121 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2121, 'Invoicing', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2121 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2121, 1, 'Fakturering')
Go

If not exists (select * from tbltext where id = 2122 AND TextType = 300)
	insert into tbltext (id, TextString, TextType) VALUES (2122, 'Please confirm that you are not a robot.', 300)
Go
If not exists (select * from tblTextTranslation where text_id = 2122 and Language_Id = 1)
    insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(2122, 1, 'Vänligen bekräfta att du inte är en robot.')
Go

-- NOTE: Please check Translations.sql to avoid tblText.Id collision
	