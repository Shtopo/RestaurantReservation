using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.API.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        public string Role { get; set; } = "User";

        public string PasswordHash { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }
    }
}
