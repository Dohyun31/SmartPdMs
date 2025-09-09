namespace SmartPdM.Models
{
    public class DashboardItem
    {
        public string RobotId { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double ErrorMax { get; set; }
        public double ErrorMean { get; set; }
        public double Sigma { get; set; }
    }
}