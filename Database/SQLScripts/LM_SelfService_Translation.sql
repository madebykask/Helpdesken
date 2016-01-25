If not exists (select * from tbltext where id = 1409)
	insert into tbltext (id, TextString, TextType) VALUES (1409, 'Start', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1409 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1409, 1, 'Start')
GO

If not exists (select * from tbltext where id = 1410)
	insert into tbltext (id, TextString, TextType) VALUES (1410, 'Initiate a case', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1410 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1410, 1, 'Skapa ett ärende')
GO

If not exists (select * from tbltext where id = 1411)
	insert into tbltext (id, TextString, TextType) VALUES (1411, 'Ongoing cases', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1411 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1411, 1, 'Pågående ärenden')
GO

If not exists (select * from tbltext where id = 1412)
	insert into tbltext (id, TextString, TextType) VALUES (1412, 'Documents', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1412 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1412, 1, 'Dokument')
GO

If not exists (select * from tbltext where id = 1413)
	insert into tbltext (id, TextString, TextType) VALUES (1413, 'Need Help?', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1413 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1413, 1, 'Hjälp')
GO

If not exists (select * from tbltext where id = 1414)
	insert into tbltext (id, TextString, TextType) VALUES (1414, 'Bulletin board', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1414 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1414, 1, 'Anslagstavla')
GO
If not exists (select * from tbltext where id = 1417)
	insert into tbltext (id, TextString, TextType) VALUES (1417, 'Search', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1417 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1417, 1, 'Sök')
GO

If not exists (select * from tbltext where id = 1418)
	insert into tbltext (id, TextString, TextType) VALUES (1418, 'Search Phrase', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1418 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1418, 1, 'Sökfras')
GO

If not exists (select * from tbltext where id = 1419)
	insert into tbltext (id, TextString, TextType) VALUES (1419, 'Select language', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1419 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1419, 1, 'Välj språk')
GO

If not exists (select * from tbltext where id = 1420)
	insert into tbltext (id, TextString, TextType) VALUES (1420, 'About', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1420 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1420, 1, 'Om')
GO
If not exists (select * from tbltext where id = 1422)
	insert into tbltext (id, TextString, TextType) VALUES (1422, 'User', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1422 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1422, 1, 'Användare')
GO

If not exists (select * from tbltext where id = 1423)
	insert into tbltext (id, TextString, TextType) VALUES (1423, 'Help', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1423 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1423, 1, 'Hjälp')
GO

If not exists (select * from tbltext where id = 1427)
	insert into tbltext (id, TextString, TextType) VALUES (1427, 'Communicate', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1427 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1427, 1, 'Kommunicera')
GO

If not exists (select * from tbltext where id = 1428)
	insert into tbltext (id, TextString, TextType) VALUES (1428, 'New Case', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1428 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1428, 1, 'Nytt ärende')
GO

If not exists (select * from tbltext where id = 1429)
	insert into tbltext (id, TextString, TextType) VALUES (1429, 'Save and Send', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1429 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1429, 1, 'Spara och skicka')
GO

If not exists (select * from tbltext where id = 1430)
	insert into tbltext (id, TextString, TextType) VALUES (1430, 'User Information', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1430 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1430, 1, 'Användarinformation')
GO

If not exists (select * from tbltext where id = 1431)
	insert into tbltext (id, TextString, TextType) VALUES (1431, 'Computer Information', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1431 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1431, 1, 'Datorinformation')
GO

If not exists (select * from tbltext where id = 1432)
	insert into tbltext (id, TextString, TextType) VALUES (1432, 'Case Information', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1432 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1432, 1, 'Ärendeinformation')
GO

If not exists (select * from tbltext where id = 1433)
	insert into tbltext (id, TextString, TextType) VALUES (1433, 'Send SMS when case is closed', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1433 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1433, 1, 'Skicka SMS när ärendet avslutas')
GO

If not exists (select * from tbltext where id = 1434)
	insert into tbltext (id, TextString, TextType) VALUES (1434, 'Item Cost', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1434 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1434, 1, 'Artikel kostnad')
GO

If not exists (select * from tbltext where id = 1435)
	insert into tbltext (id, TextString, TextType) VALUES (1435, 'Additional Cost', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1435 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1435, 1, 'Tilläggskostnad')
GO

If not exists (select * from tbltext where id = 1436)
	insert into tbltext (id, TextString, TextType) VALUES (1436, 'Currency', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1436 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1436, 1, 'Valuta')
GO

If not exists (select * from tbltext where id = 1437)
	insert into tbltext (id, TextString, TextType) VALUES (1437, 'File name', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1437 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1437, 1, 'Filnamn')
GO

If not exists (select * from tbltext where id = 1438)
	insert into tbltext (id, TextString, TextType) VALUES (1438, 'Add', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1438 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1438, 1, 'Lägg till')
GO

If not exists (select * from tbltext where id = 1439)
	insert into tbltext (id, TextString, TextType) VALUES (1439, 'Close', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1439 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1439, 1, 'Stäng')
GO

If not exists (select * from tbltext where id = 1440)
	insert into tbltext (id, TextString, TextType) VALUES (1440, 'Other', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1440 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1440, 1, 'Annat')
GO

If not exists (select * from tbltext where id = 1441)
	insert into tbltext (id, TextString, TextType) VALUES (1441, 'Case Log', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1441 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1441, 1, 'Ärendelogg')
GO

If not exists (select * from tbltext where id = 1442)
	insert into tbltext (id, TextString, TextType) VALUES (1442, 'Date', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1442 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1442, 1, 'Datum')
GO

If not exists (select * from tbltext where id = 1443)
	insert into tbltext (id, TextString, TextType) VALUES (1443, 'No', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1443 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1443, 1, 'Nej')
GO

If not exists (select * from tbltext where id = 1444)
	insert into tbltext (id, TextString, TextType) VALUES (1444, 'Yes', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1444 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1444, 1, 'Ja')
GO

If not exists (select * from tbltext where id = 1445)
	insert into tbltext (id, TextString, TextType) VALUES (1445, 'Text Note', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1445 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1445, 1, 'Notering')
GO

If not exists (select * from tbltext where id = 1446)
	insert into tbltext (id, TextString, TextType) VALUES (1446, 'Attachment', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1446 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1446, 1, 'Bifogad fil')
GO

If not exists (select * from tbltext where id = 1447)
	insert into tbltext (id, TextString, TextType) VALUES (1447, 'Closed Case', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1447 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1447, 1, 'Avslutat ärende')
GO

If not exists (select * from tbltext where id = 1448)
	insert into tbltext (id, TextString, TextType) VALUES (1448, 'Case', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1448 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1448, 1, 'Ärende')
GO

If not exists (select * from tbltext where id = 1449)
	insert into tbltext (id, TextString, TextType) VALUES (1449, 'IKEA Inside', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1449 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1449, 1, 'IKEA Inside')
GO

If not exists (select * from tbltext where id = 1450)
	insert into tbltext (id, TextString, TextType) VALUES (1450, 'Finished Service Requests', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1450 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1450, 1, 'Avslutade Service Requests')
GO

If not exists (select * from tbltext where id = 1451)
	insert into tbltext (id, TextString, TextType) VALUES (1451, 'Ongoing Service Requests', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1451 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1451, 1, 'Pågående Service Requests')
GO

If not exists (select * from tbltext where id = 1453)
	insert into tbltext (id, TextString, TextType) VALUES (1453, 'Co-worker Employee Number', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1453 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1453, 1, 'Co-worker anställningsnummer')
GO

If not exists (select * from tbltext where id = 1454)
	insert into tbltext (id, TextString, TextType) VALUES (1454, 'Co-worker First name', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1454 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1454, 1, 'Förnamn')
GO

If not exists (select * from tbltext where id = 1455)
	insert into tbltext (id, TextString, TextType) VALUES (1455, 'Co-worker Last name', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1455 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1455, 1, 'Efternamn')
GO

If not exists (select * from tbltext where id = 1456)
	insert into tbltext (id, TextString, TextType) VALUES (1456, 'E-mail', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1456 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1456, 1, 'E-post')
GO

If not exists (select * from tbltext where id = 1457)
	insert into tbltext (id, TextString, TextType) VALUES (1457, 'Co-Workers', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1457 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1457, 1, 'Medarbetare')
GO

If not exists (select * from tbltext where id = 1458)
	insert into tbltext (id, TextString, TextType) VALUES (1458, 'Name', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1458 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1458, 1, 'Namn')
GO

If not exists (select * from tbltext where id = 1459)
	insert into tbltext (id, TextString, TextType) VALUES (1459, 'Description', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1459 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1459, 1, 'Beskrivning')
GO

If not exists (select * from tbltext where id = 1461)
	insert into tbltext (id, TextString, TextType) VALUES (1461, 'HR Documents', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1461 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1461, 1, 'HR Dokument')
GO

If not exists (select * from tbltext where id = 1462)
	insert into tbltext (id, TextString, TextType) VALUES (1462, 'Error Code', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1462 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1462, 1, 'Felkod')
GO

If not exists (select * from tbltext where id = 1463)
	insert into tbltext (id, TextString, TextType) VALUES (1463, 'Me', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1463 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1463, 1, 'Jag')
GO

If not exists (select * from tbltext where id = 1464)
	insert into tbltext (id, TextString, TextType) VALUES (1464, 'Go Back', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1464 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1464, 1, 'Gå tillbaka')
GO

If not exists (select * from tbltext where id = 1465)
	insert into tbltext (id, TextString, TextType) VALUES (1465, 'Case not found!', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1465 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1465, 1, 'Ärendet hittades inte!')
GO

If not exists (select * from tbltext where id = 1466)
	insert into tbltext (id, TextString, TextType) VALUES (1466, 'Case not found among your cases!', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1466 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1466, 1, 'Ärendet hittades inte bland dina ärenden!')
GO
 
If not exists (select * from tbltext where id = 1467)
	insert into tbltext (id, TextString, TextType) VALUES (1467, 'Customer is not valid!', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1467 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1467, 1, 'Kund är inte giltig!')
GO

If not exists (select * from tbltext where id = 1468)
	insert into tbltext (id, TextString, TextType) VALUES (1468, 'Process is not valid!', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1468 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1468, 1, 'Process är inte giltig!')
GO

If not exists (select * from tbltext where id = 1469)
	insert into tbltext (id, TextString, TextType) VALUES (1469, 'You don''t have access to cases, please login again.', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1469 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1469, 1, 'Du har inte tillgång till ärenden, vänligen logga in.')
GO

If not exists (select * from tbltext where id = 1470)
	insert into tbltext (id, TextString, TextType) VALUES (1470, 'Not able to load user cases', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1470 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1470, 1, 'Kunde inte ladda  ärenden för användaren.')
GO

If not exists (select * from tbltext where id = 1471)
	insert into tbltext (id, TextString, TextType) VALUES (1471, 'You don''t have access to the portal.', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1471 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1471, 1, 'Du har inte tillgång till portalen.')
GO

If not exists (select * from tbltext where id = 1472)
	insert into tbltext (id, TextString, TextType) VALUES (1472, 'Customer not found!', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1472 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1472, 1, 'Kunden hittades inte!')
GO

If not exists (select * from tbltext where id = 1473)
	insert into tbltext (id, TextString, TextType) VALUES (1473, 'Customer Id is empty!', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1473 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1473, 1, 'Kund Id är tomt!')
GO

If not exists (select * from tbltext where id = 1474)
	insert into tbltext (id, TextString, TextType) VALUES (1474, 'You don''t have access to the portal. (User Id is not specified)', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1474 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1474, 1, 'Du har inte tillgång till portalen. (Användare är inte angiven)')
GO

If not exists (select * from tbltext where id = 1475)
	insert into tbltext (id, TextString, TextType) VALUES (1475, 'You don''t have access to the portal. (User is not manager for country)', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1475 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1475, 1, 'Du har inte tillgång till portalen. (Användaren är inte ansvarig för landet)')
GO

If not exists (select * from tbltext where id = 1476)
	insert into tbltext (id, TextString, TextType) VALUES (1476, 'You don''t have access to the portal. (Employee Number is not specified)', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1476 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1476, 1, 'Du har inte tillgång till portalen. (Anställningsnummer är inte angivet)')
GO

If not exists (select * from tbltext where id = 1477)
	insert into tbltext (id, TextString, TextType) VALUES (1477, 'You don''t have access to the portal. (User is not manager)', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1477 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1477, 1, 'Du har inte tillgång till portalen. (Användare är inte ansvarig)')
GO

If not exists (select * from tbltext where id = 1478)
	insert into tbltext (id, TextString, TextType) VALUES (1478, 'If you want to add information, please enter it in the fields below:', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1478 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1478, 1, 'Om du vill komplettera med information, gör det i fälten nedan:')
GO

If not exists (select * from tbltext where id = 1479)
	insert into tbltext (id, TextString, TextType) VALUES (1479, 'Should the initiator be contacted before action?', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1479 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1479, 1, 'Ska anmälaren kontaktas innan åtgärd?')
GO


If not exists (select * from tbltext where id = 1480)
	insert into tbltext (id, TextString, TextType) VALUES (1480, 'Initiator is available during the following times', 300)
else
	update tbltext set textstring = 'Initiator is available during the following times' where id = 1480
GO
If not exists (select * from tblTextTranslation where text_id = 1480 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1480, 1, 'Anmälaren nås säkrast på följande tider')
GO

If not exists (select * from tbltext where id = 1481)
	insert into tbltext (id, TextString, TextType) VALUES (1481, 'Add comment', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1481 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1481, 1, 'lägg till kommentar')
GO

If not exists (select * from tbltext where id = 1482)
	insert into tbltext (id, TextString, TextType) VALUES (1482, 'Send', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1482 and Language_Id = 1)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1482, 1, 'Skicka')
GO

If not exists (select * from tbltext where id = 1578)
	insert into tbltext (id, TextString, TextType) VALUES (1578, 'Select for: Hiring a new co-worker, Re-hiring a co-worker returning to IKEA, IKEA Co-workers moving between countries, IKEA Co-workers changing Business Unit.', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1578 and Language_Id = 2)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1578, 2, 'Select for: Hiring a new co-worker, Re-hiring a co-worker returning to IKEA, IKEA Co-workers moving between countries, IKEA Co-workers changing Business Unit.')
GO

If not exists (select * from tbltext where id = 1579)
	insert into tbltext (id, TextString, TextType) VALUES (1579, 'Select for: Changes to Function, Team, Job, Cost Centre or Basic Pay/Salary changes.', 300)
GO
If not exists (select * from tblTextTranslation where text_id = 1579 and Language_Id = 2)
	insert into tblTextTranslation(Text_Id, Language_Id, TextTranslation) VALUES(1579, 2, 'Select for: Changes to Function, Team, Job, Cost Centre or Basic Pay/Salary changes.')
