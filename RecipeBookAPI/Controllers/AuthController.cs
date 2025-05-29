using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace RecipeBookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    public static User user = new();
    private readonly  IAuthService _authService;

    public AuthController (IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserViewModel request)
    {
        var user = await _authService.RegisterAsync(request);
        if (user is null)
            return BadRequest("Username already exists");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserViewModel request)
    {
        var token = await _authService.LoginAsync(request);
        if (token is null)
            return BadRequest("Invalid username or password");

        return Ok(token);
    }
}