using Domain;

namespace Application.DTOs
{
    public class HotelDto
    {
        public int Id { get; set; }
        public required string Name { get; set; } 
        public required string City { get; set; } 
        public required string Address { get; set; } 
        public required string Description { get; set; }

        public ICollection<RoomDto> Rooms { get; set; } = [];
    }
}