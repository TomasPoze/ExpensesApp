namespace ExpensesApp.MAUI.Drawables;

public class PieChartDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float[] testValues = { 25, 25, 50 };
        float total = testValues.Sum();
        float startAngle = 90;
        var progressAngle = total / 100;
        var endAngle = startAngle - (int)Math.Round(progressAngle * 360, MidpointRounding.AwayFromZero);

        var centerX = dirtyRect.Width / 2;
        var centerY = dirtyRect.Height / 2;
        var radius = dirtyRect.Width / 2;


        for (int i = 0; i < testValues.Length; i++)
        {
            progressAngle = testValues[i] / total * 360f;
            endAngle += startAngle+progressAngle;
            if (i == 0) canvas.FillColor = Colors.Red;
            else if (i == 1) canvas.FillColor = Colors.Green;
            else canvas.FillColor = Colors.Blue;
            canvas.DrawCircle(centerX, centerY, radius);
            var path = new PathF();
            path.MoveTo(centerX, centerY);
            path.AddArc(centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, endAngle, true);
            path.LineTo(centerX, centerY);
            path.AddArc(centerX, centerY, 200, 200, startAngle, endAngle, true);
            canvas.FillPath(path);


            canvas.DrawArc(centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, endAngle, true,
                false);
            startAngle += progressAngle;
        }
    }
}