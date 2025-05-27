# DH.Helpdesk.CaseSolutionScheduleYearly

## Konfiguration

### 1. App.config

Lägg till följande inställningar i din `App.config` under `<appSettings>`:
<appSettings> <add key="CustomerId" value="1" /> <add key="ErrorMailTo" value="din.epost@domän.se" /> <add key="LogFilePath" value="C:/temp/Logs/app.log" /> </appSettings>

- **CustomerId**: ID för den kund vars inställningar ska användas (t.ex. Graph-klient, avsändare).
- **ErrorMailTo**: E-postadress dit felmejl ska skickas.
- **LogFilePath**: Fullständig sökväg och filnamn för loggfilen. Mappen måste finnas.

### 2. Loggning

Loggning sker till den fil som anges i `LogFilePath`. Om mappen inte finns måste du skapa den manuellt.

### 3. Felmejl via Microsoft Graph

Felmejl skickas automatiskt om något går fel under körning.  
Konfigurationen hämtas från `App.config` och från kundinställningar i databasen.

#### Kundinställningar i databasen

Följande måste vara korrekt ifyllt i kundens inställningar (t.ex. i tabellen för CustomerSettings):

- **GraphTenantId**: Azure AD Tenant ID
- **GraphClientId**: App-registreringens klient-ID
- **GraphClientSecret**: App-registreringens hemlighet
- **GraphUserName**: E-postadress för det konto som skickar mejlet (måste ha rättigheter att skicka via Graph)
- **ErrorMailTo** (om du vill styra mottagare per kund, annars används värdet från App.config)

### 4. Exempel på App.config
<configuration> <startup> <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" /> </startup> <connectionStrings> <add name="Helpdesk" connectionString="Server=DHUTVSQL6; Initial Catalog=Helpdesk-Test; User Id=helpdesk-test; Password=helpdesk-test;TrustServerCertificate=True;" providerName="System.Data.SqlClient" /> </connectionStrings> <appSettings> <add key="CustomerId" value="1" /> <add key="ErrorMailTo" value="din.epost@domän.se" /> <add key="LogFilePath" value="C:/temp/Logs/app.log" /> </appSettings> </configuration>

### 5. Tips vid felsökning

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
