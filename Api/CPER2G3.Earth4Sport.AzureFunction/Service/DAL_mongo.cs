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
using System.Xml.Serialization;

namespace CPER2G3.Earth4Sport.AzureFunction.Service
{
    public class DAL_mongo : IDAL
    {
        private readonly string _connectionString;
        private MongoClient client;
        private IMongoDatabase dbProvisioning;
        private IMongoDatabase dbSessions;

        public DAL_mongo(IConfiguration conf)
        {
            _connectionString = conf.GetConnectionString("mongo");
            client = new MongoClient(_connectionString);
            dbProvisioning = client.GetDatabase("provisioning");
            dbSessions = client.GetDatabase("sessions");
        }

        #region ClockSessions
        public async Task<ObjectResult> getSessionsList(string clockUuid)
        {
            var AllSessionsData = dbSessions.GetCollection<ActivityData>(clockUuid);
            if (AllSessionsData == null)
            {
                return new NotFoundObjectResult("Nessun dato.");
            }
            try
            {
                List<SessionSummary> res = new List<SessionSummary>();
                var sessionList = AllSessionsData.AsQueryable().Select(d => d.SessionUUID).Distinct().ToList();
                foreach (var session in sessionList)
                {
                    var data = AllSessionsData.Find(s => s.SessionUUID == session).ToList();
                    res.Add(new SessionSummary(data, session));
                }
                res.OrderByDescending(s => s.Start);
                return new ObjectResult(res);
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message);
            }
        }
        public async Task<ObjectResult> getSessionActivities(string sessionUuid, string clockUuid)
        {
            var collection = dbSessions.GetCollection<ActivityData>(clockUuid);
            if (collection == null)
            {
                return new NotFoundObjectResult("L'orologio non esiste");
            }
            try
            {
                var sessionCollection = collection.Find(s => s.SessionUUID == sessionUuid).ToList();

                sessionCollection.OrderByDescending(s => s.TimeStamp);
                if (sessionCollection == null)
                {
                    return new NotFoundObjectResult("La sessione non esiste");
                }
                return new ObjectResult(sessionCollection);
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message);
            }
        }

        public async Task<ObjectResult> postClock(ActivityData activity, string clockUuid)
        {
            var collection = dbSessions.GetCollection<ActivityData>(clockUuid);
            if (collection == null)
            {
                dbProvisioning.CreateCollection(clockUuid);
                collection = dbSessions.GetCollection<ActivityData>(clockUuid);
            }
            try
            {
                await collection.InsertOneAsync(activity);
                return new OkObjectResult("Inserimento avvenuto".ToJson());
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Errore");
            }
        }

        #endregion
    }
}
