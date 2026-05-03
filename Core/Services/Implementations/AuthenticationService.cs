using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sevices.Abstraction.Contracts;
using Shared.Common;
using Shared.Dtos.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementations
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOptions<JwtOptions> _options;

        public AuthenticationService(UserManager<User> userManager, IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _options = options;
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

            var token = await CreateTokenAsync(user);

            return new UserResultDto(user.DisplayName, token, user.Email!);
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

            var token = await CreateTokenAsync(user);

            return new UserResultDto(user.DisplayName, token, user.Email);
        }


        #region Helper Methods

        //Generate Token
        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = _options.Value;

            // Claims [Name, Email,Rloe]
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.DisplayName!),
                new Claim(ClaimTypes.Email , user.Email!),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles) 
                claims.Add(new Claim(ClaimTypes.Role,role));

            //secret key => SymmetricSecurityKey
            var secretKey = jwtOptions.SecretKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            //Algorithm
            var signInCreds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audiance,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: signInCreds
            );

            //JWTSecurityHandler => WriteToken
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
