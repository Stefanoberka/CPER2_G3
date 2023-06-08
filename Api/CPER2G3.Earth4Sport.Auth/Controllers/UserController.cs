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


        [HttpPost(Name = "RegisterUser")]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(204, Type = typeof(User))]
        public IActionResult Insert([FromBody] User user) {
            Console.WriteLine(user.Username);
            Console.WriteLine(user.Password);
            _userService.Register(user);
            return NoContent(); // 204

        }
    }
}