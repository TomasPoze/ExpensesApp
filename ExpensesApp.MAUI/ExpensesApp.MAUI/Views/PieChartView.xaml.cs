
using ExpensesApp.Core.Models;

namespace ExpensesApp.MAUI.Views;

public partial class PieChartView : ContentView
{
    public readonly PieChartDrawable _drawable;

    public PieChartView()
    {
        InitializeComponent();

        _drawable = new PieChartDrawable();
        ChartGraphicsView.Drawable = _drawable;
    }

    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IEnumerable<Expense>),
            typeof(PieChartView), propertyChanged: OnItemsChanged);

    public IEnumerable<Expense> Items
    {
        get => (IEnumerable<Expense>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PieChartView)bindable;
        var allExpenses = (IEnumerable<Expense>)newValue;
        
        if(allExpenses == null) return;
        
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        
        var processedCategories = allExpenses
           // .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
            .Where(e => e.Amount > 0)
            .GroupBy(e => e.Category)
            .Select((g, i) => new SpendingCategory(
                g.Key ?? "Other",
                g.Sum(e => e.Amount),
                GetColorForIndex(i)
            ))
            .OrderByDescending(x => x.Amount)
            .ToList();
        
        view.LegendList.ItemsSource = processedCategories;
        
        view._drawable.UpdateData(processedCategories);
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


