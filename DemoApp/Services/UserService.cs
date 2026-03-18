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
        private readonly ILogger<UserService> _logger;


        public UserService(DemoAppLoginContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<UserAccount?> getUserById(String id)
        {

            try
            {
                return await _db.UserLogins.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured when attempting to use the database to lookup a user with the ID of {id}", id);
                throw;
            }
        }

        private async Task<UserAccount?> getUserByName(String username)
        {
            try
            {
                return await _db.UserLogins.FirstOrDefaultAsync(r => r.username == username);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured when attempting to use the database to lookup a user with the name of {username}", username);
                throw;
            }
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
            try
            {
                _db.UserLogins.Add(login);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured when attempting to use the database to save a new user with the name of {username}", login.username);
                throw;
            }
        }

        public async Task<UserAccount?> ValidateUser(UserLogin loginRequest)
        {

            //String hash = HashPassword(loginRequest.password);
            //loginRequest.password = hash;

            //this will be null if username does not exist
            UserAccount? loginUser = await getUserByName(loginRequest.username);
            bool verifyResult = false;
            if (loginUser != null)
                verifyResult = verifyPassword(loginRequest.password, loginUser.passwordHash);

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
