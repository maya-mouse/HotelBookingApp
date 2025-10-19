using System.Security.Principal;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Account
{
    public class LoginModel(IIdentityService identityService) : PageModel
    {
        private readonly IIdentityService _identityService = identityService;

        [BindProperty]
        public LoginDto LoginData { get; set; } = new LoginDto();

        public string? ReturnUrl { get; set; } 

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("/Hotels/Index");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("/Hotels/Index");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _identityService.SignInAsync(LoginData);

            if (result.Succeeded)
            {
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                if (User.IsInRole("Admin"))
                {
                    return RedirectToPage("/Admin/Hotels/Index");
                }

                return LocalRedirect(Url.Content("~/"));
            }

            ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");

            return Page();
        }
    }
}