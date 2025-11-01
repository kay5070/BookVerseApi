using Microsoft.AspNetCore.Identity.Data;

namespace BookStoreApi.Interfaces;

public interface IAccountService
{
    Task RegisterAsync(RegisterRequest registerRequest);
}