using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.MAUI.Pages;
using ExpensesApp.MAUI.ViewModels;

namespace ExpensesApp.MAUI.PageModels;

public partial class TransactionPageModel : ObservableObject, IQueryAttributable
{
    private readonly AccountController _accountController;
    private readonly ExpenseController _expenseController;

    public ObservableCollection<Account> Accounts { get; } = new();
    public ObservableCollection<Expense> Expenses { get; } = new();

    [ObservableProperty] private Guid _accountId;

    [ObservableProperty] private Account _selectedAccountObject;

    public bool IsOverviewMode => SelectedAccountObject == null;
    public bool IsAccountSelected => AccountId != Guid.Empty;

    [ObservableProperty] public bool isBusy;

    public TransactionPageModel(AccountController accountController, ExpenseController expenseController)
    {
        _accountController = accountController;
        _expenseController = expenseController;
    }


    public async Task InitializeAsync()
    {
        IsBusy = true;
        try
        {
            var accountsList = await _accountController.GetAccountsAsync();
            Accounts.Clear();
            foreach (var account in accountsList) Accounts.Add(account);

            if (AccountId != Guid.Empty)
            {
                SelectedAccountObject = Accounts.FirstOrDefault(a => a.Id == AccountId);

                OnPropertyChanged(nameof(IsOverviewMode));
                OnPropertyChanged(nameof(IsAccountSelected));
                await LoadFilteredExpenses(AccountId);
            }
            else
            {
                SelectedAccountObject = null;
                OnPropertyChanged(nameof(IsOverviewMode));
                OnPropertyChanged(nameof(IsAccountSelected));
                await LoadAllExpenses();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task EditTransaction(Expense expense)
    {
        if (expense == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "ExpenseToEdit", expense }
        };
        
        await Shell.Current.GoToAsync("AddExpensePage", navigationParameter);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("accountId"))
        {
            var idString = query["accountId"].ToString();

            if (Guid.TryParse(idString, out Guid result))
            {
                AccountId = result;

                if (AccountId != Guid.Empty)
                {
                    _ = LoadFilteredExpenses(AccountId);

                    SelectedAccountObject = Accounts.FirstOrDefault(a => a.Id == AccountId);
                    OnPropertyChanged(nameof(IsOverviewMode));
                    OnPropertyChanged(nameof(IsAccountSelected));
                }
            }
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
    public async Task FilterByAccount(Account account)
    {
        if (account == null) return;

        SelectedAccountObject = account;
        AccountId = account.Id;
        await LoadFilteredExpenses(account.Id);

        OnPropertyChanged(nameof(IsAccountSelected));
        OnPropertyChanged(nameof(IsOverviewMode));
    }

    [RelayCommand]
    public async Task GoBackToOverview()
    {
        IsBusy = true;

        SelectedAccountObject = null;
        AccountId = Guid.Empty;

        OnPropertyChanged(nameof(IsAccountSelected));
        OnPropertyChanged(nameof(IsOverviewMode));

        Expenses.Clear();

        await InitializeAsync();

        IsBusy = false;
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
        IsBusy = true;
        try
        {
            var expensesList = await _expenseController.GetExpensesByAccountIdAsync(id);

            UpdateExpenseList(expensesList);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Could not load Expenses", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void UpdateExpenseList(IEnumerable<Expense> expensesList)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Expenses.Clear();
            foreach (var expense in expensesList)
            {
                Expenses.Add(expense);
            }
        });
    }
}