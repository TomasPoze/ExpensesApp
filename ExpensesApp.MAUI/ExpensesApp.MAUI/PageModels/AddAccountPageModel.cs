using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;

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
        /*var result = _accountController.AddAccount(_name, _currency, _balance, _monthlyIncome);
        if (result.Success)
            await Shell.Current.GoToAsync("..");
        else
            await Shell.Current.DisplayAlert(result.Message, result.Message, "OK");*/
    }
}