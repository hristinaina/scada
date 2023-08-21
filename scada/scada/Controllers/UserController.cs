using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services;

namespace scada.Controllers
{
    [ApiController]
    [Route("api/user/")]
    public class UserController : ControllerBase 
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] User user)
        {
            Console.WriteLine("Poslalo se");
            bool isLogged = _userService.Login(user.Email, user.Password);
            if (isLogged) return Ok(new { Message = "Login successful!" });
            else return Unauthorized(new { Message = "Email or password is not correct!" });
        }
    }
}
