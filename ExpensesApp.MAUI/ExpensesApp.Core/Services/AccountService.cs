using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Validators;

namespace ExpensesApp.Core.Services;

public class AccountService
{
    private List<Account> _accounts;
    private AccountValidator _accountValidator;
    private readonly AccountRepository _accountRepository;
    private readonly IAccountRepository _repo;
    

    public AccountService(AccountValidator accountValidator, AccountRepository accountRepository, IAccountRepository repo)
    {
        _accountValidator = accountValidator;
        _accountRepository = accountRepository;
        _repo = repo;
        _accounts = _repo.Load();
        
        Debug.WriteLine("Loading accounts JSON...");

        var (acc, errors) = _accountRepository.LoadFromFile();

        if (acc == null)
        {
            Debug.WriteLine("CRITICAL: accounts == null");
        }

        Debug.WriteLine("Loaded count: " + acc.Count);
        if (errors.Count > 0)
        {
            Debug.WriteLine("Errors loading accounts:");
            foreach (var e in errors) Debug.WriteLine(e);
        }

    }

    public (bool Success, string Message) AddExpenseToAccount(int accountId, Expense expense)
    {
        var acc = _accounts.FirstOrDefault(x => x.Id == accountId);
        if (acc == null)
            return (false, "Account not found");
        
        acc.Expenses.Add(expense);
        acc.UpdatedAt = DateTime.Now;
        _repo.Save(_accounts);
        return (true, "Expense added");
    }
    
    public (bool Success, string Message) AddAccount(Account account)
    {
        var validation = _accountValidator.ValidateAccount((account));
        if (!validation.Success)
            return (false, validation.Message);
        _accounts.Add(account);
        try
        {
            _repo.Save(_accounts);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        return (true, "Added successfully");
    }

    public List<Account> GetAllAccounts()
    {
        return _accounts.ToList();
    }

    public Account? GetAccountById(int id)
    {
        return _accounts.FirstOrDefault(x => x.Id == id);
    }

    public (bool Success, string Message) UpdateAccount(Account updatedAccount)
    {
        var acc = _accounts.FirstOrDefault(x => x.Id == updatedAccount.Id);
        if (acc == null)
            return (false, "Account not found");
        var validation = _accountValidator.ValidateAccount((updatedAccount));
        if (!validation.Success)
        {
            return (false, validation.Message);
        }

        acc.AccountName = updatedAccount.AccountName;
        acc.MonthlyIncome = updatedAccount.MonthlyIncome;
        acc.UserId = updatedAccount.UserId;
        acc.Currency = updatedAccount.Currency;
        acc.Balance = updatedAccount.Balance;
        acc.UpdatedAt = DateTime.Now;
        _repo.Save(_accounts);
        return (true, "Account updated successfully");
    }

    public (bool Success, string Message) DeleteAccount(int id)
    {
        var acc = _accounts.FirstOrDefault(x => x.Id == id);
        if (acc == null)
            return (false, "Account not found");
        _accounts.Remove(acc);
        _repo.Save(_accounts);
        return (true, "Account deleted successfully");
    }
}