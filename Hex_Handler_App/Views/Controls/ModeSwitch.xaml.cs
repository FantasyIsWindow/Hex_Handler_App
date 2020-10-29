using System.Windows;
using System.Windows.Controls;
using Hex_Handler_App.Infrastructure.Enums;

namespace Hex_Handler_App.Views.Controls
{
    public partial class ModeSwitch : UserControl
    {
        private static readonly DependencyProperty CheckedProperty;

        private static readonly DependencyProperty IsCheckedProprety;

        public Mode Checked
        {
            get => (Mode)GetValue(CheckedProperty);
            set => SetValue(CheckedProperty, value);
        }

        public bool? IsChecked
        {
            get => (bool?)GetValue(IsCheckedProprety);
            set => SetValue(IsCheckedProprety, value);
        }

        public ModeSwitch()
        {
            InitializeComponent();
        }

        static ModeSwitch()
        {
            CheckedProperty = DependencyProperty.Register("Checked", typeof(Mode), typeof(ModeSwitch), new FrameworkPropertyMetadata(Mode.Null));
            IsCheckedProprety = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(ModeSwitch), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SetCheckedValue)));
        }

        private static void SetCheckedValue(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ModeSwitch)d).SetValue();        

        private void SetValue() => checkBox.IsChecked = IsChecked;        

        private void CheckBox_Checked(object sender, RoutedEventArgs e) => Checked = Mode.Key;

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) => Checked = Mode.Hash;

        private void CheckBox_Indeterminate(object sender, RoutedEventArgs e) => Checked = Mode.Null;
    }
}
