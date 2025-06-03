using RestaurantReservation.API.Entities;

namespace RestaurantReservation.API.RestaurantReservationBLL.Abstractions
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(string userName, string email, string password);
        Task<User?> GetUserAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> RenameUserAsync(int userId, string newName);
        Task<bool> DeleteUserAsync(int userId);
        Task<string> GetTokenAsync(RestaurantReservation.API.DTOs.LoginRequest request);
        Task<bool> PromoteToAdminAsync(int userId);
    }
}
