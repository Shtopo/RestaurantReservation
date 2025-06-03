using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestaurantReservation.UI.Models;


namespace RestaurantReservation.UI.Services

{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Table>> GetTablesAsync(bool? isAvailable)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Table>>($"api/tables?isAvailable={isAvailable}");
            return response ?? new List<Table>();
        }
    }
}


