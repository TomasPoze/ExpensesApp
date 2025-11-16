using System.Runtime.InteropServices.JavaScript;
using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Validators;

public class AccountValidator
{
    public (bool Success, string Message) ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Name can't be empty");
        if (name.Length < 2 || name.Length > 50)
            return (false, "Name can't be shorter than 2 symbols or longer than 50");

        return (true, "Ok");
    }


    public (bool Success, string Message, Enum? currency) ValidateCurrency(string currency)
    {
        if (!Enum.TryParse<Currency>(currency, out var curr))
            return (false, "Invalid currency", null);
        
        return (true, "OK",  curr);
    }

    public (bool Success, string Message) ValidateBalance(decimal balance)
    {
        if (balance >= 0)
            return (true, "OK");
        return (false, "Invalid balance");
    }

    public (bool Success, string Message) ValidateMonthlyIncome(decimal? income)
    {
        if (income >= 0)
            return (true, "OK");
        return (false, "Invalid income");
    }

    public (bool Success, string Message) ValidateAccount(Account account)
    {
        var name = ValidateName(account.AccountName);
        var currency = ValidateCurrency(account.Currency.ToString());
        var balance = ValidateBalance(account.Balance);
        var income = ValidateMonthlyIncome(account.MonthlyIncome);

        if (!name.Success || !balance.Success || !income.Success || !currency.Success)
            return (false, $"Account name {account.AccountName} does not exist");

        return (true, "OK");
    }
}