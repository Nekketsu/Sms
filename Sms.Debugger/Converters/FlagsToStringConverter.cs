using Sms.Cpu;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sms.Debugger.Converters
{
    public class FlagsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Registers.Flags flags)
            {
                return $"S:{(flags.HasFlag(Registers.Flags.S) ? 1 : 0)}"
                    + $",Z:{(flags.HasFlag(Registers.Flags.Z) ? 1 : 0)}"
                    + $",-"
                    + $",H:{(flags.HasFlag(Registers.Flags.H) ? 1 : 0)}"
                    + $",-"
                    + $",P/V:{(flags.HasFlag(Registers.Flags.PV) ? 1 : 0)}"
                    + $",N:{(flags.HasFlag(Registers.Flags.N) ? 1 : 0)}"
                    + $",C:{(flags.HasFlag(Registers.Flags.C) ? 1 : 0)}";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
