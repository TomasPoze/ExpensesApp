using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _repository;
    private readonly ExpenseValidator _validator;


    public ExpenseService(IExpenseRepository repository, ExpenseValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<List<Expense>> GetExpensesAsync(Guid? accountId = null)
    {
        return await _repository.GetExpensesAsync(accountId);
    }
    
    public async Task<List<Expense>> GetCurrentMonthExpensesAsync(Guid? accountId = null)
    {
        if (accountId == Guid.Empty) return [];
        return await _repository.GetExpensesByCurrentMonthAsync(accountId);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByAccountIdAsync(Guid accountId)
    {
        if (accountId == Guid.Empty) return [];
        return await _repository.GetExpensesAsync(accountId);
    }

    public async Task<(bool Success, string Message)> AddExpenseAsync(Expense expense)
    {
        var validation = _validator.ValidateExpense(expense);
        if (!validation.Success) return (false, validation.Message);

        try
        {
            await _repository.AddExpenseAsync(expense);
            return (true, "Expense added successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Network error: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Message)> UpdateExpenseAsync(Expense expense)
    {
        var validation = _validator.ValidateExpense(expense);
        if (!validation.Success)
            return (false, validation.Message);

        try
        {
            await _repository.UpdateExpenseAsync(expense);
            return (true, "Expense updated successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Error updating expense: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Message)> DeleteExpenseAsync(Guid id)
    {
        try
        {
            await _repository.DeleteExpenseAsync(id);
            return (true, "Expense deleted successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Error deleting expense: {ex.Message}");
        }
    }

    public (decimal Sum, decimal Average, int Count) GetStatisticsSummary(List<Expense> expenses)
    {
        if (expenses == null || expenses.Count == 0)
            return (0, 0, 0);

        return (
            expenses.Sum(e => e.Amount),
            expenses.Average(e => e.Amount),
            expenses.Count
        );
    }

    public List<Expense> GetExpensesSortedByAmount(List<Expense> expenses, bool descending)
    {
        return descending
            ? expenses.OrderByDescending(e => e.Amount).ToList()
            : expenses.OrderBy(e => e.Amount).ToList();
    }

    public (IEnumerable<(int Year, int Month, string MonthName, decimal Total)> ByMonth,
        IEnumerable<(int Year, decimal Total)> ByYear) GetMonthlyStatistics(List<Expense> expenses)
    {
        var byMonth = expenses.GroupBy(e => new { e.Date.Year, e.Date.Month })
            .Select(f => (
                Year: f.Key.Year,
                Month: f.Key.Month,
                MonthName: new DateTime(f.Key.Year, f.Key.Month, 1).ToString("MMMM"),
                Total: f.Sum(x => x.Amount)
            )).OrderBy(x => x.Year).ThenBy(x => x.Month);

        var byYear = expenses.GroupBy(e => new { e.Date.Year })
            .Select(e => (
                Year: e.Key.Year,
                Total: e.Sum(x => x.Amount)))
            .OrderBy(x => x.Year);

        return (byMonth, byYear);
    }

    public IEnumerable<(string Category, decimal Sum)> GetTotalByCategory(List<Expense> expenses)
    {
        return expenses
            .GroupBy(e => e.Category)
            .Select(f => (Category: f.Key, Sum: f.Sum(x => x.Amount)));
    }
}