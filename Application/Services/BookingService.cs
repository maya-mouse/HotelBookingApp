using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Interfaces;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
     private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository,
        IRoomRepository roomRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

    public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto dto)
    {
        var isAvailable = await _roomRepository.IsRoomAvailableAsync(
            dto.RoomId, dto.CheckInDate, dto.CheckOutDate);

        if (!isAvailable)
            throw new Exception("Room is not available for selected dates");

        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room == null) throw new Exception("Room not found");

        var nights = (dto.CheckOutDate - dto.CheckInDate).Days;
        var totalPrice = room.PricePerNight * nights;

        var booking = 
        new Booking
        {
            UserId = userId,
            RoomId = dto.RoomId,
            CheckIn = dto.CheckInDate,
            CheckOut = dto.CheckOutDate,
            TotalPrice = totalPrice,

        };

        var created = await _bookingRepository.AddAsync(booking);
        return _mapper.Map<BookingDto>(created);
    }

        // Application/Services/BookingService.cs

        public async Task<IEnumerable<BookingHistoryDto>> GetUserBookingsAsync(string userId)
        {
            // Отримуємо сутності Booking (включаючи пов'язані дані)
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);

            // Мапуємо на DTO
            var historyList = new List<BookingHistoryDto>();
            foreach (var booking in bookings)
            {
                // Мапимо ОДИН об'єкт за раз
                var dto = _mapper.Map<BookingHistoryDto>(booking);
                historyList.Add(dto);
            }

            foreach (var bookingDto in historyList)
            {
                // Перевіряємо статус у DTO, а не в оригінальній сутності

                if (bookingDto.Status == Domain.BookingStatus.Cancelled)
                {
                    // Якщо вже скасовано, пропускаємо подальшу логіку
                    continue;
                }

                if (bookingDto.CheckOut.Date < DateTime.Today.Date)
                {

                    bookingDto.Status = BookingStatus.Past;
                    var booking = await _bookingRepository.GetByIdAsync(bookingDto.Id);

                    if (booking != null && booking.Status != BookingStatus.Past)
                    {
                        booking.Status = BookingStatus.Past;


                        await _bookingRepository.UpdateAsync(booking);
                    }
                }
                else
                {

                    bookingDto.Status = BookingStatus.Active;
                }
            }

            return historyList;
        }


        public async Task CancelBookingAsync(int bookingId, string userId)
        {

            var booking = await _bookingRepository.GetByIdAsync(bookingId);


            if (booking is null)
            {
                throw new Exception("Booking not found");
            }

            if (booking.UserId != userId) throw new Exception("Unauthorized");

            booking.Status = BookingStatus.Cancelled;
            
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return await _roomRepository.IsRoomAvailableAsync(roomId, checkIn, checkOut);
    }

        public async Task<IEnumerable<BookingHistoryDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
        }
    }
}