using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Services;
using ExpensesApp.MAUI.Pages;


namespace ExpensesApp.MAUI.PageModels;

public partial class MainPageModel : ObservableObject
{
    private readonly AccountController _accountController;

    public MainPageModel(AccountController accountController)
    {
        _accountController = accountController;
        //LoadExpenses();
        Accounts = new ObservableCollection<Account>(_accountController.GetAllAccounts());
    }

    [ObservableProperty] private string _today = DateTime.Today.ToString("dddd, dd MMMM yyyy");
    [ObservableProperty] private decimal _totalAmount;
    [ObservableProperty] private decimal _totalMonthlyIncome;
    [ObservableProperty] private decimal _spentThisMonth;
    [ObservableProperty] private double _incomeProgress;


    /*private void LoadExpenses()
    {
        var all = _expenseController.GetAllExpenses();

        if (all.Count == 0)
        {
            _expenseController.AddExpense("Lunch", "12.44", "Mac");
            _expenseController.AddExpense("Fuel", "55.28", "Petrol");
            _expenseController.AddExpense("Health", "19.99", "Gym");
            all = _expenseController.GetAllExpenses();
        }

        Expenses = new ObservableCollection<Expense>(all);
        TotalAmount = all.Sum(x => x.Amount);
        ExpenseCount = Expenses.Count;
    }
*/

    private void UpdateIncomeSummary()
    {
        var TotalMonthlyIncome = Accounts?.Sum(x => x.MonthlyIncome) ?? 0;
        // var SpentThisMonth = _accountController
    }

    [RelayCommand]
    private async Task GoToAddExpenseAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddExpensePage));
    }

    [ObservableProperty] private ObservableCollection<Account> _accounts;

    [RelayCommand]
    private async Task AddAccount()
    {
        // čia kol kas paprastai – pridėkim fiktyvią paskyrą testavimui
        await Shell.Current.GoToAsync(nameof(AddAccountPage));
    }


    public void RefreshAccounts()
    {
        var list = _accountController.GetAllAccounts();
        Accounts = new ObservableCollection<Account>(list);
        // LoadExpenses();
    }
}