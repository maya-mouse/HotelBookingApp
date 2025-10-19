using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Admin.Bookings
{
    [Authorize(Roles = "Admin")]
    public class IndexModel(IBookingService bookingService) : PageModel
    {
        private readonly IBookingService _bookingService = bookingService;

        public IEnumerable<BookingHistoryDto> AllBookings { get; set; } = new List<BookingHistoryDto>();


        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task OnGetAsync()
        {
            try
            {
                AllBookings = await _bookingService.GetAllBookingsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading all reservations : {ex.Message}";
            }
        }


    }
}