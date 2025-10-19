using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Admin.Hotels
{
    [Authorize(Roles = "Admin")]
    public class CreateModel(IHotelService hotelService) : PageModel
    {
        private readonly IHotelService _hotelService = hotelService;

        [BindProperty]
        public CreateHotelDto HotelData { get; set; } = new CreateHotelDto();

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

    
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var newHotel = await _hotelService.CreateHotelAsync(HotelData);

                TempData["Message"] = $"Hotel '{newHotel.Name}' successfully created";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Problem creating hotel: {ex.Message}";
                return Page();
            }
        }
    }
}