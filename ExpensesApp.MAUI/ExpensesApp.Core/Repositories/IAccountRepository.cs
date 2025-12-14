using System.Text.Json;
using ExpensesApp.Core.Models;
using Microsoft.VisualBasic;

namespace ExpensesApp.Core.Repositories;

public interface IAccountRepository
{
    Task<List<Account>> GetAccountAsync();
    Task<Account?> GetAccountAsync(int id);
    Task<Account> AddAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(int id);
}