using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MscThesis.UI.Converters
{
    internal class NotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = ((IEnumerable)value).Cast<object>();
            return list.Any();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
