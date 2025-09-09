using SmartPdM.Drawables;
using SmartPdM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SmartPdM.App.Views;

public partial class RepeatPrecisionPage : ContentPage
{
    private RepeatPrecisionChartDrawable? _drawable;

    public RepeatPrecisionPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as RepeatPrecisionViewModel;
        if (vm == null) return;

        // 처음 1회 로드
        if (vm.Count == 0)
            await vm.LoadAsyncCommand.ExecuteAsync(null);   // ← Async로 호출 권장

        Chart.Drawable ??= new RepeatPrecisionChartDrawable(() => vm.Errors, "Position Error [mm]");
        Chart.Invalidate();
    }
}
