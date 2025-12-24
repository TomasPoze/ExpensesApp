using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using Microsoft.Maui.Graphics.Text;

namespace ExpensesApp.MAUI.PageModels;

public partial class AddAccountPageModel : ObservableObject
{
    private readonly AccountController _accountController;

    public AddAccountPageModel(AccountController accountController)
    {
        _accountController = accountController;
    }

    [ObservableProperty] private string _name;
    [ObservableProperty] private string _currency; // pvz. "EUR"
    [ObservableProperty] private decimal _balance;
    [ObservableProperty] private decimal _monthlyIncome;


    [RelayCommand]
    private async Task Save()
    {
        var result = await _accountController.AddAccountAsync(new Account
        {
            AccountName = Name,
            Currency = Enum.Parse<Currency>(Currency),
            Balance = Balance,
            MonthlyIncome = MonthlyIncome,
            //UserId = Guid.Parse("3b4fdc11-e7ae-4a01-93e6-cce7ce5dd31c")
            UserId = Guid.Parse("cc486a5a-3ccf-40b3-888d-2c28e36bf54e")
            
        });
        if (result.Success)
            await Shell.Current.GoToAsync("..");
        else
            await Shell.Current.DisplayAlert(result.Message, result.Message, "OK");
    }
    
    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}