using Domain;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .ToListAsync();
        }

        public async Task<Hotel> AddAsync(Hotel entity)
        {
            _context.Hotels.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Hotel entity)
        {
            _context.Hotels.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Hotel>> GetByCityAsync(string city)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.City.ToLower().Contains(city.ToLower()))
                .ToListAsync();
        }


        public async Task<IEnumerable<Room>> FindAvailableRoomsAsync(string city, DateTime checkIn, DateTime checkOut, int? capacity = 1)
        {

            var query = _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Bookings)
                .Where(r => r.Hotel.City.ToLower() == city.ToLower())
                .Where(r => r.Hotel.City.ToLower() == city.ToLower() && r.Capacity >= capacity);

            var availableRooms = await query
                .Where(r => !r.Bookings.Any(b =>
                    b.Status != BookingStatus.Cancelled &&
                    checkIn < b.CheckOut && checkOut > b.CheckIn
                ))
                .ToListAsync();

            return availableRooms;
        }

        public async Task<IEnumerable<string>> GetAvailableCitiesAsync()
        {

            return await _context.Hotels
                .Select(h => h.City)
                .Distinct()
                .OrderBy(city => city)
                .ToListAsync();
        }
    }

}
