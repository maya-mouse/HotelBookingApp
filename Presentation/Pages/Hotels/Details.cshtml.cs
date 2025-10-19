using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Pages.Hotels
{
    public class DetailsModel(IRoomService roomService, IBookingService bookingService) : PageModel
    {
        private readonly IRoomService _roomService = roomService;
        private readonly IBookingService _bookingService = bookingService;

        public RoomSearchDto RoomDetails { get; set; } = null!;
        
        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [TempData]
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
           
            if (roomId == 0 || checkOut <= checkIn) {
              return RedirectToPage("/Hotels/Index");
            }
    
            RoomId = roomId;
            CheckInDate = checkIn;
            CheckOutDate = checkOut;
            
            try
            {
                RoomDetails = await _roomService.GetRoomDetails(roomId, checkIn, checkOut);

                if (RoomDetails == null)
                {
                    ErrorMessage = "The number was not found or is no longer available for these dates.";
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Loading error: {ex.Message}";
                Console.WriteLine(ex.ToString());
                return RedirectToPage("/Hotels/Index");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { ReturnUrl = Request.Path + Request.QueryString });
            }
            
            if (!ModelState.IsValid)
            {
                await OnGetAsync(RoomId, CheckInDate, CheckOutDate);
                return Page();
            }

            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookingDto = new CreateBookingDto
            {
                RoomId = RoomId,
                UserId = userId!,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate
            };

            try
            {
                await _bookingService.CreateBookingAsync(userId!, bookingDto);
                return RedirectToPage("/Bookings/Index", new { message = "Reservation successfully created!" });
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Booking error: {ex.Message}";
                await OnGetAsync(RoomId, CheckInDate, CheckOutDate);
                return Page();
            }
        }
    }
}