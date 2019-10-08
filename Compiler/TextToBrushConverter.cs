using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Compiler
{
    [ValueConversion(typeof(string), typeof(Brush))]
    class TextToBrushConverter : IValueConverter
    {
        public static TextToBrushConverter Instance = new TextToBrushConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
