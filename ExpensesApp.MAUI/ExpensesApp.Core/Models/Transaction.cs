using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesApp.Core.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    // Foreign key to Account
    public int AccountId { get; set; }

    // Navigation property to Account
    [ForeignKey("AccountId")]
    public Account? Account { get; set; }

    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime OccuredAt { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
    public TransactionType Type { get; set; }

    public Transaction()
    {
    }

    public Transaction(int accountId, string category, decimal amount, TransactionType type, string description = "")
    {
        AccountId = accountId;
        Category = category;
        Amount = amount;
        Type = type;
        Description = description;
        OccuredAt = DateTime.UtcNow;
    }
}

public enum TransactionType
{
    Income,
    Expense
}