namespace Application.DTOs
{
     public class RoomSearchDto
    {
        public int RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public int HotelId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfNights { get; set; } 
    }
}