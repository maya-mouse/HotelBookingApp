namespace Application.DTOs
{
    public class CreateRoomDto
    {
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string RoomNumber { get; set; } = "";
        public int HotelId { get; set; }

    }
}