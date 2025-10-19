namespace Domain.Interfaces
{
public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId);

}

}