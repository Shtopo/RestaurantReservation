using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantReservation.API.DTOs;

namespace RestaurantReservation.API.RestaurantReservationBLL.Abstractions
{
    public interface ITableService
    {
        Task<int> CreateTableAsync(int number, int seats);
        Task<List<RestaurantReservation.API.Entities.Table>> GetAllTablesAsync(bool? isAvailable, bool includeBookings = false);

        Task<RestaurantReservation.API.Entities.Table?> GetTableByIdAsync(int id);

        Task<RestaurantReservation.API.Entities.Table?> ChangeTableSettingsAsync(int id, TableRequest request);

        Task<bool> DeleteTableAsync(int id);
    }
}