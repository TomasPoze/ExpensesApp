
namespace ExpensesApp.MAUI.Pages;


public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnAddExpenseClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Add Expense", "Navigate to Add Expense Page", "OK");
        
    }

    private async void OnViewExpensesClicked(object sender, EventArgs e)
    {
        await DisplayAlert("All Expenses", "Navigate to Expenses List Page", "OK");
    }

    private async void OnExportClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Export", "Exporting to CSV...", "OK");
    }
}