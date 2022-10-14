using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscThesis.UI.Converters
{
    internal class MultipleProblemSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(val => val == null)) {
                return false;
            }

            var customProblemSizesAllowed = (bool)values[0];
            var multipleSizes = (bool)values[1];

            return customProblemSizesAllowed && multipleSizes;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
