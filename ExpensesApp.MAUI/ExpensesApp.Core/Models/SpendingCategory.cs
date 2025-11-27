using Microsoft.Maui.Graphics;

namespace ExpensesApp.Core.Models;

public class SpendingCategory
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public Color Color { get; set; }

    public SpendingCategory(string name, decimal amount, Color color)
    {
        Name = name;
        Amount = amount;
        Color = color;
    }
}