using AutoMapper;
using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatsRepository _statsRepository;

        public StatsService(IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;

        }



        public async Task<IEnumerable<BookingStatsDto>> GetBookingStatisticsAsync(DateTime start, DateTime end)
        {
            var statsDataAsObject = await _statsRepository.GetBookingStatisticsAsync(start, end);

            if (statsDataAsObject == null)
            {
                return Enumerable.Empty<BookingStatsDto>();
            }

            return statsDataAsObject.Cast<BookingStatsDto>().ToList();
        }
    }
}