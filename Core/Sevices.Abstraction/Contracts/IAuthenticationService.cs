using Shared.Dtos.IdentityModule;

namespace Sevices.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        //Login     ==> UserResultDTO [DisplayName, Token, Email]
        //          ==> Parameters [Email, Password]
        Task<UserResultDto> LoginAsync(LoginDto loginDto);

        //Register  ==> UserResultDTO [DisplayName, Token, Email]
        //          ==> Parameters [Email, Password, PhoneNumber, Name, DisplayName]
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

    }
}
