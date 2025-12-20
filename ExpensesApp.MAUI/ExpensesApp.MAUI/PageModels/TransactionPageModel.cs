using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.MAUI.Pages;
using ExpensesApp.MAUI.ViewModels;

namespace ExpensesApp.MAUI.PageModels;

[QueryProperty(nameof(AccountId), "accountId")]
public partial class TransactionPageModel : ObservableObject
{
    private readonly AccountController _accountController;
    private readonly ExpenseController _expenseController;

    public ObservableCollection<Account> Accounts { get; } = new();
    public ObservableCollection<Expense> Expenses { get; } = new();

    [ObservableProperty]
    private Guid _accountId;

    [ObservableProperty] private Account _selectedAccountObject;

    public bool IsOverviewMode => SelectedAccountObject == null;
    public bool IsAccountSelected => AccountId != Guid.Empty;

    public TransactionPageModel(AccountController accountController, ExpenseController expenseController)
    {
        _accountController = accountController;
        _expenseController = expenseController;
    }


    public async Task InitializeAsync()
    {
        var accountsList = await _accountController.GetAccountsAsync();
        Accounts.Clear();
        foreach (var account in accountsList) Accounts.Add(account);

        if (AccountId != Guid.Empty)
        {
            await LoadFilteredExpenses(AccountId);
        }
        else
        {
            await LoadAllExpenses();
        }
    }

    async partial void OnAccountIdChanged(Guid value)
    {
        if (value != Guid.Empty)
        {
            await LoadFilteredExpenses(value);
        }
    }

    [RelayCommand]
    public async Task FilterByAccount(Account selectedAccount)
    {
        if (selectedAccount == null) return;
        
        SelectedAccountObject = selectedAccount;
        AccountId = selectedAccount.Id;
        //await LoadFilteredExpenses(selectedAccount.Id);
        
        OnPropertyChanged(nameof(IsAccountSelected));
        OnPropertyChanged(nameof(IsOverviewMode));
    }
    
    [RelayCommand]
    public void GoBackToOverview()
    {
        SelectedAccountObject = null;
        AccountId = Guid.Empty;
        
        OnPropertyChanged(nameof(IsAccountSelected));
        OnPropertyChanged(nameof(IsOverviewMode));
    }

    [RelayCommand]
    public async Task GoToAddExpenseAsync()
    {
        if (!IsAccountSelected) return;

        await Shell.Current.GoToAsync($"{nameof(AddExpensePage)}?accountId={AccountId}");
    }

    [RelayCommand]
    public async Task GoToAddAccountAsync()
    {
        // Navigate to the Add Account Page (Modal)
        await Shell.Current.GoToAsync(nameof(AddAccountPage));
    }
    
    private async Task LoadAllExpenses()
    {
        var expensesList = await _expenseController.GetExpensesAsync();
        UpdateExpenseList(expensesList);
    }

    private async Task LoadFilteredExpenses(Guid id)
    {
        var expensesList = await _expenseController.GetExpensesByAccountIdAsync(id);
        UpdateExpenseList(expensesList);
    }

    private void UpdateExpenseList(IEnumerable<Expense> expensesList)
    {
        Expenses.Clear();
        foreach (var expense in expensesList) Expenses.Add(expense);
    }
}