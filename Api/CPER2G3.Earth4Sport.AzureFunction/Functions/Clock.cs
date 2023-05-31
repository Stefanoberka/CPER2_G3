using AzureFunctions.Extensions.Swashbuckle.Attribute;
using CPER2G3.Earth4Sport.AzureFunction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CPER2G3.Earth4Sport.AzureFunction.Functions
{
    public class Clock
    {
        private IDAL _dal { get; set; }
        public Clock(IDAL dal) {
            _dal = dal;
        }

        [FunctionName("get_device_data")]
        [ProducesResponseType(typeof(DeviceData), (int)HttpStatusCode.OK)]
        [QueryStringParameter("uuid", "", DataType = typeof(string))]
        public async Task<IActionResult> GetClockData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequest req,
            ILogger log
            ){
            log.LogInformation("C# HTTP trigger function processed a request.");
            string uuid = req.Query["uuid"];
            return await _dal.getClockById(uuid);
        }

        [FunctionName("post_device_data")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostClockActivity(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "post_device_data/{clock_id}")]
            HttpRequest req,
            string clock_id,
            ILogger log
            ) {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body)) {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            Console.WriteLine(data.timestamp);
            SessionData clockData = new SessionData() {
                SessionUUID = data.sessionUUID,
                Bpm = data.bpm,
                Distance = data.distance,
                Pools = data.pools,
                Gps = new Gps() {
                    latitude = data.gps.latitude,
                    longitude = data.gps.longitude,
                },
                TimeStamp = data.timestamp
            };
            //string c_id = req.Query["clock_id"];
            return await _dal.postClock(clockData, clock_id);
        }

        [FunctionName("get_sessions_list")]
        public async Task<IActionResult> GetSessionsList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="get_sessions_list/{clock_id}")]
            HttpRequest req,
            ILogger log,
            string clock_id
            )
        {
            return await _dal.getSessionsList(clock_id);
        }
        [FunctionName("get_session_data")]
        [ProducesResponseType(typeof(List<SessionData>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClockSession(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="get_session_data/{clock_id}/{session_id}" )]
            HttpRequest req,
            ILogger log,
            string clock_id,
            string session_id
            )
        {
            return await _dal.getSessionActivities(session_id, clock_id);
        }
    }
}
