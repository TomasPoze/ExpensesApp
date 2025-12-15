using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Services;
using ExpensesApp.MAUI.Pages;
using ExpensesApp.MAUI.ViewModels;


namespace ExpensesApp.MAUI.PageModels;

public partial class MainPageModel : ObservableObject
{
    [ObservableProperty] private AccountSectionViewModel _accountSection;

    //[ObservableProperty] private ObservableCollection<SpendingCategory> _spendingCategories;

    public MainPageModel(AccountSectionViewModel accountSectionVM)
    {
        AccountSection = accountSectionVM;
        AccountSection.PropertyChanged += OnAccountSectionPropertyChanged;
    }

    private void OnAccountSectionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AccountSectionViewModel.Accounts))
        {
            OnPropertyChanged(nameof(TotalMonthlyIncome));
            OnPropertyChanged(nameof(SpentThisMonth));
            OnPropertyChanged(nameof(SpentPercentage));
            OnPropertyChanged(nameof(IncomeSpentText));
        }
    }

    
    public decimal TotalMonthlyIncome => AccountSection.Accounts?.Sum(a => a.MonthlyIncome) ?? 0;
    public decimal SpentThisMonth => AccountSection.Accounts?.SelectMany(a => a.Expenses).Sum(e => e.Amount) ?? 0;

    public double SpentPercentage =>
        TotalMonthlyIncome == 0 ? 0 : Math.Round((double)(SpentThisMonth / TotalMonthlyIncome) * 100, 2);

    public string IncomeSpentText => $"{SpentThisMonth} € / {TotalMonthlyIncome} €";

    public async Task RefreshAccountsAsync()
    {
        await AccountSection.LoadAccountsAsync();
    }
}