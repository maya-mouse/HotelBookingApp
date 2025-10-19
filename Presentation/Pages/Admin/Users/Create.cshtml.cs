using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.DTOs;
using Application.Interfaces;
namespace Presentation.Pages.Admin.Users
{

    [Authorize(Roles = "Admin")]
    public class CreateModel(IIdentityService identityService) : PageModel
    {
        private readonly IIdentityService _identityService = identityService;

        [BindProperty]
        public RegisterDto RegisterData { get; set; } = new RegisterDto();

        public SelectList RoleOptions { get; set; } = null!;

        [TempData]
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        
            RoleOptions = new SelectList(new List<string> { "Client", "Admin" });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RoleOptions = new SelectList(new List<string> { "Client", "Admin" });
                return Page();
            }

            try
            {
              
                var result = await _identityService.RegisterUserAsync(RegisterData, "Client"); 
                
                if (result.Succeeded)
                {
                    TempData["Message"] = $"User {RegisterData.Email} created with role {RegisterData.RoleName}.";
                    return RedirectToPage("./Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Server error : {ex.Message}";
            }
            
            RoleOptions = new SelectList(new List<string> { "Client", "Admin" });
            return Page();
        }
    }
}