using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson;
using System;

namespace CPER2G3.Earth4Sport.AzureFunction.Models {
    public class ClockActivityData {
        public ObjectId _id {  get; set; }
        public string SessionUUID { get; set; }
        public int Pools { get; set; }
        public double Distance { get; set; }
        public int Bpm { get; set; }
        public Gps Gps { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class Gps {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
