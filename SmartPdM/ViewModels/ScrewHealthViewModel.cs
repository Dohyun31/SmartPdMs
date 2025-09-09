//using Android.Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartPdM.Models;
using SmartPdM.Services;
using System.Collections.ObjectModel;

namespace SmartPdM.ViewModels;

public partial class ScrewHealthViewModel : ObservableObject
{
    // 목표/허용치
    [ObservableProperty] private double targetTorque = 1.2;  // N·m
    [ObservableProperty] private double tolerance = 0.15;    // ±
    [ObservableProperty] private double noise = 0.06;

    public ObservableCollection<TorqueCycle> Cycles { get; } = new();
    [ObservableProperty] private TorqueCycle? selected;

    // 통계
    [ObservableProperty] private int total;
    [ObservableProperty] private int okCount;
    [ObservableProperty] private int ngCount;
    [ObservableProperty] private double meanPeak;
    [ObservableProperty] private double maxPeak;
    [ObservableProperty] private double minPeak;

    public ScrewHealthViewModel()
    {
        LoadMock();
    }

    [RelayCommand]
    private void Refresh() => LoadMock();

    [RelayCommand]
    private void Pick(TorqueCycle? c)
    {
        Selected = c;
    }

    private void LoadMock()
    {
        var data = MockTorqueService.Generate(50, TargetTorque, Tolerance, Noise);

        Cycles.Clear();
        foreach (var c in data) Cycles.Add(c);

        Total = Cycles.Count;
        OkCount = Cycles.Count(c => c.IsOk);
        NgCount = Total - OkCount;
        MeanPeak = Math.Round(Cycles.Average(c => c.PeakTorque), 3);
        MaxPeak = Math.Round(Cycles.Max(c => c.PeakTorque), 3);
        MinPeak = Math.Round(Cycles.Min(c => c.PeakTorque), 3);

        Selected = Cycles.FirstOrDefault();
    }
}
