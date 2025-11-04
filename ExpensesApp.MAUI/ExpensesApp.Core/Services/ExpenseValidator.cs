namespace ExpensesApp.Core.Services;

public class ExpenseValidator
{
    public (bool Success, string Message, int? Id) ValidateId(string idInput)
    {
        if (string.IsNullOrWhiteSpace(idInput))
            return (false, "ID cannot be empty", null);

        if (!int.TryParse(idInput, out int id))
            return (false, "ID must be an number", null);

        if (id <= 0)
            return (false, "ID must be greater than zero", null);

        return (true, "Valid ID format.", id);
    }

    public (bool Success, bool IsSkipped, string Message, string? Amount) ValidateAmount(string input)
    {
        if (input.Contains(','))
            input = input.Replace(",", ".");
        
        if (string.IsNullOrEmpty(input))
            return (true, true, "Skipper - using previous amount.", null);
        
        if (!decimal.TryParse(input, out var amount) || amount <= 0)
            return (false, false, "Invalid amount", null);
        
        return (true, false, "OK", input);
    }

    public (bool Success, string Message) ValidateCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return (false, "Category cannot be empty.");
        return (true, "OK");
    }

    public (bool Success, string Message) ValidateDescription(string description)
    {
        if (description.Length > 50)
            return (false, "Description cannot be longer than 50 characters.");
        return (true, "OK");
    }

    /*public (bool Success, string Message) ValidateExpense(string category, string amountInput, string description)
    {
        var categoryCheck = ValidateCategory(category);
        if (!categoryCheck.Success)
            return categoryCheck;

        var amountCheck = ValidateAmount(amountInput);
        if (!amountCheck.Success)
            return amountCheck;

        var descriptionCheck = ValidateDescription(description);
        if (!descriptionCheck.Success)
            return descriptionCheck;
        return (true, "OK");
    }*/
}