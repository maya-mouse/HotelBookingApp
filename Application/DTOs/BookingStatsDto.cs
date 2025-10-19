namespace Application.DTOs
{

    public class BookingStatsDto
    {
        public DateTime Date { get; set; }
        public int BookedCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int CancelledCount { get; set; }
}
}