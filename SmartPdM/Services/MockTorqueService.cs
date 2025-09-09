using SmartPdM.Models;

namespace SmartPdM.Services;

public static class MockTorqueService
{
    /// <summary>
    /// 가짜 체결 곡선 생성기.
    /// target=목표 피크 토크(N·m), tol=허용 편차(±), noise=랜덤 노이즈 비율(0~1)
    /// </summary>
    public static List<TorqueCycle> Generate(int count = 50, double target = 1.2, double tol = 0.15, double noise = 0.06)
    {
        var rnd = new Random();
        var list = new List<TorqueCycle>(count);

        for (int k = 0; k < count; k++)
        {
            // 각 싸이클의 실제 피크값(목표±편차, 실패 샘플 약간 섞기)
            var drift = (rnd.NextDouble() * 2 - 1) * tol;
            var peak = target + drift;

            // 가끔 NG 만들기 (언더/오버)
            if (rnd.NextDouble() < 0.12)
                peak += (rnd.Next(0, 2) == 0 ? -1 : 1) * (tol + 0.08);

            // 시간축 0~1.2s, 120 샘플
            int n = 120;
            double totalT = 1.2;
            var pts = new List<TorquePoint>(n);
            double tAtPeak = 0;
            double actualPeak = 0;

            for (int i = 0; i < n; i++)
            {
                double t = i * (totalT / (n - 1)); // 0~1.2s
                // 체결 특성: 초반 완만, 중후반 급상승, 피크 후 약간 릴리즈
                // 로지스틱 + 지수 꼬리로 근사
                double baseCurve = peak / (1 + Math.Exp(-10 * (t - 0.7))); // 0~peak
                double tail = 0.15 * peak * Math.Exp(-10 * Math.Max(0, t - 0.9)); // 릴리즈

                double tor = Math.Max(0, baseCurve - tail);

                // 노이즈
                var nmag = 1 + (rnd.NextDouble() * 2 - 1) * noise;
                tor *= nmag;

                pts.Add(new TorquePoint { Time = t, Torque = tor });

                if (tor > actualPeak)
                {
                    actualPeak = tor;
                    tAtPeak = t;
                }
            }

            var isOk = actualPeak >= (target - tol) && actualPeak <= (target + tol);

            list.Add(new TorqueCycle
            {
                CycleNo = k + 1,
                Timestamp = DateTime.Now.AddSeconds(-(count - k) * 3),
                Points = pts,
                PeakTorque = Math.Round(actualPeak, 3),
                TightenTime = Math.Round(tAtPeak, 3),
                IsOk = isOk
            });
        }

        return list.OrderByDescending(c => c.Timestamp).ToList();
    }
}
