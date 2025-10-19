using Application.DTOs;
namespace Application.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelDto>> GetAllHotelsAsync();
        Task<HotelDto?> GetHotelByIdAsync(int id);
        Task<HotelDto> CreateHotelAsync(CreateHotelDto dto);
        Task UpdateHotelAsync(int id, CreateHotelDto dto);
        Task DeleteHotelAsync(int id);
        Task<IEnumerable<HotelDto>> GetHotelsByCityAsync(string city);
        Task<IEnumerable<RoomSearchDto>> SearchRoomsAsync(SearchDto criteria);
    }
}