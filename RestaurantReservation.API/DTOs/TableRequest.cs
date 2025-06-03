using RestaurantReservation.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.API.DTOs
{
    public class TableRequest
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public int Seats { get; set; }
        public bool IsAvailable { get; set; }
    }
}
