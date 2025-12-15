using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.MAUI.PageModels;

namespace ExpensesApp.MAUI.Pages;

public partial class TransactionPage : ContentPage
{
    public TransactionPage(TransactionPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TransactionPageModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}