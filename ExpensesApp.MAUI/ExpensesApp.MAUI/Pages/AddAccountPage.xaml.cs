using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.MAUI.PageModels;

namespace ExpensesApp.MAUI.Pages;

public partial class AddAccountPage : ContentPage
{
    public AddAccountPage(AddAccountPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}