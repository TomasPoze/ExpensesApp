using ExpensesApp.Core.Models;

namespace ExpensesApp.Core.Repositories;

public class AccountRepository
{
    private static readonly string FilePath =
        Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "Data",
            "accounts.txt");


    public (List<Account>, List<string> Errors) LoadFromFile()
    {
        var errors = new List<string>();
        var accounts = new List<Account>();

        if (!File.Exists(FilePath))
            return (accounts, errors);

        foreach (var line in File.ReadAllLines(FilePath))
        {
            try
            {
                var parts = line.Split("|");
                if (parts.Length != 4)
                    throw new Exception("Line does not have 4 parts.");

                var name = parts[0];
                var currency = parts[1];
                if (!Enum.TryParse(currency, true, out Currency parsedCurrency))
                    throw new Exception("Invalid currency format.");

                if (!decimal.TryParse(parts[2], out var balance))
                    throw new Exception("Invalid balance format.");

                if (!decimal.TryParse(parts[3], out var income))
                    throw new Exception("Invalid income format.");
                accounts.Add(new Account(name, parsedCurrency, balance, income));
            }
            catch (Exception)
            {
                errors.Add(line);
            }
        }

        return (accounts, errors);
    }

    public void SaveToFile(List<Account> accounts)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var lines = accounts.Select(acc =>
            $"{acc.Id}|{acc.AccountName}|{acc.Currency}|{acc.Balance}|{acc.MonthlyIncome}");


        var backupPath = Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory).Parent!.Parent!.Parent!.FullName,
            "Data",
            "acc_backup.txt");

        if (File.Exists(FilePath))
            File.Copy(FilePath, backupPath, overwrite: true);

        File.WriteAllLines(FilePath, lines);
    }
}