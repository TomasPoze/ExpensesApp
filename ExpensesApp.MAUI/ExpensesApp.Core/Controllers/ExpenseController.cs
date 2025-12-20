using System.ComponentModel.DataAnnotations;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Services;

namespace ExpensesApp.Core.Controllers;

public class ExpenseController
{
    private readonly ExpenseService _service;

    public ExpenseController(ExpenseService service)
    {
        _service = service;
    }

    public async Task<List<Expense>> GetExpensesAsync()
    {
        return await _service.GetExpensesAsync();
    }

    public async Task<(bool Success, string Message)> AddExpenseAsync(Expense expense)
    {
        return await _service.AddExpenseAsync(expense);
    }

    public async Task<(bool Success, string Message)> EditExpenseAsync(Expense expense)
    {
        return await _service.UpdateExpenseAsync(expense);
    }

    public async Task<(bool Success, string Message)> RemoveExpenseAsync(Guid id)
    {
        return await _service.DeleteExpenseAsync(id);
    }


    public async Task<(decimal Sum, decimal Average, int Count)> GetStatisticsSummaryAsync()
    {
        var expenses = await _service.GetExpensesAsync();

        return _service.GetStatisticsSummary(expenses);
    }

    public async Task<(IEnumerable<(int Year, int Month, string MonthName, decimal Total)> ByMonth,
        IEnumerable<(int Year, decimal Total)> ByYear)> GetMonthlyStatisticsAsync()
    {
        var expenses = await _service.GetExpensesAsync();
        return _service.GetMonthlyStatistics(expenses);
    }

    public async Task<IEnumerable<(string Category, decimal Sum)>> GetTotalByCategoryAsync()
    {
        var expenses = await _service.GetExpensesAsync();
        return _service.GetTotalByCategory(expenses);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByAccountIdAsync(Guid accountId)
    {
        if (accountId == Guid.Empty) return [];
        return await _service.GetExpensesByAccountIdAsync(accountId);
    }
}