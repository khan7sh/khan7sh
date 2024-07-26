using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicVendorInventoryPlatform.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly LoggingService _loggingService;

        public AuthService(AppDbContext context, LoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        public async Task<bool> CheckUserExists(string username)
        {
            _loggingService.LogInfo($"Checking if user exists: {username}");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            bool exists = user != null;
            _loggingService.LogInfo($"User '{username}' exists: {exists}");
            return exists;
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            _loggingService.LogInfo($"Attempting to authenticate user: {username}");

            try
            {
                // this one checks if user exists first
                bool userExists = await CheckUserExists(username);
                if (!userExists)
                {
                    _loggingService.LogWarning($"Authentication failed. User does not exist: {username}");
                    return null;
                }

                _loggingService.LogInfo($"Looking up user: {username}");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

                if (user == null)
                {
                    _loggingService.LogWarning($"User not found: {username}");
                    return null;
                }

                _loggingService.LogInfo($"User found: {username}. Verifying password.");

                bool passwordVerified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                _loggingService.LogInfo($"Password verification result for user {username}: {(passwordVerified ? "Success" : "Failure")}");

                if (passwordVerified)
                {
                    _loggingService.LogInfo($"Authentication successful for user: {username}");
                    return user;
                }

                _loggingService.LogWarning($"Authentication failed for user: {username}");
                return null;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error during authentication: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            _loggingService.LogInfo("Retrieving all users");
            return await _context.Users.ToListAsync();
        }
    }
}