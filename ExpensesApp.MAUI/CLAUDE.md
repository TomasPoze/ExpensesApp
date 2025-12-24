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
Platform-independent business logic layer (.NET 9.0) containing:
- **Models**: `User`, `Account`, `Expense`, `Transaction`, `SpendingCategory`
- **Controllers**: Business logic orchestrators (not ASP.NET controllers) that coordinate between services and validators
- **Services**: `ExpenseService`, `AccountService` - core business operations
- **Validators**: `ExpenseValidator`, `AccountValidator` - input validation logic
- **Repositories**: Data access interfaces (`IAccountRepository`, `IExpenseRepository`)

### ExpensesApp.MAUI (Mobile/Desktop Client)
.NET MAUI app targeting Android and Windows using MVVM pattern:
- **Pages**: XAML views (MainPage, TransactionPage, SpendingPage, BudgetPage, StatisticPage, AddExpensePage, AddAccountPage)
- **PageModels**: View models bound to pages via CommunityToolkit.Mvvm
- **ViewModels**: Shared view models for reusable views (HeaderViewModel, AccountSectionViewModel)
- **Views**: Reusable XAML components (HeaderView, AccountSectionView, PieChartView)
- **Drawables**: Custom graphics (PieChartDrawable for charts)
- **Repositories**: API client implementations (ApiAccountRepository, ApiExpenseRepository)
- **Converters**: CategoryToIconConverter, PercentageToProgressConverter

Navigation uses Shell with TabBar (Home, Spendings, Transactions, Budget, Statistics).

### ExpensesApp.Api (Backend API)
ASP.NET Core Web API with:
- PostgreSQL database via Entity Framework Core (Npgsql)
- `ExpensesDbContext` for data access
- OpenAPI support enabled

## Data Models

### User
- `Id` (Guid), `Email`, `UserName`, `GoogleId` (optional)
- `CreatedAt`, `UpdatedAt`
- Has many: `Accounts`

### Account
- `Id` (Guid), `UserId` (FK)
- `AccountName`, `Type` (Cash/Bank/Card), `Currency` (EUR/USD/GBP)
- `Balance`, `MonthlyIncome` (decimal 18,2)
- Has many: `Expenses`, `Transactions`

### Expense
- `Id` (Guid), `AccountId` (FK)
- `Date`, `Category`, `Amount` (decimal 18,2), `Description`

### Transaction
- `Id` (Guid), `AccountId` (FK)
- `Category`, `Amount` (decimal 18,2), `OccuredAt`, `Description`
- `Type` (Income/Expense) - affects account balance on create/delete

### SpendingCategory (non-EF)
- `Name`, `Amount`, `Color`, `PercentageText`
- Used for pie chart visualization

## API Endpoints

### Users (`/api/users`)
- `GET /` - List all users
- `GET /{id}` - Get user by ID
- `GET /{id}/accounts` - Get user's accounts
- `POST /` - Create user
- `PUT /{id}` - Update user
- `DELETE /{id}` - Delete user

### Accounts (`/api/accounts`)
- `GET /` - List all accounts
- `GET /{id}` - Get account by ID
- `POST /` - Create account
- `PUT /{id}` - Update account
- `DELETE /{id}` - Delete account

### Expenses (`/api/expenses`)
- `GET /` - List expenses (optional `?accountId=` filter), ordered by date DESC
- `GET /{id}` - Get expense by ID
- `GET /summary` - Aggregate: total count, amount, average, by category
- `POST /` - Create expense
- `PUT /{id}` - Update expense
- `DELETE /{id}` - Delete expense

### Transactions (`/api/transactions`)
- `GET /` - List transactions (optional `?accountId=`, `?type=` filters), ordered by OccuredAt DESC
- `GET /{id}` - Get transaction by ID
- `GET /summary` - Aggregate: total count, income, expenses, net, by category
- `POST /` - Create transaction (updates account balance based on type)
- `PUT /{id}` - Update transaction
- `DELETE /{id}` - Delete transaction (reverses balance change)

## MAUI Page Structure

| Page | PageModel | Purpose |
|------|-----------|---------|
| MainPage | MainPageModel | Dashboard with accounts and spending overview |
| TransactionPage | TransactionPageModel | Master-detail transaction list with account filtering |
| AddExpensePage | AddExpensePageModel | Create expense form (query: `accountId`) |
| AddAccountPage | AddAccountPageModel | Create account form |
| SpendingPage | SpendingPageModel | Spending analytics (stub) |
| BudgetPage | BudgetPageModel | Budget management |
| StatisticPage | StatisticPageModel | Statistics view |

### API Base URLs
- **Android Emulator**: `http://10.0.2.2:5033/api`
- **Windows**: `http://localhost:5033/api`

Configured in `ApiAccountRepository.cs` and `ApiExpenseRepository.cs`.

## Key Patterns

- **Dependency Injection**: Services registered in `MauiProgram.cs` (MAUI) and `Program.cs` (API)
- **MVVM**: Pages bind to PageModels using CommunityToolkit.Mvvm (`ObservableObject`, `RelayCommand`)
- **Controller Pattern in Core**: Core controllers handle business logic coordination (distinct from ASP.NET API controllers)
- **Repository Pattern**: `IAccountRepository`, `IExpenseRepository` interfaces with API client implementations
- **Cascade Delete**: Account deletion removes related expenses and transactions

## Data Flow Example: Add Expense

1. User fills form on AddExpensePage
2. `AddExpensePageModel.AddExpenseAsync()` validates inputs
3. Calls `ExpenseController.AddExpenseAsync()`
4. `ExpenseController` delegates to `ExpenseService`
5. `ExpenseService` calls `ExpenseValidator` then `ApiExpenseRepository`
6. `ApiExpenseRepository` POSTs to `/api/expenses`
7. API controller verifies account exists, persists to DB
8. Response returned to client, UI refreshed

## Key File Locations

**Projects:**
- `ExpensesApp.MAUI.sln` - Solution file
- `ExpensesApp.Core/ExpensesApp.Core.csproj`
- `ExpensesApp.MAUI/ExpensesApp.MAUI.csproj`
- `ExpensesApp.Api/ExpensesApp.Api.csproj`

**Core Layer:**
- `ExpensesApp.Core/Models/` - User, Account, Expense, Transaction, SpendingCategory
- `ExpensesApp.Core/Services/` - ExpenseService, AccountService
- `ExpensesApp.Core/Controllers/` - ExpenseController, AccountController
- `ExpensesApp.Core/Validators/` - ExpenseValidator, AccountValidator
- `ExpensesApp.Core/Repositories/` - IAccountRepository, IExpenseRepository

**MAUI Client:**
- `ExpensesApp.MAUI/Pages/` - XAML page files
- `ExpensesApp.MAUI/PageModels/` - Page view models
- `ExpensesApp.MAUI/ViewModels/` - Reusable view models
- `ExpensesApp.MAUI/Views/` - Reusable XAML components
- `ExpensesApp.MAUI/Repositories/` - ApiAccountRepository, ApiExpenseRepository
- `ExpensesApp.MAUI/Converters/` - Value converters
- `ExpensesApp.MAUI/MauiProgram.cs` - DI configuration

**API:**
- `ExpensesApp.Api/Controllers/` - UsersController, AccountsController, ExpensesController, TransactionsController
- `ExpensesApp.Api/Data/ExpensesDbContext.cs` - EF Core context
- `ExpensesApp.Api/Migrations/` - Database migrations
- `ExpensesApp.Api/Program.cs` - API configuration

## Technology Stack

- .NET 9.0
- .NET MAUI (Android, Windows)
- CommunityToolkit.Mvvm 8.4.0
- Entity Framework Core 9.0.11 with PostgreSQL (Npgsql 9.0.4)
- Microsoft.Maui.Graphics for custom drawing

## Notes

- **Hardcoded UserId**: `AddAccountPageModel` uses a hardcoded UserId (`cc486a5a-3ccf-40b3-888d-2c28e36bf54e`) - needs proper user authentication
- **Category Icons**: Mapped in `CategoryToIconConverter` (food, groceries, transport, fuel, entertainment)
- **JSON Handling**: API uses `ReferenceHandler.IgnoreCycles` for circular reference handling
