using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.Core.Models;
using ExpensesApp.MAUI.Drawables;

namespace ExpensesApp.MAUI.Views;

public partial class ColumnChartView : ContentView
{
    private readonly ColumnChartDrawable _drawable;
    public ColumnChartView()
    {
        InitializeComponent();
        _drawable = new ColumnChartDrawable();
        ChartGraphicsView.Drawable = _drawable;
    }

    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IEnumerable<Expense>),
            typeof(ColumnChartView), propertyChanged: OnItemsChanged);

    public IEnumerable<Expense> Items
    {
        get => (IEnumerable<Expense>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (ColumnChartView)bindable;
        var allExpenses = (IEnumerable<Expense>)newValue;

        if (allExpenses == null) return;

        // 1. Group & Sum Data (Same logic as PieChart)
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        var processedData = allExpenses
            //.Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
            .Where(e => e.Amount > 0)
            .GroupBy(e => e.Category)
            .Select((g, index) => new SpendingCategory(
                g.Key ?? "Other",
                g.Sum(e => e.Amount),
                GetColorForIndex(index)
            ))
            .OrderByDescending(x => x.Amount)
            // Limit to top 5-7 columns so it fits nicely
            .Take(7) 
            .ToList();

        // 2. Update Drawable
        view._drawable.UpdateData(processedData);
        view.ChartGraphicsView.Invalidate();
    }

    
    private static readonly Color[] Palette =
    {
        Color.FromArgb("#4CC9F0"), Color.FromArgb("#4895EF"),
        Color.FromArgb("#560BAD"), Color.FromArgb("#F72585"),
        Color.FromArgb("#2A9D8F"), Color.FromArgb("#F4A261"),
        Color.FromArgb("#E76F51"),
    };
    
    private static Color GetColorForIndex(int i) => Palette[i % Palette.Length];
}