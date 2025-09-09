using Foundation;
using SmartPdM.App;   // MauiProgram 네임스페이스
namespace SmartPdM
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
