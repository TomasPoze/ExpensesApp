using System.ComponentModel.DataAnnotations;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Services;

namespace ExpensesApp.Core.Controllers;

public class ExpenseController
{
    private readonly ExpenseService _service;
    private readonly ExpenseValidator _validator;

    public ExpenseController(ExpenseService service, ExpenseValidator validator)
    {
        _service = service;
        _validator = validator;
    }

    public (bool Success, string Message) ExportToCsv()
    {
        return _service.ExportToCsv();
    }

    public (bool Success, string Message) SaveToFile()
    {
        _service.SaveToFile();
        return (true, "Expenses saved");
    }

    public (bool Success, List<string> Errors) LoadFromFile()
    {
        var (expenses,errors)=_service.LoadFromFile();
        if(errors.Any())
            return (true, errors);
        if(errors.Count == expenses.Count)
            return (false, errors);
        return (true, new List<string>());
        
    }

    public void AddExpense(string category, string amountInput, string description)
    {
        decimal.TryParse(amountInput, out var amount);
        var expense = new Expense(DateTime.Now, category, amount, description);
        _service.AddExpenses(expense);
    }

    public (bool Success, string Message) EditExpense(int id, string category, string amountInput, string description)
    {
        bool edited = _service.EditExpense(id, category, amountInput, description);
        return edited
            ? (true, "Expense edited successfully!")
            : (false, "Could not edit expense!");
    }

    public (bool Success, string Message) RemoveExpense(string id)
    {
        var idValid = CheckIfExpenseExists(id);
        if (!idValid.Success)
        {
            return (false, idValid.Message);
        }

        _service.RemoveExpense(id);
        return (true, "Expense removed successfully!");
    }

    public List<Expense> GetAllExpenses()
    {
        return _service.GetAllExpenses();
    }

    public (IEnumerable<(int Year, int Month, string MonthName, decimal Total)> ByMonth,
        IEnumerable<(int Year, decimal Total)> ByYear ) GetMonthlyStatistics()
    {
        return _service.GetMonthlyStatistics();
    }

    public (decimal Sum, decimal Average, int Count) GetStatisticsSummary()
    {
        return _service.GetStatisticsSummary();
    }

    public (bool Success, string Message) GetMostExpensiveExpense()
    {
        var expense = _service.GetMostExpensiveExpense();
        return expense == null
            ? (false, "No expenses found.")
            : (true, expense.PrintInfo());
    }

    public IEnumerable<(string Category, decimal Sum)> GetTotalByCategory()
    {
        return _service.GetTotalByCategory();
    }

    public (bool Success, string Message, int? Id) CheckIfExpenseExists(string id)
    {
        var validate = _validator.ValidateId(id);
        bool exists = _service.GetAllExpenses().Any(e => e.Id == validate.Id);

        if (!exists)
            return (false, $"Expense with ID {id} was not found.", null);

        return (true, "Expense Exists.", validate.Id);
    }

    public (bool Success, string Message) ValidateCategory(string category) => _validator.ValidateCategory(category);

    public (bool Success, bool IsSkipped, string Message, string? Amount) ValidateAmount(string amount)
    {
        return _validator.ValidateAmount(amount);
    }

    public (bool Success, string Message) ValidateDescription(string description) =>
        _validator.ValidateDescription(description);
}