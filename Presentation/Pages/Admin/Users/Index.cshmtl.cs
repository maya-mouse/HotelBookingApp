using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;
using Application.Interfaces;
using System.Collections.Generic;

namespace Presentation.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel(IIdentityService identityService) : PageModel
    {
        private readonly IIdentityService _identityService = identityService;

        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();

        [TempData]
        public string Message { get; set; } = "";
        [TempData]
        public string ErrorMessage { get; set; } = "";

        public async Task OnGetAsync()
        {
            try
            {
                var usersWithRoles = await _identityService.GetAllUsersWithRolesAsync();
                Users = usersWithRoles;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"User upload error: {ex.Message}";
            }
        }
    }
}