using Sms.Cpu;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sms.Debugger.Converters
{
    public class FlagsToByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Registers.Flags flags)
            {
                return (byte)flags;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
