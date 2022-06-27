using System.Globalization;
using System.Windows.Data;

namespace Sms.Tools.Converters
{
    public class ZeroToBooleanConverter : IValueConverter
    {
        public bool ZeroValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is 0 ? ZeroValue : !ZeroValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
