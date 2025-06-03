using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using RestaurantReservation.API.RestaurantReservationBLL.Services;
using RestaurantReservation.API.DTOs;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("Users")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public UserController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var userId = await _userService.CreateUserAsync(request.UserName, request.Email, request.Password);
            return Ok(userId);
        }


        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] int userId)
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return users.Any() ? Ok(users) : NotFound();
        }

        [HttpPost("RenameUser")]
        public async Task<IActionResult> RenameUser([FromQuery] int userId, [FromQuery] string newName)
        {
            var user = await _userService.RenameUserAsync(userId, newName);

            return user is not null ? Ok(user) : NotFound("Користувача не знайдено");
        }

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

    }
}