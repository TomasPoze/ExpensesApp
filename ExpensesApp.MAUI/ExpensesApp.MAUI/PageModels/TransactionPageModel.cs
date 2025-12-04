using CommunityToolkit.Mvvm.ComponentModel;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Repositories;

namespace ExpensesApp.MAUI.PageModels;

public class TransactionPageModel:ObservableObject
{
    private readonly AccountController _accountController;

    private TransactionPageModel(AccountController accountController)
    {
        _accountController = accountController;
        _accountController.GetAllAccounts();
    }

    
}