using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace Hex_Handler_App.Views.Controls
{
    public partial class LoadAnimation : UserControl
    {
        public LoadAnimation()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var animation = (Storyboard)FindResource("loadAnimation");
            if ((bool)e.NewValue)
            {                
                animation.Begin();
            }
            else
            {
                animation.Stop();
            }

        }
    }
}
