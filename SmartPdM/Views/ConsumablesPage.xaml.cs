using Microsoft.Extensions.DependencyInjection;
using SmartPdM.ViewModels;

namespace SmartPdM.App.Views;

public partial class ConsumablesPage : ContentPage
{
    public ConsumablesPage()
    {
        InitializeComponent();
        var vm = MauiProgram.Services.GetRequiredService<ConsumablesViewModel>();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ConsumablesViewModel m)
            await m.OnAppearingAsync(); // 스펙 재로드가 필요하면
    }
}
