using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.API.Entities
{
    public class Table
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Номер стола повинен будет більше 0.")]
        public int Number { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Кількість місць повинна будет більше 0.")]
        public int Seats { get; set; }
        public bool IsAvailable { get; set; } = true;

        [JsonIgnore]
        public virtual List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
