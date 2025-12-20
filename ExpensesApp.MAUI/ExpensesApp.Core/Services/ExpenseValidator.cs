using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Services;

public class ExpenseValidator
{
    public (bool Success, string Message) ValidateExpense(Expense expense)
    {
        if (expense == null)
            return (false, "Expense cannot be null.");

        var catCheck = ValidateCategory(expense.Category);
        if (!catCheck.Success) return catCheck;
        
        var amountCheck = ValidateAmount(expense.Amount);
        if (!amountCheck.Success) return amountCheck;

        var descCheck = ValidateDescription(expense.Description);
        if (!descCheck.Success) return descCheck;

        return (true, "OK");
    }

    public (bool Success, string Message) ValidateCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return (false, "Category cannot be empty.");

        if (category.Length > 30)
            return (false, "Category name is too long (max 30 chars).");

        return (true, "OK");
    }

    public (bool Success, string Message) ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            return (false, "Amount must be greater than zero.");

        if (amount > 10000000)
            return (false, "Amount is suspiciously large.");

        return (true, "OK");
    }

    public (bool Success, string Message) ValidateDescription(string description)
    {
        if (description != null && description.Length > 100)
            return (false, "Description cannot be longer than 100 characters.");

        return (true, "OK");
    }

    public (bool Success, string Message) ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            return (false, "Invalid ID.");
        return (true, "OK");
    }
}