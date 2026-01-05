using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.MAUI.PageModels;

namespace ExpensesApp.MAUI.Pages;

public partial class SpendingPage : ContentPage
{
    public SpendingPage(SpendingPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        if (BindingContext is SpendingPageModel vm)
        {
            await vm.LoadExpensesAsync();
        }
    }
}