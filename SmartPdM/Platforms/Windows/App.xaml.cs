using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SmartPdM.WinUI;
public partial class App : MauiWinUIApplication
{
    public App() { }

    protected override MauiApp CreateMauiApp()
        => SmartPdM.MauiProgram.CreateMauiApp(); // 공용 MauiProgram 호출
}