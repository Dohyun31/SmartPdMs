using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartPdM.Models;
using SmartPdM.Services.Spec;   // ★ 스펙 저장소
using System.Collections.ObjectModel;

namespace SmartPdM.ViewModels;

public partial class ConsumablesViewModel : ObservableObject
{
    private readonly ISpecStore _store;       // ★ 추가: 스펙 저장소
    private IDispatcherTimer? _timer;
    private bool _alarmLatched;

    [ObservableProperty] private bool isSimulating;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private ProbePin pin;

    public ObservableCollection<string> Events { get; } = new();

    // ★ DI로 ISpecStore 주입
    public ConsumablesViewModel(ISpecStore store)
    {
        _store = store;

        // 기본값(스펙 로드 전 초기값)
        Pin = new ProbePin
        {
            Name = "Probe Pin",
            MaxCycles = 100_000,
            WarnCycles = 90_000,
            CurrentCycles = 0
        };

        // 타이머 준비
        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += (s, e) => Increment(25); // 0.2초마다 +25

        // ★ 스펙 로드(비동기 시작)
        _ = LoadSpecAsync();
    }

    // 페이지 진입 시 다시 스펙 동기화 하고 싶으면, 페이지 OnAppearing에서 호출
    public async Task OnAppearingAsync() => await LoadSpecAsync();

    // ★ 스펙 로드
    [RelayCommand]
    private async Task LoadSpecAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var spec = await _store.LoadAsync();
            Pin.MaxCycles = spec.ProbePinMaxCycles;
            Pin.WarnCycles = spec.ProbePinWarnCycles;

            OnPropertyChanged(nameof(Pin));
            OnPropertyChanged(nameof(StateColor));
            OnPropertyChanged(nameof(StateText));

            AddEvent($"스펙 로드: Max={Pin.MaxCycles:N0}, Warn={Pin.WarnCycles:N0}");
        }
        catch (Exception ex)
        {
            AddEvent("스펙 로드 실패: " + ex.Message);
        }
        finally { IsBusy = false; }
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
        _alarmLatched = false;
        OnPropertyChanged(nameof(Pin));
        OnPropertyChanged(nameof(StateColor));
        OnPropertyChanged(nameof(StateText));
        AddEvent("설비 초기화(Reset) 수행");
    }

    [RelayCommand] private void Increment1() => Increment(1);
    [RelayCommand] private void Increment100() => Increment(100);
    [RelayCommand] private void Increment1000() => Increment(1000);

    [RelayCommand]
    private void ApplyWarnCycles(string? text)
    {
        if (int.TryParse(text, out var v) && v > 0 && v <= Pin.MaxCycles)
        {
            Pin.WarnCycles = v;
            OnPropertyChanged(nameof(Pin));
            OnPropertyChanged(nameof(StateColor));
            OnPropertyChanged(nameof(StateText));
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
        OnPropertyChanged(nameof(StateText));
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
        OnPropertyChanged(nameof(StateText));
        OnPropertyChanged(nameof(StateColor));

        if (Pin.IsWarning && !_alarmLatched)
        {
            _alarmLatched = true;
            AddEvent($"경고 임계치 초과! 현재 {Pin.CurrentCycles:N0} 회");
            // TODO: 소리/진동/로컬 알림 등
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
        if (Events.Count > 200) Events.RemoveAt(Events.Count - 1);
    }
}
