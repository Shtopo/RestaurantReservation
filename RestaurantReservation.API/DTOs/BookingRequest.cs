using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.API.DTOs
{
    public class BookingRequest
    {
        [Required]
        public required string CustomerName { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Дата й час обов’язкові")]
        public DateTime? BookingTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TableId повинен бути більше 0")]
        public int TableId { get; set; }
    }
}
