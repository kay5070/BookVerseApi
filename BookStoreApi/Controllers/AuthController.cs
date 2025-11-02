using BookStoreApi.Dtos.User;
using BookStoreApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegisterResponse),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            var response = await _accountService.RegisterAsync(registerRequest);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _accountService.LoginAsync(loginRequest);
            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
