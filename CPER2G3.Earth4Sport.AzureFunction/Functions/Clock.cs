using AzureFunctions.Extensions.Swashbuckle.Attribute;
using CPER2G3.Earth4Sport.AzureFunction.JwtUtils;
using CPER2G3.Earth4Sport.AzureFunction.Models;
using CPER2G3.Earth4Sport.AzureFunction.Service;
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
        private IUserService _userService { get; set; }
        public Clock(IDAL dal, IUserService userService) {
            _dal = dal;
            _userService = userService;
        }

        [FunctionName("device_data")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> PostClockActivity(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "device_data/{clock_id}")]
            HttpRequest req,
            string clock_id,
            ILogger log
            ) {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body)) {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            ActivityData clockData = new ActivityData() {
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

        [FunctionName("sessions_list")]
        [ProducesResponseType(typeof(List<SessionSummary>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetSessionsList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="sessions_list/{clock_id}")]
            HttpRequest req,
            ILogger log,
            string clock_id
            ) {
            req.Headers.TryGetValue("Bearer", out var token);
            var isAuth = JwtMethods.ValidateCurrentToken(token);
            if (!isAuth) {
                return new UnauthorizedObjectResult("Non sei autenticato!");
            }
            return await _dal.getSessionsList(clock_id);
        }
        [FunctionName("session_data")]
        [ProducesResponseType(typeof(List<ActivityData>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetClockSession(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="session_data/{clock_id}/{session_id}" )]
            HttpRequest req,
            ILogger log,
            string clock_id,
            string session_id
            ) {
            req.Headers.TryGetValue("Bearer", out var token);
            var isAuth = JwtMethods.ValidateCurrentToken(token);
            if (!isAuth) {
                return new UnauthorizedObjectResult("Non sei autenticato!");
            }
            return await _dal.getSessionActivities(session_id, clock_id);
        }

        [FunctionName("user_clocks")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserClocks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user_clocks" )]
            HttpRequest req,
            ILogger log
            ) {
            req.Headers.TryGetValue("Bearer", out var token);
            var isAuth = JwtMethods.ValidateCurrentToken(token);
            if (!isAuth) {
                return new UnauthorizedObjectResult("Non sei autenticato!");
            }
            string user_id = JwtMethods.GetTokenUserId(token);
            var clocks = await _userService.UserClocks(user_id);
            if(clocks == null ) {
                return new NotFoundObjectResult("Nessun orologio trovato!");
            }
            return new OkObjectResult(clocks);
        }
    }
}
