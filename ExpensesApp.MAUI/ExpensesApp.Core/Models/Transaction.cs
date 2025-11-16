namespace ExpensesApp.Core.Models;

public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account _account;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime OccuredAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionType Type { get; set; }

    public Transaction(int accountId, string accountName, Expense expense)
    {
        AccountId = accountId;
    }
}

public enum TransactionType
{
    Income,
    Expense
}