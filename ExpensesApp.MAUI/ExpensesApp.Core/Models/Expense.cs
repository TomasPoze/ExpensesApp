using System.Globalization;

namespace ExpensesApp.Core.Models;

public class Expense
{
    private static int _counter = 1;
    
    public int Id { get; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    
    

    public Expense(DateTime date, string category, decimal amount, string description)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty");
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");
        Id = _counter++;
        Date = date;
        Category = category;
        Amount = amount;
        Description = description;
    }

    public Expense(string category, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty");
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        Category = category;
        Amount = amount;
        Date = DateTime.Now;
        Description = "No description";
    }

    public string PrintInfo()
    {
        CultureInfo ltCulture = new CultureInfo("lt-LT");
        return $"ID:{Id} Date: {Date:d}, Category: {Category}, Amount:{Amount:C}, Description: {Description}";
    }
}