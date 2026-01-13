using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Repositories;

public interface IExpenseRepository
{
    // GET: Get expenses (optionally filtered by account)
    Task<List<Expense>> GetExpensesAsync(Guid? accountId = null);
    
    // GET: Get expenses by Month
    Task<List<Expense>> GetExpensesByCurrentMonthAsync(Guid? accountId);

    // GET: Get one expense
    //Task<Expense?> GetExpenseAsync(Guid id);
    
    // POST: Add new expense
    Task<Expense> AddExpenseAsync(Expense expense);

    // PUT: Update expense
    Task UpdateExpenseAsync(Expense expense);

    // DELETE: Delete expense
    Task DeleteExpenseAsync(Guid id);
}