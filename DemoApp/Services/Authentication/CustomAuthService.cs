using System.Security.Claims;

namespace DemoApp.Services.Authentication
{
    public class CustomAuthService
    {
        public Dictionary<String, ClaimsPrincipal> Users { get; set; }

        public CustomAuthService()
        {
            Users = new Dictionary<String, ClaimsPrincipal>();
        }
        
    }
}
