using Microsoft.Extensions.DependencyInjection;
using SmartPdM.ViewModels;

namespace SmartPdM.App.Views;

public partial class SpecPage : ContentPage
{
    public SpecPage()  // ← 기본 생성자
    {
        InitializeComponent();

        // DI 컨테이너에서 VM 해석 (안전)
        var vm = MauiProgram.Services.GetRequiredService<SpecViewModel>();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SpecViewModel m)
            await m.LoadAsync();
    }
}
