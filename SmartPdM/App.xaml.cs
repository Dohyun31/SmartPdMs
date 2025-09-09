using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace SmartPdM;
public partial class MyApp : Application
{
    public MyApp()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}