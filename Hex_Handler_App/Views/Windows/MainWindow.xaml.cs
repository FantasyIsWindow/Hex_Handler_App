using System.Windows;

namespace Hex_Handler_App.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_CloseWindow(object sender, RoutedEventArgs e) => this.Close();
    }
}
