using Application.DTOs;

namespace Application.Interfaces
{
    public interface IStatsService
    {
        Task<IEnumerable<BookingStatsDto>> GetBookingStatisticsAsync(DateTime start, DateTime end);
    }
   
}