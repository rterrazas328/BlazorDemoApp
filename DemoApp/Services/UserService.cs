using BCrypt.Net;
using DemoApp.Data;
using DemoApp.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace DemoApp.Services
{
    public class UserService : IUserService
    {
        private readonly DemoAppLoginContext _db;

        public UserService(DemoAppLoginContext db)
        {
            _db = db;
        }

        public async Task<UserAccount?> getUserById(String id)
        {
            return await _db.UserLogins.FindAsync(id);
        }

        private async Task<UserAccount?> getUserByName(String username)
        {
            return await _db.UserLogins.FirstOrDefaultAsync(r => r.username == username);
        }

        public async Task<bool> checkIfUserExists(String username)
        {
            UserAccount? user = await getUserByName(username);

            if (user == null) {
                return false;
            }

            return true;
        }

        public async Task saveUser(UserAccount login)
        {
            String hash = HashPassword(login.passwordHash);
            login.passwordHash = hash;
            _db.UserLogins.Add(login);
            await _db.SaveChangesAsync();
        }

        public async Task<UserAccount?> ValidateUser(UserLogin loginRequest)
        {

            //String hash = HashPassword(loginRequest.password);
            //loginRequest.password = hash;

            UserAccount? loginUser = await getUserByName(loginRequest.username);

            bool verifyResult = verifyPassword(loginRequest.password, loginUser.passwordHash);

            if (loginUser != null && verifyResult)
            {
                return loginUser;
            }

            return null;

        }


        private String HashPassword(String loginRequestPass)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(loginRequestPass, 12);
        }

        private bool verifyPassword(String password, String hashedPassword)
        {
            //return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }

    }
}
