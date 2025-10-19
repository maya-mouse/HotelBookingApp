using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces{
    public interface IIdentityService
    {

        Task<IdentityResult> RegisterUserAsync(RegisterDto dto, string role = "Client");
        Task<SignInResult> SignInAsync(LoginDto dto);

        Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync();
        Task SignOutAsync(); 
       
    }
}