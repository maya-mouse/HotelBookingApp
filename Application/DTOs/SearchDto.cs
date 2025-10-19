namespace Application.DTOs
{
    public class SearchDto
    {
        public string City { get; set; } = "Kyiv";
        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
        public int? Capacity { get; set; } = 1;

    }
}