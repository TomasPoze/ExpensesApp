using ExpensesApp.Core.Repositories;
using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Services;

public class ExpenseService
{
    private readonly ExpenseRepository _repository;
    private List<Expense> _expenses;

    public ExpenseService(ExpenseRepository repository)
    {
        _repository = repository;
        _expenses = _repository.LoadFromFile().Item1;
    }


    public (decimal Sum, decimal Average, int Count) GetStatisticsSummary()
    {
        if (_expenses.Count == 0)
            return (0, 0, 0);

        var sum = _expenses.Sum(e => e.Amount);
        var average = _expenses.Average(e => e.Amount);
        var count = _expenses.Count;

        return (sum, average, count);
    }

    public void RemoveExpense(string idInput)
    {
        var id = int.Parse(idInput);
        var expenseToRemove = _expenses.FirstOrDefault(x => x.Id == id);
        _expenses.Remove(expenseToRemove);
    }

    public bool EditExpense(int id, string? newCategory, string? newAmount, string? newDescription)
    {
        var expense = _expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null)
            return false;

        if (!string.IsNullOrEmpty(newCategory))
            expense.Category = newCategory;

        if (!string.IsNullOrEmpty(newAmount))
            expense.Amount = decimal.Parse(newAmount);

        if (!string.IsNullOrEmpty(newDescription))
            expense.Description = newDescription;

        return true;
    }

    public (bool Success, string Message) ExportToCsv()
    {
        return _repository.ExportToCsv(_expenses);
    }

    public (List<Expense>, List<string> Errors) LoadFromFile()
    {
        var (expenses, errors) = _repository.LoadFromFile();
        _expenses = expenses;
        
        return (_expenses, errors);
    }

    public void SaveToFile()
    {
        _repository.SaveToFile(_expenses);
    }

    public void AddExpenses(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);

        _expenses.Add(expense);
    }

    public (IEnumerable<(int Year, int Month, string MonthName, decimal Total)> ByMonth,
        IEnumerable<(int Year, decimal Total)> ByYear) GetMonthlyStatistics()
    {
        var byMonth = _expenses.GroupBy(e => new { e.Date.Year, e.Date.Month })
            .Select(f => (
                Year: f.Key.Year,
                Month: f.Key.Month,
                MonthName: new DateTime(f.Key.Year, f.Key.Month, 1).ToString("MMMM"),
                Total: f.Sum(x => x.Amount)
            )).OrderBy(x => x.Year).ThenBy(x => x.Month);

        var byYear = _expenses.GroupBy(e => new { e.Date.Year })
            .Select(e => (
                Year: e.Key.Year,
                Total: e.Sum(x => x.Amount)))
            .OrderBy(x => x.Year);

        return (byMonth, byYear);
    }

    public List<Expense> GetExpensesByCategory(string category)
    {
        return _expenses.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Expense> GetExpensesSortedByAmount(bool descending)
    {
        if (descending)
        {
            var sorted = _expenses.OrderByDescending(e => e.Amount).ToList();
            return sorted;
        }
        else
        {
            var sorted = _expenses.OrderBy(e => e.Amount).ToList();
            return sorted;
        }
    }


    public IEnumerable<(string Category, decimal Sum)> GetTotalByCategory()
    {
        var total = _expenses
            .GroupBy(e => e.Category)
            .Select(f => (Category: f.Key, Sum: f.Sum(x => x.Amount)));
        return total;
    }

    public Expense? GetMostExpensiveExpense()
    {
        var expensive = _expenses.OrderByDescending(e => e.Amount).FirstOrDefault();
        return expensive;
    }

    public Expense? GetCheapestExpense()
    {
        var cheapest = _expenses.OrderBy(e => e.Amount).FirstOrDefault();

        return cheapest;
    }

    public decimal GetExpenseSumByMonth(int month)
    {
        var expenses = _expenses.Where(e => e.Date.Month == month).Sum(f => f.Amount);
        return expenses;
    }

    public decimal GetTotalExpenses()
    {
        return _expenses.Sum(e => e.Amount);
    }

    public List<Expense> GetAllExpenses()
    {
        return _expenses.ToList();
    }
}