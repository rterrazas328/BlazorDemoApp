using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace DemoApp.Controllers
{
    /*Handles authentication logic
        includes checking for
            - input validation
            - password hash validation (in ValidateUser())
            - cookie session storage

    */
    [IgnoreAntiforgeryToken]
    [Route("api/auth")]
    [ApiController]
    public class LoginAuthenticationController : ControllerBase
    {

        private readonly IUserService _users;

        public LoginAuthenticationController(IUserService users)
        {
            _users = users;

        }

        [IgnoreAntiforgeryToken]
        [HttpPost("login")]
        //public async Task<IActionResult> Login(UserLogin login)
        public async Task<ActionResult> Login([FromForm] string username, [FromForm] string password, [FromForm] string? returnUrl)
        {
            var loginRequest = new UserLogin { username = username, password = password };

            UserAccount? loginUser = await _users.ValidateUser(loginRequest);

            if (loginUser == null)
            {
                return Unauthorized("Username or Password is invalid");
            }
            else
            {
                //successfully validated
                loginRequest.role = loginUser.role;

                //cookie stuff
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginRequest.username),
                    new Claim(ClaimTypes.Role, loginRequest.role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });

                return Redirect(returnUrl ?? "/");
            }

        }



        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromForm] string? returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(returnUrl ?? "/login");
        }

    }
}
