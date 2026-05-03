using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementations
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
            var secretKey = _configuration["JWTOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            //Algorithm
            var signInCreds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issure"],
                audience: _configuration["JWTOptions:Audiance"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signInCreds
            );

            //JWTSecurityHandler => WriteToken
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
