using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Services;
using ExpensesApp.Core.Validators;
using ExpensesApp.MAUI.PageModels;
using ExpensesApp.MAUI.Pages;
using ExpensesApp.MAUI.Repositories;
using Microsoft.Extensions.Logging;

namespace ExpensesApp.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<ExpenseRepository>();
        builder.Services.AddSingleton<ExpenseService>();
        builder.Services.AddSingleton<ExpenseValidator>();
        builder.Services.AddSingleton<ExpenseController>();
        builder.Services.AddSingleton<MainPageModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<AddExpensePageModel>();
        builder.Services.AddSingleton<AddExpensePage>();
        builder.Services.AddSingleton<AccountRepository>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<AccountValidator>();
        builder.Services.AddSingleton<AccountController>();
        builder.Services.AddTransient<AddAccountPageModel>();
        builder.Services.AddTransient<AddAccountPage>();
        builder.Services.AddSingleton<IAccountRepository, MauiAccountRepository>();
        builder.Services.AddSingleton<AccountValidator>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<AccountController>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}