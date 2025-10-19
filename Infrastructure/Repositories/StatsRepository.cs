
using Dapper;
using Application.DTOs;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly string _connectionString;

        public StatsRepository(IConfiguration configuration)
        {
           
            _connectionString = configuration.GetConnectionString("Default")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<object>> GetBookingStatisticsAsync(DateTime start, DateTime end)
        {
            const string sql = @"
        SELECT 
            DATE(b.CheckIn) AS Date,
            COUNT(b.Id) AS BookedCount,
            SUM(b.TotalPrice) AS TotalRevenue,
            
            
            SUM(CASE WHEN b.Status = 'Cancelled' THEN 1 ELSE 0 END) AS CancelledCount
            
        FROM Bookings b
        WHERE b.CheckIn >= @StartDate AND b.CheckIn <= @EndDate
        GROUP BY DATE(b.CheckIn)
        ORDER BY Date;";

            await using var connection = new MySqlConnection(_connectionString);

            var stats = await connection.QueryAsync<BookingStatsDto>(
                sql,
                new { StartDate = start.Date, EndDate = end.Date }
            );

            return stats;
        }

    }
}