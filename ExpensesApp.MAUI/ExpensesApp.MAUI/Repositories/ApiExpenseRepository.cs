using System.Net.Http.Json;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;

namespace ExpensesApp.MAUI.Repositories;

public class ApiExpenseRepository : IExpenseRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiExpenseRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // Android Emulator vs Windows localhost logic
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            _baseUrl = "http://10.0.2.2:5033/api";
        }
        else
        {
            _baseUrl = "http://localhost:5033/api";
        }
    }

    public async Task<List<Expense>> GetExpensesAsync(Guid? accountId)
    {
        // If accountId is provided, we add it to the URL query string
        // Example: /api/expenses?accountId=1
        string url = $"{_baseUrl}/expenses";
        if (accountId.HasValue)
        {
            url += $"?accountId={accountId.Value}";
        }

        try
        {
            var expenses = await _httpClient.GetFromJsonAsync<List<Expense>>(url);
            return expenses ?? new List<Expense>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching expenses: {ex.Message}");
            return new List<Expense>();
        }
    }
    
    public async Task<Expense?> GetExpenseAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Expense>($"{_baseUrl}/expenses/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<Expense> AddExpenseAsync(Expense expense)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/expenses", expense);
        
        if (response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<Expense>())!;
        }
        
        throw new Exception("Failed to add expense");
    }

    public async Task UpdateExpenseAsync(Expense expense)
    {
        await _httpClient.PutAsJsonAsync($"{_baseUrl}/expenses/{expense.Id}", expense);
    }

    public async Task DeleteExpenseAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"{_baseUrl}/expenses/{id}");
    }
}