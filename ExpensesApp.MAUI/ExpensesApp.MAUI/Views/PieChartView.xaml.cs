using ExpensesApp.MAUI.Drawables;

namespace ExpensesApp.MAUI.Views;

public partial class PieChartView : ContentView
{
    public PieChartView()
    {
        InitializeComponent();
        ChartGraphicsView.Drawable = new PieChartDrawable();
    }

}

