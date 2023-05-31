using CPER2G3.Earth4Sport.AzureFunction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
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
        public async Task<ObjectResult> getSessionActivities(string sessionUuid, string clockUuid) {


            var collection = dbSessions.GetCollection<SessionData>(clockUuid);
            if (collection == null) {
                return new NotFoundObjectResult("L'orologio non esiste");
            }
            try {
                var sessionCollection = collection.Find<SessionData>(s => s.SessionUUID == sessionUuid);
                if(sessionCollection == null) {
                    return new NotFoundObjectResult("La sessione non esiste");
                }
                return new ObjectResult(sessionCollection);
            }
            catch (Exception) {
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
        #endregion
    }
}
