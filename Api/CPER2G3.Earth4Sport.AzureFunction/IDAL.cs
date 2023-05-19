using CPER2G3.Earth4Sport.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CPER2G3.Earth4Sport.AzureFunction.Models;

namespace CPER2G3.Earth4Sport.AzureFunction {
    public interface IDAL {
        public Task<ObjectResult> getClockById(string uuid);
        public Task<ObjectResult> getSessionActivities(string uuid);
        public Task<ObjectResult> postClock(ClockActivityData activity);
    }
}