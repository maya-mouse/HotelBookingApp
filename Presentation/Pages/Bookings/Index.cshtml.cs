using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;
using System.Security.Claims;

namespace Presentation.Pages.Bookings
{
    [Authorize(Roles = "Client, Admin")]
    public class IndexModel(IBookingService bookingService) : PageModel
    {
        private readonly IBookingService _bookingService = bookingService;

        public IEnumerable<BookingHistoryDto> Bookings { get; set; } = new List<BookingHistoryDto>();

        [TempData]
        public string Message { get; set; } = "";

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                Bookings = await _bookingService.GetUserBookingsAsync(userId);
                
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading booking history : {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int bookingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _bookingService.CancelBookingAsync(bookingId, userId!);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error cancelling reservation : {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}