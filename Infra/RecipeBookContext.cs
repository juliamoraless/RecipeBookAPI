using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class RecipeBookContext : DbContext
{
    public RecipeBookContext(DbContextOptions<RecipeBookContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}