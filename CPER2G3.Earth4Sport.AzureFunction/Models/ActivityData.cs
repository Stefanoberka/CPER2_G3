using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPER2G3.Earth4Sport.AzureFunction.Models {
    public class SessionSummary
    {
        public string Id { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public double TotalDistance { get; set; }
        public int TotalPools { get; set; }
        public double AvgBpm { get; set; }
        public SessionSummary(List<ActivityData> data, string SessionId)
        {
            this.Id = SessionId;
            this.Start = data.Select(d => d.TimeStamp).Min();
            this.End = data.Select(d => d.TimeStamp).Max();
            this.TotalDistance = data.Select(d => d.Distance).Max();
            this.TotalPools = data.Select(d => d.Pools).Max();
            this.AvgBpm = data.Select(d => d.Bpm).Average();
        }
    }

    public class ActivityData {
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
