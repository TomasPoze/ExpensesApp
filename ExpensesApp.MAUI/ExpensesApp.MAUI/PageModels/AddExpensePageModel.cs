using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;

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
            string.IsNullOrWhiteSpace(Amount) ||
            string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlert("Error", "Please fill all fields.", "OK");
            return;
        }

        _controller.AddExpense(Category, Amount, Description);
        
        await Shell.Current.DisplayAlert("Success", "Expense added!", "OK");

        await Shell.Current.GoToAsync("..");
    }
}