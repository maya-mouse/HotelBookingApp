using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Mapper
{
    public class MappingProfile : Profile
{
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>();
            CreateMap<Room, CreateRoomDto>();
            CreateMap<HotelDto, CreateHotelDto>();
            CreateMap<RoomDto, CreateRoomDto>();
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<User, LoginDto>();
            CreateMap<User, RegisterDto>();
            CreateMap<RoomDto, RoomSearchDto>();
            CreateMap<Room, RoomSearchDto>()
               .ForMember(dest => dest.Name,
                           opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.Address,
                           opt => opt.MapFrom(src => src.Hotel.Address))
                .ForMember(dest => dest.City,
                           opt => opt.MapFrom(src => src.Hotel.City))
                .ForMember(dest => dest.Description,
                           opt => opt.MapFrom(src => src.Hotel.Description))
                .ForMember(dest => dest.RoomId,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HotelId,
                           opt => opt.MapFrom(src => src.Hotel.Id));

            CreateMap<Booking, CreateBookingDto>();
            CreateMap<Booking, BookingHistoryDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src => src.Room!.RoomNumber))
                .ForMember(dest => dest.Capacity,
                    opt => opt.MapFrom(src => src.Room!.Capacity))
                .ForMember(dest => dest.HotelName,
                    opt => opt.MapFrom(src => src.Room!.Hotel!.Name))
                .ForMember(dest => dest.HotelCity,
                    opt => opt.MapFrom(src => src.Room!.Hotel!.City));
            CreateMap<BookingStatsDto, BookingStatsDto>();

        }
    }
}