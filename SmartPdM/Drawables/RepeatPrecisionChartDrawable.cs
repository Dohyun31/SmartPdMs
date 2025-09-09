using Microsoft.Maui.Graphics;

namespace SmartPdM.Drawables;

public class RepeatPrecisionChartDrawable : IDrawable
{
    private readonly Func<IReadOnlyList<double>> _getSeries;
    private readonly string _title;

    public RepeatPrecisionChartDrawable(Func<IReadOnlyList<double>> getSeries, string title = "Position Error [mm]")
    {
        _getSeries = getSeries;
        _title = title;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var W = dirtyRect.Width;
        var H = dirtyRect.Height;
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(dirtyRect);

        // 스타일
        var axis = new Color(0.7f, 0.7f, 0.7f);
        var text = new Color(0.25f, 0.25f, 0.25f);
        var line = Color.FromArgb("#512BD4");

        // 여백
        float left = 48, right = 16, top = 28, bottom = 34;
        var plot = new RectF(left, top, W - left - right, H - top - bottom);

        // 배경/테두리
        canvas.StrokeColor = axis;
        canvas.StrokeSize = 1;
        canvas.DrawRectangle(plot);

        // 타이틀
        canvas.FontSize = 12;
        canvas.FontColor = text;
        canvas.DrawString(_title, left, 6, W - left - right, 20, HorizontalAlignment.Center, VerticalAlignment.Center);

        // 시리즈
        var s = _getSeries() ?? Array.Empty<double>();
        if (s.Count < 2) return;

        var min = s.Min();
        var max = s.Max();
        if (Math.Abs(max - min) < 1e-9) { max = min + 1e-3; } // flat guard

        float n = s.Count - 1;
        canvas.StrokeColor = line;
        canvas.StrokeSize = 3;
        canvas.StrokeLineJoin = LineJoin.Round;
        canvas.StrokeLineCap = LineCap.Round;

        float X(float i) => plot.Left + (i / n) * plot.Width;
        float Y(double v)
        {
            var t = (float)((v - min) / (max - min));    // 0..1
            return plot.Bottom - t * plot.Height;        // invert
        }

        var path = new PathF();
        path.MoveTo(X(0), Y(s[0]));
        for (int i = 1; i < s.Count; i++)
            path.LineTo(X(i), Y(s[i]));
        canvas.DrawPath(path);

        // Y축 눈금(최소/최대/평균)
        canvas.FontSize = 10;
        canvas.FontColor = axis;
        canvas.DrawString($"{min:F3}", 0, Y(min) - 8, left - 6, 16, HorizontalAlignment.Right, VerticalAlignment.Center);
        canvas.DrawString($"{max:F3}", 0, Y(max) - 8, left - 6, 16, HorizontalAlignment.Right, VerticalAlignment.Center);
        var mean = s.Average();
        canvas.DrawString($"{mean:F3}", 0, Y(mean) - 8, left - 6, 16, HorizontalAlignment.Right, VerticalAlignment.Center);

        // 평균선
        canvas.StrokeColor = axis;
        canvas.StrokeSize = 1;
        canvas.DrawLine(plot.Left, Y(mean), plot.Right, Y(mean));
    }
}
