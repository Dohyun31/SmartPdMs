using SmartPdM.Drawables;
using SmartPdM.ViewModels;

namespace SmartPdM.App.Views;

public partial class ScrewHealthPage : ContentPage
{
    private TorqueCurveDrawable? _drawable;

    public ScrewHealthPage()
    {
        InitializeComponent();

        var vm = (ScrewHealthViewModel)BindingContext;

        _drawable = new TorqueCurveDrawable(() => vm.Selected);
        CurveView.Drawable = _drawable;

        // 선택 변경 시 그래프 갱신
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(ScrewHealthViewModel.Selected))
                MainThread.BeginInvokeOnMainThread(() => CurveView.Invalidate());
        };
    }
}
