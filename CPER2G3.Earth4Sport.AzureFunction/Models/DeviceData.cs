using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CPER2G3.Earth4Sport.AzureFunction.Models
{
    public class DeviceData
    {
        [BsonId]
        [BsonElement("_id")]
        public string uuid { get; set; }
        [BsonElement("n_batch")]
        public int n_batch { get; set; }
        [BsonElement("data_batch")]
        public DateTime data_batch { get; set; }
    }
}
