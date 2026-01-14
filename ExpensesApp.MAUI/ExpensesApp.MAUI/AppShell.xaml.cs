using ExpensesApp.MAUI.Pages;

namespace ExpensesApp.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(SpendingPage), typeof(SpendingPage));
        Routing.RegisterRoute(nameof(AddExpensePage), typeof(AddExpensePage));
        Routing.RegisterRoute(nameof(AddAccountPage), typeof(AddAccountPage));
        Routing.RegisterRoute(nameof(TransactionPage), typeof(TransactionPage));
    }
}