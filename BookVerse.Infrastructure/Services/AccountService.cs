using BookVerse.Application.Dtos.User;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Constants;
using BookVerse.Core.Entities;
using BookVerse.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using LoginRequest = BookVerse.Application.Dtos.User.LoginRequest;
using RegisterRequest = BookVerse.Application.Dtos.User.RegisterRequest;

namespace BookVerse.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IAuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager, IUserRepository userRepository, IEmailService emailService,
        ILogger<AccountService> logger)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        try
        {
            var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

            if (userExists)
            {
                _logger.LogWarning("Registration attempt with existing email: {Email}", registerRequest.Email);

                return new RegisterResponse
                {
                    Succeeded = false,
                    Message = "A user with this email already exists.",
                    Errors = new[] { "Duplicate email" }
                };
            }

            if (registerRequest.Role != Role.User)
            {
                _logger.LogWarning("Attempt to register with invalid role: {Role}", registerRequest.Role);
                return new RegisterResponse
                {
                    Succeeded = false,
                    Message = "Invalid role specified.",
                    Errors = new[] { "You can only register as a normal user." }
                };
            }

            var identityRoleName = GetIdentityRoleName(registerRequest.Role);

            var roleExists = await _roleManager.RoleExistsAsync(identityRoleName);

            if (!roleExists)
            {
                _logger.LogError("Invalid role specified during registration: {Role}", registerRequest.Role);

                return new RegisterResponse
                {
                    Succeeded = false,
                    Message = "Invalid role specified.",
                    Errors = new[] { $"Role '{identityRoleName} does not exist'" }
                };
            }


            var user = User.Create(registerRequest.Email, registerRequest.FirstName, registerRequest.LastName);

            // user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("User registration failed for {Email}: {Errors}",
                    registerRequest.Email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                return new RegisterResponse
                {
                    Succeeded = false,
                    Message = "Registration failed due to validation errors.",
                    Errors = result.Errors.Select(x => x.Description)
                };
            }

            await _userManager.AddToRoleAsync(user, identityRoleName);
            user.CreatedAtUtc = DateTime.UtcNow;
            user.UpdatedAtUtc = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("User registered successfully: {Email} with role {Role}",
                user.Email, identityRoleName);

            return new RegisterResponse
            {
                Succeeded = true,
                Message = "User registered successfully.",
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for {Email}", registerRequest.Email);
            return new RegisterResponse
            {
                Succeeded = false,
                Message = "An error occurred during registration. Please try again later.",
                Errors = new[] { "Internal server error" }
            };
        }
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginRequest.Email);

                return new LoginResponse
                {
                    Succeeded = false,
                    Message = "Invalid Username or Password",
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user, roles);

            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await _userManager.UpdateAsync(user);
            _logger.LogInformation("User logged in successfully: {Email}", user.Email);

            return new LoginResponse
            {
                Succeeded = true,
                Message = "Login Successful.",
                AccessToken = jwtToken,
                ExpiresAtUtc = expirationDateInUtc,
                RefreshToken = refreshTokenValue
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", loginRequest.Email);
            return new LoginResponse
            {
                Succeeded = false,
                Message = "An error occurred during login. Please try again later."
            };
        }
    }

    public async Task<BasicResponse> DeleteMyAccountAsync(string userEmail)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                _logger.LogWarning("Account deletion attempted for non-existent user: {Email}", userEmail);

                return new BasicResponse()
                {
                    Succeeded = false,
                    Message = "User not found."
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete account for {Email}: {Errors}",
                    userEmail,
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                return new BasicResponse
                {
                    Succeeded = false,
                    Message = "Unable to delete account."
                };
            }

            _logger.LogInformation("Account deleted successfully: {Email}", userEmail);

            return new BasicResponse
            {
                Succeeded = true,
                Message = "Account deleted successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting account for {Email}", userEmail);
            return new BasicResponse
            {
                Succeeded = false,
                Message = "An error occurred while deleting the account."
            };
        }
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        try
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
                _logger.LogWarning("Refresh token not found or invalid");

                return new RefreshTokenResponse
                {
                    Succeeded = false,
                    Message = "Unable to retrieve user with this refresh Token"
                };
            }

            if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
            {
                _logger.LogInformation("Expired refresh token used for user: {Email}", user.Email);

                return new RefreshTokenResponse()
                {
                    Succeeded = false,
                    Message = "Refresh token is expired. Please Login Again"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user, roles);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return new RefreshTokenResponse
            {
                Succeeded = false,
                Message = "An error occurred while refreshing the token."
            };
        }
    }

    public async Task<BasicResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogInformation("Password reset requested for non-existent email: {Email}", request.Email);

                return new BasicResponse
                {
                    Succeeded = true,
                    Message = "Password reset link has been sent to your email."
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var emailBody = $"""
                             Hello {user.FirstName},

                             You requested to reset your password for BookVerseApi.


                             If the link doesn’t work, Here is your token: 
                             {token}

                             This link and token will expire in 10 minutes.

                             Best regards,
                             BookVerseApi Support
                             """;

            await _emailService.SendEmailAsync(
                user.Email!,
                "BookVerseApi Password Reset",
                emailBody
            );
            _logger.LogInformation("Password reset email sent to: {Email}", user.Email);

            return new BasicResponse
            {
                Succeeded = true,
                Message = "Password reset link has been sent to your email."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email for {Email}", request.Email);

            // ✅ SECURITY: Still return success to prevent information leakage
            return new BasicResponse
            {
                Succeeded = true,
                Message = "Password reset link has been sent to your email."
            };
        }
    }

    public async Task<BasicResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);

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

            user.RefreshToken = null;
            user.RefreshTokenExpiresAtUtc = null;
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("Password reset successfully for: {Email}", user.Email);

            return new BasicResponse
            {
                Succeeded = true,
                Message = "Password has been reset successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for {Email}", request.Email);
            return new BasicResponse
            {
                Succeeded = false,
                Message = "An error occurred while resetting the password."
            };
        }
    }

    public async Task<LogoutResponse> LogoutAsync(string userEmail)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                _logger.LogWarning("Logout attempted for non-existent user: {Email}", userEmail);

                return new LogoutResponse
                {
                    Succeeded = false,
                    Message = "User not found."
                };
            }

            // Invalidate the refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiresAtUtc = null;
            _logger.LogInformation("User logged out: {Email}", userEmail);

            await _userManager.UpdateAsync(user);
            return new LogoutResponse
            {
                Succeeded = true,
                Message = "User logged out successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for {Email}", userEmail);
            return new LogoutResponse
            {
                Succeeded = false,
                Message = "An error occurred during logout."
            };
        }
    }

    public async Task<UserProfileDto> GetCurrentUserAsync(string userEmail)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                _logger.LogWarning("User profile requested for non-existent user: {Email}", userEmail);

                return null;
            }

            return new UserProfileDto
            {
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAtUtc = user.CreatedAtUtc,
                UpdatedAtUtc = user.UpdatedAtUtc
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile for {Email}", userEmail);
            return null;
        }
    }

    private string GetIdentityRoleName(Role role)
    {
        return role switch
        {
            Role.User => IdentityRoleConstants.User,
            Role.Admin => IdentityRoleConstants.Admin,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Provided role is not supported.")
        };
    }
}