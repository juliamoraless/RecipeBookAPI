namespace Domain.Models;

public class User
{
     public Guid id { get; set; }
     public string userName { get; set; } = string.Empty;
     public string passwordHash { get; set; } 
}