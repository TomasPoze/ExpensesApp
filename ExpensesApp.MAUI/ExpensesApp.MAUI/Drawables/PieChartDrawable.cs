using ExpensesApp.Core.Models;


public class PieChartDrawable : IDrawable
{
    private readonly Func<IEnumerable<SpendingCategory>> _itemsAccessor;


    public PieChartDrawable(Func<IEnumerable<SpendingCategory>> itemsAccessor)
    {
        _itemsAccessor = itemsAccessor;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        
        float[] testValues = { 20, 40, 60, 80 };
        var items = testValues
            .Select((v, i) => new SpendingCategory($"Cat {i + 1}", (decimal)v, GetColorForIndex(i)))
            .ToList();

       
        var src = _itemsAccessor?.Invoke() ?? Enumerable.Empty<SpendingCategory>();
        /*var items = src
            .Where(x => x.Amount > 0)
            .ToList();
*/
        if (!items.Any())
            return;

        float total = (float)items.Sum(x => x.Amount);


        float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) * 0.8f / 2f;

        float cx = dirtyRect.Center.X;
        float cy = dirtyRect.Center.Y;


        float startAngle = -90f;


        var labels = new List<(float x, float y, float percentage)>();

        
        const float MinLabelAngle = 18f; // ~5% 


        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            float value = (float)item.Amount;
            float pct = value / total;
            float sweep = pct * 360f;


            var slicePath = CreateSlicePath(cx, cy, radius, startAngle, sweep);


            canvas.FillColor = item.Color ?? GetColorForIndex(i);
            canvas.FillPath(slicePath);

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            canvas.DrawPath(slicePath);

            // label position
            if (sweep >= MinLabelAngle)
            {
                float midAngle = startAngle + sweep / 2f;
                float rad = midAngle * (float)Math.PI / 180f;

                float labelR = radius * 0.55f;
                float lx = cx + (float)Math.Cos(rad) * labelR;
                float ly = cy + (float)Math.Sin(rad) * labelR;

                labels.Add((lx, ly, pct));
            }

            startAngle += sweep;
        }

        canvas.FontColor = Colors.White;
        canvas.FontSize = 14;


        foreach (var label in labels)
        {
            string txt = $"{label.percentage * 100:F0}%";

            float boxW = 40;
            float boxH = 24;

            var rect = new RectF(
                label.x - boxW / 2f,
                label.y - boxH / 2f,
                boxW,
                boxH
            );

            canvas.DrawString(
                txt,
                rect,
                HorizontalAlignment.Center,
                VerticalAlignment.Center
            );
        }
    }


    private static PathF CreateSlicePath(float cx, float cy, float radius, float startAngle, float sweep)
    {
        var path = new PathF();
        path.MoveTo(cx, cy);


        int segments = Math.Max(2, (int)(sweep / 6f)); // ~6°

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float angle = startAngle + sweep * t;
            float rad = angle * (float)Math.PI / 180f;

            float x = cx + (float)Math.Cos(rad) * radius;
            float y = cy + (float)Math.Sin(rad) * radius;

            path.LineTo(x, y);
        }

        path.Close();
        return path;
    }

    private static readonly Color[] Palette =
    {
        Color.FromArgb("#4CC9F0"),
        Color.FromArgb("#4895EF"),
        Color.FromArgb("#560BAD"),
        Color.FromArgb("#F72585"),
        Color.FromArgb("#2A9D8F"),
        Color.FromArgb("#F4A261"),
        Color.FromArgb("#E76F51"),
    };

    private static Color GetColorForIndex(int i) => Palette[i % Palette.Length];
}