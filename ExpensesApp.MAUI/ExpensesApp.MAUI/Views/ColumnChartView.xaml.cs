using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.Core.Models;
using ExpensesApp.MAUI.Drawables;

namespace ExpensesApp.MAUI.Views;

public enum FilterMode
{
    Day,
    Month,
    Year
}

public partial class ColumnChartView : ContentView
{
    private readonly ColumnChartDrawable _drawable;

    public ColumnChartView()
    {
        InitializeComponent();
        _drawable = new ColumnChartDrawable();
        ChartGraphicsView.Drawable = _drawable;
    }

    public static readonly BindableProperty FilterModeProperty =
        BindableProperty.Create(
            nameof(FilterMode),
            typeof(FilterMode),
            typeof(ColumnChartView),
            defaultValue: FilterMode.Month,
            propertyChanged: OnItemsChanged
        );

    public static readonly BindableProperty SelectedDateProperty =
        BindableProperty.Create(
            nameof(SelectedDate),
            typeof(DateTime),
            typeof(ColumnChartView),
            defaultValue: DateTime.Now,
            propertyChanged: OnItemsChanged
        );

    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IEnumerable<Expense>),
            typeof(ColumnChartView), propertyChanged: OnItemsChanged);

    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public FilterMode FilterMode
    {
        get => (FilterMode)GetValue(FilterModeProperty);
        set => SetValue(FilterModeProperty, value);
    }

    public IEnumerable<Expense> Items
    {
        get => (IEnumerable<Expense>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (ColumnChartView)bindable;
        var allExpenses = view.Items;
        var mode = view.FilterMode;
        var date = view.SelectedDate;

        if (allExpenses == null) return;

        var processedData = new List<ChartItem>();

        if (mode == FilterMode.Year)
        {
            processedData = allExpenses
                .GroupBy(e=> e.Date.Month)
                .OrderBy(g => g.Key)
                .Select(g => new ChartItem(
                    CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key), 
                    g.Sum(e => e.Amount),
                    GetColorForIndex(g.Key)))
                .ToList();
        }
        else
        {
            processedData = allExpenses
                .Where(e => mode != FilterMode.Day || e.Date.Day == date.Day)
                .GroupBy(e=> e.Category)
                .Select((g,index) => new ChartItem(
                    g.Key.ToString(), 
                    g.Sum(e => e.Amount),
                    GetColorForIndex(index)))
                .OrderByDescending(g => g.Value)
                .ToList();
        }

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