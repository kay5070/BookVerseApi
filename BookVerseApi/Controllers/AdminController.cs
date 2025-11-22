using System.Security.Claims;
using BookVerseApi.Dtos.User;
using BookVerseApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles= "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserWithRolesDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _adminService.GetAllUsersAsync();
        return Ok(users);
    }
    [HttpGet("users/{userId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UserWithRolesDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _adminService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("users/{userId:guid}/make-admin")]
    [ProducesResponseType(typeof(BasicResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakeUserAdmin(Guid userId)
    {
        var currentAdminEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";
        var response = await _adminService.MakeUserAdminAsync(userId, currentAdminEmail);

        if (response.Succeeded)
            return Ok(response);

        return BadRequest(response);
    }
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("users/{userId:guid}/remove-admin")]
    [ProducesResponseType(typeof(BasicResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAdminRole(Guid userId)
    {
        var currentAdminIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (currentAdminIdClaim == null || !Guid.TryParse(currentAdminIdClaim, out var currentAdminId))
            return Unauthorized(new BasicResponse { Succeeded = false, Message = "Invalid admin user." });
        
        var response = await _adminService.RemoveAdminRoleAsync(userId,currentAdminId);

        if (response.Succeeded)
            return Ok(response);

        return BadRequest(response);
    }
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("users/{userId:guid}")]
    [ProducesResponseType(typeof(BasicResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        var currentAdminEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";
        var response = await _adminService.DeleteUserAsync(userId, currentAdminEmail);

        if (response.Succeeded)
            return Ok(response);

        return BadRequest(response);
    }
}










