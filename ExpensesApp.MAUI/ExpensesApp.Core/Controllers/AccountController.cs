using ExpensesApp.Core.Models;
using ExpensesApp.Core.Services;
using ExpensesApp.Core.Validators;

namespace ExpensesApp.Core.Controllers;

public class AccountController
{
    private readonly AccountService _service;
    private readonly AccountValidator _accountValidator;

    public AccountController(AccountService service, AccountValidator accountValidator)
    {
        _service = service;
        _accountValidator = accountValidator;
    }

    public (bool Success, string Message) AddAccount(string name, string currency, decimal balance,
        decimal monthlyIncome)
    {
        var valAccName = _accountValidator.ValidateName(name);
        var valCurrency = _accountValidator.ValidateCurrency(currency);
        var valBalance = _accountValidator.ValidateBalance(balance);
        var valIncome = _accountValidator.ValidateMonthlyIncome(monthlyIncome);
        
        if (!valAccName.Success || !valCurrency.Success || !valBalance.Success || !valIncome.Success)
            return (false, "Bad input, account not created");

        if (!Enum.TryParse(currency, true, out Currency parsedCurrency))
            return (false, "Invalid currency type");

        _service.AddAccount(new Account(name, parsedCurrency, balance, monthlyIncome));
        return (true, "Account created");
    }

    public List<Account> GetAllAccounts()
    {
        return _service.GetAllAccounts();
    }

    public (bool Success, string) UpdateAccount(Account accountUpdated)
    {
        return _service.UpdateAccount(accountUpdated);
    }

    public (bool Success, string Message) DeleteAccount(int id)
    {
        return _service.DeleteAccount(id);
    }
}