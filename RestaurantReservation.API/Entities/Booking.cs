using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservation.API.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Ім'я клієнта не може перевищувати 100 символів.")]
        public required string CustomerName { get; set; }

        public DateTime? BookingTime { get; set; }

        [Required]
        public int TableId { get; set; }

        public Table? Table { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
