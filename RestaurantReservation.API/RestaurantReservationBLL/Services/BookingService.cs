using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using System.Linq;
using System.Text.Json;

namespace RestaurantReservation.API.RestaurantReservationBLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> PutBookingAsync(string customerName, DateTime bookingTime, int tableId, int? userId)
        {
            var booking = new Booking
            {
                CustomerName = customerName,
                BookingTime = bookingTime,
                TableId = tableId,
                UserId = userId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }
        public async Task<List<Booking>> GetBookingsAsync(string? customerName)
        {
            var query = _context.Bookings
                .Include(b => b.Table)
                .AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(b => b.CustomerName.Contains(customerName));
            }

            return await query.ToListAsync(); 
        }

        public async Task<Booking?> GetBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return null;

            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId, int userId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.UserId != userId)
            {
                return false;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}