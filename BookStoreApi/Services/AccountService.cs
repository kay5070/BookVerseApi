using BookStoreApi.Dtos.User;
using BookStoreApi.Entities;
using BookStoreApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Build.Execution;
using LoginRequest = BookStoreApi.Dtos.User.LoginRequest;
using RegisterRequest = BookStoreApi.Dtos.User.RegisterRequest;

namespace BookStoreApi.Services;

public class AccountService : IAccountService
{
    private readonly IAuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public AccountService(IAuthTokenProcessor authTokenProcessor,UserManager<User> userManager, IUserRepository userRepository,IEmailService emailService)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _userRepository = userRepository;
        _emailService = emailService;
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

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenRequest.RefreshToken))
        {
            return new RefreshTokenResponse()
            {
                Succeeded = false,
                Message = "Refresh token is missing."
            };
        }

        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshTokenRequest.RefreshToken);

        if (user == null)
        {
            return new RefreshTokenResponse
            {
                Succeeded = false,
                Message = "Unable to retrieve user with this refresh Token"
            };
        }

        if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
        {
            return new RefreshTokenResponse()
            {
                Succeeded = false,
                Message = "Refresh token is expired. Please Login Again"
            };
        }

        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();
        var refreshExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshExpirationDateInUtc;
        await _userManager.UpdateAsync(user);
        return new RefreshTokenResponse()
        {
            Succeeded = true,
            Message = "Token refreshed successfully.",
            AccessToken = jwtToken,
            ExpiresAtUtc = expirationDateInUtc,
            RefreshToken = refreshTokenValue
        };
    }

    public async Task<BasicResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return new BasicResponse
            {
                Succeeded = true,
                Message = "Password reset link has been sent to your email."
            };
        }

        var token = await _userManager .GeneratePasswordResetTokenAsync(user);
        
        var resetLink = $"https://localhost:7102/api/Auth/reset-password?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";

        var emailBody = $"""
                         Hello {user.FirstName},

                         You requested to reset your password for BookStoreApi.

                         To reset your password, click the link below:
                         {resetLink}

                         If the link doesn’t work, copy this token and use it in Swagger:
                         {token}

                         This link and token will expire in 10 minutes.

                         Best regards,
                         BookStoreApi Support
                         """;

        await _emailService.SendEmailAsync(
            user.Email!,
            "BookStoreApi Password Reset",
            emailBody
        );
        
        return new BasicResponse
        {
            Succeeded = true,
            Message = "Password reset link has been sent to your email."
        };
    }

    public async Task<BasicResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new BasicResponse
            {
                Succeeded = false,
                Message = "Invalid email address."
            };
        }

        var result = await _userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);

        if (!result.Succeeded)
        {
            return new BasicResponse
            {
                Succeeded = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        return new BasicResponse
        {
            Succeeded = true,
            Message = "Password has been reset successfully."
        };
    }

    public async Task<LogoutResponse> LogoutAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return new LogoutResponse
            {
                Succeeded = false,
                Message = "User not found."
            };
        }
        // Invalidate the refresh token
        user.RefreshToken = null;
        user.RefreshTokenExpiresAtUtc = null;

        await _userManager.UpdateAsync(user);
        return new LogoutResponse
        {
            Succeeded = true,
            Message = "User logged out successfully."
        };
    }

    public async Task<UserProfileDto> GetCurrentUserAsync(string userEmail)
    {
        var user =await _userManager.FindByEmailAsync(userEmail);
        if (user == null) return null;

        return new UserProfileDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}



















