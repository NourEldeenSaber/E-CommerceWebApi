using Microsoft.AspNetCore.Mvc;
using Sevices.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;

namespace Presentation.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        //post[Register]
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> LoginAsync([FromBody] RegisterDto registerDto)
            => Ok(await _serviceManager.AuthenticationService.RegisterAsync(registerDto));

        //post[Login]
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> LoginAsync([FromBody]LoginDto loginDto)
            => Ok(await _serviceManager.AuthenticationService.LoginAsync(loginDto));

    }
}
