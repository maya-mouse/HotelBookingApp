using Domain;

namespace Application.DTOs
{
     public class BookingHistoryDto
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public required BookingStatus Status { get; set; }
        public string RoomNumber { get; set; } = "";
        public int Capacity { get; set; }
        public string UserId { get; set; } = "";

        public string HotelName { get; set; } = "";
        public string HotelCity { get; set; } = "";
    }
}