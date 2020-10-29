using Hex_Handler_App.ViewModel;
using Hex_Handler_App.Views.Windows;
using System.Windows;

namespace Hex_Handler_App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel();
            mainWindow.Show();
        }
    }
}
