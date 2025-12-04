
using ExpensesApp.Core.Models;

namespace ExpensesApp.MAUI.Views;

public partial class PieChartView : ContentView
{
    public PieChartDrawable Drawable { get; }

    public PieChartView()
    {
        InitializeComponent();
        
        Drawable = new PieChartDrawable(() => Items);
        ChartGraphicsView.Drawable = Drawable;
    }

    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IEnumerable<SpendingCategory>),
            typeof(PieChartView), propertyChanged: OnItemsChanged);

    public IEnumerable<SpendingCategory> Items
    {
        get => (IEnumerable<SpendingCategory>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PieChartView)bindable;

        var items = (IEnumerable<SpendingCategory>)newValue;

        // legenda atsinaujina
        view.LegendList.ItemsSource = items;

        // priverstinis pie chart redraw
        view.ChartGraphicsView.Invalidate();
    }
}


