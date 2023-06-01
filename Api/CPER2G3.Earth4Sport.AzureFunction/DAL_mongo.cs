using CPER2G3.Earth4Sport.AzureFunction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPER2G3.Earth4Sport.AzureFunction {
    public class DAL_mongo : IDAL {
        private readonly string _connectionString;
        private MongoClient client;
        private IMongoDatabase dbProvisioning;
        private IMongoDatabase dbSessions;

        public DAL_mongo(IConfiguration conf) {
            _connectionString = conf.GetConnectionString("mongo");
            client = new MongoClient(_connectionString);
            dbProvisioning = client.GetDatabase("provisioning");
            dbSessions = client.GetDatabase("sessions");
        }

        #region Provisioning
        public async Task<ObjectResult> getClockById(string uuid) {
            var collection = dbProvisioning.GetCollection<BsonDocument>("devices");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", uuid);
            try {
                var document = collection.Find(filter).First();
                return new OkObjectResult(new DeviceData() {
                    uuid = document["_id"].AsString,
                    n_batch = document["n_batch"].AsInt32,
                    data_batch = DateTime.Parse(document["data_batch"].AsString)
                });
            }
            catch (Exception) {
                return new NotFoundObjectResult("L'id non esiste");
            }
        }
        #endregion

        #region ClockSessions
        public async Task<ObjectResult> getSessionsList(string clockUuid)
        {
            var AllSessionsData = dbSessions.GetCollection<SessionData>(clockUuid);
            if (AllSessionsData == null)
            {
                return new NotFoundObjectResult("Nessun dato.");
            }
            try
            {
                List<SessionSummary> res = new List<SessionSummary>();
                foreach (var session in AllSessionsData.AsQueryable().Select(d => d.SessionUUID).Distinct().ToList())
                {
                    var data = AllSessionsData.Find(s => s.SessionUUID == session).ToList();
                    res.Add(new SessionSummary(data, session));
                }
                return new ObjectResult(res);
            }
            catch (Exception)
            {
                return new ObjectResult("Errore");
            }
        }
        public async Task<ObjectResult> getSessionActivities(string sessionUuid, string clockUuid)
        {
            var collection = dbSessions.GetCollection<SessionData>(clockUuid);
            if (collection == null)
            {
                return new NotFoundObjectResult("L'orologio non esiste");
            }
            try
            {
                var sessionCollection = collection.Find<SessionData>(s => s.SessionUUID == sessionUuid).ToList();
                if (sessionCollection == null)
                {
                    return new NotFoundObjectResult("La sessione non esiste");
                }
                return new ObjectResult(sessionCollection);
            }
            catch (Exception)
            {
                return new ObjectResult("Errore");
            }
        }

        public async Task<ObjectResult> postClock(SessionData activity, string clockUuid) {
            var collection = dbSessions.GetCollection<SessionData>(clockUuid);
            if (collection == null) {
                dbProvisioning.CreateCollection(clockUuid);
                collection = dbSessions.GetCollection<SessionData>(clockUuid);
            }
            try {
                await collection.InsertOneAsync(activity);
                return new OkObjectResult("Inserimento avvenuto".ToJson());
            }
            catch (Exception) {
                return new BadRequestObjectResult("Errore");
            }
        }

        public async Task<ObjectResult> getAllClocksIds()
        {
            var filter = Builders<DeviceData>.Filter.Empty;
            var collection = dbProvisioning.GetCollection<DeviceData>("devices").Find(filter).ToList().Select(d => d.uuid);

            return new OkObjectResult(collection);
        }

        public async Task<ObjectResult> getAllClocks()
        {
            var filter = Builders<DeviceData>.Filter.Empty;
            var collection = dbProvisioning.GetCollection<DeviceData>("devices").Find(filter).ToList();

            return new OkObjectResult(collection);
        }
        #endregion
    }
}
