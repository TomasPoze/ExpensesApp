using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.MAUI.Pages;

namespace ExpensesApp.MAUI.ViewModels;

public partial class AccountSectionViewModel : ObservableObject
{
    private readonly AccountController _accountController;

    [ObservableProperty] private ObservableCollection<Account> _accounts;

    public AccountSectionViewModel(AccountController accountController)
    {
        _accountController = accountController;
        Accounts = new ObservableCollection<Account>();
    }
    
    [RelayCommand]
    private async Task GoToAddExpenseAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddExpensePage));
    }

    [RelayCommand]
    private async Task AddAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddAccountPage));
    }


    public async Task LoadAccountsAsync()
    {
        try
        {
            var accountList = await _accountController.GetAccountsAsync();
            Accounts = new ObservableCollection<Account>(accountList);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading accounts: {ex.Message}");
        }
    }
    
    public async Task OnAccountTap(Account account)
    {
        await Shell.Current.GoToAsync($"{nameof(TransactionPage)}?accountId={account.Id}");
    }
}