using CPER2G3.Earth4Sport.Auth.Models;
using CPER2G3.Earth4Sport.Auth.Service;
using Microsoft.AspNetCore.Mvc;

namespace CPER2G3.Earth4Sport.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger) {
            _userService = userService;
            _logger = logger;
        }


        [HttpPost("RegisterUser")]
        //[ProducesResponseType(201, Type = typeof(User))]
        //[ProducesResponseType(204, Type = typeof(User))]
        public async Task<ObjectResult> Insert([FromBody] User user) {
            var pippo = await _userService.Register(user);
            return new ObjectResult(pippo);

        }

        [HttpPost("LoginUser")]
        //[ProducesResponseType(201, Type = typeof(User))]
        //[ProducesResponseType(204, Type = typeof(User))]
        public async Task<ObjectResult> Login([FromBody] User user) {
            Console.WriteLine(user.Username);
            Console.WriteLine(user.Password);
            var ciao = await _userService.Login(user.Username, user.Password);
            return new ObjectResult(ciao);

        }
    }
}