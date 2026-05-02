using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;

namespace Services.Implementations
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;

        public AuthenticationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            //Email Already Exists
            if (user is null) 
                throw new UnAuthorizedException();

            // Check Password
            var result =  await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) 
                throw new UnAuthorizedException();

            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                
            };
            var res = await _userManager.CreateAsync(user, registerDto.Password);

            //Validations
            if (!res.Succeeded) 
            {
                var errors = res.Errors.Select(e=> e.Description).ToList();
                throw new ValidationException(errors);
            }

            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }
    }
}
