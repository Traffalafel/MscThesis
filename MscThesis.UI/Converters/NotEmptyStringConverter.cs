using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MscThesis.UI.Converters
{
    internal class NotEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ((string)value);
            return str.Length > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
