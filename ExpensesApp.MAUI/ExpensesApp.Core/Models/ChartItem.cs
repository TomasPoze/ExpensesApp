using Microsoft.Maui.Graphics;

namespace ExpensesApp.Core.Models;

public class ChartItem
{
    public string Label { get; set; }
    public decimal Value { get; set; }
    public Color Color { get; set; }
    
    public ChartItem(string label, decimal amount, Color color)
    {
        Label = label;
        Value = amount;
        Color = color;
    }
}