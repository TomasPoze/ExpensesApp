# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build entire solution
dotnet build ExpensesApp.MAUI.sln

# Build specific project
dotnet build ExpensesApp.MAUI/ExpensesApp.MAUI.csproj
dotnet build ExpensesApp.Core/ExpensesApp.Core.csproj
dotnet build ExpensesApp.Api/ExpensesApp.Api.csproj

# Run MAUI app (Windows)
dotnet build ExpensesApp.MAUI/ExpensesApp.MAUI.csproj -t:Run -f net9.0-windows10.0.19041.0

# Run MAUI app (Android)
dotnet build ExpensesApp.MAUI/ExpensesApp.MAUI.csproj -t:Run -f net9.0-android

# Run API project
dotnet run --project ExpensesApp.Api/ExpensesApp.Api.csproj

# EF Core migrations (API project)
dotnet ef migrations add <MigrationName> --project ExpensesApp.Api
dotnet ef database update --project ExpensesApp.Api
```

## Architecture Overview

This is an expense tracking application with three projects:

### ExpensesApp.Core (Shared Library)
Platform-independent business logic layer containing:
- **Models**: `Expense`, `Account`, `Transaction`, `SpendingCategory`, `User`
- **Controllers**: Business logic orchestrators (not ASP.NET controllers) that coordinate between services and validators
- **Services**: `ExpenseService`, `AccountService` - core business operations
- **Validators**: Input validation logic
- **Repositories**: Data access interfaces (e.g., `IAccountRepository`)

### ExpensesApp.MAUI (Mobile/Desktop Client)
.NET MAUI app targeting Android and Windows using MVVM pattern:
- **Pages**: XAML views (MainPage, TransactionPage, SpendingPage, BudgetPage, StatisticPage, AddExpensePage, AddAccountPage)
- **PageModels**: View models bound to pages via CommunityToolkit.Mvvm
- **ViewModels**: Shared view models for reusable views (HeaderViewModel, AccountSectionViewModel)
- **Views**: Reusable XAML components
- **Drawables**: Custom graphics (PieChartDrawable for charts)
- **Repositories**: Platform-specific repository implementations (MauiAccountRepository)

Navigation uses Shell with TabBar (Home, Spendings, Transactions, Budget, Statistics).

### ExpensesApp.Api (Backend API)
ASP.NET Core Web API with:
- PostgreSQL database via Entity Framework Core (Npgsql)
- `ExpensesDbContext` for data access
- OpenAPI support enabled

## Key Patterns

- **Dependency Injection**: All services registered in `MauiProgram.cs` for MAUI, `Program.cs` for API
- **MVVM**: Pages bind to PageModels using CommunityToolkit.Mvvm
- **Controller Pattern in Core**: Core controllers handle business logic coordination (distinct from ASP.NET API controllers)
- **Repository Pattern**: `IAccountRepository` interface with platform-specific implementations

## Technology Stack

- .NET 9.0
- .NET MAUI (Android, Windows)
- CommunityToolkit.Mvvm 8.4.0
- Entity Framework Core 9.0 with PostgreSQL
- Microsoft.Maui.Graphics for custom drawing
