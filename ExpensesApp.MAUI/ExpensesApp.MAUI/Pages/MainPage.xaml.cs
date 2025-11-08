using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Services;
using ExpensesApp.MAUI.PageModels;

namespace ExpensesApp.MAUI.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainPageModel _viewModel;
    private double _lastScrollY = 0;

    public MainPage(MainPageModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        ;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshExpenses();
    }

    private void MainScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY < _lastScrollY)
            HeaderViewControl.ShowAsync();
        else if (e.ScrollY > _lastScrollY)
            HeaderViewControl.HideAsync();
        
        _lastScrollY = e.ScrollY;
    }

    private async void OnAddExpenseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync((nameof(AddExpensePage)));
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