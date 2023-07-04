## Scripts per provisioning

Raccolta di scripts per generare qualche dato di partenza.

Argomenti:

 - `clockMaker`: genera una lista di 50 id con rispettivo batch e data di produzione
 - `userMaker`: associa ad ogni dispositivo generato con `clockMaker` un utente con tanto di password
 - `userPusher`: registra gli utenti generati con `userMaker` tramite la API

Utilizzo:  
```bash
node index.js <nome script, senza ".js">
```
