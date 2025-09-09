using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
//using IntelliJ.Lang.Annotations;
using SmartPdM.Services.Auth;

namespace SmartPdM.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    private readonly IAuthService _auth;

    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string displayName = "";
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;

    public SignUpViewModel(IAuthService auth) => _auth = auth;

    [RelayCommand]
    private async Task SignUpAsync()
    {
        Error = null;
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(DisplayName))
            {
                Error = "모든 항목을 입력하세요.";
                return;
            }

            var user = await _auth.SignUpAsync(new SignUpRequest(Email.Trim(), Password, DisplayName.Trim()));

            // 가입 성공 → 메인/프로필 등으로 이동
            await Shell.Current.DisplayAlert("가입 완료", $"{user.DisplayName}님 환영합니다!", "확인");
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally { IsBusy = false; }
    }
}
