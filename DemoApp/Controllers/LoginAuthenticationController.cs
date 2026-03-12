using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DemoApp.Controllers
{
    /*Handles authentication logic
        includes checking for
            - input validation
            - hashing
            - updating the model to store a login

    */

    [Route("api/auth")]
    [ApiController]
    public class LoginAuthenticationController : ControllerBase
    {

        private readonly IUserService _users;

        public LoginAuthenticationController(IUserService users)
        {
            _users = users;

        }

        [HttpPost("login")]
        //public async Task<IActionResult> Login(UserLogin login)
        public async Task<ActionResult<IEnumerable<UserLogin>>> Login(UserLogin login)
        {
            var user_response = await _users.ValidateUser(login);
            if (user_response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user_response);
            }
        }
    }
}
