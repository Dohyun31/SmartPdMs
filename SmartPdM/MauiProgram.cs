using Microsoft.Extensions.Logging;
using SmartPdM.Services;
using SmartPdM.ViewModels;


namespace SmartPdM
{
    public static class MauiProgram
    {
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


            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(BASE_URL) });
            builder.Services.AddSingleton<ApiClient>();
            builder.Services.AddSingleton<MainViewModel>();


            // Views registration (if using DI for pages)
            // builder.Services.AddTransient<MainPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}