using Application.DTOs;
namespace Application.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task<RoomDto?> GetRoomByIdAsync(int id);
        Task<RoomDto> CreateRoomAsync(CreateRoomDto dto);
        Task UpdateRoomAsync(int id, CreateRoomDto dto);
        Task DeleteRoomAsync(int id);
        Task<IEnumerable<RoomDto>> SearchAvailableRoomsAsync(SearchDto search);
        Task<IEnumerable<RoomDto>> GetRoomsByHotelAsync(int hotelId);
        Task<RoomSearchDto> GetRoomDetails(int id, DateTime checkIn, DateTime checkOut);
    }
}