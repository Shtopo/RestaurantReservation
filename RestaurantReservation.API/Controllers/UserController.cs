﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using RestaurantReservation.API.RestaurantReservationBLL.Services;
using System.Security.Claims;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public UserController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            var userId = await _userService.RegisterAsync(request.UserName, request.Email, request.Password);

            var token = await _userService.GetTokenAsync(
                new LoginRequest
                {
                    Email = request.Email,
                    Password = request.Password
                }
            );

            return Ok(new { token, userId });
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Не вдалося визначити користувача з токена");

            var user = await _userService.GetUserAsync(int.Parse(userId));
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return users.Any() ? Ok(users) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("RenameUser")]
        public async Task<IActionResult> RenameUser([FromQuery] int userId, [FromQuery] string newName)
        {
            var user = await _userService.RenameUserAsync(userId, newName);

            return user is not null ? Ok(user) : NotFound("Користувача не знайдено");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] int userId)
        {
            var deleted = await _userService.DeleteUserAsync(userId);
            return deleted ? Ok() : NotFound("Користувач не знайдений");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("PromoteToAdmin")]
        public async Task<IActionResult> PromoteToAdmin([FromQuery] int userId)
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null) return NotFound("Користувач не знайдений");

            user.Role = "Admin";
            await _context.SaveChangesAsync();

            return Ok("Користувач став адміністратором");
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _userService.RequestPasswordResetAsync(request.Email);

            return Ok(new { message = "Якщо обліковий запис із таким email існує, на нього було надіслано посилання для скидання пароля." });
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("Паролі не збігаються.");
            }

            var result = await _userService.ResetPasswordAsync(request.Token, request.NewPassword);

            if (result)
            {
                return Ok(new { message = "Пароль успішно скинуто." });
            }

            return BadRequest("Недійсний або прострочений токен.");
        }

    }
}