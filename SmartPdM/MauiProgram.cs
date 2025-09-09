using Microsoft.Extensions.Logging;
using SmartPdM.Services;
using SmartPdM.ViewModels;
using SmartPdM.Services.Auth;
using SmartPdM.Services.Storage;
using SmartPdM.Services.Sync;
using SmartPdM.Services.Spec;

namespace SmartPdM
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; } = default!;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
            .UseMauiApp<MyApp>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


            // Base URL placeholder — change later to actual API URL
            const string BASE_URL = "https://YOUR_API_BASE/";

            builder.Services.AddTransient<SmartPdM.ViewModels.SpecViewModel>();
            builder.Services.AddTransient<SmartPdM.App.Views.SpecPage>();
            
            builder.Services.AddSingleton<IAuthService, InMemoryAuthService>();
            builder.Services.AddSingleton<ILocalStore, PreferencesStore>();
            builder.Services.AddSingleton<ISyncService, NoopSyncService>();

            builder.Services.AddSingleton<ISpecStore, PreferencesSpecStore>();

            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SmartPdM.App.Views.SignUpPage>();

            builder.Services.AddTransient<SmartPdM.ViewModels.SignUpViewModel>();

            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(BASE_URL) });
            builder.Services.AddSingleton<ApiClient>();
            builder.Services.AddSingleton<MainViewModel>();


            // Views registration (if using DI for pages)
            // builder.Services.AddTransient<MainPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif
            // 🔹 Build 한 뒤 서비스 프로바이더 저장
            var app = builder.Build();
            Services = app.Services;
            return app;
        }
    }
}