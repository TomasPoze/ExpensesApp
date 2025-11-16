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

    public AccountService(AccountValidator accountValidator, AccountRepository accountRepository)
    {
        _accountValidator = accountValidator;
        _accountRepository = accountRepository;
        var (accounts, errors) = _accountRepository.LoadFromFile();
        _accounts = accounts;
    }

    public (bool Success, string Message) AddAccount(Account account)
    {
        var validation = _accountValidator.ValidateAccount((account));
        if (!validation.Success)
            return (false, validation.Message);
        _accounts.Add(account);
        try
        {
            _accountRepository.SaveToFile(_accounts);
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
        _accountRepository.SaveToFile(_accounts);
        return (true, "Account updated successfully");
    }

    public (bool Success, string Message) DeleteAccount(int id)
    {
        var acc = _accounts.FirstOrDefault(x => x.Id == id);
        if (acc == null)
            return (false, "Account not found");
        _accounts.Remove(acc);
        _accountRepository.SaveToFile(_accounts);
        return (true, "Account deleted successfully");
    }
}