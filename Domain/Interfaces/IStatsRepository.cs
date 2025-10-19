namespace Domain.Interfaces
{
    public interface IStatsRepository
    {
        Task<IEnumerable<object>> GetBookingStatisticsAsync(DateTime start, DateTime end);
    }
}