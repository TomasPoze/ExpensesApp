using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.MAUI.ViewModels;

namespace ExpensesApp.MAUI.Views;

public partial class HeaderView : ContentView
{
    public HeaderView()
    {
        InitializeComponent();
        this.Opacity = 1;
        BindingContext = new HeaderViewModel();
    }

    public async Task ShowAsync()
    {
        await this.FadeTo(1,250, Easing.Linear);
    }

    public async Task HideAsync()
    {
        await this.FadeTo(0, 250, Easing.Linear);
    }
}