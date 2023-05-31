using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CPER2G3.Earth4Sport.AzureFunction.Models {

    public class SessionData {
        [BsonId]
        public ObjectId _id {  get; set; }
        [BsonElement("SessionUUID")]
        public string SessionUUID { get; set; }
        [BsonElement("Pools")]
        public int Pools { get; set; }
        [BsonElement("Distance")]
        public double Distance { get; set; }
        [BsonElement("Bpm")]
        public int Bpm { get; set; }
        [BsonElement("Gps")]
        public Gps Gps { get; set; }
        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }
    }

    public class Gps {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
