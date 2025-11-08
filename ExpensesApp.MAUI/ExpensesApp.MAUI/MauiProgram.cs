using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Services;
using ExpensesApp.MAUI.PageModels;
using ExpensesApp.MAUI.Pages;
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
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}