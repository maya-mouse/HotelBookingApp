using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;
using System.Collections.Generic;

namespace Presentation.Pages.Admin.Hotels
{
  [Authorize(Roles = "Admin")]
    public class IndexModel(IHotelService hotelService) : PageModel
    {
        private readonly IHotelService _hotelService = hotelService;

        public IEnumerable<HotelDto> Hotels { get; set; } = new List<HotelDto>();

        [TempData]
        public string Message { get; set; } = "";
        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task OnGetAsync()
        {
            try
            {
                Hotels = await _hotelService.GetAllHotelsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading data : {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {

            try
            {
                await _hotelService.DeleteHotelAsync(id);
                Message = $"Hotel ID {id} successfully deleted";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting hotel ID {id}: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}