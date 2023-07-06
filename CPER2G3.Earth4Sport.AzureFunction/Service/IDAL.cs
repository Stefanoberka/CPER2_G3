using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CPER2G3.Earth4Sport.AzureFunction.Models;

namespace CPER2G3.Earth4Sport.AzureFunction.Service
{
    public interface IDAL
    {
        public Task<ObjectResult> getSessionActivities(string sessionUuid, string clockUuid);
        public Task<ObjectResult> postClock(ActivityData activity, string clockUuid);
        public Task<ObjectResult> getSessionsList(string clockUuid);
    }
}