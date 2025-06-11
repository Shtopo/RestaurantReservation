using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using RestaurantReservation.API.RestaurantReservationBLL.Services;
using System.Security.Claims;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBookingService _bookingService;

        public BookingController(AppDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        // Забронювати столик      
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PutBooking([FromBody] BookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                    });

                return BadRequest(errors);
            }

            if (!request.BookingTime.HasValue)
                return BadRequest("Дата бронювання обов'язкова!");

            // Перевіряємо чи є зареестрований користувач
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;

            var bookingId = await _bookingService.PutBookingAsync(request.CustomerName, request.BookingTime.Value, request.TableId, userId);
            if (bookingId == 0)
                return BadRequest("Не вдалось забронювати!");

            return Ok(new { BookingId = bookingId });
        }


        // Отримати всі бронювання
        [HttpGet ("AllBookings")]
        public async Task<IActionResult> GetBookings([FromQuery] string? customerName)
        {
            var bookings = await _bookingService.GetBookingsAsync(customerName);

            return Ok(bookings);
        }

        // Отримати бронювання за ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _bookingService.GetBookingAsync(id);

            if (booking == null)
                return NotFound("Бронювання не знайдено!");

            return Ok(booking);
        }

        // Видалити бронювання
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Вам потрібно увійти в систему!");

            var userId = int.Parse(userIdClaim);

            var del = await _bookingService.DeleteBookingAsync(id, userId);
            if (!del)
                return NotFound("Не вдалось видалити бронювання!");

            return Ok("Бронювання видалено!");
        }

    }
}
