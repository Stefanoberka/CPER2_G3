# CPER2_G3
## Orologion
## Project work, richieste:
- API Rest per acquisizione dati (C#/fastify) da smartwatch e salvataggio su db (Qualcosa)
  - da gestire nel cloud (Azure)  
- Applicazione web per la visualizzazione dei dati del proprio orologio (React/Blazor)  
  - da gestire nel cloud  
- Applicazione web per il test dei dispositivi (usata dall’azienda produttrice), come da specifiche del documento d’esame (React/Blazor)  
  - da gestire on premise  
- i dati dei lotti di produzione non devono essere inviati nel cloud, ma rimanere solo all'interno dell'azienda  
- gli ultimi dati degli orologi o le anomalie devono venir recuperate dal cloud  

### Descrizione del Sistema

Assumiamo che, al momento della fabbricazione di ogni batch di orologi, il loro GUID e quello del loro batch venga inserito nel db on prem.

#### Orologio
	- GUID
	- Ogni 10 sec all API
		- GUID
 		- Conteggio Vasche
		- Battito
		- Posizione
	- (Correttezza dati)
	- Batch di produzione
	
	
#### API Rest
	- Post su db
	- Get da data store
	- Acquisizione dati
	Le API riceveranno i dati dal simulatore ogni 10 secondi su una richiesta POST e ne scriveranno i contenuti su db.
	Le API manderanno i dati al front-end tramite richieste get per estrarre dal db i dati di una sezione a scelta, oppure di un orologio.
	
#### Data Store
	- Memorizzazione

#### Portale Web esterno
	- Autenticazione
	- Dati singolo orologio

#### Portale Web interno
	- Dati tutti gli orologi 
	- Dati in locale
	- Anomalie dei dati

#### Simulatore orologio con invio ad API => db [mongoDB]

#### App in react esterna su Azure

#### App in react interna in locale

#### API on prem con fastify
	- Dati batch orologi

#### Data DB [MongoDv]
Tabella attività
- guid sessione
- lista di attività {
- guid attività
- conteggio vasche
- battiti
- posizione (latitudine/longitudine)
- timestamp
- distanza percorsa
}

### db autenticazione [SQL Server]

Tabella dati orologio
- guid orologio
- userid

Tabella dati utente
- userid
- username
- password

### db[azienda]
Tabella orologio
- guid orologio
- batch id
- userid
