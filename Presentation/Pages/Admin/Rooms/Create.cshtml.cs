using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Admin.Rooms
{
    [Authorize(Roles = "Admin")]
    public class CreateModel(IRoomService roomService, IHotelService hotelService) : PageModel
    {
        private readonly IRoomService _roomService = roomService;
        private readonly IHotelService _hotelService = hotelService;

        [BindProperty]
        public CreateRoomDto RoomData { get; set; } = new CreateRoomDto();
        
        [BindProperty(SupportsGet = true)]
        public int HotelId { get; set; }

        public string HotelName { get; set; } = "";

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int hotelId)
        {

            HotelId = hotelId;
            var hotel = await _hotelService.GetHotelByIdAsync(hotelId);
            if (hotel == null)
            {
                TempData["ErrorMessage"] = $"Готель ID {hotelId} не знайдено.";
                return RedirectToPage("/Admin/Hotels/Index");
            }
            HotelName = hotel.Name;
            RoomData.HotelId = hotelId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
             
                var hotel = await _hotelService.GetHotelByIdAsync(HotelId);
                HotelName = hotel?.Name ?? "Undefined hotel";
                return Page();
            }

            try
            {
                RoomData.HotelId = HotelId; 
                var newRoom = await _roomService.CreateRoomAsync(RoomData);
                
                TempData["Message"] = $"Room '{newRoom.RoomNumber}' successfully added to the hotel '{HotelName}'.";
                return RedirectToPage("./Index", new { hotelId = HotelId });
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Problem with creating room : {ex.Message}";
                
                var hotel = await _hotelService.GetHotelByIdAsync(HotelId);
                HotelName = hotel?.Name ?? "Undefined hotel";
                return Page();
            }
        }
    }
}