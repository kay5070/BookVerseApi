using BookVerseApi.Dtos.User;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = BookVerseApi.Dtos.User.LoginRequest;
using RegisterRequest = BookVerseApi.Dtos.User.RegisterRequest;

namespace BookVerseApi.Interfaces;

public interface IAccountService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task<LogoutResponse> LogoutAsync(string userEmail);
    Task<UserProfileDto> GetCurrentUserAsync(string userEmail);
    Task<BasicResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<BasicResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<BasicResponse> DeleteMyAccountAsync(string userEmail);
}
