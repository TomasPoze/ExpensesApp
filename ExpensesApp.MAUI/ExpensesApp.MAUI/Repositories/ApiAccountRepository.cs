using System.Net.Http.Json;
using ExpensesApp.Core.Models;
using ExpensesApp.Core.Repositories;

namespace ExpensesApp.MAUI.Repositories;

public class ApiAccountRepository : IAccountRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiAccountRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            _baseUrl = "http://10.0.2.2:5033/api";
        }
        else
        {
            _baseUrl = "http://localhost:5033/api";
        }
    }

    public async Task<List<Account>> GetAccountAsync()
    {
        try
        {
            var accounts = await _httpClient.GetFromJsonAsync<List<Account>>($"{_baseUrl}/accounts");
            return accounts ?? new List<Account>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching accounts: {ex.Message}");
            return new List<Account>();
        }
    }


    public async Task<Account?> GetAccountAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Account>($"{_baseUrl}/accounts/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<Account> AddAccountAsync(Account account)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/accounts",account);

        if (response.IsSuccessStatusCode)
        {
            var createdAccount = await response.Content.ReadFromJsonAsync<Account>();
            return createdAccount;
        }

        throw new Exception("Failed to add account");
    }

    public async Task UpdateAccountAsync(Account account)
    {
        await _httpClient.PutAsJsonAsync($"{_baseUrl}/accounts/{account.Id}", account);
    }

    public async Task DeleteAccountAsync(int id)
    {
        await  _httpClient.DeleteAsync($"{_baseUrl}/accounts/{id}");
    }
}