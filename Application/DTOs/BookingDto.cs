using Domain;

namespace Application.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public required BookingStatus Status { get; set; }
        public required int RoomId { get; set; }
        public required string UserId { get; set; } = "";
    }
}