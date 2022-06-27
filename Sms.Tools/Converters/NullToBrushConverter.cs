using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Sms.Tools.Converters
{
    public class NullToBrushConverter : IValueConverter
    {
        public Brush NullBrush { get; set; }
        public Brush NotNullBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is null) ? NullBrush : NotNullBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
