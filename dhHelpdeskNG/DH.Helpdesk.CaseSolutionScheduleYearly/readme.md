# DH.Helpdesk.CaseSolutionScheduleYearly

## Konfiguration

### 1. App.config

Lägg till följande inställningar i din `App.config` under `<appSettings>`:
	<appSettings>
		<add key="CustomerId" value="1" />
		<add key="ErrorMailTo" value="katarina.ask@dhsolutions.se" />
		<add key="ErrorMailSender" value="noreply@dhsolutions.se" />
		<add key="SmtpServer" value="relay2.hostnet.se" />
		<add key="SmtpPort" value="25" />
		<add key="LogFilePath" value="C:/temp/logs/CaseSolutionYearly.log" />
	</appSettings>

- **CustomerId**: ID för den kund vars inställningar ska användas (t.ex. Graph-klient, avsändare).
- **ErrorMailTo**: E-postadress dit felmejl ska skickas.
- **LogFilePath**: Fullständig sökväg och filnamn för loggfilen. Mappen måste finnas.

### 2. Loggning

Loggning sker till den fil som anges i `LogFilePath`. Om mappen inte finns måste du skapa den manuellt.

### 3. Felmejl via Microsoft Graph/Inställningar i App.config

Felmejl skickas automatiskt om något går fel under körning.  
Konfigurationen hämtas från `App.config` och från kundinställningar i databasen.

#### Kundinställningar i databasen

Följande måste vara korrekt ifyllt i kundens inställningar om man skickar via Graph:

- **GraphTenantId**: Azure AD Tenant ID
- **GraphClientId**: App-registreringens klient-ID
- **GraphClientSecret**: App-registreringens hemlighet
- **GraphUserName**: E-postadress för det konto som skickar mejlet (måste ha rättigheter att skicka via Graph)
- **ErrorMailTo** 

### 4. Tips vid felsökning

- Kontrollera att loggfilen skapas på angiven plats.
- Kontrollera att mappen för loggfilen finns.
- Kontrollera att rätt CustomerId används och att kundens Graph-inställningar i databasen är korrekta.
- Om felmejl inte skickas, kontrollera loggen för detaljerad felinformation.

---

**Behöver du skicka via SMTP-relay istället för Graph?**  
Byt ut metoden för felmejl till SMTP och lägg till nödvändiga SMTP-inställningar i `App.config`.

---

**Frågor?**  
Kontakta systemansvarig eller utvecklare för hjälp med konfigurationen.
