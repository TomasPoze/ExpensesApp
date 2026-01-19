using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;

namespace ExpensesApp.MAUI.PageModels;

[QueryProperty(nameof(AccountIdRaw),"accountId")]
public partial class AddExpensePageModel : ObservableObject, IQueryAttributable
{
    private readonly ExpenseController _controller;

    public AddExpensePageModel(ExpenseController controller)
    {
        _controller = controller;
    }

    [ObservableProperty] private string _category;
    [ObservableProperty] private string _amount;
    [ObservableProperty] private string _description;
    [ObservableProperty] private Guid _accountId;
    
    [NotifyPropertyChangedFor(nameof(PageTitle))]
    [NotifyPropertyChangedFor(nameof(ButtonText))]
    [ObservableProperty] private bool _isEditMode;

    public string PageTitle => IsEditMode ? "Edit Expense": "Add Expense";
    public string ButtonText => IsEditMode ? "Update Expense" : "Add Expense";
    

    [ObservableProperty]
    private Expense _expenseToEdit;
    
    partial void OnExpenseToEditChanged(Expense value)
    {
        if (value != null)
        {
            Category = value.Category;
            Amount = value.Amount.ToString();
            Description = value.Description;
            AccountId = value.AccountId;
            IsEditMode = (value != null);
        }
    }
    
    public string AccountIdRaw
    {
        set{
            if (!string.IsNullOrEmpty(value) && Guid.TryParse(value, out Guid parsedAccountId))
            {
                AccountId = parsedAccountId;
            }}
    }
    
    [RelayCommand]
    private async Task AddExpenseAsync()
    {
        if (AccountId == Guid.Empty)
        {
            await Shell.Current.DisplayAlert("Error", "No Account Linked!", "OK");
            return;
        }        
        
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

        bool isSuccess = false;
        string message = string.Empty;

        if (ExpenseToEdit != null)
        {
            ExpenseToEdit.Category = Category;
            ExpenseToEdit.Amount = parsedAmount;
            ExpenseToEdit.Description = Description ?? "";
            ExpenseToEdit.AccountId = AccountId;

            var result = await _controller.EditExpenseAsync(ExpenseToEdit);
            isSuccess = result.Success;
            message = result.Message;
        }
        else
        {
            var newExpense = new Expense(DateTime.UtcNow, Category, parsedAmount, Description ?? "")
            {
                AccountId = this.AccountId
            };
            var result = await _controller.AddExpenseAsync(newExpense);
            isSuccess = result.Success;
            message = result.Message;
        }
        
        if (isSuccess)
        {
            string successMsg = ExpenseToEdit != null ? "Transaction Updated!" : "Expense Added!";
            await Shell.Current.DisplayAlert("Success", successMsg, "OK");

            Category = string.Empty;
            Amount = string.Empty;
            Description = string.Empty;

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", message, "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteExpenseAsync()
    {
        if(ExpenseToEdit == null) return;
        
        var result = await Shell.Current.DisplayAlert("Delete Expense", "Are you sure you want to delete this expense?", "Yes", "No");
        if (result)
        {
            await _controller.RemoveExpenseAsync(ExpenseToEdit.Id);
            await Shell.Current.GoToAsync("..");
        }
    }
    
    [RelayCommand]
    private async Task Cancel()
    {
        ExpenseToEdit = null;
        await Shell.Current.GoToAsync("..");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ExpenseToEdit"))
        {
            var expense = query["ExpenseToEdit"] as Expense;
            
            ExpenseToEdit = expense;
            
            Category = expense.Category;
            Amount = expense.Amount.ToString();
            Description = expense.Description;
            AccountId = expense.AccountId;
            
            IsEditMode = true;
        }
        else
        {
            ExpenseToEdit = null;
            Category = string.Empty;
            Amount = string.Empty;
            Description = string.Empty;
            IsEditMode = false;

            if (query.ContainsKey("accountId"))
            {
                if(Guid.TryParse(query["accountId"].ToString(), out Guid id))
                    AccountId = id;
            }
        }
    }
}