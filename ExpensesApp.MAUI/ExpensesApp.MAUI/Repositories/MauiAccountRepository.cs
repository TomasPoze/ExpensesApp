using System.Text.Json;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;

namespace ExpensesApp.MAUI.Repositories;

public class MauiAccountRepository : IAccountRepository
{
    private readonly string FilePath;

    public MauiAccountRepository()
    {
        FilePath = Path.Combine(FileSystem.AppDataDirectory, "accounts.json");
    }
    
    public List<Account> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Account>();

        var json = File.ReadAllText(FilePath);
        var result = JsonSerializer.Deserialize<List<Account>>(json);
        return result;
    }

    public void Save(List<Account> accounts)
    {
        var json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}