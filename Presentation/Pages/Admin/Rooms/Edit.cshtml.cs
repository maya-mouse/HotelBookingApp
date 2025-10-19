using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Admin.Rooms
{
    [Authorize(Roles = "Admin")]
    public class EditModel(IRoomService roomService, IHotelService hotelService) : PageModel
    {
        private readonly IRoomService _roomService = roomService;
        private readonly IHotelService _hotelService = hotelService;

        [BindProperty]
        public CreateRoomDto RoomData { get; set; } = null!;
        
        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }
        

        [BindProperty] 
        public int HotelId { get; set; }
        public string HotelName { get; set; } = "";

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int roomId)
        {
            RoomId = roomId;
            var roomDto = await _roomService.GetRoomByIdAsync(roomId);

            if (roomDto == null)
            {
                TempData["ErrorMessage"] = $"Номер ID {roomId} не знайдено.";
                return RedirectToPage("./Index");
            }
            Console.WriteLine(roomDto.HotelId);
       
            HotelId = roomDto.HotelId;
    
            var hotel = await _hotelService.GetHotelByIdAsync(HotelId);
            HotelName = hotel?.Name ?? "Невідомий Готель";
            
            RoomData = new CreateRoomDto
            {
                RoomNumber = roomDto.RoomNumber,
                Capacity = roomDto.Capacity,
                PricePerNight = roomDto.PricePerNight,
                HotelId = roomDto.HotelId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var hotel = await _hotelService.GetHotelByIdAsync(HotelId);
                HotelName = hotel?.Name ?? "Невідомий Готель";
                return Page();
            }

            try
            {
                await _roomService.UpdateRoomAsync(RoomId, RoomData);
                
                TempData["Message"] = $"Room '{RoomData.RoomNumber}' successfully updated.";
                return RedirectToPage("./Index", new { hotelId = HotelId });
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Problem room updating : {ex.Message}";
                var hotel = await _hotelService.GetHotelByIdAsync(HotelId);
                HotelName = hotel?.Name ?? "Undefined hotel";
                return Page(); 
            }
        }
    }
}