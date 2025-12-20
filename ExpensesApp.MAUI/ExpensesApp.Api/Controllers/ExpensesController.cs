using ExpensesApp.Api.Data;
using ExpensesApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public ExpensesController(ExpensesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses([FromQuery] Guid? accountId)
    {
        var query = _context.Expenses.AsQueryable();

        if (accountId.HasValue)
        {
            query = query.Where(e => e.AccountId == accountId.Value);
        }

        return await query.OrderByDescending(e => e.Date).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null)
        {
            return NotFound();
        }

        return expense;
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
    {
        // Verify account exists
        var account = await _context.Accounts.FindAsync(expense.AccountId);
        if (account == null)
        {
            return BadRequest("Account not found");
        }

        expense.Date = DateTime.UtcNow;

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, Expense expense)
    {
        if (id != expense.Id)
        {
            return BadRequest();
        }

        var existingExpense = await _context.Expenses.FindAsync(id);
        if (existingExpense == null)
        {
            return NotFound();
        }

        existingExpense.Category = expense.Category;
        existingExpense.Amount = expense.Amount;
        existingExpense.Description = expense.Description;
        existingExpense.Date = expense.Date;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
        {
            return NotFound();
        }

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetExpensesSummary([FromQuery] Guid? accountId)
    {
        var query = _context.Expenses.AsQueryable();

        if (accountId.HasValue)
        {
            query = query.Where(e => e.AccountId == accountId.Value);
        }

        var expenses = await query.ToListAsync();

        var summary = new
        {
            TotalCount = expenses.Count,
            TotalAmount = expenses.Sum(e => e.Amount),
            AverageAmount = expenses.Count > 0 ? expenses.Average(e => e.Amount) : 0,
            ByCategory = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderByDescending(x => x.Total)
        };

        return summary;
    }
}
