using System;

namespace CPER2G3.Earth4Sport.AzureFunction.Models
{
    public class ClockData
    {
        public string uuid { get; set; }
        public int n_batch { get; set; }
        public DateTime data_batch { get; set; }
    }
}
