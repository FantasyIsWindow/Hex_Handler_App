using Hex_Handler_App.Infrastructure.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hex_Handler_App.Infrastructure.Converters
{
    internal class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Mode mode)
            {
                string control = parameter as string;
                if (mode == Mode.Null && control == "Message")
                {
                    return Visibility.Visible;
                }
                else if (mode == Mode.Key && control == "Keys")
                {
                    return Visibility.Visible;
                }
                else if (mode == Mode.Hash && control == "Hash")
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
