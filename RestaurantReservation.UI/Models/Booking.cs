using System;

namespace RestaurantReservation.UI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public int TableId { get; set; }
        public Table? Table { get; set; }
    }
}
