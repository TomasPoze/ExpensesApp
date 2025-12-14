using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Validators;

namespace ExpensesApp.Core.Services;

public class AccountService
{
    private List<Account> _accounts;
    private readonly AccountValidator _accountValidator;
    private readonly IAccountRepository _repository;
    

    public AccountService(AccountValidator accountValidator, AccountRepository accountRepository, IAccountRepository repo)
    {
        _accountValidator = accountValidator;
        _repository = repo;
    }

    public async Task<List<Account>> GetAllAccountsAsync()
    {
        return await _repository.GetAccountAsync();
    }

    public async Task<Account?> GetAccountAsync(int id)
    {
        return await _repository.GetAccountAsync(id);
    }

    public async Task<(bool Success, string Message)> AddAccountAsync(Account account)
    {
        var validation = _accountValidator.ValidateAccount(account);
        if (!validation.Success)
            return (false, validation.Message);

        try
        {
            await _repository.AddAccountAsync(account);
            return (true, "Account created successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Error creating account: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Message)> UpdateAccountAsync(Account account)
    {
        var validation = _accountValidator.ValidateAccount(account);
        if (!validation.Success)
            return (false, validation.Message);

        try
        {
            await _repository.UpdateAccountAsync(account);
            return (true, "Account updated successfully");
        }catch(Exception ex)
        {
            return (false, $"Error updating account: {ex.Message}");
        }
    }
    
    public async Task<(bool Success, string Message)> DeleteAccountAsync(int id)
    {
        try
        {
            await _repository.DeleteAccountAsync(id);
            return (true, "Account deleted successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Error deleting account: {ex.Message}");
        }
    }
}