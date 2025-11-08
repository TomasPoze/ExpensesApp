using ExpensesApp.Core.Models;
using ExpensesApp.Core.Services;

namespace ExpensesApp.Core.Repositories;

public class ExpenseRepository
{
    private static readonly string FilePath =
        Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "Data",
            "expenses.txt");


    public (List<Expense>, List<string> Errors) LoadFromFile()
    {
        var errors = new List<string>();

        var expenses = new List<Expense>();

        if (File.Exists(FilePath))
            foreach (var line in File.ReadAllLines(FilePath))
            {
                try
                {
                    var parts = line.Split("|");
                    if (parts.Length != 4)
                        throw new Exception("Line does not have 4 parts.");

                    var date = DateTime.Parse(parts[0]);
                    var category = parts[1];
                    var amount = decimal.Parse(parts[2]);
                    var description = parts[3];

                    expenses.Add(new Expense(date, category, amount, description));
                }
                catch (Exception)
                {
                    errors.Add(line);
                }
            }

        return (expenses, errors);
    }

    public void SaveToFile(List<Expense> expenses)
    {
        var lines = expenses.Select(exp => $"{exp.Date:yyyy-MM-dd}|{exp.Category}|{exp.Amount}|{exp.Description}");

        var backupPath = Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName,
            "data",
            "expenses_backup.txt");

        if (File.Exists(FilePath))
            File.Copy(FilePath, backupPath, overwrite: true);

        File.WriteAllLines(FilePath, lines);
    }

    public (bool Success, string Message) ExportToCsv(List<Expense> expenses)
    {
        var csvFilePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName,
            "data",
            "expenses.csv");
        var lines = new List<string>();
        int skipped = 0;
        lines.Add("Date,Category,Amount,Description");
        if (expenses.Count == 0)
            return (false, "No expenses found, file not exported");
        foreach (var expense in expenses)
        {
            if (string.IsNullOrWhiteSpace(expense.Category) || expense.Amount <= 0)
            {
                skipped++;
                continue;
            }

            var line = $"{expense.Date:yyyy-MM-dd},{expense.Category},{expense.Amount},{expense.Description}";
            lines.Add(line);
        }

        try
        {
            File.WriteAllLines(csvFilePath, lines);

            return (true, $"Exported {expenses.Count - skipped} expenses, skipped {skipped} invalid entries.");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to export CSV: {ex.Message}");
        }
    }
}