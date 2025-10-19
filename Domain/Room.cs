namespace Domain
{
    public class Room
    {
        public int Id { get; set; }
        public required string RoomNumber { get; set; } = "";
        public required decimal PricePerNight { get; set; }
        public required int Capacity { get; set; }
        public required int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}