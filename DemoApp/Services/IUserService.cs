using DemoApp.Data;
using DemoApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace DemoApp.Services
{
    public interface IUserService
    {
        Task<UserAccount?> ValidateUser(UserLogin login);

        /*private String HashPassword(UserLogin loginRequest)
        {
            //create sha algorithm
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.password)));

        }*/


        Task<UserAccount?> getUserById(string id);

        Task<bool> checkIfUserExists(String username);

        Task saveUser(UserAccount login);
    }
}
