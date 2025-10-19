using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.Interfaces;
using Application.DTOs;

namespace Presentation.Pages.Hotels
{
    public class IndexModel(IHotelService hotelService) : PageModel
    {
        private readonly IHotelService _hotelService = hotelService;

        [BindProperty(SupportsGet = true)]
        public SearchDto SearchCriteria { get; set; } = null!;
      

    
        public IEnumerable<RoomSearchDto> AvailableRooms { get; set; } = new List<RoomSearchDto>();
        public bool IsSearched { get; set; } = false;

        [TempData]
        public string ErrorMessage { get; set; } =  "";
        public async Task OnGetAsync()
        {
            
            if (!string.IsNullOrEmpty(SearchCriteria.City) || SearchCriteria.CheckInDate != DateTime.Today.AddDays(1))
            {

                if (SearchCriteria.CheckOutDate <= SearchCriteria.CheckInDate)
                {
                    ModelState.AddModelError(string.Empty, "Дата виїзду має бути пізнішою за дату заїзду.");
                    AvailableRooms = new List<RoomSearchDto>();
                }
                else
                {
                    AvailableRooms = await _hotelService.SearchRoomsAsync(SearchCriteria);
                }
                IsSearched = true;
            }
        
        }
       
        }
    }
