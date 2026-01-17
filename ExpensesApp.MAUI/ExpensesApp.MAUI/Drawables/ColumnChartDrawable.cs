using ExpensesApp.Core.Models;

namespace ExpensesApp.MAUI.Drawables;

public class ColumnChartDrawable : IDrawable
{
    private List<ChartItem> _items = new();
    

    public void UpdateData(List<ChartItem> items)
    {
        _items = items;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_items == null || !_items.Any()) return;

        float margin = 20;
        float bottomLabelHeight = 30; // Space for text below bars
        float usableHeight = dirtyRect.Height - (margin * 2) - bottomLabelHeight;
        float usableWidth = dirtyRect.Width - (margin * 2);

        // 2. Calculate Scaling
        // Find the tallest bar value so we can scale everything relative to it
        float maxAmount = (float)_items.Max(x => x.Value);
        if (maxAmount <= 0) maxAmount = 1; // Prevent divide by zero

        // Calculate width per bar
        float columnGap = 10;
        float columnWidth = (usableWidth - (columnGap * (_items.Count - 1))) / _items.Count;

        // 3. Draw Loop
        float currentX = margin;
        foreach (var item in _items)
        {
            float val = (float)item.Value;
            
            // Calculate Bar Height relative to the maximum value
            float barHeight = (val / maxAmount) * usableHeight;
            
            // Calculate Y position (Canvas 0 is top, so we draw "down" to the bottom anchor)
            // Bottom Anchor Y = margin + usableHeight
            float x = currentX;
            float y = margin + usableHeight - barHeight;

            // A. Draw the Bar
            canvas.FillColor = item.Color;
            
            // Draw a rounded rectangle for a modern look (corner radius 4)
            canvas.FillRoundedRectangle(x, y, columnWidth, barHeight, 4);

            // B. Draw Label (Category Name) below the bar
            canvas.FontColor = Colors.Gray;
            canvas.FontSize = 10;
            
            // Draw only the first 3 letters if name is long
            //string label = item.Label.Length > 3 ? item.Label.Substring(0, 3) : item.Label;
            
            canvas.DrawString(item.Label, x, margin + usableHeight + 5, columnWidth, bottomLabelHeight, HorizontalAlignment.Center, VerticalAlignment.Top);

            // C. Draw Amount (Optional, above bar)
            canvas.FontSize = 9;
            canvas.DrawString($"{val:F0}", x, y - 15, columnWidth, 15, HorizontalAlignment.Center, VerticalAlignment.Bottom);

            // Move to next position
            currentX += columnWidth + columnGap;
        }
    }
}