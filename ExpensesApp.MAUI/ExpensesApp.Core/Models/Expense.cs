using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ExpensesApp.Core.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }

    // Foreign key to Account
    public int AccountId { get; set; }

    // Navigation property to Account
    [ForeignKey("AccountId")]
    public Account? Account { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;

    // EF Core requires parameterless constructor
    public Expense()
    {
    }

    public Expense(DateTime date, string category, decimal amount, string description)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty");
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");
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
        Date = DateTime.UtcNow;
        Description = "No description";
    }

    public string PrintInfo()
    {
        CultureInfo ltCulture = new CultureInfo("lt-LT");
        return $"ID:{Id} Date: {Date:d}, Category: {Category}, Amount:{Amount:C}, Description: {Description}";
    }
}