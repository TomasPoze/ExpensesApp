using ExpensesApp.Core.Models;

public class PieChartDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // 1. YOUR VALUES
        float[] values = { 36, 25, 25, 74 };
        float total = values.Sum();

        // 2. SETUP GEOMETRY
        float scale = 0.80f;
        // I removed 'leftShift' because it was pushing the chart off-center, 
        // causing labels to potentially clip or look weird.
        float radius = (Math.Min(dirtyRect.Width, dirtyRect.Height) * scale) / 2f;
        
        float centerX = dirtyRect.Center.X; 
        float centerY = dirtyRect.Center.Y;

        float startAngle = -90f;

        for (int i = 0; i < values.Length; i++)
        {
            float value = values[i];
            float percentage = value / total;
            float sweep = percentage * 360f;
            float endAngle = startAngle + sweep;

            // --- DRAW SLICE (Your original logic) ---
            var path = new PathF();
            path.MoveTo(centerX, centerY);

            // Reverted to 'false' since that worked for your shape
            path.AddArc(
                centerX - radius,
                centerY - radius,
                centerX + radius,
                centerY + radius,
                startAngle,
                endAngle,
                false 
            );

            path.LineTo(centerX, centerY);

            canvas.FillColor = GetColorForIndex(i);
            canvas.FillPath(path);

            canvas.StrokeColor = Color.FromArgb("#222"); // Dark border
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);

            // --- FIXED TEXT DRAWING ---
            
            // 1. Calculate precise mid-angle
            float midAngle = startAngle + (sweep / 2);

            // 2. Convert to Radians
            float rad = midAngle * (float)(Math.PI / 180.0f);

            // 3. Calculate Position (55% from center looks best)
            float textDistance = radius * 0.55f; 
            float textX = centerX + textDistance * (float)Math.Cos(rad);
            float textY = centerY + textDistance * (float)Math.Sin(rad);

            canvas.FontSize = 14;
            canvas.FontColor = Colors.White;
            string label = $"{percentage * 100:F0}%";
            
            // 4. THE FIX: Center the text box on the point
            // We define a box of 60x30
            float boxWidth = 60;
            float boxHeight = 30;

            // We subtract half the width/height from the coordinates
            // This ensures 'textX' is the dead-center of the text, not the top-left
            canvas.DrawString(
                label, 
                textX - (boxWidth / 2), 
                textY - (boxHeight / 2), 
                boxWidth, 
                boxHeight, 
                HorizontalAlignment.Center, 
                VerticalAlignment.Center
            );

            // Advance
            startAngle += sweep;
        }
    }

    private static readonly Color[] Palette = new[]
    {
        Color.FromArgb("#4CC9F0"),
        Color.FromArgb("#4895EF"),
        Color.FromArgb("#560BAD"),
        Color.FromArgb("#F72585"),
        Color.FromArgb("#2A9D8F"),
    };

    private Color GetColorForIndex(int i)
    {
        return Palette[i % Palette.Length];
    }
}