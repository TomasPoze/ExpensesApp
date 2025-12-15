using CommunityToolkit.Mvvm.ComponentModel;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Repositories;
using ExpensesApp.MAUI.ViewModels;

namespace ExpensesApp.MAUI.PageModels;

public partial class TransactionPageModel:ObservableObject
{

    [ObservableProperty] private AccountSectionViewModel _accountSection;

    public TransactionPageModel(AccountSectionViewModel accountSectionVM)
    {
        AccountSection = accountSectionVM;
    }

    public async Task InitializeAsync()
    {
        await AccountSection.LoadAccountsAsync();
    }
}