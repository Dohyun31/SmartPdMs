using System;
using System.Collections.Generic;

namespace SmartPdM.Services   // 폴더/네임스페이스 맞춰주세요
{
    public class RobotPose
    {
        public DateTimeOffset Timestamp { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }
    }

    public static class MockData
    {
        private const double X0 = 500, Y0 = 200, Z0 = 150;
        private const double RX0 = 0.10, RY0 = -0.20, RZ0 = 0.00;
        private static readonly Random _rnd = new(12345);

        private static double Jitter(double sigma, int i, double phase) =>
            (NextNormal() * sigma) + Math.Sin((i + phase) * 0.12) * sigma * 0.6;

        private static double NextNormal()
        {
            var u1 = 1.0 - _rnd.NextDouble();
            var u2 = 1.0 - _rnd.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
        }

        public static List<RobotPose> GetRobotPoses(int count = 100)
        {
            var list = new List<RobotPose>(count);
            var t0 = DateTimeOffset.UtcNow.AddSeconds(-count);

            for (int i = 0; i < count; i++)
            {
                list.Add(new RobotPose
                {
                    Timestamp = t0.AddSeconds(i),
                    X = X0 + Jitter(0.020, i, 0.0),
                    Y = Y0 + Jitter(0.020, i, 0.3),
                    Z = Z0 + Jitter(0.020, i, 0.6),
                    Rx = RX0 + Jitter(0.020, i, 0.9),
                    Ry = RY0 + Jitter(0.020, i, 1.2),
                    Rz = RZ0 + Jitter(0.020, i, 1.5)
                });
            }
            return list;
        }
    }
}
