using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookVerse.Infrastructure.Services;

public class AuthTokenProcessorService : IAuthTokenProcessor
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtOptions _jwtOptions;

    public AuthTokenProcessorService(IOptions<JwtOptions> jwtOptions,IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtOptions = jwtOptions.Value;
    }

    public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.ToString())
        }.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        var expires = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return (jwtToken, expires);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}