using System.Text.Json;
using ExpensesApp.Core.Models;
using Microsoft.VisualBasic;

namespace ExpensesApp.Core.Repositories;

public interface IAccountRepository
{
    List<Account> Load();
    void Save(List<Account> accounts);
}