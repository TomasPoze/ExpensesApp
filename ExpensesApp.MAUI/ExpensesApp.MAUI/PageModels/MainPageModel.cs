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
    private readonly ExpenseController _expenseController;

    public MainPageModel(ExpenseController controller)
    {
        _expenseController = controller;
        LoadExpenses();
    }

    [ObservableProperty] private string _today = DateTime.Today.ToString("dddd, dd MMMM yyyy");
    [ObservableProperty] private ObservableCollection<Expense> _expenses;
    [ObservableProperty] private decimal _totalAmount;
    [ObservableProperty] private int _expenseCount;

    private void LoadExpenses()
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
    
    [RelayCommand]
    private async Task GoToAddExpenseAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddExpensePage));
    }

    public void RefreshExpenses()
    {
        LoadExpenses();
    }
}