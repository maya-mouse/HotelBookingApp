using Domain;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .ToListAsync();
        }

        public async Task<Room> AddAsync(Room entity)
        {
            _context.Rooms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Room entity)
        {
            _context.Rooms.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(string city, DateTime checkIn, DateTime checkOut)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.Hotel.City.ToLower().Contains(city.ToLower()))
                .Where(r => !r.Bookings.Any(b =>
                      b.Status != BookingStatus.Cancelled &&
                    ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                     (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                     (checkIn <= b.CheckIn && checkOut >= b.CheckOut))))
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var hasConflict = await _context.Bookings
                .AnyAsync(b =>
                    b.RoomId == roomId &&
                    b.Status != BookingStatus.Cancelled &&
                    ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                     (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                     (checkIn <= b.CheckIn && checkOut >= b.CheckOut)));

            return !hasConflict;
        }
    }
}