using Domain.Models;
using Domain.ViewModels;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserViewModel request);
    Task<string?> LoginAsync(UserViewModel request);
}