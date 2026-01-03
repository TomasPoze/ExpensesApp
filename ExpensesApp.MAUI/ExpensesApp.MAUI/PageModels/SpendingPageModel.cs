using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;

namespace ExpensesApp.MAUI.PageModels;

public partial class SpendingPageModel : ObservableObject
{
    private readonly ExpenseController _expenseController;
    
    [ObservableProperty] private ObservableCollection<Expense> _expenses = new();

    public SpendingPageModel(ExpenseController expenseController)
    {
        _expenseController = expenseController;
    }

    public async Task SpendingChartAsync()
    {
        try
        {
            var allExpenses = await _expenseController.GetExpensesAsync();

            Expenses = new ObservableCollection<Expense>(allExpenses);
        }
        catch (Exception ex)
        {
            Expenses = new ObservableCollection<Expense>();
        }
    }
    
}