namespace ExpensesApp.Core.Models;

public class User
{
    private static int _counter = 1;
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Account> Accounts { get; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public User(Account account, string userName, string email)
    {
        Id = _counter++;
        UserName = userName;
        Email = email;
    }
}