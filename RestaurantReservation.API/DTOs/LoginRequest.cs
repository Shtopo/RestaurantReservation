using System.Text.Json.Serialization;

namespace RestaurantReservation.API.DTOs
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}


