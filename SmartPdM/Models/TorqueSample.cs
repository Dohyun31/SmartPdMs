namespace SmartPdM.Models;

public class TorquePoint
{
    public double Time { get; set; }   // sec
    public double Torque { get; set; } // N·m
}

public class TorqueCycle
{
    public int CycleNo { get; set; }
    public DateTime Timestamp { get; set; }
    public List<TorquePoint> Points { get; set; } = new();

    // 요약값
    public double PeakTorque { get; set; }
    public double TightenTime { get; set; } // 0~peak까지 걸린 시간
    public bool IsOk { get; set; }
    public string Judgment => IsOk ? "OK" : "NG";
}
