using System.Security.Claims;
using BookVerse.Application.Dtos.User;
using BookVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = BookVerse.Application.Dtos.User.LoginRequest;
using RegisterRequest = BookVerse.Application.Dtos.User.RegisterRequest;
using ResetPasswordRequest = Microsoft.AspNetCore.Identity.Data.ResetPasswordRequest;

namespace BookVerseApi.Controllers;

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
    [AllowAnonymous]
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
    [AllowAnonymous]
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

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _accountService.RefreshTokenAsync(refreshTokenRequest);
        if (!response.Succeeded)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(typeof(LogoutResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LogoutResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(LogoutResponse),StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new LogoutResponse
            {
                Succeeded = false,
                Message = "Invalid user context."
            });
        }

        var response = await _accountService.LogoutAsync(email);
        if (response.Succeeded)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized("User is not logged in.");
        }

        var user = await _accountService.GetCurrentUserAsync(email);
        if (user == null)
            return NotFound("User not found.");
        return Ok(user);
    }
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(BasicResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await _accountService.ForgotPasswordAsync(request);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(BasicResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await _accountService.ResetPasswordAsync(request);
        if (response.Succeeded)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
        
    [Authorize]
    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteMyAccount()
    {
            
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new LogoutResponse
            {
                Succeeded = false,
                Message = "Invalid user context."
            });
        }
        var result = await _accountService.DeleteMyAccountAsync(email);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}