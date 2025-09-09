namespace SmartPdM.Models;

public class InspectionSpec
{
    // ── 소모품(Probe Pin)
    public int ProbePinMaxCycles { get; set; } = 100_000;
    public int ProbePinWarnCycles { get; set; } = 90_000;

    // ── 설비 반복 정밀도 (확장용, 기본값 예시)
    public double RepeatPosToleranceMm { get; set; } = 0.10;   // 허용 오차(mm)
    public int RepeatSamplesPerCheck { get; set; } = 100;      // 샘플 수

    // ── 체결 공정(토크) (확장용, 기본값 예시)
    public double TargetTorqueNm { get; set; } = 1.20;
    public double TorqueToleranceNm { get; set; } = 0.15;

    // 왜/배경/특이사항 기록
    public string Notes { get; set; } = "불량 원인 및 조치 사항을 여기에 기록합니다.";
}
