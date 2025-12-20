using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;

namespace ExpensesApp.MAUI.PageModels;

public partial class AddExpensePageModel : ObservableObject
{
    private readonly ExpenseController _controller;

    public AddExpensePageModel(ExpenseController controller)
    {
        _controller = controller;
    }

    [ObservableProperty] private string _category;
    [ObservableProperty] private string _amount;
    [ObservableProperty] private string _description;

    [RelayCommand]
    private async Task AddExpenseAsync()
    {
        if (string.IsNullOrWhiteSpace(Category) ||
            string.IsNullOrWhiteSpace(Amount))
        {
            await Shell.Current.DisplayAlert("Error", "Category and Amount are required.", "OK");
            return;
        }

        string safeAmount = Amount.Replace(",", ".");
        if (!decimal.TryParse(safeAmount, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal parsedAmount))
        {
            await Shell.Current.DisplayAlert("Error", "Invalid amount format.", "OK");
            return;
        }

        var newExpense = new Expense(DateTime.UtcNow, Category, parsedAmount, Description ?? "")
        {
            AccountId = Guid.Empty
        };
        var result = await _controller.AddExpenseAsync(newExpense);
        if (result.Success)
        {
            await Shell.Current.DisplayAlert("Success", "Expense added.", "OK");

            Category = string.Empty;
            Amount = string.Empty;
            Description = string.Empty;

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", result.Message, "OK");
        }
    }
}