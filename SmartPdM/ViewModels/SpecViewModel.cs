using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartPdM.Models;
using SmartPdM.Services.Spec;

namespace SmartPdM.ViewModels;

public partial class SpecViewModel : ObservableObject
{
    private readonly ISpecStore _store;

    [ObservableProperty] private InspectionSpec spec = new();
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private bool isAdmin = true;   // 관리자 모드 토글
    [ObservableProperty] private string? message;

    public SpecViewModel(ISpecStore store) => _store = store;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            Spec = await _store.LoadAsync();
            Message = "사양 로드 완료";
        }
        catch (Exception ex) { Message = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (!IsAdmin)
        {
            Message = "관리자 모드에서만 저장할 수 있습니다.";
            return;
        }
        try
        {
            await _store.SaveAsync(Spec);
            Message = "사양 저장 완료";
        }
        catch (Exception ex) { Message = ex.Message; }
    }

    [RelayCommand]
    public async Task ResetAsync()
    {
        if (!IsAdmin)
        {
            Message = "관리자 모드에서만 초기화할 수 있습니다.";
            return;
        }
        await _store.ResetAsync();
        await LoadAsync();
        Message = "기본값으로 초기화";
    }

    [RelayCommand]
    public async Task ToggleAdminAsync()
    {
        // 간단 보호: PIN 확인(향후: 로그인/권한 연동)
        string pin = await Shell.Current.DisplayPromptAsync("관리자 모드", "PIN을 입력하세요 (기본: 0000)", "확인", "취소", "0000", maxLength: 8, keyboard: Keyboard.Numeric, initialValue: "");
        if (pin == "0000") { IsAdmin = !IsAdmin; Message = IsAdmin ? "관리자 모드 ON" : "관리자 모드 OFF"; }
        else Message = "PIN이 올바르지 않습니다.";
    }
}
