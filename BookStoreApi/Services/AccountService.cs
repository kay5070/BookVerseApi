using BookStoreApi.Dtos.User;
using BookStoreApi.Entities;
using BookStoreApi.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApi.Services;

public class AccountService : IAccountService
{
    private readonly IAuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;

    public AccountService(IAuthTokenProcessor authTokenProcessor,UserManager<User> userManager)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
    }
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

        if (userExists)
        {
            return new RegisterResponse
            {
                Succeeded = false,
                Message = "A user with this email already exists.",
                Errors = new []{"Duplicate email"}
            };
        }

        var user = User.Create(registerRequest.Email, registerRequest.FirstName, registerRequest.LastName);

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            return new RegisterResponse
            {
                Succeeded = false,
                Message = "Registration failed due to validation errors.",
                Errors = result.Errors.Select(x => x.Description)
            };
        }

        return new RegisterResponse
        {
            Succeeded = true,
            Message = "User registered successfully.",
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user,loginRequest.Password))
        {
            return new LoginResponse
            {
                Succeeded = false,
                Message = "Invalid Username or Password",
            };
        }

        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);

        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

        await _userManager.UpdateAsync(user);
        return new LoginResponse
        {
            Succeeded = true,
            Message = "Login Successful.",
            AccessToken = jwtToken,
            ExpiresAtUtc = expirationDateInUtc,
            RefreshToken = refreshTokenValue
        };
    }
}



















