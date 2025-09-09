using System.Collections.ObjectModel;
using System.Windows.Input;
using SmartPdM.Models;
using SmartPdM.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartPdM.ViewModels
{

    public partial class MainViewModel : BindableObject  // ← public 유지
    {
        public IAsyncRelayCommand OpenRepeatPrecisionCommand { get; }
        public IAsyncRelayCommand OpenScrewHealthCommand { get; }
        public IAsyncRelayCommand OpenConsumablesCommand { get; }
        public IAsyncRelayCommand OpenInfraPlatformCommand { get; }

        private readonly ApiClient _api;
        public ObservableCollection<DashboardItem> Items { get; } = new();

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        public ICommand RefreshCommand { get; }

        // ★★★ 매개변수 없는 public 생성자 추가 (XAML에서 사용할 것)
        private const string BASE_URL = "https://YOUR_API_BASE/";  // 나중에 실제 주소로 교체
        public MainViewModel() : this(new ApiClient(new HttpClient { BaseAddress = new Uri(BASE_URL) }))
        {
            OpenRepeatPrecisionCommand = new AsyncRelayCommand(() =>
                Shell.Current.GoToAsync(nameof(SmartPdM.App.Views.RepeatPrecisionPage)));

            OpenScrewHealthCommand = new AsyncRelayCommand(() =>
                Shell.Current.GoToAsync(nameof(SmartPdM.App.Views.ScrewHealthPage)));

            OpenConsumablesCommand = new AsyncRelayCommand(() =>
                Shell.Current.GoToAsync(nameof(SmartPdM.App.Views.ConsumablesPage)));

            //OpenInfraPlatformCommand = new AsyncRelayCommand(() =>
            //    Shell.Current.GoToAsync(nameof(SmartPdM.App.Views.InfraPlatformPage)));

            OpenInfraPlatformCommand = new AsyncRelayCommand(() =>
                Shell.Current.GoToAsync(nameof(SmartPdM.App.Views.SpecPage)));
        }
        [RelayCommand]
        private Task GoToSpecAsync() => Shell.Current.GoToAsync("Spec");

        [RelayCommand]
        private async Task GoToSignUpAsync()
        {
            // 위에서 등록한 문자열 라우트와 동일해야 합니다!
            await Shell.Current.GoToAsync("SignUp");
        }

        private async Task OpenRepeatPrecisionAsync()
        {
            // Shell에 등록한 라우트 이름으로 이동
            await Shell.Current.GoToAsync(nameof(SmartPdM.App.Views.RepeatPrecisionPage));
            // 또는 await Shell.Current.GoToAsync(nameof(RepeatPrecisionPage)); (using 추가 시)
        }
        // 기존 DI 생성자 (그대로 유지)
        public MainViewModel(ApiClient api)
        {
            _api = api;
            RefreshCommand = new Command(async () => await RefreshAsync());
        }

        public async Task RefreshAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Items.Clear();
                var data = await _api.GetDashboardSummaryAsync();
                foreach (var x in data) Items.Add(x);
            }
            finally { IsBusy = false; }
        }
    }
}
