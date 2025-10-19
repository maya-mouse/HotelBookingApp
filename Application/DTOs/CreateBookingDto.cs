namespace Application.DTOs
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public string UserId { get; set; } =  "";
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}