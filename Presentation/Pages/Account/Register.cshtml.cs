using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;

namespace Presentation.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public RegisterModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [BindProperty]
        public RegisterDto RegisterData { get; set; } = new RegisterDto();
        

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
                var result = await _identityService.RegisterUserAsync(RegisterData);

                if (result.Succeeded)
                {
                    return RedirectToPage("./Login", new { message = "Реєстрація успішна! Увійдіть." });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Помилка сервера при реєстрації.");
            }

            return Page();
        }
    }
}