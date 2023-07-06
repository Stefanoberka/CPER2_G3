Con docker-compose:
```yaml
# Impostare username e password a scelta, devono essere uguali in mongo e mongo-express per poter usare l'interfaccia web
# ME_CONFIG_MONGODB_URL ha il formato mongodb://<username>:<password>@mongo:<porta> ed è anche la connection string
version: '3.1'

services:

  mongo:
    image: mongo
    restart: always
    ports:
      - 27017:27017
    environment:
      MONGO_INITDB_ROOT_USERNAME: cper2g3
      MONGO_INITDB_ROOT_PASSWORD: 8z5!H7jAcA!C

  mongo-express:
    image: mongo-express
    restart: always
    ports:
      - 8081:8081
    environment:
      ME_CONFIG_MONGODB_ADMINUSERNAME: cper2g3
      ME_CONFIG_MONGODB_ADMINPASSWORD: 8z5!H7jAcA!C
      ME_CONFIG_MONGODB_URL: mongodb://cper2g3:8z5!H7jAcA!C@mongo:27017/
```

Dopodiché dall'interfaccia di [mongo-express](http://localhost:8081/) va creato un db di nome `provisioning` e dentro a questo db una collection di nome `devices`.  
In `devices` -> `new document` -> incolli il file JSON generato con `Provisioning/clockMaker.js` 