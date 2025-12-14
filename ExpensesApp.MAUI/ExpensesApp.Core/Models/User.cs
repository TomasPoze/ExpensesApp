using System.ComponentModel.DataAnnotations;

namespace ExpensesApp.Core.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string? GoogleId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property - User has many Accounts
    public List<Account> Accounts { get; set; } = new();

    public User()
    {
    }
}