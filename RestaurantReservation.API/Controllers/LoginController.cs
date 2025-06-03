using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using RestaurantReservation.API.RestaurantReservationBLL.Services;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] RestaurantReservation.API.DTOs.LoginRequest request)
        {
            try
            {
                var token = await _userService.GetTokenAsync(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }

}

