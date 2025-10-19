using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.Interfaces;

namespace Presentation.Pages.Account
{

    public class LogoutModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public LogoutModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
         
            await _identityService.SignOutAsync();
            
            return RedirectToPage("/Hotels/Index");
        }
    }
}