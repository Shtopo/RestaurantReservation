using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using RestaurantReservation.API.RestaurantReservationBLL.Services;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TableController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITableService _tableService;
        public TableController(AppDbContext context, ITableService tableService)
        {
            _context = context;
            _tableService = tableService;
        }

        [HttpPost]
        public async Task <IActionResult> CreateTable([FromBody] TableRequest request)
        {
            var existingTable = await _context.Tables.FirstOrDefaultAsync(t => t.Number == request.Number);

            if (existingTable != null)
                return BadRequest("Столик з таким номером вже існує!");

            var tableId = await _tableService.CreateTableAsync(request.Number, request.Seats);

            return Ok(new {TableId = tableId});
        }

        [HttpGet ("AllTables")]
        public async Task<IActionResult> GetAllTables([FromQuery] bool? isAvailable, [FromQuery] bool includeBookings = false)
        {
            var tables = await _tableService.GetAllTablesAsync(isAvailable, includeBookings);

            if (!tables.Any())
                return NotFound("Столики не знайдені");

            return Ok(tables);
        }

        [HttpGet]
        public async Task<IActionResult> GetTableById([FromQuery] int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);

            if (table == null)
                return NotFound("Столик не знайдено");

            return Ok(table);
        }

        [HttpPut("updateTable")]
        public async Task<IActionResult> ChangeTableSettings([FromQuery] int id, [FromBody] TableRequest request)
        {
            var updateTable = await _tableService.ChangeTableSettingsAsync(id, request);

            if (updateTable == null)
                return NotFound("Столик не знайдено!");

            return Ok(updateTable);
        }

        // Видалити бронювання
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var del = await _tableService.DeleteTableAsync(id);
            if (!del)
                return NotFound("Не вдалось видалити столик!");

            return Ok("Столик видалено!");
        }
    }
}

