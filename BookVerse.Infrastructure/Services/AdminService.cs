using BookVerse.Application.Dtos.User;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Constants;
using BookVerse.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookVerse.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AdminService> _logger;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public AdminService(UserManager<User> userManager, ILogger<AdminService> logger,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersWithRoles = new List<UserWithRolesDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersWithRoles.Add(new UserWithRolesDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            });
        }

        return usersWithRoles;
    }

    public async Task<UserWithRolesDto?> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserWithRolesDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
    }

    public async Task<BasicResponse> MakeUserAdminAsync(Guid userId, string currentAdminEmail)
    {
        try
        {
            
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || user.Id == Guid.Empty)
                return new BasicResponse { Succeeded = false, Message = "Invalid user ID." };
            if (user.Email == currentAdminEmail)
            {
                return new BasicResponse { Succeeded = false, Message = "You cannot change your own role." };
            }

            if (await _userManager.IsInRoleAsync(user, IdentityRoleConstants.Admin))
            {
                return new BasicResponse { Succeeded = false, Message = "User is already an admin." };
            }

            var result = await _userManager.AddToRoleAsync(user, IdentityRoleConstants.Admin);

            if (!result.Succeeded)
            {
                return new BasicResponse
                    { Succeeded = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            _logger.LogInformation($"User {user.Email} granted Admin role by admin");
            return new BasicResponse { Succeeded = true, Message = $"User {user.Email} has been granted Admin role." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making user {UserId} admin", userId);
            return new BasicResponse { Succeeded = false, Message = "An error occurred while updating user role." };
        }
    }

    public async Task<BasicResponse> RemoveAdminRoleAsync(Guid userId, Guid currentAdminId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new BasicResponse { Succeeded = false, Message = "User not found." };
            }

            if (user.Id == currentAdminId)
            {
                return new BasicResponse { Succeeded = false, Message = "You cannot remove your own admin role." };
            }

            if (!await _userManager.IsInRoleAsync(user, IdentityRoleConstants.Admin))
            {
                return new BasicResponse { Succeeded = false, Message = "User is not an Admin." };
            }

            var result = await _userManager.RemoveFromRoleAsync(user, IdentityRoleConstants.Admin);
            if (!result.Succeeded)
                return new BasicResponse
                    { Succeeded = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            var addResult = await _userManager.AddToRoleAsync(user, IdentityRoleConstants.User);
            if (!addResult.Succeeded)
                return new BasicResponse
                    { Succeeded = false, Message = string.Join(", ", addResult.Errors.Select(e => e.Description)) };

            _logger.LogInformation("Admin role removed from user {Email}", user.Email);

            return new BasicResponse { Succeeded = true, Message = $"Admin role removed from user {user.Email}." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error removing admin role from user {userId}");
            return new BasicResponse { Succeeded = false, Message = "An error occurred while updating user role." };
        }
    }

    public async Task<BasicResponse> DeleteUserAsync(Guid userId, string currentAdminEmail)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new BasicResponse { Succeeded = false, Message = "User not found." };

            if (user.Email == currentAdminEmail)
                return new BasicResponse { Succeeded = false, Message = "You cannot delete your own account." };

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return new BasicResponse
                    { Succeeded = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            _logger.LogInformation("User {Email} deleted by admin", user.Email);

            return new BasicResponse { Succeeded = true, Message = $"User {user.Email} has been deleted." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user {userId}");
            return new BasicResponse { Succeeded = false, Message = "An error occurred while deleting the user." };
        }
    }
}