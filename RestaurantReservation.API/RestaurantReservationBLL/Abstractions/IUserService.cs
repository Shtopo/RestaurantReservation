using RestaurantReservation.API.Entities;

namespace RestaurantReservation.API.RestaurantReservationBLL.Abstractions
{
    public interface IUserService
    {
        Task<int> RegisterAsync(string userName, string email, string password);
        Task<string> GetTokenAsync(RestaurantReservation.API.DTOs.LoginRequest request);
        Task<User?> GetUserAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> RenameUserAsync(int userId, string newName);
        Task<bool> DeleteUserAsync(int userId);     
        Task<bool> PromoteToAdminAsync(int userId);
    }
}
