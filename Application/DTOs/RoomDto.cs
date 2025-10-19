
using Domain;

namespace Application.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = "";
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }

        public int HotelId { get; set; }

    }
}