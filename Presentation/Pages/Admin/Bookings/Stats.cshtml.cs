using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Admin.Bookings
{
    [Authorize(Roles = "Admin")]
    public class StatsModel(IStatsService statsService) : PageModel
    {
        private readonly IStatsService _statsService = statsService;

        [BindProperty(SupportsGet = true)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [BindProperty(SupportsGet = true)]
    public DateTime EndDate { get; set; } =  new DateTime(DateTime.Today.Year, 12, 1); 

        public IEnumerable<BookingStatsDto> Statistics { get; set; } = new List<BookingStatsDto>();

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task OnGetAsync()
        {
            if (StartDate > EndDate)
            {
                ErrorMessage = "Start date cannot be later than the end date";
                return;
            }

            try
            {   
                Statistics = await _statsService.GetBookingStatisticsAsync(StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Помилка завантаження статистики: {ex.Message}";
            }
        }
        
     
    }
}