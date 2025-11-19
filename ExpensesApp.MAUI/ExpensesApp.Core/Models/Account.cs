namespace ExpensesApp.Core.Models;

public class Account
{
    public static int _counter = 1;
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public AccountType Type { get; set; } = AccountType.Bank;
    public Currency Currency { get; set; } = Currency.EUR;
    public decimal Balance { get; set; } = 0;
    public decimal? MonthlyIncome { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public List<Expense> Expenses { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    
    public Account() {}
    
    public Account(string accountName, Currency currency, decimal balance, decimal monthlyIncome)
    {
        
        Id = _counter++;
        AccountName = accountName;
        Currency = currency;
        Balance = balance;
        MonthlyIncome = monthlyIncome;
        CreatedAt = DateTime.Now;
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