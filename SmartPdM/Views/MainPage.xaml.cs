using SmartPdM.ViewModels;


namespace SmartPdM.App.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Kick-off initial load when page appears
            Appearing += async (_, __) =>
            {
                if (BindingContext is MainViewModel vm)
                    await vm.RefreshAsync();
            };
        }
    }
}