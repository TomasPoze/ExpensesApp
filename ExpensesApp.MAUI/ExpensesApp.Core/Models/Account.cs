using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesApp.Core.Models;

public class Account
{
    [Key]
    public Guid Id { get; set; }

    // Foreign key to User
    public Guid UserId { get; set; }

    // Navigation property to User
    [ForeignKey("UserId")]
    public User? User { get; set; }

    public string AccountName { get; set; } = string.Empty;
    public AccountType Type { get; set; } = AccountType.Bank;
    public Currency Currency { get; set; } = Currency.EUR;

    public decimal Balance { get; set; } = 0;
    public decimal? MonthlyIncome { get; set; }

    // Navigation properties - Account has many Expenses and Transactions
    public List<Expense> Expenses { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Account()
    {
    }

    public Account(string accountName, Currency currency, decimal balance, decimal monthlyIncome)
    {
        AccountName = accountName;
        Currency = currency;
        Balance = balance;
        MonthlyIncome = monthlyIncome;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum Currency
{
    EUR,
    USD,
    GBP
};

public enum AccountType
{
    Cash,
    Bank,
    Card
}