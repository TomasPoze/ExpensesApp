using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Services;
using ExpensesApp.MAUI.Pages;
using ExpensesApp.MAUI.ViewModels;


namespace ExpensesApp.MAUI.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly ExpenseController _expenseController;

    [ObservableProperty] public ObservableCollection<Expense> _expenses = new();

    [ObservableProperty] private AccountSectionViewModel _accountSection;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SpentPercentage))]
    [NotifyPropertyChangedFor(nameof(IncomeSpentText))]
    private decimal _spentThisMonth;

    [ObservableProperty] public string _currentMonthYear; 

    //[ObservableProperty] private ObservableCollection<SpendingCategory> _spendingCategories;

    public MainPageModel(AccountSectionViewModel accountSectionVM, ExpenseController expenseController)
    {
        AccountSection = accountSectionVM;
        _expenseController = expenseController;

        CurrentMonthYear = DateTime.Now.ToString("MMMM yyyy");

        AccountSection.PropertyChanged += OnAccountSectionPropertyChanged;
    }

    private void OnAccountSectionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AccountSectionViewModel.Accounts))
        {
            OnPropertyChanged(nameof(TotalMonthlyIncome));
            OnPropertyChanged(nameof(SpentPercentage));
            OnPropertyChanged(nameof(IncomeSpentText));
        }
    }

    public decimal TotalMonthlyIncome => AccountSection.Accounts?.Sum(a => a.MonthlyIncome) ?? 0;

    public double SpentPercentage =>
        TotalMonthlyIncome == 0 ? 0 : Math.Round((double)(SpentThisMonth / TotalMonthlyIncome) * 100, 2);

    public string IncomeSpentText => $"{SpentThisMonth:F2} € / {TotalMonthlyIncome:F2} €";

    public async Task RefreshAccountsAsync()
    {
        // Load Accounts (Headers, Income, Balance)
        var accountsTask = AccountSection.LoadAccountsAsync();

        // Load Expenses (For the "Spent This Month" calculation)
        var expensesTask = CalculateSpendingAsync();

        // Run both in parallel for speed
        await Task.WhenAll(accountsTask, expensesTask);
    }

    private async Task CalculateSpendingAsync()
    {
        try
        {
            // Fetch ALL expenses (or create a specific API method for "This Month" to be faster)

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var allExpenses = await _expenseController.GetExpensesByCurrentMonthAsync();
            // Filter locally
            SpentThisMonth = allExpenses
                .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                .Sum(e => e.Amount);

            Expenses = new ObservableCollection<Expense>(allExpenses);
        }
        catch (Exception ex)
        {
            // Handle error (maybe set to 0)
            SpentThisMonth = 0;
            Expenses = new ObservableCollection<Expense>();
        }
    }

    [RelayCommand]
    public async Task GoToSpending()
    {
        await Shell.Current.GoToAsync("///Spending");
    }
    
    [RelayCommand]
    public async Task GoToAccountTransactions(Account account)
    {
        if (account == null) return;

        var route = $"///Transactions?accountId={account.Id}";
        await Shell.Current.GoToAsync(route);
    }

}