using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CPER2G3.Earth4Sport.AzureFunction.Models;
using MongoDB.Driver;
using CPER2G3.Earth4Sport.AzureFunction.Service;

namespace CPER2G3.Earth4Sport.AzureFunction.Functions
{
    public class Auth
    {
        private IUserService _userService { get; set; }
        public Auth(IUserService userService) {
            _userService = userService;
        }
        [FunctionName("register")]
        public async Task<IActionResult> Insert(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body)) {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            User user = new User() {
                ClockUuid = data.uuid,
                Username = data.username,
                Password = data.password,
            };
            var pippo = await _userService.Register(user);
            return new OkObjectResult(pippo);
        }

        [FunctionName("login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log) {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body)) {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            User user = new User() {
                ClockUuid = data.uuid,
                Username = data.username,
                Password = data.password,
            };
            var pippo = await _userService.Login(user.Username, user.Password);
            return new OkObjectResult(pippo);
        }
    }
}
