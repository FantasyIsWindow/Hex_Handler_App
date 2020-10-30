using Hex_Handler_App.Infrastructure.Enums;
using Hex_Handler_App.Infrastructure.EventsArgs;
using Hex_Handler_App.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Hex_Handler_App.Views.Controls
{
    public partial class SynchronizedDataGrid : UserControl
    {
        internal static readonly DependencyProperty DisplayDataProperty;

        internal static readonly DependencyProperty SelectItemProperty;

        internal static readonly DependencyProperty ShowModeProperty;

        internal static readonly DependencyProperty MessageProperty;

        public ObservableCollection<HexadecimalKeyModel> DisplayData
        {
            get => (ObservableCollection<HexadecimalKeyModel>)GetValue(DisplayDataProperty);
            set => SetValue(DisplayDataProperty, value);
        }

        public HexadecimalKeyModel SelectItem
        {
            get => (HexadecimalKeyModel)GetValue(SelectItemProperty);
            set => SetValue(SelectItemProperty, value);
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

        public SynchronizedDataGrid()
        {
            InitializeComponent();
        }

        static SynchronizedDataGrid()
        {
            DisplayDataProperty = DependencyProperty.Register("DisplayData", typeof(ObservableCollection<HexadecimalKeyModel>), typeof(SynchronizedDataGrid), new FrameworkPropertyMetadata(null));
            SelectItemProperty = DependencyProperty.Register("SelectItem", typeof(HexadecimalKeyModel), typeof(SynchronizedDataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SetSelectedItem)));
            ShowModeProperty = DependencyProperty.Register("ShowMode", typeof(Mode), typeof(SynchronizedDataGrid), new FrameworkPropertyMetadata(Mode.Null));
            MessageProperty = DependencyProperty.Register("Message", typeof(Action<object, EventArgs>), typeof(SynchronizedDataGrid), new FrameworkPropertyMetadata(null));
        }

        private static void SetSelectedItem(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SynchronizedDataGrid)d).SetItem();
        }

        private void SetItem()
        {
            if (SelectItem != null)
            {
                if (ShowMode == Mode.Key)
                {
                    keysGrid.SelectedIndex = SelectItem.Number - 1;
                    keysGrid.ScrollIntoView(keysGrid.SelectedItem);
                }
                else if (ShowMode == Mode.Hash)
                {
                    hashGrid.SelectedIndex = SelectItem.Number - 1;
                    hashGrid.ScrollIntoView(hashGrid.SelectedItem);
                }
            }
        }

        private void KeysGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement is TextBox tb)
            {
                tb.Text = tb.Text.ToUpper();
            }
        }

        private void Grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if ((string)e.Column.Header == "Key" && e.EditingElement is TextBox tb)
                {
                    tb.Text = tb.Text.ToUpper();
                    string amounth = "";
                    for (int i = 0; i < tb.Text.Length; i += 2)
                    {
                        string nextTerm = string.Format("{0}{1}", tb.Text[i], tb.Text[i + 1]);
                        amounth = SummationOfBytes(amounth, nextTerm);
                    }
                    string invertValue = IncvertValue(amounth);
                    var row = e.Row.DataContext as HexadecimalKeyModel;
                    row.HashValue = invertValue.Substring(2, 2);
                }
            }
            catch (Exception ex)
            {
                SendMessage(ex);
            }
        }

        private string SummationOfBytes(string amounth, string nextTerm)
        {
            if (string.IsNullOrEmpty(amounth))
            {
                return nextTerm;
            }

            int firstTerm = Int32.Parse(amounth, NumberStyles.AllowHexSpecifier);
            int secondTerm = Int32.Parse(nextTerm, NumberStyles.AllowHexSpecifier);
            return string.Format("{0:X}", firstTerm + secondTerm);
        }

        private string IncvertValue(string value)
        {
            value = ByteAlignment(value);
            int newValue = Int32.Parse(value, NumberStyles.AllowHexSpecifier);
            string tempValue = string.Format("{0:X}", (~newValue) + 1);
            return tempValue.Substring(4, 4);
        }

        private string ByteAlignment(string value) =>
            value.Length == 4 ? value : ByteAlignment("0" + value);

        private void SendMessage(Exception ex)
        {
            PackageEventArgs args = new PackageEventArgs() { Message = ex };
            Message?.Invoke(this, args);
        }
    }
}
