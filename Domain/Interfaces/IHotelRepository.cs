namespace Domain.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
         Task<IEnumerable<Hotel>> GetByCityAsync(string city);
         Task<IEnumerable<Room>> FindAvailableRoomsAsync(string city, DateTime checkIn, DateTime checkOut, int? capacity);
        Task<IEnumerable<string>> GetAvailableCitiesAsync();
    }
}