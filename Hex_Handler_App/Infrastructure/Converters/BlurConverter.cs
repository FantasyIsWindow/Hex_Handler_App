using System;
using System.Globalization;
using System.Windows.Data;

namespace Hex_Handler_App.Infrastructure.Converters
{
    internal class BlurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
