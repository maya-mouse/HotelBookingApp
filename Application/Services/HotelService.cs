using Domain;
using Domain.Interfaces;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;

namespace Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<HotelDto>> GetAllHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public async Task<HotelDto?> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return null;

            return _mapper.Map<HotelDto>(hotel);

        }

        public async Task<HotelDto> CreateHotelAsync(CreateHotelDto dto)
        {
            var created = await _hotelRepository.AddAsync(_mapper.Map<Hotel>(dto));
            return _mapper.Map<HotelDto>(created);
        }

        public async Task UpdateHotelAsync(int id, CreateHotelDto dto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) throw new Exception("Hotel not found");
            _mapper.Map(dto, hotel);
            await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task DeleteHotelAsync(int id)
        {
            await _hotelRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<HotelDto>> GetHotelsByCityAsync(string city)
        {
            var hotels = await _hotelRepository.GetByCityAsync(city);
            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public async Task<IEnumerable<RoomSearchDto>> SearchRoomsAsync(SearchDto criteria)
        {
            if (criteria.CheckOutDate <= criteria.CheckInDate)
            {
                throw new ArgumentException("Дата виїзду має бути пізнішою за дату заїзду.");
            }

            var numberOfNights = (int)(criteria.CheckOutDate - criteria.CheckInDate).TotalDays;
            if (numberOfNights <= 0)
            {
                throw new ArgumentException("Період бронювання має бути принаймні 1 ніч.");
            }

            var availableRooms = await _hotelRepository.FindAvailableRoomsAsync(
                criteria.City,
                criteria.CheckInDate,
                criteria.CheckOutDate,
                criteria.Capacity
            );

            var results = new List<RoomSearchDto>();
            foreach (var room in availableRooms)
            {
                var dto = _mapper.Map<RoomSearchDto>(room);
                dto.NumberOfNights = numberOfNights;
                dto.TotalPrice = room.PricePerNight * numberOfNights;

                results.Add(dto);
            }

            return results;
        }


    }

}
