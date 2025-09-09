using Microsoft.Maui.Graphics;
using SmartPdM.Models;

namespace SmartPdM.Drawables;

public class TorqueCurveDrawable : IDrawable
{
    private readonly Func<TorqueCycle?> _get;

    public TorqueCurveDrawable(Func<TorqueCycle?> getCycle) => _get = getCycle;

    public void Draw(ICanvas canvas, RectF r)
    {
        var c = _get();
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(r);

        if (c is null || c.Points.Count < 2) return;

        // 패딩
        float left = 48, right = 12, top = 12, bottom = 30;
        var plot = new RectF(left, top, r.Width - left - right, r.Height - top - bottom);

        // 축 범위
        double tMin = c.Points.Min(p => p.Time);
        double tMax = c.Points.Max(p => p.Time);
        double yMin = 0;
        double yMax = Math.Max(1e-3, c.Points.Max(p => p.Torque) * 1.1);

        float X(double t) => plot.Left + (float)((t - tMin) / (tMax - tMin) * plot.Width);
        float Y(double v) => plot.Bottom - (float)((v - yMin) / (yMax - yMin) * plot.Height);

        // 경계
        canvas.StrokeColor = Colors.Gray.WithAlpha(0.5f);
        canvas.StrokeSize = 1;
        canvas.DrawRectangle(plot);

        // 곡선
        canvas.StrokeColor = Color.FromArgb("#512BD4");
        canvas.StrokeSize = 3;
        var path = new PathF();
        path.MoveTo(X(c.Points[0].Time), Y(c.Points[0].Torque));
        for (int i = 1; i < c.Points.Count; i++)
            path.LineTo(X(c.Points[i].Time), Y(c.Points[i].Torque));
        canvas.DrawPath(path);

        // 피크 점 표시
        var peak = c.PeakTorque;
        canvas.StrokeColor = Colors.Orange;
        canvas.StrokeSize = 1;
        canvas.DrawLine(plot.Left, Y(peak), plot.Right, Y(peak));
        canvas.FontSize = 10;
        canvas.FontColor = Colors.Orange;
        canvas.DrawString($"Peak {peak:F2} N·m", plot.Left + 4, Y(peak) - 14, plot.Width, 14, HorizontalAlignment.Left, VerticalAlignment.Center);
    }
}
