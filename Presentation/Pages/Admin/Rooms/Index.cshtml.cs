using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;
using System.Collections.Generic;

namespace Presentation.Pages.Admin.Rooms
{
    [Authorize(Roles = "Admin")]
    public class IndexModel(IRoomService roomService, IHotelService hotelService) : PageModel
    {
        private readonly IRoomService _roomService = roomService;
        private readonly IHotelService _hotelService = hotelService;

        [BindProperty(SupportsGet = true)]
        public int HotelId { get; set; }

        public string HotelName { get; set; } = "";

        public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();

        [TempData]
        public string Message { get; set; } = "";
        [TempData]
        public string ErrorMessage { get; set; } = "";
        
        public async Task<IActionResult> OnGetAsync(int hotelId)
        {
            if (hotelId <= 0)
            {
                ErrorMessage = "Invalid hotel ID";
                return RedirectToPage("/Admin/Hotels/Index");
            }

            HotelId = hotelId;
            try
            {
              
                var hotel = await _hotelService.GetHotelByIdAsync(hotelId);
                if (hotel == null)
                {
                    ErrorMessage = $"Hotel Id {hotelId} not found";
                    return RedirectToPage("/Admin/Hotels/Index");
                }
                HotelName = hotel.Name;


                Rooms = await _roomService.GetRoomsByHotelAsync(hotelId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Помилка завантаження номерів: {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int roomId)
        {
            try
            {
                await _roomService.DeleteRoomAsync(roomId);
                Message = $"The number has been successfully deleted";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting number: {ex.Message}";
            }

            return RedirectToPage(new { hotelId = HotelId });
        }
    }
}