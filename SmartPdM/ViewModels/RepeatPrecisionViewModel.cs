using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartPdM.Services;

namespace SmartPdM.ViewModels;

public partial class RepeatPrecisionViewModel : ObservableObject
{
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private double errorMax, errorMean, sigma;
    [ObservableProperty] private int count;

    public ObservableCollection<RobotPose> Poses { get; } = new();
    public ObservableCollection<double> Errors { get; } = new();

    // ⚠️ 명시적으로 커맨드 속성 선언
    public IAsyncRelayCommand LoadAsyncCommand { get; }

    public RepeatPrecisionViewModel()
    {
        // 커맨드 인스턴스 생성
        LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
    }

    // 실제 로딩 로직
    private async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            var list = MockData.GetRobotPoses(100);

            Poses.Clear();
            foreach (var p in list) Poses.Add(p);
            Count = Poses.Count;

            // 기준점(센트로이드) 기준 거리 계산
            var cx = Poses.Average(p => p.X);
            var cy = Poses.Average(p => p.Y);
            var cz = Poses.Average(p => p.Z);

            static double Dist(double x, double y, double z, double cx, double cy, double cz)
                => Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy) + (z - cz) * (z - cz));

            var errs = Poses.Select(p => Dist(p.X, p.Y, p.Z, cx, cy, cz)).ToArray();
            ErrorMax = errs.Max();
            ErrorMean = errs.Average();
            var mean = ErrorMean;
            Sigma = Math.Sqrt(errs.Select(e => (e - mean) * (e - mean)).Average());

            Errors.Clear();
            foreach (var e in errs) Errors.Add(e);
        }
        finally { IsBusy = false; }
    }
}
