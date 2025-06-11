using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using System.Security.Cryptography;


namespace RestaurantReservation.API.RestaurantReservationBLL.Services
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailService _emailService;
        public UserService(AppDbContext context, ITokenProvider tokenProvider, IEmailService emailService)
        {
            _context = context;
            _tokenProvider = tokenProvider;
            _emailService = emailService;
        }

        public async Task<int> RegisterAsync(string userName, string email, string password)
        {
            var user = new User
            {
                Name = userName,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = "User"
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            Console.WriteLine($"SaveChangesAsync result: {result}");
            return user.Id;
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> RenameUserAsync(int userId, string newName)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return null;
            }

            user.Name = newName;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetTokenAsync(RestaurantReservation.API.DTOs.LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Невірний логін або пароль.");
            }

            var token = _tokenProvider.GenerateToken(user.Id, user.Role);
            return token;
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return true;
            }

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            var resetLink = $"https://shtopo.github.io/index.html/reset-password.html?token={token}";

            var emailBody = $"Для скидання пароля, будь ласка, перейдіть за посиланням: <a href='{resetLink}'>{resetLink}</a>";
            await _emailService.SendEmailAsync(user.Email, "Скидання паролю", emailBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == token && u.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();
            return true;
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<bool> PromoteToAdminAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Role = "Admin";
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
