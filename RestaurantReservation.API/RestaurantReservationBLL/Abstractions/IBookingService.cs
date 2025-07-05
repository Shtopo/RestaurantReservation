using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Entities;

namespace RestaurantReservation.API.RestaurantReservationBLL.Abstractions
{
    public interface IBookingService
    {
        Task<int> PutBookingAsync(string customerName, DateTime bookingTime, int tableId, int? userId);
        Task<List<Booking>> GetBookingsAsync();
        Task<Booking?> GetBookingAsync(int id);
        Task<bool> DeleteBookingAsync(int bookingId, int userId);
    }
}
