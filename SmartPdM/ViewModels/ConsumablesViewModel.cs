using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartPdM.Models;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace SmartPdM.ViewModels;

// CommunityToolkit.Mvvm 패키지 필요 (이미 설치됨)
public partial class ConsumablesViewModel : ObservableObject
{
    [ObservableProperty] private bool isSimulating;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private ProbePin pin;

    private IDispatcherTimer? _timer;
    private bool _alarmLatched; // 최초 경고 진입 시 한번만 울리게 제어(확장 포인트)

    public ObservableCollection<string> Events { get; } = new();

    public ConsumablesViewModel()
    {
        // 초기값 (필요 시 여기서 WarnCycles/MaxCycles 바꿔도 됨)
        Pin = new ProbePin
        {
            Name = "Probe Pin",
            MaxCycles = 100_000,
            WarnCycles = 90_000,
            CurrentCycles = 0
        };

        // 타이머 준비(Off)
        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += (s, e) =>
        {
            Increment(25); // 시뮬레이션: 0.2초마다 +25회
        };
    }

    // --------- Commands ---------

    [RelayCommand]
    private void ToggleSimulate()
    {
        if (_timer is null) return;

        if (IsSimulating)
        {
            _timer.Stop();
            IsSimulating = false;
            AddEvent("시뮬레이터: 중지");
        }
        else
        {
            _timer.Start();
            IsSimulating = true;
            AddEvent("시뮬레이터: 시작");
        }
    }

    [RelayCommand]
    private void Reset()
    {
        Pin.Reset();
        _alarmLatched = false; // 알람 해제
        OnPropertyChanged(nameof(Pin));
        OnPropertyChanged(nameof(StateColor));
        AddEvent("설비 초기화(Reset) 수행");
    }

    [RelayCommand]
    private void Increment1() => Increment(1);

    [RelayCommand]
    private void Increment100() => Increment(100);

    [RelayCommand]
    private void Increment1000() => Increment(1000);

    [RelayCommand]
    private void ApplyWarnCycles(string? text)
    {
        if (int.TryParse(text, out var v) && v > 0 && v <= Pin.MaxCycles)
        {
            Pin.WarnCycles = v;
            OnPropertyChanged(nameof(Pin));
            OnPropertyChanged(nameof(StateColor));
            AddEvent($"경고 임계치 변경: {v:N0}");
            CheckAlarm();
        }
        else
        {
            AddEvent("경고 임계치 입력 오류");
        }
    }

    private void Increment(int n)
    {
        Pin.Increment(n);
        OnPropertyChanged(nameof(Pin));
        OnPropertyChanged(nameof(StateColor));
        CheckAlarm();
    }

    // --------- 알람/상태 표시 ---------

    public string StateText => Pin.State switch
    {
        ConsumableState.Normal => "정상",
        ConsumableState.Warning => "경고",
        ConsumableState.Exceeded => "교체 필요",
        _ => "알 수 없음"
    };

    public Color StateColor => Pin.State switch
    {
        ConsumableState.Normal => Colors.Green,
        ConsumableState.Warning => Colors.Orange,
        ConsumableState.Exceeded => Colors.Red,
        _ => Colors.Gray
    };

    private void CheckAlarm()
    {
        // 상태 텍스트/색상 갱신 알림
        OnPropertyChanged(nameof(StateText));
        OnPropertyChanged(nameof(StateColor));

        // 최초 경고 진입 시 이벤트 남김(벨/토스트 등 확장 포인트)
        if (Pin.IsWarning && !_alarmLatched)
        {
            _alarmLatched = true;
            AddEvent($"경고 임계치 초과! 현재 {Pin.CurrentCycles:N0} 회");
            // TODO: 소리/진동/로컬 알림 등 연결 가능
        }

        if (Pin.IsExceeded)
        {
            AddEvent($"최대 수명 초과! 현재 {Pin.CurrentCycles:N0} 회 (교체 필요)");
        }
    }

    private void AddEvent(string msg)
    {
        var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
        Events.Insert(0, line);
        // 리스트가 너무 커지지 않도록
        if (Events.Count > 200) Events.RemoveAt(Events.Count - 1);
    }
}
