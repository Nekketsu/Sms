using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sms.Debugger.Converters
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int number)
            {
                return number.ToString("x2");
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                if (int.TryParse(text, NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier, NumberFormatInfo.CurrentInfo, out var number))
                {
                    return number;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
