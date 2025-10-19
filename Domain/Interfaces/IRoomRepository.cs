namespace Domain.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(string city, DateTime checkIn, DateTime checkOut);
    Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
    }  
}