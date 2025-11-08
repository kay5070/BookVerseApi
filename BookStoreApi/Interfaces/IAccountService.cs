using BookStoreApi.Dtos.User;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = BookStoreApi.Dtos.User.LoginRequest;
using RegisterRequest = BookStoreApi.Dtos.User.RegisterRequest;

namespace BookStoreApi.Interfaces;

public interface IAccountService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task<BasicResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<BasicResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<LogoutResponse> LogoutAsync(string userEmail);
    Task<UserProfileDto> GetCurrentUserAsync(string userEmail);
}