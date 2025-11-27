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
        
        UpdateSpendingCategories();
    }

    [ObservableProperty] private string _today = DateTime.Today.ToString("dddd, dd MMMM yyyy");
    [ObservableProperty] private decimal _totalAmount;
    [ObservableProperty]
    private ObservableCollection<SpendingCategory> _spendingCategories;
    

    public decimal TotalMonthlyIncome => Accounts?.Sum(a => a.MonthlyIncome) ?? 0;
    public decimal SpentThisMonth => Accounts?.SelectMany(a => a.Expenses).Sum(e => e.Amount) ?? 0;
    
    public double SpentPercentage => TotalMonthlyIncome == 0 ? 0 : Math.Round((double)(SpentThisMonth/TotalMonthlyIncome) * 100, 2);
    public string IncomeSpentText => $"{SpentThisMonth} € / {TotalMonthlyIncome} €";
    
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

    
    [RelayCommand]
    private async Task GoToAddExpenseAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddExpensePage));
    }

    [ObservableProperty] private ObservableCollection<Account> _accounts;

    [RelayCommand]
    private async Task AddAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddAccountPage));
    }
    
    
    private void UpdateSpendingCategories()
    {
        if (Accounts == null)
            return;

        var allExpenses = Accounts.SelectMany(a => a.Expenses).ToList();;

        var grouped = allExpenses
            .GroupBy(e => e.Category)
            .Select(g => new SpendingCategory(
                g.Key,
                g.Sum(x => x.Amount),
                RandomColor() // arba fiksuotos spalvos
            ));

        //SpendingCategories = new ObservableCollection<SpendingCategory>(grouped);
        SpendingCategories = new ObservableCollection<SpendingCategory>
        {
            new SpendingCategory("Food", 50, Color.FromArgb("#3A6E79")),        // teal
            new SpendingCategory("Transport", 50, Color.FromArgb("#5A5A5A")),    // graphite
           // new SpendingCategory("Health", 25, Color.FromArgb("#2E4852")),       // dark teal
        };

    }

    private Color RandomColor()
    {
        var rnd = new Random();
        return Color.FromRgb(
            rnd.Next(50, 200),
            rnd.Next(50, 200),
            rnd.Next(50, 200)
        );
    }
    
    public void RefreshAccounts()
    {
        Accounts = new ObservableCollection<Account>(_accountController.GetAllAccounts());
        
        
        OnPropertyChanged(nameof(TotalMonthlyIncome));
        OnPropertyChanged(nameof(SpentThisMonth));
        OnPropertyChanged(nameof(SpentPercentage));
        OnPropertyChanged(nameof(IncomeSpentText));
        UpdateSpendingCategories();
        
        // LoadExpenses();
    }
}