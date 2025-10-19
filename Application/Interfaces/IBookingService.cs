using Application.DTOs;

namespace Application.Interfaces
{
public interface IBookingService
{
    Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto dto);
    Task<IEnumerable<BookingHistoryDto>> GetUserBookingsAsync(string userId);
    Task<IEnumerable<BookingHistoryDto>> GetAllBookingsAsync();
    Task CancelBookingAsync(int bookingId, string userId);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
}
}