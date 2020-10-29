using Hex_Handler_App.Infrastructure.Enums;
using Hex_Handler_App.Infrastructure.EventsArgs;
using Hex_Handler_App.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hex_Handler_App.Views.Controls
{
    public partial class PopupWindow : UserControl
    {
        internal ObservableCollection<HexadecimalKeyModel> FoundDataCollection { get; set; }

        internal static readonly DependencyProperty ElementParentProperty;

        internal static readonly DependencyProperty OriginCollectionProperty;

        internal static readonly DependencyProperty SelectedItemProperty;

        internal static readonly DependencyProperty ShowModeProperty;

        internal static readonly DependencyProperty MessageProperty;

        public UIElement ElementParent
        {
            get => (UIElement)GetValue(ElementParentProperty);
            set => SetValue(ElementParentProperty, value);
        }

        public ObservableCollection<HexadecimalKeyModel> OriginCollection
        {
            get => (ObservableCollection<HexadecimalKeyModel>)GetValue(OriginCollectionProperty);
            set => SetValue(OriginCollectionProperty, value);
        }

        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public Mode ShowMode
        {
            get => (Mode)GetValue(ShowModeProperty);
            set => SetValue(ShowModeProperty, value);
        }

        public Action<object, EventArgs> Message
        {
            get => (Action<object, EventArgs>)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public PopupWindow()
        {
            InitializeComponent();
            FoundDataCollection = new ObservableCollection<HexadecimalKeyModel>();
        }

        static PopupWindow()
        {
            ElementParentProperty = DependencyProperty.Register("ElementParent", typeof(UIElement), typeof(PopupWindow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SetPlacementTarget)));
            OriginCollectionProperty = DependencyProperty.Register("OriginCollection", typeof(ObservableCollection<HexadecimalKeyModel>), typeof(PopupWindow), new FrameworkPropertyMetadata(null));
            SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(PopupWindow), new FrameworkPropertyMetadata(null));
            ShowModeProperty = DependencyProperty.Register("ShowMode", typeof(Mode), typeof(PopupWindow), new FrameworkPropertyMetadata(Mode.Null, new PropertyChangedCallback(SetShowMode)));
            MessageProperty = DependencyProperty.Register("Message", typeof(Action<object, EventArgs>), typeof(PopupWindow), new FrameworkPropertyMetadata(null));
        }

        private static void SetShowMode(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PopupWindow)d).SetMode();
        }

        private void SetMode()
        {
            FoundDataCollection.Clear();
            tb.Text = "";
            popup.IsOpen = false;
        }

        private static void SetPlacementTarget(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PopupWindow)d).SetTarget();
        }

        private void SetTarget()
        {
            try
            {
                popup.PlacementTarget = ElementParent;
                Window window = Window.GetWindow(ElementParent);
                if (window != null)
                {
                    window.LocationChanged += delegate (object sender, EventArgs e)
                    {
                        var offset = popup.HorizontalOffset;
                        popup.HorizontalOffset = offset + 1;
                        popup.HorizontalOffset = offset;
                    };
                }
            }
            catch (Exception ex)
            {
                SendMessage(ex);
            }
        }

        private void OpenPopup_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = !popup.IsOpen;
            tb.Focus();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchStr = tb.Text;
            FoundDataCollection.Clear();
            try
            {
                foreach (var item in OriginCollection)
                {
                    if (item.KeyValue.Contains(searchStr))
                    {
                        FoundDataCollection.Add(new HexadecimalKeyModel()
                        {
                            Number = item.Number,
                            KeyValue = item.KeyValue,
                            HashValue = item.HashValue
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessage(ex);
            }

            if (ShowMode == Mode.Key)
            {
                keysGrid.ItemsSource = FoundDataCollection;
            }
            else if (ShowMode == Mode.Hash)
            {
                hashGrid.ItemsSource = FoundDataCollection;
            }
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search_Click(null, null);
            }
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            Window window = Window.GetWindow(ElementParent);
            window.Deactivated += ((sender, e) => { popup.IsOpen = false; });
        }

        private void SendMessage(Exception ex)
        {
            PackageEventArgs args = new PackageEventArgs() { Message = ex };
            Message?.Invoke(this, args);
        }
    }
}
