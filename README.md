BellaHair 

BellaHair er et bookingsystem udviklet til frisørsaloner.  
Systemet gør det muligt at administrere bookinger, kunder, medarbejdere, behandlinger, rabatter og fakturering i ét samlet system.

Formålet med systemet er at skabe overblik, effektivisere arbejdsprocesser og sikre en brugervenlig oplevelse for både personale og administratorer.

---

Funktionalitet

Systemet understøtter blandt andet følgende funktioner:

- Oprettelse og administration af bookinger
- Kalenderoversigt med filtrering pr. medarbejder
- Kundehåndtering (private og firmakunder)
- Medarbejder- og behandlingsstyring
- Rabat- og loyalitetssystem
- Fakturering med historisk snapshot af data
- Overskueligt dashboard med nøgletal

---

Arkitektur

BellaHair er bygget med en lagdelt arkitektur, som sikrer klar ansvarsfordeling og høj vedligeholdbarhed.

Arkitekturen består af følgende lag:

- UI 
  Brugergrænseflade udviklet i Blazor Server.  
  Ansvarlig for præsentation, navigation og brugerinteraktion.

- Application
  Indeholder applikationsservices og use cases.  
  Koordinerer forretningslogik og dataadgang.

- Domain  
  Indeholder domænemodellen med entiteter, value objects og domæneservices.  
  Dette lag er uafhængigt af tekniske detaljer.

- Infrastructure  
  Ansvarlig for databaseadgang og eksterne afhængigheder.  
  Implementeret med Entity Framework Core og SQL Server.

---

Domænemodel

Centrale domæneentiteter i systemet:

- Kunde  
- Booking  
- Medarbejder  
- Behandling  
- Faktura  

Fakturaer gemmes som historiske snapshots, hvilket betyder at relevante kunde-, booking- og prisdata kopieres ved oprettelse og ikke ændres efterfølgende.

---

Teknologier

- C#
- .NET
- Blazor Server
- Entity Framework Core
- SQL Server

---

Kørsel af projektet

1. Clone repository
2. Åbn løsningen i Visual Studio
3. Kør database-migrationer
4. Start applikationen

---

Bemærkninger

Projektet er udviklet med fokus på:
- Separation of concerns
- Skalerbarhed
- Testbarhed
- Brugervenlighed

Systemet er egnet som grundlag for videreudvikling og udvidelse med nye funktioner.
