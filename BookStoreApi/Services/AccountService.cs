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
}



















