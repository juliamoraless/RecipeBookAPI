using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Domain.Models;
using Domain.ViewModels;
using Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services;

public class AuthService: IAuthService
{
    private readonly RecipeBookContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(RecipeBookContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    public async Task<User?> RegisterAsync(UserViewModel request)
    {
        if (await _context.Users.AnyAsync(u => u.userName == request.userName))
            return null;

        var user = new User();
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, request.password);

        user.userName = request.userName;
        user.passwordHash = hashedPassword;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<string?> LoginAsync(UserViewModel request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.userName == request.userName);
        
        if (user is null)
            return null;

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.passwordHash, request.password) == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return CreateToken(user);
        
    }
    
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
            audience: _configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}