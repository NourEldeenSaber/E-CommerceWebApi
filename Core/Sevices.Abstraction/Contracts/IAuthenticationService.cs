using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;

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

        //Get Current User
        Task<UserResultDto> GetCurrentUserAsync(string userEmail);

        //Check IF Email Exist
        Task<bool> CheckEmailExistAsync(string userEmail);

        //Get Address 
        Task<AddressDto> GetUserAddressAsync(string userEmail);

        //Update Address
        Task<AddressDto> UpdateUserAddressAsync(string userEmail,  AddressDto addressDto);

    }
}
