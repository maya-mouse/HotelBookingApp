using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService(UserManager<User> userManager,
        SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto, string role = "Client")
        {
            var user = new User { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        public async Task<SignInResult> SignInAsync(LoginDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email)
               ?? await _userManager.FindByNameAsync(dto.Email);

            if (user == null)
            {
                return SignInResult.Failed;
            }


            return await _signInManager.PasswordSignInAsync(
                user,
                dto.Password,
                false,
                lockoutOnFailure: false
            );
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync()
        {
   
            var users = await _userManager.Users.ToListAsync();
            
            var usersWithRoles = new List<UserDto>();

        
            foreach (var user in users)
            {
            
                var roles = await _userManager.GetRolesAsync(user);
                
                usersWithRoles.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Role = roles.FirstOrDefault() ?? "Client"
                });
            }

            return usersWithRoles;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}