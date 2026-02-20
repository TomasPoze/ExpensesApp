using ExpensesApp.Core.Models;
using ExpensesApp.Core.Services;
using ExpensesApp.Core.Validators;

namespace ExpensesApp.Core.Controllers;

public class AccountController
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        // Call the new Async method
        return await _service.GetAllAccountsAsync();
    }

    public async Task<(bool Success, string Message)> AddAccountAsync(Account account)
    {
        // Call the new Async method
        return await _service.AddAccountAsync(account);
    }

    public async Task<(bool Success, string Message)> UpdateAccountAsync(Account account)
    {
        return await _service.UpdateAccountAsync(account);
    }

    public async Task<(bool Success, string Message)> DeleteAccountAsync(Guid id)
    {
        return await _service.DeleteAccountAsync(id);
    }
}