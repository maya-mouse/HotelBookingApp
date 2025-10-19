using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            return room == null
            ? null
            : _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto)
        {

            var created = await _roomRepository.AddAsync(_mapper.Map<Room>(dto));
            return _mapper.Map<RoomDto>(created);
        }

        public async Task UpdateRoomAsync(int id, CreateRoomDto dto)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) throw new Exception("Room not found");

            _mapper.Map(dto, room);

            await _roomRepository.UpdateAsync(room);
        }

        public async Task DeleteRoomAsync(int id)
        {
            await _roomRepository.DeleteAsync(id);
        }


        public async Task<IEnumerable<RoomDto>> GetRoomsByHotelAsync(int hotelId)
        {
            var rooms = await _roomRepository.GetByHotelIdAsync(hotelId);
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<IEnumerable<RoomDto>> SearchAvailableRoomsAsync(SearchDto search)
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync(
                search.City,
                search.CheckInDate,
                search.CheckOutDate);

            var filtered = rooms.AsEnumerable();

            if (search.Capacity.HasValue)
                filtered = filtered.Where(r => r.Capacity >= search.Capacity.Value);

            return _mapper.Map<IEnumerable<RoomDto>>(filtered);
        }

        public async Task<RoomSearchDto> GetRoomDetails(int id, DateTime checkIn, DateTime checkOut)
        {
            
            var room = await _roomRepository.GetByIdAsync(id);


            var numberOfNights = (int)(checkOut - checkIn).TotalDays;
            var totalPrice = room.PricePerNight * numberOfNights;

            var dto = _mapper.Map<RoomSearchDto>(room);

            dto.NumberOfNights = numberOfNights;
            dto.TotalPrice = totalPrice;

            return dto;
        }

    }
}
 

