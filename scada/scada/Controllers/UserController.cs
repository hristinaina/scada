using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services;

namespace scada.Controllers
{
    [ApiController]
    [Route("api/user/")]
    public class UserController : ControllerBase 
    {

        [HttpPost("login")]
        public IActionResult login([FromBody] User user)
        {
            Console.WriteLine("usaooooo");
            UserService userService = new UserService();
            bool isLogged = userService.login(user.Email, user.Password);
            if (isLogged) return Ok(new { Message = "Login successful!" });
            else return Unauthorized(new { Message = "Email or password is not correct!" });
        }
    }
}
