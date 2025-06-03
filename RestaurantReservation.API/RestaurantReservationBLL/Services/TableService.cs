using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantReservation.API.Data;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;

namespace RestaurantReservation.API.RestaurantReservationBLL.Services
{
    public class TableService : ITableService
    {
        private readonly AppDbContext _context;
        public TableService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateTableAsync(int number, int seats)
        {
            var table = new RestaurantReservation.API.Entities.Table
            {
                Number = number,
                Seats = seats,
                IsAvailable = true
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table.Id;
        }

        public async Task<List<RestaurantReservation.API.Entities.Table>> GetAllTablesAsync(bool? isAvailable,  bool includeBookings = false)
        {
            IQueryable<RestaurantReservation.API.Entities.Table> query = _context.Tables;

            if (includeBookings)
            {
                query = query.Include(t => t.Bookings);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(t => t.IsAvailable == isAvailable.Value);
            }
                 
            return await query.ToListAsync();
        }

        public async Task<RestaurantReservation.API.Entities.Table?> GetTableByIdAsync([FromQuery] int id)
        {
            var table = await _context.Tables
           .Include(t => t.Bookings)
           .FirstOrDefaultAsync(t => t.Id == id);

            if (table == null)
                return null;

            return table;
        }

        public async Task<RestaurantReservation.API.Entities.Table?> ChangeTableSettingsAsync(int id, TableRequest request)
        {
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);

            if (table == null)
                return null;

            table.Number = request.Number;
            table.Seats = request.Seats;
            table.IsAvailable = request.IsAvailable;

            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var delTable = await _context.Tables.FindAsync(id);

            if (delTable == null)
                return false;

            _context.Tables.Remove(delTable);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}