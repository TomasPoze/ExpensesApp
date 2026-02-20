namespace ExpensesApp.Core.Models;

public class Budget
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal SavedAmount { get; set; }
    public int Priority { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Budget(string Name, decimal TargetAmount, decimal SavedAmount, int Priority, DateTime? Deadline, bool IsCompleted)
    {
        this.Name = Name;
        this.TargetAmount = TargetAmount;
        this.SavedAmount = SavedAmount;
        this.Priority = Priority;
        this.Deadline = Deadline;
        this.IsCompleted = IsCompleted;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}