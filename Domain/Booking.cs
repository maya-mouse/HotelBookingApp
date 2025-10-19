namespace Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public required DateTime CheckIn { get; set; } 
        public required DateTime CheckOut { get; set; }

        public required decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Active;

        public int RoomId { get; set; }
        public string UserId { get; set; } = "";

        public Room Room { get; set; } = null!;
        public User User { get; set; } = null!;

    }
}