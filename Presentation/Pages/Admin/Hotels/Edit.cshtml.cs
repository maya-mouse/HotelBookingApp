using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Admin.Hotels
{
    [Authorize(Roles = "Admin")]
    public class EditModel(IHotelService hotelService) : PageModel
    {
        private readonly IHotelService _hotelService = hotelService;

        [BindProperty]
        public CreateHotelDto HotelData { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Id = id;
            var hotelDto = await _hotelService.GetHotelByIdAsync(id);

            if (hotelDto == null)
            {
                TempData["ErrorMessage"] = $"Hotel Id {id} not found";
                return RedirectToPage("./Index");
            }


            HotelData = new CreateHotelDto
            {
                Name = hotelDto.Name,
                City = hotelDto.City,
                Address = hotelDto.Address,
                Description = hotelDto.Description
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
    
                await _hotelService.UpdateHotelAsync(Id, HotelData);

                TempData["Message"] = $"Hotel '{HotelData.Name}' successfully updated";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Problem with updating hotel ID {Id}: {ex.Message}";
                return Page();
            }
        }
    }
}