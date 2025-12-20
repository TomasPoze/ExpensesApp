using ExpensesApp.Api.Data;
using ExpensesApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public TransactionsController(ExpensesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(
        [FromQuery] Guid? accountId,
        [FromQuery] TransactionType? type)
    {
        var query = _context.Transactions.AsQueryable();

        if (accountId.HasValue)
        {
            query = query.Where(t => t.AccountId == accountId.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(t => t.Type == type.Value);
        }

        return await query.OrderByDescending(t => t.OccuredAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        return transaction;
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        // Verify account exists
        var account = await _context.Accounts.FindAsync(transaction.AccountId);
        if (account == null)
        {
            return BadRequest("Account not found");
        }

        transaction.OccuredAt = DateTime.UtcNow;

        // Update account balance based on transaction type
        if (transaction.Type == TransactionType.Income)
        {
            account.Balance += transaction.Amount;
        }
        else
        {
            account.Balance -= transaction.Amount;
        }

        account.UpdatedAt = DateTime.UtcNow;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest();
        }

        var existingTransaction = await _context.Transactions.FindAsync(id);
        if (existingTransaction == null)
        {
            return NotFound();
        }

        existingTransaction.Category = transaction.Category;
        existingTransaction.Amount = transaction.Amount;
        existingTransaction.Description = transaction.Description;
        existingTransaction.Type = transaction.Type;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        // Reverse the balance change
        var account = await _context.Accounts.FindAsync(transaction.AccountId);
        if (account != null)
        {
            if (transaction.Type == TransactionType.Income)
            {
                account.Balance -= transaction.Amount;
            }
            else
            {
                account.Balance += transaction.Amount;
            }
            account.UpdatedAt = DateTime.UtcNow;
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetTransactionsSummary([FromQuery] Guid? accountId)
    {
        var query = _context.Transactions.AsQueryable();

        if (accountId.HasValue)
        {
            query = query.Where(t => t.AccountId == accountId.Value);
        }

        var transactions = await query.ToListAsync();

        var summary = new
        {
            TotalCount = transactions.Count,
            TotalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
            TotalExpenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
            NetAmount = transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount),
            ByCategory = transactions
                .GroupBy(t => t.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                    Expenses = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
                })
        };

        return summary;
    }
}
