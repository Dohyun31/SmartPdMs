using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPdM.Models;

public enum ConsumableState { Normal, Warning, Exceeded }

public class ProbePin
{
    public string Name { get; set; } = "Probe Pin";

    public int MaxCycles { get; set; } = 100_000;   // 최대 수명
    public int WarnCycles { get; set; } = 90_000;   // 경고 임계치
    public int CurrentCycles { get; set; } = 0;     // 현재 누적

    public DateTimeOffset LastResetAt { get; set; } = DateTimeOffset.UtcNow;

    // 파생값
    public int Remaining => Math.Max(0, MaxCycles - CurrentCycles);
    public double UsageRatio => MaxCycles <= 0 ? 0 : (double)CurrentCycles / MaxCycles;

    public bool IsWarning => CurrentCycles >= WarnCycles && CurrentCycles < MaxCycles;
    public bool IsExceeded => CurrentCycles >= MaxCycles;

    public ConsumableState State =>
        IsExceeded ? ConsumableState.Exceeded :
        IsWarning ? ConsumableState.Warning :
                     ConsumableState.Normal;

    public void Reset()
    {
        CurrentCycles = 0;
        LastResetAt = DateTimeOffset.UtcNow;
    }

    public void Increment(int n) => CurrentCycles = Math.Max(0, CurrentCycles + n);
}
