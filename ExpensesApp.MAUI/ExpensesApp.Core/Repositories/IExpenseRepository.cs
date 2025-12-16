using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Repositories;

public interface IExpenseRepository
{
    // GET: Get expenses (optionally filtered by account)
    Task<List<Expense>> GetExpensesAsync(int? accountId = null);

    // GET: Get one expense
    Task<Expense?> GetExpenseAsync(int id);

    // POST: Add new expense
    Task<Expense> AddExpenseAsync(Expense expense);

    // PUT: Update expense
    Task UpdateExpenseAsync(Expense expense);

    // DELETE: Delete expense
    Task DeleteExpenseAsync(int id);
}