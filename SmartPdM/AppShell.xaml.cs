using SmartPdM.App.Views;

namespace SmartPdM;                   // ★ AppShell.xaml 의 x:Class 와 동일해야 함

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // 페이지 라우트 등록
        Routing.RegisterRoute(nameof(RepeatPrecisionPage), typeof(RepeatPrecisionPage));
        Routing.RegisterRoute(nameof(ScrewHealthPage), typeof(ScrewHealthPage));
        Routing.RegisterRoute(nameof(ConsumablesPage), typeof(ConsumablesPage));
        Routing.RegisterRoute(nameof(InfraPlatformPage), typeof(InfraPlatformPage));
    }
}